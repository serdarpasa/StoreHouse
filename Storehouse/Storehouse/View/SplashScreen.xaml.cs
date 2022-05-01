using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Storehouse.View
{
  
    public partial class SplashScreen
    {

        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation shadow = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(5)
            };
            shadow.Completed += AnimationCompleted;

            PnlTitle.Effect.BeginAnimation(DropShadowEffect.ShadowDepthProperty, shadow);

            var wind = new DoubleAnimation(0.5, 1, new Duration(TimeSpan.FromSeconds(3)));
            BeginAnimation(OpacityProperty, wind);
            
        }
        private void AnimationCompleted(object sender, EventArgs e)
        {
            Close();   
        }  

       
        
    }
}
