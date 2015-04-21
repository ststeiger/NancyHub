
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GitAdmin.Controllers
{


    public class HomeController : Controller
    {


        public class Repo
        {
            public string Name;
            public string Path;
			public string Description;


			public string ForkSource;
			public string LastUpdate;
        }

        
        public class RepoList
        {
            public System.Collections.Generic.List<Repo> Repositories = new List<Repo>();
        }


        public class OverviewModel
        {

            public List<Models.Win8StyleMenuPoint> lsMenuPoints = new List<Models.Win8StyleMenuPoint>();


        } // End Class OverviewModel



        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            RepoList rl = new RepoList();


			string path = @"/root/sources/";

			/*
 			System.IO.Directory.EnumerateDirectories("path");
            // System.IO.Directory.EnumerateFiles ();
            // System.IO.Directory.EnumerateFileSystemEntries ("path");
            
            System.Collections.Generic.IEnumerable<string> dl = 
                System.IO.Directory.EnumerateDirectories(path);
				
            foreach (string str in dl)
            {
                System.Console.WriteLine (str);
            } // Next str
			
            */

			System.IO.DirectoryInfo dirinf = new System.IO.DirectoryInfo(path);

			foreach (System.IO.DirectoryInfo di in dirinf.EnumerateDirectories())
			{
				rl.Repositories.Add(new Repo() { 
					 Name =  di.Name
					,Description = string.Format("Description for repository {0}", di.Name)
				});
			} // Next di

            return View(rl);
        }


        public ActionResult Overview()
        {
            OverviewModel om = new OverviewModel();

            //Guid xx = System.Web.HttpContext.Current.GetUserId<Guid>();

            //var UserFromDb = System.Web.Security.Membership.GetUser();
            //var MyUserId = UserFromDb.ProviderUserKey;

            //Console.WriteLine(UserFromDb.ToJSON());



            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "FeatureRequest", strDivClass = "B3", strSpanText = "Feature Request" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "Tasks", strDivClass = "B4", strSpanText = "My Tasks" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "IssueList", strDivClass = "B5", strSpanText = "Issue List" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "Time", strDivClass = "B6", strSpanText = "Donnerstag<br />18.04.2013<br />18:09:35" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "Dashboard", strDivClass = "B7", strSpanText = "Dashboard" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "Reports", strDivClass = "B8", strSpanText = "Reports" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "Events", strDivClass = "B8", strSpanText = "Events" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "Scheduling", strDivClass = "B8", strSpanText = "Scheduling" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "TimeTracking", strDivClass = "B6", strSpanText = "Time-Tracking" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "MyTickets", strDivClass = "B6", strSpanText = "My Tickets" });
            om.lsMenuPoints.Add(new Models.Win8StyleMenuPoint() { strId = "NewTicket", strDivClass = "B6", strSpanText = "New Ticket" });
            //om.lsMenuPoints.Add(new Win8StyleMenuPoint() { strId = "", strDivClass = "", strSpanText = "" });
            //om.lsMenuPoints.Add(new Win8StyleMenuPoint() { strId = "", strDivClass = "", strSpanText = "" });

            return View(om);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }


}
