using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Logger
{
    /// <summary> ダミーロガー </summary>
    /// <inheritdoc/>
    internal class DummyLogger : BaseLogger
    {
        /// <inheritdoc/>
        protected override void Write(Level leevel, string message, string methodName, Exception? exception = null) {
            // コンソールに出すだけ
            var now = DateTime.Now;
            var exeMsg = exception is null ? string.Empty : exception.Message;
            var exeTrace = exception is null ? string.Empty : exception.StackTrace;
            Console.WriteLine($"テスト用ログ, {leevel.ToString()}, {now}, {message}, {methodName}, {exeMsg}, {exeTrace}");
        }
    }
}
