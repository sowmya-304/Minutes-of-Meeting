namespace MOMC_PROJECT
{
    partial class DrawImageIcon
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            button2 = new Button();
            button1 = new Button();
            listBox1 = new ListBox();
            button6 = new Button();
            button5 = new Button();
            btn_eraser = new Button();
            btn_pen = new Button();
            btn_remove_slide = new Button();
            bt_new_slide = new Button();
            panel2 = new Panel();
            btn_shape = new Button();
            btn_Image = new Button();
            pic = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pic).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(listBox1);
            panel1.Controls.Add(button6);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(btn_eraser);
            panel1.Controls.Add(btn_pen);
            panel1.Controls.Add(btn_remove_slide);
            panel1.Controls.Add(bt_new_slide);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btn_shape);
            panel1.Controls.Add(btn_Image);
            panel1.Controls.Add(pic);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1030, 574);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // button2
            // 
            button2.Location = new Point(296, 97);
            button2.Name = "button2";
            button2.Size = new Size(54, 45);
            button2.TabIndex = 12;
            button2.Text = "Save";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(296, 35);
            button1.Name = "button1";
            button1.Size = new Size(54, 45);
            button1.TabIndex = 11;
            button1.Text = "Color";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(3, 182);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(175, 379);
            listBox1.TabIndex = 10;
            // 
            // button6
            // 
            button6.Location = new Point(193, 66);
            button6.Name = "button6";
            button6.Size = new Size(54, 45);
            button6.TabIndex = 9;
            button6.Text = "Redo";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Location = new Point(133, 66);
            button5.Name = "button5";
            button5.Size = new Size(54, 45);
            button5.TabIndex = 8;
            button5.Text = "Undo";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // btn_eraser
            // 
            btn_eraser.Location = new Point(73, 66);
            btn_eraser.Name = "btn_eraser";
            btn_eraser.Size = new Size(54, 45);
            btn_eraser.TabIndex = 7;
            btn_eraser.Text = "Eraser";
            btn_eraser.UseVisualStyleBackColor = true;
            btn_eraser.Click += btn_eraser_Click;
            // 
            // btn_pen
            // 
            btn_pen.Location = new Point(13, 66);
            btn_pen.Name = "btn_pen";
            btn_pen.Size = new Size(54, 45);
            btn_pen.TabIndex = 6;
            btn_pen.Text = "Pen";
            btn_pen.UseVisualStyleBackColor = true;
            btn_pen.Click += btn_pen_Click;
            // 
            // btn_remove_slide
            // 
            btn_remove_slide.Location = new Point(193, 15);
            btn_remove_slide.Name = "btn_remove_slide";
            btn_remove_slide.Size = new Size(54, 45);
            btn_remove_slide.TabIndex = 5;
            btn_remove_slide.Text = "Remove Slide";
            btn_remove_slide.UseVisualStyleBackColor = true;
            btn_remove_slide.Click += btn_remove_slide_Click;
            // 
            // bt_new_slide
            // 
            bt_new_slide.Location = new Point(133, 15);
            bt_new_slide.Name = "bt_new_slide";
            bt_new_slide.Size = new Size(54, 45);
            bt_new_slide.TabIndex = 4;
            bt_new_slide.Text = "New Slide";
            bt_new_slide.UseVisualStyleBackColor = true;
            bt_new_slide.Click += bt_new_slide_Click;
            // 
            // panel2
            // 
            panel2.Location = new Point(193, 182);
            panel2.Name = "panel2";
            panel2.Size = new Size(630, 203);
            panel2.TabIndex = 3;
            panel2.Paint += panel2_Paint;
            panel2.MouseDown += panel2_MouseDown;
            panel2.MouseMove += panel2_MouseMove;
            panel2.MouseUp += panel2_MouseUp;
            // 
            // btn_shape
            // 
            btn_shape.Location = new Point(780, 35);
            btn_shape.Name = "btn_shape";
            btn_shape.Size = new Size(54, 45);
            btn_shape.TabIndex = 2;
            btn_shape.Text = "Shape";
            btn_shape.UseVisualStyleBackColor = true;
            btn_shape.Click += btn_shape_Click;
            // 
            // btn_Image
            // 
            btn_Image.Location = new Point(13, 15);
            btn_Image.Name = "btn_Image";
            btn_Image.Size = new Size(54, 45);
            btn_Image.TabIndex = 1;
            btn_Image.Text = "Image";
            btn_Image.UseVisualStyleBackColor = true;
            // 
            // pic
            // 
            pic.BorderStyle = BorderStyle.FixedSingle;
            pic.Location = new Point(375, 391);
            pic.Name = "pic";
            pic.Size = new Size(358, 139);
            pic.TabIndex = 0;
            pic.TabStop = false;
            pic.Paint += pic_Paint;
            pic.MouseDown += pic_MouseDown;
            pic.MouseMove += pic_MouseMove_1;
            pic.MouseUp += pic_MouseUp;
            // 
            // DrawImageIcon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "DrawImageIcon";
            Size = new Size(1030, 574);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pic).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btn_Image;
        private PictureBox pic;
        private Button btn_shape;
        private Panel panel2;
        private ListBox listBox1;
        private Button button6;
        private Button button5;
        private Button btn_eraser;
        private Button btn_pen;
        private Button btn_remove_slide;
        private Button bt_new_slide;
        private Button button1;
        private Button button2;
    }
}
