using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MuneakiValueObject;
using LinqLike;

namespace Tools_Test.ValueObject
{
    /// <summary> <see cref="EnumValueObject{TEnum, TInherited}"/>に対するテストです </summary>
    public class EnumValueObject_Test
    {
        /// <summary> <see cref="EnumValueObject{TEnum, TInherited}.EnumValueObject(TEnum)"/>に対するテストです </summary>
        public class 正常にインスタンス化できる
        {
            [Fact]
            public void 正常な値をコンストラクタに渡すとインスタンスが返る()
            {
                var value = TestEnum.First;
                var actual = new EnumClass(value);

                Assert.IsType<EnumClass>(actual);
                Assert.Equal(value, actual.Value);
            }

            [Fact]
            public void 定義外の値をコンストラクタに渡すと例外をスローする()
            {
                var value = (TestEnum)23132112;

                Assert.Throws<ArgumentException>(() => new EnumClass(value));
            }
        }

        public class 値を所望の形で取得できる
        {
            /// <summary>　<see cref="EnumValueObject{TEnum, TInherited}.AsString"/>に対するテストです　</summary>
            [Fact]
            public void 値を文字列として取得できる()
            {
                var data = new EnumClass(TestEnum.First);

                Assert.Equal("First", data.AsString());
            }

            /// <summary>　<see cref="EnumValueObject{TEnum, TInherited}.ASIntString"/>に対するテストです　</summary>
            [Fact]
            public void 値をIntにした場合の文字列として取得できる()
            {
                var data = new EnumClass(TestEnum.First);

                Assert.Equal("1", data.ASIntString());
            }

            /// <summary>　<see cref="EnumValueObject{TEnum, TInherited}.ASInt"/>に対するテストです　</summary>
            [Fact]
            public void 値をIntとして取得できる()
            {
                var data = new EnumClass(TestEnum.First);

                Assert.Equal(1, data.ASInt());
            }
        }

        public class 規定関数のオーバーライドやIF実装に対するテスト
        {
            public class Eqalsが使える
            {
                [Fact]
                public void 同値なオブジェクトを渡すとTrueを返す()
                {
                    var value = TestEnum.Therd;
                    var ins = new EnumClass(value);
                    var alt = new EnumClass(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.True(ins.Equals(ins));
                    Assert.True(ins.Equals(alt));
                    Assert.True(ins.Equals((object)alt));
                }

                [Fact]
                public void 同地でないオブジェクトを渡すとFalseを返す()
                {
                    var ins = new EnumClass(TestEnum.Zero);
                    var alt = new EnumClass(TestEnum.First);
                    var actual = ins.Equals(alt);

                    Assert.False(actual);
                }

                [Fact]
                public void nullや異なる型のオブジェクトを渡すとFalseを返す()
                {
                    var value = TestEnum.First;
                    var ins = new EnumClass(value);

                    Assert.False(ins.Equals(value));
                    Assert.False(ins.Equals(new EnumClassAltanative(value)));
                    Assert.False(ins.Equals(null));
                }
            }

            public class GetHashCodeが使える
            {
                [Fact]
                public void 同値なオブジェクトでは同じハッシュを返す()
                {
                    var value = TestEnum.First;
                    var ins = new EnumClass(value);
                    var alt = new EnumClass(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.Equal(ins.GetHashCode(), alt.GetHashCode());
                    Assert.Equal(ins.GetHashCode(), ins.GetHashCode());
                }

                [Fact]
                public void 同値ではないオブジェクトでは異なるハッシュを返す()
                {
                    var ins = new EnumClass(TestEnum.Zero);
                    var alt = new EnumClass(TestEnum.First);

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
                    EnumClass[] expected = {
                        new EnumClass(TestEnum.Zero),
                        new EnumClass(TestEnum.First),
                        new EnumClass(TestEnum.Second),
                        new EnumClass(TestEnum.Therd),
                        };
                    var actual = expected.RandomAll().ToList();

                    // 実行
                    actual.Sort();

                    // 検証
                    Assert.True(expected.SequenceEqual(actual));
                }
            }

            /// <summary> Inharited1のオペレータに対するテストです </summary>
            public class 演算子にて比較できる
            {
                [Fact]
                public void 期待した比較結果がでる()
                {
                    // 準備
                    var ins = new EnumClass(TestEnum.First);
                    var otr = new EnumClass(TestEnum.First);
                    var alt = new EnumClass(TestEnum.Second);

                    // 実行と検証
                    Assert.True(ins == otr);
                    Assert.False(ins == alt);
                    Assert.True(ins != alt);
                    Assert.False(ins != otr);
                }
            }
        }

        /// <summary> <see cref="EnumValueObject{TEnum, TInherited}"/>を継承した、テスト用の仮想のクラス </summary>
        public record EnumClass : EnumValueObject<TestEnum, EnumClass>
        {
            public EnumClass(TestEnum value) : base(value)
            {
            }
        }

        /// <summary>  テスト用の仮想のクラスの別Ver </summary>
        public record EnumClassAltanative : EnumValueObject<TestEnum, EnumClassAltanative>
        {
            public EnumClassAltanative(TestEnum value) : base(value)
            {
            }
        }

#pragma warning disable SA1602 // Enumeration items should be documented
        public enum TestEnum
        {
            Zero = 0,

            First = 1,

            Second = 2,

            Therd = 3,
        }
#pragma warning restore SA1602 // Enumeration items should be documented
    }
}
