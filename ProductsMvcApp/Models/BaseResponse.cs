using System.Net;

namespace ProductsMvcApp.Models
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public T Data { get; set; }
    }

    public interface IBaseResponse<T>
    {
        string Description { get; }

        HttpStatusCode StatusCode { get; }

        public T Data { get; }
    }
}
