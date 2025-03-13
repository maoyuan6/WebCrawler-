using System;

/// <summary>
/// 电子病历字典目录实体类，对应数据库表 [EMR].[DICT_CATALOG]
/// </summary>
public class DictCatalog
{
    /// <summary>
    /// 代码
    /// </summary>
    public string CCode { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string CName { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public char? CType { get; set; }

    /// <summary>
    /// 图像索引
    /// </summary>
    public decimal? ImageIndex { get; set; }

    /// <summary>
    /// 子图像索引
    /// </summary>
    public decimal? SImageIndex { get; set; }

    /// <summary>
    /// 开放标识
    /// </summary>
    public decimal? OpenFlag { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public string UType { get; set; }

    /// <summary>
    /// 模板类型
    /// </summary>
    public string MType { get; set; }

    /// <summary>
    /// 模板名称
    /// </summary>
    public string MName { get; set; }

    /// <summary>
    /// 参数
    /// </summary>
    public string Args { get; set; }

    /// <summary>
    /// 模板类是否启用
    /// </summary>
    public string IsUsed { get; set; }
}
