using System;
using Common;
using OpenTK.Graphics.OpenGL;

namespace Processing
{
    public static class OpenGlErrorThrower
    {
        public static void ThrowIfAny()
        {
            var err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                var errMessage = $"GL ERROR: {err}";
                DebugLogger.Log(errMessage);
                throw new Exception(errMessage);
            }
        }
    }
}
