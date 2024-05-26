using Minutes_of_Meeting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Mail;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;

namespace Minutes_of_Meeting
{
    public partial class Form1 : Form
    {

        private Point? previousPoint;
        private Bitmap drawing;
        private List<Bitmap> slides;
        private Bitmap currentSlide;
        private Graphics currentGraphics;
        private Pen currentPen;
        public Form1()
        {
            InitializeComponent();
            InitializeDrawingBoard();
        }
        private void InitializeDrawingBoard()
        {
            drawing = new Bitmap(panel8.Width, panel8.Height);
            slides = new List<Bitmap>();
            currentSlide = drawing; // Use drawing as the initial slide
            slides.Add(currentSlide);

            panel8.BackgroundImage = currentSlide;
            currentGraphics = Graphics.FromImage(currentSlide);

            currentPen = new Pen(Color.Black, 2);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            panel4.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            comboBox4.Items.AddRange(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }
        private void LoadData()
        {
            string meetingsFilePath = @"D:\UI\Minutes of Meeting\Minutes of Meeting\meetings.json";

            meetings = LoadMeetings(meetingsFilePath);
        }

        private void PopulateComboBox()
        {
            comboBox1.Items.Clear();
            foreach (var meeting in meetings)
            {
                comboBox1.Items.Add(meeting.Title);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel6.Visible = true;
            panel7.Visible = true;
            string selectedTitle = comboBox1.SelectedItem.ToString();
            Meeting selectedMeeting = meetings.Find(m => m.Title == selectedTitle);
            Console.WriteLine("Selected Meeting:");
            Console.WriteLine($"Title: {selectedMeeting.Title}");
            textBox1.Text = selectedMeeting.Title;
            textBox6.Text = selectedMeeting.Title;
            Console.WriteLine($"Date: {selectedMeeting.Date.ToShortDateString()}");
            textBox2.Text = selectedMeeting.Date.ToString();
            textBox7.Text = selectedMeeting.Date.ToString();
            Console.WriteLine($"Time: {selectedMeeting.Time}");
            textBox3.Text = selectedMeeting.Time.ToString();
            textBox8.Text = selectedMeeting.Time.ToString();
            Console.WriteLine($"Duration: {selectedMeeting.Duration}");
            textBox4.Text = selectedMeeting.Duration.ToString();
            textBox9.Text = selectedMeeting.Duration.ToString();
            Console.WriteLine($"Minutes: {selectedMeeting.Minutes}");
            Console.WriteLine("Attendees:");
            checkedListBox1.Items.Clear();
            foreach (var attendee in selectedMeeting.Attendees)
            {
                Console.WriteLine($"- Name: {attendee.Name}, Email: {attendee.Email.Address}");
                var names = $"{attendee.Name}({attendee.Email.Address})".Split(',');

                checkedListBox1.Items.AddRange(names);

                foreach (var name in names)
                {
                    int index = checkedListBox1.Items.IndexOf(name);
                    if (index != -1)
                    {
                        checkedListBox1.SetItemChecked(index, true);
                    }
                }
            }
            loadDescription();
            LoadFontSizes();
            string[] fontNames = { "Arial", "Times New Roman", "Verdana", "Courier New", "Tahoma" };
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(fontNames);
            comboBox2.SelectedIndex = 0; 
            Console.WriteLine($"Admin Email: {selectedMeeting.AdminEmail.Address}");
            Console.WriteLine();
        }
        private List<Meeting> LoadMeetings(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Meeting>>(json);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
            LoadData();
            PopulateComboBox();
        }
        public class Meeting
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Attendee> Attendees { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan Time { get; set; }
            public TimeSpan Duration { get; set; }
            public string Minutes { get; set; }
            public List<string> DrawboardImages { get; set; }
            [JsonConverter(typeof(MailAddressConverter))]
            public MailAddress AdminEmail { get; set; }
        }

        public class Attendee
        {
            public string Name { get; set; }
            [JsonConverter(typeof(MailAddressConverter))]
            public MailAddress Email { get; set; }
        }
        public class AttendeesData
        {
            public List<string> Attendees { get; set; }
        }

        public class DocumentsData
        {
            public List<string> Documents { get; set; }
        }
        private List<Panel> buttonPanels = new List<Panel>(); 

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true; 
            openFileDialog.Filter = "All Files (*.*)|*.*"; 

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int buttonWidth = 200; 
                int buttonHeight = 50;
                int fileButtonGap = 20; 
                int removeButtonGap = 0; 
                int maxButtonsPerRow = (panel5.Width) / (buttonWidth + fileButtonGap); 
                int fileCount = 0; 
                panel5.Controls.Clear();
                buttonPanels.Clear();
                panel5.AutoScroll = true;
                foreach (string selectedFilePath in openFileDialog.FileNames)
                {
                    string filePath = selectedFilePath; 
                    int rowIndex = fileCount / maxButtonsPerRow;
                    int colIndex = fileCount % maxButtonsPerRow;
                    int x = colIndex * (buttonWidth + fileButtonGap);
                    int y = rowIndex * (buttonHeight + removeButtonGap); 
                    Panel buttonPanel = new Panel();
                    buttonPanel.Size = new Size(buttonWidth, buttonHeight); 
                    buttonPanel.Location = new Point(x, y); 
                    buttonPanel.BorderStyle = BorderStyle.FixedSingle; 
                    PictureBox iconPictureBox = new PictureBox();
                    iconPictureBox.Size = new Size(32, 32); 
                    iconPictureBox.Location = new Point(10, (buttonHeight - iconPictureBox.Height) / 2); 
                    iconPictureBox.Image = Icon.ExtractAssociatedIcon(filePath).ToBitmap(); 
                    iconPictureBox.SizeMode = PictureBoxSizeMode.StretchImage; 
                    buttonPanel.Controls.Add(iconPictureBox);
                    Label fileInfoLabel = new Label();
                    fileInfoLabel.AutoSize = true;
                    fileInfoLabel.Location = new Point(iconPictureBox.Right + 10, (buttonHeight - fileInfoLabel.Height) / 2); 
                    fileInfoLabel.Text = $"{Path.GetFileName(filePath)} ({new FileInfo(filePath).Length / 1024} KB)"; 
                    int maxLabelWidth = buttonWidth - (iconPictureBox.Right + 40); 
                    fileInfoLabel.MaximumSize = new Size(maxLabelWidth, buttonHeight); 
                    buttonPanel.Controls.Add(fileInfoLabel);
                    System.Windows.Forms.Button removeButton = new System.Windows.Forms.Button();
                    removeButton.Text = "X"; 
                    removeButton.Size = new Size(20, 20); 
                    removeButton.Location = new Point(buttonWidth - 30, (buttonHeight - removeButton.Height) / 2); 
                    removeButton.Click += (btnSender, btnE) =>
                    {
                        int indexToRemove = buttonPanels.IndexOf(buttonPanel);
                        buttonPanels.RemoveAt(indexToRemove);
                        panel5.Controls.Remove(buttonPanel);
                        for (int i = indexToRemove; i < buttonPanels.Count; i++)
                        {
                            int row = i / maxButtonsPerRow;
                            int col = i % maxButtonsPerRow;
                            int newX = col * (buttonWidth + fileButtonGap);
                            int newY = row * (buttonHeight + removeButtonGap);
                            buttonPanels[i].Location = new Point(newX, newY);
                        }
                        fileCount--; 
                    };
                    buttonPanel.Controls.Add(removeButton);
                    /*  buttonPanel.Click += (panelSender, panelE) =>
                      {
                          Process.Start(filePath); 
                      };*/
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
                    panel5.Controls.Add(buttonPanel);
                    buttonPanels.Add(buttonPanel);
                    fileCount++;
                }
            }
        }
        public class MailAddressConverter : JsonConverter<MailAddress>
        {
            public override MailAddress ReadJson(JsonReader reader, Type objectType, MailAddress existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.String)
                {
                    string email = (string)reader.Value;
                    return new MailAddress(email);
                }
                throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
            }

            public override void WriteJson(JsonWriter writer, MailAddress value, JsonSerializer serializer)
            {
                writer.WriteValue(value.Address);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            StringBuilder selectedItems = new StringBuilder();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) && i != e.Index)
                {
                    selectedItems.AppendLine(checkedListBox1.Items[i].ToString());
                }
            }
            if (e.NewValue == CheckState.Checked)
            {
                selectedItems.AppendLine(checkedListBox1.Items[e.Index].ToString());
            }
            textBox5.Text = selectedItems.ToString();
        }
        private Dictionary<string, string> emailData = new Dictionary<string, string>(); // Dictionary to store subject and description
        private List<Meeting> meetings;

        private void button4_Click(object sender, EventArgs e)
        {
            string subject = textBox6.Text;
            string description = richTextBox1.Rtf; // Save description in RTF format
            if (!string.IsNullOrWhiteSpace(subject) && !string.IsNullOrWhiteSpace(description))
            {
                if (emailData.ContainsKey(subject))
                {
                    emailData[subject] = description; // Update existing entry
                }
                else
                {
                    emailData.Add(subject, description); // Add new entry
                }

                // Save the data to a file
                SaveEmailDataToFile();
            }
            else
            {
                MessageBox.Show("Please enter both subject and description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveEmailDataToFile()
        {
            string filePath = "EmailData.txt"; 
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var entry in emailData)
                {
                    writer.WriteLine(entry.Key); // Write subject
                    writer.WriteLine(entry.Value); // Write description
                    writer.WriteLine(); // Add empty line as separator
                }
            }
        }
        public void loadDescription()
        {
            string subject = textBox6.Text;
            if (!string.IsNullOrWhiteSpace(subject) && emailData.ContainsKey(subject))
            {
                richTextBox1.Rtf = emailData[subject]; // Load description from dictionary
            }
            else
            {
                richTextBox1.Clear();
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFontStyle();

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFontStyle();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog.Color;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Bold);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Italic);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Underline);
        }
        private void LoadFontSizes()
        {
            // Clear existing items in the ComboBox
            comboBox3.Items.Clear();

            // Add font sizes to the ComboBox
            for (int i = 8; i <= 72; i += 2) // You can adjust the range of font sizes as needed
            {
                comboBox3.Items.Add(i.ToString());
            }

            // Select the default font size
            comboBox3.SelectedIndex = 0;
        }

        private void ApplyFontStyle()
        {
            if (comboBox2.SelectedItem != null && comboBox3.SelectedItem != null)
            {
                string selectedFont = comboBox2.SelectedItem.ToString();
                float selectedSize = float.Parse(comboBox3.SelectedItem.ToString());

                // Get the current selection start and length
                int selectionStart = richTextBox1.SelectionStart;
                int selectionLength = richTextBox1.SelectionLength;

                // Set the font for the selected text
                richTextBox1.SelectionFont = new Font(selectedFont, selectedSize);

                // Restore the selection start and length
                richTextBox1.Select(selectionStart, selectionLength);
            }
        }
        private void ToggleFontStyle(FontStyle style)
        {
            FontStyle fontStyle = richTextBox1.SelectionFont.Style;

            if (fontStyle.HasFlag(style))
            {
                fontStyle &= ~style;
            }
            else
            {
                fontStyle |= style;
            }

            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, fontStyle);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            /* List<Attendee> selectedInvitees = new List<Attendee>();
             foreach (var item in checkedListBox1.CheckedItems)
             {
                 string[] parts = item.ToString().Split('(');
                 string name = parts[0].Trim();
                 string email = parts[1].Trim(')', ' ');
                 selectedInvitees.Add(new Attendee { Name = name, Email = new MailAddress(email) });
             }

             // Set up the SMTP client
             using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)) 
             {
                 smtpClient.EnableSsl = true; // Enable SSL if required
                 smtpClient.Credentials = new System.Net.NetworkCredential("ramasaninarayanareddy@gmail.com", "reddy@1234"); 

                 // Send emails to each selected invitee
                 foreach (var invitee in selectedInvitees)
                 {
                     MailMessage mailMessage = new MailMessage();
                     mailMessage.From = new MailAddress("ramasaninarayanareddy@gmail.com"); // Replace with your email address
                     mailMessage.To.Add(invitee.Email);
                     mailMessage.Subject = "Greetings";
                     mailMessage.Body = "Hello " + invitee.Name + ",\n\nThis is a test email.";

                     try
                     {
                         smtpClient.Send(mailMessage);
                         MessageBox.Show($"Email sent to {invitee.Name} at {invitee.Email}", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                     catch (Exception ex)
                     {
                         Console.WriteLine($"Failed to send email to {invitee.Name}: {ex.Message} + {ex.StackTrace} ");
                         MessageBox.Show($"Failed to send email to {invitee.Name}: {ex.Message} + {ex.StackTrace} ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                }
           }*/
            try
            {
/*                MailMessage mm = new MailMessage("ramasaninarayanareddy@gmail.com", "dheerajreddyramasani@gmail.com");
                mm.Subject = "Test";
                mm.Body = "Hai";
                SmtpClient s = new SmtpClient();
                s.Host = "smtp.gmail.com";
                s.Port = 587;
                System.Net.NetworkCredential ss = new System.Net.NetworkCredential("ramasaninarayanareddy@gmail.com", "reddy@1234");
                s.EnableSsl = true;
                s.Credentials = ss;
                s.Send(mm);
                MessageBox.Show("Mail sent");*/
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("dheerajreddyramasani@gmail.com");
                    mail.To.Add("19n81a0427@gmail.com");
                    mail.Subject = "Hello World";
                    mail.Body = "Hello";
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential("dheerajreddyramasani@gmail.com", "flek dmmr yecf lfxa");
                        smtp.Send(mail);
                        MessageBox.Show("Mail sent");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "  ,  "+ ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel7.Visible = true;
            panel8.Visible = true;
            //panel2.Visible = false;
          //  panel6.Visible=false;
        }

        private void panel8_MouseDown(object sender, MouseEventArgs e)
        {
            previousPoint = e.Location;
        }

        private void panel8_MouseMove(object sender, MouseEventArgs e)
        {
            if (previousPoint != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    using (Graphics g = Graphics.FromImage(drawing))
                    {
                        g.DrawLine(currentPen, previousPoint.Value, e.Location);
                    }
                    panel8.Invalidate();
                    previousPoint = e.Location;
                }
            }
        }

        private void panel8_MouseUp(object sender, MouseEventArgs e)
        {
            previousPoint = null;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(drawing))
            {
                g.Clear(panel8.BackColor);
            }
            panel8.Invalidate();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                currentPen.Color = colorDialog.Color;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPen.Width = Convert.ToInt32(comboBox4.SelectedItem);
        }
        //add slide
        private void button14_Click(object sender, EventArgs e)
        {
            Bitmap newSlide = new Bitmap(panel8.Width, panel8.Height);
            slides.Add(newSlide);

            // Switch to the new slide
            currentSlide = newSlide;
            panel8.BackgroundImage = currentSlide;
            currentGraphics = Graphics.FromImage(currentSlide);
        }
        // next slide
        private void button15_Click(object sender, EventArgs e)
        {
            int currentIndex = slides.IndexOf(currentSlide);
            if (currentIndex < slides.Count - 1)
            {
                currentSlide = slides[currentIndex + 1];
                panel8.BackgroundImage = currentSlide;
                currentGraphics = Graphics.FromImage(currentSlide);
            }
        }
        // previous lside
        private void button16_Click(object sender, EventArgs e)
        {
            int currentIndex = slides.IndexOf(currentSlide);
            if (currentIndex > 0)
            {
                currentSlide = slides[currentIndex - 1];
                panel8.BackgroundImage = currentSlide;
                currentGraphics = Graphics.FromImage(currentSlide);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

