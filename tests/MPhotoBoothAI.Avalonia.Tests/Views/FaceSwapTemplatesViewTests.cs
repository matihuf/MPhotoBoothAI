using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using MPhotoBoothAI.Application.ViewModels;
using MPhotoBoothAI.Avalonia.Tests.Extensions;
using MPhotoBoothAI.Avalonia.Views;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Avalonia.Tests.Views;

public class FaceSwapTemplatesViewTests(DependencyInjectionFixture dependencyInjectionFixture) : BaseMainWindowTests(dependencyInjectionFixture)
{
    private static readonly string _groupName = "groupName";

    [AvaloniaFact]
    public void AddNewGroup_MessageBoxConfirmed_Add()
    {
        //arrange
        var window = _builder.Build();
        window.OpenView(typeof(FaceSwapTemplatesViewModel));
        AddNewGroup(window, _groupName);
        //assert
        var faceSwapTemplateGroup = (FaceSwapTemplateGroupEntity)GetListBoxGroups(window).Items[0];
        Assert.Equal(_groupName, faceSwapTemplateGroup.Name);
    }

    private static void AddNewGroup(MainWindow window, string groupName)
    {
        var addGroupButton = GetAddGroupButton(window);
        addGroupButton.Command.Execute(window);
        var messageBoxWindow = window.OwnedWindows[0];
        var messageBoxInput = messageBoxWindow.FindControls<TextBox>().ElementAt(2);
        var messageBoxButtonYes = GetMessageBoxButtonYes(messageBoxWindow);
        messageBoxInput.Text = groupName;
        messageBoxButtonYes.Command.Execute(messageBoxButtonYes.Content);
    }

    [AvaloniaFact]
    public void AddNewGroup_MessageBoxNotConfirmed_DoNotAdd()
    {
        //arrange
        var window = _builder.Build();
        window.OpenView(typeof(FaceSwapTemplatesViewModel));
        var addGroupButton = GetAddGroupButton(window);
        //act
        addGroupButton.Command.Execute(window);
        var messageBoxWindow = window.OwnedWindows[0];
        var messageBoxButtonNo = GetMessageBoxButtonNo(messageBoxWindow);
        messageBoxButtonNo.Command.Execute(messageBoxButtonNo.Content);
        //assert
        Assert.Empty(GetListBoxGroups(window).Items);
    }

    [AvaloniaFact]
    public void DeleteGroup_MessageBoxConfirmed_Delete()
    {
        //arrange
        var window = _builder.Build();
        window.OpenView(typeof(FaceSwapTemplatesViewModel));
        var listBoxGroups = GetListBoxGroups(window);
        AddNewGroup(window, _groupName);
        Assert.NotEmpty(listBoxGroups.Items);
        //act
        GetDeleteGroupButton(window).Command.Execute(window);
        var messageBoxWindow = window.OwnedWindows[0];
        var messageBoxButtonYes = GetMessageBoxButtonYes(messageBoxWindow);
        messageBoxButtonYes.Command.Execute(messageBoxButtonYes.Content);
        //assert
        Assert.Empty(listBoxGroups.Items);
    }


    [AvaloniaFact]
    public void DeleteGroup_MessageBoxNotConfirmed_DoNotDelete()
    {
        //arrange
        var window = _builder.Build();
        window.OpenView(typeof(FaceSwapTemplatesViewModel));
        var listBoxGroups = GetListBoxGroups(window);
        AddNewGroup(window, _groupName);
        Assert.NotEmpty(listBoxGroups.Items);
        //act
        GetDeleteGroupButton(window).Command.Execute(window);
        var messageBoxWindow = window.OwnedWindows[0];
        var messageBoxButtonNo = GetMessageBoxButtonNo(messageBoxWindow);
        messageBoxButtonNo.Command.Execute(messageBoxButtonNo.Content);
        //assert
        Assert.NotEmpty(listBoxGroups.Items);
    }

    [AvaloniaFact]
    public void EditGroup_ShowEditControls()
    {
        //arrange
        var window = _builder.Build();
        window.OpenView(typeof(FaceSwapTemplatesViewModel));
        AddNewGroup(window, _groupName);
        var editGroupButton = GetEditGroupButton(window);
        //act
        editGroupButton.Command.Execute(window);
        //assert
        Assert.True(GetGroupNameTextBox(window).IsVisible);
        Assert.False(GetGroupNameTextBlock(window).IsVisible);
        Assert.False(GetEditGroupButton(window).IsVisible);
        Assert.True(GetSaveEditGroupButton(window).IsVisible);
        Assert.True(GetCancelEditGroupButton(window).IsVisible);
    }

    [AvaloniaFact]
    public void EditGroup_Cancel_HideEditControls()
    {
        //arrange
        var window = _builder.Build();
        window.OpenView(typeof(FaceSwapTemplatesViewModel));
        AddNewGroup(window, _groupName);
        var editGroupButton = GetEditGroupButton(window);
        var cancelEditGroupButton = GetCancelEditGroupButton(window);
        //act
        editGroupButton.Command.Execute(window);
        cancelEditGroupButton.Command.Execute(null);
        //assert
        Assert.False(GetGroupNameTextBox(window).IsVisible);
        Assert.True(GetGroupNameTextBlock(window).IsVisible);
        Assert.True(GetEditGroupButton(window).IsVisible);
        Assert.False(GetSaveEditGroupButton(window).IsVisible);
        Assert.False(GetCancelEditGroupButton(window).IsVisible);
    }

    [AvaloniaFact]
    public void EditGroup_Save_HideEditControls()
    {
        //arrange
        var window = _builder.Build();
        window.OpenView(typeof(FaceSwapTemplatesViewModel));
        AddNewGroup(window, _groupName);
        var editGroupButton = GetEditGroupButton(window);
        var saveEditGroupButton = GetSaveEditGroupButton(window);
        //act
        editGroupButton.Command.Execute(window);
        saveEditGroupButton.Command.Execute(null);
        //assert
        Assert.False(GetGroupNameTextBox(window).IsVisible);
        Assert.True(GetGroupNameTextBlock(window).IsVisible);
        Assert.True(GetEditGroupButton(window).IsVisible);
        Assert.False(GetSaveEditGroupButton(window).IsVisible);
        Assert.False(GetCancelEditGroupButton(window).IsVisible);
    }

    private static TextBlock GetGroupNameTextBlock(MainWindow window) => window.FindViewControl<TextBlock>("groupNameTextBlock");
    private static Button GetCancelEditGroupButton(MainWindow window) => window.FindViewControl<Button>("cancelEditGroupBtn");
    private static TextBox GetGroupNameTextBox(MainWindow window) => window.FindViewControl<TextBox>("groupNameTextBox");
    private static Button GetSaveEditGroupButton(MainWindow window) => window.FindViewControl<Button>("saveEditGroupBtn");
    private static Button GetEditGroupButton(MainWindow window) => window.FindViewControl<Button>("editGroupBtn");
    private static Button GetAddGroupButton(MainWindow window) => window.FindViewControl<Button>("addGroupBtn");
    private static Button GetDeleteGroupButton(MainWindow window) => window.FindViewControl<Button>("deleteGroupBtn");
    private static ListBox GetListBoxGroups(MainWindow window) => window.FindViewControl<ListBox>("ListBoxGroups");
}
