using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOMC_PROJECT
{
    public partial class Draw : Form
    {
        public Draw()
        {
            InitializeComponent();
        }

        private void Draw_Load(object sender, EventArgs e)
        {
            try
            {
                DrawBoard d = new DrawBoard();
                panel1.Dock = DockStyle.Fill;
                panel1.Controls.Add(d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Draw_FormClosing(object sender, FormClosingEventArgs e)
        {
           


        }
        private List<Panel> buttonPanels = new List<Panel>();
        public List<string> attachments = new List<string>();
        private List<string> persistentAttachments = new List<string>();
        private int buttonHeight;
        private void AttachFilesToPanel2(string filePath)
        {
            MeetingsInfo_ComposeEmail mc = new MeetingsInfo_ComposeEmail();
            if (mc != null)
            {
                Panel panel5DW = mc.Panel5DW;

                if (panel5DW != null)
                {
                   // panel5DW.Controls.Clear();
                   // buttonPanels.Clear();
                    panel5DW.AutoScroll = true;

                    int buttonWidth = 200;
                    int buttonHeight = 50;
                    int fileButtonGap = 20;
                    int removeButtonGap = 0;
                    int maxButtonsPerRow = (panel5DW.Width) / (buttonWidth + fileButtonGap);
                    int fileCount = 0;
                    attachments.Add(filePath);
                    persistentAttachments.Add(filePath);
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
                        attachments.RemoveAt(indexToRemove);
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
                    //}
                }

                /* panel1.Controls.Clear();
                 panel1.Dock = DockStyle.Fill;
                 panel1.BackColor = SystemColors.Control;
                 panel1.Controls.Add(mc);*/
            }
        }
        private void UpdateButtonPanelLocations(Panel panel5DW, int maxButtonsPerRow, int buttonWidth, int fileButtonGap, int removeButtonGap)
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

    }
}
