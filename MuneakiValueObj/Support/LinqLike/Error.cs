using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Support.LinqLike
{
    /// <summary>　エラー定義 </summary>
    　internal static class Error
    {
        /// <summary> カウントが0だといけないのに0のイテレータを渡された場合のテンプレ </summary>
        public static　readonly InvalidOperationException Empty = new InvalidOperationException("空のリストはダメです");

        /// <summary> イテレータのレンジを超えている数が指定された場合のテンプレ </summary>
        /// <param name="source">イテレータ</param>
        /// <param name="count">指定された数</param>
        /// <typeparam name="T">型</typeparam>
        /// <returns>テンプレ</returns>
        public static ArgumentOutOfRangeException OutOfRange<T>(IEnumerable<T> source, int count)
            => new ArgumentOutOfRangeException($"引数の指定数がイテレータのレンジを超えています。イテレータのレンジ:{source.Count()}。指定数:{count}。");
    }
}
