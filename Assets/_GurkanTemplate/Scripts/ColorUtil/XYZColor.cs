using UnityEngine;

namespace _GurkanTemplate.Scripts.ColorUtil
{
    /**
	 * XYZ color
	 */
    // http://www.easyrgb.com/index.php?X=MATH&H=07#text7
    public class XYZColor
    {
        public float x, y, z;

        public XYZColor(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static XYZColor FromRGB(Color col)
        {
            return ColorUtilities.RGBToXYZ(col);
        }

        public static XYZColor FromRGB(float R, float G, float B)
        {
            return ColorUtilities.RGBToXYZ(R, G, B);
        }

        public override string ToString()
        {
            return string.Format("( {0}, {1}, {2} )", x, y, z);
        }
    }
}