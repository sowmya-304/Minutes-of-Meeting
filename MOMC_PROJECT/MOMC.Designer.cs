namespace MOMC_PROJECT
{
    partial class MOMC
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MOMC));
            panel1 = new Panel();
            panel3 = new Panel();
            label6 = new Label();
            button1 = new Button();
            label4 = new Label();
            label7 = new Label();
            label3 = new Label();
            tb_otp = new TextBox();
            btn_verifyotp = new Button();
            btn_resendotp = new Button();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            label5 = new Label();
            label1 = new Label();
            label2 = new Label();
            tb_email = new TextBox();
            btn_sendotp = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.SteelBlue;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1350, 702);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(label6);
            panel3.Controls.Add(button1);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(tb_otp);
            panel3.Controls.Add(btn_verifyotp);
            panel3.Controls.Add(btn_resendotp);
            panel3.Location = new Point(425, 131);
            panel3.Name = "panel3";
            panel3.Size = new Size(502, 420);
            panel3.TabIndex = 12;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            label6.Location = new Point(164, 40);
            label6.Name = "label6";
            label6.Size = new Size(172, 30);
            label6.TabIndex = 15;
            label6.Text = "OTP Verification";
            // 
            // button1
            // 
            button1.BackColor = Color.Green;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(34, 362);
            button1.Name = "button1";
            button1.Size = new Size(84, 42);
            button1.TabIndex = 14;
            button1.Text = "<<Back";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(129, 237);
            label4.Name = "label4";
            label4.Size = new Size(43, 17);
            label4.TabIndex = 7;
            label4.Text = "label4";
            // 
            // label7
            // 
            label7.BackColor = Color.LightGreen;
            label7.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Green;
            label7.Location = new Point(37, 80);
            label7.Name = "label7";
            label7.Size = new Size(435, 67);
            label7.TabIndex = 13;
            label7.Text = "label5";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label3.Location = new Point(88, 187);
            label3.Name = "label3";
            label3.Size = new Size(84, 21);
            label3.TabIndex = 6;
            label3.Text = "Enter OTP";
            // 
            // tb_otp
            // 
            tb_otp.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tb_otp.Location = new Point(208, 184);
            tb_otp.MaxLength = 6;
            tb_otp.Name = "tb_otp";
            tb_otp.PlaceholderText = "Enter One - Time Password";
            tb_otp.Size = new Size(208, 29);
            tb_otp.TabIndex = 4;
            // 
            // btn_verifyotp
            // 
            btn_verifyotp.BackColor = Color.Green;
            btn_verifyotp.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn_verifyotp.ForeColor = Color.White;
            btn_verifyotp.Location = new Point(100, 280);
            btn_verifyotp.Name = "btn_verifyotp";
            btn_verifyotp.Size = new Size(107, 39);
            btn_verifyotp.TabIndex = 2;
            btn_verifyotp.Text = "Verify OTP";
            btn_verifyotp.UseVisualStyleBackColor = false;
            btn_verifyotp.Click += btn_verifyotp_Click;
            // 
            // btn_resendotp
            // 
            btn_resendotp.BackColor = Color.Green;
            btn_resendotp.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn_resendotp.ForeColor = Color.White;
            btn_resendotp.Location = new Point(251, 280);
            btn_resendotp.Name = "btn_resendotp";
            btn_resendotp.Size = new Size(151, 39);
            btn_resendotp.TabIndex = 8;
            btn_resendotp.Text = "Resend OTP";
            btn_resendotp.UseVisualStyleBackColor = false;
            btn_resendotp.Click += btn_resendotp_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(tb_email);
            panel2.Controls.Add(btn_sendotp);
            panel2.Location = new Point(435, 134);
            panel2.Name = "panel2";
            panel2.Size = new Size(506, 420);
            panel2.TabIndex = 11;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(204, 31);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(107, 74);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(64, 64, 64);
            label5.Location = new Point(124, 162);
            label5.Name = "label5";
            label5.Size = new Size(264, 17);
            label5.TabIndex = 6;
            label5.Text = "Please enter your email address to proceed";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(174, 118);
            label1.Name = "label1";
            label1.Size = new Size(171, 30);
            label1.TabIndex = 0;
            label1.Text = "Login to MOMC";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(72, 228);
            label2.Name = "label2";
            label2.Size = new Size(90, 21);
            label2.TabIndex = 5;
            label2.Text = "Enter Email";
            // 
            // tb_email
            // 
            tb_email.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            tb_email.Location = new Point(204, 225);
            tb_email.Name = "tb_email";
            tb_email.PlaceholderText = "Enter Email";
            tb_email.Size = new Size(268, 29);
            tb_email.TabIndex = 3;
            // 
            // btn_sendotp
            // 
            btn_sendotp.BackColor = Color.Green;
            btn_sendotp.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_sendotp.ForeColor = Color.White;
            btn_sendotp.Location = new Point(204, 296);
            btn_sendotp.Name = "btn_sendotp";
            btn_sendotp.Size = new Size(109, 39);
            btn_sendotp.TabIndex = 1;
            btn_sendotp.Text = "Continue";
            btn_sendotp.UseVisualStyleBackColor = false;
            btn_sendotp.Click += btn_sendotp_Click;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // MOMC
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1350, 702);
            Controls.Add(panel1);
            Name = "MOMC";
            Text = "MOMC";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TextBox tb_otp;
        private TextBox tb_email;
        private Button btn_verifyotp;
        private Button btn_sendotp;
        private Label label1;
        private Label label3;
        private Label label2;
        private Label label4;
        private Button btn_resendotp;
        private Panel panel3;
        private Panel panel2;
        private System.Windows.Forms.Timer timer1;
        private Label label7;
        private Button button1;
        private Label label5;
        private Label label6;
        private PictureBox pictureBox1;
    }
}
