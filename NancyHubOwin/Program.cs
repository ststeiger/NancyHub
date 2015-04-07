
// https://www.nuget.org/packages/Fos/
// FastCgi Server designed to run Owin applications side by side with a FastCgi enabled web server.
// using Fos;

using Nancy;
//using Nancy.Owin;
// https://www.nuget.org/packages/Nancy.Owin/1.1.0
// Nancy extensions for OWIN hosting.

using Owin;


namespace NancyHubOwin
{


    // https://github.com/NancyFx/Nancy/wiki/Hosting-Nancy-with-FastCgi
    // https://github.com/NancyFx/Nancy/wiki/Hosting-nancy-with-owin


    // http://stackoverflow.com/questions/13270163/how-to-serve-static-content-in-nancy
    // http://stackoverflow.com/questions/21123089/nancy-fails-to-find-static-content-in-custom-convention
    // https://github.com/NancyFx/Nancy/wiki/Managing-static-content

    class Program
    {


        private static void ConfigureOwin(IAppBuilder builder)
        {
            // Owin.AppBuilderExtensions.UseNancy(builder);
            builder.UseNancy();
        }


        static void Main(string[] args)
        {

            using (Fos.FosSelfHost fosServer = new Fos.FosSelfHost(ConfigureOwin))
            {
                fosServer.Bind(System.Net.IPAddress.Loopback, 9000);
                fosServer.Start(false);
            } // End Using fosServer

        } // End Sub Main 


    } // End Class Program 


} // End Namespace NancyHubOwin
