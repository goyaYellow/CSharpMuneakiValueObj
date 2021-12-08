using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MuneakiValueObject;

namespace Tools_Test.ValueObject
{
    /// <summary> <see cref="IntValueObject{Inherited} "/>に対するテストです </summary>
    public class IntValueObject_Test
    {
        /// <summary> <see cref="IntValueObject{Inherited}.IntValueObject(int)"/>に対するテストです </summary>
        public class 正常にインスタンス化できる
        {
            [Fact]
            public void 正常な値をコンストラクタに渡すとインスタンスが返る()
            {
                var value = 10;
                var actual = new InharitedClass(value);

                Assert.IsType<InharitedClass>(actual);
                Assert.Equal(value.ToString(), actual.AsString());
            }
        }

        /// <summary>　<see cref="IntValueObject{TInherited}.Any"/> に対するテストです　</summary>
        public class 値が1以上であるか確認できる
        {
            [Fact]
            public void 値が１以上ならTrueを返す() {
                Assert.True(new InharitedClass(1).Any());
                Assert.True(new InharitedClass(int.MaxValue).Any());
            }

            [Fact]
            public void 値が１未満ならFalseを返す()
            {
                Assert.False(new InharitedClass(-1).Any());
                Assert.False(new InharitedClass(int.MinValue).Any());
                Assert.False(new InharitedClass(0).Any());
            }
        }

        public class 規定関数のオーバーライドやIF実装に対するテスト
        {
            public class Eqalsが使える
            {
                [Fact]
                public void 同値なオブジェクトを渡すとTrueを返す()
                {
                    var value = 11;
                    var ins = new InharitedClass(value);
                    var alt = new InharitedClass(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.True(ins.Equals(ins));
                    Assert.True(ins.Equals(alt));
                    Assert.True(ins.Equals((object)alt));
                }

                [Fact]
                public void 同地でないオブジェクトを渡すとFalseを返す()
                {
                    var ins = new InharitedClass(11);
                    var alt = new InharitedClass(12);
                    var actual = ins.Equals(alt);

                    Assert.False(actual);
                }

                [Fact]
                public void nullや異なる型のオブジェクトを渡すとFalseを返す()
                {
                    var value = 12;
                    var ins = new InharitedClass(value);

                    Assert.False(ins.Equals(value));
                    Assert.False(ins.Equals(new InharitedClassAltanative(value)));
                    Assert.False(ins.Equals(null));
                }
            }

            public class GetHashCodeが使える
            {
                [Fact]
                public void 同値なオブジェクトでは同じハッシュを返す()
                {
                    var value = 123;
                    var ins = new InharitedClass(value);
                    var alt = new InharitedClass(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.Equal(ins.GetHashCode(), alt.GetHashCode());
                    Assert.Equal(ins.GetHashCode(), ins.GetHashCode());
                }

                [Fact]
                public void 同値ではないオブジェクトでは異なるハッシュを返す()
                {
                    var ins = new InharitedClass(11);
                    var alt = new InharitedClass(12);

                    Assert.NotEqual(ins.GetHashCode(), alt.GetHashCode());
                }
            }

            /// <summary> <see cref="InharitedClass.CompareTo(InharitedClass)"/>に対するテストです </summary>
            public class ソートできる
            {
                [Fact]
                public void ソートした場合に値が小さい順位に並ぶ()
                {
                    // 準備
                    InharitedClass[] expected = {
                        new InharitedClass(10),
                        new InharitedClass(20),
                        new InharitedClass(20),
                        new InharitedClass(30),
                        };
                    var actual = new List<InharitedClass> {
                        expected[2],
                        expected[3],
                        expected[0],
                        expected[1],
                        };

                    // 実行
                    actual.Sort();

                    // 検証
                    for (int i = 0; i < actual.Count; i++)
                    {
                        Assert.Equal(expected[i], actual[i]);
                    }
                }
            }

            /// <summary> Inharited1のオペレータに対するテストです </summary>
            public class 演算子にて比較できる
            {
                [Fact]
                public void 期待した比較結果がでる()
                {
                    // 準備
                    var smaller = new InharitedClass(10);
                    var smaller_alt = new InharitedClass(10);
                    var bigger = new InharitedClass(20);

                    Console.WriteLine(new InharitedClass(10) > new InharitedClass(11));

                    // 実行と検証
                    Assert.True(smaller < bigger);
                    Assert.False(bigger < smaller);
                    Assert.True(smaller <= bigger);
                    Assert.False(bigger <= smaller);
                    Assert.True(smaller <= smaller_alt);
                    Assert.True(bigger > smaller);
                    Assert.False(smaller > bigger);
                    Assert.True(bigger >= smaller);
                    Assert.False(smaller >= bigger);
                    Assert.True(smaller >= smaller_alt);
                    Assert.True(smaller == smaller_alt);
                    Assert.False(smaller == bigger);
                    Assert.True(smaller != bigger);
                    Assert.False(smaller != smaller_alt);
                }
            }
        }

        /// <summary> <see cref="IntValueObject{Inherited}"/>を継承した、テスト用の仮想のクラス </summary>
        public record InharitedClass : IntValueObject<InharitedClass>
        {
            public InharitedClass(int value) : base(value)
            {
            }
        }

        /// <summary>  テスト用の仮想のクラスの別Ver </summary>
        public record InharitedClassAltanative : IntValueObject<InharitedClassAltanative>
        {
            public InharitedClassAltanative(int value) : base(value)
            {
            }
        }
    }
}
