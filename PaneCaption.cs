using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WinnerHIS.Diagnosis.Clinic.DoctorWorkstation.UI
{
    public class PaneCaption : UserControl
    {
		#region Consts

		// Nested Types
		private class Consts
		{
			// Methods
			public Consts()
			{
			}


			// Fields
			public const string DefaultFontName = "arial";
			public const int DefaultFontSize = 9;
			public const int DefaultHeight = 20;
			public const int PosOffset = 4;
		}

		#endregion

		// Fields
		private bool _active;
		private bool _allowActive;
		private bool _antiAlias;
		private LinearGradientBrush _brushActive;
		private SolidBrush _brushActiveText;
		private LinearGradientBrush _brushInactive;
		private SolidBrush _brushInactiveText;
		private Color _colorActiveHigh;
		private Color _colorActiveLow;
		private Color _colorActiveText;
		private Color _colorInactiveHigh;
		private Color _colorInactiveLow;
		private Color _colorInactiveText;
		private StringFormat _format;

		private System.ComponentModel.Container components = null;

        // Methods
        public PaneCaption()
        {
            this._active = false;
            this._antiAlias = true;
            this._allowActive = true;

            this._colorActiveText = Color.Black;
            this._colorInactiveText = Color.White;
            this._colorActiveLow = Color.FromArgb(0xff, 0xa5, 0x4e);
            this._colorActiveHigh = Color.FromArgb(0xff, 0xe1, 0x9b);
            this._colorInactiveLow = Color.FromArgb(3, 0x37, 0x91);
            this._colorInactiveHigh = Color.FromArgb(90, 0x87, 0xd7);
            
			this.InitializeComponent();
            
			this.SetStyle(ControlStyles.DoubleBuffer | (ControlStyles.AllPaintingInWmPaint | (ControlStyles.ResizeRedraw | ControlStyles.UserPaint)), true);
            
			this.Height = 20;
            
			this._format = new StringFormat();
            this._format.FormatFlags = StringFormatFlags.NoWrap;
            this._format.LineAlignment = StringAlignment.Center;
            this._format.Trimming = StringTrimming.EllipsisCharacter;

            this.Font = new Font("黑体", 10f, FontStyle.Bold);
            
			this.ActiveTextColor = this._colorActiveText;
            this.InactiveTextColor = this._colorInactiveText;

            this.CreateGradientBrushes();
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

        private void CreateGradientBrushes()
        {
            if ((this.Width > 0) && (this.Height > 0))
            {
                if (this._brushActive != null)
                {
                    this._brushActive.Dispose();
                }
                this._brushActive = new LinearGradientBrush(this.DisplayRectangle, this._colorActiveHigh, this._colorActiveLow, LinearGradientMode.Vertical);
                if (this._brushInactive != null)
                {
                    this._brushInactive.Dispose();
                }
                this._brushInactive = new LinearGradientBrush(this.DisplayRectangle, this._colorInactiveHigh, this._colorInactiveLow, LinearGradientMode.Vertical);
            }
        }

		#region 组件设计器生成的代码

		[DebuggerStepThrough]
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // PaneCaption
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "PaneCaption";
            this.Size = new System.Drawing.Size(180, 50);
            this.ResumeLayout(false);

		}

		#endregion

        private void DrawCaption(Graphics g)
        {
            g.FillRectangle(this.BackBrush, this.DisplayRectangle);
            if (this._antiAlias)
            {
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }

            RectangleF ef2 = new RectangleF(4f, 0f, (float) (this.DisplayRectangle.Width - 4), (float) this.DisplayRectangle.Height);
            RectangleF ef1 = ef2;
            
			g.DrawString(this.Text, this.Font, this.TextBrush, ef1, this._format);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this._allowActive)
            {
                this.Focus();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawCaption(e.Graphics);
            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.CreateGradientBrushes();
        }

        // Properties
        [Category("Appearance"), Description("The active state of the caption, draws the caption with different gradient colors."), DefaultValue(false)]
        public bool Active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
                this.Invalidate();
            }
        }

        [Description("High color of the active gradient."), DefaultValue(typeof(Color), "255, 225, 155"), Category("Appearance")]
        public Color ActiveGradientHighColor
        {
            get
            {
                return this._colorActiveHigh;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(0xff, 0xe1, 0x9b);
                }
                this._colorActiveHigh = value;
                this.CreateGradientBrushes();
                this.Invalidate();
            }
        }

        [Description("Low color of the active gradient."), Category("Appearance"), DefaultValue(typeof(Color), "255, 165, 78")]
        public Color ActiveGradientLowColor
        {
            get
            {
                return this._colorActiveLow;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(0xff, 0xa5, 0x4e);
                }
                this._colorActiveLow = value;
                this.CreateGradientBrushes();
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Black"), Description("Color of the text when active."), Category("Appearance")]
        public Color ActiveTextColor
        {
            get
            {
                return this._colorActiveText;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.Black;
                }
                this._colorActiveText = value;
                this._brushActiveText = new SolidBrush(this._colorActiveText);
                this.Invalidate();
            }
        }

        [Description("True always uses the inactive state colors, false maintains an active and inactive state."), Category("Appearance"), DefaultValue(true)]
        public bool AllowActive
        {
            get
            {
                return this._allowActive;
            }
            set
            {
                this._allowActive = value;
                this.Invalidate();
            }
        }

        [Description("If should draw the text as antialiased."), DefaultValue(true), Category("Appearance")]
        public bool AntiAlias
        {
            get
            {
                return this._antiAlias;
            }
            set
            {
                this._antiAlias = value;
                this.Invalidate();
            }
        }

        private LinearGradientBrush BackBrush
        {
            get
            {
                return (this._active && this._allowActive) || false ? this._brushActive : this._brushInactive;
            }
        }

        [Description("High color of the inactive gradient."), Category("Appearance"), DefaultValue(typeof(Color), "90, 135, 215")]
        public Color InactiveGradientHighColor
        {
            get
            {
                return this._colorInactiveHigh;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(90, 0x87, 0xd7);
                }
                this._colorInactiveHigh = value;
                this.CreateGradientBrushes();
                this.Invalidate();
            }
        }

        [Description("Low color of the inactive gradient."), Category("Appearance"), DefaultValue(typeof(Color), "3, 55, 145")]
        public Color InactiveGradientLowColor
        {
            get
            {
                return this._colorInactiveLow;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(3, 0x37, 0x91);
                }
                this._colorInactiveLow = value;
                this.CreateGradientBrushes();
                this.Invalidate();
            }
        }

        [Description("Color of the text when inactive."), DefaultValue(typeof(Color), "White"), Category("Appearance")]
        public Color InactiveTextColor
        {
            get
            {
                return this._colorInactiveText;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.White;
                }
                this._colorInactiveText = value;
                this._brushInactiveText = new SolidBrush(this._colorInactiveText);
                this.Invalidate();
            }
        }

        [Description("Text that is displayed in the label."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("Appearance")]
        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        private SolidBrush TextBrush
        {
            get
            {
                return (SolidBrush) ((this._active && this._allowActive) || false ? this._brushActiveText : this._brushInactiveText);
            }
        }
    }
}

