using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1649 // File name should match first type name

namespace Tools.Support.DataTableTools
{
    /// <summary> Datseを扱うためのツール集 </summary>
    public static class DatatableTools
    {
        /// <summary> csvを読み込みtable形式にして返す </summary>
        /// <param name="path">Fileパス</param>
        /// <param name="separator">セパレータ。デフォルトではカンマ。</param>
        /// <param name="encoding">エンコーディング。特にセットしなければUTF－８になる</param>
        /// <exception cref="InvalidDataException">ヘッダーの列数とデータの列数が不一致など。読み取り失敗時にスロー。</exception>
        /// <returns>csvの中身</returns>
        public static DatatableWrapper LoadCsv(string path, char separator = ',', Encoding? encoding = null)
        {
            var table = new DataTable();

            var exeEncoding = encoding ?? Encoding.UTF8;

            try
            {
                using StreamReader sr = new StreamReader(path, encoding: exeEncoding);

                // ヘッダ
                string s = sr.ReadLine()?.Replace("\"", string.Empty)
                    ?? throw new InvalidDataException("ヘッダの読み取りに失敗しました");
                string[] cols = s.Split(separator);
                foreach (string col in cols)
                {
                    table.Columns.Add(col, typeof(string));
                }

                // ボディ
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine()?.Replace("\"", string.Empty)
                        ?? throw new InvalidDataException("データの読み取りに失敗しました");
                    string[] values = line.Split(separator);

                    if (values.Length != cols.Length)
                        throw new InvalidDataException("ヘッダーの列数とデータの列数が不一致");

                    table.Rows.Add(values);
                }

                return new DatatableWrapper(table);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"{path}の読み込みに失敗しました", e);
            }
        }
    }
}

#pragma warning restore SA1649 // File name should match first type name
