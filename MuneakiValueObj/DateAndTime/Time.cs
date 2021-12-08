using System;
using System.Collections.Generic;
using System.Text;

namespace MuneakiValueObject.DateAndTime
{
    /// <summary> 時刻 </summary>
    public record Time : IComparable<Time>
    {
        /// <summary> <see cref="Hour"/>の上限 </summary>
        private const int HOUER_MAX = 23;

        /// <summary> <see cref="Minute"/>の上限 </summary>
        private const int MINUITE_MAX = 59;

        /// <summary> <see cref="Second"/>の上限 </summary>
        private const int SECOND_MAX = 59;

        /// <summary> 時 </summary>
        public int Hour { get; init; }

        /// <summary> 分 </summary>
        public int Minute { get; }

        /// <summary> 秒 </summary>
        public int Second { get; }

        /// <summary> Initializes a new instance of the <see cref="Time"/> class. </summary>
        /// <param name="hour"> 時 </param>
        /// <param name="minute"> 分 </param>
        /// <param name="second"> 秒 </param>
        /// <exception cref="ArgumentException"/>
        public Time(int hour, int minute, int second)
        {
            if (hour < 0 || hour > HOUER_MAX) throw new ArgumentException($"Must 0 <= {nameof(hour)} <= {HOUER_MAX}");
            if (minute < 0 || minute > MINUITE_MAX) throw new ArgumentException($"Must 0 <= {nameof(minute)} <= {MINUITE_MAX}");
            if (second < 0 || second > SECOND_MAX) throw new ArgumentException($"Must 0 <= {nameof(second)} <= {SECOND_MAX}");

            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }

        /// <summary> ファクトリ｜文字列 </summary>
        /// <param name="sourceText"> hhmmssをセパレーターで分割した文字列 </param>
        /// <param name="separator"> セパレーター。空文字でも許容 </param>
        /// <returns><see cref="Time"/>インスタンス</returns>
        /// <exception cref="ArgumentException"/>
        public static Time CreateBy(string sourceText, string separator = ":")
        {
            string[] splited;

            var expect_length = 2 + 2 + 2 + (separator.Length * 2);
            if (sourceText.Length != expect_length)
                throw new ArgumentException($"{nameof(sourceText)}の長さが不正です。期待する長さ={expect_length}。{nameof(sourceText)}={sourceText}");

            if (separator == string.Empty)
                splited = new string[] { sourceText.Substring(0, 2), sourceText.Substring(2, 2), sourceText.Substring(4, 2) };
            else
                splited = sourceText.Split(separator);

            try
            {
                if (splited.Length != 3) throw new Exception();
                int hh = int.Parse(splited[0]);
                int mm = int.Parse(splited[1]);
                int ss = int.Parse(splited[2]);
                return new Time(hh, mm, ss);
            }
            catch
            {
                var format = $"hh{separator}mm{separator}ss";
                throw new ArgumentException($"{nameof(sourceText)} は{format}の形式でないといけません。{nameof(sourceText)}={sourceText}");
            }
        }

        /// <summary> Try型ファクトリ｜文字列 </summary>
        /// <param name="sourceText"> hhmmssをセパレーターで分割した文字列 </param>
        /// <param name="date">生成したインスタンス。失敗時はnull</param>
        /// <param name="separator"> セパレーター。空文字でも許容 </param>
        /// <returns>生成に成功したらtrue,失敗時はFalse</returns>
        public static bool TryCreateBy(string sourceText, out Time? date, string separator = ":")
        {
            try
            {
                date = CreateBy(sourceText, separator);
            }
            catch
            {
                date = null;
                return false;
            }

            return true;
        }

        /// <summary> ファクトリ｜Datetime </summary>
        /// <param name="sorce"> こいつの時刻をもとにインスタンス化する </param>
        /// <returns><see cref="Time"/>インスタンス</returns>
        /// <exception cref="ArgumentNullException"/>
        public static Time CreateBy(DateTime sorce)
            => new Time(sorce.Hour, sorce.Minute, sorce.Second);

        /// <summary> 今日の日時として値を取得する </summary>
        /// <returns> 日付部が今日 時刻部が本オブジェクトの値 になっている<see cref="DateTime"/>オブジェクト </returns>
        public DateTime AsTodaysDT()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, this.Hour, this.Minute, this.Second, 00);
        }

        /// <summary> 文字列として値を取得する </summary>
        /// <returns> hh:mmの形式に整えられた値 </returns>
        public string AsString()
            => $"{this.Hour:D2}:{this.Minute:D2}:{this.Second:D2}";

        #region C#規定関数のオーバーライドやIF実装

        /// <inheritdoc/>
        public int CompareTo(Time? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (this < other) return -1;
            if (this > other) return 1;
            return 0;
        }
        #endregion

        #region 演算子定義 同一性ではなく同地性を評価します
        public static bool operator <(Time x, Time y)
        {
            if (x.Hour < y.Hour) return true;
            if (x.Hour > y.Hour) return false;

            // HHが一緒ならここまでくる
            if (x.Minute < y.Minute) return true;
            if (x.Minute > y.Minute) return false;

            // MMが一緒ならここまでくる
            if (x.Second < y.Second) return true;
            if (x.Second > y.Second) return false;

            return false;
        }

        public static bool operator >(Time x, Time y) => (x != y) && !(x < y);

        public static bool operator >=(Time x, Time y) => (x == y) || (x > y);

        public static bool operator <=(Time x, Time y) => (x == y) || (x < y);

        #endregion
    }
}
