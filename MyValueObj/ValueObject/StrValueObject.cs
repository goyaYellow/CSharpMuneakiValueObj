using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OriginalException;

namespace Tools.ValueObject
{
    /// <summary>　Int型の値を一つだけ持つ値オブジェクト用の抽象クラス。IComparable等をを提供します。 </summary>
    /// <typeparam name="TInherited">こいつを継承したやつが自分の型を渡すべし。自分以外の型を渡した場合は演算子などのエラーチェックで引っかかります</typeparam>
    public abstract record StrValueObject<TInherited> : IComparable<TInherited>
       where TInherited : StrValueObject<TInherited>
    {
        /// <summary> 値 </summary>
        public string Value { get; }

        /// <summary> Initializes a new instance of the <see cref="StrValueObject{Inherited}"/> class. </summary>
        /// <param name="value">値</param>
        public StrValueObject(string value)
            => this.Value = value;

        /// <summary> 保持している値を文字列として返します </summary>
        /// <returns> 値 </returns>
        public string AsString() => this.Value.ToString();

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
