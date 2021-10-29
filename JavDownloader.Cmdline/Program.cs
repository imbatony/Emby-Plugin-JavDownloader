namespace JavDownloader.Cmdline
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Downloader;
    using Furion.Tools.CommandLine;
    using MediaBrowser.Plugins.JavDownloader;
    using MediaBrowser.Plugins.JavDownloader.Configuration;
    using MediaBrowser.Plugins.JavDownloader.Data;
    using MediaBrowser.Plugins.JavDownloader.Logger;
    using MediaBrowser.Plugins.JavDownloader.Media;
    using Newtonsoft.Json;
    using ShellProgressBar;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Gets or sets the ConsoleProgress.
        /// </summary>
        private static ProgressBar ConsoleProgress { get; set; }

        /// <summary>
        /// Gets or sets the ChildConsoleProgresses.
        /// </summary>
        private static ConcurrentDictionary<string, ChildProgressBar> ChildConsoleProgresses { get; set; }

        /// <summary>
        /// Gets or sets the ChildOption.
        /// </summary>
        private static ProgressBarOptions ChildOption { get; set; }

        /// <summary>
        /// Gets or sets the ProcessBarOption.
        /// </summary>
        private static ProgressBarOptions ProcessBarOption { get; set; }

        /// <summary>
        /// Defines the _currentDownloadService.
        /// </summary>
        private static DownloadService _currentDownloadService;

        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        internal static void Main(string[] args)
        {
            Plugin plugin = new Plugin(null, null, new CommandLineLoggerManager());
            plugin.SetConf(new PluginConfiguration());
            Cli.Inject();
        }

        /// <summary>
        /// Gets or sets the Download
        /// download......
        /// </summary>
        [Argument('u', "url", "url")]
        internal static string Url { get; set; }

        /// <summary>
        /// Gets or sets the Download
        /// download......
        /// </summary>
        [Argument('t', "target", "target path")]
        internal static string TargetPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Version
        /// 查看版本......
        /// </summary>
        [Argument('p', "popular", "popular")]
        internal static bool Popular { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Resolve
        /// Gets or sets the Resolve
        /// download......
        /// </summary>
        [Argument('r', "resoleve", "resoleve")]
        internal static bool Resolve { get; set; }

        /// <summary>
        /// The ResolveHandler.
        /// </summary>
        /// <param name="argument">The argument<see cref="ArgumentMetadata"/>.</param>
        internal static void ResolveHandler(ArgumentMetadata argument)
        {
            List<IMedia> medias;
            if (Popular)
            {
                medias = Plugin.Instance.javProvider.GetTodayPopular().Result;
            }
            else
            {
                medias = Plugin.Instance.javProvider.Resolve(Url).Result;
            }

            Console.WriteLine(JsonConvert.SerializeObject(medias, Formatting.Indented));
        }

        /// <summary>
        /// Gets or sets a value indicating whether Download
        /// Gets or sets the Resolve
        /// download......
        /// </summary>
        [Argument('d', "download", "download")]
        internal static bool Download { get; set; }

        /// <summary>
        /// The DownloadHandler.
        /// </summary>
        /// <param name="argument">The argument<see cref="ArgumentMetadata"/>.</param>
        internal static void DownloadHandler(ArgumentMetadata argument)
        {
            List<IMedia> medias;
            if (Popular)
            {
                medias = Plugin.Instance.javProvider.GetTodayPopular().Result;
            }
            else
            {
                medias = Plugin.Instance.javProvider.Resolve(Url).Result;
            }


            var downloadList = DownloadItem.FromMedias(medias, TargetPath ?? Directory.GetCurrentDirectory());

            try
            {
                new Thread(AddEscapeHandler) { IsBackground = true }.Start();
                Initial();
                DownloadAll(downloadList).Wait();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Debugger.Break();
            }

            Console.WriteLine("END");
            Console.Read();
        }

        /// <summary>
        /// Gets or sets a value indicating whether Version
        /// 查看版本......
        /// </summary>
        [Argument('v', "version", "工具版本号")]
        internal static bool Version { get; set; }

        /// <summary>
        /// The VersionHandler.
        /// </summary>
        /// <param name="argument">The argument<see cref="ArgumentMetadata"/>.</param>
        internal static void VersionHandler(ArgumentMetadata argument)
        {
            Console.WriteLine(Cli.GetVersion());
        }

        /// <summary>
        /// Gets or sets a value indicating whether Help
        /// 查看帮助文档......
        /// </summary>
        [Argument('h', "help", "查看帮助文档")]
        internal static bool Help { get; set; }

        /// <summary>
        /// The HelpHandler.
        /// </summary>
        /// <param name="argument">The argument<see cref="ArgumentMetadata"/>.</param>
        internal static void HelpHandler(ArgumentMetadata argument)
        {
            Cli.GetHelpText("Jav Downloader");
        }

        /// <summary>
        /// The NoMatchesHandler.
        /// </summary>
        /// <param name="isEmpty">The isEmpty<see cref="bool"/>.</param>
        /// <param name="operands">The operands<see cref="string[]"/>.</param>
        /// <param name="noMatches">The noMatches<see cref="Dictionary{string, object}"/>.</param>
        internal static void NoMatchesHandler(bool isEmpty, string[] operands, Dictionary<string, object> noMatches)
        {
            if (isEmpty)
            {
                Console.WriteLine(@"
       _               _____                      _                 _           
      | |             |  __ \                    | |               | |          
      | | __ ___   __ | |  | | _____      ___ __ | | ___   __ _  __| | ___ _ __ 
  _   | |/ _` \ \ / / | |  | |/ _ \ \ /\ / / '_ \| |/ _ \ / _` |/ _` |/ _ \ '__|
 | |__| | (_| |\ V /  | |__| | (_) \ V  V /| | | | | (_) | (_| | (_| |  __/ |   
  \____/ \__,_| \_/   |_____/ \___/ \_/\_/ |_| |_|_|\___/ \__,_|\__,_|\___|_|   
                                                                                
                                                                                


");
                Console.WriteLine($"欢迎使用{Cli.GetDescription()}");
            }
        }

        /// <summary>
        /// The Initial.
        /// </summary>
        private static void Initial()
        {
            ProcessBarOption = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Green,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593'
            };
            ChildOption = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                BackgroundColor = ConsoleColor.DarkGray,
                ProgressCharacter = '─'
            };
        }

        /// <summary>
        /// The AddEscapeHandler.
        /// </summary>
        private static void AddEscapeHandler()
        {
            while (true)
            {
                while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                {
                    Thread.Sleep(100);
                }

                _currentDownloadService?.CancelAsync();
            }
        }

        /// <summary>
        /// The GetDownloadConfiguration.
        /// </summary>
        /// <returns>The <see cref="DownloadConfiguration"/>.</returns>
        private static DownloadConfiguration GetDownloadConfiguration()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1";
            var cookies = new CookieContainer();

            return new DownloadConfiguration
            {
                BufferBlockSize = 10240, // usually, hosts support max to 8000 bytes, default values is 8000
                ChunkCount = 8, // file parts to download, default value is 1
                MaximumBytesPerSecond = 1024 * 1024, // download speed limited to 1MB/s, default values is zero or unlimited
                MaxTryAgainOnFailover = int.MaxValue, // the maximum number of times to fail
                OnTheFlyDownload = false, // caching in-memory or not? default values is true
                ParallelDownload = true, // download parts of file as parallel or not. Default value is false
                Timeout = 1000, // timeout (millisecond) per stream block reader, default values is 1000
                RequestConfiguration = {
                    // config and customize request headers
                    Accept = "*/*",
                    CookieContainer = cookies,
                    Headers = new WebHeaderCollection(), // { Add your custom headers }
                    KeepAlive = true,
                    ProtocolVersion = HttpVersion.Version11, // Default value is HTTP 1.1
                    UseDefaultCredentials = false,
                    UserAgent = $"DownloaderSample/{version}"
                }
            };
        }

        /// <summary>
        /// The DownloadAll.
        /// </summary>
        /// <param name="downloadList">The downloadList<see cref="IEnumerable{DownloadItem}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task DownloadAll(IEnumerable<DownloadItem> downloadList)
        {
            foreach (DownloadItem downloadItem in downloadList)
            {
                // begin download from url
                DownloadService ds = await DownloadFile(downloadItem).ConfigureAwait(false);

                // clear download to order new of one
                ds.Clear();
            }
        }

        /// <summary>
        /// The DownloadFile.
        /// </summary>
        /// <param name="downloadItem">The downloadItem<see cref="DownloadItem"/>.</param>
        /// <returns>The <see cref="Task{DownloadService}"/>.</returns>
        private static async Task<DownloadService> DownloadFile(DownloadItem downloadItem)
        {
            _currentDownloadService = new DownloadService(GetDownloadConfiguration());
            _currentDownloadService.ChunkDownloadProgressChanged += OnChunkDownloadProgressChanged;
            _currentDownloadService.DownloadProgressChanged += OnDownloadProgressChanged;
            _currentDownloadService.DownloadFileCompleted += OnDownloadFileCompleted;
            _currentDownloadService.DownloadStarted += OnDownloadStarted;

            if (string.IsNullOrWhiteSpace(downloadItem.FileName))
            {
                await _currentDownloadService.DownloadFileTaskAsync(downloadItem.Url, new DirectoryInfo(downloadItem.FolderPath)).ConfigureAwait(false);
            }
            else
            {
                await _currentDownloadService.DownloadFileTaskAsync(downloadItem.Url, Path.Combine(downloadItem.FolderPath, downloadItem.FileName)).ConfigureAwait(false);
            }

            return _currentDownloadService;
        }

        /// <summary>
        /// The OnDownloadStarted.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DownloadStartedEventArgs"/>.</param>
        private static void OnDownloadStarted(object sender, DownloadStartedEventArgs e)
        {
            ConsoleProgress = new ProgressBar(10000,
                $"Downloading {Path.GetFileName(e.FileName)} ...", ProcessBarOption);
            ChildConsoleProgresses = new ConcurrentDictionary<string, ChildProgressBar>();
        }

        /// <summary>
        /// The OnDownloadFileCompleted.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="AsyncCompletedEventArgs"/>.</param>
        private static void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ConsoleProgress?.Tick(10000);
            Console.WriteLine();
            Console.WriteLine();

            if (e.Cancelled)
            {
                Console.WriteLine("Download canceled!");
            }
            else if (e.Error != null)
            {
                Console.Error.WriteLine(e.Error);
            }
            else
            {
                Console.WriteLine("Download completed successfully.");
                Console.Title = "100%";
            }
        }

        /// <summary>
        /// The OnChunkDownloadProgressChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DownloadProgressChangedEventArgs"/>.</param>
        private static void OnChunkDownloadProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            ChildProgressBar progress = ChildConsoleProgresses.GetOrAdd(e.ProgressId, id =>
                ConsoleProgress?.Spawn(10000, $"chunk {id}", ChildOption));
            progress.Tick((int)(e.ProgressPercentage * 100));
        }

        /// <summary>
        /// The OnDownloadProgressChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DownloadProgressChangedEventArgs"/>.</param>
        private static void OnDownloadProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            ConsoleProgress.Tick((int)(e.ProgressPercentage * 100));
            UpdateTitleInfo(e);
        }

        /// <summary>
        /// The UpdateTitleInfo.
        /// </summary>
        /// <param name="e">The e<see cref="DownloadProgressChangedEventArgs"/>.</param>
        private static void UpdateTitleInfo(Downloader.DownloadProgressChangedEventArgs e)
        {
            double nonZeroSpeed = e.BytesPerSecondSpeed + 0.0001;
            int estimateTime = (int)((e.TotalBytesToReceive - e.ReceivedBytesSize) / nonZeroSpeed);
            bool isMinutes = estimateTime >= 60;
            string timeLeftUnit = "seconds";

            if (isMinutes)
            {
                timeLeftUnit = "minutes";
                estimateTime /= 60;
            }

            if (estimateTime < 0)
            {
                estimateTime = 0;
                timeLeftUnit = "unknown";
            }

            string avgSpeed = CalcMemoryMensurableUnit(e.AverageBytesPerSecondSpeed);
            string speed = CalcMemoryMensurableUnit(e.BytesPerSecondSpeed);
            string bytesReceived = CalcMemoryMensurableUnit(e.ReceivedBytesSize);
            string totalBytesToReceive = CalcMemoryMensurableUnit(e.TotalBytesToReceive);
            string progressPercentage = $"{e.ProgressPercentage:F3}".Replace("/", ".");

            Console.Title = $"{progressPercentage}%  -  " +
                            $"{speed}/s (avg: {avgSpeed}/s)  -  " +
                            $"{estimateTime} {timeLeftUnit} left    -  " +
                            $"[{bytesReceived} of {totalBytesToReceive}]";
        }

        /// <summary>
        /// The CalcMemoryMensurableUnit.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="double"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string CalcMemoryMensurableUnit(double bytes)
        {
            double kb = bytes / 1024; // · 1024 Bytes = 1 Kilobyte 
            double mb = kb / 1024; // · 1024 Kilobytes = 1 Megabyte 
            double gb = mb / 1024; // · 1024 Megabytes = 1 Gigabyte 
            double tb = gb / 1024; // · 1024 Gigabytes = 1 Terabyte 

            string result =
                tb > 1 ? $"{tb:0.##}TB" :
                gb > 1 ? $"{gb:0.##}GB" :
                mb > 1 ? $"{mb:0.##}MB" :
                kb > 1 ? $"{kb:0.##}KB" :
                $"{bytes:0.##}B";

            result = result.Replace("/", ".");
            return result;
        }
    }
}
