namespace Interview.Entity.ResponseHandler
{
    public class ResponseModel<T> where T : class
    {
        public string Message { get; set; } = default!;
        public Guid? LatestId { get; set; }
        public int TotalCount { get; set; } = 0;
        public T? Data { get; set; }
    }
}