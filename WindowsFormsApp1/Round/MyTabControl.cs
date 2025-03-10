using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnyBase
{
    public class MyTabControl : TabControl
    {
        public delegate string TabAddingEventHandler();
        public delegate void TabClosingEventHandler(int closedTabIndex, string closedTabName);
        public delegate void TabSwitchedEventHandler(object sender, int index, string tabName);

        public event TabSwitchedEventHandler TabSwitched;
        public event TabAddingEventHandler TabAdding;
        public event TabClosingEventHandler TabClosing;

        public Color SelectTabPageFontColor { get; set; } = Color.FromArgb(90, 196, 248);
        public Color DefaultTabPageFontColor { get; set; } = ColorTranslator.FromHtml("#333333");

        private Color defaultBackgroundColor { get; set; } = Color.FromArgb(90, 196, 248);
        private Color defaultTabBackgroundColor { get; set; } = ColorTranslator.FromHtml("#FFFFFD");
        private Color defaultBorderColor { get; set; } = Color.FromArgb(90, 196, 248);

        private bool isDrawBorder = false;
        public bool IsDrawBorder
        {
            get { return isDrawBorder; }
            set { isDrawBorder = value; }
        }

        private int closeButtonSize = 12; // 关闭按钮的大小
        private int closeButtonMargin = 5; // 关闭按钮离Tab右侧的间距
        private int hoveredIndex = -1; // 鼠标悬停的Tab索引
        private const int AddTabIndex = 0; // 固定加号Tab的索引为0

        public MyTabControl()
        {
            // 初始化控件
            this.SizeMode = TabSizeMode.Fixed;
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.ItemSize = new Size(100, 24);
            this.Margin = new Padding(0);
            this.Padding = new Point(0, 0);
            this.SelectedIndexChanged += MyTabControl_SelectedIndexChanged;
            this.MouseClick += MyTabControl_MouseClick;
            this.MouseMove += MyTabControl_MouseMove;
            this.MouseLeave += MyTabControl_MouseLeave;

            // 添加加号Tab
            TabPage addTab = new TabPage("+");
            this.TabPages.Add(addTab);

            // 设置自定义样式
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                          ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int tabSpacing = 3; // Tab 之间的间隙
            int cornerRadius = 5; // 右上角的圆角半径

            for (int i = 0; i < this.TabPages.Count; i++)
            {
                Rectangle rect = this.GetTabRect(i);
                rect = new Rectangle(rect.X + tabSpacing / 2, rect.Y, rect.Width - tabSpacing, rect.Height); // 加入间隙

                using (GraphicsPath path = CreateRightTopRoundedTabPath(rect, i == this.SelectedIndex, cornerRadius))
                {
                    using (SolidBrush brush = new SolidBrush(i == this.SelectedIndex ? defaultBackgroundColor : defaultTabBackgroundColor))
                    {
                        graphics.FillPath(brush, path);
                    }

                    using (Pen pen = new Pen(defaultBorderColor))
                    {
                        graphics.DrawPath(pen, path);
                    }
                }

                // 绘制文字
                using (StringFormat stringFormat = new StringFormat())
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    graphics.DrawString(this.TabPages[i].Text, this.Font, new SolidBrush(i == this.SelectedIndex ? SelectTabPageFontColor : DefaultTabPageFontColor), rect, stringFormat);
                }

                // 绘制关闭按钮
                if (i != 0) // 不允许删除固定的 "加号" Tab
                {
                    Rectangle closeRect = GetCloseButtonRect(rect);
                    DrawCloseButton(graphics, closeRect, i == hoveredIndex);
                }
            }
        } 
        private GraphicsPath CreateRightTopRoundedTabPath(Rectangle rect, bool isSelected, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;

            // 左上角（直角）
            path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top);

            // 顶部左侧直线
            path.AddLine(rect.Left, rect.Top, rect.Right - radius, rect.Top);

            // 右上角（弧度）
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);

            // 右侧直线
            path.AddLine(rect.Right, rect.Top + radius, rect.Right, rect.Bottom);

            path.CloseFigure();
            return path;
        }


        private void DrawCloseButton(Graphics graphics, Rectangle rect, bool hovered)
        {
            using (Pen pen = new Pen(hovered ? Color.IndianRed : Color.Black, 2))
            {
                graphics.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Bottom);
                graphics.DrawLine(pen, rect.Right, rect.Top, rect.Left, rect.Bottom);
            }
        }

        private Rectangle GetCloseButtonRect(Rectangle tabRect)
        {
            return new Rectangle(tabRect.Right - closeButtonSize - closeButtonMargin, tabRect.Top + (tabRect.Height - closeButtonSize) / 2, closeButtonSize, closeButtonSize);
        }


        private void MyTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            // 检查加号Tab是否被点击
            if (GetTabRect(AddTabIndex).Contains(e.Location))
            {
                // 触发新增Tab前的回调事件
                var title = TabAdding?.Invoke();
                if (string.IsNullOrEmpty(title))
                {
                    title = "新页面";
                }
                // 新增Tab
                TabPage newTab = new TabPage(title);
                this.TabPages.Insert(1, newTab);  // 将新Tab插入到加号后面 
                this.SelectedIndex = 1;  // 设置新Tab为当前选中Tab
                return;  // 结束事件处理
            }

            // 处理点击关闭按钮的逻辑
            for (int i = 0; i < this.TabPages.Count; i++)
            {
                // 如果是加号Tab，跳过，不进行关闭操作
                if (i == AddTabIndex)
                    continue;

                // 检查点击位置是否在关闭按钮区域内
                if (GetCloseButtonRect(GetTabRect(i)).Contains(e.Location))
                {
                    // 记录删除Tab前的索引
                    int deletedTabIndex = i;
                    int previousSelectedIndex = this.SelectedIndex; // 记录当前选中的Tab索引
                    string deletedTabName = this.TabPages[i]?.Text; // 获取被关闭Tab的名称 
                    // 点击关闭按钮，移除该Tab
                    this.TabPages.RemoveAt(i);

                    // 处理删除后的选中Tab逻辑
                    if (previousSelectedIndex == deletedTabIndex)
                    {
                        // 如果删除的是当前选中的Tab
                        if (this.TabPages.Count > 1) // 如果还有Tab剩余
                        {
                            // 如果删除的是最后一个Tab，选中前一个Tab
                            this.SelectedIndex = Math.Max(deletedTabIndex - 1, 0);
                        }
                        else
                        {
                            // 如果删除后只剩加号Tab，则选择加号Tab
                            this.SelectedIndex = AddTabIndex;
                        }
                    }
                    else if (previousSelectedIndex > deletedTabIndex)
                    {
                        // 如果删除的Tab在选中Tab之前，则选择下一个Tab
                        this.SelectedIndex = previousSelectedIndex - 1;
                    }

                    // 如果剩下的Tab只有加号Tab，选择加号Tab
                    if (this.TabPages.Count == 1) // Only the Add tab left
                    {
                        this.SelectedIndex = AddTabIndex;
                    }

                    // 触发关闭Tab后的回调事件
                    TabClosing?.Invoke(deletedTabIndex, deletedTabName);
                    break;
                }
            }
        }

        private void MyTabControl_MouseMove(object sender, MouseEventArgs e)
        {
            bool hoverFound = false;
            for (int i = 0; i < this.TabPages.Count; i++)
            {
                if (i == AddTabIndex) // 如果是加号Tab，跳过
                    continue;

                if (GetCloseButtonRect(GetTabRect(i)).Contains(e.Location))
                {
                    hoveredIndex = i;
                    this.Cursor = Cursors.Hand;
                    hoverFound = true;
                    break;
                }
            }

            if (!hoverFound)
            {
                hoveredIndex = -1;
                this.Cursor = Cursors.Default;
            }

            this.Invalidate();
        }

        private void MyTabControl_MouseLeave(object sender, EventArgs e)
        {
            hoveredIndex = -1;
            this.Cursor = Cursors.Default;
            this.Invalidate();
        }

        private void MyTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndex >= 0 && this.SelectedIndex < this.TabPages.Count)
            {
                string tabName = this.TabPages[this.SelectedIndex].Text;
                TabSwitched?.Invoke(this, this.SelectedIndex, tabName);
            }
        }

        // 添加Tab，确保加号Tab始终在第一位，其他Tab插入到第二位
        public void AddTab(TabPage tabPage)
        {
            // 将新Tab插入到第二位
            if (this.TabPages.Count > 1)
            {
                this.TabPages.Insert(1, tabPage);
            }
            else
            {
                this.TabPages.Add(tabPage);
            }
            this.SelectedIndex = 1; // 默认选择新添加的Tab
        }
    }
}
