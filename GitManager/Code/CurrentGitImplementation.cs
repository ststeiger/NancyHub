
namespace GitManager
{


    public class CurrentRepositoryImplementation : GitRepositoryImplementation
    {
		public CurrentRepositoryImplementation(string path) : base(path)
		{}


        public GitImplementations Git = new CurrentGitImplementation();
        public NGit.Api.Git Repository;
    }


    public class CurrentGitImplementation : GitImplementations
    {

        // public CurrentGitImplementation()
        // { }
    }


}
