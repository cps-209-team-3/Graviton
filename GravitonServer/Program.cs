using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


//https://stackoverflow.com/questions/19786668/c-sharp-udp-socket-client-and-server/19787486#19787486
namespace GravitonServer
{
    
    class Program
    {
        static void Main(string[] args)
        {


            //create a new server
            var server = new UdpListener();

            //start listening for messages and copy the messages back to the client
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await server.Receive();
                    server.Reply("copy " + received.Message, received.Sender);
                    if (received.Message == "quit")
                        break;
                }
            });

        }
            
    } 
}
