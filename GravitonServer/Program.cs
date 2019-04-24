using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



namespace GravitonServer
{
    
    class Program
    {
        private static Dictionary<IPEndPoint, Client> clients = new Dictionary<IPEndPoint, Client>();
        private static UdpListener server;
        static void Main(string[] args)
        {


            //create a new server
            server = new UdpListener();
            Logger.Log("Started Server.");
            //start listening for messages and handle them.
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await server.Receive();
                    lock (clients)
                    {
                        if (!clients.ContainsKey(received.Sender))
                        {
                            Logger.Log("new Client: " + received.Sender);
                            var c = new Client(received.Sender);
                            c.ReplyEvent += PostUdpMessage;
                            clients.Add(received.Sender, c);
                        }
                        Task.Run(() => HandleClient(clients.GetValueOrDefault(received.Sender), received.Message));
                    }
                }
            });
            GameManager.Start();
            Console.ReadLine();
        }
        static void HandleClient(Client client, string message)
        {
            lock (client)
            {
                Logger.Log("recieved message from " + client.ClientIP + ": " + message);
                client.HandleMessage(message);
            }
        } 

        static void PostUdpMessage(object sender, string message)
        {
            try
            {
                Client client = sender as Client;
                //Logger.Log("sending message to " + client.ClientIP + ": " + message);
                Task.Run(()=>server.Reply(message, client.ClientIP));
            }
            catch(Exception e)
            {
                Client client = sender as Client;
                Logger.Log($"Error with client {client.ClientIP.ToString()}: {e.Message}. Removing.");
                lock (clients)
                    clients.Remove(client.ClientIP);
                
            }
        }
    } 
}
