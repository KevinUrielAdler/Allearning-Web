using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Aula_W.Repositories
{
    public class Uploader
    {
        public string UploadImage(IFormFile vm,IWebHostEnvironment webMostEnviroment)
        {
            string FileName = null;
            if (vm != null)
            {
                string uploadDir = Path.Combine(webMostEnviroment.WebRootPath, "img/Users");
                FileName = Guid.NewGuid().ToString() + "-" + vm.FileName;
                string FilePath = Path.Combine(uploadDir, FileName);
                using (var FileStream=new FileStream(FilePath, FileMode.Create))
                {
                    vm.CopyTo(FileStream);
                }
            }
            return FileName;
        }
    }
}
