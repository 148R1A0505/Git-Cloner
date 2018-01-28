using System;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git_Cloner
{
    class Program
    {
        static void Main(string[] args)
        {

            string repourl= "http://git.dotnettech.in/sumanth.akkala.git";
            Thread t = new Thread(LoopedFetch(repourl));

            t.Start();

            Console.ReadLine();
        }

        private static ThreadStart LoopedFetch(string repourl)
        {
            Process fetchProcess = new Process();
            string reponame = getnamefromurl(repourl);
            var fetchStartInfo = new System.Diagnostics.ProcessStartInfo
            {

                WorkingDirectory = $"C:\\Users\\sumanth.akkala\\Documents\\{reponame}",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false,
                Arguments = "/c git fetch"
            };
            fetchProcess.StartInfo = fetchStartInfo;
            while (true)
            {
                try
                {
                    Console.WriteLine($"Fetching {reponame}...");
                    fetchProcess.Start();
                    fetchProcess.WaitForExit();
                    Console.WriteLine($"Fetched {reponame}");
                }
                catch (Win32Exception e)
                {
                    if (e.ErrorCode.Equals(-2147467259))
                    {
                        Console.WriteLine($"A repo for {repourl} is not available locally with dir name {reponame}. Cloniing...");
                        using (var cloneProcess = new Process())
                        {
                            var cloneStartInfo = new System.Diagnostics.ProcessStartInfo
                            {

                                WorkingDirectory = $"C:\\Users\\sumanth.akkala\\Documents",
                                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                                FileName = "cmd.exe",
                                RedirectStandardInput = true,
                                UseShellExecute = false,
                                Arguments = $"/c git clone {repourl}"
                            };
                            cloneProcess.StartInfo = cloneStartInfo;
                            cloneProcess.Start();
                            cloneProcess.WaitForExit();
                            Console.WriteLine($"Repo {repourl} cloned.");
                            
                        }
                    }

                }
                Thread.Sleep(10000);
            }
        }

        private static string getnamefromurl(string repourl)
        {
            string[] splitdata = repourl.Split('/');
            var namedotgit =  splitdata[splitdata.Length - 1];
            return namedotgit.Substring(0,namedotgit.Length-4);
        }
    }
}
