// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer;

void ThreadFunction(object? obj)
{
    var sock = (Socket)obj!;
    var bytesReceived = new byte[1024];
    var a = "";
    while (true)
    {
        var bytesRead = sock.Receive(bytesReceived, bytesReceived.Length, 0);
        if (bytesRead == 0) break;

        a += Encoding.ASCII.GetString(bytesReceived, 0, bytesRead);

        if (bytesReceived[bytesRead] == 0) break;
    }

    Console.WriteLine(a);
    var lines = a.Split();

    var req = new HttpRequest();
    var res = new HttpResponse(sock);
    var us = new FileUploadServlet();

    if (lines[0].StartsWith("GET"))
    {
        // GET 
        us.DoGet(req, res);
    }
    else if (lines[0].StartsWith("POST"))
    {
    }

    sock.Close();
}

const int port = 8000;
var ipAddress = IPAddress.Parse("127.0.0.1");
var ipEndpoint = new IPEndPoint(ipAddress, port);

var socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(ipEndpoint);
socket.Listen(10);

while (true)
{
    var connectionSock = socket.Accept();
    var thread = new Thread(ThreadFunction);
    thread.Start(connectionSock);
}