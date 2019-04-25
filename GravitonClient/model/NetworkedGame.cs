//This file contains the NetworkedGame class which contains information for a networked game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    //This class deals with the logic for a networked game to communicate with UDPClient
    public class NetworkedGame
    {
        //This is the username of the player
        internal string UserName { get; set; }

        public event EventHandler<NetworkedCameraFrame> GameUpdatedEvent;

        public NetworkedGame(){}

        //This method emits an event representing a frame update
        public void UpdateFrame(NetworkedCameraFrame ncf)
        {
            GameUpdatedEvent(this, ncf);
        }

        //This method sends the info for a keypress
        internal void KeyPressed(char v)
        {
            UDPGameClient.SendKeyPress(v);
        }

        //This method sends the info for a key release
        internal void KeyReleased(char v)
        {
            UDPGameClient.SendKeyRelease(v);
        }
    }
}
