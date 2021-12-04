using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OriginalException;

namespace Tools.Logger
{
    /// <summary> ロガーインスタンを生成して返します </summary>
    /// <remarks> 本番用＆テスト用ロガーを切り替えて提供します </remarks>
    /// <remarks> シングルトンデザインパターン </remarks>
    public static class LoggerFactory
    {
        /// <summary>　ダミー？　</summary>
        private static bool isDummy = true;

        /// <summary>　ロガーインスタンス　</summary>
        private static ILogger? logger;

        /// <summary> シングルトンなロガーインスタンスを返します </summary>
        /// <remarks><see cref="LoggerFactory.SetProductionMode"/>を叩いていないデフォルト状態だと、ダミー用ロガーを返します</remarks>
        /// <returns>ロガーインスタンス</returns>
        public static ILogger Get()
        {
            if (logger is null)
                logger = Create();
            return logger;
        }

        /// <summary>　本番用ロガーを使用するようにする </summary>
        /// <remarks> 使用する場合は必ずインスタンス生成前に使用するようにしてください。守らなければ例外をスローします。 </remarks>
        /// <exception cref="Exception">既にインスタンス生成済みなのに本関数が叩かれた場合にスロー</exception>
        public static void SetProductionMode()
        {
            if (logger is not null)
                throw new Exception($"{nameof(logger)}が生成済みなのにモードを切り替えられそうになりました。{nameof(isDummy)}:{isDummy.ToString()}");

            isDummy = false;
        }

        /// <summary> ロガーインスタンスを生成する </summary>
        /// <returns> ロガーインスタンス </returns>
        private static ILogger Create()
        {
            if (isDummy)
                return new DummyLogger();
            else
                return new Logger();
        }
    }
}
