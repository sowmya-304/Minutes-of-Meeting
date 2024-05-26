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
            panel1 = new Panel();
            button2 = new Button();
            button1 = new Button();
            btn_resendotp = new Button();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            tb_otp = new TextBox();
            tb_email = new TextBox();
            btn_verifyotp = new Button();
            btn_sendotp = new Button();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btn_resendotp);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(tb_otp);
            panel1.Controls.Add(tb_email);
            panel1.Controls.Add(btn_verifyotp);
            panel1.Controls.Add(btn_sendotp);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1350, 702);
            panel1.TabIndex = 0;
            // 
            // button2
            // 
            button2.Font = new Font("Arial", 12F, FontStyle.Bold);
            button2.Location = new Point(193, 459);
            button2.Name = "button2";
            button2.Size = new Size(108, 32);
            button2.TabIndex = 10;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Arial", 12F, FontStyle.Bold);
            button1.Location = new Point(29, 459);
            button1.Name = "button1";
            button1.Size = new Size(108, 32);
            button1.TabIndex = 9;
            button1.Text = "Naviagte";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btn_resendotp
            // 
            btn_resendotp.Font = new Font("Arial", 12F, FontStyle.Bold);
            btn_resendotp.Location = new Point(833, 556);
            btn_resendotp.Name = "btn_resendotp";
            btn_resendotp.Size = new Size(166, 32);
            btn_resendotp.TabIndex = 8;
            btn_resendotp.Text = "Resend OTP";
            btn_resendotp.UseVisualStyleBackColor = true;
            btn_resendotp.Click += btn_resendotp_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(173, 100);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 7;
            label4.Text = "label4";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(411, 421);
            label3.Name = "label3";
            label3.Size = new Size(172, 37);
            label3.TabIndex = 6;
            label3.Text = "Enter OTP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(411, 206);
            label2.Name = "label2";
            label2.Size = new Size(195, 37);
            label2.TabIndex = 5;
            label2.Text = "Enter Email";
            // 
            // tb_otp
            // 
            tb_otp.Location = new Point(723, 435);
            tb_otp.MaxLength = 6;
            tb_otp.Name = "tb_otp";
            tb_otp.Size = new Size(135, 23);
            tb_otp.TabIndex = 4;
            // 
            // tb_email
            // 
            tb_email.Location = new Point(710, 206);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(300, 23);
            tb_email.TabIndex = 3;
            // 
            // btn_verifyotp
            // 
            btn_verifyotp.Font = new Font("Arial", 12F, FontStyle.Bold);
            btn_verifyotp.Location = new Point(681, 556);
            btn_verifyotp.Name = "btn_verifyotp";
            btn_verifyotp.Size = new Size(108, 32);
            btn_verifyotp.TabIndex = 2;
            btn_verifyotp.Text = "Verify OTP";
            btn_verifyotp.UseVisualStyleBackColor = true;
            btn_verifyotp.Click += btn_verifyotp_Click;
            // 
            // btn_sendotp
            // 
            btn_sendotp.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_sendotp.Location = new Point(681, 309);
            btn_sendotp.Name = "btn_sendotp";
            btn_sendotp.Size = new Size(109, 30);
            btn_sendotp.TabIndex = 1;
            btn_sendotp.Text = "Send OTP";
            btn_sendotp.UseVisualStyleBackColor = true;
            btn_sendotp.Click += btn_sendotp_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(631, 44);
            label1.Name = "label1";
            label1.Size = new Size(104, 37);
            label1.TabIndex = 0;
            label1.Text = "Login";
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
            panel1.PerformLayout();
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
        private Button button1;
        private Button button2;
    }
}
