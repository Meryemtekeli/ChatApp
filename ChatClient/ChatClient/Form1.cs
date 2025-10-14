using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;
        Thread receiveThread;
        private string theme = "light";
        private string lastMessageSender = ""; // Son mesajý gönderen kiþiyi takip et
        private string currentUsername = ""; // Kendi kullanýcý adýmýz

        public Form1()
        {
            InitializeComponent();
            lblStatus.ForeColor = Color.Red;
        }

        public void AutoConnect(string username)
        {
            txtUsername.Text = username;
            btnConnect.PerformClick();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Lütfen kullanýcý adýnýzý girin!");
                    return;
                }

                currentUsername = txtUsername.Text; // Kullanýcý adýný kaydet

                client = new TcpClient();
                client.Connect("127.0.0.1", 8080);
                stream = client.GetStream();

                byte[] usernameData = Encoding.UTF8.GetBytes(txtUsername.Text);
                stream.Write(usernameData, 0, usernameData.Length);

                receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                lblStatus.Text = "Baðlandý";
                lblStatus.ForeColor = Color.Green;

                AddMessageBubble("Sistem", "(Baðlandýn)", false, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Baðlanýlamadý: " + ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e) => SendMessage();

        private void SendMessage()
        {
            if (stream == null || !stream.CanWrite || string.IsNullOrWhiteSpace(txtMessages.Text))
                return;

            string message = txtMessages.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);
            try { stream.Write(data, 0, data.Length); } catch { }

            AddMessageBubble(currentUsername, message, true, false);
            txtMessages.Clear();
        }

        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while (client != null && client.Connected && (bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Mesajý parse et: "Kullanýcý: mesaj" formatýnda
                    string sender = "Bilinmeyen";
                    string actualMessage = message;

                    int colonIndex = message.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        sender = message.Substring(0, colonIndex).Trim();
                        actualMessage = message.Substring(colonIndex + 1).Trim();
                    }

                    // Kendi mesajýmýzý gösterme (zaten gösterdik)
                    if (sender != currentUsername)
                    {
                        AddMessageBubble(sender, actualMessage, false, false);
                    }
                }
            }
            catch
            {
                AddMessageBubble("Sistem", "Sunucuyla baðlantý koptu.", false, true);
            }
        }

        private void AddMessageBubble(string sender, string message, bool isSender, bool isSystemMessage)
        {
            if (pnlChat.InvokeRequired)
            {
                pnlChat.Invoke(new Action(() => AddMessageBubble(sender, message, isSender, isSystemMessage)));
                return;
            }

            // Ayný kullanýcýdan mý kontrol et
            bool showSenderName = (lastMessageSender != sender);
            lastMessageSender = sender;

            Panel bubble = new Panel();
            bubble.AutoSize = true;
            bubble.Padding = new Padding(5);
            bubble.Margin = new Padding(showSenderName ? 10 : 2, 2, 5, 2); // Ayný kiþiden ise az boþluk

            // Kullanýcý adý etiketi (sadece farklý kullanýcý ise)
            if (showSenderName && !isSystemMessage)
            {
                Label lblSender = new Label();
                lblSender.AutoSize = true;
                lblSender.Text = sender;
                lblSender.Font = new Font("Segoe UI", 7, FontStyle.Bold);
                lblSender.ForeColor = theme == "light" ? Color.Gray : Color.LightGray;
                lblSender.Padding = new Padding(10, 5, 10, 0);
                lblSender.Location = new Point(isSender ? bubble.Width - lblSender.Width : 0, 0);
                bubble.Controls.Add(lblSender);
            }

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(pnlChat.Width - 60, 0);
            lbl.Text = message;
            lbl.Padding = new Padding(10);
            lbl.ForeColor = theme == "light" ? Color.Black : Color.White;
            lbl.Location = new Point(0, showSenderName && !isSystemMessage ? 20 : 0);

            // Oval balon oluþtur
            bubble.Paint += (s, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, 20, 20, 180, 90);
                    path.AddArc(bubble.Width - 20, 0, 20, 20, 270, 90);
                    path.AddArc(bubble.Width - 20, bubble.Height - 20, 20, 20, 0, 90);
                    path.AddArc(0, bubble.Height - 20, 20, 20, 90, 90);
                    path.CloseAllFigures();
                    bubble.Region = new Region(path);
                }
            };

            if (isSender)
            {
                bubble.BackColor = theme == "light" ? Color.LightGreen : Color.DodgerBlue;
                bubble.Anchor = AnchorStyles.Right;
            }
            else if (isSystemMessage)
            {
                bubble.BackColor = theme == "light" ? Color.LightYellow : Color.FromArgb(60, 60, 60);
                bubble.Anchor = AnchorStyles.None;
            }
            else
            {
                bubble.BackColor = theme == "light" ? Color.LightGray : Color.Gray;
                bubble.Anchor = AnchorStyles.Left;
            }

            bubble.Controls.Add(lbl);
            pnlChat.Controls.Add(bubble);
            pnlChat.ScrollControlIntoView(bubble);
        }

        private void chkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            ToggleTheme(chkDarkMode.Checked);
        }

        private void ToggleTheme(bool isDark)
        {
            theme = isDark ? "dark" : "light";

            this.BackColor = isDark ? Color.FromArgb(30, 30, 30) : Color.White;
            txtUsername.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            txtUsername.ForeColor = isDark ? Color.White : Color.Black;
            btnConnect.BackColor = isDark ? Color.DimGray : SystemColors.Control;
            btnConnect.ForeColor = isDark ? Color.White : Color.Black;
            lblStatus.ForeColor = isDark ? Color.White : Color.Black;
            chkDarkMode.ForeColor = isDark ? Color.White : Color.Black;
            txtMessages.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            txtMessages.ForeColor = isDark ? Color.White : Color.Black;
            pnlChat.BackColor = isDark ? Color.FromArgb(30, 30, 30) : Color.White;

            foreach (Control ctrl in pnlChat.Controls)
            {
                if (ctrl is Panel p && p.Controls.Count > 0)
                {
                    foreach (Control c in p.Controls)
                    {
                        if (c is Label lbl)
                        {
                            lbl.ForeColor = theme == "light" ? Color.Black : Color.White;
                        }
                    }

                    if (p.BackColor == Color.LightGreen || p.BackColor == Color.DodgerBlue)
                        p.BackColor = theme == "light" ? Color.LightGreen : Color.DodgerBlue;
                    else if (p.BackColor == Color.LightYellow || p.BackColor == Color.FromArgb(60, 60, 60))
                        p.BackColor = theme == "light" ? Color.LightYellow : Color.FromArgb(60, 60, 60);
                    else
                        p.BackColor = theme == "light" ? Color.LightGray : Color.Gray;
                }
            }
        }

        private void txtMessages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try { receiveThread?.Abort(); } catch { }
            try { stream?.Close(); client?.Close(); } catch { }
        }
    }
}