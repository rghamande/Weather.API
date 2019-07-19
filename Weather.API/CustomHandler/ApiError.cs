using System;

namespace Weather.API.CustomHandler
{
    public class ApiError
    {
        public string Message { get; internal set; }
        public string RequestUri { get; internal set; }
        public string RequestMethod { get; internal set; }
        public DateTime TimeUtc { get; internal set; }
    }
}