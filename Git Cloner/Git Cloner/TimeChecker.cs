using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git_Cloner
{
    public class TimeChecker
    {
        //static DirectoryInfo d = new DirectoryInfo(@"C:\Users\karthik.kasturi\Source\Repos\karthik.kasturi\CustomAutoMapper");
        
        public static bool DirectoryChanged(DirectoryInfo directory, double checkTimeInMinutes = 15)
        { 
            //Console.WriteLine($"Checking directory {directory.FullName}");
            if (directory.Name.StartsWith("."))
            {
                return false;
            }
            foreach (var file in directory.GetFiles())
            {
                var differnceInMinutes = (DateTime.Now - file.LastWriteTime).TotalMinutes;
                if (differnceInMinutes < checkTimeInMinutes && differnceInMinutes > 0)
                {
                    return true;
                }
            }
            DirectoryInfo[] innerDirectories;
            try
            {
                innerDirectories = directory.GetDirectories();
            }
            catch (Exception e)
            {
                return false;
            }

            foreach (var innerDirectory in innerDirectories)
            {
                if (DirectoryChanged(innerDirectory))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
