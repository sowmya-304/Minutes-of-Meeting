using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOMC_PROJECT
{
    public partial class Draw : Form
    {
        public Draw()
        {
            InitializeComponent();
        }

        private void Draw_Load(object sender, EventArgs e)
        {
            try
            {
                DrawBoard d = new DrawBoard();
                panel1.Dock = DockStyle.Fill;
                panel1.Controls.Add(d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Draw_FormClosing(object sender, FormClosingEventArgs e)
        {



        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
