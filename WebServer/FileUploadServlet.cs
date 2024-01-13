namespace WebServer;

public class FileUploadServlet : Servlet
{
    public void doGet(HttpRequest req, HttpResponse res)
    {
        string htmlFile = @"
            <form action=""/"" method=""post"" enctype=""multipart/form-data"">
                <input type=""file"" name=""targetImage""><br>
                <input type=""text"" name=""caption""><br>
                <input type=""text"" name=""date"" placeholder=""YYYY-MM-DD""><br>
                <input type=""submit"">
            </form>";
        res.Write(htmlFile);
    }

    public void doPost(HttpRequest req, HttpResponse res)
    {
        
    }
}