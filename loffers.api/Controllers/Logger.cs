using Loffers.Server.Controllers;
using System;

namespace loffers.api.Controllers
{
    public class Logger<T> : ILogger<T>
    {
        public void LogError(Exception ex, string message)
        {
            // Return
        }
    }
}