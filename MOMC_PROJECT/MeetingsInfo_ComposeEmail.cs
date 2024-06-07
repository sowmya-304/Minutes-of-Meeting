using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MOMC_PROJECT.MOM_Prop;

namespace MOMC_PROJECT
{
    public partial class MeetingsInfo_ComposeEmail : UserControl
    {
        public Panel Panel5DW
        {
            get { return panel5DW; }
        }

        private List<MeetingData> meetingDataList;

        public static List<string> attachments = new List<string>();
        public static List<string> drawboardAttachments = new List<string>();
        public List<string> Attachments { get; set; }

        public static string selected_meeting_name = "";
        private ComboBox comboBox1;
        private ComboBox comboBox3;
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();
        private string currentText = "";
        private DataGridView dataGridView1;
        private List<Panel> buttonPanels = new List<Panel>();

        public MeetingsInfo_ComposeEmail()
        {
            InitializeComponent();

            currentText = richTextBox3.Text;
            OnLoad();
            PopulateMeetingsComboBox();
            AttachMouseDownEventHandler(this);
            InitializeBulletButtons();
            IntializeTableLayoutPanel();

        }
        private void InitializeBulletButtons()
        {
            try
            {
                var bulletButtons = new (string Symbol, Point Position)[]
                {
                  ("●", new Point(0, 0)),
                  ("◆", new Point(60, 0)),
                  ("☐", new Point(0, 60)),
                  ("★", new Point(60, 60)),
                  ("✔", new Point(120, 0)),
                  ("➤", new Point(120, 60))
                };
                foreach (var (symbol, position) in bulletButtons)
                {
                    AddBulletButton(symbol, position);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void AddBulletButton(string bulletChar, Point location)
        {
            try
            {
                int maxWidth = panel5.ClientSize.Width / 6;
                int maxHeight = panel5.ClientSize.Height;
                int buttonSize = Math.Min(maxWidth, maxHeight);
                Button button = new Button
                {
                    Size = new Size(buttonSize, buttonSize),
                    Location = location,
                    Text = bulletChar.ToString(),
                    TextAlign = ContentAlignment.TopCenter,
                };
                button.Font = new Font(button.Font.FontFamily, buttonSize / 3, button.Font.Style);
                button.Click += btn_mail_bullets_Click;
                panel5.Controls.Add(button);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private string previousBullet = " ";
        /*  private void ApplyBulletToSelectedLine(string bulletChar)
          {
              try
              {
                  if (string.IsNullOrEmpty(richTextBox3.Text))
                  {
                      return; // Do nothing if the RichTextBox is empty
                  }
                  int selectionStart = richTextBox3.SelectionStart;
                  int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);
                  // Check if the line index is valid
                  if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
                  {
                      int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
                      string lineText = richTextBox3.Lines[lineIndex];
                      // Store the original font size
                      float originalFontSize = richTextBox3.SelectionFont.Size;

                      // Define a fixed font size for the bullet
                      float bulletFontSize = 8; // You can adjust this value

                      // Create a font for the bullet
                      Font bulletFont = new Font(richTextBox3.SelectionFont.FontFamily, bulletFontSize);

                      // Check if the current line already has a bullet point and spaces
                      if (lineText.StartsWith(bulletChar + "   "))
                      {
                          // The line already has the correct bullet and spaces, do nothing
                          return;
                      }
                      else if (lineText.StartsWith(previousBullet + "   "))
                      {
                          // Replace the existing bullet point with the new one
                          richTextBox3.Select(lineStart, previousBullet.Length + 3);
                          richTextBox3.SelectionFont = bulletFont;
                          richTextBox3.SelectedText = bulletChar + "   ";
                      }
                      else if (lineText.StartsWith(previousBullet))
                      {
                          // Replace the existing bullet point with the new one
                          richTextBox3.Select(lineStart, previousBullet.Length);
                          richTextBox3.SelectionFont = bulletFont;
                          richTextBox3.SelectedText = bulletChar + "   ";
                      }
                      else
                      {
                          // Insert the new bullet point at the beginning of the line
                          richTextBox3.Select(lineStart, 0);
                          richTextBox3.SelectionFont = bulletFont;
                          richTextBox3.SelectedText = bulletChar + "   " + lineText;

                          // Remove the original text after inserting bullet and spaces to avoid duplication
                          richTextBox3.Select(lineStart + bulletChar.Length + 3, lineText.Length);
                          richTextBox3.SelectedText = string.Empty;
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
                              richTextBox3.SelectionFont = bulletFont;
                              richTextBox3.SelectedText = bulletChar + "   ";
                          }
                          else
                          {
                              break; // Stop applying bullet points if the next line doesn't match
                          }
                      }
                      // Restore selection to a position after the text on the current line
                      richTextBox3.Select(lineStart + bulletChar.Length + 3 + lineText.Length, 0);
                      richTextBox3.SelectionFont = new Font(richTextBox3.SelectionFont.FontFamily, originalFontSize);
                      richTextBox3.Focus();
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.Message);
              }
          }*/
        private void ApplyBulletToSelectedLine(string bulletChar)
        {
            try
            {
                if (string.IsNullOrEmpty(richTextBox3.Text))
                {
                    return; // Do nothing if the RichTextBox is empty
                }

                // Get the start and end character indices of the selection
                int selectionStart = richTextBox3.SelectionStart;
                int selectionEnd = selectionStart + richTextBox3.SelectionLength;

                // Get the line indices for the start and end of the selection
                int startLineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);
                int endLineIndex = richTextBox3.GetLineFromCharIndex(selectionEnd);

                // Store the original font size, use a default font if SelectionFont is null
                float originalFontSize = richTextBox3.SelectionFont?.Size ?? 10f;
                FontFamily fontFamily = richTextBox3.SelectionFont?.FontFamily ?? new FontFamily("Arial");

                // Define a fixed font size for the bullet
                float bulletFontSize = 8; // You can adjust this value

                // Create a font for the bullet
                Font bulletFont = new Font(fontFamily, bulletFontSize);

                // Loop through each line within the selected range
                for (int lineIndex = startLineIndex; lineIndex <= endLineIndex; lineIndex++)
                {
                    // Check if the line index is valid
                    if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
                    {
                        int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
                        string lineText = richTextBox3.Lines[lineIndex];

                        // Check if the current line already has a bullet point and spaces
                        if (lineText.StartsWith(bulletChar + "   "))
                        {
                            // The line already has the correct bullet and spaces, do nothing
                            continue;
                        }
                        else if (lineText.StartsWith(previousBullet + "   "))
                        {
                            // Replace the existing bullet point with the new one
                            richTextBox3.Select(lineStart, previousBullet.Length + 3);
                            richTextBox3.SelectionFont = bulletFont;
                            richTextBox3.SelectedText = bulletChar + "   ";
                        }
                        else if (lineText.StartsWith(previousBullet))
                        {
                            // Replace the existing bullet point with the new one
                            richTextBox3.Select(lineStart, previousBullet.Length);
                            richTextBox3.SelectionFont = bulletFont;
                            richTextBox3.SelectedText = bulletChar + "   ";
                        }
                        else
                        {
                            // Insert the new bullet point at the beginning of the line
                            richTextBox3.Select(lineStart, 0);
                            richTextBox3.SelectionFont = bulletFont;
                            richTextBox3.SelectedText = bulletChar + "   " + lineText;

                            // Remove the original text after inserting bullet and spaces to avoid duplication
                            richTextBox3.Select(lineStart + bulletChar.Length + 3, lineText.Length);
                            richTextBox3.SelectedText = string.Empty;
                        }
                    }
                }
                // Store the current bullet character as the previous bullet character
                previousBullet = bulletChar;

                // Restore selection to the original range
                richTextBox3.Select(selectionStart, selectionEnd - selectionStart);
                richTextBox3.SelectionFont = new Font(fontFamily, originalFontSize);
                richTextBox3.Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void InitializeDataGridView()
        {
            try
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
                dataGridView1.MultiSelect = false;
                dataGridView1.AllowUserToAddRows = false;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void OnLoad()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            // Define the path to the JSON file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Data.json");
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
            if (meeting_names.Count > 0)
            {
                cb_emailmeetings.Items.AddRange(meeting_names.ToArray());
            }
            else
            {
                label15.Text = "There are no meetings created using logged in email address";
                label15.ForeColor = Color.Red;
                panel2.Visible = false;
                panel3.Visible = false;
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.Enabled = false;
                Draw d = new Draw();
                DrawBoard drawBoard = new DrawBoard();
                d.ShowDialog();
                this.Enabled = true;
                if (AttachmentStore.Attachments != null)
                {
                    panel5DW.Controls.Clear();
                    int buttonWidth = 200;
                    int buttonHeight = 50;
                    int fileButtonGap = 20;
                    int removeButtonGap = 0;
                    int maxButtonsPerRow = panel5DW.Width / (buttonWidth + fileButtonGap);
                    int fileCount = AttachmentStore.Attachments.Count;
                    foreach (string filePath in AttachmentStore.Attachments)
                    {
                        AddFileButton1(filePath, fileCount, buttonWidth, buttonHeight, fileButtonGap, removeButtonGap, maxButtonsPerRow);
                    }
                }
                //  panel13.Controls.Clear();
                // DrawBoard db = new DrawBoard();
                /* panel13.Dock = DockStyle.Fill;
                 panel13.Controls.Add(db);*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void cb_emailmeetings_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btn_mail_insert.Enabled = true;
                linkLabel2.Enabled = true;
                button3.Enabled = true;
                btn_sendmail.Enabled = true;
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
                        richTextBox1.Text = null;
                        foreach (var attendeeEmail in selectedMeeting.AttendeeEmail)
                        {
                            richTextBox1.Text += attendeeEmail + Environment.NewLine;
                        }
                        richTextBox2.Text = $"Minutes of Meeting: {textBox1.Text} - {textBox3.Text}";
                        richTextBox3.Text = $"Minutes of Meeting Mail: {textBox1.Text} " + "\n" +
                                            $"Meeting Start Time: {textBox2.Text}" + "\n" +
                                            $"Meeting End Time: {textBox3.Text}" + "\n" +
                                            $"Meeting Duration: {selectedMeeting.EndDateTime - selectedMeeting.StartDateTime}" + "\n";
                        clb_attendees.Items.Clear();
                        foreach (var attendee in selectedMeeting.AttendeeEmail)
                        {
                            clb_attendees.Items.Add(attendee, true);
                        }
                        panel4.Controls.Clear();
                        //    int selectedIndex = cb_emailmeetings.SelectedIndex;
                        // loadDescription();
                        undoStack.Clear();
                        redoStack.Clear();
                    }
                    if (selectedMeetingName != null &&textBox1.Text!=null)
                    {
                        LoadDataFromStructure(selectedMeetingName);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            //   SaveDataToStructure();

        }
        public void loadDescription()
        {
            string subject = richTextBox2.Text;
            if (!string.IsNullOrWhiteSpace(subject) && emailData.ContainsKey(subject))
            {
                richTextBox3.Rtf = emailData[subject]; // Load description from dictionary
            }
            else
            {
                richTextBox3.Clear();
            }
        }
        // 

        private void clb_attendees_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Only proceed if trying to uncheck an item
            if (e.NewValue == CheckState.Unchecked)
            {
                // Check if at least one item is checked, excluding the item being unchecked
                int checkedItemsCount = clb_attendees.CheckedItems.Count;

                // Include the current item being unchecked in the count if it's already checked
                if (clb_attendees.GetItemCheckState(e.Index) == CheckState.Checked)
                {
                    checkedItemsCount--;
                }

                if (checkedItemsCount <= 0) // This means no items will be checked after unchecking this one
                {
                    label17.Visible = true;
                    label17.Text = "At least one item should be checked";
                    label17.ForeColor = Color.Red;

                    // Start the timer to hide the label after 8 seconds
                    hideLabelTimer.Start();

                    e.NewValue = CheckState.Checked; // Cancel the uncheck action
                }
            }
        }
        private void HideLabelTimer_Tick(object sender, EventArgs e)
        {
            // Hide the label and stop the timer
            label17.Visible = false;
            hideLabelTimer.Stop();
        }
        private void clb_attendees_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    string selectedMeetingName = cb_emailmeetings.SelectedItem?.ToString();

            //    if (selectedMeetingName != null && meetingDataList != null)
            //    {
            //        var selectedMeeting = meetingDataList
            //            .SelectMany(md => md.Meetings)
            //            .FirstOrDefault(m => m.Name == selectedMeetingName);

            //        if (selectedMeeting != null)
            //        {
            //            richTextBox1.Clear(); // Clear existing content

            //            // Iterate through all items in the checked list box
            //            foreach (var itemIndex in clb_attendees.CheckedIndices)
            //            {
            //                int index = (int)itemIndex;
            //                richTextBox1.AppendText(selectedMeeting.AttendeeEmail[index] + Environment.NewLine);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private string RemoveSpecialCharacters(string str)
        {
            // Replace special characters with empty string
             return Regex.Replace(str, @"[^w.@]", " ");
            //return Regex.Replace(str, @"[@]", " ");
        }
        private void btn_sendmail_Click(object sender, EventArgs e)
        {
            try
            {
                label13.Visible = true;
                label13.Text = "Sending...";
                label13.Refresh();

                // Load JSON data from file
                string jsonFilePath = "D:\\UI\\Minutes of Meeting\\MOMC_PROJECT\\Data.json";
                string jsonText = File.ReadAllText(jsonFilePath);
                JArray data = JArray.Parse(jsonText);
                Console.WriteLine(data.ToString());
                // Get sender's email
                string senderEmail = MOMC.toEmail;

                // Find sender's password based on email
                string senderPassword = GetSenderPassword(data, senderEmail);

                // Proceed only if sender password is found
                if (!string.IsNullOrEmpty(senderPassword))
                {
                    MailMessage message = new MailMessage();
                    message.IsBodyHtml = true;

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    message.From = new MailAddress(senderEmail);

                    // Add recipients from richTextBox1
                    foreach (string email in richTextBox1.Lines)
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            message.To.Add(email);
                        }
                    }
                      message.Subject = richTextBox2.Text;
                    //string subject = richTextBox2.Text;
                   /* subject = RemoveSpecialCharacters(subject);
                    subject = subject.Substring(0, Math.Min(subject.Length, 255));
                    message.Subject = subject;*/
                    StringBuilder body = new StringBuilder();
                    // Convert DataGridView content to HTML table
                    if (dataGridView1 != null)
                    {
                        body.AppendLine("<table border='1'>");
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            body.AppendLine("<tr>");
                            int cellCount = row.Cells.Count;
                            for (int i = 0; i < cellCount; i++) // Iterate until the second-to-last cell
                            {
                                DataGridViewCell cell = row.Cells[i];
                                if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString())) // Check if the cell value is not null or empty
                                {
                                    body.AppendLine("<td>" + cell.Value.ToString() + "</td>");
                                }
                                else
                                {
                                    label13.Text = "Please fill in all cells in table before sending the email.";
                                    label13.ForeColor = Color.Red;
                                    //  MessageBox.Show("One or more cells contain empty values. Please fill in all cells before sending the email.");
                                    return; // Return from the method if an empty cell is found
                                }
                            }

                            body.AppendLine("</tr>");
                        }

                        body.AppendLine("</table>");
                    }
                    // Append the content of richTextBox3 to the body
                    string rtfText = richTextBox3.Rtf;
                    string plainText = ConvertRtfToPlainText(rtfText);
                    body.AppendLine(plainText);

                    //body.AppendLine(richTextBox3.Rtf);
                    message.Body = body.ToString();
                    // Attach files
                    if (attachments != null)
                    {
                        foreach (string filePath in attachments)
                        {
                            if (File.Exists(filePath))
                            {
                                message.Attachments.Add(new Attachment(filePath));
                            }
                        }
                    }
                    if (AttachmentStore.Attachments != null)
                    {
                        foreach (string f in AttachmentStore.Attachments)
                        {
                            if (File.Exists(f))
                            {
                                message.Attachments.Add(new Attachment(f));
                            }
                        }
                    }
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.Send(message);
                    label13.Text = "Sent";
                    label13.ForeColor = Color.Green;
                }
                else
                {
                    label13.Text = "Failed to send email. Sender password not found.";
                    label13.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace + ex.Message);
                label13.Text = "Failed to send email. Check your Internet Connection.";
                label13.ForeColor = Color.Red;
            }
        }
        public static string ConvertRtfToPlainText(string rtfText)
        {
            if (string.IsNullOrEmpty(rtfText))
                return string.Empty;

            using (System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox())
            {
                rtBox.Rtf = rtfText;
                return rtBox.Text;
            }
        }

        private string GetSenderPassword(JArray data, string senderEmail)
        {
            foreach (JObject obj in data)
            {
                if (obj["Email"].ToString() == senderEmail)
                {
                    return obj["PassKey"].ToString().Trim();
                }
            }
            //  Console.WriteLine("Sender Email: " + senderEmail);
            // Console.WriteLine("JSON Data: " + data.ToString());

            return null;
        }
        private void btn_mail_format_Click(object sender, EventArgs e)
        {
            try
            {
                panel7.Visible = true;
                panel8.Visible = false;
                panel11.Visible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void btn_mail_insert_Click(object sender, EventArgs e)
        {
            try
            {
                if (panel10.Visible)
                {
                    panel11.Visible = true;
                }
                else
                {
                    panel11.Visible = false;
                }
                if (dataGridView1 != null)
                {
                    button16.Enabled = false;
                }
                else
                {
                    button16.Enabled = true;
                }
                panel7.Visible = false;
                //  panel11.Visible = false;
                panel8.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private bool ContainsSpecialCharacters(string text)
        {
            // Define the special characters
            string specialCharacters = "●◆☐★✔➤";
            // Check if any of the special characters exist in the text
            foreach (char c in text)
            {
                if (specialCharacters.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
        private void btn_mail_bold_Click(object sender, EventArgs e)
        {
            try
            {
                int selectionStart = richTextBox3.SelectionStart;
                int selectionLength = richTextBox3.SelectionLength;
                bool isSelected = selectionLength > 0;

                if (isSelected)
                {
                    // Check if selected text contains special characters
                    string selectedText = richTextBox3.SelectedText;
                    bool containsSpecialCharacters = ContainsSpecialCharacters(selectedText);

                    if (!containsSpecialCharacters)
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

                        // Preserve font size and font family
                        float fontSize = Convert.ToSingle(comboBox1.Text);
                        string fontFamily = comboBox3.Text;
                        Font newFont = new Font(fontFamily, fontSize, newFontStyle);
                        richTextBox3.SelectionFont = newFont;
                    }
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

                    // Preserve font size and font family
                    float fontSize = Convert.ToSingle(comboBox1.Text);
                    string fontFamily = comboBox3.Text;
                    Font newFont = new Font(fontFamily, fontSize, existingStyle);

                    // Apply bold or unbold to entire content, excluding special characters
                    richTextBox3.SelectAll();
                    foreach (char c in richTextBox3.Text)
                    {
                        if (!ContainsSpecialCharacters(c.ToString()))
                        {
                            richTextBox3.Select(richTextBox3.Text.IndexOf(c), 1);
                            richTextBox3.SelectionFont = newFont;
                        }
                    }
                    richTextBox3.Select(selectionStart, selectionLength);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btn_mail_italic_Click(object sender, EventArgs e)
        {
            try
            {
                int selectionStart = richTextBox3.SelectionStart;
                int selectionLength = richTextBox3.SelectionLength;
                bool isSelected = selectionLength > 0;

                if (isSelected)
                {
                    // Check if selected text contains special characters
                    string selectedText = richTextBox3.SelectedText;
                    bool containsSpecialCharacters = ContainsSpecialCharacters(selectedText);

                    if (!containsSpecialCharacters)
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

                        float fontSize = Convert.ToSingle(comboBox1.Text);
                        string fontFamily = comboBox3.Text;
                        Font newFont = new Font(fontFamily, fontSize, newFontStyle);
                        richTextBox3.SelectionFont = newFont;
                    }
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

                    float fontSize = Convert.ToSingle(comboBox1.Text);
                    string fontFamily = comboBox3.Text;
                    Font newFont = new Font(fontFamily, fontSize, existingStyle);

                    // Apply italic or unitalic to entire content, excluding special characters
                    richTextBox3.SelectAll();
                    foreach (char c in richTextBox3.Text)
                    {
                        if (!ContainsSpecialCharacters(c.ToString()))
                        {
                            richTextBox3.Select(richTextBox3.Text.IndexOf(c), 1);
                            richTextBox3.SelectionFont = newFont;
                        }
                    }
                    richTextBox3.Select(selectionStart, selectionLength);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void btn_mail_underline_Click(object sender, EventArgs e)
        {
            try
            {
                int selectionStart = richTextBox3.SelectionStart;
                int selectionLength = richTextBox3.SelectionLength;
                bool isSelected = selectionLength > 0;

                if (isSelected)
                {
                    // Check if selected text contains special characters
                    string selectedText = richTextBox3.SelectedText;
                    bool containsSpecialCharacters = ContainsSpecialCharacters(selectedText);

                    if (!containsSpecialCharacters)
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

                        float fontSize = Convert.ToSingle(comboBox1.Text);
                        string fontFamily = comboBox3.Text;
                        Font newFont = new Font(fontFamily, fontSize, newFontStyle);

                        richTextBox3.SelectionFont = newFont;
                    }
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

                    float fontSize = Convert.ToSingle(comboBox1.Text);
                    string fontFamily = comboBox3.Text;
                    Font newFont = new Font(fontFamily, fontSize, existingStyle);

                    // Apply underline or ununderline to entire content, excluding special characters
                    richTextBox3.SelectAll();
                    foreach (char c in richTextBox3.Text)
                    {
                        if (!ContainsSpecialCharacters(c.ToString()))
                        {
                            richTextBox3.Select(richTextBox3.Text.IndexOf(c), 1);
                            richTextBox3.SelectionFont = newFont;
                        }
                    }
                    richTextBox3.Select(selectionStart, selectionLength);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void IntializeTableLayoutPanel()
        {
            try
            {
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
                List<int> fontSizes = new List<int>();

                for (int i = 8; i <= 72; i++)
                {
                    if (i % 2 == 0)
                    {
                        fontSizes.Add(i);
                    }
                }

                int[] fontSizesArray = fontSizes.ToArray();

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
                button1.Click += (s, args) => ToggleStrikeout();
                button2.Click += (s, args) => ToggleSubscript();
                button3.Click += (s, args) => ToggleSuperscript();
                // Fourth, fifth, and sixth rows each with a single button
                Button button4 = new Button { Text = "Highlight", Dock = DockStyle.Fill };
                Button button5 = new Button { Text = "Font Color", Dock = DockStyle.Fill };
                Button button6 = new Button { Text = "Clear All Formatting", Dock = DockStyle.Fill };
                button4.Click += (s, args) => panel9.Visible = false;
                button5.Click += (s, args) => panel9.Visible = false;
                button6.Click += (s, args) => panel9.Visible = false;
                button4.Click += (s, args) => HighlightText();
                button5.Click += (s, args) => ChangeFontColor();
                button6.Click += (s, args) => ClearAllFormatting();
                tableLayoutPanel.Controls.Add(button4, 0, 3);
                tableLayoutPanel.SetColumnSpan(button4, 3); // Span across all three columns
                tableLayoutPanel.Controls.Add(button5, 0, 4);
                tableLayoutPanel.SetColumnSpan(button5, 3); // Span across all three columns
                tableLayoutPanel.Controls.Add(button6, 0, 5);
                tableLayoutPanel.SetColumnSpan(button6, 3); // Span across all three columns              
                // Add the TableLayoutPanel to panel9
                panel9.Controls.Add(tableLayoutPanel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void btn_mail_ff_Click(object sender, EventArgs e)
        {
            // panel9.Controls.Clear();
            // IntializeTableLayoutPanel();
            panel5.Visible = false;
            panel9.Visible = true;
        }
        private void HighlightText()
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox3.SelectionBackColor = colorDialog.Color;
            }
        }
        private void ChangeFontColor()
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox3.SelectionColor = colorDialog.Color;
            }
        }
        private void ClearAllFormatting()
        {
            if (richTextBox3.SelectionLength > 0)
            {
                int start = richTextBox3.SelectionStart;
                int length = richTextBox3.SelectionLength;
                string selectedText = richTextBox3.SelectedText;

                richTextBox3.SelectionFont = richTextBox3.Font;
                richTextBox3.SelectionColor = richTextBox3.ForeColor;
                richTextBox3.SelectionBackColor = richTextBox3.BackColor;

                richTextBox3.SelectedText = selectedText;
                richTextBox3.Select(start, length);
            }
        }
        private void ToggleSuperscript()
        {
            // Apply superscript (set character offset to a small positive value)
            ChangeSelectionOffset(richTextBox3, 5);
        }

        private void ToggleSubscript()
        {
            // Apply subscript (set character offset to a small negative value)
            ChangeSelectionOffset(richTextBox3, -5);
        }
        private void ChangeSelectionOffset(RichTextBox rtb, int offset)
        {
            // Toggle the current offset if necessary
            int currentOffset = rtb.SelectionCharOffset;
            rtb.SelectionCharOffset = (currentOffset == offset) ? 0 : offset;
        }
        private void ToggleStrikeout()
        {
            try
            {
                if (richTextBox3.SelectionFont != null)
                {
                    Font currentFont = richTextBox3.SelectionFont;
                    FontStyle newFontStyle;

                    if (richTextBox3.SelectionFont.Strikeout)
                    {
                        // If the current text is already striked out, remove the strikeout style
                        newFontStyle = currentFont.Style & ~FontStyle.Strikeout;
                    }
                    else
                    {
                        // If the current text is not striked out, add the strikeout style
                        newFontStyle = currentFont.Style | FontStyle.Strikeout;
                    }
                    richTextBox3.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    int newSize = Convert.ToInt32(comboBox1.SelectedItem);
                    comboBox1.Text = newSize.ToString();
                    ApplyFontSize(newSize);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /* private void ApplyFontSize(int newSize)
         {
             try
             {
                 int selectionStart = richTextBox3.SelectionStart;
                 int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);
                 int charIndexFromLine = richTextBox3.GetFirstCharIndexFromLine(lineIndex);


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
                     // Apply font size to text from the current line onwards
                     int lengthToEnd = richTextBox3.TextLength - charIndexFromLine;
                     richTextBox3.Select(charIndexFromLine, lengthToEnd);

                     Font newFont = new Font(richTextBox3.SelectionFont.FontFamily, newSize, existingStyle);
                     richTextBox3.SelectionFont = newFont;

                     // Restore the selection and cursor position
                     richTextBox3.Select(selectionStart, 0);
                     richTextBox3.ScrollToCaret();

             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
         }*/
        private void ApplyFontSize(int newSize)
        {
            try
            {
                int selectionStart = richTextBox3.SelectionStart;
                int selectionLength = richTextBox3.SelectionLength;

                // Get the selected text
                string selectedText = richTextBox3.SelectedText;

                // Get the font style of the current selection
                FontStyle existingStyle = GetSelectionFontStyle();

                // Apply font size to the selected text excluding special characters
                richTextBox3.Select(selectionStart, selectionLength);

                Font currentFont = richTextBox3.SelectionFont;
                if (currentFont != null)
                {
                    // Create a new font with the updated size
                    Font newFont = new Font(currentFont.FontFamily, newSize, existingStyle);

                    // Apply the new font to the selected text
                    richTextBox3.SelectionFont = newFont;
                }

                // Restore the selection and cursor position
                richTextBox3.Select(selectionStart, selectionLength);
                richTextBox3.ScrollToCaret();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private FontStyle GetSelectionFontStyle()
        {
            FontStyle fontStyle = FontStyle.Regular;

            if (richTextBox3.SelectionFont != null)
            {
                if (richTextBox3.SelectionFont.Bold)
                {
                    fontStyle |= FontStyle.Bold;
                }
                if (richTextBox3.SelectionFont.Italic)
                {
                    fontStyle |= FontStyle.Italic;
                }
                if (richTextBox3.SelectionFont.Underline)
                {
                    fontStyle |= FontStyle.Underline;
                }
            }

            return fontStyle;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedItem != null)
                {
                    string newFontFamily = comboBox3.SelectedItem.ToString();
                    //string newFontFamily = comboBox3.Text.ToString();
                    ApplyFontFamily(newFontFamily);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ApplyFontFamily(string newFontFamily)
        {
            try
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
                    FontFamily currentFontFamily = richTextBox3.SelectionFont.FontFamily;
                    float size = richTextBox3.SelectionFont.Size;
                    Font newFont = new Font(currentFontFamily, size, existingStyle);
                    richTextBox3.SelectionFont = newFont;
                }
                else
                {
                    FontFamily currentFontFamily = richTextBox3.SelectionFont.FontFamily;
                    float size = richTextBox3.SelectionFont.Size;
                    Font newFont = new Font(currentFontFamily, size, existingStyle);
                    richTextBox3.Font = newFont;
                }
                // Restore the selection and preserve other formatting
                richTextBox3.Select(selectionStart, selectionLength);
                if (richTextBox3.SelectionFont != null)
                {
                    richTextBox3.SelectionFont = new Font(newFontFamily, richTextBox3.SelectionFont.Size, existingStyle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //closes panel9
        private void AttachMouseDownEventHandler(Control parent)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void AnyControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (panel9.Visible)
                {
                    panel9.Visible = false;
                    panel11.Visible = false;
                }
                if (label13.Visible)
                {
                    label13.Visible = false;
                }
                if (panel5.Visible)
                {
                    panel5.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void btn_mail_bullets_Click(object sender, EventArgs e)
        {
            try
            {
                //   panel5.Controls.Clear();
                // InitializeBulletButtons();
                Button button = (Button)sender;
                string bulletChar = button.Text;
                ApplyBulletToSelectedLine(bulletChar);
                // panel9.Visible = false;
                panel5.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void AddControlToPanel5(Control control)
        {
            panel5DW.Controls.Add(control);
        }
        // dont change this 
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "All Files|*.*" // Allow all file types
                                             //  Filter = "JPEG Image|*.jpg|JPEG Image|*.jpeg|PNG Image|*.png"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    long maxTotalSize = 25 * 1024 * 1024; // 25 MB in bytes
                    long currentTotalSize = 0;

                    // Calculate the total size of existing attachments
                    foreach (string filePath in attachments)
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        currentTotalSize += fileInfo.Length;
                    }

                    long newFilesTotalSize = 0;
                    foreach (string selectedFilePath in openFileDialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(selectedFilePath);
                        newFilesTotalSize += fileInfo.Length;
                    }

                    // Check if the total size exceeds the limit
                    if (currentTotalSize + newFilesTotalSize > maxTotalSize)
                    {
                        MessageBox.Show("The total size of the selected files exceeds 25 MB. Please select smaller files.");
                        return;
                    }
                    int buttonWidth = 200;
                    int buttonHeight = 50;
                    int fileButtonGap = 20;
                    int removeButtonGap = 0;
                    int maxButtonsPerRow = panel5DW.Width / (buttonWidth + fileButtonGap);
                    int fileCount = attachments.Count; // Start file count from current list size
                    panel4.Controls.Clear();
                    buttonPanels.Clear();
                    panel4.AutoScroll = true;

                    // Re-add existing attachments
                    foreach (string filePath in attachments)
                    {
                        AddFileButton(filePath, fileCount, buttonWidth, buttonHeight, fileButtonGap, removeButtonGap, maxButtonsPerRow);
                        fileCount++;       
                    }

                    // Add new attachments
                    foreach (string selectedFilePath in openFileDialog.FileNames)
                    {
                        int existingIndex = attachments.IndexOf(selectedFilePath);
                        if (existingIndex != -1)
                        {
                            // If file already exists, replace it
                            attachments[existingIndex] = selectedFilePath;
                            panel4.Controls.Remove(buttonPanels[existingIndex]);
                            buttonPanels[existingIndex] = CreateFileButtonPanel(selectedFilePath, existingIndex, buttonWidth, buttonHeight, fileButtonGap, removeButtonGap, maxButtonsPerRow);
                            panel4.Controls.Add(buttonPanels[existingIndex]);
                        }
                        else
                        {
                            // If file is new, add it
                            attachments.Add(selectedFilePath);
                            AddFileButton(selectedFilePath, fileCount, buttonWidth, buttonHeight, fileButtonGap, removeButtonGap, maxButtonsPerRow);
                            fileCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void AddFileButton(string filePath, int fileCount, int buttonWidth, int buttonHeight, int fileButtonGap, int removeButtonGap, int maxButtonsPerRow)
        {
            try
            {
                int rowIndex = fileCount / maxButtonsPerRow;
                int colIndex = fileCount % maxButtonsPerRow;
                int x = colIndex * (buttonWidth + fileButtonGap);
                int y = rowIndex * (buttonHeight + removeButtonGap);
                Panel buttonPanel = CreateFileButtonPanel(filePath, fileCount, buttonWidth, buttonHeight, fileButtonGap, removeButtonGap, maxButtonsPerRow);

                int newRow = buttonPanels.Count / maxButtonsPerRow;
                int newCol = buttonPanels.Count % maxButtonsPerRow;

                int newX = newCol * (buttonWidth + fileButtonGap);
                int newY = newRow * (buttonHeight + removeButtonGap);

                if ((newCol + 1) * (buttonWidth + fileButtonGap) > panel4.Width)
                {
                    panel4.HorizontalScroll.Value += buttonWidth + fileButtonGap;
                }

                if (newCol > 0)
                {
                    int previousButtonIndex = buttonPanels.Count - 1;
                    int previousButtonX = buttonPanels[previousButtonIndex].Location.X;
                    int previousButtonWidth = buttonPanels[previousButtonIndex].Width;
                    newX = previousButtonX + previousButtonWidth + fileButtonGap;
                }

                buttonPanel.Location = new Point(newX, newY);
                panel4.Controls.Add(buttonPanel);
                buttonPanels.Add(buttonPanel);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       
        private Panel CreateFileButtonPanel(string filePath, int index, int buttonWidth, int buttonHeight, int fileButtonGap, int removeButtonGap, int maxButtonsPerRow)
        {
            Panel buttonPanel = new Panel
            {
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(index % maxButtonsPerRow * (buttonWidth + fileButtonGap), index / maxButtonsPerRow * (buttonHeight + removeButtonGap)),
                BorderStyle = BorderStyle.FixedSingle
            };
            try
            {
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
                    if (indexToRemove >= 0 && indexToRemove < buttonPanels.Count)
                    {
                        buttonPanels.RemoveAt(indexToRemove);
                        attachments.RemoveAt(indexToRemove);
                        panel4.Controls.Remove(buttonPanel);
                        for (int i = indexToRemove; i < buttonPanels.Count; i++)
                        {
                            int row = i / maxButtonsPerRow;
                            int col = i % maxButtonsPerRow;
                            int newX = col * (buttonWidth + fileButtonGap);
                            int newY = row * (buttonHeight + removeButtonGap);
                            buttonPanels[i].Location = new Point(newX, newY);
                        }
                    }
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
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return buttonPanel;
        }
        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1 != null)
                {
                    dataGridView1.Dispose();
                }
                // Check if dataGridView1 is not initialized or has no data
                if (dataGridView1 == null || dataGridView1.Rows.Count == 0)
                {
                    button16.Enabled = false;
                    button7.Visible = true;
                    panel10.Visible = true;
                    panel11.Visible = true;
                    panel10.Controls.Clear();
                    // Initialize DataGridView
                    InitializeDataGridView();
                    string currentText = richTextBox3.Text;

                    // Split the text into lines
                    string[] lines = currentText.Split('\n');
                    for (int i = 0; i < 10; i++)
                    {
                        Array.Resize(ref lines, lines.Length + 1);
                        Array.Copy(lines, 0, lines, 1, lines.Length - 1);
                        lines[0] = "";
                    }
                    string newText = string.Join("\n", lines);
                    // Set the new text as the text of RichTextBox3
                    richTextBox3.Text = newText;
                    richTextBox3.Focus();
                    richTextBox3.SelectionStart = 20;
                    richTextBox3.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void btn_undo_Click(object sender, EventArgs e)
        {
            try
            {
                if (undoStack.Count > 0)
                {
                    redoStack.Push(currentText); // Store current state for redo
                    currentText = undoStack.Pop(); // Get previous state from undo stack
                    richTextBox3.Text = currentText; // Update RichTextBox
                    UpdateUndoRedoButtons();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btn_redo_Click(object sender, EventArgs e)
        {
            try
            {
                if (redoStack.Count > 0)
                {
                    undoStack.Push(currentText); // Store current state for undo
                    currentText = redoStack.Pop(); // Get next state from redo stack
                    richTextBox3.Text = currentText; // Update RichTextBox
                    UpdateUndoRedoButtons();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (currentText != richTextBox3.Text)
                {
                    undoStack.Push(currentText); // Store previous state for undo
                    currentText = richTextBox3.Text; // Update current text
                    redoStack.Clear(); // Clear redo stack
                                       // UpdateUndoRedoButtons();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void UpdateUndoRedoButtons()
        {
            try
            {
                btn_undo.Enabled = undoStack.Count > 0;
                btn_redo.Enabled = redoStack.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void jd(object sender, PaintEventArgs e)
        {

        }
        private System.Windows.Forms.Timer hideLabelTimer;

        private void MeetingsInfo_ComposeEmail_Load_2(object sender, EventArgs e)
        {
            try
            {
                btn_mail_insert.Enabled = false;
                linkLabel2.Enabled = false;
                //  panel13.Visible = false;
                button3.Enabled = false;
                btn_sendmail.Enabled = false;
                panel10.Visible = false;
                panel11.Visible = false;
                label13.Visible = false;
                panel5.Visible = false;
                button7.Visible = false;
                label15.Visible = false;
                label17.Visible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Initialize the timer
            hideLabelTimer = new System.Windows.Forms.Timer();
            hideLabelTimer.Interval = 5000; // 8000 milliseconds = 8 seconds
            hideLabelTimer.Tick += HideLabelTimer_Tick;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                contextMenuStrip1.Show(panel10, new Point(0, panel12.Height));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void deleteColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
                    dataGridView1.Columns.RemoveAt(columnIndex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void deleteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Delete the selected row
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    dataGridView1.Rows.RemoveAt(rowIndex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void deleteTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1 != null)
                {
                    button16.Enabled = true;
                    button7.Visible = false;
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Dispose(); // Dispose the dataGridView1 control to completely destroy it
                    dataGridView1 = null; // Set dataGridView1 to null to indicate it's no longer initialized
                }
                panel10.Visible = false;
                panel11.Visible = false;
                // Get the current text from RichTextBox3
                string currentText = richTextBox3.Text;

                // Split the text into lines
                string[] lines = currentText.Split('\n');

                // Remove the first 10 empty lines
                int count = 0;
                for (int i = 0; i < lines.Length && count < 10; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                    {
                        lines[i] = null; // Mark the line for removal
                        count++;
                    }
                    else
                    {
                        break; // Stop removing lines if a non-empty line is encountered
                    }
                }
                // Join the remaining lines back into a single string
                string newText = string.Join("\n", lines.Where(line => line != null));
                // Set the new text as the text of RichTextBox3
                richTextBox3.Text = newText;
                undoStack.Clear();
                redoStack.Clear();
                dataGridViewData.Clear();
                string s = cb_emailmeetings.Text;
                if (s != null)
                {
                    SaveDataToStructure();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                contextMenuStrip2.Show(panel10, new Point(0, panel12.Height));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void insertAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AllowUserToAddRows = true;
                // Insert a new row above the selected row
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    dataGridView1.Rows.Insert(rowIndex, 1);
                }
                dataGridView1.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void insertBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AllowUserToAddRows = true;
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
                dataGridView1.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void insertRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedCells[0].ColumnIndex != 0)
                {
                    int columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
                    dataGridView1.Columns.Insert(columnIndex + 1, new DataGridViewTextBoxColumn());

                    // Refresh the DataGridView to reflect the changes
                    dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception appropriately
            }
        }

        private void insertLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a column is selected
                if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedCells[0].ColumnIndex != 0)
                {
                    int columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
                    dataGridView1.Columns.Insert(columnIndex, new DataGridViewTextBoxColumn());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private bool IsTableVisible()
        {
            return dataGridView1.Visible;
        }

        //private void richTextBox3_SelectionChanged(object sender, EventArgs e)
        //{

        //    if (richTextBox3.SelectionFont != null)
        //    {
        //        // string selectedText = richTextBox3.SelectedText;
        //        string selectedText = richTextBox3.Text;
        //        bool isBulletPointSelected = selectedText.Contains("●") || selectedText.Contains("◆") ||
        //                                     selectedText.Contains("☐") || selectedText.Contains("★") ||
        //                                     selectedText.Contains("✔") || selectedText.Contains("➤");
        //        if (!isBulletPointSelected)
        //        {
        //            // Set comboBox1.Text to the font size of the selected text
        //            comboBox1.Text = richTextBox3.SelectionFont.Size.ToString();

        //            // Check if the font family of the selected text is Calibri
        //            if (richTextBox3.SelectionFont.FontFamily.Name == "Calibri")
        //            {
        //                comboBox3.Text = "Calibri";
        //            }
        //            else
        //            {
        //                // Set comboBox3.Text to another value if the font family is not Calibri
        //                comboBox3.Text = richTextBox3.SelectionFont.FontFamily.Name;
        //            }
        //        }
        //    }
        //}

        //private void richTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        //{

        //    try
        //    {
        //        /*if (e.KeyChar == (char)Keys.Enter)
        //        {
        //            // Get the current line index
        //            int selectionStart = richTextBox3.SelectionStart;
        //            int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);
        //            int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
        //            int lineLength = richTextBox3.Lines[lineIndex].Length;
        //            // Check if the line index is valid
        //            if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
        //            {
        //                string previousLineText = "";
        //                if (lineIndex > 0)
        //                {
        //                    previousLineText = richTextBox3.Lines[lineIndex - 1];
        //                }
        //                // Check if the previous line already has a bullet point
        //                if (previousLineText.TrimStart().StartsWith(previousBullet))
        //                {
        //                    // Insert the bullet point at the beginning of the new line
        //                    richTextBox3.Select(lineStart, 0);
        //                    richTextBox3.SelectionFont = new Font(richTextBox3.Font.FontFamily, richTextBox3.Font.Size); // Set font size for bullet point
        //                    richTextBox3.SelectedText = previousBullet + " ";
        //                    richTextBox3.SelectionStart = lineStart + previousBullet.Length + 1; // +1 for the space after the bullet point
        //                    richTextBox3.SelectionLength = 0;
        //                }
        //            }
        //            // Additional validation to remove bullet point if only bullet point exists in the line
        //            if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
        //            {
        //                string currentLineText = richTextBox3.Lines[lineIndex];

        //                // Check if the line contains only the bullet point and no other text
        //                if (currentLineText.Trim() == previousBullet)
        //                {
        //                    // Remove the bullet point from the current line
        //                    richTextBox3.Select(lineStart, lineLength);
        //                    richTextBox3.SelectedText = "";

        //                    // Set previousBullet to null
        //                    //  previousBullet = null;
        //                }
        //            }
        //        }*/
        //        if (e.KeyChar == (char)Keys.Enter)
        //        {
        //            // Get the current line index
        //            int selectionStart = richTextBox3.SelectionStart;
        //            int lineIndex = richTextBox3.GetLineFromCharIndex(selectionStart);
        //            // Check if the line index is valid
        //            if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
        //            {
        //                int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
        //                int lineLength = richTextBox3.Lines[lineIndex].Length;
        //                // Get the previous line text
        //                string previousLineText = "";
        //                if (lineIndex > 0)
        //                {
        //                    previousLineText = richTextBox3.Lines[lineIndex - 1];
        //                }
        //                // Check if the previous line already has a bullet point
        //                if (previousLineText.TrimStart().StartsWith(previousBullet))
        //                {
        //                    // Insert the bullet point at the beginning of the new line
        //                    richTextBox3.Select(lineStart, 0);
        //                    richTextBox3.SelectionFont = new Font(richTextBox3.Font.FontFamily, richTextBox3.Font.Size); // Set font size for bullet point
        //                    richTextBox3.SelectedText = previousBullet + " ";
        //                }
        //                if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
        //                {
        //                    string currentLineText = richTextBox3.Lines[lineIndex];
        //                    if (currentLineText.Trim() == previousBullet)
        //                    {
        //                        // Remove the bullet point from the current line
        //                        richTextBox3.Select(lineStart, lineLength);
        //                        richTextBox3.SelectedText = "";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //}
        private void richTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
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
                            // Set the selection start to after the bullet point
                            richTextBox3.SelectionStart = lineStart + previousBullet.Length + 1;
                            richTextBox3.SelectionLength = 0;
                            // Set the selection font to apply formatting to the text after the bullet point
                            richTextBox3.SelectionFont = new Font(richTextBox3.Font.FontFamily, richTextBox3.Font.Size);
                        }
                        if (lineIndex >= 0 && lineIndex < richTextBox3.Lines.Length)
                        {
                            string currentLineText = richTextBox3.Lines[lineIndex];
                            if (currentLineText.Trim() == previousBullet)
                            {
                                // Remove the bullet point from the current line
                                richTextBox3.Select(lineStart, lineLength);
                                richTextBox3.SelectedText = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void richTextBox3_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int selectionStart = richTextBox3.SelectionStart;
                int selectionEnd = selectionStart + richTextBox3.SelectionLength;
                int adjustedStart = selectionStart;
                int adjustedLength = 0;
                int totalAdjustedLength = 0;

                // Store the original selection length
                int originalSelectionLength = richTextBox3.SelectionLength;

                // Iterate through the selected lines
                for (int i = selectionStart; i < selectionEnd;)
                {
                    int lineIndex = richTextBox3.GetLineFromCharIndex(i);
                    int lineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex);
                    string lineText = richTextBox3.Lines[lineIndex];

                    // Assume the bullet takes the first few characters (adjust based on your bullet)
                    int bulletLength = previousBullet.Length;

                    // Check if the current line contains a bullet
                    if (lineText.TrimStart().StartsWith(previousBullet))
                    {
                        // Adjust the selection start and length to exclude the bullet
                        int actualBulletStart = lineStart + lineText.IndexOf(previousBullet);
                        int actualBulletEnd = actualBulletStart + bulletLength;

                        // Adjust the length to exclude the bullet
                        if (i <= actualBulletEnd)
                        {
                            adjustedStart = actualBulletEnd;
                            totalAdjustedLength += bulletLength;
                        }
                    }

                    // Move to the next line or the end of the current selection
                    int nextLineStart = richTextBox3.GetFirstCharIndexFromLine(lineIndex + 1);
                    if (nextLineStart > i)
                    {
                        i = nextLineStart;
                    }
                    else
                    {
                        break;
                    }
                }

                // Apply the adjusted selection
                richTextBox3.SelectionStart = adjustedStart;
                richTextBox3.SelectionLength = originalSelectionLength - totalAdjustedLength;

                // Ensure selection font is not null and update combo boxes if no bullet is selected
                if (richTextBox3.SelectionFont != null)
                {
                    string selectedText = richTextBox3.SelectedText;
                    bool isBulletPointSelected = selectedText.Contains("●") || selectedText.Contains("◆") ||
                                                 selectedText.Contains("☐") || selectedText.Contains("★") ||
                                                 selectedText.Contains("✔") || selectedText.Contains("➤");
                    if (!isBulletPointSelected)
                    {
                        // Set comboBox1.Text to the font size of the selected text
                        comboBox1.Text = richTextBox3.SelectionFont.Size.ToString();

                        // Check if the font family of the selected text is Calibri
                        if (richTextBox3.SelectionFont.FontFamily.Name == "Calibri")
                        {
                            comboBox3.Text = "Calibri";
                        }
                        else
                        {
                            // Set comboBox3.Text to another value if the font family is not Calibri
                            comboBox3.Text = richTextBox3.SelectionFont.FontFamily.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

  

        private void richTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (panel10.Visible)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        // Get the current cursor position
                        /* int currentPosition = richTextBox3.SelectionStart;
                         // If the cursor is at the starting line, prevent further backspace action
                         if (dataGridView1 != null && currentPosition <= richTextBox3.GetFirstCharIndexFromLine(10))
                         {
                             e.Handled = true;
                             return;
                         }*/
                        int currentPosition = richTextBox3.SelectionStart;
                        // If the cursor is at the starting line, prevent further backspace action
                        if (dataGridView1 != null && currentPosition <= richTextBox3.GetFirstCharIndexFromLine(10))
                        {
                            // Select the text from the start of line 10 to the current position
                            int startOfLine10 = richTextBox3.GetFirstCharIndexFromLine(10);
                            //   richTextBox3.SelectionStart = startOfLine10;
                            richTextBox3.SelectionLength = currentPosition - startOfLine10;
                            richTextBox3.Text = "";
                            for (int i = 0; i < 10; i++)
                            {
                                richTextBox3.AppendText(Environment.NewLine);
                            }
                            richTextBox3.SelectionStart = 10;
                            //  int selectionStart = richTextBox3.SelectionStart;
                            //   richTextBox3.Select(10, 10);
                            e.Handled = true;
                            return;
                        }
                    }
                }
                /* else
                 {
                     e.Handled = false;
                     return;
                 }*/
                if (panel10.Visible)
                {
                    if ((e.Control && e.KeyCode == Keys.A))
                    {

                        /* int currentPosition = richTextBox3.SelectionStart;
                         if (dataGridView1 != null && currentPosition <= richTextBox3.GetFirstCharIndexFromLine(10))
                         {
                             richTextBox3.Clear();
                             richTextBox3.AppendText(Environment.NewLine);
                             e.Handled = true;
                             return;
                         }*/
                        e.Handled = true;

                        // Get the start of line 10
                        int startOfLine10 = richTextBox3.GetFirstCharIndexFromLine(10);
                        // Select text from the start of line 10 to the end of the text
                        richTextBox3.SelectionStart = startOfLine10;
                        richTextBox3.SelectionLength = richTextBox3.Text.Length - startOfLine10;
                        return;
                    }
                }
                if (e.KeyCode == Keys.Tab)
                {
                    return;
                }
                // Check if Ctrl + A is pressed
                if (e.Control && e.KeyCode == Keys.A)
                {
                    e.SuppressKeyPress = true; // Prevent the default behavior
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
             //   SaveDataToStructure();
                this.Enabled = false;
                PreviewMailScreen p = new PreviewMailScreen();
                p.Text = "Preview Mail Screen";
                p.FromEmailAddress = textBox5.Text;
                p.ToEmailAddresses = new List<string>(richTextBox1.Lines.Where(line => !string.IsNullOrWhiteSpace(line)));
                p.Subject = richTextBox2.Text;
                p.Body = richTextBox3.Rtf;
                StringBuilder body = new StringBuilder();
                string projectname =  cb_emailmeetings.Text;
                ProjectData existingProject = projectDataList.FirstOrDefault(p => p.projectName == projectname);
                //  List<string[]> dataGridView = new List<string[]>(existingProject.DataGridViewData);
                if (dataGridView1 != null)
                {
                    p.TransferDataFromForm1(dataGridView1);
                }
                // Initialize p.Attachments if it's not already initialized
                if (p.Attachments == null)
                {
                    //PreviewMailScreen.Attachments = new List<string>();
                   p.Attachments  = new List<string>();
                }

                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var attachment in attachments)
                    {
                        p.Attachments.Add(attachment);
                    }
                }
               if(AttachmentStore.Attachments!=null && AttachmentStore.Attachments.Count > 0)
                {
                    p.Attachments.AddRange(AttachmentStore.Attachments);
                }
                p.ShowDialog();
                // Re-enable the main form when the dynamic form is closed
                this.Enabled = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void AddFileButton1(string filePath, int fileCount, int buttonWidth, int buttonHeight, int fileButtonGap, int removeButtonGap, int maxButtonsPerRow)
        {
            try
            {
                int rowIndex = fileCount / maxButtonsPerRow;
                int colIndex = fileCount % maxButtonsPerRow;
                int x = colIndex * (buttonWidth + fileButtonGap);
                int y = rowIndex * (buttonHeight + removeButtonGap);
                Panel buttonPanel = CreateFileButtonPanel1(filePath, fileCount, buttonWidth, buttonHeight, fileButtonGap, removeButtonGap, maxButtonsPerRow);

                int newRow = buttonPanels.Count / maxButtonsPerRow;
                int newCol = buttonPanels.Count % maxButtonsPerRow;

                int newX = newCol * (buttonWidth + fileButtonGap);
                int newY = newRow * (buttonHeight + removeButtonGap);

              /*  if ((newCol + 1) * (buttonWidth + fileButtonGap) > panel5DW.Width)
                {
                    panel5DW.HorizontalScroll.Value += buttonWidth + fileButtonGap;
                }*/

                if (newCol > 0)
                {
                    int previousButtonIndex = buttonPanels.Count - 1;
                    int previousButtonX = buttonPanels[previousButtonIndex].Location.X;
                    int previousButtonWidth = buttonPanels[previousButtonIndex].Width;
                    newX = previousButtonX + previousButtonWidth + fileButtonGap;
                }

                buttonPanel.Location = new Point(newX, newY);
                panel5DW.Controls.Add(buttonPanel);
                buttonPanels.Add(buttonPanel);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + " \n " + ex.StackTrace);
            }
        }
        private void RemoveFileButtonPanel(Panel buttonPanel, string filePath)
        {
            int indexToRemove = buttonPanels.IndexOf(buttonPanel);
            if (indexToRemove >= 0 && indexToRemove < buttonPanels.Count)
            {
                buttonPanels.RemoveAt(indexToRemove);
                AttachmentStore.Attachments.RemoveAt(indexToRemove);
                // attachments.Remove(filePath);
                drawboardAttachments.Remove(filePath);
                panel5DW.Controls.Remove(buttonPanel);
                if (AttachmentStore.Attachments.Count > 0)
                {
                    // Clear and repopulate the panel
                    panel5DW.Controls.Clear();
                    buttonPanels.Clear();
                    int index = 0;

                    foreach (var filePath1 in AttachmentStore.Attachments)
                    {
                        var newButtonPanel = CreateFileButtonPanel1(filePath1, index, buttonWidth: 100, buttonHeight: 50, fileButtonGap: 10, removeButtonGap: 5, maxButtonsPerRow: 3);
                        buttonPanels.Add(newButtonPanel);
                        panel5DW.Controls.Add(newButtonPanel);
                        index++;
                    }
                }
            }
            else
            {
                MessageBox.Show("Error: Index out of range.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public Panel CreateFileButtonPanel1(string filePath, int index, int buttonWidth, int buttonHeight, int fileButtonGap, int removeButtonGap, int maxButtonsPerRow)
        {
         
                Panel buttonPanel = new Panel
                {
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(index % maxButtonsPerRow * (buttonWidth + fileButtonGap), index / maxButtonsPerRow * (buttonHeight + removeButtonGap)),
                    BorderStyle = BorderStyle.FixedSingle
                };
            try
            {
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
                    if (indexToRemove >= 0 && indexToRemove < buttonPanels.Count)
                    {
                        buttonPanels.RemoveAt(indexToRemove);
                        AttachmentStore.Attachments.RemoveAt(indexToRemove);
                        drawboardAttachments.Remove(filePath);
                        panel5DW.Controls.Remove(buttonPanel);
                        for (int i = indexToRemove; i < buttonPanels.Count; i++)
                        {
                            int row = i / maxButtonsPerRow;
                            int col = i % maxButtonsPerRow;
                            int newX = col * (buttonWidth + fileButtonGap);
                            int newY = row * (buttonHeight + removeButtonGap);
                            buttonPanels[i].Location = new Point(newX, newY);
                        }
                    }
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
                 }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

                return buttonPanel;
           
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Controls.Clear();
                panel1.BackColor = Color.White;
                MOMC mOMC = new MOMC();
                mOMC.TopLevel = false;  // This makes it not a top-level control
                mOMC.FormBorderStyle = FormBorderStyle.None;  // Remove borders if necessary
                mOMC.Dock = DockStyle.Fill;
                panel1.Controls.Add(mOMC);
                mOMC.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // panel1.Visible = false;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = " \"JPEG Image|*.jpg|JPEG Image|*.jpeg|PNG Image|*.png\"";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    int buttonWidth = 200;
                    int buttonHeight = 50;
                    int fileButtonGap = 20;
                    int removeButtonGap = 0;
                    int maxButtonsPerRow = (panel5DW.Width) / (buttonWidth + fileButtonGap);
                    int fileCount = 0;
                    panel5DW.Controls.Clear();
                    buttonPanels.Clear();
                    panel5DW.AutoScroll = true;
                    foreach (string selectedFilePath in openFileDialog.FileNames)
                    {
                        attachments.Add(selectedFilePath);
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
                            attachments.Remove(selectedFilePath);
                            panel5DW.Controls.Remove(buttonPanel);
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
                        panel5DW.Controls.Add(buttonPanel);
                        buttonPanels.Add(buttonPanel);
                        fileCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                panel1.Controls.Clear();
                DrawImageIcon db = new DrawImageIcon();
                panel1.Dock = DockStyle.Fill;
                panel1.Controls.Add(db);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void richTextBox3_MouseLeave(object sender, EventArgs e)
        {
            /* if (Control.ModifierKeys == Keys.Control)
             {
                 ((HandledMouseEventArgs)e).Handled = false;
             }*/
        }

     
        private Dictionary<string, string> emailData = new Dictionary<string, string>(); // Dictionary to store subject and description
        List<ProjectData> projectDataList = new List<ProjectData>();
        List<string[]> dataGridViewData = new List<string[]>();
        /*       private void SaveDataToStructure()
               {
                   try
                   {
                       string projectName = cb_emailmeetings.Text;
                       string subject = richTextBox2.Text;
                      string description = richTextBox3.Rtf;
                       if (dataGridView1 != null)
                       {
                           // Iterate through the DataGridView rows and store data
                           foreach (DataGridViewRow row in dataGridView1.Rows)
                           {
                               bool hasEmptyCell = false; // Flag to track if any cell is empty in the current row
                               List<string> rowData = new List<string>();
                               foreach (DataGridViewCell cell in row.Cells)
                               {
                                   string cellValue = cell.Value?.ToString() ?? "";
                                   rowData.Add(cellValue);
                                   if (string.IsNullOrEmpty(cellValue))
                                   {
                                       hasEmptyCell = true;
                                   }
                               }
                               if (hasEmptyCell)
                               {
                                   label13.Text = "Please fill in all cells in table before sending the email.";
                                   label13.ForeColor = Color.Red;
                                   return; 
                               }

                               dataGridViewData.Add(rowData.ToArray());
                           }
                       }
                       // Create a new ProjectData object and add it to the list
                       ProjectData projectData = new ProjectData(projectName, subject, description, dataGridViewData);
                       projectDataList.Add(projectData);
                   }
                   catch(Exception ex)
                   {
                       Console.WriteLine(ex.Message);
                   }
               }*/
        /*  private void SaveDataToStructure()
          {
              try
              {
                  string projectName = cb_emailmeetings.Text;
                  string subject = richTextBox2.Text;
                  string description = richTextBox3.Rtf;
                  // Check if a project with the same name already exists
                  ProjectData existingProject = projectDataList.FirstOrDefault(p => p.projectName == projectName);
                  Console.WriteLine(attachments);
                  if (existingProject != null)
                  {
                      existingProject.Subject = subject;
                      existingProject.Description = description;
                      existingProject.DataGridViewData = dataGridViewData; // Update DataGridViewData if necessary
                      ProjectData projectData = new ProjectData(projectName, subject, description, dataGridViewData);
                      projectDataList.Add(projectData);
                  }
                  else
                  {
                      if (dataGridView1 != null)
                      {
                          // Iterate through the DataGridView rows and store data
                          foreach (DataGridViewRow row in dataGridView1.Rows)
                          {
                              bool hasEmptyCell = false; // Flag to track if any cell is empty in the current row
                              List<string> rowData = new List<string>();
                              foreach (DataGridViewCell cell in row.Cells)
                              {
                                  string cellValue = cell.Value?.ToString() ?? "";
                                  rowData.Add(cellValue);
                                  if (string.IsNullOrEmpty(cellValue))
                                  {
                                      hasEmptyCell = true;
                                  }
                              }
                              if (hasEmptyCell)
                              {
                                  label13.Text = "Please fill in all cells in table before sending the email.";
                                  label13.ForeColor = Color.Red;
                                  return;
                              }

                              dataGridViewData.Add(rowData.ToArray());
                          }
                      }
                      if (attachments != null)
                      {
                          ProjectData projectData = new ProjectData(projectName, subject, description, dataGridViewData);
                          projectDataList.Add(projectData);
                      }
                      else
                      {
                          ProjectData projectData = new ProjectData(projectName, subject, description, dataGridViewData);
                          projectDataList.Add(projectData);
                      }
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.Message);
              }
          }
  */
        private void SaveDataToStructure()
        {
            try
            {
                string projectName = cb_emailmeetings.Text;
                string subject = richTextBox2.Text;
                string description = richTextBox3.Rtf;
                // Check if a project with the same name already exists
                ProjectData existingProject = projectDataList.FirstOrDefault(p => p.projectName == projectName);

                if (existingProject != null)
                {
                    // Update existing project data
                    existingProject.Subject = subject;
                    existingProject.Description = description;
                    
                }
                else
                {
                  
                    ProjectData projectData = new ProjectData(projectName, subject, description);
                    projectDataList.Add(projectData);
                }
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /*   public void LoadDataFromStructure(string projectName)
           {
               try
               {
                   // Find the ProjectData object where the projectName matches the specified project name
                   ProjectData projectData = projectDataList.FirstOrDefault(p => p.projectName == projectName);
                   if (projectData != null)
                   {
                       if (dataGridView1 != null)
                       {
                           dataGridView1.Dispose();
                       }
                       if (projectData != null)
                       {
                           richTextBox2.Text = projectData.Subject;
                           if (projectData.DataGridViewData != null)
                           {
                               richTextBox3.Rtf = projectData.Description;
                           }
                           else
                           {
                               string[] lines = richTextBox3.Lines;
                               if (lines.Length > 10)
                               {
                                   richTextBox3.Rtf = string.Join(Environment.NewLine, lines.Skip(10));
                               }
                           }
                           if (projectData.DataGridViewData != null)
                           {
                               InitializeDataGridView();
                               if (dataGridView1 != null)
                               {
                                   dataGridView1.Rows.Clear();
                                   if (dataGridView1.Rows.Count == 0) // Check if dataGridView1 is empty
                                   {
                                       foreach (string[] rowData in projectData.DataGridViewData)
                                       {
                                           dataGridView1.Rows.Add(rowData);
                                       }
                                   }
                                   if (dataGridView1.Rows.Count > 0)
                                   {
                                       panel10.Visible = true;
                                       button16.Enabled = false;
                                       panel11.Visible = true;
                                       button7.Visible = true;
                                   }
                               }
                           }
   *//*                        List<string> s = new List<string>();

                           if (projectData != null && projectData.attachments != null)
                           {
                               s = new List<string>(projectData.attachments);
                           }
                           int buttonWidth = 200;
                           int buttonHeight = 50;
                           int fileButtonGap = 20;
                           int removeButtonGap = 0;
                           int maxButtonsPerRow = panel5DW.Width / (buttonWidth + fileButtonGap);
                           int fileCount = attachments.Count; // Start file count from current list size
                           panel4.Controls.Clear();
                           buttonPanels.Clear();
                           panel4.AutoScroll = true;
                           foreach (var filepath in s)
                           {
                               attachments.Add(filepath);
                               AddFileButton(filepath, fileCount, buttonWidth, buttonHeight, fileButtonGap, removeButtonGap, maxButtonsPerRow);
                               fileCount++;
                           }*//*
                       }
                       if (projectData.DataGridViewData == null)
                       {
                           panel10.Visible = false;
                           button16.Enabled = true;
                           panel11.Visible = false;
                           button7.Visible = false;
                           *//* string[] lines = richTextBox3.Lines;
                            if (lines.Length > 10)
                            {
                                richTextBox3.Text = string.Join(Environment.NewLine, lines.Skip(10));
                            }*//*
                       }
                   }
                   else
                   {
                       panel10.Visible = false;
                       panel11.Visible = false;
                       button7.Visible = false;
                       button16.Enabled = true;
                   }

               }

               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
               }


           }*/
        public void LoadDataFromStructure(string projectName)
        {
            try
            {
                panel10.Visible = false;
                // Find the ProjectData object where the projectName matches the specified project name
                ProjectData projectData = projectDataList.FirstOrDefault(p => p.projectName == projectName);
                if (projectData != null)
                {
                    if (dataGridView1 != null)
                    {
                        dataGridView1.Dispose();
                    }
                    if (projectData != null)
                    {
                        richTextBox2.Text = projectData.Subject;
                        richTextBox3.Rtf = projectData.Description;
                        if (dataGridView1 != null)
                        {
                            richTextBox3.Rtf = projectData.Description;
                        }
                        else
                        {
                            string[] lines = richTextBox3.Lines;
                            if (lines.Length > 10)
                            {
                                richTextBox3.Rtf = string.Join(Environment.NewLine, lines.Skip(10));
                            }
                        }
                    }
                }
                else
                {
                  /*  panel10.Visible = false;
                    panel11.Visible = false;
                    button7.Visible = false;
                    button16.Enabled = true;*/
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveDataToStructure();
        }
        // view / hide table
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (panel10.Visible)
                {
                    panel10.Visible = false;
                    panel11.Visible = false;
                    //  button16.Enabled = false;
                    string currentText = richTextBox3.Text;

                    // Split the text into lines
                    string[] lines = currentText.Split('\n');

                    // Remove the first 10 empty lines
                    int count = 0;
                    for (int i = 0; i < lines.Length && count < 10; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[i]))
                        {
                            lines[i] = null; // Mark the line for removal
                            count++;
                        }
                        else
                        {
                            break; // Stop removing lines if a non-empty line is encountered
                        }
                    }
                    // Join the remaining lines back into a single string
                    string newText = string.Join("\n", lines.Where(line => line != null));

                    // Set the new text as the text of RichTextBox3
                    richTextBox3.Text = newText;
                }
                else
                {
                    panel10.Visible = true;
                    panel11.Visible = true;
                    string currentText = richTextBox3.Text;

                    // Split the text into lines
                    string[] lines = currentText.Split('\n');
                    for (int i = 0; i < 10; i++)
                    {
                        Array.Resize(ref lines, lines.Length + 1);
                        Array.Copy(lines, 0, lines, 1, lines.Length - 1);
                        lines[0] = "";
                    }
                    string newText = string.Join("\n", lines);
                    // Set the new text as the text of RichTextBox3
                    richTextBox3.Text = newText;
                    richTextBox3.Focus();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        internal void AttachFile(string filePath)
        {
            throw new NotImplementedException();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                DrawBoard d = new DrawBoard();
                d.Show();
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void richTextBox3_MouseUp(object sender, MouseEventArgs e)
        {





        }
    }

}
