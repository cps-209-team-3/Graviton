using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class NetworkedGame
    {
        internal string UserName { get; set; }

        public event EventHandler<NetworkedCameraFrame> GameUpdatedEvent;

        public NetworkedGame(){}

        public void UpdateFrame(NetworkedCameraFrame ncf)
        {
            GameUpdatedEvent(this, ncf);
        }

        internal void KeyPressed(char v)
        {
            UDPGameClient.SendKeyPress(v);
        }

        internal void KeyReleased(char v)
        {
            UDPGameClient.SendKeyRelease(v);
        }
    }
}
