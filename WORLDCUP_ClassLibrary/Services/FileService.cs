using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Services
{
    public class FileService
    {
        public async Task<string?> ReadFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            return null;
        }
    }
}
