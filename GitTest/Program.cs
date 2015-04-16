
namespace GitTest
{


    class MainClass
    {
    	

		public static void xxx()
		{

			// http://community.sharpdevelop.net/forums/t/2842.aspx
			// Added CreateExe to FastZip
			// ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
			// fz.CreateZip("zipfilename", "sourceDir", true, null);

		}



        public static void Main(string[] args)
        {
            string repoURI = string.Format(@"D:\{0}\Documents\Visual Studio 2013\Projects\NancyHub", System.Environment.UserName);
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                repoURI = @"/root/sources/NancyHub";

            NancyHub.CurrentGitImplementation cgi = new NancyHub.CurrentGitImplementation();




            string confFile = cgi.ToInsensitiveGitIgnoreString("connections.config");
            System.Console.WriteLine(confFile);

            bool isRepo = cgi.IsRepository(repoURI);
            System.Console.WriteLine(isRepo);

            string currentBranch = cgi.GetCurrentBranch(repoURI);
            System.Console.WriteLine(currentBranch);



            string newRepoDir = System.IO.Path.Combine(repoURI, "Data");
            string remote = "https://github.com/ststeiger/TestRepo.git";
            // cgi.Clone(newRepoDir, remote);
            cgi.Init(newRepoDir, remote, true);
            // cgi.Init(newRepoDir, null, true);


            string remoteBranch = cgi.GetRemoteBranchId(repoURI);

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


    } // End Class MainClass 


} // End Namespace GitTest
