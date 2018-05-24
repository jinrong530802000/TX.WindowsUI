using System;
using System.Drawing;

namespace TX.WindowUI.Controls
{
    public class RollingBarThemeBase
    {// <summary>
        /// 半径1
        /// </summary>
        public int Radius1 { get; set; }
        /// <summary>
        /// 半径2
        /// </summary>
        public int Radius2 { get; set; }
        /// <summary>
        /// 轮辐
        /// </summary>
        public int SpokeNum { get; set; }
        /// <summary>
        /// 画笔大小
        /// </summary>
        public float PenWidth { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color BackColor { get; set; }
        /// <summary>
        /// 底色
        /// </summary>
        public Color BaseColor { get; set; }
      
        public Color DiamondColor { get; set; }

        /// <summary>
        /// 样式名称
        /// </summary>
        public string Name { get; set; }

        public RollingBarThemeBase()
        {
            Name = "default";
            Radius1 = 18;
            Radius2 = 24;
            SpokeNum = 12;
            PenWidth = 2;
            BackColor = Color.Transparent;
            BaseColor = Color.DarkGray;
            DiamondColor = Color.White;
        }

        public void  ChangeBarThemeBase(RollingBarStyle rstype)
        {

            if (rstype == RollingBarStyle.BigGuyLeadsLittleGuys)
            {
                SpokeNum = 36;
                Radius1 = 30;
                Radius2 = 20;
                PenWidth = 4;
                BaseColor = Color.DarkGray;
            }
            else if (rstype == RollingBarStyle.ChromeOneQuarter)
            {
                Radius1 = 14;
                BaseColor = System.Drawing.Color.LightSeaGreen;
                BackColor = System.Drawing.Color.White;
            }
            else if (rstype == RollingBarStyle.DiamondRing)
            {
                Radius1 = 18;
                BaseColor = Color.Gold;
                DiamondColor = Color.FromArgb(160, Color.Red);
            }
            else
            {
                Radius1 = 10;
                Radius2 = 24;
                SpokeNum = 12;
                PenWidth = 2;
                BackColor = Color.Transparent;
                BaseColor = Color.DarkGray;
                DiamondColor = Color.White;
            }
                
   
        }

    }
}
