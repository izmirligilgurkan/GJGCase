using UnityEngine;

namespace _GurkanTemplate.Scripts.ColorUtil
{
    public static class ColorHelper
    {
        public static float Difference(this Color lhs, Color rhs)
        {
            var lhsLab = ColorUtilities.XYZToCIE_Lab(ColorUtilities.RGBToXYZ(lhs));
            var rhsLab = ColorUtilities.XYZToCIE_Lab(ColorUtilities.RGBToXYZ(rhs));
            return ColorUtilities.DeltaE(lhsLab, rhsLab);
        }

        public static string Name(this Color color)
        {
            return ColorUtilities.GetColorName(color);
        }
    }
    
}