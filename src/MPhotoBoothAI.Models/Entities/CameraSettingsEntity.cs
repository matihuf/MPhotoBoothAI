﻿using MPhotoBoothAI.Models.Entities.Base;

namespace MPhotoBoothAI.Models.Entities
{
    public class CameraSettingsEntity : Entity
    {
        public string Camera { get; set; } = string.Empty;

        public string Iso { get; set; } = string.Empty;

        public string Aperture { get; set; } = string.Empty;

        public string ShutterSpeed { get; set; } = string.Empty;

        public string WhiteBalance { get; set; } = string.Empty;
    }
}
