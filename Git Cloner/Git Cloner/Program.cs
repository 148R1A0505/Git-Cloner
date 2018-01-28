using System;
using System.Threading;
using System.Collections.Generic;


namespace Git_Cloner
{
    class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, string> repoInfos = new Dictionary<string, string>()
            {
                { "https://github.com/148R1A0505/Git-Cloner.git", "" },
                { "https://github.com/148R1A0505/gitrepo.git", ""}
            };
            new Program().processThread(repoInfos).Start();

        }
        public Thread processThread(Dictionary<string, string> repoInfos)
        {
            while (true)
            {
                foreach (var repoInfo in repoInfos)
                {
                    GitRepository repository = new GitRepository(repourl: repoInfo.Key, workingDir: repoInfo.Value);
                    if (repository.IsDetached )
                        //|| repository.IsChangedLocally(2))
                    {
                        Console.WriteLine("Changed locally");
                        repository.Restore();
                        continue;
                    }
                    foreach (string branch in repository.GetBranches())
                    {
                        string branchStatus = repository.Checkout(branch);
                        Console.WriteLine($"switched to branch {branch}");
                        if (branchStatus.Equals("mergeable"))
                        {
                            repository.StashChanges();
                            repository.MergeOrigin();
                        }
                    }
                    repository.Restore();
                }
                Thread.Sleep(10000);
            }
            
        }
    }
}
