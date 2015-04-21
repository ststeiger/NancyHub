
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace GitAdmin.Controllers
{


    public class HomeController : Controller
    {


        public class RepoList
        {
            public List<GitManager.GitRepository> Repositories = new List<GitManager.GitRepository>();
        } // End Class RepoList


        public class OverviewModel
        {
            public List<Models.Win8StyleMenuPoint> lsMenuPoints = new List<Models.Win8StyleMenuPoint>();
        } // End Class OverviewModel


        public static string GetRepoServerPath()
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~");
            path = System.IO.Path.Combine(path, "..", "..");
            path = System.IO.Path.GetFullPath(path);

            return path;
        }

        public ActionResult New(string id)
        {
            string str = null;
            return View(str);
        }


		public ActionResult Search(string id, string q)
		{
            return new RedirectResult(Url.Action("Index", "Home", new { id = id + q }));
		}


		public ActionResult Index(string id)
        {
            string path = GetRepoServerPath();
			ViewBag.SearchTerm = id;



            RepoList rl = new RepoList();
			System.IO.DirectoryInfo dirinf = new System.IO.DirectoryInfo(path);

			foreach (System.IO.DirectoryInfo di in dirinf.EnumerateDirectories())
			{
                GitManager.GitRepository repo = GitManager.GitRepository.CreateInstance(di);
                if(!repo.IsGitRepo)
                    continue;

				int pos = 1;

				if(!string.IsNullOrWhiteSpace(id))
					pos = System.Globalization.CultureInfo.InvariantCulture
					.CompareInfo.IndexOf (repo.Name, id, System.Globalization.CompareOptions.IgnoreCase);

				if(pos != -1)
                	rl.Repositories.Add(repo);
			} // Next di

            rl.Repositories.Sort(
                delegate(GitManager.GitRepository g1, GitManager.GitRepository g2)
                {
                    //return g1.Name.CompareTo(g2.Name); // ASC 
                    // return g1.LastModified.CompareTo(g2.LastModified);// ASC 

                    return g2.LastModified.CompareTo(g1.LastModified); // DESC 
                }
            );

            return View(rl);
        } // End Action Index 


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
        } // End Action Overview 


        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        } // End Action About


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        } // End Action Contact


    } // End Class HomeController : Controller


} // End Namespace GitAdmin.Controllers
