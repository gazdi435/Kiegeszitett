using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Input;

namespace minikozfelvir
{
    public class HoverBtn : Button
    {
        private readonly ColorAnimation animBg, animFg;

        public HoverBtn() : base()
        {
            animBg = new ColorAnimation { Duration = TimeSpan.FromMilliseconds(300), FillBehavior = FillBehavior.HoldEnd };
            animFg = new ColorAnimation { Duration = TimeSpan.FromMilliseconds(300), FillBehavior = FillBehavior.HoldEnd };

            Cursor = Cursors.Hand;

            MouseEnter += (s, e) => Animate(animFg, animBg, this);
            MouseLeave += (s, e) => Animate(animBg, animFg, this);

            DependencyPropertyDescriptor.FromProperty(BackgroundProperty, typeof(HoverBtn)).AddValueChanged(this, OnBackgroundColorChanged);
            DependencyPropertyDescriptor.FromProperty(ForegroundProperty, typeof(HoverBtn)).AddValueChanged(this, OnForegroundColorChanged);
        }

        #region __Animate__
        private void Animate(ColorAnimation foregroundAnim, ColorAnimation backgroundAnim, HoverBtn sender)
        {
            DependencyPropertyDescriptor.FromProperty(BackgroundProperty, typeof(HoverBtn)).RemoveValueChanged(this, OnBackgroundColorChanged);
            DependencyPropertyDescriptor.FromProperty(ForegroundProperty, typeof(HoverBtn)).RemoveValueChanged(this, OnForegroundColorChanged);

            Storyboard.SetTarget(backgroundAnim, sender);
            Storyboard.SetTarget(foregroundAnim, sender);

            Storyboard.SetTargetProperty(backgroundAnim, new("Background.Color"));
            Storyboard.SetTargetProperty(foregroundAnim, new("Foreground.Color"));

            Storyboard storyboard = new();

            storyboard.Children.Add(backgroundAnim);
            storyboard.Children.Add(foregroundAnim);

            sender.BeginStoryboard(storyboard);
        }
        #endregion

        #region __Property Change Handlers__
        private void OnBackgroundColorChanged(object sender, EventArgs _)
        {
            animFg.To = ((SolidColorBrush)Background).Color;
        }
        private void OnForegroundColorChanged(object sender, EventArgs _)
        {
            animBg.To = ((SolidColorBrush)Foreground).Color;
        }
        #endregion
    }
}