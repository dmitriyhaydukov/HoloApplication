using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using Processing;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    public class ShaderProvider
    {
        private List<IImageShader> _cachedShaders = new List<IImageShader>();

        public T Get<T>() where T : IImageShader
        {
            return (T) Get(typeof(T));
        }

        public IImageShader Get(Type t)
        {
            if (!t.IsClass || t.IsAbstract)
                throw new InvalidOperationException();

            if (_cachedShaders.Any(t.IsInstanceOfType))
                return _cachedShaders.Single(t.IsInstanceOfType);

            var shader = (IImageShader)Activator.CreateInstance(t);
            _cachedShaders.Add(shader);

            return shader;
        }

        public IReadOnlyCollection<IImageShader> GetFor(IImageHandler imageHandler)
        {
            var shaderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IImageShader).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            var specificShaderTypes = shaderTypes.Where(t => t.GetCustomAttribute<TargetImageAttribute>()?.ImageFormat == imageHandler.Format).ToArray();
            if (specificShaderTypes.Any())
                return specificShaderTypes.Select(Get).ToArray();

            var commonShaderTypes = shaderTypes.Where(t => t.GetCustomAttribute<TargetImageAttribute>() == null).ToArray();
            if (commonShaderTypes.Any())
                return commonShaderTypes.Select(Get).ToArray();

            return Enumerable.Empty<IImageShader>().ToArray();
        }
    }
}
