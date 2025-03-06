using Clinic.Case.Business;
using Clinic.Case.Interface;
using DCSoft.Design;
using DCSoft.Writer;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Extension;
using DCSoft.Writer.Extension.Data;
using DevExpress.XtraEditors.Controls;
using HPSoft.Library.EmrEditor.Src.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinnerHIS.Common;
//using iNethinkPE.Helper;

namespace HIS.Clinic.ClinicCase.UI
{
    public partial class MzCotextEditor : UserControl
    {
        private DataSourceTreeViewControler dstvControler = null;
        int editSatus = 0;//0模板，1页眉，2页脚，3子模板
        public IEMRTEMPLET empTemplet; //病历模板
        public IEMRTEMPLET_HEADER empTempletHeader; //页眉模板
        public IEMRTEMPLET_FOOT empTempletfoot; //页脚模板
        public bool isEdit;//判断是否是编辑模式
        public int deptid =0;
        public string mrClass="";
        public MzCotextEditor()
        {
            InitializeComponent();
            this.myWriterControl.SelectionChanged += new DCSoft.Writer.WriterEventHandler(this.MyWriterControl_SelectionChanged);
            //this.myWriterControl.EventInsertObject += new DCSoft.Writer.InsertObjectEventHandler(this.myWriterControl_EventInsertObject);
            this.myWriterControl.EventCanInsertObject += new DCSoft.Writer.CanInsertObjectEventHandler(this.myWriterControl_EventCanInsertObject);
            this.myWriterControl.EventReadFileContent += new WriterReadFileContentEventHandler(myWriterControl_EventReadFileContent);
            this.myWriterControl.EventEndPrint += new WriterPrintEventHandler(myWriterControl_EventEndPrint);
            myWriterControl.DocumentControler = new DocumentControlerExt();// 设置为设计器使用的文档控制器
            myWriterControl.DocumentControler = new DocumentControler();// 设置为普通的文档控制器



            WestDiagnosis listerner = new WestDiagnosis();
            listerner.Name = "西医诊断";
            myWriterControl.EventTemplates.Add(listerner);
            TCMDiagnosis listerner2 = new TCMDiagnosis();
            listerner2.Name = "中医诊断";
            myWriterControl.EventTemplates.Add(listerner2);
        }

        private void PadForm_Load(object sender, EventArgs e)
        {
            DCSoft.Writer.Controls.WriterControl.StaticSetRegisterCode("02390605000512000000E69F90E69F90E8BDAFE4BBB6E585ACE58FB8EA4A12000000E69F90E69F90E8BDAFE4BBB6E585ACE58FB8010000001A3500001C00A1CF76FF");

            myWriterControl.LicenceDisplayMode = (DCLicenceDisplayMode)3;  //虚拟一种不存在的模式
            myWriterControl.SpecifyLoadFileNameOnce = "正在加载-智念医疗电子病历文件,请稍后...";

            myWriterControl.ContextMenuStrip = this.CMStripTable;
            //myWriterControl.DocumentOptions.SecurityOptions.EnablePermission = true;
            // myWriterControl.DocumentOptions.SecurityOptions.EnableLogicDelete = true;


            this.myWriterControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.myWriterControl.CurrentPageBorderColor = System.Drawing.Color.Blue;
            this.myWriterControl.AllowDragContent = true;//拖拽
            this.myWriterControl.AllowDrop = true;
            this.myWriterControl.MoveFocusHotKey = MoveFocusHotKeys.Tab;//移动焦点快捷键          
            this.myWriterControl.SetCommandEnableHotKey("ExecuteCommand", true);//强制某些快捷键是否启用
            this.myWriterControl.DocumentOptions.BehaviorOptions.PageLineUnderPageBreak = true;
            // this.myEditControl.DocumentOptions.BehaviorOptions.ParagraphFlagFollowTableOrSection = false;
            //myWriterControl.DocumentOptions.BehaviorOptions.RemoveLastParagraphFlagWhenInsertDocument = true;
            this.myWriterControl.DocumentOptions.BehaviorOptions.EnableScript = true;//启用VB脚本
            this.myWriterControl.RuleVisible = true;//标尺
            this.myWriterControl.BackColor = System.Drawing.SystemColors.GradientActiveCaption;

            this.myWriterControl.PageBorderColor = System.Drawing.Color.White; //设置边框线

            this.myWriterControl.ExecuteCommand("Zoom", false, "125%");  //缩放比率
            this.myWriterControl.CommandControler = this.writerCommandControler1;
            this.myWriterControl.CommandControler.Start();


            myWriterControl.Enabled = true;

            InitFont("", "");
            myWriterControl.ExecuteCommand("ZoomReset" , false, null);
        }

        #region myWriterControl_Event

        private void myWriterControl_EventInsertObject(object sender, InsertObjectEventArgs args)
        {
            if (this.dstvControler != null)
            {
                this.dstvControler.HandleInsertObjectEvent(myWriterControl, args);
            }
            if (args.DataObject.GetDataPresent("特殊字符"))
            {
                string text = Convert.ToString(args.DataObject.GetData("特殊字符"));
                if (!string.IsNullOrEmpty(text))
                {
                    this.myWriterControl.ExecuteCommand("InsertString", false, text);
                }
                args.Result = true;
            }
            object v = GetSupportInstance(args.DataObject);
            if (v is XTextInputFieldElement)
            {
                XTextInputFieldElement p = (XTextInputFieldElement)v;
                // 根据属性说明在创建一个文本输入域原始
                XTextInputFieldElement field = new XTextInputFieldElement();
                field.BackgroundText = p.BackgroundText;
                field.Name = p.Name;
                // 将输入域对象插入到文档当前位置
                this.myWriterControl.ExecuteCommand("InsertInputField", false, field);
                args.Result = true;
                args.Handled = true;
            }
            else if (v is ComponentTypeInfo)
            {
                ComponentTypeInfo info = (ComponentTypeInfo)v;
                // 调用InsertControlHost命令
                object result = this.myWriterControl.ExecuteCommand("InsertControlHost", false, info.FullName);
                args.Result = result != null;
                args.Handled = true;
                return;
            }

            args.Result = false;
        }
        /// <summary>
        /// 获得能向编辑器插入的对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private object GetSupportInstance(IDataObject data)
        {
            foreach (string format in data.GetFormats())
            {
                if (format.IndexOf("XTextInputFieldElement") >= 0)
                {
                    object v = data.GetData(format);
                    if (v is XTextInputFieldElement)
                    {
                        return v;
                    }
                }
                if (format.IndexOf(typeof(ComponentTypeInfo).Name) >= 0)
                {
                    object v = data.GetData(format);
                    if (v is ComponentTypeInfo)
                    {
                        return v;
                    }
                }
            }
            return null;
        }


        void myWriterControl_EventReadFileContent(object eventSender, WriterReadFileContentEventArgs args)
        {
            return;

            //加载模板内容
            //string Objectsql = @"  Select ObjectData From ET_Document where ObjectID ='" + args.FileName + "' ";
            //DataSet Objectds = null;// this.m_app.SqlHelperHis.ExecuteDataSet(Objectsql);

            //if (Objectds.Tables[0].Rows.Count > 0)
            //{
            //    string txt = Objectds.Tables[0].Rows[0]["ObjectData"].ToString();

            //    if (txt != null)
            //    {
            //        if (txt.IndexOf("<emrtextdoc") >= 0)
            //        {
            //            args.FileFormat = "OldXml";
            //        }
            //        else
            //        {
            //            args.FileFormat = "xml";
            //        }
            //    }
            //    byte[] bs = System.Text.Encoding.UTF8.GetBytes(txt);
            //    args.ResultBinary = bs;
            //}
        }

        private void myWriterControl_EventCanInsertObject(object sender, CanInsertObjectEventArgs args)
        {
            if (this.dstvControler != null)
            {
                this.dstvControler.HandleCanInsertObjectEvent(myWriterControl, args);
            }
            if (args.DataObject.GetDataPresent("特殊字符")
                || args.DataObject.GetDataPresent("图形")
                || args.DataObject.GetDataPresent("文档元素"))
            {
                args.Result = true;
            }
        }



        private void MyWriterControl_SelectionChanged(object eventSender, WriterEventArgs args)
        {
            if (this.dstvControler != null)
            {
                this.dstvControler.UpdateCurrentDataSourceNode(myWriterControl);
            }

        }

        void myWriterControl_EventEndPrint(object eventSender, WriterPrintEventEventArgs args)
        {

        }

        #endregion


        #region 特殊字符 知识库


        private void myWriterControl_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof(Button))))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void myWriterControl_EventMouseDblClickExt(object eventSender, WriterMouseEventArgs args)
        {
            //if (args.Element is XTextImageElement)
            //{
            //    string fileName = args.Element.GetAttribute("FileName");
            //    if (System.IO.File.Exists(fileName))
            //    {
            //        // 存在对应的高清图片
            //        using (frmShowImage frm = new frmShowImage())
            //        {
            //            frm.ImageFileName = fileName;
            //            frm.WindowState = FormWindowState.Maximized;
            //            frm.ShowDialog(this);
            //        }
            //        args.Handled = true;
            //    }
            //}
        }
        /// <summary>
        /// 递归填充树状结构
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="entries"></param>
        private void FillKBNode(TreeNodeCollection nodes, KBEntryList entries)
        {
            foreach (KBEntry entry in entries)
            {
                TreeNode node = new TreeNode(entry.Text);
                node.Tag = entry;
                nodes.Add(node);
                if (entry.SubEntries != null && entry.SubEntries.Count > 0)
                {
                    // 递归填充子节点
                    FillKBNode(node.Nodes, entry.SubEntries);
                }
            }
        }


        /// <summary>
        /// 加图片
        /// </summary>
        //void LayoutFree()
        //{
        //    // 查找容纳图片的单元格对象，需要事先在检查单模板中放置好单元格。
        //    XTextTableCellElement rootCell = this.myWriterControl.GetElementById("cellImages") as XTextTableCellElement;
        //    if (rootCell == null)
        //    {
        //        return;
        //    }
        //    // 清空单元格内容
        //    rootCell.Elements.Clear();
        //    List<ImageListItem> selectedItems = new List<ImageListItem>();
        //    foreach (ImageListItem item in lstItems_cc.SelectedItems)
        //    {
        //        if (selectedItems.Count == 8)
        //        {
        //            // 不能选择超过8个图片
        //            break;
        //        }
        //        selectedItems.Add(item);
        //    }
        //    if (selectedItems.Count > 0)
        //    {
        //        // 标准的一行的图片的个数
        //        int stdImageCountOneLine = 4;

        //        float clientHeight = rootCell.ClientHeight - 20;
        //        float clientWidth = rootCell.ClientWidth - 20;
        //        int imgCount = selectedItems.Count;
        //        if (selectedItems.Count > stdImageCountOneLine)
        //        {
        //            clientHeight = clientHeight / 2;
        //            imgCount = stdImageCountOneLine;
        //        }
        //        // 一行图片的总宽度
        //        float totalImageWidth = 0;
        //        // 一行图片的平均高度
        //        float avgImageHeight = 0;
        //        for (int iCount = 0; iCount < imgCount; iCount++)
        //        {
        //            ImageListItem item = selectedItems[iCount];
        //            totalImageWidth = totalImageWidth + item.Image.Width;
        //            avgImageHeight = avgImageHeight + item.Image.Height;
        //        }
        //        avgImageHeight = avgImageHeight / imgCount;
        //        totalImageWidth += 80;
        //        float imgHeight = clientHeight;
        //        if (clientWidth / clientHeight < (totalImageWidth) / avgImageHeight)
        //        {
        //            // 图片比较宽,重新计算图片元素高度
        //            imgHeight = clientWidth / (totalImageWidth / avgImageHeight);
        //        }
        //        // 动态的插入图片内容
        //        foreach (ImageListItem item in selectedItems)
        //        {
        //            System.Drawing.Image img = (Image)item.Image.Clone();
        //            XTextImageElement imgElement = new XTextImageElement();
        //            imgElement.Image = new DCSoft.Drawing.XImageValue(img);
        //            imgElement.SetAttribute("FileName", item.FileName);
        //            imgElement.Height = imgHeight;
        //            imgElement.Width = imgElement.Height * img.Width / img.Height;
        //            imgElement.Title = "双击查看高清大图";
        //            rootCell.Elements.Add(imgElement);
        //            // 插一个空格
        //            XTextStringElement str = new XTextStringElement();
        //            str.Text = " ";
        //            rootCell.Elements.Add(str);
        //        }
        //    }
        //    XTextParagraphFlagElement flag = new XTextParagraphFlagElement();
        //    flag.Style.Align = DCSoft.Drawing.DocumentContentAlignment.Center;
        //    rootCell.Elements.Add(flag);
        //    myWriterControl.ExecuteCommand("ClearUndo", false, null);
        //    myWriterControl.RefreshDocument();
        //}

        #endregion

        public class ImageListItem
        {
            public void InitControl(System.Windows.Forms.ListBox ctl)
            {
                if (ctl == null)
                {
                    throw new ArgumentNullException("ctl");
                }
                ctl.DrawMode = DrawMode.OwnerDrawFixed;
                ctl.ItemHeight = 200;
                ctl.DrawItem += new DrawItemEventHandler(ctl_DrawItem);
                ctl.Disposed += new EventHandler(ctl_Disposed);
            }

            void ctl_Disposed(object sender, EventArgs e)
            {
                System.Windows.Forms.ListBox ctl = (System.Windows.Forms.ListBox)sender;
                foreach (ImageListItem item in ctl.Items)
                {
                    if (item.Image != null)
                    {
                        item.Image.Dispose();
                        item.Image = null;
                    }
                }
                ctl.Items.Clear();
            }

            void ctl_DrawItem(object sender, DrawItemEventArgs e)
            {
                if (e.Index < 0)
                {
                    return;
                }
                System.Windows.Forms.ListBox ctl = (System.Windows.Forms.ListBox)sender;
                ImageListItem item = (ImageListItem)ctl.Items[e.Index];
                //e.DrawBackground();
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                if (item.Image != null)
                {
                    DrawImageCenter(e.Graphics, item.Image, e.Bounds);
                }
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    using (Pen p = new Pen(Color.Blue, 3))
                    {
                        e.Graphics.DrawRectangle(
                            p,
                            e.Bounds.Left + 3,
                            e.Bounds.Top + 3,
                            e.Bounds.Width - 6,
                            e.Bounds.Height - 6);
                    }
                }
                e.DrawFocusRectangle();
            }

            /// <summary>
            /// 以居中方式绘制图片，自动缩放图片来填充区域，并保持图片的长宽比
            /// </summary>
            /// <param name="g">画布对象</param>
            /// <param name="img">图片对象</param>
            /// <param name="bounds">要显示的区域</param>
            /// <returns>实际显示图片的区域</returns>
            public System.Drawing.Rectangle DrawImageCenter(
                System.Drawing.Graphics g,
                System.Drawing.Image img,
                System.Drawing.Rectangle bounds)
            {
                if (g == null)
                {
                    throw new ArgumentNullException("g");
                }
                if (img == null)
                {
                    throw new ArgumentNullException("img");
                }
                if (bounds.Width <= 0 || bounds.Height <= 0)
                {
                    return Rectangle.Empty;
                }
                int viewWidth = bounds.Width;
                int viewHeight = bounds.Height;
                if ((float)bounds.Width / (float)bounds.Height > (float)img.Width / (float)img.Height)
                {
                    // 绘制区域特别宽、图片特别窄
                    viewWidth = bounds.Height * img.Width / img.Height;
                    viewHeight = bounds.Height;
                }
                else
                {
                    // 绘制区域特别窄、图片特别宽
                    viewWidth = bounds.Width;
                    viewHeight = bounds.Width * img.Height / img.Width;
                }
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                    bounds.Left + (bounds.Width - viewWidth) / 2,
                    bounds.Top + (bounds.Height - viewHeight) / 2,
                    viewWidth,
                    viewHeight);
                g.DrawImage(img, rect);
                return rect;
            }

            private System.Drawing.Image _Image = null;
            /// <summary>
            /// 预览图片
            /// </summary>
            public System.Drawing.Image Image
            {
                get { return _Image; }
                set { _Image = value; }
            }

            private string _FileName = null;
            /// <summary>
            /// 原始图片文件名
            /// </summary>
            public string FileName
            {
                get { return _FileName; }
                set { _FileName = value; }
            }
        }





        #region 表格
        //        myWriterControl.ExecuteCommand("Table_DeleteTable", false, null);
        //myWriterControl.ExecuteCommand("Table_DeleteRow", false, null);
        //myWriterControl.ExecuteCommand("Table_DeleteColumn", false, null);
        //myWriterControl.ExecuteCommand("Table_MergeCell", false, null);
        private void InsertTable()
        {
            //TableInsert tableInsertForm = new TableInsert();
            //if (tableInsertForm.ShowDialog() == DialogResult.OK)
            //{
            //    if (tableInsertForm.Rows > 0 && tableInsertForm.Columns > 0)
            //    {
            //        //创建table
            //        XTextTableElement xTextTableElement = new XTextTableElement();
            //        xTextTableElement.Columns.Add(new XTextTableColumnElement());

            //        //创建行
            //        for (int i = 0; i < tableInsertForm.Rows; i++)
            //        {
            //            XTextTableRowElement xTextTableRowElement = new XTextTableRowElement();
            //            xTextTableElement.Rows.Add(xTextTableRowElement);

            //            //创建列
            //            for (int j = 0; j < tableInsertForm.Columns; j++)
            //            {

            //                XTextTableCellElement xTextTableCellElement = new XTextTableCellElement();
            //                xTextTableCellElement.Width = float.Parse(tableInsertForm.ColumnWidth.ToString()) * 10;
            //                xTextTableRowElement.Cells.Add(xTextTableCellElement);
            //            }
            //        }

            //        //xTextTableCellElement.SetInnerTextFast(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //        myWriterControl.ExecuteCommand("Table_InsertTable", false, xTextTableElement);
            //    }
            //    else
            //    {
            //        MessageBox.Show("行或列数必须大于0");
            //    }



            //}

        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 插入行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertTableRow();
        }

        private void InsertTableRow()
        {
            //RowsInsert rowsInsertForm = new RowsInsert();
            //if (rowsInsertForm.ShowDialog() == DialogResult.OK)
            //{
            //    if (rowsInsertForm.IsBack == false)
            //    {
            //        //上方插入                  
            //        // myWriterControl.ExecuteCommand("Table_InsertRowUp", false, null);

            //    }
            //    else
            //    {
            //        //下方插入                
            //        //  myWriterControl.ExecuteCommand("Table_InsertRowDown", false, null);
            //    }
            //}
        }

        private void 插入列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertTableColumn();
        }

        private void InsertTableColumn()
        {
            //ColumnsInsert columnsInsertForm = new ColumnsInsert();
            //if (columnsInsertForm.ShowDialog() == DialogResult.OK)
            //{
            //    if (columnsInsertForm.IsBack == false)
            //    {
            //        //  myWriterControl.ExecuteCommand("Table_InsertColumnLeft", false, null);
            //    }
            //    else
            //    {
            //        //  myWriterControl.ExecuteCommand("Table_InsertColumnRight", false, null);

            //    }
            //}
        }



        private void 拆分表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ApartCell apartCellForm = new ApartCell(1, 1);

            //if (apartCellForm.ShowDialog() == DialogResult.OK)
            //{
            //    // 执行拆分单元格命令
            //    this.myWriterControl.ExecuteCommand("Table_SplitCellExt", false, "" + apartCellForm.intRow + "," + apartCellForm.intColumn + "");
            //}
        }

        private void 表格属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.myWriterControl.ExecuteCommand("TableProperties", false, null);
        }






        #endregion

        #region 顶部菜单事件
        private void menuFile_Click(object sender, EventArgs e)
        {

        }
        //粗体
        private void btn_Bold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("Bold", false, null);
        }
        //斜体
        private void btn_Italy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("Italic", false, null);
        }

        private void btn_Undline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("Underline", false, null);
        }

        private void btn_Sump_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("Superscript", false, null);
        }

        private void btn_Sumb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("Subscript", false, null);
        }
        //左对齐
        private void btn_left_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("AlignLeft", false, null);
        }
        //居中对齐
        private void btn_center_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("AlignCenter", false, null);
        }
        //右对齐
        private void btn_right_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("AlignRight", false, null);
        }
        //分散
        private void btn_disperse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("AlignDistribute", false, null);
        }
        //插入表格
        private void btn_InsertTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TableEdit tab = new TableEdit();
            if (tab.ShowDialog() == DialogResult.OK)
            {
                List<int> nums = tab.Tag as List<int>;
                XTextTableElement table = new XTextTableElement();
                table.ID = tab.txtID.Text;
                for (int i = 0; i < tab.numColumn.Value; i++)
                {
                    XTextTableColumnElement column = new XTextTableColumnElement();
                    table.Columns.Add(column);
                }
                for (int i = 0; i < tab.numRow.Value; i++)
                {
                    XTextTableRowElement row = new XTextTableRowElement();
                    table.Rows.Add(row);
                    for (int j = 0; j < tab.numRow.Value; j++)
                    {
                        XTextTableCellElement cell = new XTextTableCellElement();
                        row.Cells.Add(cell);
                    }//for
                }//for
                myWriterControl.ExecuteCommand("Table_InsertTable", true, table);
            }



        }
        //向下插入行
        private void btn_InsertUpRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myWriterControl.CurrentTable == null)
            {
                return;
            }
            TableCommandArgs args = new TableCommandArgs();
            args.TableID = myWriterControl.CurrentTable.ID;
            args.RowIndex = myWriterControl.CurrentTableRow.Index;
            myWriterControl.ExecuteCommand("Table_InsertRowUp", false, args);
        }

        private void btn_InsertDownRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myWriterControl.CurrentTable == null)
            {
                return;
            }
            //myWriterControl.ExecuteCommand("Table_InsertRowUp", false, null);
            TableCommandArgs args = new TableCommandArgs();
            args.TableID = myWriterControl.CurrentTable.ID;
            args.RowIndex = myWriterControl.CurrentTableRow.Index;
            myWriterControl.ExecuteCommand("Table_InsertRowDown", false, args);
        }

        private void btn_InsertLeftCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myWriterControl.CurrentTable == null)
            {
                return;
            }
            TableCommandArgs args = new TableCommandArgs();
            args.TableID = myWriterControl.CurrentTable.ID;
            args.ColIndex = myWriterControl.CurrentTableCell.ColIndex;
            myWriterControl.ExecuteCommand("Table_InsertColumnLeft", false, args);
        }

        private void btn_InsertRightCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myWriterControl.CurrentTable == null)
            {
                return;
            }
            TableCommandArgs args = new TableCommandArgs();
            args.TableID = myWriterControl.CurrentTable.ID;
            args.ColIndex = myWriterControl.CurrentTableCell.ColIndex;
            myWriterControl.ExecuteCommand("Table_InsertColumnRight", false, args);
        }
        /// <summary>
        /// 初始化字体参数
        /// </summary>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        private void InitFont(string fontName, string fontSize)
        {
            try
            {

                int sizeIndex = 0;
                int defIndex = 0;
                repositoryItemComboBoxFont.Items.AddRange(FontCommon.FontList.ToArray());
                //添加字体名称
                foreach (string fontNameTmp in FontCommon.FontList)
                {
                    if (fontNameTmp == fontName)
                        defIndex = sizeIndex;
                    sizeIndex++;
                }
                //字体名称
                //this.btn_FontName.EditValue = ((DevExpress.XtraEditors.Repository.RepositoryItemComboBox)btn_FontName.Edit).Items[defIndex];
                this.btn_FontName.EditValue = repositoryItemComboBoxFont.Items[defIndex];

                sizeIndex = 0;
                defIndex = 0;
                //添加字号
                foreach (string fontsizename in FontCommon.allFontSizeName)
                {
                    if (fontsizename == fontSize)
                        defIndex = sizeIndex;
                    repositoryItemComboBox_SiZe.Items.Add(fontsizename);
                    sizeIndex++;
                }
                //字体大小
                //this.btn_FontSize.EditValue = ((DevExpress.XtraEditors.Repository.RepositoryItemComboBox)btn_FontSize.Edit).Items[defIndex];
                this.btn_FontSize.EditValue = repositoryItemComboBox_SiZe.Items[defIndex];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_FontName_EditValueChanged(object sender, EventArgs e)
        {
            if (btn_FontName.EditValue != null)
            {
                myWriterControl.ExecuteCommand("FontName", false, btn_FontName.EditValue.ToString());
            }
        }

        private void btn_FontSize_EditValueChanged(object sender, EventArgs e)
        {
            if (btn_FontName.EditValue != null)
            {
                myWriterControl.ExecuteCommand("FontSize", false, btn_FontSize.EditValue.ToString());
            }
        }

        private void btn_FontColor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ColorDialog color = new ColorDialog();
            color.ShowDialog();
            myWriterControl.ExecuteCommand("Color", false, color.Color);
        }
        //删除行
        private void btn_DeleteRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myWriterControl.CurrentTable == null)
            {
                return;
            }
            TableCommandArgs args = new TableCommandArgs();
            args.TableID = myWriterControl.CurrentTable.ID;
            args.RowIndex = myWriterControl.CurrentTableRow.Index;
            myWriterControl.ExecuteCommand("Table_DeleteRow", false, args);
        }
        //删除列
        private void btn_DeleteCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myWriterControl.CurrentTable == null)
            {
                return;
            }
            TableCommandArgs args = new TableCommandArgs();
            args.TableID = myWriterControl.CurrentTable.ID;
            args.ColIndex = myWriterControl.CurrentTableCell.ColIndex;
            myWriterControl.ExecuteCommand("Table_DeleteColumn", false, args);
        }
        //删除表格
        private void btn_DeleteTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TableCommandArgs args = new TableCommandArgs();
            args.TableID = myWriterControl.CurrentTable.ID;
            myWriterControl.ExecuteCommand("Table_DeleteColumn", false, args);
        }
        //合并单元格
        private void btn_Merge_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("Table_MergeCell", false, null);
        }

        private void btn_Split_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myWriterControl.CurrentTable == null)
            {
                return;
            }
            SplitCellEdit splitfrm = new SplitCellEdit(myWriterControl.CurrentTableCell.RowSpan);
            if (splitfrm.ShowDialog() == DialogResult.OK)
            {
                SplitCellExtCommandParameter parameter = new SplitCellExtCommandParameter();
                parameter.CellElement = myWriterControl.CurrentTableCell;
                parameter.NewColSpan = (int)splitfrm.numCol.Value;
                parameter.NewRowSpan = (int)splitfrm.numRow.Value;
                myWriterControl.ExecuteCommand("Table_SplitCellExt", false, parameter);
            }
        }

        private void btn_TableProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (myWriterControl.CurrentTable == null)
            //{
            //    return;
            //}
            //TableProperty frm = new TableProperty(myWriterControl.CurrentTable);
            //myWriterControl.DocumentOptions.ViewOptions.ShowCellNoneBorder = true;
            //TableCommandArgs args = new TableCommandArgs();
            //args.TableID = myWriterControl.CurrentTable.ID;
            //myWriterControl.ExecuteCommand("Table_CellGridLine", false, true); 
        }
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("FileSaveAs", true, null);
        }

        private void btnInputText_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("InsertInputField", true, null);
        }

        private void btnInputRadio_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("InsertRadioBox", true, null);
        }

        private void btnInputCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("InsertCheckBox", true, null);
        }

        private void btnInputRadioOrCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("InsertCheckBoxes", true, null);
        }

        private void btnPageInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("InsertPageInfo", true, null);
        }
        #endregion

        private void btn_AddNewDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TempletEdit tempFrm = new TempletEdit(deptid,mrClass);
            if (tempFrm.ShowDialog() == DialogResult.OK)
            {
                isEdit = false;//新增模式
                empTemplet = WinnerHIS.Clinic.Case.DAL.Interface.DALHelper.DALManager.CreataEMRTEMPLET();
                empTemplet.Session = CacheHelper.Session;
                empTemplet.MR_NAME = tempFrm.txtName.Text;
                empTemplet.MR_CLASS = tempFrm.cmbType.EditValue.ToString();
                empTemplet.FILE_NAME = tempFrm.txtTitle.Text;
                empTemplet.DEPT_ID = Convert.ToInt32(tempFrm.cmbDept.EditValue);
                empTemplet.ISSHOWFILENAME = tempFrm.checkShowTitle.Checked ? 1 : 0;
                foreach (CheckedListBoxItem item in tempFrm.checklist.CheckedItems)
                {
                    switch (item.Value)
                    {
                        case 0://首次病程
                            empTemplet.ISFIRSTDAILY = 1;
                            break;
                        case 1://新页结束
                            empTemplet.NEW_PAGE_END = 1;
                            break;
                        case 2://页面配置
                            empTemplet.ISCONFIGPAGESIZE = 1;
                            break;
                        case 3://新页开始
                            empTemplet.NEW_PAGE_FLAG = 1;
                            break;
                        case 4://医患沟通
                            empTemplet.ISYIHUANGOUTONG = 1;
                            break;

                    }
                }
                myWriterControl.ExecuteCommand("FileNew", true, null);
                editSatus = 0;
            }
      
        }
        private void btn_AddHeader_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("FileNew", true, null);
            editSatus = 1;
        }
        private void btn_Add_Templet_Foot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("FileNew", true, null);
            editSatus = 2;
        }
        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存的时候出现了异常：" + ex.Message);
                return; 
            }
            finally
            {

            }
        }
        private void btn_ModifiedHeader_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HeadEdit head = new HeadEdit();
            head.ShowDialog();
        }

        private void btn_ModifiedFooter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FootEdit foot = new FootEdit();
            foot.ShowDialog();
        }

        private void btn_InsertImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("InsertImage", true, null);
        }

        private void btn_PageSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myWriterControl.ExecuteCommand("FilePageSettings", true, null);
        }
    }
}
