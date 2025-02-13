﻿using Moq;
using MPhotoBooth.Unit.Tests.Application.ViewModels.Builders;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Common.Tests;
using MPhotoBoothAI.Models.Entities;
using MPhotoBoothAI.Models.FaceSwaps;

namespace MPhotoBooth.Unit.Tests.Application.ViewModels;
public class FaceSwapTemplatesViewModelTests
{
    private readonly FaceSwapGroupTemplatesViewModelBuilder _builder;

    public FaceSwapTemplatesViewModelTests()
    {
        _builder = new FaceSwapGroupTemplatesViewModelBuilder();
    }

    [Fact]
    public async Task AddGroup_UserInputGroupName_GroupShouldBeAddedToDatabase()
    {
        //arrange
        string groupName = "groupName";
        using var databaseContext = new DatabaseContextBuilder().Build();
        var viewModel = _builder.WithGroupName(groupName).Build(databaseContext);
        //act
        await viewModel.AddGroupCommand.ExecuteAsync(null);
        //assert
        var newGroup = databaseContext.FaceSwapTemplateGroups.FirstOrDefault(x => x.Name == groupName);
        Assert.NotNull(newGroup);
    }

    [Fact]
    public async Task AddGroup_UserDidNotInputGroupName_GroupShouldNotBeAddedToDatabase()
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var viewModel = _builder.Build(databaseContext);
        //act
        await viewModel.AddGroupCommand.ExecuteAsync(null);
        //assert
        Assert.False(databaseContext.FaceSwapTemplateGroups.Any());
    }

    [Fact]
    public async Task DeleteGroup_UserConfirmed_GroupShouldBeRemoved()
    {
        //arrange
        var group = new FaceSwapTemplateGroupEntity { Name = "groupName" };
        using var databaseContext = new DatabaseContextBuilder().Build();
        databaseContext.FaceSwapTemplateGroups.Add(group);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.WithDeleteGroupConfirmation(true).Build(databaseContext);
        viewModel.SelectedGroup = group;
        //act
        await viewModel.DeleteGroupCommand.ExecuteAsync(null);
        //assert
        var deletedGroup = databaseContext.FaceSwapTemplateGroups.FirstOrDefault(x => x.Id == group.Id);
        Assert.Null(deletedGroup);
        Assert.Null(viewModel.SelectedGroup);
        _builder.FaceSwapTemplateFileService.Verify(x => x.DeleteGroup(group.Id));
    }

    [Fact]
    public async Task DeleteGroup_UserNotConfirmed_GroupShouldNotBeRemovedFromDatabase()
    {
        //arrange
        var group = new FaceSwapTemplateGroupEntity { Name = "groupName" };
        using var databaseContext = new DatabaseContextBuilder().Build();
        databaseContext.FaceSwapTemplateGroups.Add(group);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.WithDeleteGroupConfirmation(false).Build(databaseContext);
        viewModel.SelectedGroup = group;
        //act
        await viewModel.DeleteGroupCommand.ExecuteAsync(null);
        //assert
        var deletedGroup = databaseContext.FaceSwapTemplateGroups.FirstOrDefault(x => x.Id == group.Id);
        Assert.NotNull(deletedGroup);
        Assert.NotNull(viewModel.SelectedGroup);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void EditGroup_IsGroupInEdit_ShouldBeReverted(bool isGroupInEdit, bool expected)
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var viewModel = _builder.Build(databaseContext);
        viewModel.IsGroupInEdit = isGroupInEdit;
        //act
        viewModel.EditGroupCommand.Execute(null);
        //assert
        Assert.Equal(expected, viewModel.IsGroupInEdit);
    }

    [Fact]
    public async Task CancelEditGroup_ShouldRevertGroupEdits()
    {
        //arrange
        string originalGroupName = "groupName";
        var group = new FaceSwapTemplateGroupEntity { Name = originalGroupName };

        using var databaseContext = new DatabaseContextBuilder().Build();
        databaseContext.FaceSwapTemplateGroups.Add(group);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.Build(databaseContext);

        viewModel.EditGroupCommand.Execute(null);
        int editedIndex = -1;
        if (viewModel.SelectedGroup != null)
        {
            editedIndex = viewModel.Groups.IndexOf(viewModel.SelectedGroup);
            viewModel.Groups[editedIndex].Name = "changed";
        }
        //act
        viewModel.CancelEditGroupCommand.Execute(null);
        //assert
        Assert.Equal(originalGroupName, viewModel.Groups[editedIndex].Name);
        Assert.False(viewModel.IsGroupInEdit);
    }

    [Fact]
    public async Task SaveSaveEditGroup_ShouldSaveGroupEdits()
    {
        //arrange
        string originalGroupName = "groupName", changed = "changed";
        var group = new FaceSwapTemplateGroupEntity { Name = originalGroupName };

        using var databaseContext = new DatabaseContextBuilder().Build();
        databaseContext.FaceSwapTemplateGroups.Add(group);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.Build(databaseContext);

        viewModel.EditGroupCommand.Execute(null);
        int editedIndex = -1;
        if (viewModel.SelectedGroup != null)
        {
            editedIndex = viewModel.Groups.IndexOf(viewModel.SelectedGroup);
            viewModel.Groups[editedIndex].Name = changed;
        }
        //act
        await viewModel.SaveEditGroupCommand.ExecuteAsync(null);
        //assert
        Assert.Equal(changed, viewModel.Groups[editedIndex].Name);
        Assert.Equal(changed, databaseContext.FaceSwapTemplateGroups.First(x => x.Id == group.Id).Name);
        Assert.False(viewModel.IsGroupInEdit);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 1)]
    public async Task OnSelectedGroupChanged_LoadGroupTemplates(int index, int expectedTemplates)
    {
        //arrange
        using var databaseContext = new DatabaseContextBuilder().Build();
        var group = new FaceSwapTemplateGroupEntity
        {
            Name = "0",
            Templates = [new FaceSwapTemplateEntity(), new FaceSwapTemplateEntity()]
        };
        var secondGroup = new FaceSwapTemplateGroupEntity
        {
            Name = "3",
            Templates = [new FaceSwapTemplateEntity()]
        };
        databaseContext.FaceSwapTemplateGroups.Add(group);
        databaseContext.FaceSwapTemplateGroups.Add(secondGroup);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.WithFullTemplateThumbnailPath("TestData/square.png").Build(databaseContext);
        //act
        viewModel.SelectedGroup = viewModel.Groups[index];
        //assert
        Assert.Equal(expectedTemplates, viewModel.Templates.Count);
    }

    [Fact]
    public async Task SaveChanges_ShouldSaveGroupChanges()
    {
        //arrange
        string originalGroupName = "groupName", changed = "changed";
        var group = new FaceSwapTemplateGroupEntity { Name = originalGroupName };

        using var databaseContext = new DatabaseContextBuilder().Build();
        databaseContext.FaceSwapTemplateGroups.Add(group);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.Build(databaseContext);

        viewModel.EditGroupCommand.Execute(null);
        int editedIndex = -1;
        if (viewModel.SelectedGroup != null)
        {
            editedIndex = viewModel.Groups.IndexOf(viewModel.SelectedGroup);
            viewModel.Groups[editedIndex].Name = changed;
        }
        //act
        await viewModel.SaveChangesCommand.ExecuteAsync(null);
        //assert
        Assert.Equal(changed, viewModel.Groups[editedIndex].Name);
        Assert.Equal(changed, databaseContext.FaceSwapTemplateGroups.First(x => x.Id == group.Id).Name);
    }

    [Fact]
    public async Task DeleteTemplate_UserConfirmed_TemplateShouldBeRemoved()
    {
        //arrange
        var group = new FaceSwapTemplateGroupEntity { Name = "groupName" };
        using var databaseContext = new DatabaseContextBuilder().Build();
        databaseContext.FaceSwapTemplateGroups.Add(group);
        await databaseContext.SaveChangesAsync();
        var template = new FaceSwapTemplateEntity { FaceSwapTemplateGroupId = group.Id };
        databaseContext.FaceSwapTemplates.Add(template);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.WithDeleteTemplateConfirmation(true).Build(databaseContext);
        viewModel.SelectedGroup = group;
        //act
        await viewModel.DeleteTemplateCommand.ExecuteAsync(ValueTuple.Create(It.IsAny<IWindow>(), new FaceSwapTemplateId(template.Id, "", 0)));
        //assert
        var deletedTemplate = databaseContext.FaceSwapTemplates.FirstOrDefault(x => x.Id == template.Id);
        Assert.Null(deletedTemplate);
        _builder.FaceSwapTemplateFileService.Verify(x => x.DeleteTemplate(group.Id, template.Id));
    }

    [Fact]
    public async Task DeleteTemplate_UserNotConfirmed_TemplateShouldNOtBeRemoved()
    {
        //arrange
        var group = new FaceSwapTemplateGroupEntity { Name = "groupName" };
        using var databaseContext = new DatabaseContextBuilder().Build();
        databaseContext.FaceSwapTemplateGroups.Add(group);
        await databaseContext.SaveChangesAsync();
        var template = new FaceSwapTemplateEntity { FaceSwapTemplateGroupId = group.Id };
        databaseContext.FaceSwapTemplates.Add(template);
        await databaseContext.SaveChangesAsync();
        var viewModel = _builder.WithDeleteTemplateConfirmation(false).Build(databaseContext);
        viewModel.SelectedGroup = group;
        //act
        await viewModel.DeleteTemplateCommand.ExecuteAsync(ValueTuple.Create(It.IsAny<IWindow>(), new FaceSwapTemplateId(template.Id, "", 0)));
        //assert
        var deletedTemplate = databaseContext.FaceSwapTemplates.FirstOrDefault(x => x.Id == template.Id);
        Assert.NotNull(deletedTemplate);
        _builder.FaceSwapTemplateFileService.Verify(x => x.DeleteTemplate(group.Id, template.Id), Times.Never);
    }
}
