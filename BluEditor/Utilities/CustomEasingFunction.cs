using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace BluEditor.Utilities
{
    public class CustomEasingFunction : EasingFunctionBase
    {
        public CustomEasingFunction() : base()
        { }

        protected override double EaseInCore(double normalizedTime)
        {
            const double c4 = (2 * Math.PI) / 6;

            return normalizedTime == 0
              ? 0
              : normalizedTime == 1
              ? 1
              : -Math.Pow(2, 10 * normalizedTime - 10) * Math.Sin((normalizedTime * 10 - 10.75) * c4);
        }

        private double easeOutBounce(double time)
        {
            const double n1 = 7.5625;
            const double d1 = 2.75;

            if (time < 1 / d1)
            {
                return n1 * time * time;
            }
            else if (time < 2 / d1)
            {
                return n1 * (time -= 1.5 / d1) * time + 0.75;
            }
            else if (time < 2.5 / d1)
            {
                return n1 * (time -= 2.25 / d1) * time + 0.9375;
            }
            else
            {
                return n1 * (time -= 2.625 / d1) * time + 0.984375;
            }
        }

        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new CustomEasingFunction();
        }
    }
}