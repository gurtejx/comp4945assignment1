using System.Net.Sockets;
using System.Text;

namespace WebServer;

public class HttpResponse
{
    private Socket socket;
    public HttpResponse(Socket socket)
    {
        this.socket = socket;
    }
    public void Write(string data)
    {
        string response = "HTTP/1.1 200 OK\r\n" +
                          "Content-Type: text/html\r\n" +
                          $"Content-Length: {Encoding.UTF8.GetByteCount(data)}\r\n" +
                          "\r\n" +
                          $"{data}";
        // Convert the response string to bytes
        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
        // Send the response bytes through the socket
        socket.Send(responseBytes, SocketFlags.None);
    }
}
