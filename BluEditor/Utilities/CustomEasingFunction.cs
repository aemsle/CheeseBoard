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

        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new CustomEasingFunction();
        }
    }
}