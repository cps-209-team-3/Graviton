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
        public event EventHandler<int> BoardUpdatedEvent;
        public bool isCheat;
        public int ticks;
        public int wasdDepressed;
        public DispatcherTimer timer;
        public List<Well> wells;
        public Ship user;
        public List<Orb> orbs;
        public Game()
        {

        }


        public void Load(string filename)
        {
            
        }

        public void Save(string filename)
        {

        }
        public void WASDPressed(int key)
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
