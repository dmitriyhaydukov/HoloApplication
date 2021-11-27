namespace HoloCommon.Synchronization
{ 
    public static class Events
    {
        public static class Camera
        {
            public const string TAKE_PICTURE = "EVENT_CAMERA_TAKE_PICTURE";
            public const string PICTURE_TAKEN = "EVENT_CAMERA_PICTURE_TAKEN";
        }

        public static class Image
        {
            public const string IMAGE_CREATED = "IMAGE_CREATED";
            public const string IMAGE_RENDERED = "IMAGE_RENDERED";
            public const string IMAGE_UPDATED = "IMAGE_UPDATED";
        }
    }
}
