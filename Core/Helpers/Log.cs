using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace AskanioPhotoSite.Core.Helpers
{
    public class Log
    {
        private static Logger _logger = LogManager.GetLogger("common");

        public static void RegisterError(Exception exception)
        {
            _logger.Error($"Date: {DateTime.Now}");
            _logger.Error($"Inner message: {exception.InnerException}");
            _logger.Error($"Source: {exception.Source}");
            _logger.Error($"Error message: {exception.Message}");
            _logger.Error($"Stack trace: {exception.StackTrace}");
        }

        public static void Trace(string message)
        {
            _logger.Trace(message);
        }
    }
}
