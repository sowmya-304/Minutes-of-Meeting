using static MOMC_PROJECT.MOM_Prop;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace MOMC_PROJECT
{
    public partial class MOMC : Form
    {
        public static string toEmail { get; set; }
        private int verificationCode;
        private readonly string senderEmail = "19n81a05c9.sowmya@gmail.com";
        private readonly string senderPassword = "tkki grgd aapo uavx\r\n";
        private List<MeetingData> meetingDataList;
        private System.Windows.Forms.Timer otpTimer;
        private int otpTimeLeft;

        public MOMC()
        {
            InitializeComponent();
            OnLoad();
        }

        private void OnLoad()
        {
            btn_resendotp.Enabled = false;
            verificationCode = GenerateVerificationCode();

            // Define the path to the JSON file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

            // Read the JSON file
            string jsonData = File.ReadAllText(filePath);

            // Deserialize the JSON data to a list of your desired object
            meetingDataList = JsonConvert.DeserializeObject<List<MeetingData>>(jsonData);
            // Initialize the timer
            otpTimer = new System.Windows.Forms.Timer();
            otpTimer.Interval = 1000; // 1 second
            otpTimer.Tick += OtpTimer_Tick;
        }
        private void OtpTimer_Tick(object sender, EventArgs e)
        {
            if (otpTimeLeft > 0)
            {
                otpTimeLeft--;
                label4.Text = $"Time left: {otpTimeLeft}s";
            }
            else
            {
                otpTimer.Stop();
                btn_verifyotp.Enabled = false;
                btn_resendotp.Enabled = true;
                MessageBox.Show("OTP has expired. Please request a new one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        private void btn_sendotp_Click(object sender, EventArgs e)
        {
            toEmail = tb_email.Text.Trim();

            if (IsValidEmail(toEmail))
            {
                //try
                //{
                SendVerificationEmail(toEmail, verificationCode);
                MessageBox.Show("Verification Code Sent Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Start the OTP timer
                otpTimeLeft = 30; // 30 seconds for OTP verification
                label4.Text = $"Time left: {otpTimeLeft}s";
                otpTimer.Start();
                btn_verifyotp.Enabled = true;

                tb_email.Enabled = true;
                btn_verifyotp.Enabled = true;
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show($"Error sending email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
            else
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendVerificationEmail(string recipientEmail, int code)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            message.From = new MailAddress(senderEmail);
            message.To.Add(recipientEmail);
            message.Subject = "MOMC";
            message.Body = $"Your verification code is: {code}";

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            smtpClient.Send(message);
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

        private void btn_verifyotp_Click(object sender, EventArgs e)
        {
            string enteredCode = tb_otp.Text.Trim();

            if (enteredCode == verificationCode.ToString())
            {
                MessageBox.Show("Verification Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                otpTimer.Stop();

                // Proceed with further actions after successful verification
                var meetingsForEmail = GetMeetingsForEmail(toEmail);

                //ToEmail = toEmail;
                panel1.Controls.Clear();
                MeetingsInfo_ComposeEmail meetingsInfoComposeEmail = new MeetingsInfo_ComposeEmail();
                panel1.Dock = DockStyle.Fill;
                panel1.Controls.Add(meetingsInfoComposeEmail);

                // Populate meetingsInfoComposeEmail with meetingsForEmail if necessary
            }
            else
            {
                MessageBox.Show("Invalid Verification Code. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            toEmail = tb_email.Text.Trim();

            if (IsValidEmail(toEmail))
            {
                try
                {
                    SendVerificationEmail(toEmail, verificationCode);
                    MessageBox.Show("Verification Code Sent Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Start the OTP timer
                    otpTimeLeft = 30; // 30 seconds for OTP verification
                    label4.Text = $"Time left: {otpTimeLeft}s";
                    otpTimer.Start();
                    btn_verifyotp.Enabled = true;

                    tb_email.Enabled = true;
                    btn_verifyotp.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error sending email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            MeetingsInfo_ComposeEmail meetingsInfo_ComposeEmail = new MeetingsInfo_ComposeEmail();
            panel1.Dock = DockStyle.Fill;
            panel1.Controls.Add(meetingsInfo_ComposeEmail);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            DrawImageIcon d=new DrawImageIcon();    
            panel1.Dock = DockStyle.Fill;
            panel1.Controls.Add(d);

        }
    }
}

