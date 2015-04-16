
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebTest
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für Zipball
    /// </summary>
    public class Zipball : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // context.Response.ContentType = "Text/unformatiert";
            // context.Response.Write("Hello World");

            // ZipUtils.SharpZipLib.DownloadSimpleZip();

            ZipUtils.DotNetZip.ZipToHttpResponse(@"D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\NancyHub\NancyHub\EmbeddedResources");
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}