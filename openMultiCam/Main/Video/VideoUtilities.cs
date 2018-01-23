using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using openMultiCam.WorkspaceUtils;
using System.Drawing.Drawing2D;

namespace openMultiCam.Utils {
    public class VideoUtilities {
        public static Image getPreviewImage(String filePath) {
            if(File.Exists(filePath + "\\" + VideoFileWriter.VIDEO_PREVIEW_FILENAME)) {
                return Image.FromFile(filePath + "\\" + VideoFileWriter.VIDEO_PREVIEW_FILENAME, true);
            } else {
                return null;
            }
        }

        public static float getRecordingQuality(String filePath) {
            VideoFileReader videoFileReader = new VideoFileReader(filePath);

            float quality = getRecordingQuality(videoFileReader);
            videoFileReader.dispose();
            videoFileReader = null;
            return quality;
        }

        public static float getRecordingQuality(VideoFileReader videoFileReader) {
            return videoFileReader.videoFileMetaData.recordingQuality;
        }

        public static PlayTime extractPlayTime(String filePath) {
            VideoFileReader videoFileReader = new VideoFileReader(filePath);

            PlayTime playTime = extractPlayTime(videoFileReader);
            videoFileReader.dispose();
            videoFileReader = null;
            return playTime;
        }

        public static PlayTime extractPlayTime(VideoFileReader videoFileReader) {
            float frameTime = 1000 / videoFileReader.videoFileMetaData.targetFramerate;
            long playTimeMilliseconds = (long)((long)frameTime * videoFileReader.videoFileMetaData.frameCount);
            int playtimeSeconds = (int)(playTimeMilliseconds / 1000);
            int playtimeMinutes = (int)(playtimeSeconds / 60);
            playtimeSeconds = (int)(playtimeSeconds % 60);

            return new PlayTime(playtimeMinutes, playtimeSeconds);
        }

        public static long getSizeOfAllFilesWithinDirectory(String filePath) {
            String[] allFilesWithinFolder = Directory.GetFiles(filePath, "*.*");

            long currentByteCount = 0;
            foreach (String fileName in allFilesWithinFolder) {
                FileInfo fileInfo = new FileInfo(fileName);
                currentByteCount += fileInfo.Length;
            }

            return currentByteCount;
        }
    }
}
