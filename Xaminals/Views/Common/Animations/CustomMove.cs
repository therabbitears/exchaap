using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xaminals.Views.Common.Animations
{
    public class CustomMoveAnimation : MoveAnimation
    {
        public CustomMoveAnimation() : this(MoveAnimationOptions.Bottom, MoveAnimationOptions.Bottom) { }

        public CustomMoveAnimation(MoveAnimationOptions positionIn, MoveAnimationOptions positionOut)
        {
            PositionIn = positionIn;
            PositionOut = positionOut;

            DurationIn = DurationOut = 300;
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;
        }

        public static int y = 0;
        public static int x = 0;

        public static int tx = 0;
        public static int ty = 300;

        public async override Task Appearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            taskList.Add(base.Appearing(content, page));

            if (content != null)
            {
                if (PositionIn == MoveAnimationOptions.Bottom)
                {
                    content.TranslationX = tx;
                    content.TranslationY = ty;
                    taskList.Add(content.TranslateTo(x, y, DurationIn, EasingIn));
                }
            }

            ShowPage(page);

            await Task.WhenAll(taskList);
        }
    }
}
