using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TX.UIDemo
{
    public partial class Form1 : TX.WindowUI.Forms .ThemeBaseForm 
    {
        public Form1()
        {
            InitializeComponent();

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.XTheme.ShowShadow = false;
            this.XTheme.ShadowWidth = 10;
            this.XTheme.FormBackColor = this.BackColor;
            //this.XTheme.FormBorderInnerColor = System.Drawing.SystemColors.Control;
            //this.XTheme.FormBorderInmostColor = System.Drawing.SystemColors.Control;
        }
    }
}
