using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedButton : Button
{
    private int _cornerRadius = 8; // 圆角大小
    private int _borderSize = 1;   // 边框厚度
    private Color _borderColor = Color.FromArgb(90, 196, 248); // 边框颜色

    public int CornerRadius
    {
        get => _cornerRadius;
        set { _cornerRadius = value; Refresh(); }
    }

    public int BorderSize
    {
        get => _borderSize;
        set { _borderSize = value; Refresh(); }
    }

    public Color BorderColor
    {
        get => _borderColor;
        set { _borderColor = value; Refresh(); }
    }

    public RoundedButton()
    {
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0; // 取消默认边框
        this.FlatAppearance.MouseDownBackColor = Color.Transparent;
        this.FlatAppearance.MouseOverBackColor = Color.Transparent;
        this.BackColor = Color.Transparent;
        this.ForeColor = Color.FromArgb(90, 196, 248);
        this.Resize += (s, e) => SetRoundedRegion();
    }

    private void SetRoundedRegion()
    {
        if (this.Width > 0 && this.Height > 0)
        {
            using (GraphicsPath path = GetRoundedPath(this.Width, this.Height, _cornerRadius))
            {
                this.Region = new Region(path);
            }
        }
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        Graphics g = pevent.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (GraphicsPath path = GetRoundedPath(this.Width - 1, this.Height - 1, _cornerRadius))
        using (Pen pen = new Pen(_borderColor, _borderSize))
        {
            pen.Alignment = PenAlignment.Inset; // 让边框向内绘制，防止右下消失
            g.DrawPath(pen, path);
        }
    }

    private GraphicsPath GetRoundedPath(int width, int height, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int adjust = _borderSize; // 让边框绘制在按钮的内部，避免右下消失

        path.AddArc(adjust, adjust, radius, radius, 180, 90);
        path.AddArc(width - radius - adjust, adjust, radius, radius, 270, 90);
        path.AddArc(width - radius - adjust, height - radius - adjust, radius, radius, 0, 90);
        path.AddArc(adjust, height - radius - adjust, radius, radius, 90, 90);
        path.CloseFigure();

        return path;
    }
}
