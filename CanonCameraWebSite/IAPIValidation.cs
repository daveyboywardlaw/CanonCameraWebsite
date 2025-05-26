using RestWrapper;

namespace CanonCameraWebSite
{
    interface IAPIValidation
    {
        public string ProcessErrorReponse(RestResponse response);
    }
}
