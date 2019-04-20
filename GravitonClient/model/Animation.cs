using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GravitonClient.model
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
            get { return currentImage; }
            set { currentImage = value; }
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
            set { passedTicks = value; }
        }
    }
}
