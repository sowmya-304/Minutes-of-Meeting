using FontAwesome.Sharp;
using System.Windows.Shapes;
using static MOMC_PROJECT.MOM_Prop;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Microsoft.VisualBasic.ApplicationServices;

namespace MOMC_PROJECT
{
    public partial class DrawBoard : UserControl
    {
        Bitmap bm;
        Graphics g;
        private Point px, py;
        Pen pen = new Pen(Color.Black, 1);
        private bool paint = false;
        Pen erase = new Pen(Color.White, 10);
        private int index;
        int x, y, sx, sy, cx, cy;
        ColorDialog cd = new ColorDialog();
        Color new_color;
        //Image
        private System.Drawing.Image selectedImage;
        private bool isImageDrawing;
        private Point ImagestartPoint;
        private System.Drawing.Rectangle currentImageRect;
        private List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>> images = new List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>();
        //shapes
        private System.Drawing.Image selectedIconImage; // Initialize with a default value
        private bool isShapeDrawing;
        private Point ShapeStartPoint;
        private Point shapeEndPoint;
        private System.Drawing.Rectangle currentShapeRect;
        private List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>> shapes = new List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>();
        //select and delete 
        private System.Drawing.Rectangle selectionRectangle; // Field to store the selection rectangle
        private bool isSelecting = false; // Field to indicate if selection mode is active
        private IconChar selectedIcon = IconChar.None; // Initialize with a default value
        private Point startPoint;
        private Point endPoint;
        private bool isDragging = false;
        //slides
        private List<Slide> slides;
        private int currentSlideIndex;

        private Stack<Bitmap> undoStack = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();
        private Panel drawingPanel = new Panel();

        //Saved picturebox path
        public static List<System.Drawing.Image> picimages = new List<System.Drawing.Image>(); // Initialize the list
        //
        private bool isPencilSelected = false;
        private bool isEraserSelected = false;
        private bool isImageSelected = false;
        private bool isIconselected = false;
        private Bitmap currentBitmap;
        private PointF rect;

        private Stack<Bitmap> undoStack1 = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack2 = new Stack<Bitmap>();


        public class Slide
        {
            public string Name { get; set; }
            public List<System.Drawing.Image> Images { get; set; } = new List<System.Drawing.Image>();
            public List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>> Shapes { get; set; } = new List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>();

            public Slide(string name)
            {
                Name = name;
            }
        }

      /*  private List<string> persistentAttachments = new List<string>();
        public List<string> Attachments { get; set; }
*/

        private enum DrawingMode
        {
            None,
            Circle,
            Rectangle,
            Line,
            Diamond,
            Arrow,

        }

        private DrawingMode currentDrawingMode = DrawingMode.None;




        public DrawBoard()
        {
            InitializeComponent();
            ellipse.BackgroundImageLayout = ImageLayout.Zoom;

            ScreenLoad();
            btn_undo.Click += btn_undo_Click;
            btn_redo.Click += btn_redo_Click;
            SaveState();
        }
        private void SaveState()
        {
            undoStack.Push(new Bitmap(bm));
            // redoStack.Clear();
            //  btn_undo.Enabled = undoStack.Count > 1;
            // btn_redo.Enabled = redoStack.Count > 1;

            //Bitmap currentState = new Bitmap(bm);
            //undoStack.Push(currentState);
            //redoStack.Clear();

            //Bitmap currentImage = new Bitmap(pic.Image);
            //undoStack.Push(currentImage);
            //redoStack.Clear();
            //Debug.WriteLine("State saved. Undo stack size: " + undoStack.Count);
        }

        private void SaveStateToUndoStack()
        {
            SaveState();
        }
        private void Undo()
        {
            if (undoStack.Count > 1)
            {
                redoStack.Push(new Bitmap(bm)); // Save current state to redo stack before undo
                Bitmap previousState = undoStack.Pop();
                bm = new Bitmap(undoStack.Peek());
                // bm = new Bitmap(previousState); // Revert to the previous state
                g = Graphics.FromImage(bm); // Update graphics object
                pic.Image = bm;
                pic.Invalidate(); // Refresh to show the updated image
                btn_redo.Enabled = true;
            }
            // btn_undo.Enabled = undoStack.Count > 1;
            // btn_redo.Enabled = redoStack.Count > 1;
        }
        private void Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new Bitmap(bm)); // Save the current state to the undo stack before redoing
                bm = new Bitmap(redoStack.Pop()); // Revert to the next state
                g = Graphics.FromImage(bm); // Update graphics object
                pic.Image = bm;
                pic.Invalidate(); // Refresh to show the updated image
            }
            // btn_undo.Enabled = undoStack.Count > 1; // Disable if only the initial state is left
            //btn_redo.Enabled = redoStack.Count > 0;

        }


        private void ScreenLoad()
        {
            pic.Visible = false;
            panel4.Visible = false;
            txtRename.Visible = false;
            trackBar1.Visible = false;
            trackBar2.Visible = false;
            this.Width = 1366;
            this.Height = 720;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            // g.Clear(Color.White);
            pic.Image = bm;
            slides = new List<Slide>();
            currentSlideIndex = -1;
            DisplayCurrentSlide();
            label1.Text = "";
            AttachMouseDownEventHandler(this);
            this.lstSlides.DoubleClick += new System.EventHandler(this.lstSlides_DoubleClick);
            currentBitmap = new Bitmap(pic.Width, pic.Height);
            pic.Image = currentBitmap;
            //this.btn_rename.Click += new System.EventHandler(this.btn_rename_Click);
        }

        //

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;
            cx = e.X; cy = e.Y;
            if (selectedImage != null)
            {
                isImageDrawing = true;
                ImagestartPoint = e.Location;
            }
            //if (selectedIconImage != null)
            //{
            //    isShapeDrawing = true;
            //    ShapeStartPoint = e.Location;
            //}
            if (isSelecting)
            {
                isDragging = true;
                startPoint = e.Location;
                selectionRectangle = System.Drawing.Rectangle.Empty;
            }
            //if (currentDrawingMode != DrawingMode.None)
            //{
            //    ShapeStartPoint = e.Location;
            //    shapeEndPoint = e.Location;
            //}
        }
        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                //px = e.Location;
                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(pen, px, py);
                    py = px;
                }
                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(erase, px, py);
                    py = px;
                }
                // py = px;
                // pic.Refresh();

            }
            pic.Refresh();
            x = e.X; y = e.Y;
            sx = cx - cx;
            sy = e.Y - cy;


            if (isImageDrawing && selectedImage != null)
            {
                Point currentPoint = e.Location;
                currentImageRect = new System.Drawing.Rectangle(
                    Math.Min(ImagestartPoint.X, currentPoint.X),
                    Math.Min(ImagestartPoint.Y, currentPoint.Y),
                    Math.Abs(ImagestartPoint.X - currentPoint.X),
                    Math.Abs(ImagestartPoint.Y - currentPoint.Y)
                );
                pic.Invalidate();
            }
            if (isShapeDrawing && selectedIconImage != null)
            {
                Point currentPoint = e.Location;
                currentShapeRect = new System.Drawing.Rectangle(
                    Math.Min(ShapeStartPoint.X, currentPoint.X),
                    Math.Min(ShapeStartPoint.Y, currentPoint.Y),
                    Math.Abs(ShapeStartPoint.X - currentPoint.X),
                    Math.Abs(ShapeStartPoint.Y - currentPoint.Y)
                );
                pic.Invalidate();
            }
            if (isDragging && isSelecting)
            {
                endPoint = e.Location;
                selectionRectangle = GetShapeBounds(startPoint, endPoint);
                pic.Invalidate(); // Triggers the Paint event
            }
            if (e.Button == MouseButtons.Left && currentDrawingMode != DrawingMode.None) //new for shapes
            {
                shapeEndPoint = e.Location;
                pic.Invalidate();
            }
            if (pic.ClientRectangle.Contains(e.Location))
            {
                if (isPencilSelected)
                {
                    System.Drawing.Image pencilImage = System.Drawing.Image.FromFile(@"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\pencil.png");

                    if (pencilImage != null)
                    {
                        Cursor pencilCursor = CreateCursor(pencilImage);
                        Cursor = pencilCursor;
                    }
                }
                if (isEraserSelected)
                {
                    System.Drawing.Image eraserImage = System.Drawing.Image.FromFile(@"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\eraser.png");

                    if (eraserImage != null)
                    {
                        Cursor eraserCursor = CreateCursor(eraserImage);
                        Cursor = eraserCursor;
                    }
                }
                if (isImageSelected && selectedImage != null)
                {
                    Cursor = CreateCursor(selectedImage);
                }
                if (isIconselected && selectedIconImage != null)
                {
                    Cursor = CreateCursor(selectedIconImage);
                }

            }
            else
            {
                Cursor = Cursors.Default;
            }

        }
        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            sx = x - cx; sy = y - cy;
            if (index == 3)
            {

                g.DrawEllipse(pen, cx, cy, sx, sy);
            }
            if (index == 4)
            {
                g.DrawRectangle(pen, cx, cy, sx, sy);
            }
            if (index == 5)
            {
                g.DrawLine(pen, cx, cy, x, y);

            }
            if (index == 6) // Diamond button
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Point[] diamondPoints = new Point[]
                {
            new Point(cx, cy + sy / 2),
            new Point(cx + sx / 2, cy),
            new Point(cx + sx, cy + sy / 2),
            new Point(cx + sx / 2, cy + sy)
                };

                g.DrawPolygon(pen, diamondPoints);
            }
            //if (index == 7) // Arrow button
            //{
            //    // Draw arrow
            //    using (GraphicsPath arrowPath = new GraphicsPath())
            //    {
            //        // Define arrow shape
            //        arrowPath.AddLine(ShapeStartPoint, shapeEndPoint);
            //        pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            //        // Draw arrow
            //        g.DrawPath(pen, arrowPath);
            //    }

            //}
            if (isImageDrawing && selectedImage != null)
            {
                isImageDrawing = false;
                images.Add(new Tuple<System.Drawing.Image, System.Drawing.Rectangle>(selectedImage, currentImageRect));
                selectedImage = null;
                Cursor = Cursors.Default;
                pic.Invalidate();
            }
            if (isShapeDrawing && selectedIconImage != null)
            {
                isShapeDrawing = false;
                shapes.Add(new Tuple<System.Drawing.Image, System.Drawing.Rectangle>(selectedIconImage, currentShapeRect));
                selectedIconImage = null;
                Cursor = Cursors.Default;
                pic.Invalidate();
            }
            if (isDragging && isSelecting)
            {
                endPoint = e.Location;
                selectionRectangle = GetShapeBounds(startPoint, endPoint);
                isDragging = false;
                Cursor = Cursors.Default;
                pic.Invalidate();
            }
            currentDrawingMode = DrawingMode.None;
            pic.Invalidate();
            SaveState();


        }
        private System.Drawing.Rectangle GetShapeBounds(Point p1, Point p2)
        {
            return new System.Drawing.Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y)
            );
        }
        private void btn_pencil_Click(object sender, EventArgs e)
        {
            trackBar2.Visible = true;
            isPencilSelected = true;
            isEraserSelected = false;
            Cursor = Cursors.Default;
            index = 1;
            // Load the pencil image from file
            System.Drawing.Image pencilImage = System.Drawing.Image.FromFile(@"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\pencil.png");
            // Create a cursor from the pencil image
            Cursor pencilCursor = CreateCursor(pencilImage);
            // Set the cursor to the pencil cursor
            Cursor = pencilCursor;
        }
        private void btn_eraser_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = true;
            isPencilSelected = false;
            isEraserSelected = true;
            Cursor = Cursors.Default;
            // Load the eraser image from file
            System.Drawing.Image eraserImage = System.Drawing.Image.FromFile(@"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\eraser.png");
            // Create a cursor from the eraser image
            Cursor eraserCursor = CreateCursor(eraserImage);
            // Set the cursor to the eraser cursor
            Cursor = eraserCursor;
            index = 2;
            erase.Width = trackBar1.Value;
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            if (isSelecting && selectionRectangle != System.Drawing.Rectangle.Empty)
            {
                using (Pen selectionPen = new Pen(Color.Blue))
                {
                    selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(selectionPen, selectionRectangle);
                }
            }

            // Draw all images stored in the list
            foreach (var imageRect in images)
            {
                e.Graphics.DrawImage(imageRect.Item1, imageRect.Item2);
            }

            // If currently drawing, draw the current image rectangle
            if (isImageDrawing && selectedImage != null)
            {
                e.Graphics.DrawRectangle(Pens.Blue, currentImageRect);
                e.Graphics.DrawImage(selectedImage, currentImageRect);
            }

            // Draw all shapes stored in the list
            foreach (var shapeRect in shapes)
            {
                e.Graphics.DrawImage(shapeRect.Item1, shapeRect.Item2);
            }

            // If currently drawing a shape, draw the current shape rectangle
            if (isShapeDrawing && selectedIconImage != null)
            {
                e.Graphics.DrawRectangle(Pens.Blue, currentShapeRect);
                e.Graphics.DrawImage(selectedIconImage, currentShapeRect);
            }

            // Draw shapes based on currentDrawingMode
            if (currentDrawingMode != DrawingMode.None)
            {
                if (currentDrawingMode == DrawingMode.Circle)
                {
                    int width = Math.Abs(shapeEndPoint.X - ShapeStartPoint.X);
                    int height = Math.Abs(shapeEndPoint.Y - ShapeStartPoint.Y);
                    int diameter = Math.Min(width, height);
                    e.Graphics.DrawEllipse(Pens.Black, ShapeStartPoint.X, ShapeStartPoint.Y, diameter, diameter);
                }
                else if (currentDrawingMode == DrawingMode.Rectangle)
                {
                    int width = Math.Abs(shapeEndPoint.X - ShapeStartPoint.X);
                    int height = Math.Abs(shapeEndPoint.Y - ShapeStartPoint.Y);
                    e.Graphics.DrawRectangle(Pens.Black, ShapeStartPoint.X, ShapeStartPoint.Y, width, height);
                }
                else if (currentDrawingMode == DrawingMode.Line)
                {
                    e.Graphics.DrawLine(Pens.Black, ShapeStartPoint, shapeEndPoint);
                }

            }
            Cursor cursor = Cursors.Default;
            SaveStateToUndoStack();
            //if (isSelecting && selectionRectangle != System.Drawing.Rectangle.Empty)
            //{
            //    using (Pen selectionPen = new Pen(Color.Blue))
            //    {
            //        selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //        e.Graphics.DrawRectangle(selectionPen, selectionRectangle);
            //    }
            //}
            //// Draw all images stored in the list
            //foreach (var imageRect in images)
            //{
            //    e.Graphics.DrawImage(imageRect.Item1, imageRect.Item2);
            //}

            //// If currently drawing, draw the current image rectangle
            //if (isImageDrawing && selectedImage != null)
            //{
            //    e.Graphics.DrawRectangle(Pens.Blue, currentImageRect);
            //    e.Graphics.DrawImage(selectedImage, currentImageRect);
            //}

            //// Draw all shapes stored in the list
            //foreach (var shapeRect in shapes)
            //{
            //    e.Graphics.DrawImage(shapeRect.Item1, shapeRect.Item2);
            //}

            //// If currently drawing a shape, draw the current shape rectangle
            //if (isShapeDrawing && selectedIconImage != null)
            //{
            //    e.Graphics.DrawRectangle(Pens.Blue, currentShapeRect);
            //    e.Graphics.DrawImage(selectedIconImage, currentShapeRect);
            //}
            //if (currentDrawingMode != DrawingMode.None)
            //{
            //    if (currentDrawingMode == DrawingMode.Circle)
            //    {
            //        int width = Math.Abs(shapeEndPoint.X - ShapeStartPoint.X);
            //        int height = Math.Abs(shapeEndPoint.Y - ShapeStartPoint.Y);
            //        int diameter = Math.Min(width, height);
            //        e.Graphics.DrawEllipse(Pens.Black, ShapeStartPoint.X, ShapeStartPoint.Y, diameter, diameter);
            //    }
            //    else if (currentDrawingMode == DrawingMode.Rectangle)
            //    {
            //        int width = Math.Abs(shapeEndPoint.X - ShapeStartPoint.X);
            //        int height = Math.Abs(shapeEndPoint.Y - ShapeStartPoint.Y);
            //        e.Graphics.DrawRectangle(Pens.Black, ShapeStartPoint.X, ShapeStartPoint.Y, width, height);
            //    }
            //    else if (currentDrawingMode == DrawingMode.Line)
            //    {
            //        e.Graphics.DrawLine(Pens.Black, ShapeStartPoint, shapeEndPoint);
            //    }
            //}
            //Cursor cursor = Cursors.Default;
            //SaveStateToUndoStack();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            g.Clear(Color.White);
            pic.Image = bm;
            index = 0;
        }

        private void btn_color_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            pen.Color = cd.Color;
        }

        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void picbox_color_picker_MouseClick(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            Point point = set_point(picbox_color_picker, e.Location);
            pic_color.BackColor = ((Bitmap)picbox_color_picker.Image).GetPixel(point.X, point.Y);
            new_color = pic_color.BackColor;
            pen.Color = pic_color.BackColor;
        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr) return;
            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                Point point = set_point(pic, e.Location);
                Fill(bm, point.X, point.Y, new_color);
            }
        }

        private void btn_fill_Click(object sender, EventArgs e)
        {
            index = 7;
            if (selectionRectangle != System.Drawing.Rectangle.Empty)
            {
                // Create a new bitmap to draw on
                Bitmap bmp = new Bitmap(pic.Width, pic.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Fill the selected area with a color (e.g., red)
                    SolidBrush brush = new SolidBrush(Color.Red);
                    g.FillRectangle(brush, selectionRectangle);
                }

                // Assign the modified bitmap to the PictureBox
                pic.Image = bmp;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            erase.Width = trackBar1.Value;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = trackBar2.Value;
        }
        private void Icon_Click(object sender, EventArgs e)
        {
            // Determine which icon was clicked and set the selectedIcon accordingly
            selectedIconImage = GetClickedIconImage(sender);

            // If an icon is selected, change the cursor to represent the selected shape/icon
            if (selectedIconImage != null)
            {
                Cursor = CreateCursor(selectedIconImage);
            }
        }
        private System.Drawing.Image GetClickedIconImage(object sender)
        {
            // Assuming sender is the control representing the clicked icon
            System.Windows.Forms.Control clickedControl = sender as System.Windows.Forms.Control;

            // Example: If your icon controls have their Image stored in the Tag property
            if (clickedControl != null && clickedControl.Tag != null)
            {
                if (clickedControl.Tag is System.Drawing.Image)
                {
                    return (System.Drawing.Image)clickedControl.Tag;
                }
            }

            // If no icon is identified, return null
            return null;
        }

        private void btn_image_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Image selectedImage = System.Drawing.Image.FromFile(openFileDialog.FileName);
                    if (selectedImage != null && currentSlideIndex >= 0 && currentSlideIndex < slides.Count)
                    {
                        // Add the selected image to the current slide
                        slides[currentSlideIndex].Images.Add(selectedImage);

                        // Update the display to show the new image
                        DisplayCurrentSlide();
                    }
                }
            }
            //index = 0;
            //Cursor cursor = Cursors.Default;
            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
            //    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        selectedImage = System.Drawing.Image.FromFile(openFileDialog.FileName);
            //        isImageSelected = true;
            //        if (selectedImage != null)
            //        {
            //            Cursor = CreateCursor(selectedImage);
            //        }
            //    }
            //}
        }
        private Cursor CreateCursor(System.Drawing.Image iconImage)
        {
            if (iconImage == null)
            {
                throw new ArgumentNullException(nameof(iconImage), "The image parameter cannot be null.");
            }

            System.Drawing.Image resizedImage = ResizeImage(iconImage, 50, 50);

            // Convert the resized image to a bitmap
            Bitmap bmp = new Bitmap(resizedImage);

            // Create a cursor from the bitmap
            IntPtr ptr = bmp.GetHicon();
            return new Cursor(ptr);
        }
        private System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "The image parameter cannot be null.");
            }

            // Create a new bitmap with the desired dimensions
            Bitmap resizedImage = new Bitmap(width, height);

            // Create a graphics object from the new bitmap
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                // Set the interpolation mode to high quality bicubic
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw the original image onto the new bitmap with the desired dimensions
                g.DrawImage(image, 0, 0, width, height);
            }

            // Return the resized image
            return resizedImage;
        }
        private void IconButton_Click(object sender, EventArgs e)
        {
            //// Get the clicked button
            //IconButton clickedButton = sender as IconButton;

            //// Store the selected icon
            //selectedImage = clickedButton.Image;

            //// Hide the panel
            //panel4.Visible = false;
            // Get the clicked button
            // Get the clicked icon button
            IconButton clickedButton = sender as IconButton;

            // Store the selected icon image
            selectedIconImage = clickedButton.Image;
            isIconselected = true;
            Cursor cursor = CreateCursor(selectedIconImage);
            panel4.Visible = false;
        }


        public enum CustomIconType
        {
            Square,
            Circle,
            Star,
            Heart,
            flower,
            Bell,
            Check,
            Times,
            Plus,
            Minus,
            ArrowUp,
            ArrowDown,
            ArrowLeft,
            ArrowRight,
            Pause,
            Play,
            Stop,
            FastForward,
            StepForward,
            Music,
            Video,
            Image,
            Camera,
            Film,
            Bolt,
            Lightbulb,
            Cloud,
            Sun,
            Moon,
            Wind,
            Fire,
            Water,
            Snowflake,
            Tree,
            Leaf,
            Globe,
            Map,
            Compass,
            Flag,
            Cog,
            Tools,
            Hammer,
            Wrench,
            Screwdriver,
            Lock,
            Unlock,
            Key,
            Home,
            Building,
            Car,
            Bicycle,
            Bus,
            Train,
            Plane,
            Ship,
            Subway,
            Truck,
            Horse,
            Dog,
            Cat,
            Fish,
            Dove,
            Dragon,
            Frog,
            Paw
        }

        public class CustomIcon
        {
            public CustomIconType IconType { get; set; }
            public System.Drawing.Icon IconImage { get; set; }

            public CustomIcon(CustomIconType iconType, System.Drawing.Icon iconImage)
            {
                IconType = iconType;
                IconImage = iconImage;
            }
        }
        private void btn_shape_Click(object sender, EventArgs e)  //
        {

            index = 0;
            Cursor cursor = Cursors.Default;
            panel4.Visible = true;
            ellipse.Click += ellipse_Click;
            rectangle.Click += rectangle_Click;
            Line.Click += Line_Click;
            //    // Clear the panel before adding buttons and images
            //panel4.Controls.Clear();
            //panel4.Controls.Add(ellipse); 
            //panel4.Controls.Add(rectangle);
            //panel4.Controls.Add(Line);


            //         IconChar[] icons = new IconChar[]
            //{
            //       IconChar.Line,IconChar.Square, IconChar.Minus, IconChar.Star, IconChar.Ellipsis,IconChar.TriangleCircleSquare,
            //        IconChar.StarHalfAlt, IconChar.StarHalf,  IconChar.ArrowUp,
            //        IconChar.ArrowDown, IconChar.ArrowLeft, IconChar.ArrowRight,
            //        IconChar.CaretUp, IconChar.CaretDown, IconChar.CaretLeft,
            //        IconChar.CaretRight, IconChar.ChevronUp, IconChar.ChevronDown,

            //        IconChar.ChevronLeft, IconChar.ChevronRight, IconChar.LongArrowUp,
            //        IconChar.LongArrowDown, IconChar.LongArrowLeft, IconChar.LongArrowRight
            //};

            //         // Set the dimensions of the grid
            //         int rowCount = 5;
            //         int colCount = 5;
            //         int cellWidth = 50; // Adjust as 
            //         int cellHeight = 50; // Adjust as needed

            //         int iconIndex = 0;
            //         int totalIcons = icons.Length;

            //         for (int row = 0; row < rowCount; row++)
            //         {
            //             for (int col = 0; col < colCount; col++)
            //             {
            //                 if (iconIndex < totalIcons)
            //                 {
            //                     IconButton btn = new IconButton();
            //                     btn.Width = cellWidth;
            //                     btn.Height = cellHeight;
            //                     btn.Top = row * cellHeight;
            //                     btn.Left = col * cellWidth;
            //                     btn.IconChar = icons[iconIndex];
            //                     btn.IconColor = Color.Black;
            //                     btn.IconSize = 20;
            //                     btn.Text = "";
            //                     btn.FlatStyle = FlatStyle.Flat;
            //                     btn.FlatAppearance.BorderSize = 0;
            //                     btn.Click += IconButton_Click; // Add click event handler
            //                     panel4.Controls.Add(btn);
            //                     iconIndex++;
            //                 }
            //             }
            //         }
        }




        private void btn_select_Click(object sender, EventArgs e)
        {

            Cursor = Cursors.Cross;

            isSelecting = true; // Enable selection mode
            selectedIcon = IconChar.None; // Disable drawing icons when selecting
            index = 0;

            // Optionally clear any existing selection rectangle
            selectionRectangle = System.Drawing.Rectangle.Empty;
            pic.Invalidate(); // Refresh the PictureBox to update th
        }
        private bool IsRectangleIntersecting(System.Drawing.Rectangle rect1, System.Drawing.Rectangle rect2)
        {
            return rect1.IntersectsWith(rect2);
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {

            if (selectionRectangle != System.Drawing.Rectangle.Empty)
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.FillRectangle(Brushes.White, selectionRectangle);
                }
                selectionRectangle = System.Drawing.Rectangle.Empty;
                //if (selectionRectangle != System.Drawing.Rectangle.Empty)
                //{
                //    // Find the images intersecting with the selection area
                //    List<int> itemsToRemove = new List<int>();
                //    for (int i = 0; i < images.Count; i++)
                //    {
                //        // Access the rectangle directly from the tuple
                //        if (selectionRectangle.IntersectsWith(images[i].Item2))
                //        {
                //            itemsToRemove.Add(i);
                //        }
                //    }

                //    // Iterate over the list in reverse order to safely remove items
                //    for (int i = itemsToRemove.Count - 1; i >= 0; i--)
                //    {
                //        images.RemoveAt(itemsToRemove[i]);
                //    }

                //    // Clear the selection rectangle
                //    selectionRectangle = System.Drawing.Rectangle.Empty;

                // Refresh the PictureBox to update the display
                pic.Invalidate();
                // }

                // Disable selection mode
                isSelecting = false;
            }
        }
        //public static List<String> attachments = new List<String>();
        //private List<Panel> buttonPanels = new List<Panel>();===
        public static List<string> attachments = new List<string>();
        public static List<string> drawboardAttachments = new List<string>();
        
        private List<Panel> buttonPanels = new List<Panel>();
        private int buttonHeight;

        private void btn_uplaod_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All Files (*.*)|*.*";
            // openFileDialog.Filter = "JPEG Image|*.jpg|JPEG Image|*.jpeg|PNG Image|*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> selectedFilePaths = new List<string>(openFileDialog.FileNames);
                AttachFilesToPanel1(selectedFilePaths);
            }
        }
        private void AttachFilesToPanel1(List<string> filePaths)
        {
            MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
            if (mc != null)
            {
                Panel panel5DW = mc.Panel5DW;
                if (panel5DW != null)
                {
                    panel5DW.AutoScroll = true;
                    int buttonWidth = 200;
                    int buttonHeight = 50;
                    int fileButtonGap = 20;
                    int removeButtonGap = 0;
                    int maxButtonsPerRow = (panel5DW.Width) / (buttonWidth + fileButtonGap);
                    int fileCount = 0;

                    foreach (string filePath in filePaths)
                    {
                        drawboardAttachments.Add(filePath);
                       // attachments.Add(filePath);
                        int rowIndex = fileCount / maxButtonsPerRow;
                        int colIndex = fileCount % maxButtonsPerRow;
                        int x = colIndex * (buttonWidth + fileButtonGap);
                        int y = rowIndex * (buttonHeight + removeButtonGap);

                        Panel buttonPanel = new Panel
                        {
                            Size = new Size(buttonWidth, buttonHeight),
                            Location = new Point(x, y),
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        PictureBox iconPictureBox = new PictureBox
                        {
                            Size = new Size(32, 32),
                            Location = new Point(10, (buttonHeight - 32) / 2),
                            Image = System.Drawing.Icon.ExtractAssociatedIcon(filePath).ToBitmap(),
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };
                        buttonPanel.Controls.Add(iconPictureBox);

                        Label fileInfoLabel = new Label
                        {
                            AutoSize = true,
                            Location = new Point(iconPictureBox.Right + 10, (buttonHeight - 32) / 2),
                            Text = $"{System.IO.Path.GetFileName(filePath)} ({new FileInfo(filePath).Length / 1024} KB)",
                            MaximumSize = new Size(buttonWidth - (iconPictureBox.Right + 40), buttonHeight)
                        };
                        buttonPanel.Controls.Add(fileInfoLabel);

                        System.Windows.Forms.Button removeButton = new System.Windows.Forms.Button
                        {
                            Text = "X",
                            Size = new Size(20, 20),
                            Location = new Point(buttonWidth - 30, (buttonHeight - 20) / 2)
                        };
                        removeButton.Click += (btnSender, btnE) =>
                        {
                            int indexToRemove = buttonPanels.IndexOf(buttonPanel);
                            buttonPanels.RemoveAt(indexToRemove);
                            panel5DW.Controls.Remove(buttonPanel);
                            drawboardAttachments.RemoveAt(indexToRemove);
                           // attachments.RemoveAt(indexToRemove);
                            UpdateButtonPanelLocations(panel5DW, maxButtonsPerRow, buttonWidth, fileButtonGap, removeButtonGap);
                            fileCount--;
                        };
                        buttonPanel.Controls.Add(removeButton);

                        buttonPanel.Click += (panelSender, panelE) =>
                        {
                            try
                            {
                                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        panel5DW.Controls.Add(buttonPanel);
                        buttonPanels.Add(buttonPanel);
                        fileCount++;
                    }
                }
                panel1.Controls.Clear();
                panel1.Dock = DockStyle.Fill;
                panel1.BackColor = SystemColors.Control;
                panel1.Controls.Add(mc);
            }
        }
      
        public void AttachFilesToPanel2(string filePaths)
        {
            MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
            // attachments.Add(filePaths);
            // Get all files in the directory specified by folderPath
            // Add each file path to the static AttachmentStore
            if (AttachmentStore.Attachments == null)
            {
                //AttachmentStore.Attachments = new List<string>();
                AttachmentStore.Attachments = new List<string>();

            }

            if (!AttachmentStore.Attachments.Contains(filePaths))
            {
                AttachmentStore.Attachments.Add(filePaths);
                // attachments.Add(filePaths); // Add to the attachments list as well
                drawboardAttachments.Add(filePaths);
            }
            //foreach (string filePath in attachments)
            //{
            //    if (!AttachmentStore.Attachments.Contains(filePath))
            //    {
            //        // If it's not in the HashSet, add it
            //        AttachmentStore.Attachments.Add(filePath);
            //    }
            //    AttachmentStore.Attachments = new List<string>(AttachmentStore.Attachments);

            //    // AttachmentStore.Attachments.Add(filePath);
            //}
            Panel panel5DW = mc.Panel5DW;
            panel5DW.Controls.Clear();
            buttonPanels.Clear();
            if (AttachmentStore.Attachments.Count > 0) // Run the loop only if there are attachments
            {
                int index = 0;
                foreach (var filePath in AttachmentStore.Attachments)
                {
                    var buttonPanel = mc.CreateFileButtonPanel1(filePath, index, buttonWidth: 100, buttonHeight: 50, fileButtonGap: 10, removeButtonGap: 5, maxButtonsPerRow: 3);
                    buttonPanels.Add(buttonPanel);
                    panel5DW.Controls.Add(buttonPanel);
                    index++;
                }
            }
            /*  MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
              if (mc != null)
              {
                  Panel panel5DW = mc.Panel5DW;

                  if (panel5DW != null)
                  {
                      panel5DW.AutoScroll = true;

                      int buttonWidth = 200;
                      int buttonHeight = 50;
                      int fileButtonGap = 20;
                      int removeButtonGap = 0;
                      int maxButtonsPerRow = (panel5DW.Width) / (buttonWidth + fileButtonGap);
                      int fileCount = 0;

                      // Iterate over each file path in the list
                         foreach (string filePath in attachments)
                         {
                             // Calculate position for the button panel
                             int rowIndex = fileCount / maxButtonsPerRow;
                             int colIndex = fileCount % maxButtonsPerRow;
                             int x = colIndex * (buttonWidth + fileButtonGap);
                             int y = rowIndex * (buttonHeight + removeButtonGap);

                             // Create and configure button panel
                             Panel buttonPanel = new Panel
                             {
                                 Size = new Size(buttonWidth, buttonHeight),
                                 Location = new Point(x, y),
                                 BorderStyle = BorderStyle.FixedSingle
                             };

                             // Add PictureBox, label, remove button, and event handlers to button panel
                             PictureBox iconPictureBox = new PictureBox
                             {
                                 Size = new Size(32, 32),
                                 Location = new Point(10, (buttonHeight - 32) / 2),
                                 Image = System.Drawing.Icon.ExtractAssociatedIcon(filePath).ToBitmap(),
                                 SizeMode = PictureBoxSizeMode.StretchImage
                             };
                             buttonPanel.Controls.Add(iconPictureBox);

                             Label fileInfoLabel = new Label
                             {
                                 AutoSize = true,
                                 Location = new Point(iconPictureBox.Right + 10, (buttonHeight - 32) / 2),
                                 Text = $"{System.IO.Path.GetFileName(filePath)} ({new FileInfo(filePath).Length / 1024} KB)",
                                 MaximumSize = new Size(buttonWidth - (iconPictureBox.Right + 40), buttonHeight)
                             };
                             buttonPanel.Controls.Add(fileInfoLabel);

                             System.Windows.Forms.Button removeButton = new System.Windows.Forms.Button
                             {
                                 Text = "X",
                                 Size = new Size(20, 20),
                                 Location = new Point(buttonWidth - 30, (buttonHeight - 20) / 2)
                             };
                             removeButton.Click += (btnSender, btnE) =>
                             {
                                 int indexToRemove = buttonPanels.IndexOf(buttonPanel);
                                 buttonPanels.RemoveAt(indexToRemove);
                                 panel5DW.Controls.Remove(buttonPanel);
                                 AttachmentStore.Attachments.RemoveAt(indexToRemove);
                                 UpdateButtonPanelLocations(panel5DW, maxButtonsPerRow, buttonWidth, fileButtonGap, removeButtonGap);
                                 fileCount--;
                             };
                             buttonPanel.Controls.Add(removeButton);

                             buttonPanel.Click += (panelSender, panelE) =>
                             {
                                 try
                                 {
                                     Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                                 }
                                 catch (Exception ex)
                                 {
                                     MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                 }
                             };
                             panel5DW.Controls.Add(buttonPanel);
                             buttonPanels.Add(buttonPanel);
                             fileCount++;
                         }
                  }
              }
  */
            //MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
            //if (mc != null)
            //{
            //    Panel panel5DW = mc.Panel5DW;

            //    if (panel5DW != null)
            //    {
            //        panel5DW.AutoScroll = true;

            //        int buttonWidth = 200;
            //        int buttonHeight = 50;
            //        int fileButtonGap = 20;
            //        int removeButtonGap = 0;
            //        int maxButtonsPerRow = (panel5DW.Width) / (buttonWidth + fileButtonGap);
            //        int fileCount = 0;
            //       //  attachments.Add(filePaths);
            //        AttachmentStore.Attachments.Add(filePaths);
            //        // Iterate over each file path in the list
            //        foreach (string filePath in AttachmentStore.Attachments)
            //            {
            //            // Calculate position for the button panel
            //            int rowIndex = fileCount / maxButtonsPerRow;
            //            int colIndex = fileCount % maxButtonsPerRow;
            //            int x = colIndex * (buttonWidth + fileButtonGap);
            //            int y = rowIndex * (buttonHeight + removeButtonGap);
            //            // Create and configure button panel
            //            Panel buttonPanel = new Panel
            //            {
            //                Size = new Size(buttonWidth, buttonHeight),
            //                Location = new Point(x, y),
            //                BorderStyle = BorderStyle.FixedSingle
            //            };

            //            // Add PictureBox, label, remove button, and event handlers to button panel
            //            PictureBox iconPictureBox = new PictureBox
            //            {
            //                Size = new Size(32, 32),
            //                Location = new Point(10, (buttonHeight - 32) / 2),
            //                Image = System.Drawing.Icon.ExtractAssociatedIcon(filePath).ToBitmap(),
            //                SizeMode = PictureBoxSizeMode.StretchImage
            //            };
            //            buttonPanel.Controls.Add(iconPictureBox);

            //            Label fileInfoLabel = new Label
            //            {
            //                AutoSize = true,
            //                Location = new Point(iconPictureBox.Right + 10, (buttonHeight - 32) / 2),
            //                Text = $"{System.IO.Path.GetFileName(filePath)} ({new FileInfo(filePath).Length / 1024} KB)",
            //                MaximumSize = new Size(buttonWidth - (iconPictureBox.Right + 40), buttonHeight)
            //            };
            //            buttonPanel.Controls.Add(fileInfoLabel);

            //            System.Windows.Forms.Button removeButton = new System.Windows.Forms.Button
            //            {
            //                Text = "X",
            //                Size = new Size(20, 20),
            //                Location = new Point(buttonWidth - 30, (buttonHeight - 20) / 2)
            //            };
            //            removeButton.Click += (btnSender, btnE) =>
            //            {
            //                int indexToRemove = buttonPanels.IndexOf(buttonPanel);
            //                buttonPanels.RemoveAt(indexToRemove);
            //                panel5DW.Controls.Remove(buttonPanel);
            //                attachments.RemoveAt(indexToRemove);
            //                UpdateButtonPanelLocations(panel5DW, maxButtonsPerRow, buttonWidth, fileButtonGap, removeButtonGap);
            //                fileCount--;
            //            };
            //            buttonPanel.Controls.Add(removeButton);

            //            buttonPanel.Click += (panelSender, panelE) =>
            //            {
            //                try
            //                {
            //                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }
            //            };
            //            panel5DW.Controls.Add(buttonPanel);
            //            buttonPanels.Add(buttonPanel);
            //            fileCount++;
            //        }
            //    }
            //    // Add mc to panel1
            //   /* panel1.Controls.Clear();
            //    panel1.Dock = DockStyle.Fill;
            //    panel1.BackColor = SystemColors.Control;
            //    panel1.Controls.Add(mc);*/
            //}      
        }
        //private void LoadAttachments()
        //{
        //    MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
        //    if (mc != null)
        //    {
        //        Panel panel5DW = mc.Panel5DW;
        //        if (panel5DW != null)
        //        {
        //            panel5DW.Controls.Clear(); // Clear existing controls
        //            buttonPanels.Clear(); // Clear buttonPanels list

        //            // Loop through persistent attachments and attach them to panel5DW
        //            foreach (string filePath in persistentAttachments)
        //            {
        //                AttachFilesToPanel2(filePath); // Pass panel5DW to AttachFilesToPanel2
        //            }
        //        }

        //        Draw d = new Draw();
        //        d.Close();
        //    } 
        //}
        private void UpdateButtonPanelLocations(Panel panel5DW, int maxButtonsPerRow, int buttonWidth, int fileButtonGap, int removeButtonGap)
        {
            try
            {
                for (int i = 0; i < buttonPanels.Count; i++)
                {
                    int row = i / maxButtonsPerRow;
                    int col = i % maxButtonsPerRow;
                    int newX = col * (buttonWidth + fileButtonGap);
                    int newY = row * (buttonHeight + removeButtonGap);
                    buttonPanels[i].Location = new Point(newX, newY);
                }
            }
            catch(Exception ex) { }
        }
        private void btn_SaveClose_Click(object sender, EventArgs e)
        {
            if (slides == null || slides.Count == 0)
            {
                MessageBox.Show("No slides to save.");
                return;
            }
          //  MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set the file filter to allow saving as JPEG, JPG, or PNG
                saveFileDialog.Filter = "JPEG Image|*.jpg|JPEG Image|*.jpeg|PNG Image|*.png";
                Slide currentSlide = slides[currentSlideIndex];
                string displayName = string.IsNullOrEmpty(currentSlide.Name) ? "Unnamed Slide" : currentSlide.Name;
                // Set the initial file name in the save file dialog
                saveFileDialog.FileName = displayName;
                // Display the dialog and wait for the user's response
                DialogResult result = saveFileDialog.ShowDialog();

                // If the user clicks "Save"
                if (result == DialogResult.OK)
                {
                    // Get the selected filename
                    string fileName = saveFileDialog.FileName;

                    // Use the selected meeting name as the new folder name
                    string meetingName = "Drawboard Images"; // Replace with the actual selected meeting name
                    string newFolderPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName), meetingName);
                    if (!System.IO.Directory.Exists(newFolderPath))
                    {
                        System.IO.Directory.CreateDirectory(newFolderPath);
                    }

                    // Create a new bitmap with the same size as the PictureBox
                    Bitmap combinedImage = new Bitmap(pic.Width, pic.Height);

                    // Get the graphics object of the combined image
                    using (Graphics g = Graphics.FromImage(combinedImage))
                    {
                        // Draw the background image from the PictureBox
                        g.DrawImage(pic.Image, Point.Empty);

                        // Draw the images stored in the list
                        foreach (var imageRect in images)
                        {
                            g.DrawImage(imageRect.Item1, imageRect.Item2);
                        }

                        // Draw the shapes/icons on top of the background image
                        foreach (var shape in shapes)
                        {
                            g.DrawImage(shape.Item1, shape.Item2);
                        }
                    }
                    // Save the combined image to the new folder
                    string newFilePath = System.IO.Path.Combine(newFolderPath, System.IO.Path.GetFileName(fileName));
                    combinedImage.Save(newFilePath);
                    AttachFilesToPanel2(newFilePath);
                    MessageBox.Show("Saved and Uploaded successfully");
                    currentSlide.Name = System.IO.Path.GetFileNameWithoutExtension(fileName);

                }
            }
        }
        
        private void btnAddSlide_Click(object sender, EventArgs e)
        {
            pic.Visible = true;
            SaveCurrentSlideContent();
            Slide newSlide = new Slide("New Slide");
            // Store the content of the PictureBox in the new slide
            newSlide.Images.AddRange(images.Select(imageRect => imageRect.Item1).ToList());
            newSlide.Shapes.AddRange(shapes);
            slides.Add(newSlide);
            currentSlideIndex = slides.Count - 1;
            DisplayCurrentSlide();
            UpdateSlideList();
            lstSlides.SelectedIndex = currentSlideIndex;
            int c = 0;
            foreach (Slide slide in slides)
            {
                c++;
            }
            int totalSlides = slides.Count;
            int currentSlideNumber = currentSlideIndex + 1; // Adding 1 to make it human-readable (1-indexed)
            // Update the label text
            label1.Text = $"{currentSlideNumber}/{totalSlides}";
        }

        private void btnRemoveSlide_Click(object sender, EventArgs e)
        {
            if (currentSlideIndex >= 0 && slides.Count > 0)
            {
                
                slides.RemoveAt(currentSlideIndex);
                if (currentSlideIndex >= slides.Count)
                {
                    currentSlideIndex = slides.Count - 1;
                }
                DisplayCurrentSlide();
                UpdateSlideList(); 
            }
            if (slides.Count == 0)
            {
                pic.Visible = false;
            }
            lstSlides.SelectedIndex = currentSlideIndex;

            int c = 0;
            foreach (Slide slide in slides)
            {
                c++;
            }
            int totalSlides = slides.Count;
            int currentSlideNumber = currentSlideIndex + 1; // Adding 1 to make it human-readable (1-indexed)

            // Update the label text
            label1.Text = $"{currentSlideNumber}/{totalSlides}";
        }

        private void btnNextSlide_Click(object sender, EventArgs e)
        {
            if (currentSlideIndex < slides.Count - 1)
            {
                SaveCurrentSlideContent();
                currentSlideIndex++;
                DisplayCurrentSlide();

                // Update the ListBox selection and force redraw to highlight the selected slide
                lstSlides.SelectedIndex = currentSlideIndex;
                lstSlides.Invalidate();
            }
        }
        private void btnPreviousSlide_Click(object sender, EventArgs e)
        {
            if (currentSlideIndex > 0)
            {
                SaveCurrentSlideContent();
                currentSlideIndex--;
                DisplayCurrentSlide();
                lstSlides.SelectedIndex = currentSlideIndex;
                lstSlides.Invalidate();
            }
        }
        private void DisplayCurrentSlide()
        {
            //if (currentSlideIndex >= 0 && currentSlideIndex < slides.Count)
            //{
            //    var currentSlide = slides[currentSlideIndex];

            //    // Create a new bitmap to draw on
            //   // Bitmap slideBitmap = new Bitmap(pic.Width, pic.Height);
            //    g.Clear(Color.White);

            //    using (Graphics g = Graphics.FromImage(slideBitmap))
            //    {
            //        // Draw the images stored in the current slide
            //        foreach (var image in currentSlide.Images)
            //        {
            //            // For simplicity, draw images at the top-left corner. Adjust as needed.
            //            g.DrawImage(image, 0, 0);
            //        }

            //        // Draw the shapes stored in the current slide
            //        foreach (var shape in currentSlide.Shapes)
            //        {
            //            g.DrawImage(shape.Item1, shape.Item2);
            //        }
            //    }

            //    // Set the PictureBox image to the combined image
            //    pic.Image = bm;
            //    pic.Invalidate(); // Refresh the PictureBox to show the updated content
            //}
            //else
            //{
            //    g.Clear(Color.White);
            //    pic.Image = bm;
            //}
            if (currentSlideIndex >= 0 && slides.Count > 0)
            {
                var slide = slides[currentSlideIndex];
                // Clear the PictureBox
                g.Clear(Color.White);
                // Draw images and shapes stored in the current slide
                foreach (var image in slide.Images)
                {
                    g.DrawImage(image, 0, 0); // Draw images
                }
                foreach (var shape in slide.Shapes)
                {
                    g.DrawImage(shape.Item1, shape.Item2); // Draw shapes
                }
                pic.Image = bm;
            }
            else
            {
                g.Clear(Color.White);
                pic.Image = bm;
            }
        }

        private void SaveCurrentSlideContent()
        {
            if (currentSlideIndex >= 0 && currentSlideIndex < slides.Count)
            {
                if (pic.Image != null)
                {
                    Bitmap slideImage = new Bitmap(pic.Image);
                    slides[currentSlideIndex].Images.Add(slideImage);
                }
            }

        }
        private void UpdateSlideList()
        {
            lstSlides.Items.Clear();
            foreach (var slide in slides)
            {
                // Check if slide.Name is null or empty and provide a default name if it is
                string displayName = string.IsNullOrEmpty(slide.Name) ? "Unnamed Slide" : slide.Name;
                lstSlides.Items.Add(displayName);
            }
        }
        private void lstSlides_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSlides.SelectedIndex >= 0 && lstSlides.SelectedIndex < slides.Count)
            {
                SaveCurrentSlideContent();
                currentSlideIndex = lstSlides.SelectedIndex;
                DisplayCurrentSlide();
            }
            // Assuming you have variables to keep track of slides
            int totalSlides = slides.Count;
            int currentSlideNumber = currentSlideIndex + 1; // Adding 1 to make it human-readable (1-indexed)

            // Update the label text
            label1.Text = $"{currentSlideNumber}/{totalSlides}";
        }
        private void btn_SaveAll_Click(object sender, EventArgs e)
        {


            if (slides == null || slides.Count == 0)
            {
                MessageBox.Show("No slides to save.");
                return;
            }


            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                //if (slides.Count == 1)
                //{
                //    // saveDialog.Filter = "JPEG Image|*.jpg|JPEG Image|*.jpeg|PNG Image|*.png";
                //    saveDialog.Filter = "PNG Image (*.png)|*.png";
                //    saveDialog.Title = "Save Drawboard Image";
                //}
                //else if (slides.Count > 1)
                //{
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.Title = "Save Drawboard PDF file";

                // }
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string pdfFilePath = saveDialog.FileName;

                    // Create a new PDF document
                    using (Document document = new Document())
                    {
                        try
                        {
                            // Create a PdfWriter instance to write to the PDF file
                            PdfWriter.GetInstance(document, new System.IO.FileStream(pdfFilePath, System.IO.FileMode.Create));

                            // Open the document
                            document.Open();

                            // Iterate through each slide and add it to the PDF
                            for (int i = 0; i < slides.Count; i++)
                            {
                                // Create a new bitmap with the same size as the PictureBox
                                Bitmap combinedImage = new Bitmap(pic.Width, pic.Height);

                                // Get the graphics object of the combined image
                                using (Graphics g = Graphics.FromImage(combinedImage))
                                {
                                    // Clear the background with white color
                                    g.Clear(Color.White);

                                    // Draw the images stored in the slide
                                    foreach (var imageRect in slides[i].Images)
                                    {
                                        g.DrawImage(imageRect, Point.Empty);
                                    }

                                    // Draw the shapes stored in the slide
                                    foreach (var shape in slides[i].Shapes)
                                    {
                                        g.DrawImage(shape.Item1, shape.Item2);
                                    }
                                }

                                // Convert the combined image to a byte array
                                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                                {
                                    combinedImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    byte[] imageBytes = stream.ToArray();

                                    // Add the image to the PDF document
                                    iTextSharp.text.Image slideImage = iTextSharp.text.Image.GetInstance(imageBytes);
                                    slideImage.ScaleToFit(document.PageSize.Width - document.LeftMargin - document.RightMargin,
                                                          document.PageSize.Height - document.TopMargin - document.BottomMargin);
                                    document.Add(slideImage);

                                    // Add a new page for the next slide
                                    if (i < slides.Count - 1)
                                    {
                                        document.NewPage();
                                    }
                                }
                            }

                            MessageBox.Show("Saved and Uploaded Successfully.");

                            // Attach the saved PDF file to the panel
                            AttachFilesToPanel2(pdfFilePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }

        }

        private iTextSharp.text.Image ConvertSlideToImage(Slide slide)
        {
            int slideWidth = 800;
            int slideHeight = 600;
            using (Bitmap bitmap = new Bitmap(slideWidth, slideHeight))
            {
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = stream.ToArray();
                    return iTextSharp.text.Image.GetInstance(imageBytes);
                }
            }
        }

        private void lstSlides_DoubleClick(object sender, EventArgs e)
        {
            ShowRenameTextBox();
        }

        private void ShowRenameTextBox()
        {
            if (lstSlides.SelectedIndex != -1)
            {
                System.Drawing.Rectangle itemRect = lstSlides.GetItemRectangle(lstSlides.SelectedIndex);
                txtRename.Bounds = new System.Drawing.Rectangle(itemRect.Location.X, itemRect.Location.Y + lstSlides.Top, itemRect.Width, itemRect.Height);
                txtRename.Text = lstSlides.SelectedItem.ToString();
                txtRename.Visible = true;
                txtRename.Focus();
                txtRename.SelectAll();
                txtRename.KeyDown += txtRename_KeyDown;
                txtRename.LostFocus += txtRename_LostFocus;
            }
        }
        private void txtRename_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveRenamedItem();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CancelRenaming();
            }
        }
        private void txtRename_LostFocus(object sender, EventArgs e)
        {
            SaveRenamedItem();
        }
        private void SaveRenamedItem()
        {
            int selectedIndex = lstSlides.SelectedIndex;
            if (selectedIndex != -1)
            {
                slides[selectedIndex].Name = txtRename.Text;
                UpdateSlideList();
                lstSlides.SelectedIndex = selectedIndex;
            }
            CancelRenaming();
        }
        private void CancelRenaming()
        {
            txtRename.Visible = false;
            txtRename.KeyDown -= txtRename_KeyDown;
            txtRename.LostFocus -= txtRename_LostFocus;
        }
        private void AttachMouseDownEventHandler(System.Windows.Forms.Control parent)
        {
            foreach (System.Windows.Forms.Control control in parent.Controls)
            {
                control.MouseDown += new MouseEventHandler(AnyControl_MouseDown);
                if (control.HasChildren)
                {
                    AttachMouseDownEventHandler(control);
                }
            }
        }
        private void AnyControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (panel4.Visible)
            {
                panel4.Visible = false;
            }
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (panel1.ClientRectangle.Contains(e.Location))
            {
                panel1.Cursor = Cursors.Default;
            }
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)

        {
            trackBar1.Visible = false;
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            trackBar2.Visible = false;
        }

        private void PerformDrawing(System.Drawing.Image image, System.Drawing.Rectangle rectangle)
        {

        }
        public void btn_undo_Click(object sender, EventArgs e)
        {
            Undo();

        }


        private void ClearLastDrawing(List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>> lastDrawing)
        {

        }

        private void AddDrawingAction(System.Drawing.Image image, System.Drawing.Rectangle location)
        {

        }

        private void pic_Click(object sender, EventArgs e)
        {

        }

        private void pic_PaddingChanged(object sender, EventArgs e)
        {

        }

        private void btn_redo_Click(object sender, EventArgs e)
        {
            Redo();


        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pic_color_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void Closebutton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the page?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
              //  MeetingsInfo_ComposeEmail m = new MeetingsInfo_ComposeEmail();
                //panel1.Controls.Clear();
               // panel1.Visible = false;
                //LoadAttachments();
            }
            //MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
            //if (mc != null)
            //{

            //    //DialogResult result = MessageBox.Show("Are you sure you want to close the page?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    DialogResult result = MessageBox.Show("Are you sure you want to close the page?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    // If the user clicks "Yes", close the form
            //    if (result == DialogResult.Yes)
            //    {

            //        panel1.Controls.Clear();
            //        MeetingsInfo_ComposeEmail meetingsInfo_ComposeEmail = new MeetingsInfo_ComposeEmail();
            //        panel1.Dock = DockStyle.Fill;
            //        panel1.BackColor = SystemColors.Control;
            //        panel1.Controls.Add(meetingsInfo_ComposeEmail);

            //    }


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void ellipse_Click(object sender, EventArgs e)
        {

            index = 3;
            //currentDrawingMode = DrawingMode.Circle;

        }

        private void rectangle_Click(object sender, EventArgs e)
        {
            index = 4;
            // currentDrawingMode = DrawingMode.Rectangle;

        }

        private void Line_Click(object sender, EventArgs e)
        {
            index = 5;
            //currentDrawingMode = DrawingMode.Line;

        }


        private void picbox_color_picker_Click(object sender, EventArgs e)
        {

        }

        private void Diamond_Click(object sender, EventArgs e)
        {
            index = 6;

        }

        private void Arrow_Click(object sender, EventArgs e)
        {
           // index= 7;
        }
    }
}