using System;
using System.Drawing;
using System.Windows.Forms;

namespace TabControlWithCloseButton
{
    public class TabControlWithCloseButton : TabControl
    {
        private int tabHeight = 40; // Tab 高度
        private Font tabFont = new Font("Microsoft YaHei", 12, FontStyle.Bold); // Tab 字体
        private Color selectedTabColor = Color.FromArgb(30, 144, 255); // 选中Tab颜色（DodgerBlue）
        private Color unselectedTabColor = Color.FromArgb(220, 220, 220); // 未选中Tab颜色（LightGray）

        public TabControlWithCloseButton()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.ItemSize = new Size(120, tabHeight); // 设定默认大小
            this.SizeMode = TabSizeMode.Fixed;
            this.DrawItem += TabControl_DrawItem;
            this.MouseDown += TabControl_MouseDown;
            this.DoubleBuffered = true; // 开启双缓冲，减少闪烁
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tab = this.TabPages[e.Index];
            Rectangle tabRect = this.GetTabRect(e.Index);

            // 选中Tab颜色
            Color bgColor = (e.State == DrawItemState.Selected) ? selectedTabColor : unselectedTabColor;
            Color textColor = (e.State == DrawItemState.Selected) ? Color.White : Color.Black;

            using (Brush brush = new SolidBrush(bgColor))
                g.FillRectangle(brush, tabRect);

            // 关闭按钮区域
            Rectangle closeButton = new Rectangle(tabRect.Right - 25, tabRect.Top + (tabHeight / 2 - 7), 14, 14);

            // 绘制Tab标题
            TextRenderer.DrawText(g, tab.Text, tabFont, new Point(tabRect.X + 10, tabRect.Y + 10), textColor);

            // 绘制关闭按钮（白色圆形背景 + 红色 X）
            using (Brush closeBrush = new SolidBrush(Color.White))
                g.FillEllipse(closeBrush, closeButton);

            using (Pen pen = new Pen(Color.Red, 2))
            {
                g.DrawLine(pen, closeButton.Left + 3, closeButton.Top + 3, closeButton.Right - 3, closeButton.Bottom - 3);
                g.DrawLine(pen, closeButton.Right - 3, closeButton.Top + 3, closeButton.Left + 3, closeButton.Bottom - 3);
            }
        }

        private void TabControl_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < this.TabPages.Count; i++)
            {
                Rectangle tabRect = this.GetTabRect(i);
                Rectangle closeButton = new Rectangle(tabRect.Right - 25, tabRect.Top + (tabHeight / 2 - 7), 14, 14);

                if (closeButton.Contains(e.Location))
                {
                    this.TabPages.RemoveAt(i);
                    UpdateTabSize();
                    break;
                }
            }
        }

        public void AddTab(string title)
        {
            TabPage newTab = new TabPage(title);
            this.TabPages.Add(newTab);
            this.SelectedTab = newTab;
            UpdateTabSize();
        }

        private void UpdateTabSize()
        {
            int maxWidth = 120; // 默认最小宽度
            using (Graphics g = this.CreateGraphics())
            {
                foreach (TabPage tab in this.TabPages)
                {
                    Size textSize = TextRenderer.MeasureText(tab.Text, tabFont);
                    maxWidth = Math.Max(maxWidth, textSize.Width + 40);
                }
            }
            this.ItemSize = new Size(maxWidth, tabHeight);
        }
    }
}
