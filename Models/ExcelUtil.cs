using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Instalments.Models
{
	public class ExcelUtil
	{
		public static string GetFileName(String filesFolder, string fileName, string ext)
		{
			string result = fileName;
			if (System.IO.File.Exists(filesFolder + fileName + ext))
			{
				Random Rd = new Random();
				result = fileName + Convert.ToString(Rd.Next(1, 100));
			}
			return result;
		}

		public static MemoryStream GetFileStream(string FileSample)
		{
			FileStream fss = null;
			fss = File.OpenRead(FileSample);
			MemoryStream memoryStreamSource = new MemoryStream();
			try
			{
				byte[] bytes = new byte[fss.Length];
				bytes = File.ReadAllBytes(FileSample);
				memoryStreamSource.Write(bytes, 0, (int)fss.Length);
			}
			finally
			{
				if (fss != null)
				{
					fss.Close();
					fss.Dispose();
				}
			}
			return memoryStreamSource;
		}

		protected static string GetCellValue(ICell cell)
		{
			string result = "";
			try
			{
				switch (cell.CellType)
				{
					case CellType.Blank:
						result = cell.StringCellValue;
						break;
					case CellType.Boolean:
						result = cell.BooleanCellValue.ToString();
						break;
					case CellType.Error:
						result = cell.ErrorCellValue.ToString();
						break;
					case CellType.Formula:
						result = cell.CellFormula;
						break;
					case CellType.Numeric:
						result = cell.NumericCellValue.ToString();
						break;
					case CellType.String:
						result = cell.RichStringCellValue.ToString();
						break;
				}
			}
			catch (Exception)
			{
				return result;
			}
			return result;
		}

		public static MemoryStream RenderDataTableToExcel(DataTable sourceTable)
		{
			HSSFWorkbook workbook = new HSSFWorkbook();
			MemoryStream memoryStream = new MemoryStream();
			// By default NPOI creates "Sheet0" which is inconsistent with Excel using "Sheet1"
			ISheet sheet = workbook.CreateSheet("Sheet1");
			IRow headerRow = sheet.CreateRow(0);

			// Header Row
			foreach (DataColumn column in sourceTable.Columns)
				headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

			// Detail Rows
			int rowIndex = 1;

			foreach (DataRow row in sourceTable.Rows)
			{
				IRow dataRow = sheet.CreateRow(rowIndex);

				foreach (DataColumn column in sourceTable.Columns)
				{
					dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
				}

				rowIndex++;
			}

			workbook.Write(memoryStream);
			memoryStream.Flush();
			memoryStream.Position = 0;

			return memoryStream;
		}
	}
}