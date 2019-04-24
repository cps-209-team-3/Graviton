using System;
using System.Net;

namespace GravitonServer
{
    internal class Client
    {
        internal Ship MyPlayer;

        public IPEndPoint ClientIP
        {
            get; private set;
        }
        public Client(IPEndPoint ipEndPoint)
        {
            ClientIP = ipEndPoint;
            CurrentState = new NotAssignedGameState(this);
        }

        internal ClientState CurrentState{ get; set;}
        internal event EventHandler<string> ReplyEvent;
        internal void HandleMessage(string message)
        {
            CurrentState.HandleIncomingMessage(message);
        }
        internal void RaiseReply(string message)
        {
            ReplyEvent(this, message);
        }

        internal void HandleFromServer(object o)
        {
            CurrentState.UpdateFromServer(o);
        }
    }

    internal abstract class ClientState
    {
        public ClientState(Client client)
        {
            
            CurrentClient = client;
        }
        internal Client CurrentClient;
        
        internal abstract void HandleIncomingMessage(string message);
        internal abstract void UpdateFromServer(object o);
    }

    internal class NotAssignedGameState : ClientState
    {
        private string RequestedUsername;

        private double CameraWidth;
        private double CameraHeight;

        public NotAssignedGameState(Client client) : base(client){
            Logger.Log($"client {client.ClientIP} is in state: NotAssignedGameState");
        }

        internal override void HandleIncomingMessage(string message) {
            string[] parts = message.Split('|');
            RequestedUsername = parts[0];
            CameraWidth = Convert.ToDouble(parts[ parts.Length - 2]);
            CameraHeight = Convert.ToDouble(parts[parts.Length - 1]);

            GameManager.JoinGame(CurrentClient);
        }

        internal override void UpdateFromServer(object o) {
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

    internal class GameNotStartedState : ClientState
    {

        public GameNotStartedState(Client client) : base(client) {
            Logger.Log($"client {client.ClientIP} is in state: GameNotStartedState");
        }
        internal override void HandleIncomingMessage(string message) { }

        internal override void UpdateFromServer( object o)
        {
            if(o is int)
            {
                CurrentClient.RaiseReply('\x03' + ((int)o).ToString() );
            } else if (o is Game)
            {
                var igs = new InGameState(CurrentClient);
                igs.SetGame((Game)o);
                CurrentClient.CurrentState = igs;
            }
        }
    }

    internal class InGameState : ClientState
    {
        
        private Game CurrentGame;
        private void GameUpdated(object sender, int e)
        {
            if(!CurrentGame.IsOver)
                CurrentClient.RaiseReply('\0' +
                    CurrentClient.MyPlayer.ViewCamera.GetCameraFrame().Serialize());
            else
            {
                GameOver();
            }
        }

        private void GameOver(object sender, EventArgs e)
        {
            GameOver();
        }

        private void GameOver()
        {
            CurrentClient.RaiseReply("\x01");
            var gos = new GameOverState(CurrentClient);
            gos.SetGameStats(CurrentGame.GetStats());
            CurrentClient.CurrentState = gos;
        }

        public InGameState(Client client) : base(client) {
            Logger.Log($"client {client.ClientIP} is in state: InGameState");
        }
        internal override void HandleIncomingMessage(string message)
        {
            if (message[0] == '\0')
                CurrentClient.MyPlayer.KeyPressed(message[1]);
            else
                CurrentClient.MyPlayer.KeyReleased(message[1]);
        }
        internal override void UpdateFromServer(object o) { }

        internal void SetGame(Game g)
        {
            CurrentGame = g;
            g.GameUpdatedEvent += GameUpdated;
            CurrentClient.MyPlayer.PlayerDiedEvent += GameOver;
        }
    }

    internal class GameOverState : ClientState
    {
        private GameStats gameStats;
        internal void SetGameStats(GameStats gameStats)
        {
            this.gameStats = gameStats;
            CurrentClient.RaiseReply('\x02' + gameStats.Serialize());
        }

        public GameOverState(Client client) : base(client) {
            Logger.Log($"client {client.ClientIP} is in state: GameOverState");
        }
        internal override void HandleIncomingMessage(string message) { }
        internal override void UpdateFromServer(object o) {
            
        }
    }
}
