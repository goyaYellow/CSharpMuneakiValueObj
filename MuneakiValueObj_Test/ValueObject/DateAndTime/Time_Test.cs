using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Tools.ValueObject.DateAndTime;

namespace Tools_Test.ValueObject.DateAndTime
{
    /// <summary>　<see cref="Time"/>に対するテストです　</summary>
    public class Time_Test
    {
        public class 値が正常であることを保証してインスタンスを生成できる
        {

            /// <summary> <see cref="Time.CreateBy(string)"/>に対するテストです </summary>
            public class stringを基に正常にインスタンス生成できる
            {
                [Fact]
                public void 正常な値を渡すとインスタンスが返る()
                {
                    var actual = Time.CreateBy("10:00:00");

                    Assert.IsType<Time>(actual);
                }

                [Theory]
                [InlineData("100010", "")]
                [InlineData("10.00.10", ".")]
                public void 正常な値を渡すとインスタンスが返る_セパレータ指定版(string source, string separater)
                {
                    var actual = Time.CreateBy(source, separater);

                    Assert.IsType<Time>(actual);
                }

                [Theory]
                [InlineData("")]
                [InlineData("10:00")]
                [InlineData("10;00;00")]
                [InlineData("10:oo:oo")]
                [InlineData("aaaa")]
                [InlineData("aaaabbbbb")]
                public void Format違反の文字列を渡すと例外をスローする(string source)
                {
                    Assert.Throws<ArgumentException>(() => Time.CreateBy(source));
                }

                [Theory]
                [InlineData("25:00:00")]
                [InlineData("10:60:00")]
                [InlineData("10:00:60")]
                [InlineData("-1:22:22")]
                [InlineData("22:-2:22")]
                [InlineData("22:22:-3")]
                public void 許容範囲外の文字列を渡すと例外をスローする(string source)
                {
                    Assert.Throws<ArgumentException>(() => Time.CreateBy(source));
                }

                /// <summary> <see cref="Time.(string)"/>に対するテストです </summary>
                public class Try型でインスタンス生成できる
                {
                    [Fact]
                    public void 正常な値を渡すとTrueとインスタンスが返る()
                    {
                        var actualRslt = Time.TryCreateBy("10:00:00", out var actualInstance);

                        Assert.True(actualRslt);
                        Assert.IsType<Time>(actualInstance);
                    }

                    [Theory]
                    [InlineData("")]
                    [InlineData("10:00")]
                    [InlineData("10;00;00")]
                    [InlineData("10:oo:oo")]
                    [InlineData("aaaa")]
                    [InlineData("aaaabbbbb")]
                    public void Format違反の文字列を渡すとFalseとNullが返る(string source)
                    {
                        var actualRslt = Time.TryCreateBy(source, out var actualInstance);

                        Assert.False(actualRslt);
                        Assert.Null(actualInstance);
                    }

                    [Theory]
                    [InlineData("25:00:00")]
                    [InlineData("10:60:00")]
                    [InlineData("10:00:60")]
                    [InlineData("-1:00:00")]
                    public void 許容範囲外の文字列を渡すとFalseとNullが返る(string source)
                    {
                        var actualRslt = Time.TryCreateBy(source, out var actualInstance);

                        Assert.False(actualRslt);
                        Assert.Null(actualInstance);
                    }
                }
            }

            /// <summary> <see cref="Time.CreateBy(DateTime)"/>に対するテストです </summary>
            public class DateTimeを基に正常にインスタンス化できる
            {
                [Fact]
                public void 正常な値をコンストラクタに渡すとインスタンスが返る()
                {
                    var actual = Time.CreateBy(DateTime.Now);

                    Assert.IsType<Time>(actual);
                }
            }
        }

        /// <summary> <see cref="Time.AsTodaysDT()"/>に対するテストです </summary>
        public class 現在の日時として値を取得できる
        {
            [Fact]
            public void 保持している値を現在の日時として取得できる()
            {
                // 準備
                int hh = 11;
                int mm = 29;
                int ss = 59;
                var today = DateTime.Now;

                var now = new DateTime(today.Year, today.Month, today.Day, hh, mm, ss, 0);  // miliSecに対応していないので0埋め
                var time = Time.CreateBy(hh + ":" + mm + ":" + ss);

                // 実行
                var resultTime = time.AsTodaysDT();
                var actual = resultTime - now;

                // 検証
                Assert.Equal(TimeSpan.Zero, actual);
            }
        }

        /// <summary> <see cref="Time.AsString()"/>に対するテストです </summary>
        public class 文字列として値を取得できる
        {
            [Fact]
            public void 保持している値をhhmmssの形式の文字列として取得できる()
            {
                // 準備
                var expect = "11:09:59";
                var time = Time.CreateBy(expect);

                // 実行
                var actual = time.AsString();

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
                    var value = "10:10:10";
                    var ins = Time.CreateBy(value);
                    var alt = Time.CreateBy(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.True(ins.Equals(ins));
                    Assert.True(ins.Equals(alt));
                    Assert.True(ins.Equals((object)alt));
                }

                [Fact]
                public void 同地でないオブジェクトを渡すとFalseを返す()
                {
                    var ins = Time.CreateBy("10:10:10");
                    var alt = Time.CreateBy("10:10:11");
                    var actual = ins.Equals(alt);

                    Assert.False(actual);
                }

                [Fact]
                public void nullや異なる型のオブジェクトを渡すとFalseを返す()
                {
                    var ins = Time.CreateBy("10:10:10");

                    Assert.False(ins.Equals("10:10:10"));
                    Assert.False(ins.Equals(null));
                }
            }

            public class GetHashCodeが使える
            {
                [Fact]
                public void 同値なオブジェクトでは同じハッシュを返す()
                {
                    var value = "10:10:10";
                    var ins = Time.CreateBy(value);
                    var alt = Time.CreateBy(value);

                    // インスタンスが異なるかどうかは関係ない
                    Assert.Equal(ins.GetHashCode(), alt.GetHashCode());
                    Assert.Equal(ins.GetHashCode(), ins.GetHashCode());
                }

                [Fact]
                public void 同値ではないオブジェクトでは異なるハッシュを返す()
                {
                    var ins = Time.CreateBy("10:10:10");
                    var alt = Time.CreateBy("10:10:11");

                    Assert.NotEqual(ins.GetHashCode(), alt.GetHashCode());
                }
            }

            public class CompareToが使える
            {
                [Fact]
                public void ソートした場合に値が小さい順位に並ぶ()
                {
                    // ぐちゃぐちゃな時間で並べる
                    var list = new List<Time>() {
                        Time.CreateBy("00:00:10"),
                        Time.CreateBy("00:10:10"),
                        Time.CreateBy("00:00:09"),
                        Time.CreateBy("10:10:10"),
                        Time.CreateBy("00:00:10"),
                    };

                    list.Sort();

                    var pre = Time.CreateBy("00:00:00");
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
                     Time.CreateBy("11:22:33").CompareTo(null));
                }
            }
        }

        /// <summary>　オペレータに対するテストです　</summary>
        public class 演算子にて比較できる
        {
            [Fact]
            public void 期待した比較結果がでる()
            {
                // 別の演算子のベースにってるので　＜ と = は特別手厚くテストする
                {
                    Assert.True(Time.CreateBy("09:10:10") < Time.CreateBy("10:10:10"));
                    Assert.True(Time.CreateBy("10:09:10") < Time.CreateBy("10:10:10"));
                    Assert.True(Time.CreateBy("10:10:09") < Time.CreateBy("10:10:10"));
                    Assert.False(Time.CreateBy("11:10:10") < Time.CreateBy("10:10:10"));
                    Assert.False(Time.CreateBy("10:11:10") < Time.CreateBy("10:10:10"));
                    Assert.False(Time.CreateBy("10:10:11") < Time.CreateBy("10:10:10"));
                    Assert.False(Time.CreateBy("10:10:10") < Time.CreateBy("10:10:10"));

                    Assert.True(Time.CreateBy("10:10:10") == Time.CreateBy("10:10:10"));
                    Assert.False(Time.CreateBy("11:10:10") == Time.CreateBy("10:10:10"));
                    Assert.False(Time.CreateBy("10:11:10") == Time.CreateBy("10:10:10"));
                    Assert.False(Time.CreateBy("10:10:11") == Time.CreateBy("10:10:10"));
                }

                Assert.True(Data.BASE != Data.LATER);
                Assert.False(Data.BASE != Data.BASE_ALT);
                Assert.True(Data.LATER > Data.BASE);
                Assert.False(Data.BASE > Data.LATER);
                Assert.False(Data.BASE > Data.BASE_ALT);
                Assert.True(Data.LATER >= Data.BASE);
                Assert.True(Data.BASE >= Data.BASE_ALT);
                Assert.False(Data.BASE >= Data.LATER);
                Assert.True(Data.BASE <= Data.LATER);
                Assert.True(Data.BASE <= Data.BASE_ALT);
                Assert.False(Data.LATER <= Data.BASE);
            }
        }

        private static class Data
        {
            public static readonly Time BASE = Time.CreateBy("10:10:10");
            public static readonly Time BASE_ALT = Time.CreateBy("10:10:10");
            public static readonly Time LATER = Time.CreateBy("11:11:11");
        }
    }
}
