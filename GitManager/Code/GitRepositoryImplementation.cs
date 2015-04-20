
namespace GitManager
{


    public abstract class GitRepositoryImplementation
    {
        protected GitImplementations m_Git;
		protected System.IO.DirectoryInfo m_di;
		protected string m_Name;
		protected string m_Path;


		public GitRepositoryImplementation(System.IO.DirectoryInfo dirInf)
		{
			m_Git = new CurrentGitImplementation();
			m_di = dirInf;
			m_Name = m_di.Name;
			m_Path = m_di.FullName;
		}


		public GitRepositoryImplementation(string path) : this(new System.IO.DirectoryInfo(path))
		{ }


        public string LastModified;

		public string Name { 
			get{ 
				return m_Name;
			}
		}

		public string RepoPath { 
			get {
				return m_Path;
			}
		}



		public string Description { 
			get; set; 
		}

		public bool IsGitRepo { 
			get{ 
				return m_Git.IsRepository(this.RepoPath);
			} 
		}

		public bool IsBare { 
			get{ 

				string nonBareRepoPath = System.IO.Path.Combine(this.RepoPath, ".git");
				return !System.IO.Directory.Exists (nonBareRepoPath);
			}
		}

		public bool IsNonBare { 
			get{ 

				string nonBareRepoPath = System.IO.Path.Combine(this.RepoPath, ".git");
				return System.IO.Directory.Exists(nonBareRepoPath);
			}
		}

		public bool IsArchived { 
			get{ 
				return System.IO.File.Exists(System.IO.Path.Combine(this.RepoPath, "archived"));
			}
		}


    } // End Class GitRepositoryImplementation 


} // End namespace NancyHub 
