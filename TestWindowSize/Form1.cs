//MW0LGE
//Uses Form.Size to set it to 500,500
//However the form will be slightly less if there are any drop shadows.
//The Shown event will compensate for this
//
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DropShadowWindowSize
{
    public partial class frmMain : Form
    {
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private Size getShadowSize()
        {
            RECT rectWithShadow;
            if (Environment.OSVersion.Version.Major < 6)
            {
                return new Size(0, 0);
            }
            else 
            if (DwmGetWindowAttribute(this.Handle, DWMWA_EXTENDED_FRAME_BOUNDS, out rectWithShadow, Marshal.SizeOf(typeof(RECT))) == 0)
            {
                return new Size(this.Width - (rectWithShadow.right - rectWithShadow.left), this.Height - (rectWithShadow.bottom - rectWithShadow.top));
            }
            return new Size(0, 0);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Size = this.Size + getShadowSize(); // get shadow size will be fine here
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size sz = new Size(640, 320);
            this.Size = sz;
            //this.Size = sz + getShadowSize(); // get shadow size will be 0,0
            //                                  // which is incorrect
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            string s = "Form.Size = " + this.Size.ToString();
            s += "\n" + "Drop shadow = " + getShadowSize();
            s += "\n" + "Form.Size less shadow = " + (this.Size - getShadowSize()).ToString();
            lblMsg.Text = s;
        }
    }
}
