using System.Text;

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
        string docPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        var file = req.Parts.Find(data => data.name == "targetImage");
        var date = req.Parts.Find(data => data.name == "date");
        var caption = req.Parts.Find(data => data.name == "caption");

        var dateStr = Encoding.ASCII.GetString(date.byteData);
        var captionStr = Encoding.ASCII.GetString(caption.byteData);

        using var outputFile = new BinaryWriter(File.Open(Path.Combine(docPath, dateStr + captionStr.Trim()), FileMode.CreateNew));
        outputFile.Write(file.byteData);
    }
}