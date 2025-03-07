using System;
using System.Drawing;
using System.Windows.Forms;

public class BorderedTextBox : TextBox
{
    private int _borderSize = 1; // 边框厚度
    private Color _borderColor = Color.Gray; // 默认边框颜色
    private Color _borderFocusColor = Color.FromArgb(90, 196, 248); // 获取焦点时边框颜色
    private bool _isFocused = false; // 是否获取焦点
    private bool _isMouseDown = false; // 是否按下鼠标

    public int BorderSize
    {
        get => _borderSize;
        set { _borderSize = value; Invalidate(); }
    }

    public Color BorderColor
    {
        get => _borderColor;
        set { _borderColor = value; Invalidate(); }
    }

    public Color BorderFocusColor
    {
        get => _borderFocusColor;
        set { _borderFocusColor = value; Invalidate(); }
    }

    public BorderedTextBox()
    {
        this.BorderStyle = BorderStyle.None; // 取消默认边框
        this.Padding = new Padding(5, 8, 5, 8); // 增加上下边距，使高度更合理
        this.MinimumSize = new Size(100, 30); // 设置合理的最小高度
        this.Font = new Font("Microsoft YaHei", 10F, FontStyle.Regular); // 适配更大字体
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (m.Msg == 0xF) // WM_PAINT 消息
        {
            using (Graphics g = Graphics.FromHwnd(Handle))
            using (Pen pen = new Pen(
                _isMouseDown ? _borderColor : (_isFocused ? _borderFocusColor : _borderColor),
                _borderSize))
            {
                Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                g.DrawRectangle(pen, rect);
            }
        }
    }

    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        _isFocused = true;
        Invalidate();
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        _isFocused = false;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        _isMouseDown = true;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _isMouseDown = false;
        Invalidate();
    }
}
