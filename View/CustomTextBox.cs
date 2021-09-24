using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XmlCompare.View
{
    public class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            //InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            Text = "shdsklfdfds djf ";
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var backgroundBrush = new SolidBrush(Color.Transparent);
            Graphics g = e.Graphics;
            g.FillRectangle(backgroundBrush, 0, 0, this.Width, this.Height);
            //g.DrawString(Text, Font, new SolidBrush(ForeColor), new PointF(0, 0), StringFormat.GenericDefault);
        }
    }
}
