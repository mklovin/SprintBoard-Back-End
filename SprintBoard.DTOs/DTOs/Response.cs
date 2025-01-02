namespace SprintBoard.DTOs
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static Response<T> Success(T data, string message = "Operation succeeded.")
        {
            return new Response<T> { IsSuccess = true, Message = message, Data = data };
        }

        public static Response<T> Failure(string message)
        {
            return new Response<T> { IsSuccess = false, Message = message, Data = default };
        }
    }
}
