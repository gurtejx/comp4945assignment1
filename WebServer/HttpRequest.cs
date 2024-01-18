namespace WebServer;

public class HttpRequest
{
    public enum Method
    {
        GET,
        POST
    }

    public List<FormData> parts;
    public string url;
    public Method method;
}