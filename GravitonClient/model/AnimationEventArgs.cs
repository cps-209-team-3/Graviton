//-----------------------------------------------------------
//File:   AnimationEventArgs.cs
//Desc:   Contains model class for arguments to an animation 
//        update event.
//----------------------------------------------------------- 

using System;

namespace GravitonClient
{
    //-----------------------------------------------------------
    //        This class contains the model for an animation 
    //        update argument package.
    //----------------------------------------------------------- 
    public class AnimationEventArgs : EventArgs
    {
        //Determines if the animation to be displayed is a transition animation.
        private bool isTransition;
        public bool IsTransition
        {
            get { return isTransition; }
            set { isTransition = value; }
        }

        //Determines the gameobject the event applies to (i.e. gravity well, black hole, orb).
        private AnimationType type;
        public AnimationType Type
        {
            get { return type; }
            set { type = value; }
        }

        //Determines the index of the gameobject in its respective list in the game model (helps bind animations to a particular object).
        private int objIndex;
        public int ObjIndex
        {
            get { return objIndex; }
            set { objIndex = value; }
        }

        //The index of the new animation (in the animator's animation array).
        private int animIndex;
        public int AnimIndex
        {
            get { return animIndex; }
            set { animIndex = value; }
        }

        //The index of the animation to come before the new animation (in the animator's animation array).
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
