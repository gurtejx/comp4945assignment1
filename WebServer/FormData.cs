namespace WebServer;

public class FormData
{
    private string Date;
    private string Caption;
    private byte[] ImageData;

    public FormData(string date, string caption, byte[] imageData)
    {
        Date = date;
        Caption = caption;
        ImageData = imageData;
    }
}