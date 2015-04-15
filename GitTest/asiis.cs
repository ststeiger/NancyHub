
using ICSharpCode.SharpZipLib.Zip;


// Create a Zip as a browser download attachment in IIS
namespace GitTest
{


    class asIIS
    {

        // the aspx page has just one line e.g. <%@ Page language="c#" Codebehind=...
        // but if you must run this from within a page that has other output, start with a Response.Clear();



        // This will accumulate each of the files named in the fileList into a zip file,
        // and stream it to the browser.
        // This approach writes directly to the Response OutputStream.
        // The browser starts to receive data immediately which should avoid timeout problems.
        // This also avoids an intermediate memorystream, saving memory on large files.
        //
        private void DownloadZipToBrowser(System.Collections.Generic.List<string> zipFileList)
        {
            var Response = System.Web.HttpContext.Current.Response;


            Response.ContentType = "application/zip";
            // If the browser is receiving a mangled zipfile, IIS Compression may cause this problem. Some members have found that
            //    Response.ContentType = "application/octet-stream"     has solved this. May be specific to Internet Explorer.

            Response.AppendHeader("content-disposition", "attachment; filename=\"Download.zip\"");
            Response.CacheControl = "Private";
            Response.Cache.SetExpires(System.DateTime.Now.AddMinutes(3)); // or put a timestamp in the filename in the content-disposition

            byte[] buffer = new byte[4096];

            ZipOutputStream zipOutputStream = new ZipOutputStream(Response.OutputStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

            foreach (string fileName in zipFileList)
            {

                System.IO.Stream fs = System.IO.File.OpenRead(fileName);    // or any suitable inputstream

                ZipEntry entry = new ZipEntry(ZipEntry.CleanName(fileName));
                entry.Size = fs.Length;
                // Setting the Size provides WinXP built-in extractor compatibility,
                //  but if not available, you can set zipOutputStream.UseZip64 = UseZip64.Off instead.

                zipOutputStream.PutNextEntry(entry);

                int count = fs.Read(buffer, 0, buffer.Length);
                while (count > 0)
                {
                    zipOutputStream.Write(buffer, 0, count);
                    count = fs.Read(buffer, 0, buffer.Length);
                    if (!Response.IsClientConnected)
                    {
                        break;
                    }
                    Response.Flush();
                }
                fs.Close();
            }
            zipOutputStream.Close();

            Response.Flush();
            Response.End();
        }


    }
}
