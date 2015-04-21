
namespace GitManager
{


	// http://meta.stackexchange.com/questions/224873/all-stack-exchange-data-dumps/224922#224922 
    public abstract class GitRepository
    {
        protected GitImplementation m_Git;
		protected System.IO.DirectoryInfo m_di;
		protected string m_Name;
		protected string m_Path;


		public GitRepository(System.IO.DirectoryInfo dirInf)
		{
			m_Git = new CurrentGitImplementation();
			m_di = dirInf;
			m_Name = m_di.Name;
			m_Path = m_di.FullName;
		}


        public GitRepository(string path)
            : this(new System.IO.DirectoryInfo(path))
		{ }


        public static GitRepository CreateInstance(string path)
        {
            return new CurrentRepositoryImplementation(path);
        }


        public static GitRepository CreateInstance(System.IO.DirectoryInfo di)
        {
            return new CurrentRepositoryImplementation(di);
        }


        protected System.DateTime? m_LastModified;

        public System.DateTime LastModified
        {
            get
            {
                if (m_LastModified.HasValue)
                    return m_LastModified.Value;

                m_LastModified = m_Git.GetLastCommitDate(this.RepoPath);

                return m_LastModified.Value;
            }
        }


        public string LastModifiedString
        {
            get {
                return this.LastModified.ToString("dd'.'MM'.'yyyy' 'HH':'mm':'ss' ('UTC')'");
            }
        }


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
            get {
                return m_Git.GetDescription(this.RepoPath);
            }
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


		protected System.Random m_seed = new System.Random();


		public int StarCount
		{
			get{ 
				return m_seed.Next (100, 501);
			}
		}


		public int ForkCount
		{
			get{ 
				return m_seed.Next (100, 501);
			}
		}

		public string ProgrammingLanguage
		{
			get{ 
				return "C#";
			}
		}


		public bool HasForkSource
		{
			get{ 
				return !string.IsNullOrWhiteSpace (ForkSource);
			}
		}

		public string ForkSource
		{
			get{ 
				return "";
			}
		}



    } // End Class GitRepositoryImplementation 


} // End namespace NancyHub 
