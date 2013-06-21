using System;

namespace SuperKoikoukesse.Webservice.ViewModels
{
    public class ImportResultModel
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public Exception Exception { get; set; }
    }
}