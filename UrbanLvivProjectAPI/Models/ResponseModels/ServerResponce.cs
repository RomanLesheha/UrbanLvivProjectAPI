namespace UrbanLvivProjectAPI.Models
{
    public class ServerResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        
        public ServerResponse()
        {
            statusCode = 0;
            message = string.Empty;
            data = null;
        }

        public ServerResponse(string message, int statusCode)
        {
            this.statusCode = statusCode;
            this.message = message;
            this.data = null;
        }

        public ServerResponse(string message, int statusCode, object data)
        {
            this.statusCode = statusCode;
            this.message = message;
            this.data = data;
        }
    }
}