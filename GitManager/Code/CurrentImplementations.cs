
namespace GitManager
{


    public class CurrentRepositoryImplementation : GitRepository
    {
        public CurrentRepositoryImplementation(string path)
            : base(path)
        { }

        public CurrentRepositoryImplementation(System.IO.DirectoryInfo di)
            : base(di)
        { }


        public GitImplementation Git = new CurrentGitImplementation();
        public NGit.Api.Git Repository;
    }


    public class CurrentGitImplementation : GitImplementation
    {

        // public CurrentGitImplementation()
        // { }
    }


}
