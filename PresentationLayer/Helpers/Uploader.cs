using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Helpers
{
    public static class Uploader   {

        public static string UploadImage(IFormFile Image,IHostingEnvironment hosting, string OldImage="")
        {
            string ImageName = "";
            if (Image != null)
            {
                try
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "Images");
                    ImageName = Image.FileName;
                    string ImagePath = Path.Combine(uploads, ImageName);
                    string OldImagePath = Path.Combine(uploads, OldImage!=null? OldImage:"");
                    if (System.IO.File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                        Image.CopyTo(new FileStream(ImagePath, FileMode.Create));
                    }
                    else
                    {
                        Image.CopyTo(new FileStream(ImagePath, FileMode.Create));
                    }
                }
                catch (Exception ex)
                {
                    //ImageName = "";
                }   

            }
            return ImageName;

        }




    }
}
