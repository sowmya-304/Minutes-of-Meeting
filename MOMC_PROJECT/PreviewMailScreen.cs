using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOMC_PROJECT
{
    public partial class PreviewMailScreen : Form
    {
        public string FromEmailAddress { get; set; }

        public List<string> ToEmailAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; } 
        public string DataGridViewContent { get; set; }
        public PreviewMailScreen()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void PreviewMailScreen_Load(object sender, EventArgs e)
        {
            label6.Text = FromEmailAddress;
            label7.Text = Subject;
            richTextBox1.Text = string.Join(", ", ToEmailAddresses);
            richTextBox3.Text = Body + Environment.NewLine + DataGridViewContent;
            foreach (string attachment in Attachments)
            {
                listBox1.Items.Add(Path.GetFileName(attachment));
            }
            using (Graphics g = CreateGraphics())
            {
                SizeF sizeTo = g.MeasureString(label6.Text, label6.Font);
                label6.Width = (int)Math.Ceiling(sizeTo.Width);
                label6.Height = (int)Math.Ceiling(sizeTo.Height);

                SizeF sizeSubject = g.MeasureString(label7.Text, label7.Font);
                label7.Width = (int)Math.Ceiling(sizeSubject.Width);
                label7.Height = (int)Math.Ceiling(sizeSubject.Height);
            }
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedAttachment = listBox1.SelectedItem.ToString();
                string selectedAttachmentPath = Attachments[listBox1.SelectedIndex];
                Process.Start(selectedAttachmentPath);
            }
        }
    }
}
