using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OriginalException;

namespace Tools.Support.LinqLike
{
    /// <summary> LINQっぽい使い勝手の拡張を提供します </summary>
    public static class Commons
    {
        /// <summary>　イテレータのメンバを文字列化して返します　</summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <param name="source">文字列化したいイテレータ</param>
        /// <param name="sepalater">セパレータ。デフォルトはカンマ。</param>
        /// <returns>文字列</returns>
        public static string AsString<TSource>(this IEnumerable<TSource?> source, char sepalater = '.')
        {
            var ret = "{ ";

            foreach (var x in source)
            {
                string str = x?.ToString() ?? "null";
                ret = ret + "{ " + str + " }" + sepalater + " ";
            }

            ret = ret.TrimEnd(' ').TrimEnd(sepalater);

            ret += " }";

            return ret;
        }

        /// <summary>　与えた条件式に 合致する or 合致しない 要素に分割して返します　</summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <param name="source">分割したいイテレータ</param>
        /// <param name="predicate">分割の条件式</param>
        /// <returns> Matched:条件式に合致する要素一覧、UnMatched:条件式に合致しない要素一覧</returns>
        public static (IEnumerable<TSource> Matched, IEnumerable<TSource> UnMatched) Sepalate<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            // 遅延評価のままだと結果がおかしくなる場合があるのでのでToList()で確定させる
            var matched = source.Where(predicate).ToList();
            var unmatched = source.Except(matched).ToList();
            return (matched, unmatched);
        }

        /// <summary> イテレータの値を用いてハッシュ値を生成します </summary>
        /// <remarks>
        /// <para> イテレータの実態（List or Dict など）が異なっても同じ値になります </para>
        /// <para> イテレータ内の値の格納順位が異なっても値がおなじなら、同じハッシュ値になります </para>
        /// </remarks>
        /// <typeparam name="TSource"> 型 </typeparam>
        /// <param name="source"> ハッシュ値を生成したいイテレータ </param>
        /// <returns> ハッシュ値 </returns>
        public static int CreteHashCode<TSource>(this IEnumerable<TSource> source)
        {
            /*
            参考： https://stackoverflow.com/questions/10567450/implement-gethashcode-for-objects-that-contain-collections/10567544#10567544
            機械翻訳Ver:　https://try2explore.com/questions/jp/10663974
            参考２: https://mocotan.hatenablog.com/entry/2017/10/31/064738
            　　　　　C#だとハッシュ値の合成のときにXOR（排他的論理和）使うのが一般的らしい
             */

            int hc = 0;
            foreach (TSource s in source)
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                hc ^= s.GetHashCode(); // 排他的論理和で合成

                // hc = (hc << 7) | (hc >> (32 - 7)); // ハッシュ値の被りリスクを減らすために、シフトしたあと論理和をとってる（っぽい）
            }

            return hc;
        }

        /// <summary> イシーケンス丸ごとDeepコピーします </summary>
        /// <remarks> <see cref="CommonExtentions.DeepCopy{T}(T)"/>を使用するので、同関数の注意が必要です </remarks>
        /// <typeparam name="TSource">型</typeparam>
        /// <param name="source">自分</param>
        /// <returns>等価:True、等価じゃない:False</returns>
        public static IEnumerable<TSource> SequenceDeepCopy<TSource>(this IEnumerable<TSource> source)
        {
            var ret = new List<TSource>();

            foreach (var s in source) ret.Add(s.DeepCopy());

            return ret;
        }

        #region カウント系

        /// <summary> 要素が空ならTruを返します </summary>
        /// <typeparam name="TSource"> 元ネタのイテレータのサイズ </typeparam>
        /// <param name="source"> 確認したいイテレータ </param>
        /// <returns> 空ならTrue, 要素有ならFalse </returns>
        public static bool Empty<TSource>(this IEnumerable<TSource> source) => !source.Any();

        /// <summary> 要素が空ならTruを返します </summary>
        /// <typeparam name="TSource">元ネタのイテレータのサイズ</typeparam>
        /// <param name="source">確認したいイテレータ</param>
        /// <param name="predicate">比較に使用する値の抽出関数</param>
        /// <returns> 空ならTrue, 要素有ならFalse </returns>
        public static bool Empty<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => !source.Any(predicate);

        /// <summary>　イテレータのサイズが1ならTrueを返します　</summary>
        /// <typeparam name="TSource">元ネタのイテレータのサイズ</typeparam>
        /// <param name="source">確認したいイテレータ</param>
        /// <returns> サイズが1ならTrue, 1以外ならFalse </returns>
        public static bool Only<TSource>(this IEnumerable<TSource> source)
        {
            // Count()を使うと、サイズがデカい時に遅くなるのでこう作る（Any（）の実装を参考にした
            using IEnumerator<TSource> enumerator = source.GetEnumerator();

            // MoveNexが1回しか出来なければTrue
            if (enumerator.MoveNext() is false) return false; // 1回目は成功して欲しい
            if (enumerator.MoveNext() is true) return false; // 2回目は失敗して欲しい
            return true;
        }

        /// <summary>　イテレータのサイズが1ならTrueを返します　</summary>
        /// <typeparam name="TSource">元ネタのイテレータのサイズ</typeparam>
        /// <param name="source">確認したいイテレータ</param>
        /// <param name="predicate">比較に使用する値の抽出関数</param>
        /// <returns> サイズが1ならTrue, 1以外ならFalse </returns>
        public static bool Only<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            // Count()を使うと、サイズがデカい時に遅くなるのでこう作る（Any（）の実装を参考にした
            using IEnumerator<TSource> enumerator = source.Where(predicate).GetEnumerator();

            // MoveNexが1回しか出来なければTrue
            if (enumerator.MoveNext() is false) return false; // 1回目は成功して欲しい
            if (enumerator.MoveNext() is true) return false; // 2回目は失敗して欲しい
            return true;
        }

        /// <summary>　イテレータのサイズが2以上ならTrueを返します　</summary>
        /// <typeparam name="TSource">元ネタのイテレータのサイズ</typeparam>
        /// <param name="source">確認したいイテレータ</param>
        /// <returns> サイズが2以上ならTrue, 2未満ならFalse </returns>
        public static bool Many<TSource>(this IEnumerable<TSource> source)
        {
            // Count()を使うと、サイズがデカい時に遅くなるのでこう作る（Any（）の実装を参考にした
            using IEnumerator<TSource> enumerator = source.GetEnumerator();

            // 2回MoveNex出来れば2つ以上ある
            enumerator.MoveNext();
            return enumerator.MoveNext();
        }

        /// <summary>　イテレータのサイズが2以上ならTrueを返します　</summary>
        /// <typeparam name="TSource">元ネタのイテレータのサイズ</typeparam>
        /// <param name="source">確認したいイテレータ</param>
        /// <param name="predicate">比較に使用する値の抽出関数</param>
        /// <returns> サイズが2以上ならTrue, 2未満ならFalse </returns>
        public static bool Many<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            // Count()を使うと、サイズがデカい時に遅くなるのでこう作る（Any（）の実装を参考にした
            using IEnumerator<TSource> enumerator = source.Where(predicate).GetEnumerator();

            // 2回MoveNex出来れば2つ以上ある
            enumerator.MoveNext();
            return enumerator.MoveNext();
        }

        #endregion

        #region 最大・最小の値を扱いたい系

        /// <summary> <typeparamref name="TKey"/>が最大の要素を返します </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <typeparam name="TKey">　比較に使用する値の型　</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="selector">比較に使用する値の抽出関数</param>
        /// <returns><typeparamref name="TKey"/>が最大の要素</returns>
        /// <exception cref="InvalidOperationException">要素が空だとスローします</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            if (source.Empty()) throw Error.Empty;

            // データ数が増えるとorderByは遅くなるので使わない
            var maxValue = source.Max(selector);
            return source.First(x => selector(x).Equals(maxValue));
        }

        /// <summary> <typeparamref name="TKey"/>が最小の要素を返します </summary>
        /// <typeparam name="TSource">元ネタのイテレータが持つ型</typeparam>
        /// <typeparam name="TKey">　比較に使用する値の型　</typeparam>
        /// <param name="source">重複を確認したいイテレータ</param>
        /// <param name="selector">比較に使用する値の抽出関数</param>
        /// <returns><typeparamref name="TKey"/>が最大の要素</returns>
        /// <exception cref="InvalidOperationException">要素が空だとスローします</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            if (source.Empty()) throw Error.Empty;

            // データ数が増えるとorderByは遅くなるので使わない
            var minValue = source.Min(selector);
            return source.First(x => selector(x).Equals(minValue));
        }

        #endregion
    }
}
