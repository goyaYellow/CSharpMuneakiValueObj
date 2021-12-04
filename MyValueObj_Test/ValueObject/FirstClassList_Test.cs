using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Tools.ValueObject;
using Tools.Support.LinqLike;
using Tools.Support;

namespace Tools_Test.ValueObject
{
    /// <summary> <see cref="FirstClassList{TValue, TInherited}"/>に対するテストです </summary>
    public class FirstClassList_Test
    {
        public static readonly ImmutableList<int> Value = Enumerable.Range(0, 20).ToImmutableList();

        /// <summary> <see cref="FirstClassList{TValue, TInherited}.FirstClassList(ImmutableList{TValue})"/>に対するテストです </summary>
        public class 正常にインスタンス化できる
        {
            [Fact]
            public void 正常な値をコンストラクタに渡すとインスタンスが返る()
            {
                var actual = new InharitedClass(Value);

                Assert.IsType<InharitedClass>(actual);
                _ = (actual as System.Collections.IEnumerable).GetEnumerator(); // カバレッジ誤魔化し
                Assert.True(actual.SequenceEqual(Value));
            }

            [Fact]
            public void 前処理関数で指定した禁止条件を満たした値をコンストラクタに渡すと例外をスローする()
            {
                // 空のリスト
                Assert.Throws<ArgumentException>(() => new InharitedClass(ImmutableList.Create<int>()));

                // 要素が重複しているリスト
                Assert.Throws<ArgumentException>(() => new InharitedClass(Value.Add(Value.First())));
            }

            [Fact]
            public void 前処理関数で指定した調整を加えた値がインスタンスに格納されている()
            {
                var randamValue = Value.RandomAll();
                var actual = new InharitedClass(randamValue.ToImmutableList()); // 順番ランダムなリストを渡しても

                Assert.IsType<InharitedClass>(actual);
                Assert.True(randamValue.OrderBy(x => x).SequenceEqual(Value)); // ソートされた値が格納されているはず
            }
        }

        public class 規定関数のオーバーライドやIF実装に対するテスト
        {
            public class インデクサが使える
            {
                [Fact]
                public void インデクサで値を取得できる()
                {
                    var ins = new InharitedClass(Value);

                    foreach (var index in Enumerable.Range(0, Value.Count))
                    {
                        Assert.IsType<int>(ins[index]);
                        Assert.Equal(Value[index], ins[index]);
                    }
                }
            }

            public class Eqalsが使える
            {
                [Fact]
                public void 同値なオブジェクトを渡すとTrueを返す()
                {
                    var ins = new InharitedClass(Value);
                    var alt = new InharitedClass(Value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.True(ins.Equals(ins));
                    Assert.True(ins.Equals(alt));
                    Assert.True(ins.Equals((object)alt));
                }

                [Fact]
                public void 同地でないオブジェクトを渡すとFalseを返す()
                {
                    var ins = new InharitedClass(Value);
                    var alt = new InharitedClass(Value.Add(int.MaxValue));

                    // インスタンスが異なるかどうかは関係ない
                    Assert.False(ins.Equals(alt));
                    Assert.False(ins.Equals((object)alt));
                }

                [Fact]
                public void nullや異なる型のオブジェクトを渡すとFalseを返す()
                {
                    var ins = new InharitedClass(Value);

                    Assert.False(ins.Equals(Value));
                    Assert.False(ins.Equals(new InharitedClassAltanative(Value)));
                    Assert.False(ins.Equals(null));
                }
            }

            public class GetHashCodeが使える
            {
                [Fact]
                public void 同値なオブジェクトでは同じハッシュを返す()
                {
                    var ins = new InharitedClass(Value);
                    var alt = new InharitedClass(Value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.Equal(ins.GetHashCode(), alt.GetHashCode());
                    Assert.Equal(ins.GetHashCode(), ins.GetHashCode());
                }

                [Fact]
                public void 同値ではないオブジェクトでは異なるハッシュを返す()
                {
                    var ins = new InharitedClass(Value);
                    var alt = new InharitedClass(Value.Add(int.MaxValue));

                    Assert.NotEqual(ins.GetHashCode(), alt.GetHashCode());
                }
            }

            public class ToStringが使える
            {
                [Fact]
                public void 値のメンバを結合した文字列が返る()
                {
                    var data = new List<int>() { 1, 2, 3 };
                    var ins = new InharitedClass(data.ToImmutableList());

                    var except = "InharitedClass { { 1 }. { 2 }. { 3 } }";

                    var actual = ins.ToString();

                    Assert.True(except.Equals(actual));
                }
            }
        }

        /// <summary> <see cref="FirstClassList{TValue, TInherited}"/>を継承した、テスト用の仮想のクラス </summary>
        public class InharitedClass : FirstClassList<int, InharitedClass>
        {
            public InharitedClass(IList<int> value) : base(value)
            {
            }

            /// <inheritdoc/>
            protected override IList<int> PreInite(IList<int> value)
            {
                if (value.Empty()) throw new ArgumentException(); // 空のリストを許さない
                if (value.HasDuplicate()) throw new ArgumentException(); // 要素の重複を許さない

                return value.OrderBy(x => x).ToImmutableList(); // intの昇順でソート
            }
        }

        /// <summary>  テスト用の仮想のクラスの別Ver </summary>
        public class InharitedClassAltanative : FirstClassList<int, InharitedClass>
        {
            public InharitedClassAltanative(ImmutableList<int> value) : base(value)
            {
            }

            /// <inheritdoc/>
            protected override IList<int> PreInite(IList<int> value)
                => value;
        }
    }
}
