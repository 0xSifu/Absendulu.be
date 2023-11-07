namespace AbsenDulu.BE.Response;
public class Responses<T>
{
    public Responses()
    {
    }
    public Responses(T data)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }
    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; }
    public string Message { get; set; }
}