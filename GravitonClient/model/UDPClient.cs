//This file contains the UDPClient class which deals with a connection to a server
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace GravitonClient
{


    //This class deals wih the logic to connect with a server.
    internal static class UDPGameClient
    {
        //This is a localhost connection
        private static string gameServerName = "127.0.0.1";
        // This is the port number
        private static int gameServerPort = 8020;
        //This is the IP endpoint which represents the IP address and port number
        private static IPEndPoint gameEndPoint;

        //This is a reference to a UdpClient object that deals with a connection
        private static UdpClient gameConn;

        //This bool represents whether the client is listening
        private static bool IsListening;
        //This is a reference to a GameReporter object
        private static GameReporter reporter;

        //This is a reference to the current networked game
        private static NetworkedGame currentGame;

        //This method joins a game and returns the corresponding networked game
        internal static NetworkedGame JoinGame(string userName, double CameraWidth, double CameraHeight)
        {            
            IsListening = false;
            gameConn = new UdpClient();

            gameConn.Connect(gameServerName, gameServerPort);
            byte[] nameBytes = Encoding.ASCII.GetBytes($"{userName}|{CameraWidth}|{CameraHeight}");
            gameConn.Send(nameBytes, nameBytes.Length);
            gameEndPoint = new IPEndPoint(IPAddress.Any, 0);
            currentGame = new NetworkedGame();
            currentGame.UserName = userName;

            return currentGame;
           
        }

        //This method sets the GameReporter
        internal static void SetCurrentGameReporter(GameReporter gr)
        {
            reporter = gr;
        }

        //This method starts the listening process
        internal static void StartListening()
        {
            IsListening = true;
            Task.Run(() =>
            {
                while (IsListening)
                {
                    try
                    {
                        DoAction(gameConn.Receive(ref gameEndPoint));
                    }
                    catch (Exception e)
                    {
                       reporter.DisplayError(e.Message);
                    }
                }
            });
        }

        //This method sends a keypress to the server
        internal static void SendKeyPress(char key)
        {
            gameConn.Send(new byte[] { 0, (byte)key}, 2);
        }

        //This method sends a keyrelease to the server
        internal static void SendKeyRelease(char key)
        {
            gameConn.Send(new byte[] { 1, (byte)key}, 2);
        }

        //This method implements a certain action to communicate
        internal static void DoAction(byte[] data)
        {
            switch (data[0])
            {
                case 0:
                    string s = Encoding.ASCII.GetString(data, 1, data.Length - 1);
                    NetworkedCameraFrame cameraFrame = NetworkedCameraFrame.Deserialize(s);
                    currentGame.UpdateFrame(cameraFrame);
                    break;
                case 1:
                    reporter.GameOver();
                    break;
                case 2:
                    string str = Encoding.ASCII.GetString(data, 1, data.Length - 1);
                    reporter.DisplayStats(GameStats.Deserialize(str));
                    IsListening = true;
                    break;
                case 3:
                        reporter.DisplaySecondsTillStart(
                            Convert.ToInt32(
                                Encoding.ASCII.GetString(data, 1, data.Length - 1)));
                    break;
            }
        }

        //This method stops listening to the server
        internal static void StopListening()
        {
            IsListening = false;
        }


    }
}
