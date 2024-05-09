using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.IO;

using log4net;
using System.ComponentModel;

namespace Instalments.Models
{
    public class excelXLSXTools
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable ConvertToDataTable<T>(IList<T> data, string[] arrField)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (string field in arrField)
                table.Columns.Add(field);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (arrField.Contains(prop.Name))
                    {
                        if (prop.PropertyType == typeof(DateTime) && prop.GetValue(item).Equals(DateTime.MinValue))
                        {
                            row[prop.Name] = "";
                        }
                        else
                        {
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        }
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor prop = properties[i];
                string name = !String.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name;
                table.Columns.Add(name, prop.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public MemoryStream ExportFileExcel(string header, DataTable dt, string templateFile, int beginRow, bool useSTT)
        {
            try
            {
                #region cap phat o nho
                FileStream fs = null;
                fs = System.IO.File.OpenRead(templateFile);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    bytes = System.IO.File.ReadAllBytes(templateFile);
                    memoryStream.Write(bytes, 0, (int)fs.Length);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                memoryStream.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(memoryStream);
                #endregion cap phat o nho
                var sheet = workbook.GetSheetAt(0);
                var newDataFormat = workbook.CreateDataFormat();
                try
                {
                    writeHeaderSheetExcel(workbook, header, sheet);
                    writeSheetExcel(workbook, sheet, beginRow, dt, useSTT);
                }
                catch (Exception ex1)
                {
                    log.Error("[excelTools][ExportFileExcel] Exception: " + ex1.Message, ex1);
                }

                #region dong o nho
                memoryStream.Close();
                MemoryStream memoryStreamNew = new MemoryStream();
                workbook.Write(memoryStreamNew);

                //memoryStreamNew.Seek(0, SeekOrigin.Begin);

                return memoryStreamNew;
                #endregion dong o nho

            }
            catch (Exception ex)
            {
                log.Error("[excelTools][ExportFileExcel] Exception: " + ex.Message, ex);
                return null;
            }
        }
        public MemoryStream ExportFileExcel(string title, DataTable dt, string templateFile, int beginRow)
        {
            try
            {
                #region cap phat o nho
                FileStream fs = null;
                fs = System.IO.File.OpenRead(templateFile);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    bytes = System.IO.File.ReadAllBytes(templateFile);
                    memoryStream.Write(bytes, 0, (int)fs.Length);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                memoryStream.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(memoryStream);
                #endregion cap phat o nho
                var sheet = workbook.GetSheetAt(0);
                var newDataFormat = workbook.CreateDataFormat();
                try
                {
                    writeTitleSheetExcel(title, dt, sheet, 0);
                    writeSheetExcel(workbook, sheet, beginRow, dt);
                }
                catch (Exception ex1)
                {
                    log.Error("[excelTools][ExportFileExcel] Exception: " + ex1.Message, ex1);
                }

                #region dong o nho
                memoryStream.Close();
                MemoryStream memoryStreamNew = new MemoryStream();
                workbook.Write(memoryStreamNew);

                //memoryStreamNew.Seek(0, SeekOrigin.Begin);

                return memoryStreamNew;
                #endregion dong o nho

            }
            catch (Exception ex)
            {
                log.Error("[excelTools][ExportFileExcel] Exception: " + ex.Message, ex);
                return null;
            }
        }
        public MemoryStream ExportFileExcel(DataTable dt, string templateFile, int beginRow)
        {
            try
            {
                #region cap phat o nho
                FileStream fs = null;
                fs = System.IO.File.OpenRead(templateFile);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    bytes = System.IO.File.ReadAllBytes(templateFile);
                    memoryStream.Write(bytes, 0, (int)fs.Length);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                memoryStream.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(memoryStream);
                #endregion cap phat o nho
                var sheet = workbook.GetSheetAt(0);
                var newDataFormat = workbook.CreateDataFormat();
                try
                {
                    writeSheetExcel(workbook, sheet, beginRow, dt);
                }
                catch (Exception ex1)
                {
                    log.Error("[excelTools][ExportFileExcel] Exception: " + ex1.Message, ex1);
                }

                #region dong o nho
                memoryStream.Close();
                MemoryStream memoryStreamNew = new MemoryStream();
                workbook.Write(memoryStreamNew);

                //memoryStreamNew.Seek(0, SeekOrigin.Begin);

                return memoryStreamNew;
                #endregion dong o nho

            }
            catch (Exception ex)
            {
                log.Error("[excelTools][ExportFileExcel] Exception: " + ex.Message, ex);
                return null;
            }
        }
        public MemoryStream ExportFileExcel(DataTable dt, string templateFile, int beginRow, bool useSTT)
        {
            try
            {
                #region cap phat o nho
                FileStream fs = null;
                fs = System.IO.File.OpenRead(templateFile);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    bytes = System.IO.File.ReadAllBytes(templateFile);
                    memoryStream.Write(bytes, 0, (int)fs.Length);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                memoryStream.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(memoryStream);
                #endregion cap phat o nho
                var sheet = workbook.GetSheetAt(0);
                var newDataFormat = workbook.CreateDataFormat();
                try
                {
                    writeSheetExcel(workbook, sheet, beginRow, dt, useSTT);
                }
                catch (Exception ex1)
                {
                    log.Error("[excelTools][ExportFileExcel] Exception: " + ex1.Message, ex1);
                }
                #region dong o nho
                memoryStream.Close();
                MemoryStream memoryStreamNew = new MemoryStream();
                workbook.Write(memoryStreamNew);

                //memoryStreamNew.Seek(0, SeekOrigin.Begin);

                return memoryStreamNew;
                #endregion dong o nho

            }
            catch (Exception ex)
            {
                log.Error("[excelTools][ExportFileExcel] Exception: " + ex.Message, ex);
                return null;
            }
        }
        private void writeHeaderSheetExcel(IWorkbook workbook, string header, ISheet sheet)
        {
            //dòng đầu
            SetCellValse(workbook, ref sheet, header, 0, 0, "");
        }
        private void writeSheetExcel(IWorkbook workbook, ISheet sheet, int beginRow, DataTable dt, bool useSTT)
        {
            int soCot = dt.Columns.Count;
            if (dt.Rows.Count > 0)
            {
                int begin = beginRow - 1;
                //dòng đầu
                if (useSTT)
                    SetCellValse(workbook, ref sheet, "1", begin, 0, "Int32");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[0][i] != DBNull.Value)
                        SetCellValse(workbook, ref sheet, dt.Rows[0][i].ToString(), begin, useSTT ? i + 1 : i, dt.Columns[i].DataType.Name);
                    else
                        SetCellValseNull(ref sheet, begin, useSTT ? i + 1 : i);
                }

                int _donghientai = beginRow;
                for (int rIndex = 1; rIndex < dt.Rows.Count; rIndex++)
                {
                    DataRow row = dt.Rows[rIndex];
                    sheet.CreateRow(_donghientai);
                    if (useSTT)
                        SetCellValse(ref sheet, (rIndex + 1).ToString(), _donghientai, 0, "Int32", begin);
                    for (int i = 0; i < dt.Columns.Count; i++)
                        if (row[i] != DBNull.Value)
                            SetCellValse(ref sheet, row[i].ToString(), _donghientai, useSTT ? i + 1 : i, dt.Columns[i].DataType.Name, begin);
                        else
                            SetCellValseNull(ref sheet, _donghientai, useSTT ? i + 1 : i, begin);
                    _donghientai += 1;
                }
            }
        }
        public MemoryStream ExportFileExcel_UsedTableForHeader(string title, DataTable dt, string templateFile)
        {
            try
            {
                #region cap phat o nho
                FileStream fs = null;
                fs = System.IO.File.OpenRead(templateFile);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    bytes = System.IO.File.ReadAllBytes(templateFile);
                    memoryStream.Write(bytes, 0, (int)fs.Length);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                memoryStream.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(memoryStream);
                #endregion cap phat o nho
                var sheet = workbook.GetSheetAt(0);
                var newDataFormat = workbook.CreateDataFormat();
                try
                {
                    writeTitleSheetExcel(title, dt, sheet, 0);
                    writeHeaderRowSheetExcel(workbook, dt, sheet, 1);
                    writeSheetExcel(workbook, sheet, 2, dt);
                }
                catch (Exception ex1)
                {
                    log.Error("[excelTools][ExportFileExcel] Exception: " + ex1.Message, ex1);
                }

                #region dong o nho
                memoryStream.Close();
                MemoryStream memoryStreamNew = new MemoryStream();
                workbook.Write(memoryStreamNew);

                return memoryStreamNew;
                #endregion dong o nho

            }
            catch (Exception ex)
            {
                log.Error("[excelTools][ExportFileExcel] Exception: " + ex.Message, ex);
                return null;
            }
        }

        private void writeHeaderRowSheetExcel(IWorkbook workbook, DataTable dt, ISheet sheet, int row)
        {
            for (int i = sheet.GetRow(row).Cells.Count; i <= dt.Columns.Count; i++)
            {
                sheet.GetRow(row).CreateCell(i);
            }

            SetCellValse(workbook, ref sheet, "STT", row, 0, "");

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sheet.GetRow(row).GetCell(i + 1).SetCellValue(dt.Columns[i].ColumnName);
                sheet.GetRow(row).GetCell(i + 1).CellStyle = sheet.GetRow(row).GetCell(0).CellStyle;
            }
        }
        private void writeTitleSheetExcel(string title, DataTable dt, ISheet sheet, int row)
        {
            for (int i = sheet.GetRow(row).Cells.Count; i <= dt.Columns.Count; i++)
                sheet.GetRow(row).CreateCell(i);
            var cra = new CellRangeAddress(row, row, 0, dt.Columns.Count);
            sheet.AddMergedRegion(cra);
            sheet.GetRow(row).GetCell(0).SetCellValue(title.ToUpper());
        }

        private void writeSheetExcel(IWorkbook workbook, ISheet sheet, int beginRow, DataTable dt)
        {
            int soCot = dt.Columns.Count;
            if (dt.Rows.Count > 0)
            {
                int begin = beginRow;
                //dòng đầu
                SetCellValse(workbook, ref sheet, "1", begin, 0, "Int32");
                for (int i = sheet.GetRow(begin).Cells.Count; i <= dt.Columns.Count; i++)
                {
                    sheet.GetRow(begin).CreateCell(i);
                    sheet.GetRow(begin).GetCell(i).CellStyle = sheet.GetRow(begin).GetCell(0).CellStyle;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[0][i] != DBNull.Value)
                        SetCellValse(workbook, ref sheet, dt.Rows[0][i].ToString(), begin, i + 1, dt.Columns[i].DataType.Name);
                    else
                        SetCellValseNull(ref sheet, begin, i + 1);
                }

                int _donghientai = beginRow + 1;
                for (int rIndex = 1; rIndex < dt.Rows.Count; rIndex++)
                {
                    DataRow row = dt.Rows[rIndex];
                    sheet.CreateRow(_donghientai);

                    SetCellValse(ref sheet, (rIndex + 1).ToString(), _donghientai, 0, "Int32", begin);
                    for (int i = 0; i < dt.Columns.Count; i++)
                        if (row[i] != DBNull.Value)
                            SetCellValse(workbook, ref sheet, row[i].ToString(), _donghientai, i + 1, dt.Columns[i].DataType.Name, begin);
                        else
                            SetCellValseNull(ref sheet, _donghientai, i + 1, begin);
                    _donghientai += 1;
                }
            }
        }

        private void writeSheetExcel(IWorkbook workbook, ISheet sheet, int beginRow, DataTable dt, int rowIndex, int maxRowExcel)
        {
            int soCot = dt.Columns.Count;

            if (dt.Rows.Count > 0)
            {
                int begin = beginRow;
                //dòng đầu border
                SetCellValse(workbook, ref sheet, (rowIndex + 1).ToString(), begin, 0, "Int32");
                for (int i = sheet.GetRow(begin).Cells.Count; i <= dt.Columns.Count; i++)
                {
                    sheet.GetRow(begin).CreateCell(i);
                    sheet.GetRow(begin).GetCell(i).CellStyle = sheet.GetRow(begin).GetCell(0).CellStyle;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[rowIndex][i] != DBNull.Value)
                        SetCellValse(workbook, ref sheet, dt.Rows[rowIndex][i].ToString(), begin, i + 1, dt.Columns[i].DataType.Name);
                    else
                        SetCellValseNull(ref sheet, begin, i + 1);
                }
                int _donghientai = beginRow + 1;

                if (maxRowExcel > dt.Rows.Count)
                {
                    maxRowExcel = dt.Rows.Count;
                }
                rowIndex += 1;
                for (int rIndex = rowIndex; rIndex < maxRowExcel; rIndex++)
                {
                    DataRow row = dt.Rows[rIndex];
                    sheet.CreateRow(_donghientai);

                    SetCellValse(ref sheet, (rIndex + 1).ToString(), _donghientai, 0, "Int32", begin);
                    for (int i = 0; i < dt.Columns.Count; i++)
                        if (row[i] != DBNull.Value)
                            SetCellValse(workbook, ref sheet, row[i].ToString(), _donghientai, i + 1, dt.Columns[i].DataType.Name, begin);
                        else
                            SetCellValseNull(ref sheet, _donghientai, i + 1, begin);
                    _donghientai += 1;
                }
            }
        }

        private void SetCellValse(IWorkbook workbook, ref ISheet sheet, String text, int donghientai, int cell, string datatype)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    IDataFormat dataFormatCustom = workbook.CreateDataFormat();
                    switch (datatype)
                    {
                        case "DateTime":
                            sheet.GetRow(donghientai).GetCell(cell).SetCellValue(Convert.ToDateTime(text));//ToString("dd/MM/yyyy")
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                            break;
                        case "Decimal":
                        case "Double":
                            sheet.GetRow(donghientai).GetCell(cell).SetCellValue(Convert.ToDouble(text).ToString("#,##0.#0"));
                            break;
                        case "Int16":
                        case "Int32":
                        case "Int64":
                        case "UInt16":
                        case "UInt32":
                        case "UInt64":
                            sheet.GetRow(donghientai).GetCell(cell).SetCellValue(Convert.ToInt32(text).ToString("#,##0"));
                            break;
                        default:

                            sheet.GetRow(donghientai).GetCell(cell).SetCellValue(text);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("[ReportUtils][SetCellValse] {0}", ex);
            }
        }
        private void SetCellValse(IWorkbook workbook, ref ISheet sheet, String text, int donghientai, int cell, string datatype, int rowStyle)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    IDataFormat dataFormatCustom = workbook.CreateDataFormat();
                    switch (datatype)
                    {
                        case "DateTime":
                            //sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(DateTime.Parse(text).ToString("dd/MM/yyyy"));
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(Convert.ToDateTime(text));
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                            break;
                        case "Decimal":
                        case "Double":
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(Convert.ToDouble(text).ToString("#,##0.#0"));
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            break;
                        case "Int16":
                        case "Int32":
                        case "Int64":
                        case "UInt16":
                        case "UInt32":
                        case "UInt64":
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(Convert.ToInt32(text).ToString("#,##0"));
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            break;
                        default:
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(text != null ? text : string.Empty);
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            break;
                    }
                }
                else
                    sheet.GetRow(donghientai).CreateCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
            }
            catch (Exception ex)
            {
                log.InfoFormat("[ReportUtils][SetCellValse] {0}", ex);
            }
        }
        private void SetCellValse(ref ISheet sheet, String text, int donghientai, int cell, string datatype, int rowStyle)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    switch (datatype)
                    {
                        case "DateTime":
                            //sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(DateTime.Parse(text).ToString("dd/MM/yyyy"));
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(text);
                            //sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            //sheet.GetRow(donghientai).GetCell(cell).CellStyle.DataFormat
                            break;
                        case "Decimal":
                        case "Double":
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(Convert.ToDouble(text).ToString("#,##0.#0"));
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            break;
                        case "Int16":
                        case "Int32":
                        case "Int64":
                        case "UInt16":
                        case "UInt32":
                        case "UInt64":
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(Convert.ToInt32(text).ToString("#,##0"));
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            break;
                        default:
                            sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(text != null ? text : string.Empty);
                            sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
                            break;
                    }
                }
                else
                    sheet.GetRow(donghientai).CreateCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
            }
            catch (Exception ex)
            {
                log.InfoFormat("[ReportUtils][SetCellValse] {0}", ex);
            }
        }
        private void SetCellValseNull(ref ISheet sheet, int donghientai, int cell, int rowStyle)
        {
            try
            {
                sheet.GetRow(donghientai).CreateCell(cell).SetCellValue(string.Empty);
                sheet.GetRow(donghientai).GetCell(cell).CellStyle = sheet.GetRow(rowStyle).GetCell(cell).CellStyle;
            }
            catch (Exception ex)
            {
                log.InfoFormat("[ReportUtils][SetCellValseNull] {0}", ex);
            }
        }
        private void SetCellValseNull(ref ISheet sheet, int donghientai, int cell)
        {
            try
            {
                sheet.GetRow(donghientai).GetCell(cell).SetCellValue(string.Empty);
            }
            catch (Exception ex)
            {
                log.InfoFormat("[ReportUtils][SetCellValseNull] {0}", ex);
            }
        }

        public MemoryStream ExportFileExcelAutoSeperateSheet(DataTable dt, string templateFile, int beginRow)
        {
            try
            {
                #region cap phat o nho
                FileStream fs = null;
                fs = System.IO.File.OpenRead(templateFile);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    bytes = System.IO.File.ReadAllBytes(templateFile);
                    memoryStream.Write(bytes, 0, (int)fs.Length);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                memoryStream.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(memoryStream);
                #endregion cap phat o nho

                var newDataFormat = workbook.CreateDataFormat();
                double totalRow = dt.Rows.Count;
                int maxRow = 65000;
                try
                {
                    double totalRun = Math.Ceiling(totalRow / maxRow);
                    for (int i = 1; i <= totalRun; i++)
                    {
                        var sheet = workbook.GetSheet(string.Format("Sheet{0}", i));
                        int maxRowExcel = Convert.ToInt32(totalRow) - (i - 1) * maxRow <= 0 ? Convert.ToInt32(totalRow) : i * maxRow;
                        writeSheetExcel(workbook, sheet, beginRow, dt, (i - 1) * maxRow, maxRowExcel);
                    }
                }
                catch (Exception ex1)
                {
                    log.Error("[excelTools][ExportFileExcel] Exception: " + ex1.Message, ex1);
                }

                #region dong o nho
                memoryStream.Close();
                MemoryStream memoryStreamNew = new MemoryStream();
                workbook.Write(memoryStreamNew);


                return memoryStreamNew;
                #endregion dong o nho

            }
            catch (Exception ex)
            {
                log.Error("[excelTools][ExportFileExcel] Exception: " + ex.Message, ex);
                return null;
            }
        }
        private void writeSheetExcelWithoutCount(IWorkbook workbook, ISheet sheet, int beginRow, DataTable dt)
        {
            int soCot = dt.Columns.Count;
            if (dt.Rows.Count > 0)
            {
                int begin = beginRow;
                //dòng đầu
                //SetCellValse(workbook, ref sheet, "1", begin, 0, "Int32");
                for (int i = sheet.GetRow(begin).Cells.Count; i <= dt.Columns.Count; i++)
                {
                    sheet.GetRow(begin).CreateCell(i);
                    sheet.GetRow(begin).GetCell(i).CellStyle = sheet.GetRow(begin).GetCell(0).CellStyle;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[0][i] != DBNull.Value)
                        SetCellValse(workbook, ref sheet, dt.Rows[0][i].ToString(), begin, i, dt.Columns[i].DataType.Name);
                    else
                        SetCellValseNull(ref sheet, begin, i);
                }

                int _donghientai = beginRow + 1;
                for (int rIndex = 1; rIndex < dt.Rows.Count; rIndex++)
                {
                    DataRow row = dt.Rows[rIndex];
                    sheet.CreateRow(_donghientai);

                    SetCellValse(ref sheet, (rIndex + 1).ToString(), _donghientai, 0, "Int32", begin);
                    for (int i = 0; i < dt.Columns.Count; i++)
                        if (row[i] != DBNull.Value)
                            SetCellValse(workbook, ref sheet, row[i].ToString(), _donghientai, i, dt.Columns[i].DataType.Name, begin);
                        else
                            SetCellValseNull(ref sheet, _donghientai, i, begin);
                    _donghientai += 1;
                }
            }
        }

        public MemoryStream ExportFileExcelWithoutCount(DataTable dt, string templateFile, int beginRow)
        {
            try
            {
                #region cap phat o nho
                FileStream fs = null;
                fs = System.IO.File.OpenRead(templateFile);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    byte[] bytes = new byte[fs.Length];
                    bytes = System.IO.File.ReadAllBytes(templateFile);
                    memoryStream.Write(bytes, 0, (int)fs.Length);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                memoryStream.Position = 0;
                XSSFWorkbook workbook = new XSSFWorkbook(memoryStream);
                #endregion cap phat o nho
                var sheet = workbook.GetSheetAt(0);
                var newDataFormat = workbook.CreateDataFormat();
                try
                {
                    writeSheetExcelWithoutCount(workbook, sheet, beginRow, dt);
                }
                catch (Exception ex1)
                {
                    log.Error("[excelTools][ExportFileExcel] Exception: " + ex1.Message, ex1);
                }

                #region dong o nho
                memoryStream.Close();
                MemoryStream memoryStreamNew = new MemoryStream();
                workbook.Write(memoryStreamNew);

                //memoryStreamNew.Seek(0, SeekOrigin.Begin);

                return memoryStreamNew;
                #endregion dong o nho

            }
            catch (Exception ex)
            {
                log.Error("[excelTools][ExportFileExcel] Exception: " + ex.Message, ex);
                return null;
            }
        }

    }
}