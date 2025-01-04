namespace MPhotoBoothAI.Application;

public static class Consts
{
    public static class AiModels
    {
        public const string Yolov8nFace = "yolov8n-face";
        public const string ArcfaceBackbone = "arcface_backbone";
        public const string Gunet2blocks = "G_unet_2blocks";
        public const string FaceLandmarks = "face_landmarks";
        public const string Gfpgan = "gfpgan_1.4";
        public const string VggGender = "vgg_ilsvrc_16_gender_imdb_wiki";
    }

    public static class Sizes
    {
        public const int PhotoWidth = 3000;
        public const int PhotoHeight = 2000;
        public const int BasicPrintWidth = 3000;
    }

    public static class Background
    {
        public const string PostcardBackground = "\\Background\\Postcard";
        public const string StripeBackground = "\\Background\\Stripe";
        public const double PostcardBackgroundRatio = 1.5;
        public const double StripeBackgroundRatio = 3.0;
    }
}
