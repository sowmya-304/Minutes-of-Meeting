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
        public static string toEmailId { get; set; }
        public static List<MeetingData> meetingDataList;
        string otp;
        private int otpCountDown = 200;
        public MOMC()
        {
            InitializeComponent();
            OnLoad();
        }
        private void OnLoad()
        {
            panel2.Visible = true;
            panel3.Visible = false;
            btn_resendotp.Enabled = false;
            btn_resendotp.BackColor = Color.LightGray;
            btn_resendotp.ForeColor = Color.Gray;
            btn_verifyotp.Enabled = false;
            btn_verifyotp.BackColor = Color.LightGray;
            btn_verifyotp.ForeColor = Color.Gray;
            label4.Visible = false;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"D:\UI\Minutes of Meeting\MOMC_PROJECT\Data.json");
            string jsonData = System.IO.File.ReadAllText(filePath);
            meetingDataList = JsonConvert.DeserializeObject<List<MeetingData>>(jsonData);
        }

        private void btn_sendotp_Click(object sender, EventArgs e)
        {
            label7.Text = $"We've Sent an OTP to your email- {tb_email.Text}";
            toEmail = tb_email.Text.Trim();
            string fromEmail = "19n81a05c9.sowmya@gmail.com";
            string password = "tkki grgd aapo uavx\r\n";
            string subject = "Your One-Time Password (OTP) for MOMC";
            otp = GenerateOTP();
            string body = $"Hi\n\nPlease find below your requested OTP:\n{otp}\n\n" +
                $"Please note that this OTP is valid only for particular time after requesting\n\n\n" +
                $"In case the OTP expires you can request it again by clicking on 'Resend OTP' button on MOMC";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(toEmail);

            try
            {
                smtpClient.Send(mailMessage);
                DialogResult result = MessageBox.Show("OTP sent successfully!", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                {
                    otp = null;
                    panel2.Visible = true;
                    panel3.Visible = false;
                }
                if (result == DialogResult.OK)
                {
                    panel2.Visible = false;
                    panel3.Visible = true;
                    StartOtpTimer();
                    tb_email.Enabled = true;

                    btn_sendotp.Enabled = false;
                    btn_sendotp.BackColor = Color.LightGray;
                    btn_sendotp.ForeColor = Color.Gray;

                    btn_verifyotp.Enabled = true;
                    btn_verifyotp.BackColor = Color.Green;
                    btn_verifyotp.ForeColor = Color.White;

                    btn_resendotp.Enabled = false;
                    btn_resendotp.BackColor = Color.LightGray;
                    btn_resendotp.ForeColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send OTP: {ex.Message}");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void StartOtpTimer()
        {
            label4.Visible = true;
            label4.Text = $"time left: {otpCountDown}s";
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
                if (enteredCode == otp.ToString())
                {
                    MessageBox.Show("Verification Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Invalid Verification Code. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {

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

        private void btn_resendotp_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            label7.Text = $"We've Sent an OTP to your email- {tb_email.Text}";
            toEmail = tb_email.Text.Trim();
            string fromEmail = "19n81a05c9.sowmya@gmail.com";
            string password = "tkki grgd aapo uavx\r\n";
            string subject = "Your One-Time Password (OTP) for MOMC";
            otp = GenerateOTP();
            string body = $"Hi\n\nPlease find below your requested OTP:\n{otp}\n\n" +
                $"Please note that this OTP is valid only for particular time after requesting\n\n\n" +
                $"In case the OTP expires you can request it again by clicking on 'Resend OTP' button on MOMC";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(toEmail);

            try
            {
                smtpClient.Send(mailMessage);
                DialogResult result = MessageBox.Show("OTP sent successfully!", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                {
                    panel2.Visible = false;
                    panel3.Visible = false;
                }
                if (result == DialogResult.OK)
                {
                    panel2.Visible = false;
                    panel3.Visible = true;
                    otp = null;
                    otpCountDown = 120;
                    StartOtpTimer();
                    //tb_email.Enabled = true;

                    //btn_sendotp.Enabled = false;
                    //btn_sendotp.BackColor = Color.LightGray;
                    //btn_sendotp.ForeColor = Color.Gray;

                    btn_verifyotp.Enabled = true;
                    btn_verifyotp.BackColor = Color.Green;
                    btn_verifyotp.ForeColor = Color.White;

                    btn_resendotp.Enabled = false;
                    btn_resendotp.BackColor = Color.LightGray;
                    btn_resendotp.ForeColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send OTP: {ex.Message}");
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
                label4.Text = $"time left: {otpCountDown}s";
                btn_resendotp.Enabled = false;
                btn_resendotp.BackColor = Color.LightGray;
                btn_resendotp.ForeColor = Color.Gray;
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
                MessageBox.Show("OTP has expired. Please request a new one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
