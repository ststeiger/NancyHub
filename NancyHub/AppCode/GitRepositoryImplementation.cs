
namespace NancyHub
{


    public abstract class GitRepositoryImplementation
    {
        GitImplementations giti;

        public string LastModified;


		public string RepoPath { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool IsGitRepo { get; set; }

		public bool IsBare { get; set; }

		public bool IsArchived { get; set; }

    } // End Class GitRepositoryImplementation 


} // End namespace NancyHub 
