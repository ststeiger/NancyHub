
namespace TestApp
{


    class MainClass
    {


        public static void ConsoleMain(string[] args)
        {
            // string FileToCompress = "";
            // SevenZip.Compression.LZMA.SevenZipHelper.Compress(FileToCompress, FileToCompress + ".lzma");
            // SevenZip.Compression.LZMA.SevenZipHelper.Decompress(@"D:\wkhtmltopdf\msvc\x32\bin\wkhtmltox.dll.lzma", @"D:\wkhtmltopdf\msvc\x32\bin\decomp.dll");


            string repoURI = string.Format(@"D:\{0}\Documents\Visual Studio 2013\Projects\NancyHub", System.Environment.UserName);
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                repoURI = @"/root/sources/NancyHub";

            
            GitManager.CurrentGitImplementation cgi = new GitManager.CurrentGitImplementation();


			cgi.GetOldestCommit(repoURI);


			string repPath = cgi.GetRepoPath (repoURI);
			System.Console.WriteLine (repPath);


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
        } // End Sub Main 




		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[System.STAThread]
		static void Main(string[] args)
		{
#if false
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new frmTextEditor());
#else
			ConsoleMain(args);

			System.Console.WriteLine(System.Environment.NewLine);
			System.Console.WriteLine(" --- Press any key to continue --- ");
			System.Console.ReadKey();
#endif
		} // End Sub Main


    } // End Class MainClass 


} // End Namespace TestApp
