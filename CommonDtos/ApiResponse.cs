namespace CommonDtos
{
    public class ApiResponse<T>
    {
        public int Code { get; set; } // 0：成功，非0：失败（可自定义）
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "成功")
        {
            return new ApiResponse<T> { Code = 0, Message = message, Data = data };
        }

        public static ApiResponse<T> Fail(string message, int code = 1)
        {
            return new ApiResponse<T> { Code = code, Message = message, Data = default };
        }
    }

    public class ApiResponse
    {
        public int Code { get; set; } // 0：成功，非0：失败（可自定义）
        public string Message { get; set; } = string.Empty;

        public static ApiResponse Success(string message = "成功")
        {
            return new ApiResponse { Code = 0, Message = message };
        }

        public static ApiResponse Fail(string message, int code = 1)
        {
            return new ApiResponse { Code = code, Message = message };
        }
    }

    public class ApiResult<T>
    {
        public T? Data { get; set; }
    }
}
