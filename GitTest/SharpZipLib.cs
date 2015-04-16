
using ZipOutputStream = ICSharpCode.SharpZipLib.Zip.ZipOutputStream;
using ZipEntry = ICSharpCode.SharpZipLib.Zip.ZipEntry;
using ZipExeBytes = ICSharpCode.SharpZipLib.Zip.ZipExeBytes;


// Create a Zip as a browser download attachment in IIS
namespace ZipUtils
{


    class SharpZipLib
    {


		public static void xxx()
		{
			ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();

			fastZip.CreateEmptyDirectories = true;
			// fastZip.Password = "foobar";


			// Include all files by recursing through the directory structure
			bool recurse = true; 

			// Dont filter any files at all 
			string filter = null;

			// System.IO.Stream strm = System.Web.HttpContext.Current.Response.OutputStream;
			// fastZip.CreateZip (strm, "sourced", true, null);

			fastZip.CreateZip("fileName.zip", @"C:\SourceDirectory", recurse, filter);
		}



		/// <summary>
		/// Function to create a selfextracting archive file
		/// </summary>
		/// <param name="zipFilename">the zip that u want to create an exe from</param>
		/// <param name="outputName">the name of the exe to create</param>
		public static void CreateExe(string zipFilename, string outputName)
		{
			//check for valid 
			if (outputName == null)
			{
				outputName = System.IO.Path.ChangeExtension(zipFilename, ".exe");
			}
			else if (outputName.Length == 0)
			{
				outputName = System.IO.Path.ChangeExtension(zipFilename, ".exe");
			}


			//make sure it ends with the exe extention
			if (!outputName.EndsWith(".exe"))
			{
				outputName = outputName + ".exe";
			}

			//make the two filestreams needed for reading zip and writing exe
			System.IO.FileStream Zip = new System.IO.FileStream(
				zipFilename
				, System.IO.FileMode.Open
				, System.IO.FileAccess.Read
				, System.IO.FileShare.Read
			);

			System.IO.FileStream exe = new System.IO.FileStream(
				outputName
				, System.IO.FileMode.Create
				, System.IO.FileAccess.Write
			);

			int ReadIn = 0, chunksize = 2048;
			byte[] buffer = new byte[chunksize];
			//write the startercode for the exe
			exe.Write(ZipExeBytes.exe, 0, ZipExeBytes.exe.Length);
			//start reading the zipfile
			ReadIn = Zip.Read(buffer, 0, chunksize);
			//add the zipp in the extention of the starter
			while (ReadIn > 0)
			{
				exe.Write(buffer, 0, ReadIn);
				ReadIn = Zip.Read(buffer, 0, chunksize);
			}

			//close the zip
			Zip.Close();

			//close the exe
			exe.Close();

			//null it out
			buffer = null;
		}





		// http://blogs.msdn.com/b/dotnetinterop/archive/2008/06/04/dotnetzip-now-can-save-directly-to-asp-net-response-outputstream.aspx

        // This will accumulate each of the files named in the fileList into a zip file,
        // and stream it to the browser.
        // This approach writes directly to the Response OutputStream.
        // The browser starts to receive data immediately which should avoid timeout problems.
        // This also avoids an intermediate memorystream, saving memory on large files.
        //
        public static void DownloadZipToBrowser(System.Collections.Generic.List<string> zipFileList)
        {
			System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;

			Response.ClearContent();
			Response.ClearHeaders();
			Response.Clear();

			Response.Buffer = false;

            Response.ContentType = "application/zip";
            // If the browser is receiving a mangled zipfile, IIS Compression may cause this problem. Some members have found that
            //    Response.ContentType = "application/octet-stream"     has solved this. May be specific to Internet Explorer.

            Response.AppendHeader("content-disposition", "attachment; filename=\"Download.zip\"");
            // Response.CacheControl = "Private";
            // Response.Cache.SetExpires(System.DateTime.Now.AddMinutes(3)); // or put a timestamp in the filename in the content-disposition

			// http://stackoverflow.com/questions/9303919/pack-empty-directory-with-sharpziplib


            byte[] buffer = new byte[4096];

			using (ZipOutputStream zipOutputStream = new ZipOutputStream (Response.OutputStream))
			{
				zipOutputStream.SetLevel (3); //0-9, 9 being the highest level of compression

				// zipOutputStream.Dispose

				foreach (string directoryName in zipFileList)
				{
					string dname = "myfolder/";

					// ZipEntry entry = new ZipEntry (ZipEntry.CleanName (fileName));
					// entry.Size = fs.Length;
				}


				foreach (string fileName in zipFileList)
				{
					// or any suitable inputstream
					using (System.IO.Stream fs = System.IO.File.OpenRead (fileName))
					{

						ZipEntry entry = new ZipEntry (ZipEntry.CleanName (fileName));
						entry.Size = fs.Length;

                
						// Setting the Size provides WinXP built-in extractor compatibility,
						// but if not available, you can set zipOutputStream.UseZip64 = UseZip64.Off instead.
						zipOutputStream.PutNextEntry (entry);

						int count = fs.Read (buffer, 0, buffer.Length);
						while (count > 0)
						{
							zipOutputStream.Write (buffer, 0, count);
							count = fs.Read (buffer, 0, buffer.Length);

							if (!Response.IsClientConnected)
								break;

							Response.Flush ();
						} // Whend 

						fs.Close ();
					} // End Using fs 

				} // Next fileName

				zipOutputStream.Close ();
			} // End Using zipOutputStream 

            Response.Flush();
            Response.End();
		} // End Function DownloadZipToBrowser


	} 


}
