using System.Collections.Generic;
using System.IO;

namespace Git_Cloner
{
    public class GitRepository
    {
        private string _workingDir;
        private string _repourl;
        private string _initbranch;
        private ProcessCaller _processCaller;
        public GitRepository(string repourl, string workingDir = "")
        {
            _processCaller = new ProcessCaller();
            if (workingDir.Equals(string.Empty))
            {
                string[] usernameSplit = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');
                string username = usernameSplit[usernameSplit.Length - 1];
                workingDir = $"C:\\Users\\{username}\\Documents\\{getnamefromurl(repourl)}";
            }
            _workingDir = workingDir;
            _repourl = repourl;
            if (!Directory.Exists(_workingDir))
            {
                //Cloning
                string prevDir = "";
                string[] splittedWorkDir = workingDir.Split('\\');
                for (int i = 0; i < splittedWorkDir.Length - 1; i++) prevDir += splittedWorkDir[i] + "\\";
                _processCaller.GitCall(prevDir, $"clone {repourl} {_workingDir}");
            }
            //set init branch
            _initbranch = GetCurrentBranch();
            Fetch();
        }

        public string StashChanges()
        {
            return _processCaller.GitCall(_workingDir, "stash save");
        }

        public string MergeOrigin()
        {
            string branchName = GetCurrentBranch();
            return _processCaller.GitCall(_workingDir, $"merge origin/{branchName}");
        }
        public IEnumerable<string> GetBranches()
        {
            // git branch --all
            // return all origin/branches
            List<string> branches = new List<string>();
            string branchOp = _processCaller.GitCall(_workingDir, "branch --all");
            string[] branchOpLines = branchOp.Split('\n');
            foreach(string line in branchOpLines)
            {
                if(!line.Contains("->") && line.Contains("remotes/origin/"))
                {
                    int remoteindex = line.IndexOf("remotes/origin/");
                    branches.Add(line.Substring(remoteindex+15));
                }
            }
            return branches;
        }

        public void Fetch()
        {
            // Fetch repo
            _processCaller.GitCall(_workingDir, "fetch");
        }

        public void Restore()
        {
            if (! _initbranch.Equals(GetCurrentBranch()) )
            {
                // git checkout initbranch
                Checkout(_initbranch);
            }
        }

        public string GetCurrentBranch() 
        {
            //git branch
            //return starred branch
            string[] branchOp = _processCaller.GitCall(_workingDir, "branch").Split('\n');
            foreach (string line in branchOp)
            {
                if (line.Contains("*"))
                {
                    return line.Split(' ')[1];
                }
            }

            return null;
        }

        public bool IsDetached
        {
            get
            {
                if (_initbranch.Contains("detached"))
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsChangedLocally(double checkTimeInMinutes)
        {
            return TimeChecker.DirectoryChanged(new DirectoryInfo(_workingDir), checkTimeInMinutes);
            
        }

        public string Checkout(string branchName)
        {
            string[] processOutput = _processCaller.GitCall(_workingDir, $"checkout {branchName}").Split('\n');
            foreach (string line in processOutput)
            {
                if (line.Contains("deviated") || line.Contains("conflict")) return "conflict";
            }
            return "mergeable";
        }
        private static string getnamefromurl(string repourl)
        {
            string[] splitdata = repourl.Split('/');
            var namedotgit = splitdata[splitdata.Length - 1];
            return namedotgit.Substring(0, namedotgit.Length - 4);
        }
    }
}
