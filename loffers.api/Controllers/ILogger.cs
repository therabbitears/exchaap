using System;

namespace Loffers.Server.Controllers
{
    public interface ILogger<T>
    {
        void LogError(Exception ex, string message);
    }
}