//-----------------------------------------------------------
//File:   Animation.cs
//Desc:   Contains model class for animations.
//----------------------------------------------------------- 

using System.Windows.Media.Imaging;

namespace GravitonClient
{
    //-----------------------------------------------------------
    //        This class contains the logic and assets for an 
    //        animation sequence.
    //----------------------------------------------------------- 
    class Animation
    {
        //List of image assets for the animation sequence (in order to be displayed).
        private BitmapImage[] imageList;
        public BitmapImage[] ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        //List of number of ticks for each image to be displayed.
        private int[] tickDiffs;
        public int[] TickDiffs
        {
            get { return tickDiffs; }
            set { tickDiffs = value; }
        }

        //Reference to the currently displayed image.
        private BitmapImage currentImage;
        public BitmapImage CurrentImage
        {
            get { return currentImage; }
        }

        //The maximum number of ticks for the animation sequence.
        private int maxTicks;
        public int MaxTicks
        {
            get { return maxTicks; }
            set { maxTicks = value; }
        }

        //The number of elapsed ticks since the beginning of the animation sequence.
        //Automatically updates the currentimage field.
        private int passedTicks;
        public int PassedTicks
        {
            get { return passedTicks; }
            set
            {
                passedTicks = value > maxTicks ? 1 : value;
                int sumHolder = 0;
                for (int i = 0; i < imageList.Length; ++i)
                {
                    sumHolder += tickDiffs[i];
                    if (passedTicks <= sumHolder)
                    {
                        currentImage = imageList[i];
                        break;
                    }
                }
            }
        }

        //Constructor - regular
        public Animation(BitmapImage[] images, int[] ticks)
        {
            imageList = images;
            tickDiffs = ticks;
            currentImage = images[0];
            maxTicks = 0;
            for (int i = 0; i < ticks.Length; ++i)
            {
                maxTicks += ticks[i];
            }
            passedTicks = 0;
        }

        //Constructor - allows for starting in the middle of an animation sequence
        public Animation(BitmapImage[] images, int[] ticks, int startImage)
        {
            imageList = images;
            tickDiffs = ticks;
            currentImage = images[startImage];
            maxTicks = 0;
            for (int i = 0; i < ticks.Length; ++i)
            {
                maxTicks += ticks[i];
            }
            passedTicks = 0;
        }

        //Resets the animation sequence to its initial state.
        //Accepts nothing.
        //Returns nothing.
        public void Reset()
        {
            currentImage = imageList[0];
            passedTicks = 0;
        }
    }
}
