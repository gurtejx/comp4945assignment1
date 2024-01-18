namespace WebServer;

public class FileUploadServlet : IServlet
{
    public void DoGet(HttpRequest req, HttpResponse res)
    {
        const string htmlFile = """
                                            <form action="/" method="post" enctype="multipart/form-data">
                                                <input type="file" name="targetImage"><br>
                                                <input type="text" name="caption"><br>
                                                <input type="text" name="date" placeholder="YYYY-MM-DD"><br>
                                                <input type="submit">
                                            </form>
                                """;
        res.Write(htmlFile);
    }

    public void DoPost(HttpRequest req, HttpResponse res)
    {
    }
}
