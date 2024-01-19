using System.Text;
using System.Text.RegularExpressions;

namespace WebServer;

public class MultipartParser(string data, HttpRequest req)
{
    private readonly string data = data;
    private readonly HttpRequest req = req;

    public static bool IsBoundaryStart(string data, string boundary)
    {
        return data.Equals("--" + boundary);
    }

    public static bool IsBoundaryEnd(string data, string boundary)
    {
        boundary = boundary.Replace("\r", string.Empty);
        data = data.Replace("\r", string.Empty);
        
        var other = "--" + boundary + "--";
        var ret = data.Equals(other);
        
        return ret;
    }

    public static bool IsBoundary(string data, string boundary)
    {
        return data.Equals("--" + boundary);
    }

    public string ParseContentDisposition(string mpContentDisposition)
    {
        var wordRegex = new Regex("; name=\"(\\w+)\"", RegexOptions.ECMAScript);

        var matches = wordRegex.Matches(mpContentDisposition);

        if (matches.Count > 0)
            return matches[0].Groups[1].Value;
        return "";
    }

    public FormData ParsePart(ref List<string>.Enumerator dataIter, string boundary)
    {
        var mpContentDisposition = dataIter.Current;
        var partName = ParseContentDisposition(mpContentDisposition);

        if (mpContentDisposition.Contains("filename="))
        {
            dataIter.MoveNext();
        }

        dataIter.MoveNext();
        var mpDataBytes = Array.Empty<byte>();
        while (!IsBoundary(dataIter.Current, boundary) && !IsBoundaryEnd(dataIter.Current, boundary))
        {
            var mpCurStr = dataIter.Current;

            // Convert each character to byte and concatenate to mpDataBytes
            mpDataBytes = mpDataBytes.Concat(Encoding.ASCII.GetBytes(mpCurStr)).ToArray();

            // Add back the newline characters
            mpDataBytes = mpDataBytes.Concat(Encoding.ASCII.GetBytes("\n")).ToArray();

            dataIter.MoveNext();
        }

        // Remove the last newline characters
        mpDataBytes = mpDataBytes.Skip(2).ToArray();
        mpDataBytes = mpDataBytes.Take(mpDataBytes.Length - 2).ToArray();

        var part = new FormData(partName, "file.png", mpDataBytes);
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

        // if (!dataIter.MoveNext()) throw new InvalidOperationException("Content-Type: multipart/form-data not found.");

        var contentType = dataIter.Current;
        var boundary = contentType[(contentType.IndexOf('=') + 1)..];

        while (dataIter.MoveNext() && !IsBoundaryStart(dataIter.Current, boundary))
        {
            // Move to the next line
        }

        // dataIter.MoveNext();
        while (!IsBoundaryEnd(dataIter.Current, boundary) && dataIter.MoveNext())
        {
            var part = ParsePart(ref dataIter, boundary);
            req.Parts.Add(part);
        }
    }
}