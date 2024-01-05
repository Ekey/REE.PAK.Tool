using System;

namespace REE.Packer
{
    class PakUtils
    {
        public static Int32 iGetPercent(Int32 dwKnown, Int32 dwMaxValue)
        {
            return (Int32)Math.Floor(((float)dwKnown / (float)dwMaxValue) * 100.0);
        }

        public static String iPrintInfo(Int32 dwKnown, Int32 dwMaxValue)
        {
            return String.Format("{0} of {1} ({2}%)", dwKnown, dwMaxValue, iGetPercent(dwKnown, dwMaxValue));
        }
    }
}
