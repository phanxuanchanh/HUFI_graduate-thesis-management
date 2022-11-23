
namespace GraduateThesis.Models
{
    public class DataResponse
    {
        public DataResponseStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class DataResponse<TModel> : DataResponse
    {
        public TModel Data { get; set; }
    }
}
