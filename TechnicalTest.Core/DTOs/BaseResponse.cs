namespace TechnicalTest.Core.DTOs
{

    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public T? Content { get; set; }
    }
}
