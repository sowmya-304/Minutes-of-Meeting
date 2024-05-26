using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MOMC_PROJECT
{
    public partial class DrawImageIcon : UserControl
    {
        private string selectedShape = ""; // Variable to store the selected shape

        public DrawImageIcon()
        {
            InitializeComponent();
        }
        public List<System.Drawing.Image> GenerateShapeImages(int width, int height)
        {
            List<System.Drawing.Image> shapeImages = new List<System.Drawing.Image>();
            string[] shapes = new string[]
            {
                "Rectangle", "Ellipse", "Star", "Triangle","Curve",
                "RoundedRectangle", "Polygon","Parallelogram", "Rhombus", "Hexagon",
                "Pentagon","RightArrow", "LeftArrow", "UpArrow", "DownArrow"
            };

            foreach (var shapeType in shapes)
            {
                System.Drawing.Image shapeImage = CreateShapeImage(shapeType, width, height);
                shapeImages.Add(shapeImage);
            }
            return shapeImages;
        }

        private System.Drawing.Image CreateShapeImage(string shapeType, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width - 1, height - 1); // Adjusted to avoid border clipping

                switch (shapeType)
                {
                    case "Rectangle":
                        g.FillRectangle(Brushes.White, rect);
                        g.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width - 1, rect.Height - 1); // Draw border
                        break;
                    case "Ellipse":
                        g.FillEllipse(Brushes.White, rect);
                        g.DrawEllipse(Pens.Black, rect.X, rect.Y, rect.Width - 1, rect.Height - 1); // Draw border
                        break;
                    case "Star":
                        const int numPoints = 5;
                        PointF[] starPoints = new PointF[2 * numPoints];
                        float outerRadius = width / 2;
                        float innerRadius = outerRadius * 0.4f;
                        double angleIncrement = Math.PI / numPoints;

                        for (int i = 0; i < 2 * numPoints; i++)
                        {
                            float radius = i % 2 == 0 ? outerRadius : innerRadius;
                            double angle = i * angleIncrement;
                            starPoints[i] = new PointF(
                                (float)(width / 2 + radius * Math.Cos(angle)),
                                (float)(height / 2 + radius * Math.Sin(angle))
                            );
                        }

                        g.FillPolygon(Brushes.White, starPoints);
                        g.DrawPolygon(Pens.Black, starPoints); // Draw border
                        break;
                    case "Triangle":
                        // Define triangle points
                        Point[] trianglePoints = {
                            new Point(width / 2, 0),
                            new Point(width, height - 1), // Adjusted to include the bottom edge
                            new Point(0, height - 1) // Adjusted to include the bottom edge
                        };

                        // Fill triangle with white color
                        g.FillPolygon(Brushes.White, trianglePoints);

                        // Draw triangle border
                        g.DrawPolygon(Pens.Black, trianglePoints);

                        break;

                    case "Curve":
                        Point[] curvePoints = {
                    new Point(0, height),
                    new Point(width / 2, 0),
                    new Point(width, height)
                };
                        g.DrawCurve(Pens.Black, curvePoints);
                        break;
                    case "RoundedRectangle":
                        int cornerRadius = 10;
                        GraphicsPath roundedRectPath = RoundedRectangle(rect, cornerRadius);
                        g.FillPath(Brushes.White, roundedRectPath);
                        g.DrawPath(Pens.Black, roundedRectPath); // Draw border
                        break;
                    case "Polygon":
                        // Define polygon points
                        Point[] polygonPoints = {
                        new Point(width / 2, 0),
                        new Point(width - 1, height / 3), // Adjusted to include the bottom edge
                        new Point(width - 1, height - 1), // Adjusted to include the bottom edge
                        new Point(0, height - 1), // Adjusted to include the bottom edge
                        new Point(0, height / 3)
                    };

                        // Fill polygon with white color
                        g.FillPolygon(Brushes.White, polygonPoints);

                        // Draw polygon border
                        g.DrawPolygon(Pens.Black, polygonPoints);

                        break;

                    case "Parallelogram":
                        // Define parallelogram points
                        Point[] parallelogramPoints = {
        new Point(width / 4, 0),
        new Point(width - 1, 0), // Adjusted to include the bottom edge
        new Point(3 * width / 4 - 1, height - 1), // Adjusted to include the bottom edge
        new Point(0, height - 1) // Adjusted to include the bottom edge
    };

                        // Fill parallelogram with white color
                        g.FillPolygon(Brushes.White, parallelogramPoints);

                        // Draw parallelogram border
                        g.DrawPolygon(Pens.Black, parallelogramPoints);

                        break;

                    case "Rhombus":
                        // Define rhombus points
                        Point[] rhombusPoints = {
        new Point(width / 2, 0),
        new Point(width - 1, height / 2), // Adjusted to include the bottom edge
        new Point(width / 2, height - 1), // Adjusted to include the bottom edge
        new Point(0, height / 2) // Adjusted to include the bottom edge
    };

                        // Fill rhombus with white color
                        g.FillPolygon(Brushes.White, rhombusPoints);

                        // Draw rhombus border
                        g.DrawPolygon(Pens.Black, rhombusPoints);

                        break;

                    case "Hexagon":
                        // Define hexagon points
                        Point[] hexagonPoints = {
        new Point(width / 4, 0),
        new Point(3 * width / 4, 0),
        new Point(width - 1, height / 2), // Adjusted to include the bottom edge
        new Point(3 * width / 4, height - 1), // Adjusted to include the bottom edge
        new Point(width / 4, height - 1), // Adjusted to include the bottom edge
        new Point(0, height / 2)
    };

                        // Fill hexagon with white color
                        g.FillPolygon(Brushes.White, hexagonPoints);

                        // Draw hexagon border
                        g.DrawPolygon(Pens.Black, hexagonPoints);

                        break;

                    case "Pentagon":
                        double pentagonRatio = 0.4;
                        Point[] pentagonPoints = {
                    new Point(width / 2, 0),
                    new Point((int)(width * (1 - pentagonRatio)), (int)(height * 0.3)),
                    new Point((int)(width * pentagonRatio), (int)(height * 0.3)),
                    new Point(0, height / 2),
                    new Point(width, height / 2)
                };
                        g.FillPolygon(Brushes.White, pentagonPoints);
                        g.DrawPolygon(Pens.Black, pentagonPoints); // Draw border
                        break;
                    case "RightArrow":
                        Point[] rightArrowPoints = {
                    new Point(0, 0),
                    new Point(width, height / 2),
                    new Point(0, height)
                };
                        g.FillPolygon(Brushes.White, rightArrowPoints);
                        g.DrawPolygon(Pens.Black, rightArrowPoints); // Draw border
                        break;
                    case "LeftArrow":
                        // Define left arrow points
                        Point[] leftArrowPoints = {
        new Point(width - 1, 0), // Adjusted to include the right edge
        new Point(0, height / 2),
        new Point(width - 1, height - 1) // Adjusted to include the right edge
    };

                        // Fill left arrow with white color
                        g.FillPolygon(Brushes.White, leftArrowPoints);

                        // Draw left arrow border
                        g.DrawPolygon(Pens.Black, leftArrowPoints);

                        break;

                    case "UpArrow":
                        // Define up arrow points
                        Point[] upArrowPoints = {
        new Point(0, 0), // Adjusted to start from the top-left corner
        new Point(width, 0), // Adjusted to end at the top-right corner
        new Point(width / 2, height - 1) // Adjusted to include the bottom edge
    };

                        // Fill up arrow with white color
                        g.FillPolygon(Brushes.White, upArrowPoints);

                        // Draw up arrow border
                        g.DrawPolygon(Pens.Black, upArrowPoints);

                        break;

                    case "DownArrow":
                        Point[] downArrowPoints = {
                        new Point(width / 2, height),
                        new Point(0, 0),
                        new Point(width, 0)
                        };
                        g.FillPolygon(Brushes.White, downArrowPoints);
                        g.DrawPolygon(Pens.Black, downArrowPoints); // Draw border
                        break;
                }
            }
            return bmp;
        }
        private GraphicsPath RoundedRectangle(Rectangle rect, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = cornerRadius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(rect.Location, size);

            // Top left arc
            path.AddArc(arc, 180, 90);

            // Top right arc
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right arc
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left arc
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();

            return path;
        }

        private void btn_shape_Click(object sender, EventArgs e)
        {
            {
                panel2.Visible = true;
                panel2.Controls.Clear();
                List<System.Drawing.Image> shapeImages = GenerateShapeImages(30, 30);
                int rowCount = 5;
                int colCount = 5;
                int cellWidth = 40;
                int cellHeight = 40;
                int imageIndex = 0;
                int totalImages = shapeImages.Count;
                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < colCount; col++)
                    {
                        if (imageIndex >= totalImages)
                            break;

                        Button btn = new Button();
                        btn.Width = cellWidth;
                        btn.Height = cellHeight;
                        btn.Top = row * cellHeight;
                        btn.Left = col * cellWidth;
                        btn.BackgroundImage = shapeImages[imageIndex];
                        btn.BackgroundImageLayout = ImageLayout.Center;
                        btn.Text = "";
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.Click += ShapeButton_Click; // Add click event handler
                                                        // Set the Tag property to store the shape type
                       // btn.Tag = shapes[imageIndex];

                        panel2.Controls.Add(btn);
                        imageIndex++;
                    }
                }
            }
        }
        private void ShapeButton_Click(object sender, EventArgs e)
        {
            selectedShape = ((Button)sender).BackgroundImage.Tag.ToString();
        }
        private void pic_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && selectedShape != "")
            {
                // Draw the selected shape while mouse movement (dragging)
                DrawShapeOnPictureBox(e.Location);
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            selectedShape = "";

        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {

        }
        private void DrawShapeOnPictureBox(Point location)
        {
            if (pic.Image == null)
            {
                pic.Image = new Bitmap(pic.Width, pic.Height);
            }

            using (Graphics g = Graphics.FromImage(pic.Image))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int width = 50; // Example width for the drawn shape
                int height = 50; // Example height for the drawn shape

                // Draw the selected shape at the mouse location
                switch (selectedShape)
                {
                    case "Rectangle":
                        g.FillRectangle(Brushes.Black, new Rectangle(location.X - width / 2, location.Y - height / 2, width, height));
                        break;
                    case "Ellipse":
                        g.FillEllipse(Brushes.Black, new Rectangle(location.X - width / 2, location.Y - height / 2, width, height));
                        break;
                        // Add cases for other shapes as needed
                }
            }

            pic.Invalidate(); // Refresh the PictureBox to show the changes
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (selectedShape != "")
            {
                // Draw the selected shape at the mouse location
                DrawShapeOnPictureBox(e.Location);
            }
        }
    }
}
