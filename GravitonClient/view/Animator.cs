//-----------------------------------------------------------
//File:   Animator.cs
//Desc:   Contains logic for managing all animation for 
//        a single gameobject.
//----------------------------------------------------------- 

using System.Windows.Controls;

namespace GravitonClient
{
    //-----------------------------------------------------------
    //        This class contains the logic for animating a 
    //        gameobject. It allows for one gameobject to have 
    //        multiple animations for different events as well 
    //        as transitions between animations.
    //----------------------------------------------------------- 
    class Animator
    {
        //The image component used to display the animation.
        private Image displayImage;
        public Image DisplayImage
        {
            get { return displayImage; }
            set { displayImage = value; }
        }

        //The canvas component on which the image is displayed.
        private Canvas displayCanvas;
        public Canvas DisplayCanvas
        {
            get { return displayCanvas; }
            set { displayCanvas = value; }
        }

        //The list of animations that the gameobject possesses.
        private Animation[] animations;
        public Animation[] Animations
        {
            get { return animations; }
            set { animations = value; }
        }

        //The currently active animation.
        private Animation currentAnimation;
        public Animation CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        //Zindex of the image component (determines what is displayed below and on top of the image).
        private int zIndex;
        public int ZIndex
        {
            get { return zIndex; }
            set { zIndex = value; }
        }

        //determines if there is an animation queued up.
        private bool queued;
        //contains the remaining number of ticks until the queued animation becomes the active animation.
        private int queueTicks;
        //reference to the queued animation.
        private int queuedAnim;

        //Constructor - regular variant
        public Animator(Canvas canvas, Animation[] animations, int startAnim, int zIndex)
        {
            displayCanvas = canvas;
            this.animations = animations;
            currentAnimation = animations[startAnim];
            this.zIndex = zIndex;
            displayImage = new Image();
            AddToScreen();
        }

        //Constructor - specifies image component width
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

        // Updates the visibility and position of the animation on screen.
        // Accepts two offsets as doubles.
        // Returns nothing.
        public void Animate(double x, double y)
        {
            if (displayImage.Visibility != System.Windows.Visibility.Visible)
                displayImage.Visibility = System.Windows.Visibility.Visible;
            Canvas.SetLeft(displayImage, x);
            Canvas.SetTop(displayImage, y);
        }

        // Updates the animation image source (whether on or off screen).
        // Accepts nothing.
        // Returns nothing.
        public void Update()
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
            displayImage.Visibility = System.Windows.Visibility.Hidden;
            displayImage.Source = currentAnimation.CurrentImage;
        }

        // Adds the image component to the canvas.
        // Accepts nothing.
        // Returns nothing.
        public void AddToScreen()
        {
            Canvas.SetZIndex(displayImage, zIndex);
            displayCanvas.Children.Add(displayImage);
        }

        // Removes the image component from the canvas.
        // Accepts nothing.
        // Returns nothing.
        public void RemoveFromScreen()
        {
            displayCanvas.Children.Remove(displayImage);
        }

        // Changes the animation to another specified animation.
        // Accepts the index of the new animation (in the animation array).
        // Returns nothing.
        public void ChangeAnimation(int newAnim)
        {
            animations[newAnim].Reset();
            currentAnimation = animations[newAnim];
        }

        // Changes the animation to another specified animation and queues up another animation to display after the first is done.
        // Accepts the index of the transition animation (in the animation array) and the index of the queued animation.
        // Returns nothing.
        public void Transition(int transitionAnim, int finalAnim)
        {
            ChangeAnimation(transitionAnim);
            queued = true;
            queuedAnim = finalAnim;
            queueTicks = animations[transitionAnim].MaxTicks;
        }
    }
}
