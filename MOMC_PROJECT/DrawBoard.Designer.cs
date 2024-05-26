namespace MOMC_PROJECT
{
    partial class DrawBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawBoard));
            panel1 = new Panel();
            panel4 = new Panel();
            txtRename = new TextBox();
            trackBar2 = new TrackBar();
            trackBar1 = new TrackBar();
            lstSlides = new ListBox();
            panel3 = new Panel();
            btnPreviousSlide = new Button();
            label1 = new Label();
            btnNextSlide = new Button();
            panel2 = new Panel();
            panel5 = new Panel();
            label2 = new Label();
            btn_select = new Button();
            btn_delete = new Button();
            btn_uplaod = new Button();
            btn_close = new Button();
            btn_clear = new Button();
            btnRemoveSlide = new Button();
            btn_color = new Button();
            btnAddSlide = new Button();
            btn_SaveAll = new Button();
            btn_redo = new Button();
            btn_SaveClose = new Button();
            btn_undo = new Button();
            btn_image = new Button();
            pic_color = new Button();
            btn_shape = new Button();
            picbox_color_picker = new PictureBox();
            btn_fill = new Button();
            btn_pencil = new Button();
            btn_eraser = new Button();
            pic = new PictureBox();
            label3 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picbox_color_picker).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pic).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gray;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(txtRename);
            panel1.Controls.Add(trackBar2);
            panel1.Controls.Add(trackBar1);
            panel1.Controls.Add(lstSlides);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pic);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1366, 741);
            panel1.TabIndex = 0;
            panel1.MouseMove += panel1_MouseMove;
            // 
            // panel4
            // 
            panel4.AutoScroll = true;
            panel4.BackColor = Color.White;
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Location = new Point(259, 92);
            panel4.Name = "panel4";
            panel4.Size = new Size(260, 162);
            panel4.TabIndex = 18;
            // 
            // txtRename
            // 
            txtRename.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRename.Location = new Point(3, 92);
            txtRename.Name = "txtRename";
            txtRename.Size = new Size(205, 23);
            txtRename.TabIndex = 25;
            // 
            // trackBar2
            // 
            trackBar2.AutoSize = false;
            trackBar2.Location = new Point(678, 90);
            trackBar2.Maximum = 50;
            trackBar2.Minimum = 1;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(286, 36);
            trackBar2.TabIndex = 17;
            trackBar2.TickFrequency = 5;
            trackBar2.Value = 10;
            trackBar2.ValueChanged += trackBar2_ValueChanged;
            trackBar2.MouseUp += trackBar2_MouseUp;
            // 
            // trackBar1
            // 
            trackBar1.AutoSize = false;
            trackBar1.Location = new Point(678, 90);
            trackBar1.Maximum = 50;
            trackBar1.Minimum = 1;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(286, 36);
            trackBar1.TabIndex = 16;
            trackBar1.TickFrequency = 5;
            trackBar1.Value = 10;
            trackBar1.ValueChanged += trackBar1_ValueChanged;
            trackBar1.MouseUp += trackBar1_MouseUp;
            // 
            // lstSlides
            // 
            lstSlides.BorderStyle = BorderStyle.FixedSingle;
            lstSlides.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lstSlides.FormattingEnabled = true;
            lstSlides.ItemHeight = 20;
            lstSlides.Location = new Point(3, 90);
            lstSlides.Name = "lstSlides";
            lstSlides.Size = new Size(205, 562);
            lstSlides.TabIndex = 23;
            lstSlides.SelectedIndexChanged += lstSlides_SelectedIndexChanged;
            lstSlides.DoubleClick += lstSlides_DoubleClick;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(64, 64, 64);
            panel3.Controls.Add(btnPreviousSlide);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(btnNextSlide);
            panel3.Location = new Point(0, 658);
            panel3.Name = "panel3";
            panel3.Size = new Size(1366, 83);
            panel3.TabIndex = 1;
            // 
            // btnPreviousSlide
            // 
            btnPreviousSlide.Image = (Image)resources.GetObject("btnPreviousSlide.Image");
            btnPreviousSlide.Location = new Point(1220, 3);
            btnPreviousSlide.Name = "btnPreviousSlide";
            btnPreviousSlide.Size = new Size(41, 33);
            btnPreviousSlide.TabIndex = 19;
            btnPreviousSlide.UseVisualStyleBackColor = true;
            btnPreviousSlide.Click += btnPreviousSlide_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(759, 14);
            label1.Name = "label1";
            label1.Size = new Size(61, 22);
            label1.TabIndex = 26;
            label1.Text = "label1";
            // 
            // btnNextSlide
            // 
            btnNextSlide.Image = (Image)resources.GetObject("btnNextSlide.Image");
            btnNextSlide.Location = new Point(1302, 3);
            btnNextSlide.Name = "btnNextSlide";
            btnNextSlide.Size = new Size(41, 35);
            btnNextSlide.TabIndex = 20;
            btnNextSlide.UseVisualStyleBackColor = true;
            btnNextSlide.Click += btnNextSlide_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(64, 64, 64);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(btn_uplaod);
            panel2.Controls.Add(btn_close);
            panel2.Controls.Add(btn_clear);
            panel2.Controls.Add(btnRemoveSlide);
            panel2.Controls.Add(btn_color);
            panel2.Controls.Add(btnAddSlide);
            panel2.Controls.Add(btn_SaveAll);
            panel2.Controls.Add(btn_redo);
            panel2.Controls.Add(btn_SaveClose);
            panel2.Controls.Add(btn_undo);
            panel2.Controls.Add(btn_image);
            panel2.Controls.Add(pic_color);
            panel2.Controls.Add(btn_shape);
            panel2.Controls.Add(picbox_color_picker);
            panel2.Controls.Add(btn_fill);
            panel2.Controls.Add(btn_pencil);
            panel2.Controls.Add(btn_eraser);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1366, 89);
            panel2.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.BackColor = Color.Black;
            panel5.Controls.Add(label2);
            panel5.Controls.Add(btn_select);
            panel5.Controls.Add(btn_delete);
            panel5.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel5.ForeColor = Color.White;
            panel5.Location = new Point(743, 6);
            panel5.Name = "panel5";
            panel5.Size = new Size(169, 80);
            panel5.TabIndex = 22;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(31, 60);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 19;
            label2.Text = "Select && Delete";
            // 
            // btn_select
            // 
            btn_select.BackColor = Color.White;
            btn_select.Cursor = Cursors.Hand;
            btn_select.Image = (Image)resources.GetObject("btn_select.Image");
            btn_select.Location = new Point(19, 5);
            btn_select.Name = "btn_select";
            btn_select.Size = new Size(58, 52);
            btn_select.TabIndex = 10;
            btn_select.TextAlign = ContentAlignment.BottomCenter;
            btn_select.UseVisualStyleBackColor = false;
            btn_select.Click += btn_select_Click;
            // 
            // btn_delete
            // 
            btn_delete.BackColor = Color.White;
            btn_delete.BackgroundImage = (Image)resources.GetObject("btn_delete.BackgroundImage");
            btn_delete.BackgroundImageLayout = ImageLayout.None;
            btn_delete.Location = new Point(96, 6);
            btn_delete.Name = "btn_delete";
            btn_delete.Size = new Size(58, 51);
            btn_delete.TabIndex = 11;
            btn_delete.TextAlign = ContentAlignment.BottomCenter;
            btn_delete.UseVisualStyleBackColor = false;
            btn_delete.Click += btn_delete_Click;
            // 
            // btn_uplaod
            // 
            btn_uplaod.BackColor = Color.White;
            btn_uplaod.Image = (Image)resources.GetObject("btn_uplaod.Image");
            btn_uplaod.ImageAlign = ContentAlignment.TopCenter;
            btn_uplaod.Location = new Point(1110, 4);
            btn_uplaod.Name = "btn_uplaod";
            btn_uplaod.Size = new Size(58, 80);
            btn_uplaod.TabIndex = 15;
            btn_uplaod.Text = "Upload\r\n";
            btn_uplaod.TextAlign = ContentAlignment.BottomCenter;
            btn_uplaod.UseVisualStyleBackColor = false;
            btn_uplaod.Click += btn_uplaod_Click;
            // 
            // btn_close
            // 
            btn_close.BackColor = Color.White;
            btn_close.Image = (Image)resources.GetObject("btn_close.Image");
            btn_close.ImageAlign = ContentAlignment.TopCenter;
            btn_close.Location = new Point(1302, 6);
            btn_close.Name = "btn_close";
            btn_close.Size = new Size(58, 80);
            btn_close.TabIndex = 18;
            btn_close.Text = "Close";
            btn_close.TextAlign = ContentAlignment.BottomCenter;
            btn_close.UseVisualStyleBackColor = false;
            btn_close.Click += btn_close_Click;
            // 
            // btn_clear
            // 
            btn_clear.BackColor = Color.White;
            btn_clear.Image = (Image)resources.GetObject("btn_clear.Image");
            btn_clear.ImageAlign = ContentAlignment.TopCenter;
            btn_clear.Location = new Point(1046, 3);
            btn_clear.Name = "btn_clear";
            btn_clear.Size = new Size(58, 80);
            btn_clear.TabIndex = 14;
            btn_clear.Text = "Clear";
            btn_clear.TextAlign = ContentAlignment.BottomCenter;
            btn_clear.UseVisualStyleBackColor = false;
            btn_clear.Click += btn_clear_Click;
            // 
            // btnRemoveSlide
            // 
            btnRemoveSlide.BackColor = Color.White;
            btnRemoveSlide.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 64, 64);
            btnRemoveSlide.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 255, 128);
            btnRemoveSlide.Image = (Image)resources.GetObject("btnRemoveSlide.Image");
            btnRemoveSlide.ImageAlign = ContentAlignment.TopCenter;
            btnRemoveSlide.Location = new Point(67, 4);
            btnRemoveSlide.Name = "btnRemoveSlide";
            btnRemoveSlide.Size = new Size(58, 80);
            btnRemoveSlide.TabIndex = 2;
            btnRemoveSlide.Text = "Remove Slide";
            btnRemoveSlide.TextAlign = ContentAlignment.BottomCenter;
            btnRemoveSlide.UseVisualStyleBackColor = false;
            btnRemoveSlide.Click += btnRemoveSlide_Click;
            // 
            // btn_color
            // 
            btn_color.BackColor = Color.White;
            btn_color.Cursor = Cursors.Hand;
            btn_color.Image = (Image)resources.GetObject("btn_color.Image");
            btn_color.ImageAlign = ContentAlignment.TopCenter;
            btn_color.Location = new Point(131, 5);
            btn_color.Name = "btn_color";
            btn_color.Size = new Size(58, 80);
            btn_color.TabIndex = 3;
            btn_color.Text = "Color";
            btn_color.TextAlign = ContentAlignment.BottomCenter;
            btn_color.UseVisualStyleBackColor = false;
            btn_color.Click += btn_color_Click;
            // 
            // btnAddSlide
            // 
            btnAddSlide.BackColor = Color.White;
            btnAddSlide.FlatAppearance.MouseDownBackColor = Color.White;
            btnAddSlide.FlatAppearance.MouseOverBackColor = Color.DarkGoldenrod;
            btnAddSlide.Image = (Image)resources.GetObject("btnAddSlide.Image");
            btnAddSlide.ImageAlign = ContentAlignment.TopCenter;
            btnAddSlide.Location = new Point(3, 4);
            btnAddSlide.Name = "btnAddSlide";
            btnAddSlide.Size = new Size(58, 80);
            btnAddSlide.TabIndex = 1;
            btnAddSlide.Text = "New Slide";
            btnAddSlide.TextAlign = ContentAlignment.BottomCenter;
            btnAddSlide.UseVisualStyleBackColor = false;
            btnAddSlide.Click += btnAddSlide_Click;
            // 
            // btn_SaveAll
            // 
            btn_SaveAll.BackColor = Color.White;
            btn_SaveAll.Image = (Image)resources.GetObject("btn_SaveAll.Image");
            btn_SaveAll.ImageAlign = ContentAlignment.TopCenter;
            btn_SaveAll.Location = new Point(1174, 4);
            btn_SaveAll.Name = "btn_SaveAll";
            btn_SaveAll.Size = new Size(58, 80);
            btn_SaveAll.TabIndex = 16;
            btn_SaveAll.Text = "Save All";
            btn_SaveAll.TextAlign = ContentAlignment.BottomCenter;
            btn_SaveAll.UseVisualStyleBackColor = false;
            btn_SaveAll.Click += btn_SaveAll_Click;
            // 
            // btn_redo
            // 
            btn_redo.BackColor = Color.White;
            btn_redo.Cursor = Cursors.Hand;
            btn_redo.Image = (Image)resources.GetObject("btn_redo.Image");
            btn_redo.ImageAlign = ContentAlignment.TopCenter;
            btn_redo.Location = new Point(982, 3);
            btn_redo.Name = "btn_redo";
            btn_redo.Size = new Size(58, 80);
            btn_redo.TabIndex = 13;
            btn_redo.Text = "Redo";
            btn_redo.TextAlign = ContentAlignment.BottomCenter;
            btn_redo.UseVisualStyleBackColor = false;
            // 
            // btn_SaveClose
            // 
            btn_SaveClose.BackColor = Color.White;
            btn_SaveClose.Image = (Image)resources.GetObject("btn_SaveClose.Image");
            btn_SaveClose.ImageAlign = ContentAlignment.TopCenter;
            btn_SaveClose.Location = new Point(1238, 6);
            btn_SaveClose.Name = "btn_SaveClose";
            btn_SaveClose.Size = new Size(58, 80);
            btn_SaveClose.TabIndex = 17;
            btn_SaveClose.Text = "Save";
            btn_SaveClose.TextAlign = ContentAlignment.BottomCenter;
            btn_SaveClose.UseVisualStyleBackColor = false;
            btn_SaveClose.Click += btn_SaveClose_Click;
            // 
            // btn_undo
            // 
            btn_undo.BackColor = Color.White;
            btn_undo.Cursor = Cursors.Hand;
            btn_undo.Image = (Image)resources.GetObject("btn_undo.Image");
            btn_undo.ImageAlign = ContentAlignment.TopCenter;
            btn_undo.Location = new Point(918, 4);
            btn_undo.Name = "btn_undo";
            btn_undo.Size = new Size(58, 80);
            btn_undo.TabIndex = 12;
            btn_undo.Text = "Undo";
            btn_undo.TextAlign = ContentAlignment.BottomCenter;
            btn_undo.UseVisualStyleBackColor = false;
            // 
            // btn_image
            // 
            btn_image.BackColor = Color.White;
            btn_image.Cursor = Cursors.Hand;
            btn_image.Image = (Image)resources.GetObject("btn_image.Image");
            btn_image.ImageAlign = ContentAlignment.TopCenter;
            btn_image.Location = new Point(553, 3);
            btn_image.Name = "btn_image";
            btn_image.Size = new Size(58, 80);
            btn_image.TabIndex = 7;
            btn_image.Text = "Picture";
            btn_image.TextAlign = ContentAlignment.BottomCenter;
            btn_image.UseVisualStyleBackColor = false;
            btn_image.Click += btn_image_Click;
            // 
            // pic_color
            // 
            pic_color.BackColor = Color.White;
            pic_color.Location = new Point(323, 21);
            pic_color.Name = "pic_color";
            pic_color.Size = new Size(53, 45);
            pic_color.TabIndex = 6;
            pic_color.UseVisualStyleBackColor = false;
            // 
            // btn_shape
            // 
            btn_shape.BackColor = Color.White;
            btn_shape.Cursor = Cursors.Hand;
            btn_shape.Image = (Image)resources.GetObject("btn_shape.Image");
            btn_shape.ImageAlign = ContentAlignment.TopCenter;
            btn_shape.Location = new Point(259, 6);
            btn_shape.Name = "btn_shape";
            btn_shape.Size = new Size(58, 80);
            btn_shape.TabIndex = 5;
            btn_shape.Text = "Shapes";
            btn_shape.TextAlign = ContentAlignment.BottomCenter;
            btn_shape.UseVisualStyleBackColor = false;
            btn_shape.Click += btn_shape_Click;
            // 
            // picbox_color_picker
            // 
            picbox_color_picker.Cursor = Cursors.Hand;
            picbox_color_picker.Image = Properties.Resources.color_palette;
            picbox_color_picker.Location = new Point(382, 3);
            picbox_color_picker.Name = "picbox_color_picker";
            picbox_color_picker.Size = new Size(165, 80);
            picbox_color_picker.SizeMode = PictureBoxSizeMode.StretchImage;
            picbox_color_picker.TabIndex = 5;
            picbox_color_picker.TabStop = false;
            picbox_color_picker.MouseClick += picbox_color_picker_MouseClick;
            // 
            // btn_fill
            // 
            btn_fill.BackColor = Color.White;
            btn_fill.Cursor = Cursors.Hand;
            btn_fill.Image = (Image)resources.GetObject("btn_fill.Image");
            btn_fill.ImageAlign = ContentAlignment.TopCenter;
            btn_fill.Location = new Point(195, 5);
            btn_fill.Name = "btn_fill";
            btn_fill.Size = new Size(58, 80);
            btn_fill.TabIndex = 4;
            btn_fill.Text = "Fill";
            btn_fill.TextAlign = ContentAlignment.BottomCenter;
            btn_fill.UseVisualStyleBackColor = false;
            btn_fill.Click += btn_fill_Click;
            // 
            // btn_pencil
            // 
            btn_pencil.BackColor = Color.White;
            btn_pencil.Cursor = Cursors.Hand;
            btn_pencil.Image = (Image)resources.GetObject("btn_pencil.Image");
            btn_pencil.ImageAlign = ContentAlignment.TopCenter;
            btn_pencil.Location = new Point(678, 5);
            btn_pencil.Name = "btn_pencil";
            btn_pencil.Size = new Size(58, 80);
            btn_pencil.TabIndex = 9;
            btn_pencil.Text = "Pencil";
            btn_pencil.TextAlign = ContentAlignment.BottomCenter;
            btn_pencil.UseVisualStyleBackColor = false;
            btn_pencil.Click += btn_pencil_Click;
            // 
            // btn_eraser
            // 
            btn_eraser.BackColor = Color.White;
            btn_eraser.Cursor = Cursors.Hand;
            btn_eraser.Image = (Image)resources.GetObject("btn_eraser.Image");
            btn_eraser.ImageAlign = ContentAlignment.TopCenter;
            btn_eraser.Location = new Point(617, 5);
            btn_eraser.Name = "btn_eraser";
            btn_eraser.Size = new Size(58, 80);
            btn_eraser.TabIndex = 8;
            btn_eraser.Text = "Eraser";
            btn_eraser.TextAlign = ContentAlignment.BottomCenter;
            btn_eraser.UseVisualStyleBackColor = false;
            btn_eraser.Click += btn_eraser_Click;
            // 
            // pic
            // 
            pic.BackColor = Color.White;
            pic.BorderStyle = BorderStyle.FixedSingle;
            pic.Location = new Point(208, 91);
            pic.Name = "pic";
            pic.Size = new Size(1152, 561);
            pic.TabIndex = 2;
            pic.TabStop = false;
            pic.Paint += pic_Paint;
            pic.MouseClick += pic_MouseClick;
            pic.MouseDown += pic_MouseDown;
            pic.MouseMove += pic_MouseMove;
            pic.MouseUp += pic_MouseUp;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(717, 260);
            label3.Name = "label3";
            label3.Size = new Size(116, 32);
            label3.TabIndex = 24;
            label3.Text = "Add Slide";
            // 
            // DrawBoard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "DrawBoard";
            Size = new Size(1366, 741);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picbox_color_picker).EndInit();
            ((System.ComponentModel.ISupportInitialize)pic).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private Button btn_fill;
        private Button btn_pencil;
        private Button btn_eraser;
        private PictureBox picbox_color_picker;
        private Button btn_shape;
        private Button pic_color;
        private Button btn_image;
        private Button btn_redo;
        private Button btn_undo;
        private Button btn_select;
        private Button btn_SaveClose;
        private Button btn_SaveAll;
        private Button btn_color;
        private PictureBox pic;
        private Button btn_clear;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private Panel panel4;
        private Button btn_delete;
        private Button btn_uplaod;
        private Button btn_close;
        private ListBox lstSlides;
        private Button btnPreviousSlide;
        private Button btnNextSlide;
        private Button btnRemoveSlide;
        private Button btnAddSlide;
        private TextBox txtRename;
        private Label label1;
        private Panel panel5;
        private Label label2;
        private Label label3;
    }
}
