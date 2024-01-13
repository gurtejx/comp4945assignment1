using System;
using System.Collections.Generic;

public class FormData
{
    public string Date { get; set; }
    public string Caption { get; set; }
    public byte[] ImageData { get; set; } 

    public FormData(string date, string caption, byte[] imageData)
    {
        Date = date;
        Caption = caption;
        ImageData = imageData;
    }

}