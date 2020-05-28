using ShineSpike.Utils;

namespace ShineSpike.Models
{
    public class FormUploadResponse
    {
        public int Success { get; private set; }
        public string Message { get; private set; }
        public string Url { get; private set; }

        public FormUploadResponse(string url)
        {
            Success = 1;
            Url = url;
            Message = Constants.SuccessfulUploadMessage;
        }

        public FormUploadResponse(bool success, string url)
        {
            Success = success ? 1 : 0;
            Url = url;
            Message = success ? Constants.SuccessfulUploadMessage : Constants.FailedUploadMessage;
        }

        public FormUploadResponse(bool success, string url, string message)
        {
            Success = success ? 1 : 0;
            Url = url;
            Message = message;
        }
    }
}
