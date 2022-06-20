using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string FileToScallPath;
            string NoScallingPath;
            string FileName;
            string StorageDirectory;
            string[] dirs;
            string LogFilePath= "";
            List<ScallingConfig> ScallingPaths = new List<ScallingConfig>();

            using (StreamReader file = new StreamReader("../../config.txt"))
            {
                string ln;
                FileToScallPath = file.ReadLine();
                NoScallingPath = file.ReadLine();
                while ((ln = file.ReadLine()) != null)
                {
                    ScallingPaths.Add(new ScallingConfig(ln));
                }
                file.Close();
            }

            
            StorageDirectory = FileToScallPath + DateTime.Now.ToString("yyyy-MM-dd   HH_mm")+ '\\';
            


            //Directory.GetFiles("","*.jpg");
            dirs = Directory.GetFiles(FileToScallPath, "*.jpg");

            if (dirs.Length > 0)
            {
                Directory.CreateDirectory(StorageDirectory);
                LogFilePath = StorageDirectory + "_log.txt";
                if (!File.Exists(LogFilePath))
                {
                    using (StreamWriter sw = File.CreateText(LogFilePath)) ;
                }
;           }

            foreach (string filePath in dirs)
            {
                Console.WriteLine(filePath.Split('\\').Last());
                FileName = filePath.Split('\\').Last();
                
                using (Bitmap bitmap = new Bitmap(filePath))
                {
                    foreach(ScallingConfig scallingConfig in ScallingPaths)
                    {
                        ScaleImage(bitmap, scallingConfig).Save(scallingConfig.directPath + scallingConfig.FileSufix + FileName.Replace(".jpg",scallingConfig.FileSufix+".jpg")) ;   
                    }
                    bitmap.Dispose();
                    using (StreamWriter sw = File.AppendText(LogFilePath))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+'\t'+FileName+ '\t' +FileName.Split('_')[0]);
                    }
                }
                File.Copy(filePath, NoScallingPath + FileName,true);
                
                Directory.Move(filePath, StorageDirectory+ FileName);
            }

            Console.ReadLine();
        }

        public static Bitmap ScaleImage(Bitmap bmp, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / bmp.Width;
            var ratioY = (double)maxHeight / bmp.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(bmp.Width * ratio);
            var newHeight = (int)(bmp.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static Bitmap ScaleImage(Bitmap bmp, ScallingConfig scallingConfig)
        {

            return ScaleImage(bmp, scallingConfig.width, scallingConfig.height);
        }
    }
}
