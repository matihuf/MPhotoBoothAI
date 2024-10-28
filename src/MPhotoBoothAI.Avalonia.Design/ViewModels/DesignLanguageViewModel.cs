﻿using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;
public class DesignLanguageViewModel : LanguageViewModel
{
    public DesignLanguageViewModel() : base(new Mock<IDatabaseContext>().Object, new Mock<IAppRestarterService>().Object)
    {
        IsRestartVisible = true;
    }
}
