
namespace ZipUtils
{


	public class DotNetZip
	{

		// zzzzzzz
		// http://blogs.msdn.com/b/dotnetinterop/archive/2008/06/04/dotnetzip-now-can-save-directly-to-asp-net-response-outputstream.aspx
		public void ZipToHttpResponse(string DirectoryToZip)
		{
			string ReadmeText= "This is a zip file dynamically generated at " + System.DateTime.Now.ToString("G");

			System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
			Response.Clear();

			string fileName = @"Довнлоад.zip"; // @"Довнлоад.зип"
			fileName = @"Download.zip";

			Response.ContentType = "application/zip";
			Response.AddHeader("content-disposition", "filename=" + fileName);

			//using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(Response.OutputStream)) 
			using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(System.Text.Encoding.UTF8))
			{
				// Add to the zip archive, the file selected in the dropdownlist.
				// zip.AddFile(ListOfFiles.SelectedItem.Text, "files");
				// Add a boilerplate copyright file into the zip.
				// zip.AddFile(@"\static\Copyright.txt", "");
				// The string ReadmeText becomes the content for an entry in the zip archive, with the filename "Readme.txt".
				// zip.AddStringAsFile(ReadmeText, "Readme.txt", "");

				zip.Comment = "This will be embedded into a self-extracting exe";
				zip.AddEntry("ReadMeZB.txt", ReadmeText);

				// http://dotnetslackers.com/articles/aspnet/Use-ASP-NET-and-DotNetZip-to-Create-and-Extract-ZIP-Files.aspx
				// zip.Password = "Rumpelstilzchen";
				// zip.Encryption = Ionic.Zip.EncryptionAlgorithm.PkzipWeak;

				//zip.StatusMessageTextWriter = System.Console.Out;
				zip.AddDirectory(DirectoryToZip); // recurses subdirectories

				// zip.Save();
				zip.Save(Response.OutputStream);
			} // End Using zip

			Response.End();
		} // End Sub ZipToHttpResponse 


		public static void ZipWithDotNetZip()
		{

			using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
			{
				// add this map file into the "images" directory in the zip archive
				zip.AddFile("c:\\images\\personal\\7440-N49th.png", "images");
				// add the report into a different directory in the archive
				zip.AddFile("c:\\Reports\\2008-Regional-Sales-Report.pdf", "files");
				zip.AddFile("ReadMe.txt");
				zip.Save("MyZipFile.zip");
			} // End Using zip

		} // End Sub ZipWithDotNetZip 


		public static void ZipAutoExtract()
		{

			using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
			{
				// zip up a directory
				zip.AddDirectory("C:\\project1\\datafiles", "data");

				zip.Comment = "This will be embedded into a self-extracting exe";
				zip.AddEntry("Readme.txt", "This is content for a 'Readme' file that will appear in the zip.");
				zip.SaveSelfExtractor("archive.exe", Ionic.Zip.SelfExtractorFlavor.ConsoleApplication);
			} // End Using zip 

		} // End Using ZipAutoExtract 


		public static void MyExtractProgress(object sender, Ionic.Zip.ExtractProgressEventArgs e)
		{

		}


		// public delegate void ExtractProgress_t(object sender, Ionic.Zip.ExtractProgressEventArgs e);


		public static void Unzip(string zipToUnpack, string targetDir)
		{
			// Unzip (zipToUnpack, targetDir, MyExtractProgress);
			Unzip (zipToUnpack, targetDir, null);
		}


		// Unzip using DotNetZip
		// Unzip("abc.zip", "/foo/bar");
		public static void Unzip(string zipToUnpack, string targetDir, System.EventHandler<Ionic.Zip.ExtractProgressEventArgs> extractProgress)
		{
			System.Console.WriteLine("Extracting file {0} to {1}", zipToUnpack, targetDir);
			using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipToUnpack))
			{
				if(extractProgress != null)
					zip.ExtractProgress += extractProgress;
				// here, we extract every entry, but we could extract    
				// based on entry name, size, date, etc.   
				foreach (Ionic.Zip.ZipEntry e in zip)
				{
					e.Extract(targetDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
				}
			}

		}



		// http://stackoverflow.com/questions/1578823/creating-a-zip-extractor
		public static void CreateSelfExtra()
		{
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

		} // End Sub CreateSelfExtra


	} // End Class 


} // End Namespace 
