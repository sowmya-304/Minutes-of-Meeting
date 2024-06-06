using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOMC_PROJECT
{
    public partial class PreviewMailScreen : Form
    {
        private ImageList imageList1 = new ImageList();
        public string FromEmailAddress { get; set; }

        public List<string> ToEmailAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; }

        public PreviewMailScreen()
        {
            InitializeComponent();
            imageList1.ImageSize = new Size(32, 32);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.BackgroundColor = Color.White;
            //  dataGridView1.Size = panel3.Size;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.Enabled = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void PreviewMailScreen_Load(object sender, EventArgs e)
        {
            try
            {
              

                label6.Text = FromEmailAddress;
                label7.Text = Subject;
                richTextBox1.Text = string.Join(", ", ToEmailAddresses);
                richTextBox3.Rtf = Body;
                // Clear listView1 items before adding new ones
                listView1.Items.Clear();
                // Ensure that Attachments and listBox1 are not null
                if (Attachments != null)
                {
                    foreach (string attachment in Attachments)
                    {
                        string fileName = Path.GetFileName(attachment);
                        // Add the file name to the listView1
                        listView1.Items.Add(fileName);
                        // Extract associated icon
                        Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(attachment);
                        // Add the icon to the ImageList
                        imageList1.Images.Add(fileName, fileIcon.ToBitmap());
                        // Get the index of the added image in the ImageList
                        int imageIndex = imageList1.Images.IndexOfKey(fileName);
                        // Assign the image index to the ListViewItem
                        listView1.Items[listView1.Items.Count - 1].ImageIndex = imageIndex;
                    }
                }
                // Set the LargeImageList of listView1 to the imageList1
                listView1.LargeImageList = imageList1;
                using (Graphics g = CreateGraphics())
                {
                    SizeF sizeTo = g.MeasureString(label6.Text, label6.Font);
                    label6.Width = (int)Math.Ceiling(sizeTo.Width);
                    label6.Height = (int)Math.Ceiling(sizeTo.Height);
                    SizeF sizeSubject = g.MeasureString(label7.Text, label7.Font);
                    label7.Width = (int)Math.Ceiling(sizeSubject.Width);
                    label7.Height = (int)Math.Ceiling(sizeSubject.Height);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void TransferDataFromForm1(DataGridView sourceGridView)
        {
          
            /* try
             {
                 if (dataGridView1.Columns.Count == 0)
                 {
                     foreach (DataGridViewColumn col in dataGridView.Columns)
                     {
                         dataGridView1.Columns.Add(col.Clone() as DataGridViewColumn);
                     }
                 }

                 foreach (DataGridViewRow row in dataGridView.Rows)
                 {
                     if (!row.IsNewRow)
                     {
                         DataGridViewRow newRow = new DataGridViewRow();
                         for (int i = 0; i < Math.Min(3, row.Cells.Count); i++)
                         {
                             newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row.Cells[i].Value });
                         }
                         dataGridView1.Rows.Add(newRow);
                     }
                 }
                 if (dataGridView1.Rows.Count == 0)
                 {
                     panel3.Visible = false;
                     dataGridView1.Visible = false;
                 }
                 else
                 {
                     panel3.Visible = true;
                     dataGridView1.Visible = true;
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }*/
            try
            {
                if (sourceGridView.Columns.Count == 0 || sourceGridView.Rows.Count == 0)
                {
                    // If there are no columns or rows in the source grid view, hide the destination grid view and panel
                    panel3.Visible = false;
                    dataGridView1.Visible = false;
                }
                else
                {
                    // Ensure destination grid view has enough columns
                    while (dataGridView1.Columns.Count < sourceGridView.Columns.Count)
                    {
                        // Add columns dynamically based on the number of columns in the source grid view
                        foreach (DataGridViewColumn col in sourceGridView.Columns)
                        {
                            dataGridView1.Columns.Add(col.Clone() as DataGridViewColumn);
                        }
                    }

                    // Ensure destination grid view has enough rows
                    while (dataGridView1.Rows.Count < sourceGridView.Rows.Count)
                    {
                        dataGridView1.Rows.Add();
                    }

                    // Copy data from source grid view to destination grid view
                    for (int rowIndex = 0; rowIndex < sourceGridView.Rows.Count; rowIndex++)
                    {
                        DataGridViewRow sourceRow = sourceGridView.Rows[rowIndex];
                        DataGridViewRow destRow = dataGridView1.Rows[rowIndex];

                        for (int colIndex = 0; colIndex < sourceGridView.Columns.Count; colIndex++)
                        {
                            // Add values from each cell up to the first three cells of each row
                            destRow.Cells[colIndex].Value = sourceRow.Cells[colIndex].Value;
                        }
                    }

                    // Show the destination grid view and panel
                    panel3.Visible = true;
                    dataGridView1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Log or handle the exception appropriately
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView listView = sender as ListView;
                if (listView != null && listView.SelectedItems.Count > 0)
                {
                    // Extract the selected item's text (file name)
                    string fileName = listView.SelectedItems[0].Text;

                    // Find the corresponding attachment
                    string attachment = Attachments.FirstOrDefault(a => Path.GetFileName(a) == fileName);

                    // If the attachment is found, open it
                    if (!string.IsNullOrEmpty(attachment))
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(attachment) { UseShellExecute = true });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count==0)
                {
                    panel3.Visible = false;
                    return;
                }
               
                if (panel3.Visible)
                {
                    panel3.Visible = false;
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
                    panel3.Visible = true;
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
