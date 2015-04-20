
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GitAdmin.Models
{



    public class ProfileOverview
    {

        public long RowNr;

        public long ProfileId;
        public string Country;
        public string Region;
        public string City;
        public string Name;

        public int? Height_Metric;
        public int? Weight_Metric;

        public string LookingFor;
        public int? AgeFrom;
        public int? AgeTo;


        public DateTime Registered;
        public DateTime BirthDate;
        public DateTime Updated;
        public DateTime LastLoggedDate;


        public DateTime LastVisited
        {
            get
            {
                return this.LastLoggedDate;
            }
            set { this.LastLoggedDate = value; }
        } // End Property LastVisited


        public string ThumbnailURL
        {
            get
            {
                System.Web.Mvc.UrlHelper Url = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
                return Url.Action("GetImg", "Gallery", new { id = this.ProfileId });
            }
        } // End Property ImageURL


        public string ImageURL
        {
            get
            {
                System.Web.Mvc.UrlHelper Url = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
                return Url.Action("GetFullImg", "Gallery", new { id = this.ProfileId });
            }
        } // End Property ImageURL



        public string AltText
        {
            get
            {
                return string.Format("Profile {0}", this.ProfileId);
            }
        } // End Property AltText 


        public string DisplayName
        {
            get { return this.Name; }
            set
            {
                this.Name = value;
            }
        } // End Property DisplayName


        // http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age
        public int Age
        {
            get
            {
                DateTime bday = this.BirthDate; // System.DateTime.UtcNow;
                bday = bday.Date;

                DateTime today = DateTime.UtcNow; //.UtcToday(); //DateTime.Today;
                int age = today.Year - bday.Year;
                if (bday > today.AddYears(-age))
                    age--;

                return age;
            }

            set { }
        } // End Property GetAge


        public string cm2MeterAndcm(int iHeightcm)
        {
            int iHeightMeter = iHeightcm / 100;
            int iHeightCentimeter = iHeightcm - iHeightMeter * 100;

            return iHeightMeter.ToString() + "m " + iHeightCentimeter.ToString() + "cm";
        } // End Function cm2MeterAndcm


        public string cm2FeetAndInch(int iHeightcm)
        {
            double feet = iHeightcm / 30.48;
            int iHeightFeet = (int)feet;
            feet = feet - (double)iHeightFeet;
            int iInches = (int)Math.Round(feet * 12, 0, MidpointRounding.AwayFromZero);

            return iHeightFeet.ToString() + "'" + iInches.ToString() + "\"";
        } // End Function cm2FeetAndInch


        public string strHeight
        {
            get
            {
                //string str = HttpContext.Current.Request.UserLanguages[0];
                //System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(str);
                //return Registered.ToString(ci.DateTimeFormat.ShortDatePattern);
                // 1 cm = inch 0.393701
                // 1 cm = 0.0328084 feet
                // 1 foot = 30.48
                // 1 foot = 12 inch
                // 1 inch = 2.54 cm
                if (Height_Metric.HasValue)
                    return cm2MeterAndcm(Height_Metric.Value) + " (" + cm2FeetAndInch(Height_Metric.Value) + ")";

                return "";
            }
        } // End Property strHeight


        public string strWeight
        {
            get
            {
                //string str = HttpContext.Current.Request.UserLanguages[0];
                //System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(str);
                //return Registered.ToString(ci.DateTimeFormat.ShortDatePattern);

                if (Weight_Metric.HasValue)
                    return Weight_Metric + "kg (" + Math.Round((Weight_Metric.Value * 2.20462), 0, MidpointRounding.AwayFromZero) + " lbs)";

                return "";
            }
        } // End Property strWeight


        public string strRegistered
        {
            get
            {
                string str = System.Web.HttpContext.Current.Request.UserLanguages[0];
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(str);
                return Registered.ToString(ci.DateTimeFormat.ShortDatePattern);
            }
        } // End Property strRegistered


        public string strLastVisited
        {
            get
            {
                string str = System.Web.HttpContext.Current.Request.UserLanguages[0];
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(str);
                ci = new System.Globalization.CultureInfo("en-us");

                return LastVisited.ToString(ci.DateTimeFormat.ShortDatePattern);
                //return LastVisited.ToString("dddd, MMMM dd, yyyy", ci); 
            }
        } // End Property strLastVisited


        public string LastVisitAgo
        {
            get
            {
                return LastVisited.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }


        public void MsgBox(object obj)
        {
            if (obj != null)
                Console.WriteLine(obj.ToString());
            else
                Console.WriteLine("obj IS NULL");
        } // End Sub MsgBox


    } // End Class ProfileOverview





}