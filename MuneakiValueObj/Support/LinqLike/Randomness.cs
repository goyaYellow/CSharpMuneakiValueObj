using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Support.LinqLike
{
    /// <summary> ランダムさを利用する機能を提供します </summary>
    public static class Randomness
    {
        private static readonly System.Random SysRandom = new();

        /// <summary> イテレータの要素をランダムに一つ取得します </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="source">対象となるイテレータ</param>
        /// <returns>ランダムに取得された値</returns>
        /// <exception cref="InvalidOperationException">イテレータが空の場合にスローされます</exception>
        public static T Random<T>(this IEnumerable<T> source)
        {
            if (source.Empty()) throw Error.Empty;
            return source.ElementAt(SysRandom.Next(0, source.Count()));
        }

        /// <summary> イテレータの要素を指定数分ランダムに取得します </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="source">対象となるイテレータ</param>
        /// <param name="count">取得したい数</param>
        /// <returns>ランダムに取得された値</returns>
        /// <exception cref="ArgumentOutOfRangeException">指定数がイテレータのサイズより大きい場合にスローされます</exception>
        public static IEnumerable<T> RandomTake<T>(this IEnumerable<T> source, int count)
        {
            if (source.Count() < count) throw Error.OutOfRange(source, count);

            // ランダムにソートして先頭からとる
            return source.OrderBy(x => Guid.NewGuid()).Take(count);
        }

        /// <summary> イテレータの要素の順番をランダムにして返します </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="source">対象となるイテレータ</param>
        /// <returns>順番をランダムにされた値</returns>
        public static IEnumerable<T> RandomAll<T>(this IEnumerable<T> source)
            => source.OrderBy(x => Guid.NewGuid());
    }
}