using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluEditor.Utilities
{
    public static class ID
    {
        public static int INVALID_ID => -1;

        public static bool IsValid(int in_id) => in_id != INVALID_ID;
    }

    public static class MathUtil
    {
        public static float Epsilon => 0.00001f;

        public static bool Approx(this float in_value, float in_other)
        {
            return (Math.Abs(in_value - in_other)) < Epsilon;
        }

        public static bool Approx(this float? in_value, float? in_other)
        {
            if (!in_value.HasValue || !in_other.HasValue) return false;
            return (Math.Abs(in_value.Value - in_other.Value)) < Epsilon;
        }
    }
}