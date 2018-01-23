using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openMultiCam.WorkspaceUtils {
    public class PlayTime {
        public int minutes { get; set; }
        public int seconds { get; set; }

        public PlayTime(int minutes, int seconds) {
            this.minutes = minutes;
            this.seconds = seconds;
        }

        public String getZeroLeadingMinutes() {
            return getZeroLeadingTwoDigits(minutes);
        }

        public String getZeroLeadingSeconds() {
            return getZeroLeadingTwoDigits(seconds);
        }

        public String getFormattedTime() {
            return getZeroLeadingMinutes() + ":" + getZeroLeadingSeconds();
        }

        private String getZeroLeadingTwoDigits(int value) {
            if (value < 10) {
                return "0" + value;
            } else {
                return value.ToString();
            }
        }
    }
}
