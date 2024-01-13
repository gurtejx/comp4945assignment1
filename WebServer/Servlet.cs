namespace WebServer;

public interface IServlet
{
    public void DoGet(HttpRequest req, HttpResponse res);
    public void DoPost(HttpRequest req, HttpResponse res);
}