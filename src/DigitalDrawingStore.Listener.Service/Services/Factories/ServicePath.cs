using System;
using System.IO;

namespace DigitalDrawingStore.Listener.Service.Services.Factories
{
    internal static class ServicePath
    {
        internal static string GetFullPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }
            if (path.StartsWith("."))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return path;
        }
    }
}
