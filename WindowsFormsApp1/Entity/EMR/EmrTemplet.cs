using System;

/// <summary>
/// 电子病历模板实体类，对应数据库表 [EMR].[EMRTEMPLET]
/// </summary>
public class EmrTemplet
{
    /// <summary>
    /// 模板ID（主键）
    /// </summary>
    public int TEMPLET_ID { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FILE_NAME { get; set; }

    /// <summary>
    /// 科室ID
    /// </summary>
    public int? DEPT_ID { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    public int? CREATOR_ID { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime? CREATE_DATETIME { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LAST_TIME { get; set; }

    /// <summary>
    /// 访问权限
    /// </summary>
    public int? PERMISSION { get; set; }

    /// <summary>
    /// 类别
    /// </summary>
    public string MR_CLASS { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string MR_CODE { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string MR_NAME { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    public int? MR_ATTR { get; set; }

    /// <summary>
    /// 质控代码
    /// </summary>
    public string QC_CODE { get; set; }

    /// <summary>
    /// 新页标记
    /// </summary>
    public int? NEW_PAGE_FLAG { get; set; }

    /// <summary>
    /// 文件标记: 0-新建, 1-科室医生审签, 2-科室主任审签, 3-医务科审签（模板生效）
    /// </summary>
    public int? FILE_FLAG { get; set; }

    /// <summary>
    /// 书写次数: 0 不限制次数，大于0限制书写次数
    /// </summary>
    public int? WRITE_TIMES { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string CODE { get; set; }

    /// <summary>
    /// 医院代码
    /// </summary>
    public string HOSPITAL_CODE { get; set; }

    /// <summary>
    /// 模板文件（二进制格式）
    /// </summary>
    public byte[] XML_DOC { get; set; }

    /// <summary>
    /// 模板文件（文本格式）
    /// </summary>
    public string XML_DOC_NEW { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    public string PY { get; set; }

    /// <summary>
    /// 五笔
    /// </summary>
    public string WB { get; set; }

    /// <summary>
    /// 是否首次病程: 0-否, 1-是
    /// </summary>
    public int? ISFIRSTDAILY { get; set; }

    /// <summary>
    /// FILE_NAME 是否显示: 0-否, 1-是
    /// </summary>
    public int? ISSHOWFILENAME { get; set; }

    /// <summary>
    /// 是否医患沟通: 0-否, 1-是
    /// </summary>
    public int? ISYIHUANGOUTONG { get; set; }

    /// <summary>
    /// 是否新页结束
    /// </summary>
    public int? NEW_PAGE_END { get; set; }

    /// <summary>
    /// 有效性标记
    /// </summary>
    public int? VALID { get; set; }

    /// <summary>
    /// 状态: 0-保存未提交, 1-提交, 2-审核通过, 3-审核未通过
    /// </summary>
    public int? STATE { get; set; }

    /// <summary>
    /// 审核人
    /// </summary>
    public int? AUDITOR { get; set; }

    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? AUDITDATE { get; set; }

    /// <summary>
    /// 是否配置页面大小
    /// </summary>
    public int? ISCONFIGPAGESIZE { get; set; }

    /// <summary>
    /// 中医诊断
    /// </summary>
    public string ZYZhenDuan { get; set; }

    /// <summary>
    /// 西医诊断
    /// </summary>
    public string XYZhenDuan { get; set; }
}
