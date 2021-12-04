using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Logger
{
    /// <summary> ログレベル </summary>
    /// <remarks>
    /// <para> log4netのログレベルを踏襲しつつ,<see cref="Level.Operation"/>を追加 2021/2/1 somei </para>
    /// <para> 参照｜https://qiita.com/nanasess/items/350e59b29cceb2f122b3 </para>
    /// </remarks>
    public enum Level
    {
        /// <summary> システムの動作状況に関する詳細な情報 </summary>
        Debug = 0,

        /// <summary> 実行時の何らかの注目すべき事象（開始や終了など） </summary>
        Info,

        /// <summary> ユーザの操作を記録する。ユーザの「なにもしてないのに壊れた(笑)」を防止する </summary>
        Operation,

        /// <summary> 廃要素となったAPIの使用、APIの不適切な使用、エラーに近い事象など。実行時に生じた異常とは言い切れないが正常とも異なる何らかの予期しない問題 </summary>
        Warn,

        /// <summary> 予期しないその他の実行時エラー。例外など。 </summary>
        Error,

        /// <summary> プログラムの異常終了を伴うようなもの。 </summary>
        Fatal,
    }
}
