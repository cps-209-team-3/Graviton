using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Threading;

namespace GravitonClient
{



    internal static class UDPGameClient
    {
        private static string gameServerName = "127.0.0.1";
        private static int gameServerPort = 8020;
        private static IPEndPoint gameEndPoint;

        private static UdpClient gameConn;

        private static bool IsListening;
        private static GameReporter reporter;

        private static NetworkedGame currentGame;

        internal static NetworkedGame JoinGame(string userName, double CameraWidth, double CameraHeight)
        {            
            IsListening = false;
            gameConn = new UdpClient();

            gameConn.Connect(gameServerName, gameServerPort);
            byte[] nameBytes = Encoding.ASCII.GetBytes($"{userName}|{CameraWidth}|{CameraHeight}");
            gameConn.Send(nameBytes, nameBytes.Length);
            gameEndPoint = new IPEndPoint(IPAddress.Any, 0);
            currentGame = new NetworkedGame(userName);
            

            return currentGame;
           
        }

        internal static void SetCurrentGameReporter(GameReporter gr)
        {
            reporter = gr;
        }

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

        internal static void SendKeyPress(char key)
        {
            gameConn.Send(new byte[] { 0, (byte)key}, 2);
        }

        internal static void SendKeyRelease(char key)
        {
            gameConn.Send(new byte[] { 1, (byte)key}, 2);
        }

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

        
    }
}
