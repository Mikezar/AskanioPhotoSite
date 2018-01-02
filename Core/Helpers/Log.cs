using System;
using NLog;

namespace AskanioPhotoSite.Core.Helpers
{
    public class Log
    {
        private static Logger _logger = LogManager.GetLogger("common");

        public static void RegisterError(Exception exception)
        {
            _logger.Error($"Date: {DateTime.Now}");
            _logger.Error($"Source: {exception.Source}");
            _logger.Error($"Error message: {exception.Message}");
            _logger.Error($"Stack trace: {exception.StackTrace}");
            _logger.Error($"Target site: {exception.TargetSite}");
            if (exception.InnerException != null)
            {
                _logger.Error($"Inner exception: ");
                RegisterError(exception.InnerException);
            }
        }

        public static void Trace(string message)
        {
            _logger.Trace(message);
        }
    }
}
