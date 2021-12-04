using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OriginalException;

namespace Tools.Support.LinqLike
{
    /// <summary> 要素の重複を扱う機能を提供します </summary>
    public static class Duplication
    {

        /// <summary> 重複があればTrueを返します </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <returns>重複あり:True, なし：False</returns>
        public static bool HasDuplicate<TSource>(this IEnumerable<TSource> source)
            where TSource : IEquatable<TSource>
        {
            /* Distict(の実装を参考にしてます */

            if (source.Empty()) return false; // 空ならFalseで終わり

            var set = new HashSet<TSource>();

            // ハッシュセットに追加できる？
            foreach (var item in source)
                if (!set.Add(item)) return true;

            return false;
        }

        /// <summary> 重複があればTrueを返します（条件指定Ver） </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="predicate">等価性評価に使用する値の抽出関数</param>
        /// <returns>重複あり:True, なし：False</returns>
        public static bool HasDuplicate<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            where TSource : IEquatable<TSource>
            => source.Where(predicate).HasDuplicate();

        /// <summary> 重複があればTrueを返します(要素指定Ver) </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <typeparam name="TKey">チェックに用いる値の型</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="selector">等価性評価に使用する値の抽出関数</param>
        /// <returns>重複あり:True, なし：False</returns>
        public static bool HasDuplicateBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IEquatable<TKey>
            => source.Select(selector).HasDuplicate();

        /// <summary> 異なる値があればTrueを返します </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <returns> 異なる値あり:True, なし：False </returns>
        public static bool HasDifference<TSource>(this IEnumerable<TSource> source)
            where TSource : IEquatable<TSource>
        {
            /* Distict(の実装を参考にしてます */

            if (source.Empty()) return false; // 空ならFalseで終わり

            // ハッシュセットに追加できる値があれば即False
            var set = new HashSet<TSource>();
            set.Add(source.First());
            foreach (var item in source.Skip(1))
                if (set.Add(item)) return true;

            return false;
        }

        /// <summary> 異なる値があればTrueを返します(条件指定Ver) </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="predicate">等価性評価に使用する値の抽出関数</param>
        /// <returns> 異なる値あり:True, なし：False </returns>
        public static bool HasDifference<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            where TSource : IEquatable<TSource>
             => source.Where(predicate).HasDifference();

        /// <summary> 異なる値があればTrueを返します(要素指定Ver) </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <typeparam name="TKey">チェックに用いる値の型</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="selector">等価性評価に使用する値の抽出関数</param>
        /// <returns> 異なる値あり:True, なし：False </returns>
        public static bool HasDifferenceBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IEquatable<TKey>
            => source.Select(selector).HasDifference();

        /// <summary> 重複している要素の一覧（重複なし）を返します </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <returns>重複している要素の一覧（重複なし）</returns>
        public static IEnumerable<TSource> FindDeplicated<TSource>(this IEnumerable<TSource> source)
            where TSource : IEquatable<TSource>
        {
            // GroupByした後、グループ内の件数が2以上（つまり重複あり）に絞り込み返す
            return source.GroupBy(x => x).Where(x => x.Many()).Select(group => group.First());
        }

        /// <summary> <typeparamref name="TKey"/>が重複している要素の一覧（重複なし）を返します </summary>
        /// <remarks> 返るのは重複キーが同じ要素達の内、先頭の物のみになります </remarks>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <typeparam name="TKey">等価性評価に使用する値の形</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="selector">比較に使用する値の抽出関数</param>
        /// <returns><typeparamref name="TKey"/>が重複している要素の一覧（重複なし）</returns>
        public static IEnumerable<TSource> FindDeplicatedBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IEquatable<TKey>
        {
            // GroupByした後、グループ内の件数が2以上（つまり重複あり）に絞り込み返す
            return source.GroupBy(selector).Where(x => x.Many()).Select(group => group.First());
        }

        /// <summary> <see cref="Enumerable.Distinct{TSource}(IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>のセレクター使用版 </summary>
        /// <remarks> 返るのは重複キーが同じ要素達の内、先頭の物のみになります </remarks>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <typeparam name="TKey">等価性評価に使用する値の形</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="selector">比較に使用する値の抽出関数</param>
        /// <returns><typeparamref name="TKey"/>が重複している要素の一覧（重複なし）</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IEquatable<TKey>
        {
            // SelectorでGroupByした後、グループ内の最初のメンバを抽出して返す
            return source.GroupBy(selector).Select(x => x.First());
        }
    }
}
