using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Services;

namespace WORLDCUP_ClassLibrary.Factories
{
    public static class ServiceFactory
    {
        private static HttpService? _httpService;
        private static FileService? _fileService;

        public static HttpService GetHttpService()
        {
            if (_httpService == null)
            {
                _httpService = new HttpService();
            }
            return _httpService;
        }

        public static FileService GetFileService()
        {
            if (_fileService == null)
            {
                _fileService = new FileService();
            }
            return _fileService;
        }
    }
}
