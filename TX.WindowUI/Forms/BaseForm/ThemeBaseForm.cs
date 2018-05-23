using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TX.WindowUI.Controls;
using TX.WindowUI.Mode;
using TX.WindowUI.Tools;
using TX.WindowUI.Win32;
 

namespace TX.WindowUI.Forms
{
    public partial class ThemeBaseForm : Form
    {
        bool _resizable = true;             // not with theme
        Padding _padding = new Padding(0);  // not with theme
        ThemeBaseFormEntity _myTheme;
        private ShadowForm shadowform;
        private WLButton closeBtn;
        private WLButton maxBtn;
        private WLButton resBtn;
        private WLButton minBtn;


        public ThemeBaseForm()
            :base()
        {
            InitializeComponent();
            FormIni();
            
            // 下面这个条件是永远不为true的，如果不把此类直接设置成mdicontainer
            //if (this.IsMdiContainer)
            //    SetMdiClient();
        }

        private void FormIni()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            this.UpdateStyles();
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Padding = DefaultPadding;
            base.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            base.MinimumSize= new Size(200,50);
            BtnIni();
            
        }

        /// <summary>
        /// 添加窗体右上方的按钮
        /// </summary>
        private void BtnIni()
        {
            // close box
            closeBtn = new WLButton(this);
            closeBtn.Visible = true;
            closeBtn.Bounds = this.CloseBoxRect;

            closeBtn.Click += new EventHandler(CloseBtnClick);
            closeBtn.ForePathGetter = new ButtonForePathGetter(
                    GraphicsPathHelper.CreateCloseFlagPath);

            // max res box
            if (this.MaximizeBox)
            {
                maxBtn = new WLButton(this);
                resBtn = new WLButton(this);

                if (this.WindowState == FormWindowState.Normal)
                {
                    maxBtn.Visible = true;
                    resBtn.Visible = false;
                }
                else
                {
                    maxBtn.Visible = false;
                    resBtn.Visible = true;
                }

                maxBtn.Bounds = this.MaxBoxRect;
                resBtn.Bounds = this.MaxBoxRect;

                maxBtn.Click += new EventHandler(MaxBtnClick);
                maxBtn.ForePathGetter = new ButtonForePathGetter(
                    GraphicsPathHelper.CreateMaximizeFlagPath);

                resBtn.Click += new EventHandler(ResBtnClick);
                resBtn.ForePathGetter = new ButtonForePathGetter(
                    GraphicsPathHelper.CreateRestoreFlagPath);
            }

            // min box
            if (this.MinimizeBox)
            {
                minBtn = new WLButton(this);

                minBtn.Visible = true;
                minBtn.Bounds = this.MinBoxRect;
              
                minBtn.Click += new EventHandler(MinBtnClick);
                minBtn.ForePathGetter = new ButtonForePathGetter(
                    GraphicsPathHelper.CreateMinimizeFlagPath);
            }

            SetControlBoxColor();
        }

        private void SetControlBoxColor()
        {

            closeBtn.ColorTable = this.XTheme.CloseBoxColor;
            closeBtn.BackImageNormal = this.XTheme.CloseBoxBackImageNormal;
            closeBtn.BackImageHover = this.XTheme.CloseBoxBackImageHover;
            closeBtn.BackImagePressed = this.XTheme.CloseBoxBackImagePressed;

            // max res box
            if (this.MaximizeBox)
            {
                maxBtn.ColorTable = this.XTheme.MaxBoxColor;
                maxBtn.BackImageNormal = this.XTheme.MaxBoxBackImageNormal;
                maxBtn.BackImageHover = this.XTheme.MaxBoxBackImageHover;
                maxBtn.BackImagePressed = this.XTheme.MaxBoxBackImagePressed;

                resBtn.ColorTable = this.XTheme.MaxBoxColor;
                resBtn.BackImageNormal = this.XTheme.ResBoxBackImageNormal;
                resBtn.BackImageHover = this.XTheme.ResBoxBackImageHover;
                resBtn.BackImagePressed = this.XTheme.ResBoxBackImagePressed;
            }

            // min box
            if (this.MinimizeBox)
            {
                minBtn.ColorTable = this.XTheme.MinBoxColor;
                minBtn.BackImageNormal = this.XTheme.MinBoxBackImageNormal;
                minBtn.BackImageHover = this.XTheme.MinBoxBackImageHover;
                minBtn.BackImagePressed = this.XTheme.MinBoxBackImagePressed;
            }

        }


        private void CloseBtnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MaxBtnClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void ResBtnClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void MinBtnClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        #region property

           [DefaultValue(typeof(Padding), "0")]
        public new Padding Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                base.Padding = new Padding(
                    BorderWidth + _padding.Left,
                    CaptionHeight + BorderWidth + _padding.Top,
                    BorderWidth + _padding.Right,
                    BorderWidth + _padding.Bottom);
            }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(
                    BorderWidth,
                    BorderWidth + CaptionHeight,
                    BorderWidth,
                    BorderWidth);
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Invalidate(TextRect);
            }
        }

        [Browsable(false)]
        public ThemeBaseFormEntity XTheme
        {
            get
            {
                if (_myTheme == null)
                    _myTheme = new ThemeBaseFormEntity();
                return _myTheme;
            }
            set
            {
                _myTheme = value;
                //PrepareForNewTheme();
                Invalidate();
            }
        }

        #endregion

        #region editable baseform properties

        [Category("GMForm")]
        [Description("是否可拖动改变窗体大小")]
        [DefaultValue(true)]
        public bool Resizable
        {
            get { return _resizable; }
            set
            {
                _resizable = value;
            }
        }

        [Category("GMForm")]
        [Description("窗体边界大小，鼠标移动到该边界将变成指针形状，拖动可改变窗体大小")]
        [DefaultValue(6)]
        public int SideResizeWidth
        {
            get { return XTheme.SideResizeWidth; }
            set
            {
                if (value != XTheme.SideResizeWidth)
                {
                    XTheme.SideResizeWidth = value;
                }
            }
        }

        [Category("GMForm")]
        [Description("窗体边框大小")]
        [DefaultValue(6)]
        public int BorderWidth
        { 
            get { return XTheme.BorderWidth; }
            set
            {
                if (value != XTheme.BorderWidth)
                {
                    XTheme.BorderWidth = value;
                    Invalidate();
                }
            }
        }

        [Category("GMForm")]
        [Description("标题栏高度")]
        [DefaultValue(35)]
        public int CaptionHeight
        { 
            get { return XTheme.CaptionHeight; }
            set
            {
                if (value != XTheme.CaptionHeight)
                {
                    XTheme.CaptionHeight = value;
                    Invalidate();
                }
            }
        }

        [Category("GMForm")]
        [DefaultValue(8)]
        public int Radius
        {
            get { return XTheme.Radius; }
            set
            {
                if (value != XTheme.Radius)
                {
                    XTheme.Radius = (value < 0 ? 0 : value);
                    Invalidate();
                }
            }
        }

        [Category("GMForm")]
        [DefaultValue(typeof(RoundStyle), "2")]
        public RoundStyle Round
        {
            get { return XTheme.RoundedStyle; }
            set
            {
                if (value != XTheme.RoundedStyle)
                {
                    XTheme.RoundedStyle = value;
                    Invalidate();
                }
            }
        }

        [Category("GMForm")]
        [Description("控制按钮相对于右上角的偏移量")]
        [DefaultValue(typeof(Point),"8, 8")]
        public Point ControlBoxOffset
        {
            get { return XTheme.ControlBoxOffset;}
            set
            {
                XTheme.ControlBoxOffset = value;
                Invalidate();
            }
        }

        [Category("GMForm")]
        [DefaultValue(0)]
        public int ControlBoxSpace
        {
            get { return XTheme.ControlBoxSpace; }
            set
            {
                XTheme.ControlBoxSpace = value;
                Invalidate();
            }
        }

        [Category("GMForm")]
        [DefaultValue(typeof(Size), "16,16")]
        public Size IconSize
        {
            get 
            {
                if (ShowIcon)
                    return XTheme.IconSize;
                else
                    return System.Drawing.Size.Empty;
            }
            set
            {
                XTheme.IconSize = value;
                Invalidate();
            }
        }

        [Category("GMForm")]
        [DefaultValue(2)]
        public int IconLeftMargin
        {
            get { return (this.ShowIcon ? XTheme.IconLeftMargin : 0); }
            set
            {
                XTheme.IconLeftMargin = value;
                Invalidate();
            }
        }

        [Category("GMForm")]
        [DefaultValue(2)]
        public int TextLeftMargin
        {
            get { return XTheme.TextLeftMargin; }
            set
            {
                XTheme.TextLeftMargin = value;
                Invalidate();
            }
        }

        [Category("GMForm")]
        [DefaultValue(typeof(Size), "37, 17")]
        public Size CloseBoxSize
        {
            get { return XTheme.CloseBoxSize; }
            set
            {
                XTheme.CloseBoxSize = value;
                Invalidate();
            }
        }

        [Category("GMForm")]
        [DefaultValue(typeof(Size), "25, 17")]
        public Size MaxBoxSize
        {
            get { return XTheme.MaxBoxSize; }
            set
            {
                XTheme.MaxBoxSize = value;
                Invalidate();
            }
        }

        [Category("GMForm")]
        [DefaultValue(typeof(Size), "25, 17")]
        public Size MinBoxSize
        {
            get { return XTheme.MinBoxSize; }
            set
            {
                XTheme.MinBoxSize = value;
                Invalidate();
            }
        }

        #endregion

        #region form resize region, internal readonly

        internal Rectangle TopLeftRect
        {
            get
            {
                return new Rectangle(0, 0, SideResizeWidth, SideResizeWidth);
            }
        }

        internal Rectangle TopRect
        {
            get
            {
                return new Rectangle(
                    SideResizeWidth,
                    0,
                    this.Size.Width - SideResizeWidth * 2,
                    SideResizeWidth);
            }
        }

        internal Rectangle TopRightRect
        {
            get
            {
                return new Rectangle(
                    this.Size.Width - SideResizeWidth,
                    0,
                    SideResizeWidth,
                    SideResizeWidth);
            }
        }

        internal Rectangle LeftRect
        {
            get
            {
                return new Rectangle(
                    0,
                    SideResizeWidth,
                    SideResizeWidth,
                    this.Size.Height - SideResizeWidth * 2);
            }
        }

        internal Rectangle RightRect
        {
            get
            {
                return new Rectangle(
                    this.Size.Width - SideResizeWidth,
                    SideResizeWidth,
                    SideResizeWidth,
                    this.Size.Height - SideResizeWidth * 2);
            }
        }

        internal Rectangle BottomLeftRect
        {
            get
            {
                return new Rectangle(
                    0,
                    this.Size.Height - SideResizeWidth,
                    SideResizeWidth,
                    SideResizeWidth);
            }
        }

        internal Rectangle BottomRect
        {
            get
            {
                return new Rectangle(
                    SideResizeWidth,
                    this.Size.Height - SideResizeWidth,
                    this.Size.Width - SideResizeWidth * 2,
                    SideResizeWidth);
            }
        }

        internal Rectangle BottomRightRect
        {
            get
            {
                return new Rectangle(
                    this.Size.Width - SideResizeWidth,
                    this.Size.Height - SideResizeWidth,
                    SideResizeWidth,
                    SideResizeWidth);
            }
        }

        #endregion

        #region calculated rect

        /// <summary>
        /// 标题
        /// </summary>
        internal Rectangle CaptionRect
        {
            get
            {
                return new Rectangle(
                    BorderWidth,
                    BorderWidth,
                    this.ClientSize.Width - BorderWidth * 2,
                    CaptionHeight);
            }
        }

        /// <summary>
        /// 真正工作的区域
        /// </summary>
        internal Rectangle ClientRectToDraw
        {
            get
            {
                return new Rectangle(
                      base.Padding.Left,
                      base.Padding.Top,
                     this.ClientSize.Width- base.Padding.Left*2 -1,
                     this.ClientSize.Height  - base.Padding.Top - base.Padding .Bottom -1);
            }
        }

        internal Rectangle CaptionRectToDraw
        {
            get
            {
                return new Rectangle(
                    0,
                    0,
                    this.ClientSize.Width,
                    CaptionHeight + BorderWidth);
            }
        }
        
        internal Rectangle CloseBoxRect
        {
            get
            {
                if (ControlBox)
                {
                    int x = ClientSize.Width - ControlBoxOffset.X - CloseBoxSize.Width;
                    return new Rectangle(
                        new Point(x, ControlBoxOffset.Y),
                        CloseBoxSize);
                }
                else
                    return Rectangle.Empty;
            }
        }

        internal Rectangle MaxBoxRect
        {
            get
            {
                if ( MaximizeBox)
                {
                    int x = CloseBoxRect.Left - ControlBoxSpace - MaxBoxSize.Width;
                    return new Rectangle(
                        new Point(x, ControlBoxOffset.Y),
                        MaxBoxSize);
                }
                else
                    return Rectangle.Empty;
            }
        }

        internal Rectangle MinBoxRect
        {
            get
            {
                if ( MinimizeBox)
                {
                    int x;
                    if (MaximizeBox)
                        x = MaxBoxRect.Left - ControlBoxSpace - MinBoxSize.Width;
                    else
                        x = CloseBoxRect.Left - ControlBoxSpace - MinBoxSize.Width;
                    return new Rectangle(
                        new Point(x, ControlBoxOffset.Y),
                        MinBoxSize);
                }
                else
                    return Rectangle.Empty;
            }
        }

        internal Rectangle IconRect
        {
            get
            {
                if (ControlBox && ShowIcon)
                {
                    int x = BorderWidth + IconLeftMargin;
                    int y = BorderWidth + (CaptionHeight - IconSize.Height) / 2;
                    return new Rectangle(new Point(x, y), IconSize);
                }
                else
                    return new Rectangle(BorderWidth, BorderWidth, 0, 0);
            }
        }
        /// <summary>
        /// 文本位置
        /// </summary>
        internal Rectangle TextRect
        {
            get
            {
                int x = IconRect.Right + TextLeftMargin;
                int y = BorderWidth;
                int height = CaptionHeight;
                int right = this.ClientSize.Width - x;
                if (ControlBox)
                {
                    right = CloseBoxRect.Left;
                    if (MinimizeBox)
                    {
                        right = MinBoxRect.Left;
                    }
                    else if (MaximizeBox)
                    {
                        right = MaxBoxRect.Left;
                    }
                }
                int width = right - x;
                return new Rectangle(x, y, width, height);
            }
        }

        #endregion

        #region  Win-Message handler method

        private bool WmNcActivate(ref Message m)
        {
            // something here
            m.Result = WinAPI.TRUE;
            return true;
        }

        private bool WmNcCalcSize(ref Message m)
        {
            if (m.WParam == new IntPtr(1))
            {
                WinAPI.NCCALCSIZE_PARAMS info = (WinAPI.NCCALCSIZE_PARAMS)
                    Marshal.PtrToStructure(m.LParam, typeof(WinAPI.NCCALCSIZE_PARAMS));
                if (IsAboutToMaximize(info.rectNewForm))
                {
                    Rectangle workingRect = Screen.GetWorkingArea(this);
                    info.rectNewForm.Left = workingRect.Left - BorderWidth;
                    info.rectNewForm.Top = workingRect.Top - BorderWidth;
                    info.rectNewForm.Right = workingRect.Right + BorderWidth;
                    info.rectNewForm.Bottom = workingRect.Bottom + BorderWidth;
                    Marshal.StructureToPtr(info, m.LParam, false);
                }
            }
            return true;
        }

        private bool WmNcHitTest(ref Message m)
        {
            int para = m.LParam.ToInt32();
            int x0 = WinAPI.LOWORD(para);
            int y0 = WinAPI.HIWORD(para);
            Point p = PointToClient(new Point(x0, y0));

            if (Resizable && base.WindowState != FormWindowState.Maximized )
            {
                if (TopLeftRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTTOPLEFT);
                    return true;
                }

                if (TopRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTTOP);
                    return true;
                }

                if (TopRightRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTTOPRIGHT);
                    return true;
                }

                if (LeftRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTLEFT);
                    return true;
                }

                if (RightRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTRIGHT);
                    return true;
                }

                if (BottomLeftRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTBOTTOMLEFT);
                    return true;
                }

                if (BottomRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTBOTTOM);
                    return true;
                }

                if (BottomRightRect.Contains(p))
                {
                    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTBOTTOMRIGHT);
                    return true;
                }
            }

            //if (IconRect.Contains(p))
            //{
            //    m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTSYSMENU);
            //    return true;
            //}

            if (CloseBoxRect.Contains(p) || MaxBoxRect.Contains(p) || MinBoxRect.Contains(p))
            {
                m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTCLIENT);
                return true;
            }

            //if (IsMdiContainer && _mdiBarController != null)
            //{
            //    if (_mdiBarController.HitTestBounds.Contains(p))
            //    {
            //        m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTCLIENT);
            //        return true;
            //    }
            //}

            if (CaptionRect.Contains(p))
            {
                m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTCAPTION);
                return true;
            }

            m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTCLIENT);
            return true;
        }

        #endregion

        #region override method

        protected override void WndProc(ref Message m)
        {
            bool alreadyHandled = false;

            switch (m.Msg)
            {
                case (int)WinAPI.WindowMessages.WM_NCCALCSIZE:
                    alreadyHandled = WmNcCalcSize(ref m);
                    break;

                case (int)WinAPI.WindowMessages.WM_NCHITTEST:
                    alreadyHandled = WmNcHitTest(ref m);
                    break;

                case (int)WinAPI.WindowMessages.WM_NCACTIVATE:
                    alreadyHandled = WmNcActivate(ref m);
                    break;

                case (int)WinAPI.WindowMessages.WM_NCPAINT:
                    alreadyHandled = true;
                    break;

                default:
                    break;
            }

            if (!alreadyHandled)
                base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawCaptionBackground(e.Graphics);
            DrawFormBackground(e.Graphics);
                   
           // DrawFormBorder(e.Graphics);
            DrawFormIconAndText(e.Graphics);
            closeBtn.DrawButton(e.Graphics);
            maxBtn.DrawButton(e.Graphics);
            resBtn.DrawButton(e.Graphics);
            minBtn.DrawButton(e.Graphics);

            //if (XTheme.SetClientInset)
            //    DrawInsetClientRect(e.Graphics);
 
 
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (XTheme != null && XTheme.ShowShadow)
            {
                shadowform = new ShadowForm(this);
            }
            SetFormRegion();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (shadowform!=null)
            {
                shadowform.Close();
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetWLButtonPoint();
            SetFormRegion();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ControlBox)
                SetMouseType(e.Location, MouseOperationType.Move);
         
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            if (ControlBox)
                SetMouseType(e.Location, MouseOperationType.Down);
          
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            if (e.Clicks > 1)
                return;

            if (ControlBox)
                SetMouseType(e.Location, MouseOperationType.Up);
 
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (ControlBox)
                SetMouseType(Point.Empty, MouseOperationType.Leave);
       
        }


       //Show或Hide被调用时
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                //判断不是在设计器中
                if (!DesignMode && shadowform == null && XTheme.ShowShadow )
                {
                    shadowform = new ShadowForm(this);
                    shadowform.Show(this);
                }
            }
            base.OnVisibleChanged(e);
        }



        #endregion

        #region private method

       /// <summary>
       /// 重设按钮的位置
       /// </summary>
        private void SetWLButtonPoint()
        {
            closeBtn.Bounds = this.CloseBoxRect;
            maxBtn.Bounds = this.MaxBoxRect;
            minBtn.Bounds = this.MinBoxRect;
            resBtn.Bounds = this.MaxBoxRect;
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBtn.Visible = false;
                resBtn.Visible = true;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                maxBtn.Visible = true;
                resBtn.Visible = false;
            }
            else {
                maxBtn.Visible = true;
                resBtn.Visible = false;
            }
           
         
        }

        private void SetMouseType(Point loc,MouseOperationType mtype)
        {
            closeBtn.MouseOperation(loc, mtype);
            if (maxBtn != null && maxBtn.Visible)
                maxBtn.MouseOperation(loc, mtype);
            if (resBtn != null && resBtn.Visible)
                resBtn.MouseOperation(loc, mtype);
            if (minBtn != null)
                minBtn.MouseOperation(loc, mtype);
        
        }
        
        private void SetFormMinimizeSize()
        {
            int minW = 160;
            int minH = 60;

            int w = BorderWidth * 2 + IconLeftMargin + IconSize.Width
                + TextLeftMargin + MinBoxSize.Width + MaxBoxSize.Width
                + CloseBoxSize.Width + ControlBoxSpace * 2
                + ControlBoxOffset.X + 12;
            if (w < minW)
                w = minW;
            int h = BorderWidth * 2 + CaptionHeight + 8;
            if (h < minH)
                h = minH;
            base.MinimumSize = new Size(w, h);
        }

       
        private void PrepareForNewTheme()
        {
            if (base.Region != null)
                base.Region.Dispose();
            base.Region = null;
                   
          
            //Padding = new Padding(0);
            base.BackColor = XTheme.FormBackColor;
            base.OnSizeChanged(EventArgs.Empty);
            SetFormMinimizeSize();
     
        }

        private void SetFormRegion()
        {
            if (base.Region != null)
                base.Region.Dispose();

            Rectangle rect = new Rectangle(Point.Empty, base.Size);
            GraphicsPath path;

            if (XTheme.UseDefaultTopRoundingFormRegion)
                path = GraphicsPathHelper.CreateTopRoundedPathForFormRegion(rect);
            else
                path = GraphicsPathHelper.CreateRoundedRect(rect, Radius, Round, false);

            this.Region = new Region(path);
        }

        /// <summary>
        /// 判断所接收到的 wm_nc-calc-size 消息是否指示窗体即将最小化
        /// </summary>        
        private bool IsAboutToMinimize(WinAPI.RECT rect)
        {
            if (rect.Left == -32000 && rect.Top == -32000)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断所接收到的 wm_nc-calc-size 消息是否指示窗体即将最大化
        /// </summary>        
        private bool IsAboutToMaximize(WinAPI.RECT rect)
        {
            /*
             * 判断的方法是，只要窗体的左右、上下都延伸到了屏幕工作区之外，
             * 并且左和右、上和下都延伸相同的量，就认为窗体是要进行最大化
             */

            int left = rect.Left;
            int top = rect.Top;
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            if (left <= 0 && top <= 0)
            {
                Rectangle workingArea = Screen.GetWorkingArea(this);
                if (width == (workingArea.Width + (-left) * 2)
                    && height == (workingArea.Height + (-top) * 2))
                    return true;
            }
            return false;
        }

        private void DrawFormBackground(Graphics g)
        {

            //SmoothingMode oldMode = g.SmoothingMode;
            //if (Round != RoundStyle.None)
            //    g.SmoothingMode = SmoothingMode.AntiAlias;

            //using (SolidBrush sb = new SolidBrush(XTheme.FormBackColor))
            //{
            //    using (GraphicsPath path = GraphicsPathHelper.CreateRoundedRect(
            //        ClientRectangle, Radius, Round, false))
            //    {
            //        g.FillPath(sb, path);
            //    }
            //}
            //g.SmoothingMode = oldMode;

            SmoothingMode oldMode = g.SmoothingMode;
            if (Round != RoundStyle.None)
                g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush sb = new SolidBrush(XTheme.FormBackColor))
            {
                //using (GraphicsPath path = GraphicsPathHelper.CreateRoundedRect(
                //    ClientRectToDraw, Radius, Round, false))
                //{
                //    g.FillPath(sb, path);
                //}
                g.FillRectangle(sb, ClientRectToDraw);
               

            }
            g.SmoothingMode = oldMode;

        }

     
        private void DrawCaptionBackground(Graphics g)
        {

            //using (LinearGradientBrush lb = new LinearGradientBrush(
            //     CaptionRectToDraw,
            //     XTheme.CaptionBackColorTop,
            //     XTheme.CaptionBackColorBottom,
            //     LinearGradientMode.Vertical))
            //{
            //    g.FillRectangle(lb, CaptionRectToDraw);
            //}
            using (LinearGradientBrush lb = new LinearGradientBrush(
                 base.ClientRectangle,
                 XTheme.CaptionBackColorTop,
                 XTheme.CaptionBackColorBottom,
                 LinearGradientMode.Vertical))
            {
                g.FillRectangle(lb, base.ClientRectangle);
            }

            
        }

        private void DrawClientBackground(Graphics g)
        {
            using (LinearGradientBrush lb = new LinearGradientBrush(
                 ClientRectToDraw,
                 XTheme.FormBackColor,
                 XTheme.FormBackColor,
                 LinearGradientMode.Vertical))
            {
                g.FillRectangle(lb, ClientRectToDraw);
            }
            
        }


        private void DrawFormIconAndText(Graphics g)
        {
            if (ShowIcon && Icon != null && XTheme.DrawCaptionIcon)
            {
                g.DrawIcon(this.Icon, IconRect);
            }

            if (!string.IsNullOrEmpty(Text) && XTheme.DrawCaptionText)
            {
                TextRenderer.DrawText(
                    g,
                    this.Text,
                    SystemFonts.CaptionFont,
                    TextRect,
                    XTheme.CaptionTextColor,
                    (XTheme.CaptionTextCenter ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Left) |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis);
            }
        }

        private void DrawFormBorder(Graphics g)
        {
            int width = BorderWidth;
            Rectangle rect = ClientRectangle;

            SmoothingMode oldMode = g.SmoothingMode;
            if (Round != RoundStyle.None)
                g.SmoothingMode = SmoothingMode.AntiAlias;

            // outter border
            if (width > 0)
            {
                using (Pen p = new Pen(XTheme.FormBorderOutterColor))
                {
                    using (GraphicsPath path = GraphicsPathHelper.CreateRoundedRect(
                        rect, Radius, Round, true))
                    {
                        g.DrawPath(p, path);
                    }
                }
            }
            width--;

            //// inner border
            //if (width > 0)
            //{
            //    using (Pen p = new Pen(XTheme.FormBorderInnerColor))
            //    {
            //        rect.Inflate(-1, -1);
            //        using (GraphicsPath path = GraphicsPathHelper.CreateRoundedRect(
            //            rect, Radius, Round, true))
            //        {
            //            g.DrawPath(p, path);
            //        }
            //    }
            //}
            //width--;

            g.SmoothingMode = oldMode;

            // other inside border
            using (Pen p = new Pen(XTheme.FormBorderInnerColor))
            {
                while (width > 0)
                {
                    rect.Inflate(-1, -1);
                    using (GraphicsPath path = GraphicsPathHelper.CreateRoundedRect(
                     rect, Radius, Round, true))
                    {
                        g.DrawPath(p, path);
                    }
                   // g.DrawRectangle(p, rect);
                    width--;
                }
            }
        }

        /// <summary>
        /// to make the client area to  have 3D view
        /// </summary>        
        private void DrawInsetClientRect(Graphics g)
        {
            int x = BorderWidth;
            int y = BorderWidth + CaptionHeight;
            int w = ClientSize.Width - BorderWidth * 2;
            int h = ClientSize.Height - BorderWidth * 2 - CaptionHeight;
            Rectangle clientRect = new Rectangle(x, y, w, h);
            clientRect.Width--;
            clientRect.Height--;

            Color inner = ColorHelper.GetDarkerColor(this.BackColor, 20);
            clientRect.Inflate(1, 1);
            using (Pen p1 = new Pen(inner))
            {
               // g.DrawRectangle(p1, clientRect);
                using (GraphicsPath path = GraphicsPathHelper.CreateRoundedRect(
                clientRect, Radius, Round, true))
                {
                    g.DrawPath(p1, path);
                }
            }

            Color outter = Color.FromArgb(80, 255, 255, 255);
            clientRect.Inflate(1, 1);
            using (Pen p2 = new Pen(outter))
            {
                //g.DrawRectangle(p2, clientRect);
                using (GraphicsPath path = GraphicsPathHelper.CreateRoundedRect(
                    clientRect, Radius, Round, true))
                {
                    g.DrawPath(p2, path);
                }
            }
        }

        #endregion
    }

}

