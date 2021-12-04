using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Support.LinqLike
{
    /// <summary>　<see cref="Enumerable"/>っぽい使い勝手の機能を提供します　</summary>
    public static class EnumerableLike
    {
        /// <summary>　指定された始点-終点間の連番を返します　</summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <returns>指定された範囲の連番</returns>
        /// <exception cref="ArgumentException">指定範囲がおかしい場合にスローします</exception>
        public static IEnumerable<int> RangeTo(int start, int end)
        {
            var count = end - start;
            if (count.IsNegative()) throw new ArgumentException($"{end}が{start}より前です。");
            return Enumerable.Range(start, count);
        }

        /// <summary>　0-指定された終点間の連番を返します　</summary>
        /// <param name="end">終点</param>
        /// <returns>指定された範囲の連番</returns>
        /// <exception cref="ArgumentException">指定範囲がおかしい場合にスローします</exception>
        public static IEnumerable<int> RangeTo(int end)
            => RangeTo(0, end);
    }
}
