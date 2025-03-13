using System;

/// <summary>
/// 电子病历模板实体类，对应数据库表 [EMR].[EMRTEMPLET]
/// </summary>
public class EmrTemplet
{
    /// <summary>
    /// 模板ID
    /// </summary>
    public int TempletId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 科室ID
    /// </summary>
    public int? DeptId { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    public int? CreatorId { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime? CreateDateTime { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastTime { get; set; }

    /// <summary>
    /// 访问权限
    /// </summary>
    public int? Permission { get; set; }

    /// <summary>
    /// 类别
    /// </summary>
    public string MrClass { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string MrCode { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string MrName { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    public int? MrAttr { get; set; }

    /// <summary>
    /// 质控代码
    /// </summary>
    public string QcCode { get; set; }

    /// <summary>
    /// 新页标记
    /// </summary>
    public int? NewPageFlag { get; set; }

    /// <summary>
    /// 文件标记: 0-新建, 1-科室医生审签, 2-科室主任审签, 3-医务科审签（模板生效）
    /// </summary>
    public int? FileFlag { get; set; }

    /// <summary>
    /// 书写次数: 0 不限制次数，大于0限制书写次数
    /// </summary>
    public int? WriteTimes { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 医院代码
    /// </summary>
    public string HospitalCode { get; set; }

    /// <summary>
    /// 模板文件（二进制格式）
    /// </summary>
    public byte[] XmlDoc { get; set; }

    /// <summary>
    /// 模板文件（文本格式）
    /// </summary>
    public string XmlDocNew { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    public string Py { get; set; }

    /// <summary>
    /// 五笔
    /// </summary>
    public string Wb { get; set; }

    /// <summary>
    /// 是否首次病程: 0-否, 1-是
    /// </summary>
    public int? IsFirstDaily { get; set; }

    /// <summary>
    /// FileName 是否显示: 0-否, 1-是
    /// </summary>
    public int? IsShowFileName { get; set; }

    /// <summary>
    /// 是否医患沟通: 0-否, 1-是
    /// </summary>
    public int? IsYiHuanGouTong { get; set; }

    /// <summary>
    /// 是否新页结束
    /// </summary>
    public int? NewPageEnd { get; set; }

    /// <summary>
    /// 有效性标记
    /// </summary>
    public int? Valid { get; set; }

    /// <summary>
    /// 状态: 0-保存未提交, 1-提交, 2-审核通过, 3-审核未通过
    /// </summary>
    public int? State { get; set; }

    /// <summary>
    /// 审核人
    /// </summary>
    public int? Auditor { get; set; }

    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? AuditDate { get; set; }

    /// <summary>
    /// 是否配置页面大小
    /// </summary>
    public int? IsConfigPageSize { get; set; }

    /// <summary>
    /// 中医诊断
    /// </summary>
    public string ZyZhenDuan { get; set; }

    /// <summary>
    /// 西医诊断
    /// </summary>
    public string XyZhenDuan { get; set; }
}
