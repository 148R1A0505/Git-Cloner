using System;
using System.Threading;
using System.Collections.Generic;


namespace Git_Cloner
{
    class Program
    {
        double checkTimeInMinutes;
        public static void Main(string[] args)
        {
            Dictionary<string, string> repoInfos = //stores url as key and working directory as value.
                new Dictionary<string, string>()
                {
                    { "https://github.com/148R1A0505/Git-Cloner.git", "" },
                    { "https://github.com/148R1A0505/gitrepo.git", ""}
                };

            new Program() { checkTimeInMinutes = 2 }.processThread(repoInfos).Start();
        }
        public Thread processThread(Dictionary<string, string> repoInfos)
        {
            while (true)
            {
                foreach (var repoInfo in repoInfos)
                {
                    GitRepository repository = new GitRepository(repourl: repoInfo.Key, workingDir: repoInfo.Value);
                    Console.WriteLine($"Working on {repository.Name}");
                    if (repository.IsDetached 
                        || repository.IsChangedLocally(checkTimeInMinutes) )
                    {
                        Console.WriteLine("Changed recently");
                        continue;
                    }
                    foreach (string branch in repository.GetBranches())
                    {
                        Console.WriteLine($"switching to branch {branch}");
                        string branchStatus = repository.Checkout(branch);
                        //if (repository.IsChangedLocally(checkTimeInMinutes))
                        //{
                        //    Console.WriteLine("Changed recently");
                        //    continue;
                        //}
                        if (branchStatus.Equals("mergeable"))
                        {
                            Console.WriteLine("stashing and merging");
                            repository.StashChanges();
                            repository.MergeOrigin();
                        }
                        else
                        {
                            Console.WriteLine($"Possible merge conflicts on {branch} branch in {repository.Name} in folder {repository.WorkingDirectory}");
                        }
                    }
                    repository.Restore();
                    Console.WriteLine($"Done updating {repository.Name}");
                }
                Thread.Sleep( (int) checkTimeInMinutes * 1000 * 60);
            }
            
        }
    }
}
