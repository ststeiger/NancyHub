
namespace COR.AJAX
{


    public class Time
    {


        public static System.DateTime FromUnixTicks(System.Int64 lngMilliseconds)
        {
            System.DateTime d1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            System.DateTime d2 = d1.AddMilliseconds(lngMilliseconds);

            System.DateTime dt = d2.ToLocalTime();
            return dt;
        } // FromUnixTicks


        public static System.Int64 ToUnixTicks()
        {
            return ToUnixTicks(System.DateTime.Now);
        } // ToUnixTicks


        public static System.Int64 ToUnixTicks(string strPathToFile)
        {
            System.DateTime dLastWriteTime = System.IO.File.GetLastWriteTimeUtc(strPathToFile);
            return ToUnixTicks(dLastWriteTime);
        } // ToUnixTicks


        public static System.Int64 ToUnixTicksMapped(string strPathToFile)
        {
            return ToUnixTicksMapped(strPathToFile, false);
        }


        public static System.Int64 ToUnixTicksMapped(string strPathToFile, bool bNoChek)
        {
            strPathToFile = System.Web.HttpContext.Current.Server.MapPath(strPathToFile);

            if (bNoChek)
            {
                return ToUnixTicks(strPathToFile);
            }

            if (System.IO.File.Exists(strPathToFile))
            {
                return ToUnixTicks(strPathToFile);
            }
            else
            {
                if (!(strPathToFile.EndsWith("\\DMS", System.StringComparison.OrdinalIgnoreCase) | strPathToFile.EndsWith("\\DMS\\bilder\\{0}", System.StringComparison.OrdinalIgnoreCase)))
                {
                    throw new System.IO.FileNotFoundException("Die Datei \"" + strPathToFile + "\" existiert nicht");
                }

            }

            return 0;
        } // ToUnixTicksMapped


        public static System.Int64 ToUnixTicks(System.DateTime dt)
        {
            System.DateTime d1 = new System.DateTime(1970, 1, 1);
            System.DateTime d2 = dt.ToUniversalTime();
            System.TimeSpan ts = new System.TimeSpan(d2.Ticks - d1.Ticks);
            return System.Convert.ToInt64(ts.TotalMilliseconds);
        } // ToUnixTicks


    } // Time


} // COR.AJAX
