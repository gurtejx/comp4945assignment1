namespace WebServer;

public class HttpRequest
{
    public enum Method
    {
        GET,
        POST
    }

    public Method method;

    public List<FormData> Parts = [];
    public string url;
}