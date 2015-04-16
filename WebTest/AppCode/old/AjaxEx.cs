
namespace COR.AJAX
{


    [System.Serializable()]
    public class AJAXException
    {

        // Inherits Exception
        protected System.Exception m_Exception = null;

        protected string m_Message = null;


        public bool sessionExpired
        {
            get
            {
                if (m_Exception != null)
                {
                    if (object.ReferenceEquals(m_Exception.GetType(), typeof(COR_Helper.SessionExpiredException)))
                    {
                        return true;
                    }
                }

                return false;
            }
        } // sessionExpired


        public string message
        {
            get
            {
                if (sessionExpired)
                {
                    return "Session expired";
                }

                if (!string.IsNullOrEmpty(m_Message))
                {
                    return m_Message;
                }

                if (m_Exception != null)
                {
                    return m_Exception.Message;
                }

                return "";
            }

            set { m_Message = value; }
        } // message


        public string originalMessage
        {
            get
            {
                if (m_Exception != null)
                {
                    if (!string.IsNullOrEmpty(m_Exception.Message))
                    {
                        return m_Exception.Message;
                    }

                }

                return "";
            }
        } // originalMessage


        public string source
        {
            get
            {
                if (m_Exception != null)
                {
                    if (!string.IsNullOrEmpty(m_Exception.Source))
                    {
                        return m_Exception.Source;
                    }
                }

                return "";
            }
        } // source


        public string stackTrace
        {
            get
            {
                if (m_Exception != null)
                {
                    if (!string.IsNullOrEmpty(m_Exception.StackTrace))
                    {
                        return m_Exception.StackTrace;
                    }
                }

                return "";
            }
        } // stackTrace


        public System.Exception innerException
        {
            get { return m_Exception; }
        } // innerException


        public AJAXException(System.Exception exBaseException)
        {
            //MyBase.New(exBaseException.Message, exBaseException)
            // Me.m_RealStackTrace = New StackTrace(exBaseException)
            this.m_Exception = exBaseException;
        } // Constructor


        public AJAXException(string strMessage, System.Exception exBaseException)
        {
            this.message = strMessage;
            this.m_Exception = exBaseException;
        } // Constructor


    } // AJAXException


    [System.Serializable()]
    public class JavaScriptErrorException : System.Exception
    {

        public string source
        {
            get { return "javaScript"; }
            set { }
        }


        public JavaScriptErrorException(string message, System.Exception ex)
            : base(message, ex)
        {
        }


    }


} // COR.AJAX 

