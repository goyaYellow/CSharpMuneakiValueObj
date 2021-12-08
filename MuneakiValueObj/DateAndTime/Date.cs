using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuneakiValueObject.DateAndTime
{
    /// <summary> 日にち </summary>
    /// <remarks> イミュターブルな値オブジェクトです </remarks>
    public record Date : IComparable<Date>
    {
        /// <summary> <see cref="Year"/>の上限 </summary>
        private const int YEAR_MAX = int.MaxValue;

        /// <summary> <see cref="Month"/>の上限 </summary>
        private const int MONTH_MAX = 12;

        /// <summary> <see cref="Day"/>の上限 </summary>
        private const int DAY_MAX = 31;

        /// <summary> 年 </summary>
        public int Year { get; }

        /// <summary> 月 </summary>
        public int Month { get; }

        /// <summary> 日 </summary>
        public int Day { get; }

        /// <summary> Initializes a new instance of the <see cref="Date"/> class. </summary>
        /// <param name="year"> 年 </param>
        /// <param name="mounth"> 月 </param>
        /// <param name="day"> 日 </param>
        /// <exception cref="ArgumentException"/>
        public Date(int year, int mounth, int day)
        {
            if (year < 0 || year > YEAR_MAX) throw new ArgumentException($"Must 0 <= {nameof(year)} <= {YEAR_MAX}");
            if (mounth < 0 || mounth > MONTH_MAX) throw new ArgumentException($"Must 0 <= {nameof(mounth)} <= {MONTH_MAX}");
            if (day < 0 || day > DAY_MAX) throw new ArgumentException($"Must 0 <= {nameof(day)} <= {DAY_MAX}");

            this.Year = year;
            this.Month = mounth;
            this.Day = day;
        }

        /// <summary> ファクトリ｜文字列 </summary>
        /// <param name="sourceText"> YYYYMMDDをセパレーターで分割した文字列 </param>
        /// <param name="separator"> セパレーター。空文字でも許容 </param>
        /// <returns><see cref="Time"/>インスタンス</returns>
        /// <exception cref="ArgumentException"/>
        public static Date CreateBy(string sourceText, string separator = "/")
        {
            string[] splited;

            var expect_length = 4 + 2 + 2 + (separator.Length * 2);
            if (sourceText.Length != expect_length)
                throw new ArgumentException($"{nameof(sourceText)}の長さが不正です。期待する長さ={expect_length}。{nameof(sourceText)}={sourceText}");

            if (separator == string.Empty)
                splited = new string[] { sourceText.Substring(0, 4), sourceText.Substring(4, 2), sourceText.Substring(6, 2) };
            else
                splited = sourceText.Split(separator);

            try
            {
                if (splited.Length != 3) throw new Exception($"サイズ違い length={splited.Length}。{nameof(sourceText)}={sourceText}");
                int yy = int.Parse(splited[0]);
                int mm = int.Parse(splited[1]);
                int dd = int.Parse(splited[2]);
                return new Date(yy, mm, dd);
            }
            catch
            {
                var format = $"YYYY{separator}MM{separator}DD";
                throw new ArgumentException($"{nameof(sourceText)} は{format}の形式でないといけません。{nameof(sourceText)}={sourceText}");
            }
        }

        /// <summary> Try型ファクトリ｜文字列 </summary>
        /// <param name="sourceText"> YYYYMMDDをセパレーターで分割した文字列 </param>
        /// <param name="date">生成したインスタンス。失敗時はnull</param>
        /// <param name="separator"> セパレーター。空文字でも許容 </param>
        /// <returns>生成に成功したらtrue,失敗時はFalse</returns>
        public static bool TryCreateBy(string sourceText, out Date? date, string separator = "/")
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
        /// <returns><see cref="Date"/>インスタンス</returns>
        /// <exception cref="ArgumentNullException"/>
        public static Date CreateBy(DateTime sorce)
            => new Date(sorce.Year, sorce.Month, sorce.Day);

        /// <summary> その日の始点日時として値を取得する </summary>
        /// <returns> 日付部本オブジェクトの値　時刻部が00:00:00 になっている<see cref="DateTime"/>オブジェクト </returns>
        public DateTime AsDatetimeWithDaysStart() => new DateTime(this.Year, this.Month, this.Day, 00, 00, 00);

        /// <summary> その日の終点日時として値を取得する </summary>
        /// <returns> 日付部本オブジェクトの値　時刻部が23:59:59 になっている<see cref="DateTime"/>オブジェクト </returns>
        public DateTime AsDatetimeWithDaysEnd() => new DateTime(this.Year, this.Month, this.Day, 23, 59, 59);

        /// <summary> 文字列として値を取得する </summary>
        /// <returns> YYYY/MM/DDの形式に整えられた値 </returns>
        public string AsString() => $"{this.Year:D2}/{this.Month:D2}/{this.Day:D2}";

        #region C#規定関数のオーバーライドやIF実装

        /// <inheritdoc/>
        public int CompareTo(Date? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (this < other) return -1;
            if (this > other) return 1;
            return 0;
        }
        #endregion

        #region 演算子定義 同一性ではなく同地性を評価します
        public static bool operator <(Date x, Date y)
        {
            if (x.Year < y.Year) return true;
            if (x.Year > y.Year) return false;

            // HHが一緒ならここまでくる
            if (x.Month < y.Month) return true;
            if (x.Month > y.Month) return false;

            // MMが一緒ならここまでくる
            if (x.Day < y.Day) return true;
            if (x.Day > y.Day) return false;

            return false;
        }

        public static bool operator >(Date x, Date y) => (x != y) && !(x < y);

        public static bool operator >=(Date x, Date y) => (x == y) || (x > y);

        public static bool operator <=(Date x, Date y) => (x == y) || (x < y);

        #endregion
    }
}
