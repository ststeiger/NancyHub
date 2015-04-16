
namespace COR.AJAX
{


    public class cAjaxResult
    {
        public object data;

        public COR.AJAX.AJAXException error = null;

        public bool hasError
        {
            get
            {
                if (error != null)
                {
                    return true;
                }
                return false;
            }
        }


    } // cAjaxResult


} // COR.AJAX
