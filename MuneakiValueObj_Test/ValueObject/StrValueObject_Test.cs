using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Tools.ValueObject;
using Tools.OriginalException;

namespace Tools_Test.ValueObject
{
    /// <summary> <see cref="StrValueObject{Inherited}"/>に対するテストです </summary>
    public class StrValueObject_Test
    {
        /// <summary> <see cref="StrValueObject{Inherited}.StrValueObject(string)"/>に対するテストです </summary>
        public class 正常にインスタンス化できる
        {
            [Fact]
            public void 正常な値をコンストラクタに渡すとインスタンスが返る()
            {
                var value = "Test";
                var actual = new InharitedClass(value);

                Assert.IsType<InharitedClass>(actual);
                Assert.Equal(value.ToString(), actual.AsString());
            }
        }

        public class 規定関数のオーバーライドやIF実装に対するテスト
        {
            public class Eqalsが使える
            {
                [Fact]
                public void 同値なオブジェクトを渡すとTrueを返す()
                {
                    var value = "Test";
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
                    var ins = new InharitedClass("Test1");
                    var alt = new InharitedClass("Test2");
                    var actual = ins.Equals(alt);

                    Assert.False(actual);
                }

                [Fact]
                public void nullや異なる型のオブジェクトを渡すとFalseを返す()
                {
                    var value = "Test";
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
                    var value = "Test";
                    var ins = new InharitedClass(value);
                    var alt = new InharitedClass(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.Equal(ins.GetHashCode(), alt.GetHashCode());
                    Assert.Equal(ins.GetHashCode(), ins.GetHashCode());
                }

                [Fact]
                public void 同値ではないオブジェクトでは異なるハッシュを返す()
                {
                    var ins = new InharitedClass("Test1");
                    var alt = new InharitedClass("Test2");

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
                        new InharitedClass("A"),
                        new InharitedClass("B"),
                        new InharitedClass("B"),
                        new InharitedClass("C"),
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
                    var ins = new InharitedClass("Test1");
                    var another = new InharitedClass("Test1");
                    var alt = new InharitedClass("Test2");

                    // 実行と検証
                    Assert.True(ins == another);
                    Assert.False(ins == alt);
                    Assert.True(ins != alt);
                    Assert.False(ins != another);
                }
            }
        }

        /// <summary> <see cref="IntValueObject{Inherited}"/>を継承した、テスト用の仮想のクラス </summary>
        public record InharitedClass : StrValueObject<InharitedClass>
        {
            public InharitedClass(string value) : base(value)
            {
            }
        }

        /// <summary>  テスト用の仮想のクラスの別Ver </summary>
        public record InharitedClassAltanative : StrValueObject<InharitedClassAltanative>
        {
            public InharitedClassAltanative(string value) : base(value)
            {
            }
        }
    }
}
