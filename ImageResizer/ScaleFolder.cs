using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    class ScaleFolder
    {
        string FileToScalePath;
        string NoScalingPath;
        string FileName;
        string StorageDirectory;
        string[] dirs;
        string LogFilePath = "";
        List<ScalingConfig> ScalingPaths = new List<ScalingConfig>();

        public ScaleFolder()
        {
            using (StreamReader file = new StreamReader("../../config.txt"))
            {
                string ln;
                FileToScalePath = file.ReadLine();
                NoScalingPath = file.ReadLine();
                while ((ln = file.ReadLine()) != null)
                {
                    ScalingPaths.Add(new ScalingConfig(ln));
                }
                file.Close();
            }
        }

        public void Start()
        {
            StorageDirectory = FileToScalePath + DateTime.Now.ToString("yyyy-MM-dd   HH_mm") + '\\';
            dirs = Directory.GetFiles(FileToScalePath, "*.jpg");

            if (dirs.Length > 0)
            {
                Directory.CreateDirectory(StorageDirectory);
                LogFilePath = StorageDirectory + "_log.txt";
                if (!File.Exists(LogFilePath))
                {
                    File.CreateText(LogFilePath).Dispose();
                }
            }

            ScaleDiredImages(dirs);
        }

        private void ScaleDiredImages(string[] dirs)
        {
            foreach (string filePath in dirs)
            {
                Console.WriteLine(filePath.Split('\\').Last());
                FileName = filePath.Split('\\').Last();

                using (Bitmap bitmap = new Bitmap(filePath))
                {
                    foreach (ScalingConfig scalingConfig in ScalingPaths)
                    {
                        ScaleImage(bitmap, scalingConfig).Save(scalingConfig.directPath + scalingConfig.FileSufix + FileName.Replace(".jpg", scalingConfig.FileSufix + ".jpg"));
                    }
                    bitmap.Dispose();
                    using (StreamWriter sw = File.AppendText(LogFilePath))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + '\t' + FileName + '\t' + FileName.Split('_')[0]);
                    }
                }
                File.Copy(filePath, NoScalingPath + FileName, true);

                Directory.Move(filePath, StorageDirectory + FileName);
            }
        }

        private void ScaleDiredImages(string[] dirs, bool fillWithWhiteBackground)
        {
            if (fillWithWhiteBackground)
            {
                foreach (string filePath in dirs)
                {
                    Console.WriteLine(filePath.Split('\\').Last());
                    FileName = filePath.Split('\\').Last();

                    using (Bitmap bitmap = new Bitmap(filePath))
                    {
                        //int ratio ScalingPaths.ElementAt(0).height/ ScalingPaths.ElementAt(0).width;

                        foreach (ScalingConfig scalingConfig in ScalingPaths)
                        {
                            //ScaleImage(bitmap, scalingConfig).Save(scalingConfig.directPath + scalingConfig.FileSufix + FileName.Replace(".jpg", scalingConfig.FileSufix + ".jpg"));

                            
                        }
                        bitmap.Dispose();
                        using (StreamWriter sw = File.AppendText(LogFilePath))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + '\t' + FileName + '\t' + FileName.Split('_')[0]);
                        }
                    }
                    File.Copy(filePath, NoScalingPath + FileName, true);

                    Directory.Move(filePath, StorageDirectory + FileName);
                }
            }
            else
            {
                ScaleDiredImages(dirs);
            }            
        }

        private Bitmap ScaleImage(Bitmap bmp, int maxWidth, int maxHeight)
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

        private Bitmap ScaleImage(Bitmap bmp, ScalingConfig scallingConfig)
        {

            return ScaleImage(bmp, scallingConfig.width, scallingConfig.height);
        }
    }
}
