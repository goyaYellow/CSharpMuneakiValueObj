using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OriginalException;
using Tools.Support;

namespace Tools.ValueObject
{
    /// <summary>　Int型の値を一つだけ持つ値オブジェクト用の抽象レコード。IComparableや演算子を提供します。 </summary>
    /// <typeparam name="TInherited">こいつを継承したやつが自分の型を渡すべし。自分以外の型を渡した場合は演算子などのエラーチェックで引っかかります</typeparam>
    public abstract record IntValueObject<TInherited> : IComparable<TInherited>
       where TInherited : IntValueObject<TInherited>
    {
        /// <summary>　値 </summary>
        public int Value { get; }

        /// <summary> Initializes a new instance of the <see cref="IntValueObject{Inherited}"/> class. </summary>
        /// <param name="value">値</param>
        public IntValueObject(int value)
            => this.Value = value;

        /// <summary> 保持している値を文字列として返します </summary>
        /// <returns> 値 </returns>
        public string AsString() => this.Value.ToString();

        /// <summary>値が１以上の場合、trueを返します</summary>
        /// <returns>true:値が１以上 false:それ以外</returns>
        public bool Any() => this.Value.IsPositive();

        #region C#規定関数のオーバーライドやIF実装

        /// <inheritdoc/>
        public int CompareTo(TInherited? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (this < other) return -1;
            if (this > other) return 1;
            return 0;
        }
        #endregion

        #region 演算子定義 同一性ではなく同値性を評価します
        public static bool operator <(IntValueObject<TInherited> x, TInherited y)
        {
            var castedX = x as TInherited ?? throw new ArgumentException($"不正なタイプと比較されそうになりました。I am {y.GetType().Name}");
            return castedX.Value < y.Value;
        }

        public static bool operator >(IntValueObject<TInherited> x, TInherited y) => (x != y) && !(x < y);

        public static bool operator >=(IntValueObject<TInherited> x, TInherited y) => (x == y) || (x > y);

        public static bool operator <=(IntValueObject<TInherited> x, TInherited y) => (x == y) || (x < y);
        #endregion
    }
}
