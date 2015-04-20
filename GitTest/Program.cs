
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


		public static void Ena()
		{
			System.IO.Directory.EnumerateDirectories("path");
			// System.IO.Directory.EnumerateFiles ();
			// System.IO.Directory.EnumerateFileSystemEntries ("path");
		}


		public static void ListDirectories()
		{
			string path= @"/root/sources/";

			/*
			System.Collections.Generic.IEnumerable<string> dl = 
				System.IO.Directory.EnumerateDirectories(path);
				
			foreach (string str in dl)
			{
				System.Console.WriteLine (str);
			}
			
			*/

			System.IO.DirectoryInfo dirinf = new System.IO.DirectoryInfo (path);

			foreach (System.IO.DirectoryInfo di in dirinf.EnumerateDirectories())
			{
				System.Console.WriteLine (di.Name);
			}


		}


        public static void Main(string[] args)
        {
			// ListDirectories();





            // string FileToCompress = "";
            // SevenZip.Compression.LZMA.SevenZipHelper.Compress(FileToCompress, FileToCompress + ".lzma");
            // SevenZip.Compression.LZMA.SevenZipHelper.Decompress(@"D:\wkhtmltopdf\msvc\x32\bin\wkhtmltox.dll.lzma", @"D:\wkhtmltopdf\msvc\x32\bin\decomp.dll");
            

            string repoURI = string.Format(@"D:\{0}\Documents\Visual Studio 2013\Projects\NancyHub", System.Environment.UserName);
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                repoURI = @"/root/sources/NancyHub";

            NancyHub.CurrentGitImplementation cgi = new NancyHub.CurrentGitImplementation();

			cgi.GetOldestCommit (repoURI);


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
