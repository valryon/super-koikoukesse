
namespace SuperKoikoukesse.Webservice.Service
{
    /// <summary>
    /// Available error codes for webservice
    /// </summary>
    public enum ErrorCodeEnum : int
    {
        Ok  = 0,
        EmptyRequest = 100,
        InvalidRequest = 101,
        ServiceError = 500,
    }
}