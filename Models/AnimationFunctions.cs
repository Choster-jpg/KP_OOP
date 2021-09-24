using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace KP_OOP
{
    public class AnimationFunctions
    {
        private static PowerEase LocalPowerEase = new PowerEase();
        public static DoubleAnimation ReturnNewDoubleAnimationWBT(double from, double to, TimeSpan duration, TimeSpan begintime)
        {
            LocalPowerEase.Power = 6;
            DoubleAnimation HideButtons = new DoubleAnimation();
            HideButtons.From = from;
            HideButtons.To = to;
            HideButtons.Duration = duration;
            HideButtons.BeginTime = begintime;
            HideButtons.EasingFunction = LocalPowerEase;
            return HideButtons;
        }

        public static DoubleAnimation ReturnNewDoubleAnimationWOBT(double from, double to, TimeSpan duration)
        {
            LocalPowerEase.Power = 6;
            DoubleAnimation HideButtons = new DoubleAnimation();
            HideButtons.From = from;
            HideButtons.To = to;
            HideButtons.Duration = duration;
            HideButtons.EasingFunction = LocalPowerEase;
            return HideButtons;
        }

        public static ThicknessAnimation ReturnNewThicknessAnimationWBT(System.Windows.Thickness from, System.Windows.Thickness to, TimeSpan duration, TimeSpan begintime)
        {
            LocalPowerEase.Power = 6;
            ThicknessAnimation ShowLoginFormPart2 = new ThicknessAnimation();
            ShowLoginFormPart2.From = from;
            ShowLoginFormPart2.To = to;
            ShowLoginFormPart2.Duration = duration;
            ShowLoginFormPart2.BeginTime = begintime;
            ShowLoginFormPart2.EasingFunction = LocalPowerEase;
            return ShowLoginFormPart2;
        }

        public static ThicknessAnimation ReturnNewThicknessAnimationWOBT(System.Windows.Thickness from, System.Windows.Thickness to, TimeSpan duration)
        {
            LocalPowerEase.Power = 6;
            ThicknessAnimation ShowLoginFormPart2 = new ThicknessAnimation();
            ShowLoginFormPart2.From = from;
            ShowLoginFormPart2.To = to;
            ShowLoginFormPart2.Duration = duration;
            ShowLoginFormPart2.EasingFunction = LocalPowerEase;
            return ShowLoginFormPart2;
        }
    }
}
