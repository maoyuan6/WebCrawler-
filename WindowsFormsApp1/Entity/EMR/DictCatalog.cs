using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;

/// <summary>
/// 电子病历字典目录实体类，对应数据库表 [EMR].[DICT_CATALOG]
/// </summary>
[Table("[EMR].[DICT_CATALOG]")]
public class DictCatalog
{
    /// <summary>
    /// 代码（对应数据库字段 CCODE）
    /// </summary>
    public string CCODE { get; set; }

    /// <summary>
    /// 名称（对应数据库字段 CNAME）
    /// </summary>
    public string CNAME { get; set; }

    /// <summary>
    /// 类型（对应数据库字段 CTYPE）
    /// </summary>
    public char? CTYPE { get; set; }

    /// <summary>
    /// 图像索引（对应数据库字段 IMAGE_INDEX）
    /// </summary>
    public decimal? IMAGE_INDEX { get; set; }

    /// <summary>
    /// 子图像索引（对应数据库字段 SIMAGE_INDEX）
    /// </summary>
    public decimal? SIMAGE_INDEX { get; set; }

    /// <summary>
    /// 开放标识（对应数据库字段 OPEN_FLAG）
    /// </summary>
    public decimal? OPEN_FLAG { get; set; }

    /// <summary>
    /// 用户类型（对应数据库字段 UTYPE）
    /// </summary>
    public string UTYPE { get; set; }

    /// <summary>
    /// 模板类型（对应数据库字段 MTYPE）
    /// </summary>
    public string MTYPE { get; set; }

    /// <summary>
    /// 模板名称（对应数据库字段 MNAME）
    /// </summary>
    public string MNAME { get; set; }

    /// <summary>
    /// 参数（对应数据库字段 ARGS）
    /// </summary>
    public string ARGS { get; set; }

    /// <summary>
    /// 模板类是否启用（对应数据库字段 ISUSED）
    /// </summary>
    public string ISUSED { get; set; }
}

