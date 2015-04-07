
namespace NancyHub
{

    using Nancy;


    public class Bootstrapper : DefaultNancyBootstrapper
    {


        public void OMG(System.IO.Stream strm)
        {
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(strm, enc))
            {
                sw.Write("OMG");
                sw.Flush();//otherwise you are risking empty stream
            } // End using sw

        } // End Sub OMG


        public Response CustomErrorMessage(NancyContext context, System.Exception ex)
        {
            // return HttpStatusCode.InternalServerError;

            return new Response()
            {
                StatusCode = HttpStatusCode.NotImplemented
                ,ContentType="text/html"
              //,Contents = OMG // Cannot pass exception...
                ,Contents = delegate(System.IO.Stream strm)
                            {
                                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(strm, enc))
                                {
                                    sw.Write("OMG: " + ex.Message);
                                    sw.Flush();//otherwise you are risking empty stream
                                } // End Using sw

                            } // End Delegate
                
            };
        } // End Function CustomErrorMessage


        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            pipelines.OnError += CustomErrorMessage;

            base.ApplicationStartup(container, pipelines);
        } // End Sub ApplicationStartup 


    } // End Class Bootstrapper : DefaultNancyBootstrapper


} // End Namespace NancyHub
