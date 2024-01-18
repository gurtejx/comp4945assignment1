using System.Text.RegularExpressions;

namespace WebServer;

public class MultipartParser(string data, HttpRequest req)
{
    private readonly string data = data;
    private readonly HttpRequest req = req;
    
    public static bool IsBoundaryStart(string data, string boundary)
    {
        return data == "--" + boundary;
    }

    public static bool IsBoundaryEnd(string data, string boundary)
    {
        return data == "--" + boundary + "--";
    }

    public static bool IsBoundary(string data, string boundary)
    {
        return data == "--" + boundary;
    }
    
    public string ParseContentDisposition(string mpContentDisposition)
    {
        Regex wordRegex = new Regex("; name=\"(\\w+)\"", RegexOptions.ECMAScript);

        MatchCollection matches = wordRegex.Matches(mpContentDisposition);

        if (matches.Count > 0)
        {
            return matches[0].Groups[1].Value;
        }
        else
        {
            return "";
        }
    }
    
    public FormData ParsePart(ref List<string>.Enumerator dataIter, string boundary)
    {
        dataIter.MoveNext();

        var mpContentDisposition = dataIter.Current;
        var partName = ParseContentDisposition(mpContentDisposition);

        bool hasFile = false;
        string mpContentType = "";

        if (mpContentDisposition.Contains("filename="))
        {
            hasFile = true;
            dataIter.MoveNext();
            mpContentType = dataIter.Current;
        }

        byte[] mpDataBytes = new byte[0];
        while (!IsBoundary(dataIter.Current, boundary) && !IsBoundaryEnd(dataIter.Current, boundary))
        {
            var mpCurStr = dataIter.Current;

            // Convert each character to byte and concatenate to mpDataBytes
            mpDataBytes = mpDataBytes.Concat(System.Text.Encoding.UTF8.GetBytes(mpCurStr)).ToArray();

            // Add back the newline characters
            mpDataBytes = mpDataBytes.Concat(new byte[] { 13, 10 }).ToArray();

            dataIter.MoveNext();
        }

        // Remove the last newline characters
        mpDataBytes = mpDataBytes.Take(mpDataBytes.Length - 2).ToArray();

        FormData part = new FormData(partName, "file.png", mpDataBytes);
        return part;
    }
    
    public void ParseMultipartData()
    {
        var dataLines = data.Split('\n').ToList();
        var dataIter = dataLines.GetEnumerator();

        while (dataIter.MoveNext() && !dataIter.Current.StartsWith("Content-Type: multipart/form-data"))
        {
            // Move to the next line
        }

        if (!dataIter.MoveNext())
        {
            throw new InvalidOperationException("Content-Type: multipart/form-data not found.");
        }

        var contentType = dataIter.Current;
        var boundary = contentType.Substring(contentType.IndexOf("=") + 1);

        while (dataIter.MoveNext() && !IsBoundaryStart(dataIter.Current, boundary))
        {
            // Move to the next line
        }

        while (dataIter.MoveNext() && !IsBoundaryEnd(dataIter.Current, boundary))
        {
            var part = ParsePart(ref dataIter, boundary);
            req.parts.Add(part);
        }
    }

}