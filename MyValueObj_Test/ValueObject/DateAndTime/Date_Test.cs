using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Tools.ValueObject.DateAndTime;

namespace Tools_Test.ValueObject.DateAndTime
{
    /// <summary>　<see cref="Date"/>に対するテストです　</summary>
    public class Date_Test
    {
        public class 値が正常であることを保証してインスタンスを生成できる
        {

            /// <summary> <see cref="Date.CreateBy(string)"/>に対するテストです </summary>
            public class stringを基に正常にインスタンス生成できる
            {
                [Fact]
                public void 正常な値を渡すとインスタンスが返る()
                {
                    var actual = Date.CreateBy("1111/11/11");

                    Assert.IsType<Date>(actual);
                }

                [Theory]
                [InlineData("12341212", "")]
                [InlineData("1234.12.12", ".")]
                public void 正常な値を渡すとインスタンスが返る_セパレータ指定版(string source, string separater)
                {
                    var actual = Date.CreateBy(source, separater);

                    Assert.IsType<Date>(actual);
                }

                [Theory]
                [InlineData("")]
                [InlineData("10/00")]
                [InlineData("10;00/00")]
                [InlineData("10/oo/oo")]
                [InlineData("1/2/3/4/5/")]
                [InlineData("aaaa")]
                [InlineData("aaaabbbbb")]
                public void Format違反の文字列を渡すと例外をスローする(string source)
                {
                    Assert.Throws<ArgumentException>(() => Date.CreateBy(source));
                }

                [Theory]
                [InlineData("11111/00/00")]
                [InlineData("1111/60/11")]
                [InlineData("1111/11/60")]
                [InlineData("-111/22/22")]
                [InlineData("1111/-2/11")]
                [InlineData("1111/11/-3")]
                public void 許容範囲外の文字列を渡すと例外をスローする(string source)
                {
                    Assert.Throws<ArgumentException>(() => Date.CreateBy(source));
                }

                /// <summary> <see cref="Date.(string)"/>に対するテストです </summary>
                public class Try型でインスタンス生成できる
                {
                    [Fact]
                    public void 正常な値を渡すとTrueとインスタンスが返る()
                    {
                        var actualRslt = Date.TryCreateBy("1111/11/11", out var actualInstance);

                        Assert.True(actualRslt);
                        Assert.IsType<Date>(actualInstance);
                    }

                    [Theory]
                    [InlineData("")]
                    [InlineData("10/00")]
                    [InlineData("10;00/00")]
                    [InlineData("10/oo/oo")]
                    [InlineData("aaaa")]
                    [InlineData("aaaabbbbb")]
                    public void Format違反の文字列を渡すとFalseとNullが返る(string source)
                    {
                        var actualRslt = Date.TryCreateBy(source, out var actualInstance);

                        Assert.False(actualRslt);
                        Assert.Null(actualInstance);
                    }

                    [Theory]
                    [InlineData("11111/00/00")]
                    [InlineData("1111/60/11")]
                    [InlineData("1111/11/60")]
                    [InlineData("-111/22/22")]
                    [InlineData("1111/-2/11")]
                    [InlineData("1111/11/-3")]
                    public void 許容範囲外の文字列を渡すとFalseとNullが返る(string source)
                    {
                        var actualRslt = Date.TryCreateBy(source, out var actualInstance);

                        Assert.False(actualRslt);
                        Assert.Null(actualInstance);
                    }
                }
            }

            /// <summary> <see cref="Date.CreateBy(DateTime)"/>に対するテストです </summary>
            public class DateTimeを基に正常にインスタンス化できる
            {
                [Fact]
                public void 正常な値をコンストラクタに渡すとインスタンスが返る()
                {
                    var actual = Date.CreateBy(DateTime.Now);

                    Assert.IsType<Date>(actual);
                }
            }
        }

        public class その日の始点終了日時として値を取得する
        {
            /// <summary> <see cref="Date.AsDatetimeWithDaysStart()"/>に対するテストです </summary>
            [Fact]
            public void その日の始点日時として値を取得する()
            {
                // 準備
                var year = 1111;
                var month = 11;
                var day = 11;
                var expect = year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00");
                var date = Date.CreateBy(expect);
                var alt = new DateTime(year, month, day, 0, 0, 0);

                // 実行
                var actual = date.AsDatetimeWithDaysStart();

                // 検証
                Assert.Equal(alt, actual);
            }

            /// <summary> <see cref="Date.AsDatetimeWithDaysEnd()"/>に対するテストです </summary>
            [Fact]
            public void その日の終了日時として値を取得する()
            {
                // 準備
                var year = 1111;
                var month = 11;
                var day = 11;
                var expect = year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00");
                var date = Date.CreateBy(expect);
                var alt = new DateTime(year, month, day, 23, 59, 59);

                // 実行
                var actual = date.AsDatetimeWithDaysEnd();

                // 検証
                Assert.Equal(alt, actual);
            }
        }

        /// <summary> <see cref="Date.AsString()"/>に対するテストです </summary>
        public class 文字列として値を取得できる
        {
            [Fact]
            public void 保持している値をhhmmssの形式の文字列として取得できる()
            {
                // 準備
                var expect = "1111/11/11";
                var date = Date.CreateBy(expect);

                // 実行
                var actual = date.AsString();

                // 検証
                Assert.Equal(expect, actual);
            }
        }

        public class 規定関数のオーバーライドやIF実装に対するテスト
        {
            public class Eqalsが使える
            {
                [Fact]
                public void 同値なオブジェクトを渡すとTrueを返す()
                {
                    var value = "1111/11/11";
                    var ins = Date.CreateBy(value);
                    var alt = Date.CreateBy(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.True(ins.Equals(ins));
                    Assert.True(ins.Equals(alt));
                    Assert.True(ins.Equals((object)alt));
                }

                [Fact]
                public void 同地でないオブジェクトを渡すとFalseを返す()
                {
                    var ins = Date.CreateBy("1111/11/11");
                    var alt = Date.CreateBy("1111/11/00");
                    var actual = ins.Equals(alt);

                    Assert.False(actual);
                }

                [Fact]
                public void nullや異なる型のオブジェクトを渡すとFalseを返す()
                {
                    var ins = Date.CreateBy("1111/11/11");

                    Assert.False(ins.Equals("1111/11/11"));
                    Assert.False(ins.Equals(null));
                }
            }

            public class GetHashCodeが使える
            {
                [Fact]
                public void 同値なオブジェクトでは同じハッシュを返す()
                {
                    var value = "1111/11/11";
                    var ins = Date.CreateBy(value);
                    var alt = Date.CreateBy(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.Equal(ins.GetHashCode(), alt.GetHashCode());
                    Assert.Equal(ins.GetHashCode(), ins.GetHashCode());
                }

                [Fact]
                public void 同値ではないオブジェクトでは異なるハッシュを返す()
                {
                    var ins = Date.CreateBy("1111/11/11");
                    var alt = Date.CreateBy("1111/12/11");

                    Assert.NotEqual(ins.GetHashCode(), alt.GetHashCode());
                }
            }

            public class CompareToが使える
            {
                [Fact]
                public void ソートした場合に値が小さい順位に並ぶ()
                {
                    // ぐちゃぐちゃな時間で並べる
                    var list = new List<Date>() {
                        Date.CreateBy("1111/11/11"),
                        Date.CreateBy("2222/11/11"),
                        Date.CreateBy("0011/11/11"),
                        Date.CreateBy("3333/11/11"),
                        Date.CreateBy("1111/11/11"),
                    };

                    list.Sort();

                    var pre = Date.CreateBy("0000/00/00");
                    foreach (var t in list)
                    {
                        Assert.True(pre <= t);
                        pre = t;
                    }
                }

                [Fact]
                public void Nullを渡すと例外をスローする()
                {
                    Assert.Throws<ArgumentNullException>(() =>
                     Date.CreateBy("1234/12/12").CompareTo(null));
                }
            }
        }

        public class 演算子にて比較できる
        {
            [Fact]
            public void 期待した比較結果がでる()
            {
                // 別の演算子のベースにってるので　＜ と = は特別手厚くテストする
                {
                    Assert.True(Date.CreateBy("1998/10/10") < Date.CreateBy("1999/10/10"));
                    Assert.True(Date.CreateBy("1999/09/10") < Date.CreateBy("1999/10/10"));
                    Assert.True(Date.CreateBy("1999/10/09") < Date.CreateBy("1999/10/10"));
                    Assert.False(Date.CreateBy("2000/10/10") < Date.CreateBy("1999/10/10"));
                    Assert.False(Date.CreateBy("1999/11/10") < Date.CreateBy("1999/10/10"));
                    Assert.False(Date.CreateBy("1999/10/11") < Date.CreateBy("1999/10/10"));
                    Assert.False(Date.CreateBy("1999/10/10") < Date.CreateBy("1999/10/10"));

                    Assert.True(Date.CreateBy("1999/10/10") == Date.CreateBy("1999/10/10"));
                    Assert.False(Date.CreateBy("1998/10/10") == Date.CreateBy("1999/10/10"));
                    Assert.False(Date.CreateBy("1999/11/10") == Date.CreateBy("1999/10/10"));
                    Assert.False(Date.CreateBy("1999/10/11") == Date.CreateBy("1999/10/10"));
                }

                Assert.True(Data.EARLY != Data.LATER);
                Assert.False(Data.EARLY != Data.EARLY_ALT);
                Assert.True(Data.LATER > Data.EARLY);
                Assert.False(Data.EARLY > Data.LATER);
                Assert.False(Data.EARLY > Data.EARLY_ALT);
                Assert.True(Data.LATER >= Data.EARLY);
                Assert.True(Data.EARLY >= Data.EARLY_ALT);
                Assert.False(Data.EARLY >= Data.LATER);
                Assert.True(Data.EARLY <= Data.LATER);
                Assert.True(Data.EARLY <= Data.EARLY_ALT);
                Assert.False(Data.LATER <= Data.EARLY);
            }
        }

        private static class Data
        {
            public static readonly Date EARLY = Date.CreateBy("1999/04/01");
            public static readonly Date EARLY_ALT = Date.CreateBy("1999/04/01");
            public static readonly Date LATER = Date.CreateBy("3000/04/01");
        }
    }
}
