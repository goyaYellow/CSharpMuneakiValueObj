using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Tools.ValueObject.SizeObj;

namespace Tools_Test.ValueObject.SizeObj
{
    /// <summary>
    /// <see cref="Length"/>に対するテストです
    /// </summary>
    public class Length_Test
    {
        /// <summary>
        /// <see cref="Length.Length(int)"/>に対するテストです
        /// </summary>
        public class 正常にインスタンス化できる
        {
            [Fact]
            public void 正常な値をコンストラクタに渡すとインスタンスが返る()
            {
                var value = 10;
                var actual = new Length(value);

                Assert.IsType<Length>(actual);
                Assert.Equal(value, actual.Value);
            }

            [Fact]
            public void 負の数を渡すと例外をスローする()
            {
                Assert.Throws<ArgumentException>(() => new Length(-1));
            }
        }

        /// <summary>
        /// <see cref="Length.CompareTo(Length)"/>に対するテストです
        /// </summary>
        public class ソートできる
        {
            [Fact]
            public void 正常な値をコンストラクタに渡すとインスタンスが返る()
            {
                // 準備
                int[] values = { 10, 20, 30, };
                var lengths = new List<Length> {
                    new Length(values[2]),
                    new Length(values[0]),
                    new Length(values[1]),
                };

                // 実行
                lengths.Sort();

                // 検証
                for (int i = 0; i < lengths.Count; i++)
                {
                    Assert.Equal(values[i], lengths[i].Value);
                }
            }
        }

        /// <summary>
        /// Lengthのオペレータに対するテストです
        /// </summary>
        public class オペレータの比較が行える
        {
            [Fact]
            public void 値１と値２の大小を比較できる()
            {
                // 準備
                var smaller = new Length(10);
                var bigger = new Length(20);

                // 実行と検証
                Assert.True(smaller < bigger);
                Assert.False(smaller > bigger);
                Assert.True(bigger > smaller);
                Assert.False(bigger < smaller);
            }
        }
    }
}
