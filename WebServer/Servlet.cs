namespace WebServer;

public interface Servlet
{
    public void doGet(HttpRequest req, HttpResponse res);
    public void doPost(HttpRequest req, HttpResponse res);
}