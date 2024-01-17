using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class ImageUploader
{
    static void Main(string[] args)
    {
        string server = "localhost";
        int port = 8000;
        string filePath = "path_to_your_image.jpg"; // Replace with your image path

        try
        {
            UploadFile(server, port, filePath);
            Console.WriteLine("Image uploaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static Socket ConnectSocket(string server, int port)
    {
        Socket s = null;
        IPHostEntry hostEntry = Dns.GetHostEntry(server);

        foreach (IPAddress address in hostEntry.AddressList)
        {
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
            {
                s = tempSocket;
                break;
            }
        }
        return s;
    }

    static byte[] GetMultipartFormData(string filePath, string boundary)
    {
        var encoding = Encoding.UTF8;
        using (var stream = new MemoryStream())
        {
            // Add file content
            stream.Write(encoding.GetBytes($"--{boundary}\r\n"));
            stream.Write(encoding.GetBytes($"Content-Disposition: form-data; name=\"file\"; filename=\"{Path.GetFileName(filePath)}\"\r\n"));
            stream.Write(encoding.GetBytes("Content-Type: application/octet-stream\r\n\r\n"));
            stream.Write(File.ReadAllBytes(filePath));
            stream.Write(encoding.GetBytes($"\r\n--{boundary}--\r\n"));

            return stream.ToArray();
        }
    }

    static void UploadFile(string server, int port, string filePath)
    {
        string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        byte[] formData = GetMultipartFormData(filePath, boundary);
        Socket socket = ConnectSocket(server, port);

        if (socket == null)
            throw new Exception("Failed to connect to server.");

        string headers = $"POST / HTTP/1.1\r\n" +
                         $"Host: {server}\r\n" +
                         $"Content-Type: multipart/form-data; boundary={boundary}\r\n" +
                         $"Content-Length: {formData.Length}\r\n" +
                         $"Connection: close\r\n\r\n";

        socket.Send(Encoding.UTF8.GetBytes(headers));
        socket.Send(formData);

        // Optionally receive response here

        socket.Close();
    }
}
