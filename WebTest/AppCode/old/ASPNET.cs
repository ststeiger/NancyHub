
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;


using System.Web.UI;


namespace COR.ASP
{

	//http://weblogs.asp.net/avnerk/archive/2006/05/23/Serilalizing-Dictionaries.aspx

	public class NET
	{


		public static void DownloadFile(string strFileName, byte[] Buffer)
		{
			DownloadFile(strFileName, Buffer, true);
		}
		// DownloadFile


		public static void DownloadFile(string strFileName, byte[] Buffer, bool EndRequest)
		{
			string strDefaultDisposition = "attachment";

			DownloadFile(strFileName, strDefaultDisposition, Buffer, EndRequest);
		}
		// DownloadFile


		public static void DownloadFile(string strFileName, string strDisposition, byte[] Buffer, bool EndRequest)
		{
			DownloadFile(strFileName, strDisposition, "application/pdf", Buffer, EndRequest);
		}
		// DownloadFile


		public static void DownloadFile(string strFileName, string strDisposition, string strMime, byte[] Buffer)
		{
			DownloadFile(strFileName, strDisposition, strMime, Buffer, true);
		}
		// DownloadFile


		public static void DownloadFile(string strFileName, string strDisposition, string strMime, byte[] Buffer, bool EndRequest)
		{
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.ClearHeaders();
			System.Web.HttpContext.Current.Response.ClearContent();

			//System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", GetContentDisposition(strFileName))
			string strDispo = GetContentDisposition(strFileName, strDisposition);
			System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", strDispo);

			if (Buffer == null) {
				System.Web.HttpContext.Current.Response.AddHeader("Content-Length", "0");
			} else {
				System.Web.HttpContext.Current.Response.AddHeader("Content-Length", Buffer.Length.ToString());
			}



			// http://superuser.com/questions/219870/how-to-open-pdf-in-chromes-integrated-viewer-without-downloading-it#
			System.Web.HttpContext.Current.Response.ContentType = strMime;
			//  "application/octet-stream"

			//System.Web.HttpContext.Current.Response.TransmitFile(File.FullName)
			System.Web.HttpContext.Current.Response.BinaryWrite(Buffer);
			System.Web.HttpContext.Current.Response.Flush();

			if (EndRequest) {
				System.Web.HttpContext.Current.Response.End();
			}

		}
		// DownloadFile


		// COR.ASP.NET.GetParameter("id")
		public static string GetParameter(string strRequestedKey)
		{
			string strValue = null;
			if (StringComparer.OrdinalIgnoreCase.Equals(System.Web.HttpContext.Current.Request.HttpMethod, "GET")) {
				//For Each strKey As String In HttpContext.Current.Request.QueryString.Keys
				//    strValue = HttpContext.Current.Request.QueryString(strKey)

				//    Dim strPrint As String = strKey + " = " + strValue

				//    If StringComparer.OrdinalIgnoreCase.Equals(strKey, "id") Then
				//        Exit For
				//    End If
				//Next strKey

				return System.Web.HttpContext.Current.Request.QueryString[strRequestedKey];
			} else if (StringComparer.OrdinalIgnoreCase.Equals(System.Web.HttpContext.Current.Request.HttpMethod, "POST")) {
				//For Each strKey As String In HttpContext.Current.Request.Form.AllKeys
				//    strValue = HttpContext.Current.Request.Form(strKey)
				//    Dim strPrint As String = strKey + " = " + strValue
				//    If StringComparer.OrdinalIgnoreCase.Equals(strKey, "id") Then
				//        Exit For
				//    End If
				//Next strKey

				return System.Web.HttpContext.Current.Request.Form[strRequestedKey];
			} else {
				throw new System.Web.HttpException(500, "Invalid request method");
			}

			return null;
		}
		// GetParameter


		// COR.ASP.NET.StripInvalidFileNameChars("")
		public static string StripInvalidFileNameChars(string str)
		{
			string strReturnValue = null;

			if (str == null) {
				return strReturnValue;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			char[] achrInvalid = System.IO.Path.GetInvalidFileNameChars();
			foreach (char cThisChar in str) {
				bool bIsValid = true;

				foreach (char cInvalid in achrInvalid) {
					if (cThisChar == cInvalid) {
						bIsValid = false;
						break; // TODO: might not be correct. Was : Exit For
					}
				}

				if (bIsValid) {
					sb.Append(cThisChar);
				}
			}

			strReturnValue = sb.ToString();
			sb = null;
			return strReturnValue;
		}
		// StripInvalidFileNameChars


		// COR.ASP.NET.StripInvalidPathChars("")
		public static string StripInvalidPathChars(string str)
		{
			string strReturnValue = null;

			if (str == null) {
				return strReturnValue;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			char[] achrInvalidPathChars = System.IO.Path.GetInvalidPathChars();

			foreach (char cThisChar in str) {
				bool bIsValid = true;

				foreach (char cInvalid in achrInvalidPathChars) {
					if (cThisChar == cInvalid) {
						bIsValid = false;
						break; // TODO: might not be correct. Was : Exit For
					}
				}

				if (bIsValid) {
					sb.Append(cThisChar);
				}
			}

			strReturnValue = sb.ToString();
			sb = null;
			return strReturnValue;
		}
		// StripInvalidPathChars


		// COR.ASP.NET.StripInvalidFileNameAndPathChars("")
		public static string StripInvalidFileNameAndPathChars(string str)
		{
			string strReturnValue = null;
			strReturnValue = StripInvalidFileNameChars(str);
			strReturnValue = StripInvalidPathChars(strReturnValue);

			if (strReturnValue != null) {
				strReturnValue = strReturnValue.Trim();
			}

			// If filename consists entirely out of invalid path chars || filename = string.empty
			if (string.IsNullOrEmpty(strReturnValue)) {
				string extension = System.IO.Path.GetExtension(str);
				strReturnValue = "Download" + extension;
			}

			return strReturnValue;
		}
		// StripInvalidFileNameAndPathChars


		public static string GetContentDisposition(string strFileName)
		{
			return GetContentDisposition(strFileName, "attachment");
		}
		// GetContentDisposition


		// http://www.iana.org/assignments/cont-disp/cont-disp.xhtml
		public static string GetContentDisposition(string strFileName, string strDisposition)
		{
			// http://stackoverflow.com/questions/93551/how-to-encode-the-filename-parameter-of-content-disposition-header-in-http
			string contentDisposition = null;
			strFileName = StripInvalidFileNameAndPathChars(strFileName);

			if (string.IsNullOrEmpty(strDisposition)) {
				strDisposition = "inline";
			}

			if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request.Browser != null) {
				if ((System.Web.HttpContext.Current.Request.Browser.Browser == "IE" & (System.Web.HttpContext.Current.Request.Browser.Version == "7.0" | System.Web.HttpContext.Current.Request.Browser.Version == "8.0"))) {
					contentDisposition = strDisposition + "; filename=" + Uri.EscapeDataString(strFileName).Replace("'", Uri.HexEscape('\''));
				} else if ((System.Web.HttpContext.Current.Request.Browser.Browser == "Safari")) {
					contentDisposition = strDisposition + "; filename=" + strFileName;
				} else {
					contentDisposition = strDisposition + "; filename*=UTF-8''" + Uri.EscapeDataString(strFileName);
				}
			} else {
				contentDisposition = strDisposition + "; filename*=UTF-8''" + Uri.EscapeDataString(strFileName);
			}

			return contentDisposition;
		}
		// GetContentDisposition


		// COR.ASP.NET.FindClientID("GridView1")
		public static string FindJQueryClientID(string strControlName)
		{
			return "#" + FindClientID(strControlName);
		}
		// FindJQueryClientID


		// COR.ASP.NET.FindClientID(Me, "GridView1")
		public static string FindJQueryClientID(Control ctrlParentControl, string strControlName)
		{
			return "#" + FindClientID(ctrlParentControl, strControlName);
		}
		// FindJQueryClientID


		// COR.ASP.NET.FindClientID(".NET ID")
		public static string FindClientID(string strControlName)
		{
			Page pgCallingPage = (Page)System.Web.HttpContext.Current.CurrentHandler;
			return FindClientID(pgCallingPage, strControlName);
		}
		// FindClientID


		// COR.ASP.NET.FindClientID(Me, ".NET ID")
		public static string FindClientID(Control ctrlParentControl, string strControlName)
		{
			Control ctrl = RecursiveFindControl(ctrlParentControl, strControlName);

			if (ctrl != null) {
				return ctrl.ClientID;
			}

			return strControlName;
		}
		// FindClientID


		// COR.ASP.NET.RecursiveFindControl(".NET ID")
		public static Control RecursiveFindControl(string strControlName)
		{
			Page pgCallingPage = (Page)System.Web.HttpContext.Current.CurrentHandler;
			return RecursiveFindControl(pgCallingPage, strControlName);
		}
		// RecursiveFindControl


		// COR.ASP.NET.RecursiveFindControl(Me, ".NET ID")
		public static Control RecursiveFindControl(Control ctrlParentControl, string strControlName)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(ctrlParentControl.ID, strControlName)) {
				return ctrlParentControl;
			}

			foreach (Control ctrlThisControl in ctrlParentControl.Controls) {
				if (StringComparer.OrdinalIgnoreCase.Equals(ctrlThisControl.ID, strControlName)) {
					return ctrlThisControl;
				} else {
					if (ctrlThisControl.HasControls()) {
						Control ctrlFoundControl = RecursiveFindControl(ctrlThisControl, strControlName);
						if (ctrlFoundControl != null) {
							return ctrlFoundControl;
						}
					}
				}
			}

			return null;
		}
		// RecursiveFindControl


        // http://codeverge.com/asp.net.getting-started/determining-a-user-s-internet-speed/709357
		// Dim dblConnectionSpeed = COR.ASP.NET.GetConnectionSpeed()
		public static double GetConnectionSpeed()
		{
			int iNumKB = 512;
			DateTime dtTime1 = default(DateTime);
			DateTime dtTime2 = default(DateTime);
			TimeSpan tsDuration = default(TimeSpan);
			int iLength = 0;
			string strCheckLen = "\\n";
			string strSpeedTest = "";

			//Response.Flush()
			System.Web.HttpContext.Current.Response.Flush();
			dtTime1 = DateTime.Now;
			iLength = strCheckLen.Length;

			int iIndex = 0;
			for (iIndex = 0; iIndex <= iNumKB - 1; iIndex += iIndex + 1) {
                strSpeedTest += "".PadRight(1024 - iLength, "/*\\*"[0]) + "\n"; 
				//Response.Flush()
				System.Web.HttpContext.Current.Response.Flush();
			}

			dtTime2 = DateTime.Now;
			tsDuration = (dtTime2 - dtTime1);

			double dDeltaT = iNumKB / tsDuration.TotalSeconds;
			double dSpeed = System.Math.Round(dDeltaT, 3);
			strSpeedTest = "";
			return dSpeed;
		}
		// GetConnectionSpeed


		// http://support.microsoft.com/kb/812406
		// COR.ASP.NET.SafeDownloadFile(strFileName, strClientFileName)
		public static void SafeDownloadFile(string strFilePath, string strClientFileName)
		{
			byte[] buffer = new byte[10001];
			// Buffer to read 10K bytes in chunk:
			int length = 0;
			// Length of the file:
			long dataToRead = 0;
			// Total bytes to read

			try {
				System.IO.FileInfo fiDownloadFile = new System.IO.FileInfo(strFilePath);
				if (fiDownloadFile.Exists) {
					System.Web.HttpContext.Current.Response.Clear();
					// Open the file.
                    using (System.IO.FileStream iStream = new System.IO.FileStream(strFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                    {

						// Total bytes to read:
						dataToRead = iStream.Length;

						System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strClientFileName));
						System.Web.HttpContext.Current.Response.AddHeader("Pragma", "public");
						System.Web.HttpContext.Current.Response.AddHeader("Expires", "0");
						System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
						System.Web.HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
						System.Web.HttpContext.Current.Response.AddHeader("Content-Length", fiDownloadFile.Length.ToString());
						//Response.ContentType = "application/pdf"
						System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";

						// Read the bytes.
						while (dataToRead > 0) {
							// Verify that the client is connected.
							if (System.Web.HttpContext.Current.Response.IsClientConnected) {
								// Read the data in buffer
								length = iStream.Read(buffer, 0, 10000);

								// Write the data to the current output stream.
								System.Web.HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);

								// Flush the data to the HTML output.
								System.Web.HttpContext.Current.Response.Flush();

								buffer = new byte[10001];
								// Clear the buffer
								dataToRead = dataToRead - length;
							} else {
								//prevent infinite loop if user disconnects
								dataToRead = -1;
							}
						}

						if (iStream != null) {
							// Close the file.
							iStream.Close();
						}

					}
				} else {
					//COR.Debug.Output.MsgBox("Diese Datei ist auf dem Datenträger nicht vorhanden.")
				}

			} catch (Exception ex) {
				// Trap the error, if any.
				System.Web.HttpContext.Current.Response.Write("Error : " + ex.Message);
			} finally {
				System.Web.HttpContext.Current.Response.Close();
				//System.Web.HttpContext.Current.Response.End()
			}

		}
		// SafeDownloadFile


		//COR.ASP.NET.AddApplicationUser(tempUser.User)
		//Public Shared Sub AddApplicationUser(ByVal strUser As String)
		//    Try
		//        System.Web.HttpContext.Current.Application.Lock()

		//        Try
		//            If Not DirectCast(System.Web.HttpContext.Current.Application["UserList"], List(Of String)).Contains(strUser) Then
		//                DirectCast(System.Web.HttpContext.Current.Application["UserList"], List(Of String)).Add(strUser)
		//            Else
		//                System.Diagnostics.Debug.WriteLine("'Application(""ActiveUsers"") = CInt(Application(""ActiveUsers"")) - 1")
		//            End If
		//        Catch ex As Exception
		//            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
		//        End Try

		//        System.Web.HttpContext.Current.Application.UnLock()
		//    Catch ex As Exception
		//        Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
		//    End Try
		//End Sub ' AddApplicationUser


		//Public Shared Function IsNotAnApertureSession() As Boolean
		//    If Not SafeSession.Exists("enviroment") Then
		//        Return True
		//    End If

		//    Return Not SafeSession.GetValue(Of String)("enviroment") = "aperture"
		//End Function ' IsNotAnApertureSession



		//Public Shared Function LocalizedContentUrl(ByVal strPath As String) As String
		//    If String.IsNullOrEmpty(strPath) Then
		//        Return strPath
		//    End If

		//    Dim strLocale As String = "DE"

		//    Try
		//        strLocale = SafeSession.GetValue(Of corBenutzer)("DMSUSER").Sprache
		//    Catch ex As Exception
		//        ' Session expired
		//    End Try


		//    strPath = System.Text.RegularExpressions.Regex.Replace(strPath, "locale-en", "locale-" + strLocale, RegexOptions.IgnoreCase)

		//    Return ContentUrl(strPath)
		//End Function


		public static string BaseUrl()
		{
			System.Web.HttpContext HttpContext = System.Web.HttpContext.Current;

			if (HttpContext != null) {
				string strURL = HttpContext.Request.Url.Scheme + Uri.SchemeDelimiter + HttpContext.Request.Url.Authority;
				strURL += System.Web.VirtualPathUtility.ToAbsolute("~/");
				return strURL;
			}

			return "";
		}
		// BaseUrl


		public static string ContentUrl(string strPath)
		{
			return ContentUrl(strPath, false);
		}


		public static string ContentUrl(string strPath, bool bNoCheck)
		{
			long lngFileTime = COR.AJAX.Time.ToUnixTicksMapped(strPath, bNoCheck);

			string strReturnValue = null;
			if (lngFileTime == 0) {
				return System.Web.VirtualPathUtility.ToAbsolute(strPath);
			} else {
				strReturnValue = System.Web.VirtualPathUtility.ToAbsolute(strPath);
			}

			if (!bNoCheck) {
				strReturnValue += "?no_cache_LastWriteTimeUtc=" + lngFileTime.ToString();
			}

			return strReturnValue;
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
		}
		// ContentUrl


		public static string ToCanonicalUrl(string relativeUrl)
		{
			string strCanonicalBase = System.Web.HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + System.Web.HttpContext.Current.Request.Url.Authority;
			string strAbsPath = System.Web.VirtualPathUtility.ToAbsolute(relativeUrl);
			string strCanonical = strCanonicalBase + strAbsPath;

			return strCanonical;
		}
		// ToCanonicalUrl


	} // COR.ASP.NET


}
