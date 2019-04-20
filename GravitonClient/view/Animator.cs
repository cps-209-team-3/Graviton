using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GravitonClient.model
{
    class Animator
    {
        private Image displayImage;
        public Image DisplayImage
        {
            get { return displayImage; }
            set { displayImage = value; }
        }

        private Canvas displayCanvas;
        public Canvas DisplayCanvas
        {
            get { return displayCanvas; }
            set { displayCanvas = value; }
        }

        private Animation[] animations;
        public Animation[] Animations
        {
            get { return animations; }
            set { animations = value; }
        }

        private Animation currentAnimation;
        public Animation CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        public void Animate()
        {
            //update the image source on the image component.
        }
    }
}
