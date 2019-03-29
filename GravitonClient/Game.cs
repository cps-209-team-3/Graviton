using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace GravitonClient
{
    class Game
    {
        public event EventHandler<int> GameUpdatedEvent;
        public bool IsCheat { get; set; }
        public bool Points { get; set; }
        public int Ticks { get; set; }
        public int HorizontalInput { get; set; }
        public int VerticalInput { get; set; }
        public DispatcherTimer Timer { get; set; }
        public List<Well> Wells { get; set; }
        public Ship User { get; set; }
        public List<Orb> Orbs { get; set; }
        public Game()
        {

        }


        public void Load(string filename)
        {
            
        }

        public void Save(string filename)
        {

        }
        public void KeyPressed(char c)
        {

        }

        public void KeyReleased(char c)
        {

        }
        public void Timer_Tick()
        {

        }
        public void UpdateWells()
        {

        }
        public void UpdateUser()
        {

        }
        public void SpawnWell()
        {

        }
        public void SpawnOrb()
        {

        }
    }
}
