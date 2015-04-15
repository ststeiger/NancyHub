
namespace NancyHub
{


    public class CurrentRepositoryImplementation : GitRepositoryImplementation
    {
        public GitImplementations Git = new CurrentGitImplementation();
        public NGit.Api.Git Repository;
    }


    public class CurrentGitImplementation : GitImplementations
    {
        public CurrentGitImplementation()
        {
        }
    }


}
