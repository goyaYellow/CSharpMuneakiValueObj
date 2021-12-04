using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using Tools.OriginalException;

namespace Tools.Support
{
    /// <summary> 汎用拡張メソッド </summary>
    public static class CommonExtentions
    {
        /// <summary>
        /// <para> インスタンスのメンバにNullが含まれるかを確認します </para>
        /// <para> 非Static＆Publicなフィールド＆プロパティについてチェックします </para>
        /// <para> 再起呼び出しにより,インスタンスの最下層までチェックします </para>
        /// </summary>
        /// <param name="obj">チェックしたいオブジェクト</param>
        /// <exception cref="NullReferenceException">Nullが存在した場合にスローします</exception>
        public static void AssertNotExistNull(this object? obj)
        {
            if (obj is null) throw new NullReferenceException($"{nameof(obj)} is null");

            var type = obj.GetType();

            try
            {
                // コレクションはキャストして比較
                // これしないと下のリフレクション参照でTargetParameterCountExceptionが出る
                if (obj is IEnumerable)
                {
                    foreach (var x in (IEnumerable)obj)
                        x.AssertNotExistNull();
                    return;
                }

                // フィールド編
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (var m in fields)
                {
                    var value = m.GetValue(obj);
                    if (value is null) throw new NullReferenceException($"{m.Name} is null");
                    value.AssertNotExistNull();
                }

                // プロパティ編
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var p in properties)
                {
                    var value = p.GetValue(obj);
                    if (value is null) throw new NullReferenceException($"{p.Name} is null");
                    value.AssertNotExistNull();
                }
            }
            catch (Exception e)
            {
                throw new NullReferenceException(e.Message + " -> " + type.Name, e);
            }
        }

        /// <summary> 正の値ならTrueを返す </summary>
        /// <param name="source"> チェックしたい値 </param>
        /// <returns> True：正の値、False：それ以外(0も含む) </returns>
        public static bool IsPositive(this int source)
            => source > 0;

        /// <summary> 負の値ならTrueを返す </summary>
        /// <param name="source"> チェックしたい値 </param>
        /// <returns> True：負の値、False：それ以外(0も含む)  </returns>
        public static bool IsNegative(this int source)
            => source < 0;

        /// <summary> 奇数ならTrueを返す </summary>
        /// <param name="source"> チェックしたい値 </param>
        /// <returns> True：奇数、False：それ以外(0も含む)  </returns>
        public static bool IsOdd(this int source)
            => source % 2 == 1;

        /// <summary> 偶数ならTrueを返す </summary>
        /// <param name="source"> チェックしたい値 </param>
        /// <returns> True：偶数、False：それ以外(0も含む)  </returns>
        public static bool IsEven(this int source)
            => source % 2 == 0;

        /// <summary> 正の値ならTrueを返す </summary>
        /// <param name="source"> チェックしたい値 </param>
        /// <returns> True：正の値、False：それ以外(0も含む)  </returns>
        public static bool IsPositive(this double source)
            => source > 0;

        /// <summary> 負の値ならTrueを返す </summary>
        /// <param name="source"> チェックしたい値 </param>
        /// <returns> True：負の値、False：それ以外(0も含む)  </returns>
        public static bool IsNegative(this double source)
            => source < 0;

        /// <summary> 小数の値が存在すればTrueを返す </summary>
        /// <param name="source"> チェックしたい値 </param>
        /// <returns> True：小数の値が存在する、False：存在しない </returns>
        public static bool HasDecimal(this double source)
            => (source % 1) > 0;

        /// <summary>　小数値を整数に繰上変換します　</summary>
        /// <remarks>
        /// <para> ①ソースの整数部は無視されます </para>
        /// <para> ②対象範囲より下の位の値は完全切り捨てされます </para>
        /// </remarks>
        /// <param name="source"> 小数部を変換したい値 </param>
        /// <param name="digitsNumber"> 繰り上げ桁数。デフォルト値は３桁で、これは 0.500sec->500msec変換 等に対応している </param>
        /// <returns> 繰り上げ返還後の値 </returns>
        /// <exception cref="ArgumentException">繰り上げ桁数指定が不正時にスロー</exception>
        public static int ConvertDecimalToInteger(this double source, int digitsNumber = 3)
        {
            if (digitsNumber < 0) throw new ArgumentException($"桁数は自然数である必要があります。 {nameof(digitsNumber)} {digitsNumber}");
            var coefficient = (int)Math.Pow(10, digitsNumber);
            return ((int)((Math.Abs(source) * coefficient) / 1)) % coefficient;
        }

        /// <summary>　<see cref="string.IsNullOrEmpty(string?)"/>のインスタンス関数風版　</summary>
        /// <param name="source"> チェックしたい文字列 </param>
        /// <returns>　Nullか空文字ならTrue、それ以外はFalse </returns>
        public static bool IsNullOrEmpty(this string? source)
            => string.IsNullOrEmpty(source);

        /// <summary>　<see cref="string.IsNullOrWhiteSpace(string?)"/>のインスタンス関数風版　</summary>
        /// <param name="source"> チェックしたい文字列 </param>
        /// <returns>　Null・空文字・スペースだけのいづれかならTrue、それ以外はFalse </returns>
        public static bool IsNullOrWhiteSpace(this string? source)
            => string.IsNullOrWhiteSpace(source);

        /// <summary>　セパレータで指定された範囲内において、文字を置換します　</summary>
        /// <param name="source">元ネタとなる文字列</param>
        /// <param name="sepalater">セパレータ</param>
        /// <param name="oldChr">置換対象の文字</param>
        /// <param name="newChr">置換したい文字</param>
        /// <returns>置換後の文字列</returns>
        public static string ReplaceInRange(this string source, char sepalater, char oldChr, char newChr)
        {
            var splited = source.Split(sepalater);
            foreach (int i in Enumerable.Range(0, splited.Length))
            {
                if (i % 2 == 1)
                    splited[i] = splited[i].Replace(oldChr, newChr);
            }

            return string.Join(sepalater, splited);
        }

        /// <summary> ディープコピーをします </summary>
        /// <remarks>
        /// <para> ① 処理速度めっちゃ遅い </para>
        /// <para> ② JSON変換がうまくいかないオブジェクトでは失敗します </para>
        /// </remarks>
        /// <typeparam name="T"> 型 </typeparam>
        /// <param name="soruce"> コピーしたい元ネタ </param>
        /// <returns> ディープコピーしたオブジェクト </returns>
        public static T DeepCopy<T>(this T soruce)
        {
            /* jsonへのシリアライズ→デシリアライズを通してディープコピー */

            // 設定
            JsonSerializerOptions jsonOptions = new() {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }, // Enumを文字としてJson化する
            };

            // シリアライズ
            var jsonText = JsonSerializer.Serialize<T>(soruce, jsonOptions);

            // デシリアライズ
            T ret = JsonSerializer.Deserialize<T>(jsonText)
                ?? throw new InvalidOperationException("デシリアライズ出来ないオブジェクトだよー");

            return ret;
        }
    }
}
