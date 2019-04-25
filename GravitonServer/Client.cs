/*********************
 * File: Client.cs
 * Desc: Manages client information, including which states the client is in.
**********************/
using System;
using System.Net;

namespace GravitonServer
{
    //model class for a Graviton client
    internal class Client
    {
        //The client's ship object
        internal Ship MyPlayer;

        // The IP and port of the client. Used when sending messages.
        public IPEndPoint ClientIP
        {
            get; private set;
        }

        public Client(IPEndPoint ipEndPoint)
        {
            ClientIP = ipEndPoint;
            CurrentState = new NotAssignedGameState(this);
        }

        //The current state of the client
        internal ClientState CurrentState { get; set; }
        //Event raised when the server needs to reply to the client.
        internal event EventHandler<string> ReplyEvent;
        //Handles messages sent to the server.
        internal void HandleMessage(string message)
        {
            CurrentState.HandleIncomingMessage(message);
        }
        // Sends a message back to the client
        internal void RaiseReply(string message)
        {
            ReplyEvent(this, message);
        }
        //Used when something happens on the server that needs to be reflected to the client.
        internal void HandleFromServer(object o)
        {
            CurrentState.UpdateFromServer(o);
        }
    }

    //Class that represents the current client connection state
    internal abstract class ClientState
    {
        public ClientState(Client client)
        {
            CurrentClient = client;
        }
        internal Client CurrentClient;

        // Handles the incoming message in the curretn context.
        internal abstract void HandleIncomingMessage(string message);
        internal abstract void UpdateFromServer(object o);
    }

    //Class that represents a client connection that has not been assigned to an active game
    internal class NotAssignedGameState : ClientState
    {
        private string RequestedUsername;

        private double CameraWidth;
        private double CameraHeight;

        public NotAssignedGameState(Client client) : base(client)
        {
            Logger.Log($"client {client.ClientIP} is in state: NotAssignedGameState");
        }

        //handles incoming messages from the client
        internal override void HandleIncomingMessage(string message)
        {
            string[] parts = message.Split('|');
            RequestedUsername = parts[0];
            CameraWidth = Convert.ToDouble(parts[parts.Length - 2]);
            CameraHeight = Convert.ToDouble(parts[parts.Length - 1]);

            GameManager.JoinGame(CurrentClient);
        }

        //gets an update on the state of the game and UI from server
        internal override void UpdateFromServer(object o)
        {
            if (o is Ship)
            {

                CurrentClient.MyPlayer = o as Ship;
                CurrentClient.MyPlayer.Username = RequestedUsername;
                CurrentClient.MyPlayer.ViewCamera.Width = CameraWidth;
                CurrentClient.MyPlayer.ViewCamera.Height = CameraHeight;
                CurrentClient.CurrentState = new GameNotStartedState(CurrentClient);
            }
        }


    }

    //Class that represents a client connection state in countdown
    internal class GameNotStartedState : ClientState
    {

        public GameNotStartedState(Client client) : base(client)
        {
            Logger.Log($"client {client.ClientIP} is in state: GameNotStartedState");
        }

        //handles incoming messages from the client
        internal override void HandleIncomingMessage(string message) { }

        //gets an update on the state of the game and UI from server
        internal override void UpdateFromServer(object o)
        {
            if (o is int)
            {
                CurrentClient.RaiseReply('\x03' + ((int)o).ToString());
            }
            else if (o is Game)
            {
                var igs = new InGameState(CurrentClient);
                igs.SetGame((Game)o);
                CurrentClient.CurrentState = igs;
            }
        }
    }

    //Class that represents a client connection state in game
    internal class InGameState : ClientState
    {
        //reference to the current Game object
        private Game CurrentGame;

        //event handler for gameupdatedevent in Game.cs
        private void GameUpdated(object sender, int e)
        {
            if (!CurrentGame.IsOver)
                CurrentClient.RaiseReply('\0' +
                    CurrentClient.MyPlayer.ViewCamera.GetCameraFrame().Serialize());
            else
            {
                GameOver();
            }
        }

        //event handler for when the game is over
        private void GameOver(object sender, EventArgs e)
        {
            GameOver();
        }

        //takes care of client-server interactions when the game is over
        private void GameOver()
        {
            CurrentClient.RaiseReply("\x01");
            var gos = new GameOverState(CurrentClient);
            gos.SetGameStats(CurrentGame.GetStats());
            CurrentClient.CurrentState = gos;
        }

        public InGameState(Client client) : base(client)
        {
            Logger.Log($"client {client.ClientIP} is in state: InGameState");
        }

        //handles incoming messages from the client
        internal override void HandleIncomingMessage(string message)
        {
            if (message[0] == '\0')
                CurrentClient.MyPlayer.KeyPressed(message[1]);
            else
                CurrentClient.MyPlayer.KeyReleased(message[1]);
        }

        //gets an update on the state of the game and UI from server
        internal override void UpdateFromServer(object o) { }

        //sets the game up
        internal void SetGame(Game g)
        {
            CurrentGame = g;
            g.GameUpdatedEvent += GameUpdated;
            CurrentClient.MyPlayer.PlayerDiedEvent += GameOver;
        }
    }

    //Class that represents a client connection state in game
    internal class GameOverState : ClientState
    {
        //reference to GameStats object
        private GameStats gameStats;

        //makes changes to GameStats object
        internal void SetGameStats(GameStats gameStats)
        {
            this.gameStats = gameStats;
            CurrentClient.RaiseReply('\x02' + gameStats.Serialize());
        }

        public GameOverState(Client client) : base(client)
        {
            Logger.Log($"client {client.ClientIP} is in state: GameOverState");
        }

        //handles incoming messages from the client
        internal override void HandleIncomingMessage(string message) { }

        //gets an update on the state of the game and UI from server
        internal override void UpdateFromServer(object o)
        {

        }
    }
}
