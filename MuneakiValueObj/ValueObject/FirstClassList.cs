using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Support.LinqLike;
using Tools.OriginalException;

#pragma warning disable SA1202 // Elements should be ordered by access

namespace Tools.ValueObject
{
    /// <summary>　ファーストクラスコレクション（リスト）のベース　</summary>
    /// <typeparam name="TValue">リスト格納値の型</typeparam>
    /// <typeparam name="TInherited">このレコードを継承した型</typeparam>
    public abstract class FirstClassList<TValue, TInherited> : IEnumerable<TValue>, IEquatable<TInherited>
         where TInherited : FirstClassList<TValue, TInherited>
    {
        /// <summary> <see cref="Values"/>の値保持君。直接触るなよ（警告） </summary>
        private ImmutableList<TValue> values;

        /// <summary>　値　</summary>
        public IList<TValue> Values {
            protected get => this.values; // 集約ルートを守るためにprotected
            init {
                this.values = this.PreInite(value).ToImmutableList();
            }
        }

        /// <summary> インデクサ </summary>
        /// <param name="index">インデックス</param>
        /// <returns>指定されたインデックスに格納されていた値</returns>
        public TValue this[int index] {
            get { return this.Values[index]; }
        }

        #region 初期化系

        /// <summary> Initializes a new instance of the <see cref="FirstClassList{TValue, TInherited}"/> class. </summary>
        /// <param name="values">値</param>
        public FirstClassList(IList<TValue> values)
        {
            this.values = ImmutableList.Create<TValue>();
            this.Values = values;
        }

        /// <summary>　値セットの前処理を行う（値チェックや整形・ソートなど） </summary>
        /// <remarks> 継承先で実現したい前処理を実装してください </remarks>
        /// <param name="value"> チェック＆整形したい値 </param>
        /// <returns> 前処理後の値。この値をインスタンス変数として持ちます </returns>
        /// <exception cref="ArgumentException">値がルール外であればスローしてください</exception>
        protected abstract IList<TValue> PreInite(IList<TValue> value);

        #endregion

        #region C#規定関数のオーバーライドやIF実装

        /// <inheritdoc/>
        public bool Equals(TInherited? other)
            => other is null ? false : this.Values.SequenceEqual(other.Values);

        /// <inheritdoc/>
        public override int GetHashCode() => this.Values.CreteHashCode();

        /// <inheritdoc/>
        public override bool Equals(object? other) => other is TInherited inherited && this.Equals(inherited);

        /// <inheritdoc/>
        public override string ToString() => $"{this.GetType().Name} {this.Values.AsString()}";  // TODO C#10 になったらsealedをつける => 継承先がrecordの場合、自動生成関数に上書きされちゃうから

        /// <inheritdoc/>
        public IEnumerator<TValue> GetEnumerator() => this.Values.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion
    }
}

#pragma warning restore SA1202 // Elements should be ordered by access
