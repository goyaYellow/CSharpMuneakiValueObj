using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MuneakiValueObject.SizeObj;

namespace Tools_Test.ValueObject.SizeObj
{
    /// <summary> <see cref="SizeObj"/>に対するテストです </summary>
    public class Size_Test
    {
        /// <summary> <see cref="Size.Size(Length, Width, Height)"/>に対するテストです </summary>
        public class 値が正常であることを保証してインスタンスを生成できる
        {
            private static readonly Length GoodLength = new Length(10);
            private static readonly Width GoodWidth = new Width(10);
            private static readonly Height GoodHeight = new Height(10);

            [Fact]
            public void 正常な渡すとインスタンスが返る()
            {
                var actual = new Size(GoodLength, GoodWidth, GoodHeight);

                Assert.IsType<Size>(actual);
                Assert.Equal(GoodLength, actual.Length);
                Assert.Equal(GoodWidth, actual.Width);
                Assert.Equal(GoodHeight, actual.Height);
            }
        }
    }
}
