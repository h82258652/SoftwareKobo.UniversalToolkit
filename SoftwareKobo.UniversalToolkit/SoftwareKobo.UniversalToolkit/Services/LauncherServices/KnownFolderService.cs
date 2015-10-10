using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace SoftwareKobo.UniversalToolkit.Services.LauncherServices
{
    public class KnownFolderService
    {
        public async Task OpenAppCapturesFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.AppCaptures);
        }

        public async Task OpenCameraRollFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.CameraRoll);
        }

        public async Task OpenDocumentsLibraryFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.DocumentsLibrary);
        }

        public async Task OpenHomeGroupFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.HomeGroup);
        }

        public async Task OpenMediaServerDevicesFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.MediaServerDevices);
        }

        public async Task OpenMusicLibraryFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.MusicLibrary);
        }

        public async Task OpenObjects3DFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.Objects3D);
        }

        public async Task OpenPicturesLibraryFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.PicturesLibrary);
        }

        public async Task OpenPlaylistsFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.Playlists);
        }

        public async Task OpenRecordedCallsFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.RecordedCalls);
        }

        public async Task OpenRemovableDevicesFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.RemovableDevices);
        }

        public async Task OpenSavedPicturesFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.SavedPictures);
        }

        public async Task OpenVideosLibraryFolderAsync()
        {
            await Launcher.LaunchFolderAsync(KnownFolders.VideosLibrary);
        }
    }
}