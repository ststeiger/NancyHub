
namespace NancyHub
{


	// http://www.mcnearney.net/blog/ngit-tutorial/
	// https://github.com/centic9/jgit-cookbook
	public abstract class GitImplementations
	{


		// ToInsensitiveGitString("connections.config")
		public virtual string ToInsensitiveGitString(string input)
		{
			char[] gitStringArray = new char[input.Length * 4 ];

			for (int i = 0; i < input.Length; ++i)
			{
				gitStringArray [i * 4 + 0] = '[';
				gitStringArray [i * 4 + 1] = char.ToUpperInvariant(input[i]);
				gitStringArray [i * 4 + 2] = char.ToLowerInvariant(input[i]);
				gitStringArray [i * 4 + 3] = ']';
			} // Next i

			input = new string (gitStringArray);
			gitStringArray = null;

			return input;
		} // End Function ToInsensitiveGitString


		public virtual bool CloseRepository(NGit.Api.Git repository)
		{
			return CloseRepository(repository, false);
		} // End Function CloseRepository


		// Since NGit is a port from Java, it doesn’t implement IDisposable when accessing files.
		// To remove its lock on files, you can dispose of the Git object by doing the following:
		public virtual bool CloseRepository(NGit.Api.Git repository, bool resetAttributes)
		{
			// NGit.Api.Git repository = NGit.Api.Git.Open (@"C:\Git\NGit");

			// Handle disposing of NGit's locks
			repository.GetRepository ().Close ();
			repository.GetRepository ().ObjectDatabase.Close ();
			repository = null;

			if (resetAttributes)
			{
				string[] files = System.IO.Directory.GetFiles (@"C:\Git\NGit", "*", System.IO.SearchOption.AllDirectories);

				// You may also want to recursively remove any read-only file attributes set by NGit in the repository’s path 
				// if you need to remove the repository later or you will receive permission exceptions when attempting to do so.
				// Remove the read-only attribute applied by NGit to some of its files
				// http://stackoverflow.com/questions/7399611/how-to-remove-a-single-attribute-e-g-readonly-from-a-file
				foreach (string file in files)
				{
					System.IO.FileAttributes attributes = System.IO.File.GetAttributes (file);
					if ((attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
					{
						attributes = RemoveAttribute (attributes, System.IO.FileAttributes.ReadOnly);
						// file.Attributes = System.IO.FileAttributes.Normal;
						System.IO.File.SetAttributes (file, attributes);
					} // End if ((attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)

				} // Next file

			} // End if (resetAttributes)

			return true;
		} // End Function CloseRepository


		private static System.IO.FileAttributes RemoveAttribute(System.IO.FileAttributes attributes, System.IO.FileAttributes attributesToRemove)
		{
			return attributes & ~attributesToRemove;
		} // End Function RemoveAttribute 


		// https://code.google.com/p/egit/wiki/JGitTutorialRepository
		// http://www.codeaffine.com/2014/09/22/access-git-repository-with-jgit/
		// Git is lazy. An empty, just initialized repository does not have an object database
		// Therefore the test will return false for an empty repository even though it is a perfectly valid repository.
		// What has proven a better approach so far is to test if the HEAD reference exists:
		// Even an empty repository has a HEAD and getRef() returns only null if there is actually no repository.
		public virtual bool IsRepository(string path)
		{
			// Git git = Git.open( new F‌ile( "/path/to/repo/.git" ) );

			// The method expects a File parameter that denotes the directory in which the repository is located. 
			// The directory can either point to the work directory or the git directory. 
			// Again, I recommend to use the git directory here.

			try
			{
				NGit.Api.Git repository = NGit.Api.Git.Open (path);
				bool b = repository.GetRepository ().ObjectDatabase.Exists ();
				if (repository.GetRepository ().GetRef ("HEAD") != null)
					b = true;

				// string name = repository.GetRepository().GetRef("HEAD").GetName();
				// System.Console.WriteLine(name);

				CloseRepository(repository);

				return b;
			}
			catch(System.Exception)
			{
			}

			return false;
		} // End Function IsRepository 


		public virtual string GetCurrentBranch(string path)
		{
			// Wrong result
			// NGit.Storage.File.FileRepository dir = new NGit.Storage.File.FileRepository (path);
			// string branch = dir.GetBranch ();
			// return branch;

			NGit.Api.Git repository = NGit.Api.Git.Open (path);

#if false
			System.Collections.Generic.IList<NGit.Ref> refsR = repository.BranchList ().SetListMode (NGit.Api.ListBranchCommand.ListMode.ALL).Call ();

			foreach (NGit.Ref refa in refsR)
			{
			//System.out.println("Branch: " + refa + " " + refa.GetName() + " " + ref.GetObjectId().Name);
			System.Console.WriteLine ("Branch: {0} \nGetname: {1} \n Name: {2}"
			, refa, refa.GetName (), refa.GetObjectId ().Name);
			} // Next refa
#endif
			string branchName = null;
			string head = repository.GetRepository ().GetFullBranch ();

			if (head.StartsWith ("refs/heads/"))
			{
				// Print branch name with "refs/heads/" stripped.
				branchName = repository.GetRepository ().GetBranch ();
				// System.Console.WriteLine ("Current branch is \"{0}\".", branchName);
			} // End if (head.StartsWith ("refs/heads/"))

			CloseRepository(repository);

			return branchName;
		} // End Function GetCurrentBranch


		public virtual void Init(string path)
		{
			// https://github.com/centic9/jgit-cookbook/blob/master/src/main/java/org/dstadler/jgit/porcelain/InitRepository.java
			// Sharpen.FilePath pth = new Sharpen.FilePath ("path");
			NGit.Api.Git repo = NGit.Api.Git.Init().SetDirectory(path).Call();

			// NGit.Storage.File.FileRepositoryBuilder frb = new NGit.Storage.File.FileRepositoryBuilder();

			// http://stackoverflow.com/questions/13667988/how-to-use-ls-remote-in-ngit
			// after init, you can call the below line to open
			// Git git = Git.Open(new FilePath(path));


			// git remote origin
			NGit.StoredConfig config = repo.GetRepository().GetConfig();
			config.SetString ("remote", "origin", "url", @"http://user:password@github.com/user/repo1.git");
			config.Save ();


			// git ls-remote
			System.Collections.Generic.ICollection<NGit.Ref> refs = repo.LsRemote ().SetRemote ("origin").Call ();

			// NGit.Ref master = refs.FirstOrDefault(a => a.GetName() == "refs/heads/master");
			NGit.Ref master = null;
			foreach (NGit.Ref a in refs)
			{
				if (a.GetName () == "refs/heads/master")
				{
					master = a;
					break;
				} // End if (a.GetName() == "refs/heads/master")
			} // Next a 


			if (master != null)
			{
				string hash = master.GetObjectId ().Name;
			} // End if (master != null)

		} // End Sub Init


		public virtual void ListTags(string path)
		{
			NGit.Api.Git repository = NGit.Api.Git.Open (path);

			// repository.GetRepository().GetTags()

			System.Collections.Generic.IList<NGit.Ref> call = repository.TagList ().Call ();

			foreach (NGit.Ref refa in call)
			{
				System.Console.WriteLine ("Tag: " + refa + " " + refa.GetName () + " " + refa.GetObjectId ().Name);

				NGit.Api.LogCommand log = repository.Log ();

				if (refa.GetPeeledObjectId () != null)
					log.Add (refa.GetPeeledObjectId ());
				else
					log.Add (refa.GetObjectId ());

				Sharpen.Iterable<NGit.Revwalk.RevCommit> logs = log.Call ();

				foreach (NGit.Revwalk.RevCommit rev in logs)
				{
					System.Console.WriteLine ("Commit: " + rev /* + ", name: " + rev.getName() + ", id: " + rev.getId().getName() */);
				} // Next rev

			} // Next refa

			CloseRepository(repository);
		} // End Sub ListTags


		public virtual void ListBranches(string path)
		{
			NGit.Api.Git repository = NGit.Api.Git.Open (path);

			// Local only
			System.Collections.Generic.IList<NGit.Ref> refs = repository.BranchList ().Call ();

			foreach (NGit.Ref refa in refs)
			{
				//System.out.println("Branch: " + refa + " " + refa.GetName() + " " + ref.GetObjectId().Name);
				System.Console.WriteLine ("Branch: " + refa + " " + refa.GetName () + " " + refa.GetObjectId ().Name);
			} // Next refa


			System.Collections.Generic.IList<NGit.Ref> refsR = repository.BranchList ().SetListMode (NGit.Api.ListBranchCommand.ListMode.ALL).Call ();

			foreach (NGit.Ref refa in refsR)
			{
				//System.out.println("Branch: " + refa + " " + refa.GetName() + " " + ref.GetObjectId().Name);
				System.Console.WriteLine ("Branch: " + refa + " " + refa.GetName () + " " + refa.GetObjectId ().Name);
			} // Next refa

			CloseRepository(repository);
		}


		public virtual void ListCommits()
		{ 
			NGit.Api.Git repository = NGit.Api.Git.Open (@"C:\Git\NGit");

			Sharpen.Iterable<NGit.Revwalk.RevCommit> la = repository.Log ().All ().Call ();

			int count = 0;
			foreach (NGit.Revwalk.RevCommit commit in la)
			{
				System.Console.WriteLine ("LogCommit: " + commit);
				count++;
			} // Next commit

			CloseRepository(repository);
		}


		public virtual void Fetch()
		{
			NGit.Api.Git repository = NGit.Api.Git.Open (@"C:\Git\NGit");

			// Fetch changes without merging them
			NGit.Transport.FetchResult fetch = repository.Fetch ().Call ();

			// Pull changes (will automatically merge/commit them)
			NGit.Api.PullResult pull = repository.Pull ().Call ();


			// Get the current branch status
			NGit.Api.Status status = repository.Status ().Call ();

			// The IsClean() method is helpful to check if any changes
			// have been detected in the working copy. I recommend using it,
			// as NGit will happily make a commit with no actual file changes.
			bool isClean = status.IsClean ();


			// You can also access other collections related to the status
			System.Collections.Generic.ICollection<string> added = status.GetAdded ();
			System.Collections.Generic.ICollection<string> changed = status.GetChanged ();
			System.Collections.Generic.ICollection<string> removed = status.GetRemoved ();


			// Clean our working copy
			System.Collections.Generic.ICollection<string> clean = repository.Clean ().Call ();

			// Add all files to the stage (you could also be more specific)
			NGit.Dircache.DirCache add = repository.Add ()
					.AddFilepattern (".")
					.Call ();

			// Remove files from the stage
			NGit.Dircache.DirCache remove = repository.Rm ()
					.AddFilepattern (".gitignore")
					.Call ();
		} // End Sub Fetch


		// http://stackoverflow.com/questions/8234373/committing-and-pushing-to-github-using-jgit-bare-repo
		// http://programcreek.com/java-api-examples/index.php?api=org.eclipse.jgit.lib.RepositoryBuilder
		public virtual void UnleashHeavokOnBareRepo()
		{
			NGit.Storage.File.FileRepositoryBuilder builder = new NGit.Storage.File.FileRepositoryBuilder ();

			var repository = builder.SetGitDir (new Sharpen.FilePath ("path/to/my/repo"))
					.ReadEnvironment () // scan environment GIT_* variables
			                  //.SetBare() // Create bare repo
					.FindGitDir () // scan up the file system tree
					.Build ();

			NGit.Api.Git git = new NGit.Api.Git (repository);
			NGit.Api.AddCommand add = git.Add ();
			try
			{
				add.AddFilepattern ("PlayState.as").Call ();
			}
			catch (System.Exception)
			{
			}
		}


		public virtual void CreateBranch(string path, string branchName)
		{
			// Here is a snippet that corresponds to the --set-upstream option to git branch:
			NGit.Api.Git repository = NGit.Api.Git.Open (path);

			NGit.Api.CreateBranchCommand create = repository.BranchCreate ();
			create.SetUpstreamMode (NGit.Api.CreateBranchCommand.SetupUpstreamMode.SET_UPSTREAM);
			create.SetName (branchName);
			create.SetStartPoint ("origin/" + branchName);
			create.SetForce (true);
			// Beware that you also need to have .setForce(true) 
			// if the branch already exists locally and you only need to track it remotely. 
			create.Call ();

			CloseRepository(repository);
		}




		public virtual void Commit()
		{
			NGit.Api.Git repository = NGit.Api.Git.Open (@"C:\Git\NGit");

			NGit.PersonIdent author = new NGit.PersonIdent ("Lance Mcnearney", "lance@mcnearney.net");
			string message = "My commit message";

			// Commit our changes after adding files to the stage
			NGit.Revwalk.RevCommit commit = repository.Commit ()
					.SetMessage (message)
					.SetAuthor (author)
					.SetAll (true) // This automatically stages modified and deleted files
					.Call ();

			// Our new commit's hash
			NGit.ObjectId hash = commit.Id;

			// Push our changes back to the origin
			Sharpen.Iterable<NGit.Transport.PushResult> push = repository.Push ().Call ();
		}


		public virtual void Reset()
		{
			NGit.Api.Git repository = NGit.Api.Git.Open (@"C:\Git\NGit");

			NGit.Ref reset = repository.Reset ()
					.SetMode (NGit.Api.ResetCommand.ResetType.HARD)
					.SetRef ("origin/master")
					.Call ();

			CloseRepository(repository);
		}


		public virtual void Open()
		{
			NGit.Api.Git repo = NGit.Api.Git.Open (@"C:\Git\NGit");
		}

		public virtual void SetCredentials()
		{
			NGit.Api.Git repository = null;

			NGit.Transport.CredentialsProvider credentials = new NGit.Transport.UsernamePasswordCredentialsProvider ("username", "password");

			// On a per-command basis
			NGit.Transport.FetchResult fetch = repository.Fetch ()
					.SetCredentialsProvider (credentials)
					.Call ();

			// Or globally as the default for each new command
			NGit.Transport.CredentialsProvider.SetDefault (credentials);
		}




		public class CustomConfigSessionFactory : NGit.Transport.JschConfigSessionFactory
		{
			public string PrivateKey { get; set; }

			public string PublicKey { get; set; }

			// Quick tip: If you have a GIT_SSH environment variable set,
			// Jsch will use that instead of using the configured JschConfigSessionFactory
			// when initializing its SSH connection.
			// In my case, it will calling TortoisePlink.exe.

			/*
            private string PrivateKeyPath { get; set; }

            public CustomConfigSessionFactory(string privateKeyPath)
            {
                PrivateKeyPath = privateKeyPath;

                // Clear the GIT_SSH environment variable as NGit will use it 
                // for SSH transport instead of the session factory
                System.Environment.SetEnvironmentVariable("GIT_SSH", string.Empty, System.EnvironmentVariableTarget.Process);
            }
            */

			protected override void Configure(NGit.Transport.OpenSshConfig.Host hc, NSch.Session session)
			{
				Sharpen.Properties config = new Sharpen.Properties ();
				config ["StrictHostKeyChecking"] = "no";
				config ["PreferredAuthentications"] = "publickey";
				session.SetConfig (config);

				NSch.JSch jsch = this.GetJSch (hc, NGit.Util.FS.DETECTED);
				jsch.AddIdentity ("KeyPair", System.Text.Encoding.UTF8.GetBytes (PrivateKey), System.Text.Encoding.UTF8.GetBytes (PublicKey), null);
			}

		}
		// End Class CustomConfigSessionFactory


		// http://stackoverflow.com/questions/13764435/ngit-making-a-connection-with-a-private-key-file/
		public virtual void CloneWithPrivateKey()
		{
			CustomConfigSessionFactory customConfigSessionFactory = new CustomConfigSessionFactory ();
			customConfigSessionFactory.PrivateKey = "properties.PrivateKey"; // Enter own
			customConfigSessionFactory.PublicKey = "properties.PublicKey"; // Enter own

			NGit.Transport.JschConfigSessionFactory.SetInstance (customConfigSessionFactory);

			NGit.Api.Git git = NGit.Api.Git.CloneRepository ()
					.SetDirectory ("properties.OutputPath")
					.SetURI ("properties.SourceUrlPath")
					.SetBranchesToClone (new System.Collections.ObjectModel.Collection<string> () { "master" })
					.Call ();
		}



		public virtual bool CreateRepo()
		{

			return false;
		}

		public virtual bool InitRepo()
		{

			return false;
		}



		public virtual void Clone(string dir, string url)
		{
			NGit.Api.CloneCommand clone = NGit.Api.Git.CloneRepository ().SetDirectory ("dir").SetURI (url);
			NGit.Api.Git repo = clone.Call ();
		}



	}


}
