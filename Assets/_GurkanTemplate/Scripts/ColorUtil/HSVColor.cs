using UnityEngine;

namespace _GurkanTemplate.Scripts.ColorUtil
{
    /**
	 * Hue (0,360), Saturation (0,1), Value (0,1)
	 */
    public class HSVColor
    {
        public float h, s, v;

        public HSVColor(float h, float s, float v)
        {
            this.h = h;
            this.s = s;
            this.v = v;
        }

        /**
		 * Wikipedia colors are from 0-100 %, so this constructor includes and S, V normalizes the values.
		 * modifier value that affects saturation and value, making it useful for any SV value range.
		 */
        public HSVColor(float h, float s, float v, float sv_modifier)
        {
            this.h = h;
            this.s = s * sv_modifier;
            this.v = v * sv_modifier;
        }

        public static HSVColor FromRGB(Color col)
        {
            return ColorUtilities.RGBtoHSV(col);
        }

        public override string ToString()
        {
            return string.Format("( {0}, {1}, {2} )", h, s, v);
        }

        public float SqrDistance(HSVColor InColor)
        {
            return (InColor.h/360f - this.h/360f) + (InColor.s - this.s) + (InColor.v - this.v);
        }
    }
}