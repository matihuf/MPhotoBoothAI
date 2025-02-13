﻿using CommunityToolkit.Mvvm.ComponentModel;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;
using MPhotoBoothAI.Models.UI;
using System.Collections.ObjectModel;

namespace MPhotoBoothAI.Application.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    [ObservableProperty]
    private ViewModelBase _content = default!;

    [ObservableProperty]
    private ListItemTemplate? _selectedPage;

    private readonly INavigationService<ViewModelBase> _navigationService;

    public ObservableCollection<ListItemTemplate> Pages { get; }

    private readonly List<ListItemTemplate> _pages =
    [
        new ListItemTemplate(typeof(HomeViewModel), "Home", "Home"),
        new ListItemTemplate(typeof(CameraSettingsViewModel), "Camera", "cameraSettings"),
        new ListItemTemplate(typeof(FaceSwapGroupTemplatesViewModel), "ImageEditOutline", Assets.UI.templates),
        new ListItemTemplate(typeof(FaceDetectionViewModel), "FaceRecognition", "FaceDetection"),
        new ListItemTemplate(typeof(DesignPrintTemplateViewModel), "BorderOutside", Assets.UI.printLayout),
    ];

    public MainViewModel(INavigationService<ViewModelBase> navigationService)
    {
        Pages = new ObservableCollection<ListItemTemplate>(_pages);
        _navigationService = navigationService;
        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        SelectedPage = Pages[0];
    }

    private void OnCurrentViewModelChanged(ViewModelBase viewModel)
    {
        Content = viewModel;
    }

    partial void OnSelectedPageChanged(ListItemTemplate? value)
    {
        if (value is null)
        {
            return;
        }
        _navigationService.GoTo(value.ModelType);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _navigationService.CurrentViewModelChanged -= OnCurrentViewModelChanged;
        }
    }
}
