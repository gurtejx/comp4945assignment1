namespace ConsoleClient;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ConsoleClient
{ 
    protected int? _port;
    protected string? _server;
    protected Socket _socket;
    
    public ConsoleClient()
    {
        _port = null;
        _server = null;
        _socket = null;
        Console.WriteLine("ConsoleClient: Object instantiated.");
    }

    protected ConsoleClient Port(int portNum)
    {
        _port = portNum;
        return this;
    }

    protected ConsoleClient Server(string serverName)
    {
        _server = serverName;
        return this;
    }

    public virtual ConsoleClient Connect()
    {
        Console.WriteLine($"ConsoleClient: Connecting to Server \'{_server}\', Port {_port}.");
        try
        {
            
            IPHostEntry hostEntry = Dns.GetHostEntry(_server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, _port.Value);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    _socket = tempSocket;
                    break;
                }
            }
            Console.WriteLine($"ConsoleClient: Connection was successful to  Server \'{_server}\', Port {_port}.");
            return this;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool Close()
    {
        try
        {
            _socket.Close();
            Console.WriteLine("ConsoleClient: Connection was successfully closed.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public void Debug()
    {
        Console.WriteLine($"ConsoleClient: Debug _ Port={_port}, Server={_server}");
    }
}
