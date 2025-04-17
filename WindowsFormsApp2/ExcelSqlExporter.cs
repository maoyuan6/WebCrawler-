using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using NPOI.XSSF.Streaming;
using System.Threading.Tasks;

public class ExcelSqlExporterNpoi
{
    private const string ConnectionString = "";
    private string columnName = "档案ID";
    public void Execute()
    {
        try
        {
            // 1. 选择Excel文件
            var filePath = SelectExcelFile();
            if (string.IsNullOrEmpty(filePath)) return;

            // 2. 读取ID列
            var ids = ReadIdsFromExcel(filePath, columnName);
            if (ids.Count == 0)
            {
                MessageBox.Show("未找到有效的ID");
                return;
            }

            // 3. 执行SQL查询
            var dataTable = ExecuteQuery(ids);

            // 4. 导出到Excel
            ExportToExcel(dataTable, filePath);

            MessageBox.Show("导出成功！");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"发生错误：{ex.Message}");
        }
    }

    private string SelectExcelFile()
    {
        using (var openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "Excel文件|*.xlsx;*.xls";
            return openFileDialog.ShowDialog() == DialogResult.OK
                ? openFileDialog.FileName
                : null;
        }
    }

    private List<string> ReadIdsFromExcel(string filePath, string columnName)
    {
        var ids = new List<string>();

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook;
            if (Path.GetExtension(filePath).ToLower() == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                workbook = new HSSFWorkbook(fs);
            }

            var sheet = workbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);

            // 查找ID列索引
            int columnIndex = -1;
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                if (headerRow.GetCell(i)?.ToString().Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    columnIndex = i;
                    break;
                }
            }

            if (columnIndex == -1) throw new Exception($"未找到列：{columnName}");

            // 读取数据行
            for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                var row = sheet.GetRow(rowIdx);
                if (row == null) continue;

                var cell = row.GetCell(columnIndex);
                if (cell != null)
                {
                    ids.Add(cell.ToString());
                }
            }
        }
        return ids;
    }

    private DataTable ExecuteQuery(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return new DataTable();
        }

        // 参数化查询
        var parameters = new SqlParameter[ids.Count];
        var paramNames = new string[ids.Count];

        for (int i = 0; i < ids.Count; i++)
        {
            parameters[i] = new SqlParameter($"@id{i}", ids[i]);
            paramNames[i] = $"@id{i}";
        }

        const string sql = @"SELECT DISTINCT
        a.ID AS 档案编号,
        a.Name AS 企业名称,
        a.HotelCd AS 酒店编号,
        a.HotelName AS 酒店名称,
        (CASE WHEN a.IsAuth = 1 THEN '已认证' ELSE '未认证' END) AS 认证状态,
        (CASE WHEN a.Cooperator IS NULL OR a.Cooperator = '' THEN '否' ELSE '是' END) AS 是否存在协同,
        ISNULL(a.Cooperator, '') AS 协同人,
        ISNULL(a.CooperateTime, null) AS 协同时间,
        ISNULL(a.Author, '') AS 认证人,
        ISNULL(a.AuthTime, null) AS 认证时间, 
        ISNULL(b.CreatedDatetime, null) AS 创建时间,
        (CASE WHEN c.ARBillID IS NULL THEN '否' ELSE '是' END) AS 是否存在AR 
    FROM 
        Enterprise a 
        LEFT JOIN Enterprise_Created b ON a.ID = b.id  
        LEFT JOIN ARBillRelation c ON a.ID = c.EnterpriseID 
    WHERE  
        a.ID IN ({0})";

        using (var conn = new SqlConnection(ConnectionString))
        using (var cmd = new SqlCommand(string.Format(sql, string.Join(",", paramNames)), conn))
        {
            cmd.Parameters.AddRange(parameters);

            // 设置超时时间（300秒=5分钟）
            cmd.CommandTimeout = 3000;

            using (var adapter = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

    private void ExportToExcel(DataTable dataTable, string sourceFilePath)
    {
        using (var folderDialog = new FolderBrowserDialog())
        {
            folderDialog.Description = "选择导出目录";
            folderDialog.ShowNewFolderButton = true;

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = $"{Path.GetFileNameWithoutExtension(sourceFilePath)}_Result.xlsx";
                string savePath = Path.Combine(folderDialog.SelectedPath, fileName);

                // 处理文件已存在的情况
                if (File.Exists(savePath))
                {
                    var result = MessageBox.Show("文件已存在，是否覆盖？", "确认覆盖", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes) return;
                    File.Delete(savePath);
                }

                // 使用SXSSFWorkbook优化大数据内存（自动支持流式写入）
                using (var fs = new FileStream(savePath, FileMode.Create))
                {
                    // 创建流式工作簿（内存中保留1000行）
                    IWorkbook workbook = new SXSSFWorkbook(1000);
                    ISheet sheet = workbook.CreateSheet("导出结果");

                    // 创建标题样式
                    ICellStyle headerStyle = CreateHeaderStyle(workbook);

                    // 创建标题行
                    IRow headerRow = sheet.CreateRow(0);
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        var cell = headerRow.CreateCell(i);
                        cell.SetCellValue(dataTable.Columns[i].ColumnName);
                        cell.CellStyle = headerStyle;
                    }

                    // 分批次写入数据（降低内存占用）
                    int batchSize = 50000;
                    for (int startRow = 0; startRow < dataTable.Rows.Count; startRow += batchSize)
                    {
                        int endRow = Math.Min(startRow + batchSize, dataTable.Rows.Count);

                        for (int rowIdx = startRow; rowIdx < endRow; rowIdx++)
                        {
                            var dataRow = sheet.CreateRow(rowIdx + 1 - startRow);
                            for (int colIdx = 0; colIdx < dataTable.Columns.Count; colIdx++)
                            {
                                var value = dataTable.Rows[rowIdx][colIdx];
                                var cell = dataRow.CreateCell(colIdx);

                                // 智能数据类型处理
                                SetCellValueWithFormat(cell, value);
                            }
                        }

                        // 每批次写入后强制GC回收（针对大数据优化）
                        if (endRow % 100000 == 0)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }

                    // 自动调整列宽（性能优化版）
                    //AutoSizeColumns(sheet, dataTable.Columns.Count);

                    // 写入文件
                    workbook.Write(fs);
                }

                MessageBox.Show($"文件已保存到：\n{savePath}");
            }
        }
    }
    // 创建标题样式
    private ICellStyle CreateHeaderStyle(IWorkbook workbook)
    {
        ICellStyle style = workbook.CreateCellStyle();
        NPOI.SS.UserModel.IFont font = workbook.CreateFont();
        font.IsBold = true;
        font.FontHeightInPoints = 11;
        style.SetFont(font);
        style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        style.FillPattern = FillPattern.SolidForeground;
        return style;
    }

    // 智能数据类型处理
    private void SetCellValueWithFormat(ICell cell, object value)
    {
        if (value == null || value == DBNull.Value)
        {
            cell.SetCellValue("");
            return;
        }

        switch (value)
        {
            case int num:
                cell.SetCellValue(num);
                break;
            case decimal dec:
                cell.SetCellValue((double)dec);
                break;
            case bool b:
                cell.SetCellValue(b);
                break;
            case DateTime dt:
                cell.SetCellValue(dt.ToString("yyyy-MM-dd"));
                break;
            default:
                cell.SetCellValue(value?.ToString() ?? "");
                break;
        }
    }

    // 自动调整列宽（性能优化）
    private void AutoSizeColumns(ISheet sheet, int columnCount)
    {
        // 并行处理列宽计算（针对多核CPU优化）
        Parallel.For(0, columnCount, i =>
        {
            sheet.AutoSizeColumn(i);
            int currentWidth = sheet.GetColumnWidth(i);
            sheet.SetColumnWidth(i, Math.Min(currentWidth + 512, 255 * 256)); // 最大255字符
        });
    }
    // 辅助方法：创建日期格式样式
    private ICellStyle CreateDateStyle(IWorkbook workbook)
    {
        ICellStyle style = workbook.CreateCellStyle();
        IDataFormat format = workbook.CreateDataFormat();
        style.DataFormat = format.GetFormat("yyyy-mm-dd");
        return style;
    }

    // 辅助方法：判断是否为数值类型
    private bool IsNumeric(object value)
    {
        return value is int || value is decimal || value is double || value is long;
    }
}