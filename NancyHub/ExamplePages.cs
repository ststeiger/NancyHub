
namespace NancyHub
{
    using Nancy;


    public class IndexModule : NancyModule
    {


        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };


            Get["/hello"] = parameters =>
            {
                // this.Request.Headers
                // this.Request.UserHostAddress
                // System.Web.HttpContext.Current.Request.ServerVariables["LOGON_USER"]

                foreach (string strKey in this.Request.Headers.Keys)
                {
                    foreach (string val in this.Request.Headers[strKey])
                    {
                        System.Console.WriteLine("{0}:{1}", strKey, val);
                    } // Next val
                } // Next strKey

                return "<html><body><h1>Hello world</h1></body></html>";
            };


            Get["/hallo"] = parameters =>
            {
                throw new System.Exception("This is a text-exception");
            };


            Get["/json"] = parameters =>
            {
                return this.Response.AsJson(new TestObject(), HttpStatusCode.OK);
            };

        } // End Constructor IndexModule


        public class TestObject
        {
            public int OBJ_ID = 123;
            public string OBJ_Name = "TestObjekt";
        } // End Constructor TestObject


    } // End Class IndexModule


} // End Namespace NancyHub 
