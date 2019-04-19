using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class NetworkedGame
    {
        private bool started;
        private string userName;

        public event EventHandler<SoundEffect> GameInvokeSoundEvent;
        public event EventHandler<NetworkedCameraFrame> GameUpdatedEvent;

        public bool IsOver
        {
            get;
            set;
        }
        

        public NetworkedGame(string userName)
        {
            this.userName = userName;
        }

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
