
namespace NancyHub
{
    using Nancy;



	// https://github.com/centic9/jgit-cookbook
	public abstract class GitImplementations
	{
		public virtual bool CreateRepo()
		{

			return false;
		}

		public virtual bool InitRepo()
		{

			return false;
		}
	}

	public class CurrentGitImplementation : GitImplementations 
	{

	}



	public abstract class GitAdmin
	{

		public virtual bool CreateRepo()
		{
			return true;
		}

		public virtual bool DeleteRepo()
		{
			return true;
		}


		public virtual bool DownloadRepo()
		{
			return true;
		}

		public virtual void GetRepo()
		{

		}

		public virtual void ListRepos()
		{


		}


		public virtual void SearchRepo()
		{

		}

		public virtual void SearchRepos()
		{


		}

	}



	public class AdminV1 : GitAdmin
	{

	}


	public class Repo
	{
		public string Name;
		public string Description;
		public string LastModified;
	}


    public class IndexModule : NancyModule
    {
		// Create repo
		// Delete repo
		// Description / Create Edit View


		// List repos
		// List file
		// View file
		// Search 

		// Download zip

		// Nice to have:
		// Last updated
		// Gist


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
