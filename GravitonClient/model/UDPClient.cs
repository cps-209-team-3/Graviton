using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace GravitonClient
{



    internal static class UDPGameClient
    {
        private static string gameServerName = "127.0.0.1";
        private static int gameServerPort = 8000;

        private static UdpClient gameConn;

        private static bool IsDoneWithCurrentGame;
        private static GameReporter reporter;

        private static NetworkedGame currentGame;

        internal static NetworkedGame JoinGame(string userName, double CameraWidth, double CameraHeight)
        {
            IsDoneWithCurrentGame = false;
            gameConn = new UdpClient();
            
            gameConn.Connect(gameServerName, gameServerPort);
            byte[] nameBytes = Encoding.ASCII.GetBytes($"{userName}|{CameraWidth}|{CameraHeight}");
            gameConn.Send(nameBytes, nameBytes.Length);
            IPEndPoint gameEndPoint = new IPEndPoint(IPAddress.Any, 0);
            currentGame = new NetworkedGame(userName);
            Task.Run(() =>
            {
                while (!IsDoneWithCurrentGame)
                {
                    DoAction(gameConn.Receive(ref gameEndPoint));
                }
            });

            return currentGame;
        }

        internal static void SetCurrentGameReporter(GameReporter gr)
        {
            reporter = gr;
        }



        internal static void SendKeyPress(char key)
        {
            gameConn.Send(new byte[] { (byte)key, 1}, 2);
        }

        internal static void SendKeyRealease(char key)
        {
            gameConn.Send(new byte[] { (byte)key, 0}, 2);
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
                    reporter.DisplayStart();
                    break;
                case 3:
                    string str = Encoding.ASCII.GetString(data, 1, data.Length - 1);
                    reporter.DisplayStats(GameStats.Deserialize(str));
                    IsDoneWithCurrentGame = true;
                    break;
                case 4:
                    reporter.DisplaySecondsTillStart((int) data[1]);
                    break;
            }
        }

        
    }
}
