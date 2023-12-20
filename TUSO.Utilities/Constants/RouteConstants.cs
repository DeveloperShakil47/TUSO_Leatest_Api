/*
* Created by: Labib
* Date created: 31.08.2022
* Last modified: 06.11.2022
* Modified by:Bithy
*/
namespace TUSO.Utilities.Constants
{
    public static class RouteConstants
    {
        public const string BaseRoute = "tuso-api";

        #region Country
        public const string CreateCountry = "country";

        public const string ReadCountries = "countries";

        public const string ReadCountriesbyPage = "countries-pagination";

        public const string ReadCountryByKey = "country/key/{key}";

        public const string UpdateCountry = "country/{key}";

        public const string DeleteCountry = "country/{key}";

        #endregion
    }
}