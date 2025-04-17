using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;

public class ExcelSqlExporter
{
    private const string ConnectionString = "Data Source=192.168.210.170;Initial Catalog=CT2_UAT;User ID=CT2;Password=CT2123456;";

    public void Execute()
    {
        try
        {
            // 1. 选择Excel文件
            var filePath = SelectExcelFile();
            if (string.IsNullOrEmpty(filePath)) return;

            // 2. 读取ID列
            var ids = ReadIdsFromExcel(filePath, "ID");  // 假设列名为"ID"
            if (ids.Count == 0)
            {
                MessageBox.Show("未找到有效的ID");
                return;
            }

            // 3. 执行SQL查询
            var dataTable = ExecuteQuery(ids);

            // 4. 导出到Excel
            ExportToExcel(dataTable);

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

    private System.Collections.Generic.List<int> ReadIdsFromExcel(string filePath, string columnName)
    {
        var ids = new System.Collections.Generic.List<int>();

        var fileInfo = new FileInfo(filePath);
        using (var package = new ExcelPackage(fileInfo))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var columnIndex = GetColumnIndex(worksheet, columnName);

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var cellValue = worksheet.Cells[row, columnIndex].Text;
                if (int.TryParse(cellValue, out int id))
                {
                    ids.Add(id);
                }
            }
        }
        return ids;
    }

    private int GetColumnIndex(ExcelWorksheet worksheet, string columnName)
    {
        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (worksheet.Cells[1, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
            {
                return col;
            }
        }
        throw new Exception($"未找到列：{columnName}");
    }

    private DataTable ExecuteQuery(System.Collections.Generic.List<int> ids)
    {
        var sql = $@"SELECT 
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
            a.EnterpriseID IN ({string.Join(",", ids)})";

        using (var conn = new SqlConnection(ConnectionString))
        using (var adapter = new SqlDataAdapter(sql, conn))
        {
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }

    private void ExportToExcel(DataTable dataTable)
    {
        using (var saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "Excel文件|*.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("导出结果");

                    // 写入标题
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                    }

                    // 写入数据
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                        }
                    }

                    package.SaveAs(new FileInfo(saveFileDialog.FileName));
                }
            }
        }
    }
}