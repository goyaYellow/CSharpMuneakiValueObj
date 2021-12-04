using System;
using System.Diagnostics;
using Tools.OriginalException;

namespace Tools.Logger
{
    /// <summary> 汎用ロガー </summary>
    /// <inheritdoc/>
    public class Logger : BaseLogger
    {
        /// <inheritdoc/>
        protected override void Write(Level leevel, string message, string methodName, Exception? exception = null)
        {
            // todo 実際のプロダクトで以下を実装
            // 実際のロギング
            // log4net、画面、コンソールなど
            var now = DateTime.Now;
            var exeMsg = exception is null ? string.Empty : exception.Message;
            var exeTrace = exception is null ? string.Empty : exception.StackTrace;

            // コンソール
            Console.WriteLine($"ログ出力, {leevel.ToString()}, {now}, {message}, {methodName}, {exeMsg}, {exeTrace}");
        }
    }
}
