namespace Library_Managment.Application.Common
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static Result Ok(string message = "") => new()
        {
            Success = true,
            Message = message
        };

        public static Result Fail(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
}
