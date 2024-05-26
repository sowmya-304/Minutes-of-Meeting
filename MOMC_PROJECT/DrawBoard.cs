using FontAwesome.Sharp;
using System.Windows.Shapes;
using static MOMC_PROJECT.MOM_Prop;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace MOMC_PROJECT
{
    public partial class DrawBoard : UserControl
    {
        //draw and color 
        Bitmap bm;
        Graphics g;
        private Point px, py;
        Pen pen = new Pen(Color.Black, 1);
        private bool paint = false;
        Pen erase = new Pen(Color.White, 10);
        private int index;
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
        //undo redo
        private Stack<List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>> undoStack = new Stack<List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>>();
        private Stack<List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>> redoStack = new Stack<List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>>();
        //Saved picturebox path
        public static List<System.Drawing.Image> picimages = new List<System.Drawing.Image>(); // Initialize the list
        //
        private bool isPencilSelected = false;
        private bool isEraserSelected = false;
        private bool isImageSelected = false;
        private bool isIconselected = false;
        public DrawBoard()
        {
            InitializeComponent();
            ScreenLoad();
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
            g.Clear(Color.White);
            pic.Image = bm;
            slides = new List<Slide>();
            currentSlideIndex = -1;
            DisplayCurrentSlide();
            label1.Text = "";
            AttachMouseDownEventHandler(this);
            this.lstSlides.DoubleClick += new System.EventHandler(this.lstSlides_DoubleClick);
            //this.btn_rename.Click += new System.EventHandler(this.btn_rename_Click);
        }
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;
            if (selectedImage != null)
            {
                isImageDrawing = true;
                ImagestartPoint = e.Location;
            }
            if (selectedIconImage != null)
            {
                isShapeDrawing = true;
                ShapeStartPoint = e.Location;
            }
            if (isSelecting)
            {
                isDragging = true;
                startPoint = e.Location;
                selectionRectangle = System.Drawing.Rectangle.Empty;
            }
        }
        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                px = e.Location;
                if (index == 1)
                {
                    g.DrawLine(pen, px, py);
                }
                else if (index == 2)
                {
                    g.DrawLine(erase, px, py);
                }
                py = px;
                pic.Refresh();
            }
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
            if (isImageDrawing && selectedImage != null)
            {
                isImageDrawing = false;
                images.Add(new Tuple<System.Drawing.Image, System.Drawing.Rectangle>(selectedImage, currentImageRect));
                selectedImage = null; // Reset selected image
                Cursor = Cursors.Default; // Change cursor back to default
                pic.Invalidate();
            }
            if (isShapeDrawing && selectedIconImage != null)
            {
                isShapeDrawing = false;
                shapes.Add(new Tuple<System.Drawing.Image, System.Drawing.Rectangle>(selectedIconImage, currentShapeRect));
                selectedIconImage = null; // Reset selected icon image
                Cursor = Cursors.Default; // Change cursor back to default
                pic.Invalidate();
            }
            if (isDragging && isSelecting)
            {
                endPoint = e.Location;
                selectionRectangle = GetShapeBounds(startPoint, endPoint);
                isDragging = false;
                Cursor = Cursors.Default;
                pic.Invalidate(); // Refresh to show the final selection rectangle
            }
            Cursor cursor = Cursors.Default;
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
            Cursor cursor = Cursors.Default;
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
            Control clickedControl = sender as Control;

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
            index = 0;
            Cursor cursor = Cursors.Default;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImage = System.Drawing.Image.FromFile(openFileDialog.FileName);
                    isImageSelected = true;
                    if (selectedImage != null)
                    {
                        Cursor = CreateCursor(selectedImage);
                    }
                }
            }
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
        private void btn_shape_Click(object sender, EventArgs e)
        {
            index = 0;
            Cursor cursor = Cursors.Default;
            panel4.Visible = true;
            // Clear the panel before adding buttons and images
            panel4.Controls.Clear();

            // Define the dimensions of the grid
            int rowCount = 8;
            int colCount = 8;
            int cellWidth = 30; // Adjust as needed
            int cellHeight = 30; // Adjust as needed

            // Create an array of FontAwesome icons
            IconChar[] icons = new IconChar[]
            {
                IconChar.Square,IconChar.Circle, IconChar.Star,
                IconChar.Heart, IconChar.Bell,
                IconChar.Check, IconChar.Times, IconChar.Plus,
                IconChar.Minus, IconChar.ArrowUp, IconChar.ArrowDown,
                IconChar.ArrowLeft, IconChar.ArrowRight, IconChar.Pause,
                IconChar.Play, IconChar.Stop, IconChar.FastForward,
                IconChar.StepForward, IconChar.Music, IconChar.Video,
                IconChar.Image, IconChar.Camera, IconChar.Film,
                IconChar.Bolt, IconChar.Lightbulb, IconChar.Cloud,
                IconChar.Sun, IconChar.Moon, IconChar.Wind,
                IconChar.Fire, IconChar.Water, IconChar.Snowflake,
                IconChar.Tree, IconChar.Leaf,IconChar.Globe,
                IconChar.Map, IconChar.Compass,
                IconChar.Flag, IconChar.Cog, IconChar.Tools,
                IconChar.Hammer, IconChar.Wrench, IconChar.Screwdriver,
                IconChar.Lock, IconChar.Unlock, IconChar.Key,
                IconChar.Home, IconChar.Building, IconChar.Car,
                IconChar.Bicycle, IconChar.Bus, IconChar.Train,
                IconChar.Plane, IconChar.Ship, IconChar.Subway,
                IconChar.Truck, IconChar.Horse, IconChar.Dog,
                IconChar.Cat, IconChar.Fish, IconChar.Dove,
                IconChar.Dragon, IconChar.Frog, IconChar.Paw
            };

            int iconIndex = 0;
            int totalIcons = icons.Length;

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    IconButton btn = new IconButton();
                    btn.Width = cellWidth;
                    btn.Height = cellHeight;
                    btn.Top = row * cellHeight;
                    btn.Left = col * cellWidth;
                    btn.IconChar = icons[iconIndex % totalIcons];
                    btn.IconColor = Color.Black;
                    btn.IconSize = 20;
                    btn.Text = "";
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += IconButton_Click; // Add click event handler
                    panel4.Controls.Add(btn);
                    iconIndex++;
                }
            }
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;

            isSelecting = !isSelecting; // Toggle the selection mode
            if (!isSelecting)
            {
                selectionRectangle = System.Drawing.Rectangle.Empty; // Clear the selection rectangle if deselecting
                pic.Invalidate(); // Refresh the PictureBox to update the display
            }
            selectedIcon = IconChar.None; // Disable drawing icons when selecting
            index = 0;
        }
        private bool IsRectangleIntersecting(System.Drawing.Rectangle rect1, System.Drawing.Rectangle rect2)
        {
            return rect1.IntersectsWith(rect2);
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (selectionRectangle != System.Drawing.Rectangle.Empty)
            {
                // Iterate over the list in reverse order to safely remove items
                for (int i = images.Count - 1; i >= 0; i--)
                {
                    if (IsRectangleIntersecting(images[i].Item2, selectionRectangle))
                    {
                        images.RemoveAt(i);
                    }
                }

                selectionRectangle = System.Drawing.Rectangle.Empty; // Clear the selection rectangle
                pic.Invalidate(); // Refresh the PictureBox to update the display
            }
            isSelecting = false; // Disable selection mode
        }
        private void btn_uplaod_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set the file filter to allow saving as JPEG, JPG, or PNG
                saveFileDialog.Filter = "JPEG Image|*.jpg|JPEG Image|*.jpeg|PNG Image|*.png";

                // Display the dialog and wait for the user's response
                DialogResult result = saveFileDialog.ShowDialog();

                // If the user clicks "Save"
                if (result == DialogResult.OK)
                {
                    // Get the selected filename
                    string fileName = saveFileDialog.FileName;

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

                    // Save the combined image to the selected location
                    combinedImage.Save(fileName);
                }
            }
            panel1.Controls.Clear();
            panel1.BackColor = SystemColors.Control;
            MeetingsInfo_ComposeEmail m=new MeetingsInfo_ComposeEmail();
            panel1.Dock= DockStyle.Fill;    
            panel1.Controls.Add(m);
        }
        private void btn_SaveClose_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set the file filter to allow saving as JPEG, JPG, or PNG
                saveFileDialog.Filter = "JPEG Image|*.jpg|JPEG Image|*.jpeg|PNG Image|*.png";

                // Display the dialog and wait for the user's response
                DialogResult result = saveFileDialog.ShowDialog();

                // If the user clicks "Save"
                if (result == DialogResult.OK)
                {
                    // Get the selected filename
                    string fileName = saveFileDialog.FileName;

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

                    // Save the combined image to the selected location
                    combinedImage.Save(fileName);
                }
            }
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            MeetingsInfo_ComposeEmail meetingsInfo_ComposeEmail = new MeetingsInfo_ComposeEmail();
            panel1.Dock = DockStyle.Fill;
            panel1.BackColor = SystemColors.Control;
            panel1.Controls.Add(meetingsInfo_ComposeEmail);
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
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.Title = "Save PDF";
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
                                    combinedImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
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

                            MessageBox.Show("PDF saved successfully.");
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
        private void AttachMouseDownEventHandler(Control parent)
        {
            foreach (Control control in parent.Controls)
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
    }
}