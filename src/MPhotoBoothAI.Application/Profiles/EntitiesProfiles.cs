using AutoMapper;
using MPhotoBoothAI.Models;
using MPhotoBoothAI.Models.Entities;

namespace MPhotoBoothAI.Application.Profiles;
public class EntitiesProfiles : Profile
{
    public EntitiesProfiles()
    {
        CreateMap<UserSettingsEntity, UserSettings>();
    }
}
