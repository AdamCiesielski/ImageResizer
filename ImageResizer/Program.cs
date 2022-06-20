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
            ScaleFolder tmp = new ScaleFolder();
            tmp.Start();
            
            Console.ReadLine();
        }
    }
}
