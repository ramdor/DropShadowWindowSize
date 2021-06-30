//MW0LGE
//Uses Form.Size to set it to 500,500
//However the form will be slightly less if there are any drop shadows.
//The Shown event will compensate for this
//
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

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

        private Size m_szShadowSize;

        public frmMain()
        {
            InitializeComponent();
        }

        private Size getShadowSize()
        {
            RECT rectWithoutShadow;
            if (Environment.OSVersion.Version.Major < 6)
            {
                return new Size(0, 0);
            }
            else 
            if (DwmGetWindowAttribute(this.Handle, DWMWA_EXTENDED_FRAME_BOUNDS, out rectWithoutShadow, Marshal.SizeOf(typeof(RECT))) == 0)
            {
                return new Size(this.Width - (rectWithoutShadow.right - rectWithoutShadow.left), this.Height - (rectWithoutShadow.bottom - rectWithoutShadow.top));
            }
            return new Size(0, 0);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            m_szShadowSize = getShadowSize();
            this.Size = this.Size + m_szShadowSize; // get shadow size will be fine here
            setMsg();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_stOldWindowState = this.WindowState;

            Size sz = new Size(640, 320);
            this.Size = sz;
            //this.Size = sz + getShadowSize(); // get shadow size will be 0,0
            //                                  // which is incorrect
        }
        private FormWindowState m_stOldWindowState;
        private void Form1_Resize(object sender, EventArgs e)
        {
            if(this.WindowState != m_stOldWindowState)
            {
                m_stOldWindowState = this.WindowState;
                m_szShadowSize = getShadowSize();
            }
            setMsg();
        }

        private void setMsg()
        {
            string s = "Form.Size = " + this.Size.ToString();
            s += "\n" + "Form.ClientSize " + this.ClientSize.ToString();
            s += "\n" + "Drop shadow = " + m_szShadowSize;
            s += "\n" + "Form.Size less shadow = " + (this.Size - m_szShadowSize).ToString();
            s += "\n" + "SystemInformation.CaptionHeight = " + SystemInformation.CaptionHeight.ToString();
            s += "\n" + "SystemInformation.BorderSize = " + SystemInformation.BorderSize.ToString();
            lblMsg.Text = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Debug.Print(getShadowSize().ToString());
        }
    }
}
