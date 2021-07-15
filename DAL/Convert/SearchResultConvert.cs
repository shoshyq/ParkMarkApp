using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Convert
{
    public static class SearchResultConvert
    {
        public static DAL.SearchResult ConvertSResultToEF(Entities.SearchResult sr)
        {
            return new DAL.SearchResult
            {
                Code = sr.Code,
                Usercode = sr.Usercode,
                SearchCode = sr.SearchCode,
                ResultPSCode = sr.ResultPSCode

            };
        }
        public static Entities.SearchResult ConvertSearchResultToEntity(DAL.SearchResult sr)
        {
            return new Entities.SearchResult
            {
                Code = sr.Code,
                Usercode = sr.Usercode,
                SearchCode = sr.SearchCode,
                ResultPSCode = sr.ResultPSCode

            };
        }

        public static List<Entities.SearchResult> ConvertSearchResultListToEntity(IEnumerable<DAL.SearchResult> sresults)
        {
            return sresults.Select(p => ConvertSearchResultToEntity(p)).ToList();
        }

    }
}
