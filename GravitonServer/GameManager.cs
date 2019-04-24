using System;
using System.Collections.Generic;
using System.Timers;
using System.Text;

namespace GravitonServer
{
    internal static class GameManager
    {
        private static readonly int SECONDS_BETWEEN_GAME_START = 20;
        private static int SecondCounter = 0;

        private static List<Game> OnGoingGames = new List<Game>();
        private static List<Client> WaitingClients = new List<Client>();
        private static Game UnstartedGame;
        private static Timer GameTimer = new Timer();
        
        internal static void Start()
        {
            GameTimer.Interval = 1000;
            GameTimer.Elapsed += Second_tick;
            GameTimer.AutoReset = true;
        }

        internal static void JoinGame(Client client)
        {
            Ship clientShip;
            if (UnstartedGame == null)
            {
                UnstartedGame = new Game();
                GameTimer.Start();
            }
            lock (UnstartedGame) {
                clientShip = UnstartedGame.AddPlayer();
            }
            lock (client)
            {
                if(client.CurrentState is NotAssignedGameState)
                {
                    WaitingClients.Add(client);
                    client.HandleFromServer(clientShip);
                }
            }
        }

        private static void Second_tick(object sender, object args)
        {
            SecondCounter++;
            if(SecondCounter % SECONDS_BETWEEN_GAME_START == 0)
            {
                SecondCounter = 0;
                StartGame();
                GameTimer.Stop();
                WaitingClients.RemoveAll((client) => true);
            }
            else
            {
                lock (WaitingClients)
                {
                    foreach (Client c in WaitingClients)
                    {
                        lock(c)
                            c.HandleFromServer(SECONDS_BETWEEN_GAME_START - SecondCounter);
                    }
                }
            }
            lock (OnGoingGames)
            {
                foreach( Game game in OnGoingGames.ToArray())
                {
                    if (game.IsOver)
                    {
                        OnGoingGames.Remove(game);
                    }
                }
            }
        }

        private static void StartGame()
        {
            lock (WaitingClients)
            {
                foreach (Client c in WaitingClients)
                {
                    lock (c)
                    {
                        c.HandleFromServer(UnstartedGame);
                    }
                }
                WaitingClients = new List<Client>();
            }
            lock (OnGoingGames)
            {
                OnGoingGames.Add(UnstartedGame);
                UnstartedGame.StartGame();
                UnstartedGame = null;
            }
        }
    }
}
