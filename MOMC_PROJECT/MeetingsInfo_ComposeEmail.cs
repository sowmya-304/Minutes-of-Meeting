using FontAwesome.Sharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MOMC_PROJECT.MOM_Prop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MOMC_PROJECT
{
    public partial class MeetingsInfo_ComposeEmail : UserControl
    {
        private List<MeetingData> meetingDataList;
        public static List<string> attachments = new List<string>();
        public static string selected_meeting_name = "";
        private ComboBox comboBox1; // Declare comboBox1 at the class level
        private ComboBox comboBox3;
        private Button selectedButton = null; // To keep track of the selected button in panel9
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();
        private string currentText = "";
        private DataGridView dataGridView1;
        public MeetingsInfo_ComposeEmail()
        {
            InitializeComponent();
            currentText = richTextBox3.Text; // Initialize current text
            UpdateUndoRedoButtons();
            OnLoad();
            PopulateMeetingsComboBox();
            AttachMouseDownEventHandler(this);
            InitializeDataGridView();
            InitializeComponent1();

        }
        private void InitializeComponent1()
        {
            AddBulletButton("•", new Point(0, 0));
            AddBulletButton("◦", new Point(60, 0));
            AddBulletButton("▪", new Point(0, 60));
            AddBulletButton("▫", new Point(60, 60));
        }
        private void AddBulletButton(string bulletChar, Point location)
        {
            // Calculate the maximum button size based on panel size
            int maxWidth = panel9.ClientSize.Width / 6; // Divide by 2 to allow for multiple buttons
            int maxHeight = panel9.ClientSize.Height;

            // Determine the smaller dimension to maintain the button's aspect ratio
            int buttonSize = Math.Min(maxWidth, maxHeight);

            Button button = new Button
            {
                Size = new Size(buttonSize, buttonSize),
                Location = location,
                Text = bulletChar.ToString(),
                TextAlign = ContentAlignment.TopCenter,
            };

            // Adjust font size based on button size
            button.Font = new Font(button.Font.FontFamily, buttonSize / 3, button.Font.Style); // Adjusted font size based on button size

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(button, $"Bullet: {bulletChar}");
            button.Click += btn_mail_bullets_Click;

            panel9.Controls.Add(button);
        }

        /*      private string previousBullet = "";
              private void ApplyBulletToSelectedLine(string bulletChar)
              {
                  int selectionStart = richTextBox3.SelectionStart;
                  int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);

                  // Check if the line index is valid
                  if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
                  {
                      int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
                      int lineLength = richTextBox3.Lines[lineIndex].Length;

                      // Get the current line text
                      string lineText = richTextBox3.Lines[lineIndex];

                      // Check if the current line already has a bullet point
                      if (lineText.StartsWith(previousBullet))
                      {
                          // Replace the existing bullet point with the new one
                          richTextBox3.Select(lineStart, previousBullet.Length);
                          richTextBox3.SelectedText = bulletChar;
                      }
                      else
                      {
                          // Insert the new bullet point at the beginning of the line
                          richTextBox3.Select(lineStart, 0);
                          richTextBox3.SelectedText = bulletChar + " ";
                      }
                      // Store the current bullet character as the previous bullet character
                      previousBullet = bulletChar;
                      // Restore selection to original position
                      richTextBox3.Select(selectionStart + bulletChar.Length + 1, 0);
                      richTextBox3.Focus();
                  }
              }
      */
        /* private string previousBullet = "";
         private void ApplyBulletToSelectedLine(string bulletChar)
         {
             int selectionStart = richTextBox3.SelectionStart;
             int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);

             // Check if the line index is valid
             if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
             {
                 int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
                 int lineLength = richTextBox3.Lines[lineIndex].Length;

                 // Get the current line text
                 string lineText = richTextBox3.Lines[lineIndex];

                 // Check if the current line already has a bullet point
                 if (lineText.StartsWith(previousBullet))
                 {
                     // Replace the existing bullet point with the new one
                     richTextBox3.Select(lineStart, previousBullet.Length);
                     richTextBox3.SelectedText = bulletChar;
                 }
                 else
                 {
                     // Insert the new bullet point at the beginning of the line
                     richTextBox3.Select(lineStart, 0);
                     richTextBox3.SelectedText = bulletChar + " ";
                 }
                 // Store the current bullet character as the previous bullet character
                 previousBullet = bulletChar;

                 // Apply bullet points to subsequent lines with the same bullet point until an empty line or different bullet point
                 for (int i = lineIndex + 1; i < richTextBox3.Lines.Length; i++)
                 {
                     int nextLineStart = richTextBox3.GetFirstCharIndexFromLine(i);
                     string nextLineText = richTextBox3.Lines[i];
                     if (nextLineText.TrimStart().StartsWith(previousBullet) || nextLineText.Trim() == "")
                     {
                         richTextBox3.Select(nextLineStart, 0);
                         richTextBox3.SelectedText = bulletChar + " ";
                     }
                     else
                     {
                         break; // Stop applying bullet points if the next line doesn't match
                     }
                 }

                 // Restore selection to original position
                 richTextBox3.Select(selectionStart + bulletChar.Length + 1, 0);
                 richTextBox3.Focus();
             }
         }*/
        private string previousBullet = "";
        private void ApplyBulletToSelectedLine(string bulletChar)
        {
            int selectionStart = richTextBox3.SelectionStart;
            int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);

            // Check if the line index is valid
            if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
            {
                int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
                int lineLength = richTextBox3.Lines[lineIndex].Length;

                // Get the current line text
                string lineText = richTextBox3.Lines[lineIndex];

                // Check if the current line already has a bullet point
                if (lineText.StartsWith(previousBullet))
                {
                    // Replace the existing bullet point with the new one
                    richTextBox3.Select(lineStart, previousBullet.Length);
                    richTextBox3.SelectedText = bulletChar;
                }
                else if (lineText.StartsWith(previousBullet + " "))
                {
                    // Replace the existing sub-bullet point with the new one
                    richTextBox3.Select(lineStart, previousBullet.Length + 1);
                    richTextBox3.SelectedText = bulletChar + " ";
                }
                else
                {
                    // Insert the new bullet point at the beginning of the line as sub-bullet
                    richTextBox3.Select(lineStart, 0);
                    richTextBox3.SelectedText = bulletChar + " ";
                }

                // Store the current bullet character as the previous bullet character
                previousBullet = bulletChar;

                // Apply bullet points to subsequent lines with the same bullet point until an empty line or different bullet point
                for (int i = lineIndex + 1; i < richTextBox3.Lines.Length; i++)
                {
                    int nextLineStart = richTextBox3.GetFirstCharIndexFromLine(i);
                    string nextLineText = richTextBox3.Lines[i];
                    if (nextLineText.TrimStart().StartsWith(bulletChar) || nextLineText.Trim() == "")
                    {
                        richTextBox3.Select(nextLineStart, 0);
                        richTextBox3.SelectedText = bulletChar + " ";
                    }
                    else
                    {
                        break; // Stop applying bullet points if the next line doesn't match
                    }
                }

                // Restore selection to original position
                richTextBox3.Select(selectionStart + bulletChar.Length + 1, 0);
                richTextBox3.Focus();
            }
        }

        private void InitializeDataGridView()
        {
            // Initialize DataGridView
            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.Size = panel10.Size;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            panel10.Controls.Add(dataGridView1);

            // Create columns
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Columns.Add("Column" + i, "");
            }

            // Create rows
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Rows.Add();
            }
        }
        private void OnLoad()
        {
            // Define the path to the JSON file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

            // Read the JSON file
            string jsonData = File.ReadAllText(filePath);

            // Deserialize the JSON data to a list of your desired object
            meetingDataList = JsonConvert.DeserializeObject<List<MeetingData>>(jsonData);
            cb_emailmeetings.SelectedIndexChanged += cb_emailmeetings_SelectedIndexChanged;
            panel8.Visible = false;
            panel9.Visible = false;
        }
        private void PopulateMeetingsComboBox()
        {
            var meeting_names = meetingDataList.Where(t => t.Email.Equals(MOMC.toEmail)).SelectMany(t => t.Meetings.Select(m => m.Name)).ToList();
            cb_emailmeetings.Items.Clear();
            cb_emailmeetings.Items.AddRange(meeting_names.ToArray());
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel1.Controls.Clear();
            DrawBoard db = new DrawBoard();
            panel1.Dock = DockStyle.Fill;
            panel1.Controls.Add(db);
        }
        private void cb_emailmeetings_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMeetingName = cb_emailmeetings.SelectedItem.ToString();

            if (selectedMeetingName != null && meetingDataList != null)
            {
                var selectedMeeting = meetingDataList.SelectMany(md => md.Meetings).FirstOrDefault(m => m.Name == selectedMeetingName);
                if (selectedMeeting != null)
                {
                    textBox1.Text = selectedMeetingName;
                    textBox2.Text = selectedMeeting.StartDateTime.ToString();
                    textBox3.Text = selectedMeeting.EndDateTime.ToString();
                    textBox4.Text = (selectedMeeting.EndDateTime - selectedMeeting.StartDateTime).ToString();
                    textBox5.Text = MOMC.toEmail;
                    foreach (var attendeeEmail in selectedMeeting.AttendeeEmail)
                    {
                        richTextBox1.Text += attendeeEmail + Environment.NewLine;
                    }
                    richTextBox2.Text = $"Minutes of Meeting: {textBox1.Text} - {textBox3.Text}";
                    clb_attendees.Items.Clear();
                    foreach (var attendee in selectedMeeting.AttendeeEmail)
                    {
                        clb_attendees.Items.Add(attendee, true);
                    }

                    panel4.Controls.Clear();
                    /* foreach (var document in selectedMeeting.Documents)
                     {
                         AddAttachmentPanel(document);
                     }*/
                }
            }
        }
        /*   private void AddAttachmentPanel(string filePath)
           {
               Panel outerPanel = new Panel
               {
                   AutoSize = true,
                   Height = 30
               };

               Panel innerPanel = new Panel
               {
                   Size = new Size(panel4.Width - 20, 30),
                   Location = new Point(0, 0),
                   Tag = filePath
               };

               PictureBox pictureBox = new PictureBox
               {
                   Size = new Size(24, 24),
                   Location = new Point(0, 3),
                   SizeMode = PictureBoxSizeMode.StretchImage,
                   ImageLocation = GetIconPathForFileType(filePath)
               };

               Label attachmentLabel = new Label
               {
                   Text = Path.GetFileName(filePath),
                   AutoSize = true,
                   Location = new Point(30, 0),
                   Cursor = Cursors.Hand,
                   MaximumSize = new Size(panel4.Width - 100, 24),
                   Tag = filePath
               };
               attachmentLabel.Click += AttachmentLabel_Click;

               Button removeButton = new Button
               {
                   Text = "X",
                   Size = new Size(24, 24),
                   Location = new Point(innerPanel.Width - 30, 3),
                   Tag = outerPanel
               };
               removeButton.Click += RemoveButton_Click;

               innerPanel.Controls.Add(pictureBox);
               innerPanel.Controls.Add(attachmentLabel);
               innerPanel.Controls.Add(removeButton);

               outerPanel.Controls.Add(innerPanel);

               panel4.Controls.Add(outerPanel);

               UpdateAttachmentPanelsLayout();
           }*/
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            /*  try
              {
                  if (sender is Button button)
                  {
                      Panel? attachmentPanel = button.Tag as Panel;
                      if (attachmentPanel != null)
                      {
                          string filePath = attachmentPanel.Tag as string;
                          attachments.Remove(filePath);

                          string selectedMeetingName = cb_emailmeetings.SelectedItem.ToString();
                          var selectedMeeting = meetingDataList?
                              .SelectMany(md => md.Meetings)
                              .FirstOrDefault(m => m.Name == selectedMeetingName);

                          if (selectedMeeting != null)
                          {
                              selectedMeeting.Documents.Remove(filePath);
                          }

                          panel4.Controls.Remove(attachmentPanel);
                          UpdateAttachmentPanelsLayout();
                      }
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.StackTrace);
              }*/
        }
        private void AttachmentLabel_Click(object sender, EventArgs e)
        {
            /* if (sender is Label label)
             {
                 string? filePath = label.Tag as string;
                 if (File.Exists(filePath))
                 {
                     Process.Start(new ProcessStartInfo
                     {
                         FileName = filePath,
                         UseShellExecute = true
                     });
                 }
                 else
                 {
                     MessageBox.Show("File not found: " + filePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             }*/
        }
        /*  private string GetIconPathForFileType(string filePath)
          {
              string extension = Path.GetExtension(filePath).ToLower();
              switch (extension)
              {
                  case ".pdf":
                      return @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\pdf.png"; // Provide the path to your PDF icon
                  case ".doc":
                  case ".docx":
                      return @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\word.png"; // Provide the path to your Word icon
                  case ".xls":
                  case ".xlsx":
                      return @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\excel.png"; // Provide the path to your Excel icon
                  case ".png":
                  case ".jpg":
                  case ".jpeg":
                      return @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\image.png"; // Provide the path to your Image icon
                  default:
                      return @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Images\document.png"; // Provide the path to your default icon for other file types
              }
          }
          private void UpdateAttachmentPanelsLayout()
          {
              int y = 0;
              foreach (Control control in panel4.Controls)
              {
                  if (control is Panel panel)
                  {
                      panel.Location = new Point(0, y);
                      y += panel.Height + 5;
                  }
              }
          }*/

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            /*    OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "All Files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        if (!attachments.Contains(fileName))
                        {
                            attachments.Add(fileName);
                            AddAttachmentPanel(fileName);

                            string? selectedMeetingName = cb_emailmeetings.SelectedItem?.ToString();
                            var selectedMeeting = meetingDataList?
                                .SelectMany(md => md.Meetings)
                                .FirstOrDefault(m => m.Name == selectedMeetingName);

                            if (selectedMeeting != null)
                            {
                                selectedMeeting.Documents.Add(fileName);
                            }
                        }
                    }
                }*/
        }

        private void clb_attendees_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMeetingName = cb_emailmeetings.SelectedItem?.ToString();

            if (selectedMeetingName != null && meetingDataList != null)
            {
                var selectedMeeting = meetingDataList
                    .SelectMany(md => md.Meetings)
                    .FirstOrDefault(m => m.Name == selectedMeetingName);

                if (selectedMeeting != null)
                {
                    richTextBox1.Clear(); // Clear existing content

                    // Iterate through all items in the checked list box
                    foreach (var itemIndex in clb_attendees.CheckedIndices)
                    {
                        int index = (int)itemIndex;
                        richTextBox1.AppendText(selectedMeeting.AttendeeEmail[index] + Environment.NewLine);
                    }
                }
            }
        }

        private void btn_sendmail_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                string senderEmail = MOMC.toEmail;
                string senderPassword = "tkki grgd aapo uavx\r\n";
                message.From = new MailAddress(senderEmail);

                // Add all email addresses from richTextBox1 to the To list of the email
                foreach (string email in richTextBox1.Lines)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        message.To.Add(email);
                    }
                }
                message.Subject = richTextBox2.Text;
                message.Body = richTextBox3.Text;
                // Attach files
                foreach (string filePath in attachments)
                {
                    if (File.Exists(filePath))
                    {
                        message.Attachments.Add(new Attachment(filePath));
                    }
                }
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                smtpClient.Send(message);

                // Display success message
                MessageBox.Show("Email sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                // Display error message
                MessageBox.Show("Failed to send email. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_mail_format_Click(object sender, EventArgs e)
        {
            panel7.Visible = true;
            panel8.Visible = false;

        }

        private void btn_mail_insert_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
            panel8.Visible = true;
        }

        private void btn_mail_bold_Click(object sender, EventArgs e)
        {
            int selectionStart = richTextBox3.SelectionStart;
            int selectionLength = richTextBox3.SelectionLength;
            bool isSelected = selectionLength > 0;

            if (isSelected)
            {
                Font currentFont = richTextBox3.SelectionFont;
                FontStyle newFontStyle;

                if (currentFont.Style.HasFlag(FontStyle.Bold))
                {
                    newFontStyle = currentFont.Style & ~FontStyle.Bold; // Unbold
                }
                else
                {
                    newFontStyle = currentFont.Style | FontStyle.Bold; // Bold
                }

                richTextBox3.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
            else
            {
                // Check if the text is already bold or not
                bool isAllBold = richTextBox3.SelectionFont != null && richTextBox3.SelectionFont.Bold;

                // Preserve existing italic, underline, font family, and font size formatting
                FontStyle existingStyle;

                if (isAllBold)
                {
                    existingStyle = richTextBox3.SelectionFont.Style & ~FontStyle.Bold; // Unbold
                }
                else
                {
                    existingStyle = richTextBox3.SelectionFont.Style | FontStyle.Bold; // Bold
                }

                // Apply bold or unbold to entire content
                richTextBox3.SelectAll();
                richTextBox3.SelectionFont = new Font(richTextBox3.Font.FontFamily, richTextBox3.Font.Size, existingStyle);
                richTextBox3.Select(selectionStart, selectionLength);
            }
        }

        private void btn_mail_italic_Click(object sender, EventArgs e)
        {
            int selectionStart = richTextBox3.SelectionStart;
            int selectionLength = richTextBox3.SelectionLength;
            bool isSelected = selectionLength > 0;

            if (isSelected)
            {
                Font currentFont = richTextBox3.SelectionFont;
                FontStyle newFontStyle;

                if (currentFont.Style.HasFlag(FontStyle.Italic))
                {
                    newFontStyle = currentFont.Style & ~FontStyle.Italic; // Unitalic
                }
                else
                {
                    newFontStyle = currentFont.Style | FontStyle.Italic; // Italic
                }

                richTextBox3.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
            else
            {
                // Check if the text is already italic or not
                bool isAllItalic = richTextBox3.SelectionFont != null && richTextBox3.SelectionFont.Italic;

                // Preserve existing bold, underline, font family, and font size formatting
                FontStyle existingStyle;

                if (isAllItalic)
                {
                    existingStyle = richTextBox3.SelectionFont.Style & ~FontStyle.Italic; // Unitalic
                }
                else
                {
                    existingStyle = richTextBox3.SelectionFont.Style | FontStyle.Italic; // Italic
                }

                // Apply italic or unitalic to entire content
                richTextBox3.SelectAll();
                richTextBox3.SelectionFont = new Font(richTextBox3.Font.FontFamily, richTextBox3.Font.Size, existingStyle);
                richTextBox3.Select(selectionStart, selectionLength);
            }
        }

        private void btn_mail_underline_Click(object sender, EventArgs e)
        {
            int selectionStart = richTextBox3.SelectionStart;
            int selectionLength = richTextBox3.SelectionLength;
            bool isSelected = selectionLength > 0;

            if (isSelected)
            {
                Font currentFont = richTextBox3.SelectionFont;
                FontStyle newFontStyle;

                if (currentFont.Style.HasFlag(FontStyle.Underline))
                {
                    newFontStyle = currentFont.Style & ~FontStyle.Underline; // Ununderline
                }
                else
                {
                    newFontStyle = currentFont.Style | FontStyle.Underline; // Underline
                }

                richTextBox3.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
            else
            {
                // Check if the text is already underlined or not
                bool isAllUnderline = richTextBox3.SelectionFont != null && richTextBox3.SelectionFont.Underline;

                // Preserve existing bold, italic, font family, and font size formatting
                FontStyle existingStyle;

                if (isAllUnderline)
                {
                    existingStyle = richTextBox3.SelectionFont.Style & ~FontStyle.Underline; // Ununderline
                }
                else
                {
                    existingStyle = richTextBox3.SelectionFont.Style | FontStyle.Underline; // Underline
                }

                // Apply underline or ununderline to entire content
                richTextBox3.SelectAll();
                richTextBox3.SelectionFont = new Font(richTextBox3.Font.FontFamily, richTextBox3.Font.Size, existingStyle);
                richTextBox3.Select(selectionStart, selectionLength);
            }
        }

        private void btn_mail_ff_Click(object sender, EventArgs e)
        {
            // Make panel9 visible
            panel9.Controls.Clear();
            panel9.Visible = true;

            // Set the size of panel9
            panel9.Size = new Size(212, 167);

            // Clear any existing controls from panel9
            panel9.Controls.Clear();

            // Create a TableLayoutPanel to organize the controls
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                ColumnCount = 3,  // Three columns to accommodate the third row buttons
                RowCount = 6,     // Six rows as specified
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            // Define the column and row styles to fit the panel size
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            for (int i = 0; i < 6; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66F)); // Approximately 1/6 of the height
            }

            // First row with a single label
            Label label1 = new Label
            {
                Text = "Font Formatting",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            tableLayoutPanel.Controls.Add(label1, 0, 0);
            tableLayoutPanel.SetColumnSpan(label1, 3); // Span across all three columns

            // Second row with two combo boxes
            comboBox1 = new ComboBox { Dock = DockStyle.Fill }; // Use the class-level comboBox1
            comboBox3 = new ComboBox { Dock = DockStyle.Fill };
            int[] fontSizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            // Add font sizes to comboBox1
            foreach (int size in fontSizes)
            {
                comboBox1.Items.Add(size);
            }
            comboBox1.IntegralHeight = false;
            comboBox1.MaxDropDownItems = 5;
            comboBox1.SelectedIndexChanged += (s, args) => panel9.Visible = false;

            foreach (FontFamily fontFamily in FontFamily.Families)
            {
                comboBox3.Items.Add(fontFamily.Name);
            }
            comboBox3.IntegralHeight = false;
            comboBox3.MaxDropDownItems = 5;
            comboBox3.SelectedIndexChanged += (s, args) => panel9.Visible = false;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;

            tableLayoutPanel.Controls.Add(comboBox1, 0, 1);
            tableLayoutPanel.Controls.Add(comboBox3, 1, 1);
            tableLayoutPanel.SetColumnSpan(comboBox3, 2); // Span across the second and third columns

            // Third row with three buttons
            Button button1 = new Button { Text = "ab", Dock = DockStyle.Fill };
            Button button2 = new Button { Text = "X2", Dock = DockStyle.Fill };
            Button button3 = new Button { Text = "2X", Dock = DockStyle.Fill };
            button1.Click += (s, args) => panel9.Visible = false;
            button2.Click += (s, args) => panel9.Visible = false;
            button3.Click += (s, args) => panel9.Visible = false;
            tableLayoutPanel.Controls.Add(button1, 0, 2);
            tableLayoutPanel.Controls.Add(button2, 1, 2);
            tableLayoutPanel.Controls.Add(button3, 2, 2);

            // Fourth, fifth, and sixth rows each with a single button
            Button button4 = new Button { Text = "Highlight", Dock = DockStyle.Fill };
            Button button5 = new Button { Text = "Font Color", Dock = DockStyle.Fill };
            Button button6 = new Button { Text = "Clear All Formatting", Dock = DockStyle.Fill };
            button4.Click += (s, args) => panel9.Visible = false;
            button5.Click += (s, args) => panel9.Visible = false;
            button6.Click += (s, args) => panel9.Visible = false;
            tableLayoutPanel.Controls.Add(button4, 0, 3);
            tableLayoutPanel.SetColumnSpan(button4, 3); // Span across all three columns
            tableLayoutPanel.Controls.Add(button5, 0, 4);
            tableLayoutPanel.SetColumnSpan(button5, 3); // Span across all three columns
            tableLayoutPanel.Controls.Add(button6, 0, 5);
            tableLayoutPanel.SetColumnSpan(button6, 3); // Span across all three columns

            // Add the TableLayoutPanel to panel9
            panel9.Controls.Add(tableLayoutPanel);

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                int newSize = Convert.ToInt32(comboBox1.SelectedItem);
                ApplyFontSize(newSize);
            }
        }
        private void ApplyFontSize(int newSize)
        {
            int selectionStart = richTextBox3.SelectionStart;
            int selectionLength = richTextBox3.SelectionLength;
            bool isSelected = selectionLength > 0;

            FontStyle existingStyle = FontStyle.Regular;

            if (richTextBox3.SelectionFont != null)
            {
                if (richTextBox3.SelectionFont.Bold)
                {
                    existingStyle |= FontStyle.Bold;
                }
                if (richTextBox3.SelectionFont.Italic)
                {
                    existingStyle |= FontStyle.Italic;
                }
                if (richTextBox3.SelectionFont.Underline)
                {
                    existingStyle |= FontStyle.Underline;
                }
            }

            if (isSelected)
            {
                Font currentFont = richTextBox3.SelectionFont;
                Font newFont = new Font(currentFont.FontFamily, newSize, existingStyle);
                richTextBox3.SelectionFont = newFont;
            }
            else
            {
                Font currentFont = richTextBox3.Font;
                Font newFont = new Font(currentFont.FontFamily, newSize, existingStyle);
                richTextBox3.Font = newFont;
            }

            // Restore the selection and preserve other formatting
            richTextBox3.Select(selectionStart, selectionLength);
            if (richTextBox3.SelectionFont != null)
            {
                richTextBox3.SelectionFont = new Font(richTextBox3.SelectionFont.FontFamily, newSize, existingStyle);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                string newFontFamily = comboBox3.SelectedItem.ToString();
                ApplyFontFamily(newFontFamily);
            }
        }

        private void ApplyFontFamily(string newFontFamily)
        {
            int selectionStart = richTextBox3.SelectionStart;
            int selectionLength = richTextBox3.SelectionLength;
            bool isSelected = selectionLength > 0;

            FontStyle existingStyle = FontStyle.Regular;

            if (richTextBox3.SelectionFont != null)
            {
                if (richTextBox3.SelectionFont.Bold)
                {
                    existingStyle |= FontStyle.Bold;
                }
                if (richTextBox3.SelectionFont.Italic)
                {
                    existingStyle |= FontStyle.Italic;
                }
                if (richTextBox3.SelectionFont.Underline)
                {
                    existingStyle |= FontStyle.Underline;
                }
            }

            if (isSelected)
            {
                Font currentFont = richTextBox3.SelectionFont;
                Font newFont = new Font(newFontFamily, currentFont.Size, existingStyle);
                richTextBox3.SelectionFont = newFont;
            }
            else
            {
                Font currentFont = richTextBox3.Font;
                Font newFont = new Font(newFontFamily, currentFont.Size, existingStyle);
                richTextBox3.Font = newFont;
            }

            // Restore the selection and preserve other formatting
            richTextBox3.Select(selectionStart, selectionLength);
            if (richTextBox3.SelectionFont != null)
            {
                richTextBox3.SelectionFont = new Font(newFontFamily, richTextBox3.SelectionFont.Size, existingStyle);
            }
        }
        //closes panel9
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
            if (panel9.Visible)
            {
                panel9.Visible = false;
            }
        }

        private void btn_mail_bullets_Click(object sender, EventArgs e)
        {

            Button button = (Button)sender;
            string bulletChar = button.Text;
            ApplyBulletToSelectedLine(bulletChar);
            panel9.Visible = true;
            /* panel9.Controls.Clear();
             panel9.Visible = true;
             panel9.Size = new Size(212, 167);

             // Create a new TableLayoutPanel
             TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
             {
                 ColumnCount = 1,
                 RowCount = 3, // Adjusted to 3 rows
                 Dock = DockStyle.Fill,
                 AutoSize = false
             };

             // Add rows to the TableLayoutPanel
             tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // First row for the label
             tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33F)); // Second row for buttons
             tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Third row for single button

             // Create and add the label to the first row
             Label bulletsLabel = new Label
             {
                 Text = "Bullets",
                 AutoSize = true,
                 TextAlign = ContentAlignment.MiddleCenter,
                 Dock = DockStyle.Fill
             };
             tableLayoutPanel.Controls.Add(bulletsLabel, 0, 0);

             // Define the file paths for each image
             string[] images = new string[]
             {
         "D:\\UI\\Minutes of Meeting\\MOMC_PROJECT\\Resources\\icons8-done-26.png",
         "D:\\UI\\Minutes of Meeting\\MOMC_PROJECT\\Resources\\icons8-filled-circle-24.png",
         "D:\\UI\\Minutes of Meeting\\MOMC_PROJECT\\Resources\\icons8-square-26.png"
             };

             // Create and add buttons with images to the second row
             for (int i = 0; i < 2; i++)
             {
                 FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                 {
                     Dock = DockStyle.Fill,
                     FlowDirection = FlowDirection.LeftToRight,
                     AutoSize = false
                 };

                 for (int j = 0; j < 3; j++) // Adjusted loop to iterate through all images
                 {
                     Button button = new Button
                     {
                         Size = new Size(60, 30), // Set the size of each button
                         TextAlign = ContentAlignment.MiddleCenter,
                         FlatStyle = FlatStyle.Flat // Make the button flat for better image display
                     };

                     // Calculate the correct index in the images array
                     int index = (i * 3) + j;
                     if (index < images.Length)
                     {
                         button.Image = Image.FromFile(images[index]); // Load image from file path and set it as button's image
                     }

                     // Handle the click event for each button
                     button.Click += (btnSender, btnEvent) =>
                     {
                         selectedButton = (Button)btnSender;
                     };

                     buttonPanel.Controls.Add(button);
                 }

                 tableLayoutPanel.Controls.Add(buttonPanel, 0, i + 1);
             }

             // Create and add a single button to the third row
             Button singleButton = new Button
             {
                 Size = new Size(60, 30), // Set the size of the button
                 TextAlign = ContentAlignment.MiddleCenter,
                 FlatStyle = FlatStyle.Flat // Make the button flat for better image display
             };

             // Handle the click event for the single button
             singleButton.Click += (btnSender, btnEvent) =>
             {
                 if (selectedButton != null)
                 {
                     // Append the image of the selected button to richTextBox3
                     richTextBox3.AppendText(Environment.NewLine);
                     richTextBox3.SelectionStart = richTextBox3.TextLength;
                     // Convert the image to a bitmap and place it on the clipboard
                     Clipboard.SetImage((Bitmap)selectedButton.Image);

                     // Paste the image from the clipboard into the rich text box
                     richTextBox3.Paste();
                 }
             };

             tableLayoutPanel.Controls.Add(singleButton, 0, 2);

             // Add the TableLayoutPanel to panel9
             panel9.Controls.Add(tableLayoutPanel);*/
        }
        private List<Panel> buttonPanels = new List<Panel>();
        private void button15_Click(object sender, EventArgs e)
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
                int maxButtonsPerRow = (panel4.Width) / (buttonWidth + fileButtonGap);
                int fileCount = 0;
                panel4.Controls.Clear();
                buttonPanels.Clear();
                panel4.AutoScroll = true;
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
                    iconPictureBox.Image = System.Drawing.Icon.ExtractAssociatedIcon(filePath).ToBitmap();
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
                        panel4.Controls.Remove(buttonPanel);
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
                    panel4.Controls.Add(buttonPanel);
                    buttonPanels.Add(buttonPanel);
                    fileCount++;
                }
            }

        }
        private void button16_Click(object sender, EventArgs e)
        {
            panel10.Visible = true;
            panel10.Controls.Clear();

            // Initialize DataGridView
            InitializeDataGridView();
        }
        private void btn_undo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(currentText); // Store current state for redo
                currentText = undoStack.Pop(); // Get previous state from undo stack
                richTextBox3.Text = currentText; // Update RichTextBox
                UpdateUndoRedoButtons();
            }
        }

        private void btn_redo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(currentText); // Store current state for undo
                currentText = redoStack.Pop(); // Get next state from redo stack
                richTextBox3.Text = currentText; // Update RichTextBox
                UpdateUndoRedoButtons();
            }
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (currentText != richTextBox3.Text)
            {
                undoStack.Push(currentText); // Store previous state for undo
                currentText = richTextBox3.Text; // Update current text
                redoStack.Clear(); // Clear redo stack
                UpdateUndoRedoButtons();
            }
        }
        private void UpdateUndoRedoButtons()
        {
            btn_undo.Enabled = undoStack.Count > 0;
            btn_redo.Enabled = redoStack.Count > 0;
        }

        private void jd(object sender, PaintEventArgs e)
        {

        }

        private void MeetingsInfo_ComposeEmail_Load_2(object sender, EventArgs e)
        {
            panel10.Visible = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* contextMenuStrip1.Visible = true;*/
            contextMenuStrip1.Show(panel12, new Point(0, panel12.Height));

        }

        private void deleteColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
                dataGridView1.Columns.RemoveAt(columnIndex);
            }
        }

        private void deleteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Delete the selected row
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows.RemoveAt(rowIndex);
            }
        }
        private void deleteTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            panel10.Visible = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            /* contextMenuStrip2.Visible = true;*/
            contextMenuStrip2.Show(panel12, new Point(0, panel12.Height));
        }
        private void insertAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Insert a new row above the selected row
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows.Insert(rowIndex, 1);
            }
        }

        private void insertBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the last row is an uncommitted new row
            if (dataGridView1.AllowUserToAddRows && dataGridView1.NewRowIndex != -1)
            {
                // Save the uncommitted new row index
                int newRowIndex = dataGridView1.NewRowIndex;

                // End edit to commit the changes
                dataGridView1.EndEdit();

                // Insert a new row at the end
                dataGridView1.Rows.Add();

                // Restore the uncommitted new row
                dataGridView1.Rows[newRowIndex].Visible = false;
                dataGridView1.Rows[newRowIndex].Visible = true;
            }
            else
            {
                // Insert a new row below the selected row
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    dataGridView1.Rows.Insert(rowIndex + 1, 1);
                }
            }
        }

        private void insertRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedCells[0].ColumnIndex != dataGridView1.Columns.Count - 1)
            {
                int columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
                dataGridView1.Columns.Insert(columnIndex + 1, new DataGridViewTextBoxColumn());
            }
        }

        private void insertLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a column is selected
            if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedCells[0].ColumnIndex != 0)
            {
                int columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
                dataGridView1.Columns.Insert(columnIndex, new DataGridViewTextBoxColumn());
            }
        }

        private void richTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the Enter key is pressed
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Get the current line index
                int selectionStart = richTextBox3.SelectionStart;
                int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);

                // Check if the line index is valid
                if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
                {
                    int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
                    int lineLength = richTextBox3.Lines[lineIndex].Length;

                    // Get the previous line text
                    string previousLineText = "";
                    if (lineIndex > 0)
                    {
                        previousLineText = richTextBox3.Lines[lineIndex - 1];
                    }

                    // Check if the previous line already has a bullet point
                    if (previousLineText.TrimStart().StartsWith(previousBullet))
                    {
                        // Insert the bullet point at the beginning of the new line
                        richTextBox3.Select(lineStart, 0);
                        richTextBox3.SelectedText = previousBullet + " ";
                    }
                }
            }
        }
    }
}
