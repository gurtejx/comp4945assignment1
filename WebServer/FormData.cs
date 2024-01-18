namespace WebServer;

public class FormData(string date, string caption, byte[] imageData)
{
    public string caption = caption;
    public string date = date;
    public byte[] imageData = imageData;
}