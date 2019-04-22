using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GravitonClient
{
    class Animation
    {
        private BitmapImage[] imageList;
        public BitmapImage[] ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        private int[] tickDiffs;
        public int[] TickDiffs
        {
            get { return tickDiffs; }
            set { tickDiffs = value; }
        }

        private BitmapImage currentImage;
        public BitmapImage CurrentImage
        {
            get
            {
                int sumHolder = passedTicks;
                for (int i = 0; i < imageList.Length; ++i)
                {
                    sumHolder -= tickDiffs[i];
                    if (sumHolder < 0)
                    {
                        currentImage = imageList[i];
                        break;
                    }
                }
                return currentImage;
            }
        }

        private int maxTicks;
        public int MaxTicks
        {
            get { return maxTicks; }
            set { maxTicks = value; }
        }

        private int passedTicks;
        public int PassedTicks
        {
            get { return passedTicks; }
            set
            {
                passedTicks = value > maxTicks ? 0 : value;
            }
        }

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

        public void Reset()
        {
            currentImage = imageList[0];
            passedTicks = 0;
        }
    }
}
