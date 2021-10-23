namespace MediaBrowser.Plugins.JavDownloader.Extensions.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the <see cref="StringExtensionsTests" />.
    /// </summary>
    [TestClass()]
    public class StringExtensionsTests
    {
        /// <summary>
        /// The ExtractKeyTest.
        /// </summary>
        [TestMethod()]
        public void ExtractKeyTest()
        {
            var num = "DVDMS-433 ザ・マジックミラー 顔出し！女子○校生限定 インタビュー中の素人娘にいきなりデカチン即ハメ！はじめましてで巨根をねじこまれ戸惑う間もなくうぶオマ○コの感度は急上昇！激ピストンで制服着たまま本気イキ！！ in池袋".ExtractKey();

            Console.WriteLine(num);
        }
    }
}
