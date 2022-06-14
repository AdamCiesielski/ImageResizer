using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    class ScallingConfig
    {
        public string directPath;
        public int width;
        public int height;
        public string FileSufix;

        public ScallingConfig(string LineFromConfig)
        {
            string[] tmp = LineFromConfig.Split(';');

            directPath = tmp[0];
            width = int.Parse(tmp[1]);
            height = int.Parse(tmp[2]);
            FileSufix = tmp[3];
        }
    }
}
