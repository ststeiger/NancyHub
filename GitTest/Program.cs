using System;

namespace GitTest
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			string repoURI = @"/root/sources/NancyHub";

			NancyHub.CurrentGitImplementation cgi = new NancyHub.CurrentGitImplementation ();
			bool isRepo = cgi.IsRepository (repoURI);
			System.Console.WriteLine (isRepo);


			string currentBranch = cgi.GetCurrentBranch(repoURI);
			System.Console.WriteLine (currentBranch);


			string confFile = cgi.ToInsensitiveGitString ("connections.config");
			System.Console.WriteLine (confFile);


			Console.WriteLine ("Hello World!");
		}
	}
}
