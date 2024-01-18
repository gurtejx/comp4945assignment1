namespace WebServer;

public class FormData(string name, string fileName, byte[] byteData)
{
    public byte[] byteData = byteData;
    public string fileName = fileName;
    public string name = name;
}