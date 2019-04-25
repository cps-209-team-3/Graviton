/*
File: GameManager.cs
Desc: Manages starting games and alerting clients to the new game.
*/
using System;
using System.Collections.Generic;
using System.Timers;
using System.Text;

namespace GravitonServer
{
    //Manages all games for the server
    internal static class GameManager
    {
        //countdown starting value
        private static readonly int SECONDS_BETWEEN_GAME_START = 20;
        //countdown value holder
        private static int SecondCounter = 0;

        //list of ongoing games
        private static List<Game> OnGoingGames = new List<Game>();
        //list of clients awaiting a game
        private static List<Client> WaitingClients = new List<Client>();
        //reference to an unstarted game object for new clients to connect to
        private static Game UnstartedGame;
        //game timer for the unstarted game?
        private static Timer GameTimer = new Timer();
        
        //sets up the game timer
        internal static void Start()
        {
            GameTimer.Interval = 1000;
            GameTimer.Elapsed += Second_tick;
            GameTimer.AutoReset = true;
        }

        //lets the client join a game
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

        //event handler for game timer
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

        //starts a new game
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
