using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GravitonClient
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

        private int zIndex;
        public int ZIndex
        {
            get { return zIndex; }
            set { zIndex = value; }
        }

        private bool queued;
        private int queueTicks;
        private int queuedAnim;

        public Animator(Canvas canvas, Animation[] animations, int startAnim, int zIndex)
        {
            displayCanvas = canvas;
            this.animations = animations;
            currentAnimation = animations[startAnim];
            this.zIndex = zIndex;
            displayImage = new Image();
            AddToScreen();
        }

        public Animator(Canvas canvas, Animation[] animations, int startAnim, int zIndex, int width)
        {
            displayCanvas = canvas;
            this.animations = animations;
            currentAnimation = animations[startAnim];
            this.zIndex = zIndex;
            displayImage = new Image();
            displayImage.Width = width;
            AddToScreen();
        }

        public void Animate(double x, double y)
        {
            if (queued)
            {
                --queueTicks;
                if (queueTicks == 0)
                {
                    queued = false;
                    ChangeAnimation(queuedAnim);
                }
            }

            currentAnimation.PassedTicks++;
            //update the image source on the image component.
            displayImage.Source = currentAnimation.CurrentImage;
            Canvas.SetLeft(displayImage, x);
            Canvas.SetTop(displayImage, y);
        }

        public void AddToScreen()
        {
            Canvas.SetZIndex(displayImage, zIndex);
            displayCanvas.Children.Add(displayImage);
        }

        public void RemoveFromScreen()
        {
            displayCanvas.Children.Remove(displayImage);
        }

        public void ChangeAnimation(int newAnim)
        {
            animations[newAnim].Reset();
            currentAnimation = animations[newAnim];
        }

        public void Transition(int transitionAnim, int finalAnim)
        {
            ChangeAnimation(transitionAnim);
            queued = true;
            queuedAnim = finalAnim;
            queueTicks = animations[transitionAnim].MaxTicks;
        }
    }
}
