using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git_Cloner
{
    class ProcessCaller
    {
        internal string gitCall(string WorkDir, string args)
        {
            Process gitProcess = new Process();
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                WorkingDirectory = WorkDir,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                Arguments = $"/c {args}"
            };
            gitProcess.StartInfo = processStartInfo;
            gitProcess.Start();
            string output = gitProcess.StandardOutput.ReadToEnd();
            gitProcess.WaitForExit();
            return output;
        }
    }
}
