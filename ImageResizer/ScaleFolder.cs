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

            ScaleDiredImages(dirs, true);
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
                        Bitmap tmp;
                        double ratio = (double)ScalingPaths.ElementAt(0).height / ScalingPaths.ElementAt(0).width;
                        Console.WriteLine(ratio);

                        if (bitmap.Height / bitmap.Width > ratio)
                        {
                            tmp = new Bitmap((int)(bitmap.Width / (ratio / ((double)bitmap.Height / bitmap.Width))), bitmap.Height);
                        }
                        else
                        {
                            tmp = new Bitmap(bitmap.Width, (int)(bitmap.Height * (ratio / ((double)bitmap.Height / bitmap.Width))));
                        }

                        using (var graphics = Graphics.FromImage(tmp))
                        {
                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                            {
                                graphics.FillRectangle(brush, 0, 0, tmp.Width, tmp.Height);
                            }
                            graphics.DrawImage(bitmap, (tmp.Width - bitmap.Width) / 2, (tmp.Height - bitmap.Height) / 2, bitmap.Width, bitmap.Height);
                        }

                        foreach (ScalingConfig scalingConfig in ScalingPaths)
                        {
                            ScaleImage(tmp, scalingConfig).Save(scalingConfig.directPath + scalingConfig.FileSufix + FileName.Replace(".jpg", scalingConfig.FileSufix + ".jpg"));
                            // TO DO

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
