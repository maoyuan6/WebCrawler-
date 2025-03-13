using System;

namespace EMR
{
    /// <summary>
    /// SYMBOLS表的实体模型，表示电子病历系统中的符号信息。
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// 符号ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// RTF格式的内容
        /// </summary>
        public string RTF { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 长度（可为空）
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// 备注（可为空）
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 内容（可为空）
        /// </summary>
        public string Content { get; set; }
    }
}
