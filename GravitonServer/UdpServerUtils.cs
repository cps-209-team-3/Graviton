/*
* File: UdpServerUtils.cs
* Desc: Brutally ripped off code from 
* https://stackoverflow.com/questions/19786668/c-sharp-udp-socket-client-and-server/19787486#19787486
*/


using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

//Credit:
//https://stackoverflow.com/questions/19786668/c-sharp-udp-socket-client-and-server/19787486#19787486
namespace GravitonServer
{
    public struct Received
    {
        public IPEndPoint Sender;
        public string Message;
    }

    abstract class UdpBase
    {
        protected UdpClient Client;

        protected UdpBase()
        {
            Client = new UdpClient();
        }

        public async Task<Received> Receive()
        {
            var result = await Client.ReceiveAsync();
            return new Received()
            {
                Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                Sender = result.RemoteEndPoint
            };
        }
    }

    //Server
    class UdpListener : UdpBase
    {
        private static int port = 8020;
        private IPEndPoint _listenOn;

        public UdpListener() : this(new IPEndPoint(IPAddress.Loopback, port))
        {
        }

        public UdpListener(IPEndPoint endpoint)
        {
            _listenOn = endpoint;
            Client = new UdpClient(_listenOn);
        }

        public void Reply(string message, IPEndPoint endpoint)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length, endpoint);
        }

    }

    //Client
    class UdpUser : UdpBase
    {
        private UdpUser() { }

        public static UdpUser ConnectTo(string hostname, int port)
        {
            var connection = new UdpUser();
            connection.Client.Connect(hostname, port);
            return connection;
        }

        public void Send(string message)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length);
        }

    }
}
