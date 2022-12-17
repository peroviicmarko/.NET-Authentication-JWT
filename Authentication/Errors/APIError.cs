namespace Authentication.Errors
{
    public class APIError: Exception
    {
        public int StatusCode = StatusCodes.Status400BadRequest;

        public APIError(string message = "", int statusCode = StatusCodes.Status400BadRequest) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
