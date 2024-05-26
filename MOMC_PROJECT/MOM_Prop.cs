using System;
using System.Collections.Generic;
using System.Drawing; // Add this namespace for System.Drawing.Image and System.Drawing.Rectangle
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOMC_PROJECT
{
    public class MOM_Prop
    {
        public class MeetingData
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public List<Meeting> Meetings { get; set; }
        }

        public class Meeting
        {
            public string Name { get; set; }
            public DateTime StartDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
            public List<string> AttendeeEmail { get; set; }
            public List<string> AttendeeName { get; set; }
            public List<string> DrawBoardImages { get; set; }
            public List<string> Documents { get; set; }
        }
        public class Slide
        {
            public string Title { get; set; }
            public List<System.Drawing.Image> Images { get; set; } // Fix here
            public List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>> Shapes { get; set; } // Fix here
            public string Name { get; set; } = "";

            // Add any other properties you need for the slide

            public Slide(string title)
            {
                Title = title;
                Images = new List<System.Drawing.Image>(); // Fix here
                Shapes = new List<Tuple<System.Drawing.Image, System.Drawing.Rectangle>>(); // Fix here
            }
        }
        public class PictureBoxContent
        {
            public string Title { get; set; }

            // Store pen lines
            public List<Point[]> PenLines { get; set; }

            // Store pen colors
            public List<Color> PenColors { get; set; }

            // Store images
            public List<System.Drawing.Image> Images { get; set; } // Fix here

            // Store shapes (rectangles, ellipses, etc.)
            // You can define another class to represent shapes if needed
            public List<Tuple<System.Drawing.Image, Rectangle>> Shapes { get; set; } // Fix here

            // Constructor
            public PictureBoxContent(string title)
            {
                Title = title;
                PenLines = new List<Point[]>();
                PenColors = new List<Color>();
                Images = new List<System.Drawing.Image>(); // Fix here
                Shapes = new List<Tuple<System.Drawing.Image, Rectangle>>(); // Fix here
            }
        }
        public class Shape
        {
            public Rectangle Bounds { get; set; }
            public Color FillColor { get; set; }
            public string ShapeType { get; set; }

            // Constructor
            public Shape(Rectangle bounds, Color fillColor, string shapeType)
            {
                Bounds = bounds;
                FillColor = fillColor;
                ShapeType = shapeType;
            }
        }
    }
}
