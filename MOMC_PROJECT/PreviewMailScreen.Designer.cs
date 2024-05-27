namespace MOMC_PROJECT
{
    partial class PreviewMailScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            panel1 = new Panel();
            label4 = new Label();
            panel2 = new Panel();
            label5 = new Label();
            richTextBox1 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            richTextBox3 = new RichTextBox();
            label6 = new Label();
            label7 = new Label();
            listBox1 = new ListBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(5, 23);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 0;
            label1.Text = "From      :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 81);
            label2.Name = "label2";
            label2.Size = new Size(49, 15);
            label2.TabIndex = 1;
            label2.Text = "To         :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(4, 51);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 2;
            label3.Text = "Subject  :";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveBorder;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label4);
            panel1.Location = new Point(4, 224);
            panel1.Name = "panel1";
            panel1.Size = new Size(646, 37);
            panel1.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 14);
            label4.Name = "label4";
            label4.Size = new Size(75, 15);
            label4.TabIndex = 4;
            label4.Text = "Attachments";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveBorder;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label5);
            panel2.Location = new Point(4, 400);
            panel2.Name = "panel2";
            panel2.Size = new Size(646, 37);
            panel2.TabIndex = 4;
            panel2.Paint += panel2_Paint;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 14);
            label5.Name = "label5";
            label5.Size = new Size(34, 15);
            label5.TabIndex = 4;
            label5.Text = "Body";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(55, 81);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(584, 122);
            richTextBox1.TabIndex = 5;
            richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(4, 267);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(646, 127);
            richTextBox2.TabIndex = 6;
            richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            richTextBox3.Location = new Point(4, 443);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.ReadOnly = true;
            richTextBox3.Size = new Size(646, 219);
            richTextBox3.TabIndex = 7;
            richTextBox3.Text = "";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(77, 23);
            label6.Name = "label6";
            label6.Size = new Size(38, 15);
            label6.TabIndex = 8;
            label6.Text = "label6";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(77, 51);
            label7.Name = "label7";
            label7.Size = new Size(38, 15);
            label7.TabIndex = 9;
            label7.Text = "label7";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 282);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(627, 94);
            listBox1.TabIndex = 10;
            listBox1.DoubleClick += listBox1_DoubleClick;
            // 
            // PreviewMailScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(651, 661);
            Controls.Add(listBox1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(richTextBox3);
            Controls.Add(richTextBox2);
            Controls.Add(richTextBox1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "PreviewMailScreen";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PreviewMailScreen";
            Load += PreviewMailScreen_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Panel panel1;
        private Label label4;
        private Panel panel2;
        private Label label5;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private RichTextBox richTextBox3;
        private Label label6;
        private Label label7;
        private ListBox listBox1;
    }
}