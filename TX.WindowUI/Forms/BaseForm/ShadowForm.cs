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
using TX.WindowUI.AppCode.Tools;
using TX.WindowUI.Win32;
 

namespace TX.WindowUI.Forms
{

    //绘图层
    partial class ShadowForm : Form
    {
        public Bitmap shadowimg { get; set; }
        //控件层
        private Form Main;
        //带参构造
        public ShadowForm(Form main) {
            //将控制层传值过来
            this.Main = main;
            InitializeComponent();
            //置顶窗体
            Main.TopMost = TopMost = Main.TopMost;
            Main.BringToFront();
            //是否在任务栏显示
            ShowInTaskbar = false;
            //无边框模式
            FormBorderStyle = FormBorderStyle.None;
            //设置绘图层显示位置
            this.Location = new Point(Main.Location.X - 5, Main.Location.Y - 5);
            //设置ICO
            Icon = Main.Icon;
            ShowIcon = Main.ShowIcon;
            //设置大小
            Width = Main.Width  + 10;
            Height = Main.Height + 10;
            //设置标题名
            Text = Main.Text;
            //绘图层窗体移动
            
            Main.LocationChanged  += new EventHandler(Main_LocationChanged);
            Main.SizeChanged  += new EventHandler(Main_SizeChanged);
            Main.VisibleChanged  += new EventHandler(Main_VisibleChanged);
            //还原任务栏右键菜单
            //CommonClass.SetTaskMenu(Main);
            //加载背景
            SetBits();
            //窗口鼠标穿透效果
            CanPenetrate();

            shadowimg = Properties.Resources.main_light_bkg_top123;
        }

        #region 初始化
        private void Init() {
            //置顶窗体
            TopMost = Main.TopMost;
            Main.BringToFront();
            //是否在任务栏显示
            ShowInTaskbar = false;
            //无边框模式
            FormBorderStyle = FormBorderStyle.None;
            //设置绘图层显示位置
            this.Location = new Point(Main.Location.X - 5, Main.Location.Y - 5);
            //设置ICO
            Icon = Main.Icon;
            ShowIcon = Main.ShowIcon;
            //设置大小
            Width = Main.Width  + 10;
            Height = Main.Height +  10;
            //设置标题名
            Text = Main.Text;
            //绘图层窗体移动
            Main.LocationChanged  += new EventHandler(Main_LocationChanged);
            Main.SizeChanged  += new EventHandler(Main_SizeChanged);
            Main.VisibleChanged  += new EventHandler(Main_VisibleChanged);
            //还原任务栏右键菜单
            //CommonClass.SetTaskMenu(Main);
            //加载背景
            SetBits();
            //窗口鼠标穿透效果
            CanPenetrate();
           
        }
        #endregion

        #region 还原任务栏右键菜单
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }
        public class CommonClass
        {
            [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
            static extern int GetWindowLong(HandleRef hWnd, int nIndex);
            [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
            static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);
            public const int WS_SYSMENU = 0x00080000;
            public const int WS_MINIMIZEBOX = 0x20000;
            public static void SetTaskMenu(Form form)
            {
                int windowLong = (GetWindowLong(new HandleRef(form, form.Handle), -16));
                SetWindowLong(new HandleRef(form, form.Handle), -16, windowLong | WS_SYSMENU | WS_MINIMIZEBOX);
            }
        }
        #endregion

        #region 减少闪烁
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }
        #endregion

        #region 控件层相关事件
        //移动主窗体时
        void Main_LocationChanged(object sender, EventArgs e)
        {
            Location = new Point(Main.Left - 5, Main.Top - 5);
        }

        //主窗体大小改变时
        void Main_SizeChanged(object sender, EventArgs e) {
            //设置大小
            Width = Main.Width  + 10;
            Height = Main.Height + 10;
            SetBits();
        }
     


        //主窗体显示或隐藏时
        void Main_VisibleChanged(object sender, EventArgs e)
        {
            this.Visible = Main.Visible;
        }
        #endregion

        #region 使窗口有鼠标穿透功能
        /// <summary>
        /// 使窗口有鼠标穿透功能
        /// </summary>
        private void CanPenetrate()
        {
            int intExTemp = WinAPI.GetWindowLong(this.Handle, (int)WinAPI.WindowStyleEx.GWL_EXSTYLE);
            int oldGWLEx = WinAPI.SetWindowLong(this.Handle, (int)WinAPI.WindowStyleEx.GWL_EXSTYLE, (int)WinAPI.WindowStyleEx.WS_EX_TRANSPARENT | (int)WinAPI.WindowStyleEx.WS_EX_LAYERED);

        }
        #endregion

        #region 不规则无毛边方法
        public void SetBits() {
            //绘制绘图层背景
            Bitmap bitmap = new Bitmap(Main.Width+10, Main.Height+ 10);
            Rectangle _BacklightLTRB = new Rectangle(20, 20, 20, 20);//窗体光泽重绘边界
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality; //高质量
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量
            ImageDrawRect.DrawRect(g, shadowimg, ClientRectangle, Rectangle.FromLTRB(_BacklightLTRB.X, _BacklightLTRB.Y, _BacklightLTRB.Width, _BacklightLTRB.Height), 1, 1);

           // RenderHelper.DrawImageWithNineRect(g, shadowimg, ClientRectangle, new Rectangle { Size = shadowimg.Size });
 
            if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");

            WinAPI.POINT ptSrc = new WinAPI.POINT(0, 0);
            WinAPI.POINT ptWinPos = new WinAPI.POINT(base.Left, base.Top);
            WinAPI.SIZE szWinSize = new WinAPI.SIZE(Width, Height);
            byte biAlpha = 0xFF;
            WinAPI.BLENDFUNCTION stBlend = new WinAPI.BLENDFUNCTION(
                (byte)WinAPI.BlendOp.AC_SRC_OVER, 0, biAlpha, (byte)WinAPI.BlendOp.AC_SRC_ALPHA);

            IntPtr gdiBitMap = IntPtr.Zero;
            IntPtr memoryDC = IntPtr.Zero;
            IntPtr preBits = IntPtr.Zero;
            IntPtr screenDC = IntPtr.Zero;

            try
            {
                screenDC = WinAPI.GetDC(IntPtr.Zero);
                memoryDC = WinAPI.CreateCompatibleDC(screenDC);

                gdiBitMap = bitmap.GetHbitmap(Color.FromArgb(0));

                preBits = WinAPI.SelectObject(memoryDC, gdiBitMap);
                WinAPI.UpdateLayeredWindow(base.Handle
                    , screenDC
                    , ref ptWinPos
                    , ref szWinSize
                    , memoryDC
                    , ref ptSrc
                    , 0
                    , ref stBlend
                    , (uint)WinAPI.ULWPara.ULW_ALPHA);
            }
            finally
            {
                if (gdiBitMap != IntPtr.Zero)
                {
                    WinAPI.SelectObject(memoryDC, preBits);
                    WinAPI.DeleteObject(gdiBitMap);
                }

                WinAPI.DeleteDC(memoryDC);
                WinAPI.ReleaseDC(IntPtr.Zero, screenDC);
                g.Dispose();
                bitmap.Dispose();
            }
        }
        #endregion
    }



}
