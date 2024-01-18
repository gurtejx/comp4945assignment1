namespace ConsoleClient;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ImageUploader : ConsoleClient
{
    public ImageUploader() : base() { }
    
    public new ImageUploader Port(int portNum)
    {
        base.Port(portNum);
        return this;
    }

    public new ImageUploader Server(string serverName)
    {
        base.Server(serverName);
        return this;
    }

    public new ImageUploader Connect()
    {
        base.Connect();
        return this;
    }
    private byte[] GetMultipartFormData(string filePath, string boundary)
    {
        var encoding = Encoding.UTF8;
        using (var stream = new MemoryStream())
        {
            // Add file content
            stream.Write(encoding.GetBytes($"--{boundary}\r\n"));
            stream.Write(encoding.GetBytes(
                $"Content-Disposition: form-data; name=\"file\"; filename=\"{Path.GetFileName(filePath)}\"\r\n"));
            stream.Write(encoding.GetBytes("Content-Type: application/octet-stream\r\n\r\n"));
            stream.Write(File.ReadAllBytes(filePath));
            stream.Write(encoding.GetBytes($"\r\n--{boundary}--\r\n"));

            return stream.ToArray();
        }
    }

    public void Upload(string filePath)
    {
        string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        byte[] formData = GetMultipartFormData(filePath, boundary);
        
        string headers = $"POST / HTTP/1.1\r\n" +
                         $"Host: {_server}\r\n" +
                         $"Content-Type: multipart/form-data; boundary={boundary}\r\n" +
                         $"Content-Length: {formData.Length}\r\n" +
                         $"Connection: close\r\n\r\n";
        Console.WriteLine($"ImageUploader: Headers = \n{headers}");
        
        _socket.Send(Encoding.UTF8.GetBytes(headers));
        _socket.Send(formData);
        Console.WriteLine("ImageUploader: Upload was successful.");
    }
}