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
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void RegisterError(Exception exception)
        {
            _logger.Error($"Date: {DateTime.Now}\r\n");
            _logger.Error($"Inner message: {exception.InnerException}\r\n");
            _logger.Error($"Source: {exception.Source}\r\n");
            _logger.Error($"Error message: {exception.Message}\r\n");
            _logger.Error($"Stack trace: {exception.StackTrace}\r\n");
        }
    }
}
