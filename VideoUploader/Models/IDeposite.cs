using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VideoUploader.Models
{

    public interface IDeposite
    {
        string DepositeFile(HttpPostedFileBase file, string videoName, string inputPath);

        string GetThumbnail(string pathToVideoFile, string pathToImageFile, float frameThumb);
    }

    public static class DepositeFactory
    {
        public static IDeposite CreateDeposite()
        {
            if (ConfigurationManager.AppSettings["DepositeType"].ToString() == "Local")
                return new LocalDeposite();
            else
                return new AmazoneDeposite();
        }
    }

    public class LocalDeposite : IDeposite
    {
        string IDeposite.DepositeFile(HttpPostedFileBase file, string videoName, string inputPath)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);
            string path1 = string.Format("{0}/{1}{2}", inputPath, videoName, extension);
            if (System.IO.File.Exists(path1))
                System.IO.File.Delete(path1);

            file.SaveAs(path1);

            return path1;
        }

        string IDeposite.GetThumbnail(string pathToVideoFile, string pathToImageFile, float frameThumb)
        {
            string filename;
            FileInfo fi = new FileInfo(pathToVideoFile);
            filename = pathToImageFile + "\\" + Path.GetFileNameWithoutExtension(fi.Name) + ".jpeg";
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.GetVideoThumbnail(pathToVideoFile, filename, frameThumb);
            return filename;
        }
    }

    public class AmazoneDeposite : IDeposite
    {
        string IDeposite.DepositeFile(HttpPostedFileBase file, string videoName, string inputPath)
        {
            return "";
        }

        string IDeposite.GetThumbnail(string pathToVideoFile, string pathToImageFile, float frameThumb)
        {
            throw new NotImplementedException();
        }
    }

}
