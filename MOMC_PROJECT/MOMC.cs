using static MOMC_PROJECT.MOM_Prop;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
namespace MOMC_PROJECT
{
    public partial class MOMC : Form
    {
        public static string toEmail { get; set; }
        public static List<MeetingData> meetingDataList;
        string otp;
        private int otpCountDown;
        public MOMC()
        {
            InitializeComponent();
            OnLoad();
        }
        private void OnLoad()
        {
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            panel2.Visible = true;
            panel3.Visible = false;
            btn_resendotp.Enabled = false;
            btn_resendotp.BackColor = Color.LightGray;
            btn_resendotp.ForeColor = Color.Gray;
            btn_verifyotp.Enabled = false;
            btn_verifyotp.BackColor = Color.LightGray;
            btn_verifyotp.ForeColor = Color.Gray;
            label7.Text = "";
            label4.Text = "01:30";
            label8.Text = "";
            label9.Text = "";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Data.json");
            string jsonData = System.IO.File.ReadAllText(filePath);
            meetingDataList = JsonConvert.DeserializeObject<List<MeetingData>>(jsonData);
        }

        private bool IsInternetConnected()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        private async void btn_sendotp_Click(object sender, EventArgs e)
        {
            if (!IsInternetConnected())
            {
                DisplayLabelText("Please connect to the internet and try again.", 8000, Color.Red, label8);
                return;
            }

            if (string.IsNullOrWhiteSpace(tb_email.Text))
            {
                DisplayLabelText("Please Enter Email", 8000, Color.Red, label8);
                return;
            }
            string enteredEmail = tb_email.Text.Trim().ToLower();
            toEmail = enteredEmail;
            var userData = meetingDataList.FirstOrDefault(data => data.Email.ToLower() == enteredEmail);
            if (userData == null)
            {
                DisplayLabelText("Email Not Found", 8000, Color.Red, label8);
                return;
            }
            DisplayLabelText("Sending OTP...", Timeout.Infinite, Color.Red, label8); // Use Timeout.Infinite for duration

            string fromEmail = "19n81a05c9.sowmya@gmail.com";
            string password = "tkki grgd aapo uavx\r\n";
            string subject = "Your One-Time Password (OTP) for MOMC";
            otp = GenerateOTP();
            string body = $"Hi\n\nPlease find below your requested OTP:\n{otp}\n\n" +
                          $"Please note that this OTP is valid only for a particular time after requesting.\n\n\n" +
                          $"In case the OTP expires, you can request it again by clicking on 'Resend OTP' button on MOMC";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("19n81a05c9.sowmya@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(enteredEmail);
            try
            {
                await smtpClient.SendMailAsync(mailMessage); // Send mail asynchronously
                DisplayLabelText("OTP sent successfully!", 8000, Color.Green, label8);
                label7.Text = $"We have sent an OTP to your entered email - {toEmail}";
                System.Windows.Forms.Timer navigationTimer = new System.Windows.Forms.Timer();
                navigationTimer.Interval = 500;
                navigationTimer.Tick += (timerSender, timerArgs) =>
                {
                    navigationTimer.Stop();
                    panel2.Visible = false;
                    panel3.Visible = true;
                    otpCountDown = 90;
                    StartOtpTimer();
                };
                navigationTimer.Start();
            }
            catch (Exception ex)
            {
                DisplayLabelText($"Failed to send OTP. Check Your Network Connection and TryAgain", 8000, Color.Red, label8);
            }
        }

        private void DisplayLabelText(string text, int duration, Color fontColor, Label labelToUpdate)
        {
            labelToUpdate.Text = text;
            labelToUpdate.ForeColor = fontColor;
            if (duration != Timeout.Infinite) // Check if the duration is not infinite
            {
                System.Windows.Forms.Timer labelTimer = new System.Windows.Forms.Timer();
                labelTimer.Interval = duration;
                labelTimer.Tick += (timerSender, timerArgs) =>
                {
                    labelTimer.Stop();
                    labelToUpdate.Text = "";
                };
                labelTimer.Start();
            }
        }


        private void StartOtpTimer()
        {
            label4.Visible = true;
            int minutes = (otpCountDown / 60);
            int seconds = (otpCountDown % 60);
            label4.Text = $"time left: {minutes:D2}:{seconds:D2}";
            btn_resendotp.Enabled = false;
            btn_resendotp.BackColor = Color.LightGray;
            btn_resendotp.ForeColor = Color.Gray;
            timer1.Start();
        }
        private void btn_verifyotp_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            string enteredCode = tb_otp.Text.Trim();
            try
            {
                if (string.IsNullOrEmpty(enteredCode))
                {
                    DisplayLabelText("Please enter the OTP.", 8000, Color.Red, label9);
                    return;
                }

                if (enteredCode.Length != 6)
                {
                    DisplayLabelText("OTP must contain 6 digits.", 8000, Color.Red, label9);
                    return;
                }

                if (enteredCode == otp.ToString())
                {
                    DisplayLabelText("Verification Successful!", 8000, Color.Green, label9);
                    timer1.Stop();
                    var meetingsForEmail = GetMeetingsForEmail(toEmail);
                    panel1.Controls.Clear();
                    panel1.BackColor = Color.White;
                    MeetingsInfo_ComposeEmail meetingsInfoComposeEmail = new MeetingsInfo_ComposeEmail();
                    panel1.Dock = DockStyle.Fill;
                    panel1.Controls.Add(meetingsInfoComposeEmail);
                }
                else
                {
                    DisplayLabelText("Invalid Verification Code. Please try again.", 8000, Color.Red, label9);
                }
            }
            catch (Exception ex)
            {
                DisplayLabelText($"An error occurred: {ex.Message}", 8000, Color.Red, label9);
            }
        }
        private List<string> GetMeetingsForEmail(string email)
        {
            return meetingDataList
                .Where(md => md.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                .SelectMany(md => md.Meetings)
                .Select(m => m.Name)
                .ToList();
        }
        private async void btn_resendotp_Click(object sender, EventArgs e)
        {
            if (!IsInternetConnected())
            {
                DisplayLabelText("Please connect to the internet and try again.", 8000, Color.Red, label9);
                return;
            }

            // Change label9 text to indicate sending
            DisplayLabelText("Sending OTP...", Timeout.Infinite, Color.Red, label9); // Use Timeout.Infinite for duration

            string enteredEmail = tb_email.Text.Trim().ToLower();
            toEmail = enteredEmail;
            string fromEmail = "19n81a05c9.sowmya@gmail.com";
            string password = "tkki grgd aapo uavx\r\n";
            string subject = "Your One-Time Password (OTP) for MOMC";
            otp = GenerateOTP();
            string body = $"Hi\n\nPlease find below your requested OTP:\n{otp}\n\n" +
                          $"Please note that this OTP is valid only for a particular time after requesting.\n\n\n" +
                          $"In case the OTP expires, you can request it again by clicking on 'Resend OTP' button on MOMC";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("19n81a05c9.sowmya@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(enteredEmail);
            try
            {
                await smtpClient.SendMailAsync(mailMessage); // Send mail asynchronously
                DisplayLabelText("OTP sent successfully!", 8000, Color.Green, label9);
                label7.Text = $"We have sent an OTP to your -{toEmail} ";
                System.Windows.Forms.Timer navigationTimer = new System.Windows.Forms.Timer();
                navigationTimer.Interval = 500;
                navigationTimer.Tick += (timerSender, timerArgs) =>
                {
                    navigationTimer.Stop();
                    panel2.Visible = false;
                    panel3.Visible = true;
                    otpCountDown = 90;
                    StartOtpTimer();
                };
                navigationTimer.Start();
            }
            catch (Exception ex)
            {
                DisplayLabelText($"Failed to send OTP. Check Your Network Connection and TryAgain", 8000, Color.Red, label8);
            }
        }

        public static string GenerateOTP(int length = 6)
        {
            const string chars = "0123456789";
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                crypto.GetBytes(data);
                var result = new StringBuilder(length);
                foreach (var byteValue in data)
                {
                    result.Append(chars[byteValue % chars.Length]);
                }
                return result.ToString();
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;
            btn_sendotp.Enabled = true;
            btn_sendotp.BackColor = Color.Green;
            btn_sendotp.ForeColor = Color.White;
            timer1.Stop();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (otpCountDown > 0)
            {
                otpCountDown--;
                int minutes = (otpCountDown / 60);
                int seconds = (otpCountDown % 60);
                label4.Text = $"time left: {minutes:D2}:{seconds:D2}";
                btn_resendotp.Enabled = false;
                btn_resendotp.BackColor = Color.LightGray;
                btn_resendotp.ForeColor = Color.Gray;
                btn_verifyotp.Enabled = true;
                btn_verifyotp.BackColor = Color.Green;
                btn_verifyotp.ForeColor = Color.White;
            }
            else
            {
                timer1.Stop();
                btn_verifyotp.Enabled = false;
                btn_verifyotp.BackColor = Color.LightGray;
                btn_verifyotp.ForeColor = Color.Gray;
                btn_resendotp.Enabled = true;
                btn_resendotp.BackColor = Color.Green;
                btn_resendotp.ForeColor = Color.White;
                DisplayLabelText("OTP has expired. Please request a new one.", 8000, Color.Red, label9);
            }
        }
        private void tb_otp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                DisplayLabelText("Please enter only digits (0-9).", 8000, Color.Red, label9);
                e.Handled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MeetingsInfo_ComposeEmail m = new MeetingsInfo_ComposeEmail();
            panel1.Controls.Clear();
            panel1.Controls.Add(m);
        }

        private void MOMC_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit the application?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
