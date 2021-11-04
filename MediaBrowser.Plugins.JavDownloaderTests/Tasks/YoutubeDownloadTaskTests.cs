namespace MediaBrowser.Plugins.JavDownloader.Tasks.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using YoutubeDLSharp;

    /// <summary>
    /// Defines the <see cref="YoutubeDownloadTaskTests" />.
    /// </summary>
    [TestClass()]
    public class YoutubeDownloadTaskTests
    {
        /// <summary>
        /// The YoutubeDownloadTaskTest.
        /// </summary>
        [TestMethod()]
        [Ignore]
        public async Task YoutubeDownloadTaskTest()
        {
            var dl = new YoutubeDL();
            dl.FFmpegPath = "C:\\tools\\ffmpeg.exe";
            dl.YoutubeDLPath = "C:\\tools\\youtube-dl.exe";
            dl.OutputFolder = "D:\\";
            var progress = new Progress<DownloadProgress>(p => Console.WriteLine($"{p.DownloadSpeed} {p.Progress*100}%"));
            await dl.RunVideoDownload("https://www.xvideos.com/video64457745/_-_", progress:progress);
        }
    }
}
