
namespace GitTest
{


    class MainClass
    {
        public static void MyExtractProgress(object sender, Ionic.Zip.ExtractProgressEventArgs e)
        {

        }

        // http://stackoverflow.com/questions/1578823/creating-a-zip-extractor
        public static void xxx()
        {

            // http://community.sharpdevelop.net/forums/t/2842.aspx
            // Added CreateExe to FastZip
            // ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
            // fz.CreateZip("zipfilename", "sourceDir", true, null);

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                // add this map file into the "images" directory in the zip archive
                zip.AddFile("c:\\images\\personal\\7440-N49th.png", "images");
                // add the report into a different directory in the archive
                zip.AddFile("c:\\Reports\\2008-Regional-Sales-Report.pdf", "files");
                zip.AddFile("ReadMe.txt");
                zip.Save("MyZipFile.zip");
            }



            string ZipToUnpack = "C1P3SML.zip";
            string TargetDir = "C1P3SML";
            System.Console.WriteLine("Extracting file {0} to {1}", ZipToUnpack, TargetDir);
            using (Ionic.Zip.ZipFile zip1 = Ionic.Zip.ZipFile.Read(ZipToUnpack))
            {
                zip1.ExtractProgress += MyExtractProgress;
                Ionic.Zip.ZipEntry e = default(Ionic.Zip.ZipEntry);
                // here, we extract every entry, but we could extract    
                // based on entry name, size, date, etc.   
                foreach (var ae in zip1)
                {
                    ae.Extract(TargetDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }
            }


            using (Ionic.Zip.ZipFile zip1 = new Ionic.Zip.ZipFile())
            {
                // zip up a directory
                zip1.AddDirectory("C:\\project1\\datafiles", "data");
                zip1.Comment = "This will be embedded into a self-extracting exe";
                zip1.AddEntry("Readme.txt", "This is content for a 'Readme' file that will appear in the zip.");
                zip1.SaveSelfExtractor("archive.exe", Ionic.Zip.SelfExtractorFlavor.ConsoleApplication);
            }

            string DirectoryToZip = "";
            string ZipFileToCreate = "";

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                zip.Comment = "This will be embedded into a self-extracting console-based exe";
                //zip.StatusMessageTextWriter = System.Console.Out;
                zip.AddDirectory(DirectoryToZip); // recurses subdirectories
                //zip.Password = "foobar";
                //zip.Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes256;
                //zip.Save(ZipFileToCreate);


                Ionic.Zip.SelfExtractorSaveOptions options = new Ionic.Zip.SelfExtractorSaveOptions();
                options.Flavor = Ionic.Zip.SelfExtractorFlavor.ConsoleApplication;
                //options.DefaultExtractDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), System.IO.Directory.GetParent(ZipFileToCreate).Name);
                options.DefaultExtractDirectory = "%TEMP%";
                options.ExtractExistingFile = Ionic.Zip.ExtractExistingFileAction.OverwriteSilently;

                // http://dotnetzip.codeplex.com/workitem/10682
                //options.IconFile = System.IO.Path.Combine(Application.StartupPath, "box_software.ico");
                //options.PostExtractCommandLine = "putty.exe";
                //options.Quiet = true;
                //options.RemoveUnpackedFilesAfterExecute = true;

                zip.SaveSelfExtractor(System.IO.Path.ChangeExtension(ZipFileToCreate, ".exe"), options);
            } // End Using zip

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
