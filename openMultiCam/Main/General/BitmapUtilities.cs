using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openMultiCam.Utils {
    public class BitmapUtilities {
        public static ImageCodecInfo pngCodecInfo;
        public static EncoderParameters encodingParameters;

        static BitmapUtilities(){
            pngCodecInfo = getEncoderByImageFormat(ImageFormat.Png);
            encodingParameters = new EncoderParameters(1);
            setEncoderQuality(100);
        }

        public static void setEncoderQuality(int percentage) {
            EncoderParameter encodingParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, percentage);
            encodingParameters.Param[0] = encodingParameter;
        }

        public static byte[] bitmapToByteArray(Bitmap bitmap) {
            ImageConverter converter = new ImageConverter();
            byte[] currentBytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            return currentBytes;
        }

        public static byte[] bitmapToByteArrayFromMemoryStream(Bitmap bitmap) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                bitmap.Save(memoryStream, BitmapUtilities.pngCodecInfo, BitmapUtilities.encodingParameters);

                return memoryStream.ToArray();
            }
        }

        public static Bitmap byteArrayToBitmap(byte[] imageData) {
            using (MemoryStream memoryStream = new MemoryStream(imageData)) {
                return new Bitmap(memoryStream);
            }
        }

        private static ImageCodecInfo getEncoderByImageFormat(ImageFormat imageFormat) {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FormatID == imageFormat.Guid) {
                    return codec;
                }
            }
            return null;
        }

    }
}
