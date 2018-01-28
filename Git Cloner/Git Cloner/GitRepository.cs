using System;
using System.IO;

namespace Git_Cloner
{
    public class GitRepository
    {
        private string _workingDir;
        private string _repourl;
        private string _initbranch;
        public GitRepository(string repourl, string workingDir = "")
        {
            if (workingDir.Equals(string.Empty))
            {
                string[] usernameSplit = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');
                string username = usernameSplit[usernameSplit.Length - 1];

                workingDir = $"C:\\Users\\{username}\\Source\\Repos\\{getnamefromurl(_repourl)}";
                
            }
            else
            {
                _workingDir = workingDir;
            }
            _repourl = repourl;
            if (!Directory.Exists(_workingDir))
            {
                //TODO: clone repository
            }
            //TODO: set init branch
            Fetch();
        }

        public string[] GetBranches()
        {
            //TODO: git branch --all
            //TODO: return all origin/branches
            throw new NotImplementedException();
        }

        public void Fetch()
        {
            throw new NotImplementedException();
            //TODO: fetch repo
        }

        public void Restore()
        {
            if (! _initbranch.Equals(GetCurrentBranch()) )  {
                //TODO: git checkout initbranch
            }
        }

        public string GetCurrentBranch() {
            //TODO: git branch
            //return starred branch

            throw new NotImplementedException();
        }

        public bool IsDetached
        {
            get
            {
                //TODO: check if init brnach is detached
                throw new NotImplementedException();
            }
        }

        public bool IsChangedLocally(double checkTimeInMinutes)
        {
            return TimeChecker.DirectoryChanged(new DirectoryInfo(_workingDir), checkTimeInMinutes);
            
        }

        private static string getnamefromurl(string repourl)
        {
            string[] splitdata = repourl.Split('/');
            var namedotgit = splitdata[splitdata.Length - 1];
            return namedotgit.Substring(0, namedotgit.Length - 4);
        }
    }
}
