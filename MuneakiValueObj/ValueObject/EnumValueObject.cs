using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.ValueObject
{
    /// <summary>　Enum型の値を一つだけ持つ値オブジェクト用の抽象クラス。IComparable,IEquatableや演算子を提供します。 </summary>
    /// <typeparam name="TEnum">値のEnum型</typeparam>
    /// <typeparam name="TInherited">こいつを継承したやつが自分の型を渡すべし。自分以外の型を渡した場合は演算子などのエラーチェックで引っかかります</typeparam>
    public abstract record EnumValueObject<TEnum, TInherited> : IComparable<TInherited>
        where TEnum : struct, Enum
        where TInherited : EnumValueObject<TEnum, TInherited>
    {
        /// <summary>　値　</summary>
        public TEnum Value { get; }

        /// <summary> Initializes a new instance of the <see cref="EnumValueObject{TEnum, TInherited}"/> class. </summary>
        /// <param name="value"> 値 </param>
        public EnumValueObject(TEnum value)
        {
            if (!Enum.IsDefined<TEnum>(value)) throw new ArgumentException($"未定義の列挙である{value}が渡されました");
            this.Value = value;
        }

        /// <summary>値をstringにして返します</summary>
        /// <returns>値の文字列</returns>
        public string AsString() => this.Value.ToString();

        /// <summary>値をintの文字列にして返します</summary>
        /// <returns>値の文字列</returns>
        public string ASIntString() => this.Value.ToString("d");

        /// <summary>値をintにして返します</summary>
        /// <returns>値の文字列</returns>
        public int ASInt() => int.Parse(this.Value.ToString("d"));

        #region C#規定関数のオーバーライドやIF実装

        /// <inheritdoc/>
        public int CompareTo(TInherited? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return this.Value.CompareTo(other.Value);
        }
        #endregion
    }
}
