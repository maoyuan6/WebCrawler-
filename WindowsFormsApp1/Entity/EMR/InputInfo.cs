using System;

namespace EMR
{
    /// <summary>
    /// InputInfo 表的实体模型，存储输入信息。
    /// </summary>
    public class InputInfo
    {
        /// <summary>
        /// 文件名（主键）
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 备选文本
        /// </summary>
        public string BackText { get; set; }
    }
}
