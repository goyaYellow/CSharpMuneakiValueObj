using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Support.DataTableTools
{
    /// <summary> Datasetを使いやすくするためのラッパー </summary>
    public class DatatableWrapper : IEnumerable<DataRowWrapper>
    {
        private readonly DataTable table;

        /// <summary> Initializes a new instance of the <see cref="DatatableWrapper"/> class. </summary>
        /// <param name="table">データテーブル</param>
        public DatatableWrapper(DataTable table)
            => this.table = table;

        /// <inheritdoc/>
        public IEnumerator<DataRowWrapper> GetEnumerator()
        {
            foreach (DataRow row in this.table.Rows)
                yield return new DataRowWrapper(row);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

   /// <summary> Dataflameを使いやすくするためのラッパー </summary>
    public class DataRowWrapper
    {
        private readonly DataRow row;

        /// <summary> Initializes a new instance of the <see cref="DataRowWrapper"/> class. </summary>
        /// <param name="row">データ行</param>
        public DataRowWrapper(DataRow row)
        {
            this.row = row;
        }

        /// <summary> 指定列の値をStringとして取得する </summary>
        /// <param name="header">取得したい列のヘッダ</param>
        /// <exception cref="InvalidDataException">該当ヘッダが存在しない場合などにスローします</exception>
        /// <returns>指定列の値</returns>
        public string FindAsStr(string header)
        {
            try
            {
                return this.row[header]?.ToString() ?? string.Empty;
            }
            catch
            {
                throw new InvalidDataException($"指定されたヘッダ{header}に該当するデータが存在しませんでした");
            }
        }

        /// <summary> 指定列の値をStringとして取得する </summary>
        /// <param name="header">取得したい列のヘッダ</param>
        /// <exception cref="InvalidDataException">該当ヘッダが存在しない、データがキャストできない場合などにスローします</exception>
        /// <returns>指定列の値</returns>
        public int FindAsInt(string header)
        {
            var data = this.FindAsStr(header);
            var rslt = int.TryParse(data, out var ret);
            if (!rslt) throw new InvalidDataException($"指定されたヘッダ{header}に該当するデータはintにキャスト出来ません。データ={data}");
            return ret;
        }
    }
}
