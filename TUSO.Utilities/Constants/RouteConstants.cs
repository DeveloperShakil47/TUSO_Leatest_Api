/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
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