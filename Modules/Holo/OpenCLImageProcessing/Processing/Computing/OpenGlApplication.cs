using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Processing.Computing
{
    public static class OpenGlApplication
    {
        private static bool _initialized = false;
        public static void Setup()
        {
            if (_initialized)
                return;

            // TODO: переместить в надлежащее место
            Toolkit.Init();

            _initialized = true;
        }

        public static void EnableDebugMessages()
        {
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback((source, type, id, severity, length, message, param) =>
            {
                var msg = Marshal.PtrToStringAnsi(message, length);
                Debug.Print($"{source} {type} {id} {severity} {length} {msg}\n");
            }, IntPtr.Zero);
        }
    }
}
