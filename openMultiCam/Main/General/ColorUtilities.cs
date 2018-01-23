using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace openMultiCam.Utils {
    public class ColorUtilities {

        public static Color getTintByPercentage(double percentage) {
            if(percentage > 1) {
                percentage = 1;
            }
            byte redChannel;
            byte greenChannel;

            if (percentage < 0.3 || percentage > 0.7) {
                redChannel = (byte)((byte.MaxValue - (byte.MaxValue * percentage)) * 0.6);
                greenChannel = (byte)(byte.MaxValue * percentage * 0.6);
            } else {
                redChannel = (byte)(byte.MaxValue - (byte.MaxValue * percentage));
                greenChannel = (byte)(byte.MaxValue * percentage);
            }

            return Color.FromRgb(redChannel, greenChannel, 20);
        }
    }
}
