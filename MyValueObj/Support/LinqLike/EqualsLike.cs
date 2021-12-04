using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OriginalException;

namespace Tools.Support.LinqLike
{
    /// <summary> SequenceEqualっぽい機能を提供します </summary>
    public static class EqualsLike
    {
        /// <summary> 多次元コレクションに対する<see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>です </summary>
        /// <remarks> イテレータでなはい要素が出現した時点で再起呼び出しを打ち切ります。つまり、Dict・Group等があった場合に、その値メンバまでは展開されません。 </remarks>
        /// <typeparam name="TSource">比較したいリストが持つ型</typeparam>
        /// <param name="source">自分</param>
        /// <param name="other">比較相手</param>
        /// <returns>等価:True、等価じゃない:False</returns>
        public static bool SequenceEqualNestable<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource>? other)
        {
            /// <see cref="IEquatable{T}.Equals(T?)"/>で使えるようにしたいので、引数にNullを許容してここでNullチェック
            if (other is null) return false;

            if (ReferenceEquals(source, other)) return true;
            if (source.Count() != other.Count()) return false;
            if (source.Empty()) return true;

            var isIterator = source.First() is IEnumerable;
            var isNotStr = source.First() is not string; // 文字列は分解する必要なし

            if (isIterator && isNotStr)
            {
                // 要素がイテレータなら分解して再起呼び出し
                var pair = source.Zip(other, (s, o) // Countが同じなのは上で確認済み
                    => new {
                        // 要素が値型の場合直接キャストすると失敗するので,一度IEnumerable化してCast関数を通しています
                        myElement = (s as IEnumerable)?.Cast<object>() ?? throw JustBugException.Template,
                        othersElement = (o as IEnumerable)?.Cast<object>() ?? throw JustBugException.Template,
                    });
                foreach (var element in pair)
                {
                    if (!element.myElement.SequenceEqualNestable(element.othersElement))
                        return false;
                }

                return true;
            }
            else
            {
                // 要素がイテレータじゃないなら普通にSeqEqal
                return source.SequenceEqual(other);
            }
        }

        /// <summary> 要素の順番が一致しなくてもOKな<see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>です </summary>
        /// <remarks> 要素がコレクションだった場合にさらに展開したりはしません。めんどくさいから。 </remarks>
        /// <typeparam name="TSource">比較したいリストが持つ型</typeparam>
        /// <param name="source">自分</param>
        /// <param name="other">比較相手</param>
        /// <returns>等価:True、等価じゃない:False</returns>
        public static bool SequenceEqualOrderless<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource>? other)
        {
            /// <see cref="IEquatable{T}.Equals(T?)"/>で使えるようにしたいので、引数にNullを許容してここでNullチェック
            if (other is null) return false;

            if (ReferenceEquals(source, other)) return true;
            if (source.Count() != other.Count()) return false;
            if (source.Empty()) return true;

            return source.SequenceContains(other); // サイズが同じ事チェック済みなので、含有でも全比較になる
        }

        /// <summary> 自分に比較対象が全て含まれている場合にTrueを返します </summary>
        /// <remarks> 重複する要素も分けて確認します（比較対象に"X"が2つあれば自分にも"X"が2つ以上ないとFalse） </remarks>
        /// <typeparam name="TSource">型</typeparam>
        /// <param name="source">自分</param>
        /// <param name="other">比較相手</param>
        /// <returns>全て含まれている:True、含まれていないものがいる:False</returns>
        public static bool SequenceContains<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource>? other)
        {
            if (other is null) return true;
            if (other.Empty()) return true;

            if (ReferenceEquals(source, other)) return true;
            if (source.Count() < other.Count()) return false;

            List<TSource> soruceList = source.ToList(); // 副作用対策として別インスタンス化
            try
            {
                foreach (var s in other)
                {
                    var data = soruceList.First(x => x?.Equals(s) ?? throw JustBugException.Template);

                    // データ量が多い時のために消しておく(データ量が少ない時は逆に遅くなるだろうなー)
                    soruceList.Remove(data);
                }
            }
            catch
            {
                return false; // First見つからなかったらFalse
            }

            return true;
        }
    }
}
