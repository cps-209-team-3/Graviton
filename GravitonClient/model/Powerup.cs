using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GravitonClient
{
    internal abstract class PowerUp
    {
        internal static PowerUp GetRandomPowerUpFactory(Ship ship)
        {
            throw new System.NotImplementedException();
        }

        internal abstract void Execute();
    }

    internal class GhostingPowerUp : PowerUp
    {

        public override string ToString()
        {
            return "ghost";
        }

        public GhostingPowerUp(Ship ship)
        {

        }

        internal override void Execute()
        {

            throw new NotImplementedException();
        }
    }

    internal class NeutralizePowerUp : PowerUp
    {
        public NeutralizePowerUp(Ship ship)
        {

        }


        internal override void Execute()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "neutralize";
        }
    }

    internal class DestabilizePowerUp : PowerUp
    {

        public DestabilizePowerUp(Ship ship)
        {

        }


        internal override void Execute()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "destabilize";
        }
    }
}