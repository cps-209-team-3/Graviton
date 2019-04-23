using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    public class AnimationEventArgs : EventArgs
    {
        private bool isTransition;
        public bool IsTransition
        {
            get { return isTransition; }
            set { isTransition = value; }
        }

        private AnimationType type;
        public AnimationType Type
        {
            get { return type; }
            set { type = value; }
        }

        private int objIndex;
        public int ObjIndex
        {
            get { return objIndex; }
            set { objIndex = value; }
        }

        private int animIndex;
        public int AnimIndex
        {
            get { return animIndex; }
            set { animIndex = value; }
        }

        private int transitionIndex;
        public int TransitionIndex
        {
            get { return transitionIndex; }
            set { transitionIndex = value; }
        }

        public AnimationEventArgs(bool isTransition, AnimationType type, int objIndex, int animIndex, int transitionIndex)
        {
            this.isTransition = isTransition;
            this.type = type;
            this.objIndex = objIndex;
            this.animIndex = animIndex;
            this.transitionIndex = transitionIndex;
        }
    }
}
