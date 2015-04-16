
namespace COR_Helper
{


    public class SafeSession
    {


        public static bool Exists(string strKey)
        {
            foreach (string strThisKey in System.Web.HttpContext.Current.Session.Keys)
            {
                if (System.StringComparer.OrdinalIgnoreCase.Equals(strThisKey, strKey))
                {
                    return true;
                }
                // StringComparer.OrdinalIgnoreCase.Equals(strThisKey, strKey)
            }

            return false;
        }
        // Exists


        public static void SetValue(string strKey, object obj)
        {
            System.Web.HttpContext.Current.Session[strKey] = obj;
        }
        // SetValue


        public static T GetValue<T>(string strKey)
        {
            bool bKeyNotFound = true;

            foreach (string strThisKey in System.Web.HttpContext.Current.Session.Keys)
            {
                if (System.StringComparer.OrdinalIgnoreCase.Equals(strThisKey, strKey))
                {
                    bKeyNotFound = false;
                    break; // TODO: might not be correct. Was : Exit For
                } // System.StringComparer.OrdinalIgnoreCase.Equals(strThisKey, strKey)
            }

            if (bKeyNotFound)
            {
                System.Diagnostics.StackTrace stStackTrace = new System.Diagnostics.StackTrace();
                string strStacktrace = stStackTrace.ToString();
                throw new SessionExpiredException("Session für Key \"" + strKey + "\" nicht gefunden. Session abgelaufen oder nicht vorhanden.", null, stStackTrace);
            } // bKeyNotFound

            object obj = System.Web.HttpContext.Current.Session[strKey];
            if (obj != null && object.ReferenceEquals(typeof(T), obj.GetType()))
            {
                return (T)obj;
            } // End if (obj != null && object.ReferenceEquals(typeof(T), obj.GetType()))

            //Return CType(obj, T)
            return (T)System.Convert.ChangeType(obj, typeof(T));
        } // GetValue


        public static void SessionExpiredRedirect(string strInstance, string strMessage)
        {
            strMessage = JavaScriptUrlEncode(ref strMessage);
            System.Web.HttpContext.Current.Response.Redirect(ContentUrl("~/DMS/frames/expired.aspx") + "?instance=" + strInstance + "&message=" + strMessage, true);
        } // SessionExpiredRedirect


        protected static string ContentUrl(string strPath)
        {
            return System.Web.VirtualPathUtility.ToAbsolute(strPath);
            //Response.Write("<h1>" + VirtualPathUtility.ToAbsolute("~/lol/yuk/Home.aspx") + "</h1>")

            //strPath = HttpContext.Current.Server.MapPath(strPath)
            //Dim strAppPath As String = HttpContext.Current.Server.MapPath("~")
            //'Dim url As String = String.Format("~{0}", strPath.Replace(strAppPath, "").Replace("\", "/"))
            //'Dim AbsolutePath As String = Request.ServerVariables("APPL_PHYSICAL_PATH")
            //'Dim AbsolutePath2 As String = HttpContext.Current.Request.ApplicationPath
            //'Dim str As String = HttpRuntime.AppDomainAppVirtualPath

            //Dim url As String = String.Format("{0}", HttpContext.Current.Request.ApplicationPath + strPath.Replace(strAppPath, "").Replace("\", "/"))

            //' https://www4.cor-asp.ch/REM_Demo_DMSc:/inetpub/wwwroot/ajax/NavigationContent.ashx?filter=nofilter1342703258627 404

            //Return url
        } // ContentUrl


        public static string JavaScriptUrlEncode(ref string strText)
        {
            string strMessage = System.Web.HttpContext.Current.Server.HtmlEncode(strText);
            //Grrr
            strMessage = Microsoft.JScript.GlobalObject.escape(strMessage);
            return System.Web.HttpContext.Current.Server.UrlEncode(strMessage);
        } // JavaScriptUrlEncode 


    } // SafeSession



    public class SessionExpiredException : System.Exception
    {

        protected System.Diagnostics.StackTrace m_RealStackTrace;

        protected string m_strHelpLink = "See stacktrace for callee location";

        public SessionExpiredException(string message)
            : base(message)
        {
        } // Constructor


        public SessionExpiredException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        } // Constructor


        public SessionExpiredException(string message, System.Exception innerException, System.Diagnostics.StackTrace stStackTraceFromPreviousThread)
            : base(message, innerException)
        {
            this.m_RealStackTrace = stStackTraceFromPreviousThread;
        } // Constructor


        public override string HelpLink
        {
            get { return this.m_strHelpLink; }
            set { this.m_strHelpLink = value; }
        } // HelpLink 


        public override string StackTrace
        {
            get
            {
                if (m_RealStackTrace != null)
                {
                    m_RealStackTrace.ToString();
                }

                return "";
            }
        } // StackTrace


    } // SessionExiredException


} // COR_Helper
