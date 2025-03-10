using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TextBoxDemo
{
    public partial class MyTextBox : UserControl
    {
        private bool isMove = false;
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public override string Text
        {
            get => this.tbContent.Text;
            set
            {
                this.tbContent.Text = value;
            }
        }
        [Browsable(true)]
        public new event EventHandler TextChanged;
        private Color borderColor = Color.FromArgb(90, 196, 248); // 选中和输入时边框颜色
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                this.tbContent.BackColor = value;
                this.Invalidate();
            }
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                this.tbContent.Font = value;
                SizeChange();
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                this.tbContent.ForeColor = value;
            }
        }


        public int MaxLength
        {
            get => this.tbContent.MaxLength;
            set
            {
                this.tbContent.MaxLength = value;
            }
        }

        public int SelectionStart
        {
            get => this.tbContent.SelectionStart;
            set
            {
                this.tbContent.SelectionStart = value;
            }
        }

        public string SelectedText
        {
            get => this.tbContent.SelectedText;
            set
            {
                if (this.tbContent != null && this.tbContent.IsHandleCreated)
                {
                    this.tbContent.SelectedText = value;
                }
            }
        }

        public int SelectionLength
        {
            get => this.tbContent.SelectionLength;
            set
            {
                this.tbContent.SelectionLength = value;
            }
        }


        public bool ReadOnly
        {
            get
            {
                return this.tbContent.ReadOnly;
            }
            set
            {
                this.tbContent.ReadOnly = value;
            }
        }

        private int leftMargin = 16;

        public int LeftMargin
        {
            get { return leftMargin; }
            set { leftMargin = value; }
        }

        private int rightMargin = 20;

        public int RightMargin
        {
            get { return rightMargin; }
            set { rightMargin = value; }
        }


        private bool isMiddle = false;

        public bool IsMiddle
        {
            get { return isMiddle; }
            set { isMiddle = value; }
        }


        public Action<object, KeyEventArgs> ActionKeyUp;

        public MyTextBox()
        {
            InitializeComponent();
            this.SetStyle(
                         ControlStyles.OptimizedDoubleBuffer |  // 该控件首先在缓冲区中绘制，而不是直接绘制到屏幕上，这样可以减少闪烁  
                         ControlStyles.AllPaintingInWmPaint,         // 控件将忽略 WM_ERASEBKGND 窗口消息以减少闪烁  
                            true);
            this.tbContent.LostFocus += TbContent_LostFocus;
            this.tbContent.KeyUp += TbContentKeyUp;
        }

        private void TbContentKeyUp(object sender, KeyEventArgs e)
        {
            ActionKeyUp?.Invoke(this, e);
        }

        public void SetFocus()
        {
            tbContent.Focus();
        }

        private void TbContent_LostFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.tbContent.BackColor = this.BackColor;
            FillRadius(this.ClientRectangle, graphics, this.BackColor, true, true, true, true);
            if (this.Enabled)
            {
                this.tbContent.BackColor = this.BackColor;
                FillRadius(this.ClientRectangle, graphics, this.BackColor, true, true, true, true);
            }
            else
            {
                this.tbContent.BackColor = ColorTranslator.FromHtml("#E5E5E5");
                FillRadius(this.ClientRectangle, graphics, ColorTranslator.FromHtml("#E5E5E5"), true, true, true, true);
            }

            if (isMove == true)
            {
                DrawRadius(this.ClientRectangle, graphics, Color.FromArgb(90, 196, 248), true, true, true, true);
            }
            else
            {
                DrawRadius(this.ClientRectangle, graphics, this.borderColor, true, true, true, true);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SizeChange();
        }

        private void SizeChange()
        {
            this.tbContent.Top = (Height - tbContent.Height) / 2;
            this.tbContent.Left = LeftMargin;
            this.tbContent.Width = Width - rightMargin;
            if (isMiddle)
            {
                this.tbContent.TextAlign = HorizontalAlignment.Center;
            }
        }

        public void SelectAll()
        {
            this.tbContent.SelectAll();
        }


        private void BiolabTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            isMove = true;
            this.Invalidate();
        }

        private void BiolabTextBox1_MouseLeave(object sender, EventArgs e)
        {
            isMove = false;
            this.Invalidate();
        }

        private void tbContent_MouseMove(object sender, MouseEventArgs e)
        {
            isMove = true;
            this.Invalidate();
        }

        private void tbContent_MouseLeave(object sender, EventArgs e)
        {
            isMove = false;
            this.Invalidate();
        }

        private void tbContent_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void tbContent_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void BiolabTextBox_Load(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void tbContent_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.TextChanged?.Invoke(this, e);
        }

        private void tbContent_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }

        private void DrawRadius(Rectangle rectangle, Graphics graphics, Color color, bool leftTop = true, bool rightTop = true, bool rightBottom = true, bool leftBottom = true, int penWidth = 1, int radius = 5)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath graphicsPath = new GraphicsPath();
            int x = rectangle.X;
            int y = rectangle.Y;
            int width = rectangle.Width - 1;
            int height = rectangle.Height - 2;

            int diameter = radius * 2;

            if (leftTop)
            {
                //左上圆角
                graphicsPath.AddArc(x, y, diameter, diameter, 180, 90);
                if (rightTop)
                {
                    //上边
                    graphicsPath.AddLine(x + radius, y, x + width - radius, y);
                }
                else
                {
                    //上边
                    graphicsPath.AddLine(x + radius, y, x + width, y);
                }
            }
            else
            {
                if (rightTop)
                {
                    //上边
                    graphicsPath.AddLine(x, y, x + width - radius, y);
                }
                else
                {
                    //上边
                    graphicsPath.AddLine(x, y, x, y);
                }
            }


            if (rightTop)
            {
                //右上圆角
                graphicsPath.AddArc(x + width - diameter, y, diameter, diameter, 270, 90);
                if (rightBottom)
                {
                    //右边
                    graphicsPath.AddLine(x + width, y + radius, x + width, y + height - radius);
                }
                else
                {
                    //右边
                    graphicsPath.AddLine(x + width, y + radius, x + width, y + height);
                }
            }
            else
            {
                if (rightBottom)
                {
                    //右边
                    graphicsPath.AddLine(x + width, y, x + width, y + height - radius);
                }
                else
                {
                    //右边
                    graphicsPath.AddLine(x + width, y, x + width, y + height);
                }
            }

            if (rightBottom)
            {
                //右下圆角
                graphicsPath.AddArc(x + width - diameter, y + height - diameter, diameter, diameter, 0, 90);
                if (leftBottom)
                {
                    //下边
                    graphicsPath.AddLine(x + width - radius, y + height, x + radius, y + height);
                }
                else
                {
                    //下边
                    graphicsPath.AddLine(x + width - radius, y + height, x, y + height);
                }
            }
            else
            {
                if (leftBottom)
                {
                    //下边
                    graphicsPath.AddLine(x + width, y + height, x + radius, y + height);
                }
                else
                {
                    //下边
                    graphicsPath.AddLine(x + width, y + height, x, y + height);
                }

            }

            if (leftBottom)
            {
                //左下圆角
                graphicsPath.AddArc(x, y + height - diameter, diameter, diameter, 90, 90);
                if (leftTop)
                {
                    //左边
                    graphicsPath.AddLine(x, y + height - radius, x, y + radius);
                }
                else
                {
                    //左边
                    graphicsPath.AddLine(x, y + height - radius, x, y);
                }
            }
            else
            {
                if (leftTop)
                {
                    //左边
                    graphicsPath.AddLine(x, y + height, x, y + radius);
                }
                else
                {
                    //左边
                    graphicsPath.AddLine(x, y + height, x, y);
                }
            }
            graphics.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.DrawPath(new Pen(color, penWidth), graphicsPath);
            graphics.SmoothingMode = SmoothingMode.Default;
            graphics.InterpolationMode = InterpolationMode.Default;
            graphics.CompositingQuality = CompositingQuality.Default;
        }

        private void FillRadius(Rectangle rectangle, Graphics graphics, Color color, bool leftTop = true, bool rightTop = true, bool rightBottom = true, bool leftBottom = true, int penWidth = 1, int radius = 5)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath graphicsPath = new GraphicsPath();
            int x = rectangle.X;
            int y = rectangle.Y;
            int width = rectangle.Width - 1;
            int height = rectangle.Height - 2;

            int diameter = radius * 2;

            if (leftTop)
            {
                //左上圆角
                graphicsPath.AddArc(x, y, diameter, diameter, 180, 90);
                if (rightTop)
                {
                    //上边
                    graphicsPath.AddLine(x + radius, y, x + width - radius, y);
                }
                else
                {
                    //上边
                    graphicsPath.AddLine(x + radius, y, x + width, y);
                }
            }
            else
            {
                if (rightTop)
                {
                    //上边
                    graphicsPath.AddLine(x, y, x + width - radius, y);
                }
                else
                {
                    //上边
                    graphicsPath.AddLine(x, y, x, y);
                }
            }


            if (rightTop)
            {
                //右上圆角
                graphicsPath.AddArc(x + width - diameter, y, diameter, diameter, 270, 90);
                if (rightBottom)
                {
                    //右边
                    graphicsPath.AddLine(x + width, y + radius, x + width, y + height - radius);
                }
                else
                {
                    //右边
                    graphicsPath.AddLine(x + width, y + radius, x + width, y + height);
                }
            }
            else
            {
                if (rightBottom)
                {
                    //右边
                    graphicsPath.AddLine(x + width, y, x + width, y + height - radius);
                }
                else
                {
                    //右边
                    graphicsPath.AddLine(x + width, y, x + width, y + height);
                }
            }

            if (rightBottom)
            {
                //右下圆角
                graphicsPath.AddArc(x + width - diameter, y + height - diameter, diameter, diameter, 0, 90);
                if (leftBottom)
                {
                    //下边
                    graphicsPath.AddLine(x + width - radius, y + height, x + radius, y + height);
                }
                else
                {
                    //下边
                    graphicsPath.AddLine(x + width - radius, y + height, x, y + height);
                }
            }
            else
            {
                if (leftBottom)
                {
                    //下边
                    graphicsPath.AddLine(x + width, y + height, x + radius, y + height);
                }
                else
                {
                    //下边
                    graphicsPath.AddLine(x + width, y + height, x, y + height);
                }

            }

            if (leftBottom)
            {
                //左下圆角
                graphicsPath.AddArc(x, y + height - diameter, diameter, diameter, 90, 90);
                if (leftTop)
                {
                    //左边
                    graphicsPath.AddLine(x, y + height - radius, x, y + radius);
                }
                else
                {
                    //左边
                    graphicsPath.AddLine(x, y + height - radius, x, y);
                }
            }
            else
            {
                if (leftTop)
                {
                    //左边
                    graphicsPath.AddLine(x, y + height, x, y + radius);
                }
                else
                {
                    //左边
                    graphicsPath.AddLine(x, y + height, x, y);
                }
            }
            graphics.FillPath(new SolidBrush(color), graphicsPath);
        }
    }
}
