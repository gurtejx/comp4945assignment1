namespace WebServer;

public class FormData(string date, string caption, byte[] imageData)
{
    private string Caption = caption;
    private string Date = date;
    private byte[] ImageData = imageData;
}