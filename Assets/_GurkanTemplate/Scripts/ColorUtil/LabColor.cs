using UnityEngine;

namespace _GurkanTemplate.Scripts.ColorUtil
{
    /**
	 * CIE_Lab* color
	 */
    public class LabColor
    {
        public float L, a, b;

        public LabColor(float L, float a, float b)
        {
            this.L = L;
            this.a = a;
            this.b = b;
        }

        public static LabColor FromXYZ(XYZColor xyz)
        {
            return ColorUtilities.XYZToCIE_Lab(xyz);
        }

        public static LabColor FromRGB(Color col)
        {
            XYZColor xyz = XYZColor.FromRGB(col);

            return ColorUtilities.XYZToCIE_Lab(xyz);
        }

        public override string ToString()
        {
            return string.Format("( {0}, {1}, {2} )", L, a, b);
        }
    }
}