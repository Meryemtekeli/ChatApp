namespace ChatClient
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel pnlChat;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckBox chkDarkMode;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlChat = new System.Windows.Forms.FlowLayoutPanel();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.chkDarkMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // pnlChat
            // 
            this.pnlChat.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlChat.WrapContents = false;
            this.pnlChat.AutoScroll = true;
            this.pnlChat.Location = new System.Drawing.Point(10, 50);
            this.pnlChat.Size = new System.Drawing.Size(380, 450);
            this.pnlChat.BackColor = System.Drawing.Color.White;
            this.pnlChat.Name = "pnlChat";
            // 
            // txtMessages
            // 
            this.txtMessages.Location = new System.Drawing.Point(10, 510);
            this.txtMessages.Size = new System.Drawing.Size(300, 22);
            this.txtMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessages_KeyDown);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(320, 508);
            this.btnSend.Size = new System.Drawing.Size(70, 25);
            this.btnSend.Text = "Gönder";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(10, 15);
            this.lblStatus.AutoSize = true;
            this.lblStatus.Text = "Bağlı değil";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(100, 12);
            this.txtUsername.Size = new System.Drawing.Size(100, 22);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(210, 10);
            this.btnConnect.Size = new System.Drawing.Size(70, 25);
            this.btnConnect.Text = "Bağlan";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // chkDarkMode
            // 
            this.chkDarkMode.Location = new System.Drawing.Point(300, 14);
            this.chkDarkMode.Text = "Karanlık";
            this.chkDarkMode.CheckedChanged += new System.EventHandler(this.chkDarkMode_CheckedChanged);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(400, 550);
            this.Controls.Add(this.chkDarkMode);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.pnlChat);
            this.Name = "Form1";
            this.Text = "Chat Client";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
