namespace WebServer;

public class HttpRequest
{
    public enum Method {
        GET,
        POST
    };

    public string url;
    public List<FormData> parts;
}