using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openMultiCam {
    public class UniqueFileName {

        public static String generate() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("omc_");
            stringBuilder.Append(DateTime.Today.Year);
            stringBuilder.Append(DateTime.Today.Day);
            stringBuilder.Append(DateTime.Today.Month + "_");
            stringBuilder.Append(DateTime.Now.Second);
            stringBuilder.Append(DateTime.Now.Minute);

            return stringBuilder.ToString();
        }
    }
}
