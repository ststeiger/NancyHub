﻿
namespace GitAdmin
{

    public abstract class GitAdministration 
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



    public class AdminV1 : GitAdministration
    {

    }

} 
