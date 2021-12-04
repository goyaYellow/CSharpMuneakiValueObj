using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OriginalException;

namespace Tools.Logger
{
    /// <summary> 汎用ロガーInterface </summary>W
    public interface ILogger
    {
        /// <summary> ログを書き込む </summary>
        /// <param name="leevel">ログレベル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exception">記録したい例外</param>
        /// <exception cref="ArgumentException">不正な文字を含むメッセージを渡された</exception>
        void Log(Level leevel, string? message, Exception? exception = null);
    }

    /// <summary> 汎用ロガー基底クラス </summary>
    /// <inheritdoc/>
    public abstract class BaseLogger : ILogger
    {
        /// <summary> 禁止文字 </summary>
        /// <remarks> プロジェクト毎に変えていい。ログがカンマ区切りの場合を考慮してとりまカンマ </remarks>
        public static readonly string InvalidChar = ",";

        /// <inheritdoc/>
        public void Log(Level leevel, string? message, Exception? exception = null)
        {
            // 引数チェック
            if (message == null) message = string.Empty; // 空メッセージ許容
            if (message.Contains(InvalidChar)) throw new ArgumentException($"{nameof(message)}:「{message}」には禁止文字「{InvalidChar}」が含まれています");

            // 呼び出し元の関数名を取得
            var methodName = new StackFrame(2).GetMethod()?.Name ?? string.Empty;

            // todo 実際のプロダクトで以下を実装
            // 実際のロギング
            // log4net、画面、コンソールなど
            var now = DateTime.Now;
            var exeMsg = exception is null ? string.Empty : exception.Message;
            var exeTrace = exception is null ? string.Empty : exception.StackTrace;

            // コンソール
            Console.WriteLine($"ログ出力, {leevel.ToString()}, {now}, {message}, {methodName}, {exeMsg}, {exeTrace}");
        }

        /// <summary> ログ書き込みの実施 </summary>
        /// <param name="leevel">ログレベル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="methodName">呼び出し元関数名</param>
        /// <param name="exception">記録したい例外</param>
        protected abstract void Write(Level leevel, string message, string methodName, Exception? exception = null);
    }
}
