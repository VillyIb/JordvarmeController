using System;

namespace JordvarmeController.Businesslayer
{
    public static class FloatExtension
    {
        public static bool IsEqualTo(this float a, float b, float margin)
        {
            return Math.Abs(a - b) < margin;
        }


        public static bool IsEqualTo(this float a, float b)
        {
            return Math.Abs(a - b) < float.Epsilon;
        }


        public static bool IsValid(this float value, float minValue, float maxValue)
        {
            return minValue <= value && value <= maxValue;
        }

        public static bool IsValid(this int value, int minValue, int maxValue)
        {
            return minValue <= value && value <= maxValue;
        }

        public static bool IsValid(this TimeSpan value, TimeSpan minValue, TimeSpan maxValue)
        {
            return minValue.Ticks <= value.Ticks && value.Ticks <= maxValue.Ticks;
        }

    }
}
