using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using openMultiCam.Utils;
using openMultiCam.WorkspaceUtils;

namespace openMultiCam {
    public class WorkspaceManager {
        private List<VideoPreview> previewList;

        public WorkspaceManager() {
            previewList = new List<VideoPreview>();
        }

        public void indexWorkspace() {
            previewList = new List<VideoPreview>();
            DirectoryInfo directoryInfo = new DirectoryInfo(CamConstants.WORKSPACE_PATH);

            foreach(DirectoryInfo currentDirectory in directoryInfo.GetDirectories()) {
                VideoFileReader currentVideoFileReader = new VideoFileReader(currentDirectory.FullName + "\\");
                previewList.Add(new VideoPreview(VideoUtilities.getPreviewImage(currentDirectory.FullName + "\\"), 
                                                    VideoUtilities.extractPlayTime(currentVideoFileReader),
                                                    VideoUtilities.getSizeOfAllFilesWithinDirectory(currentDirectory.FullName + "\\"),
                                                    currentDirectory.Name,
                                                    currentDirectory.FullName,
                                                    VideoUtilities.getRecordingQuality(currentVideoFileReader)));
                currentVideoFileReader.dispose();
                currentVideoFileReader = null;
            }

        }

        public VideoPreview[] getPreviews() {
            return previewList.ToArray();
        }

        public float totalWorkspaceSizeInGigaByte() {
            double currentTotalFileSize = 0;
            foreach(VideoPreview currentVideoPreview in previewList) {
                currentTotalFileSize += currentVideoPreview.getFileSizeInGigaByte();
            }

            return (float)currentTotalFileSize;
        }

        public String getFormattedTotalWorkspaceSizeInGigaByte() {
            return totalWorkspaceSizeInGigaByte().ToString("0.00") + " GB";
        }

        public static String getWorkspacePathForGivenFile(String fileName) {
            return CamConstants.WORKSPACE_PATH + fileName + "\\";
        }
    }
}
