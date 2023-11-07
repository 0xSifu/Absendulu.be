namespace AbsenDulu.BE.Response;
public class ResponseMessage<T>
{
    public bool IsError { get; set; } = false;
    public string? Message { get; set; }
    public T? Data { get; set; }
}