namespace JordvarmeController
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.XuWeBrowser = new System.Windows.Forms.WebBrowser();
            this.XuLogin = new System.Windows.Forms.Button();
            this.XuSchema = new System.Windows.Forms.Button();
            this.XuAnalyzeDom = new System.Windows.Forms.Button();
            this.XuTimer = new System.Windows.Forms.Timer(this.components);
            this.XuTimer1 = new System.Windows.Forms.Button();
            this.XuStop = new System.Windows.Forms.Button();
            this.XuDelay = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.XuOffset = new System.Windows.Forms.TextBox();
            this.XuThreshold = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.XuAmplificationDown = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.XuAmplificationUp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.XuSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // XuWeBrowser
            // 
            this.XuWeBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XuWeBrowser.Location = new System.Drawing.Point(0, 0);
            this.XuWeBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.XuWeBrowser.Name = "XuWeBrowser";
            this.XuWeBrowser.Size = new System.Drawing.Size(1067, 874);
            this.XuWeBrowser.TabIndex = 0;
            this.XuWeBrowser.Url = new System.Uri("https://cmi.ta.co.at/webi/CMI004096/schema.html#1", System.UriKind.Absolute);
            this.XuWeBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.XuWeBrowser_DocumentCompleted);
            // 
            // XuLogin
            // 
            this.XuLogin.Location = new System.Drawing.Point(25, 27);
            this.XuLogin.Name = "XuLogin";
            this.XuLogin.Size = new System.Drawing.Size(75, 23);
            this.XuLogin.TabIndex = 1;
            this.XuLogin.Text = "Login";
            this.XuLogin.UseVisualStyleBackColor = true;
            this.XuLogin.Click += new System.EventHandler(this.XuLogin_Click);
            // 
            // XuSchema
            // 
            this.XuSchema.Location = new System.Drawing.Point(25, 93);
            this.XuSchema.Name = "XuSchema";
            this.XuSchema.Size = new System.Drawing.Size(75, 23);
            this.XuSchema.TabIndex = 2;
            this.XuSchema.Text = "Schema";
            this.XuSchema.UseVisualStyleBackColor = true;
            this.XuSchema.Click += new System.EventHandler(this.XuSchema_Click);
            // 
            // XuAnalyzeDom
            // 
            this.XuAnalyzeDom.Location = new System.Drawing.Point(25, 163);
            this.XuAnalyzeDom.Name = "XuAnalyzeDom";
            this.XuAnalyzeDom.Size = new System.Drawing.Size(75, 23);
            this.XuAnalyzeDom.TabIndex = 3;
            this.XuAnalyzeDom.Text = "DOM";
            this.XuAnalyzeDom.UseVisualStyleBackColor = true;
            this.XuAnalyzeDom.Click += new System.EventHandler(this.XuAnalyzeDom_Click);
            // 
            // XuTimer
            // 
            this.XuTimer.Interval = 2000;
            this.XuTimer.Tick += new System.EventHandler(this.XuTimer_Tick);
            // 
            // XuTimer1
            // 
            this.XuTimer1.Location = new System.Drawing.Point(25, 239);
            this.XuTimer1.Name = "XuTimer1";
            this.XuTimer1.Size = new System.Drawing.Size(75, 23);
            this.XuTimer1.TabIndex = 4;
            this.XuTimer1.Text = "Timer";
            this.XuTimer1.UseVisualStyleBackColor = true;
            this.XuTimer1.Click += new System.EventHandler(this.XuTimer1_Click);
            // 
            // XuStop
            // 
            this.XuStop.Location = new System.Drawing.Point(25, 300);
            this.XuStop.Name = "XuStop";
            this.XuStop.Size = new System.Drawing.Size(75, 23);
            this.XuStop.TabIndex = 5;
            this.XuStop.Text = "Stop T";
            this.XuStop.UseVisualStyleBackColor = true;
            this.XuStop.Click += new System.EventHandler(this.XuStop_Click);
            // 
            // XuDelay
            // 
            this.XuDelay.Interval = 20000;
            this.XuDelay.Tick += new System.EventHandler(this.XuDelay_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 539);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Offsett";
            // 
            // XuOffset
            // 
            this.XuOffset.Location = new System.Drawing.Point(149, 536);
            this.XuOffset.Name = "XuOffset";
            this.XuOffset.Size = new System.Drawing.Size(100, 20);
            this.XuOffset.TabIndex = 7;
            this.XuOffset.Text = "0,05";
            // 
            // XuThreshold
            // 
            this.XuThreshold.Location = new System.Drawing.Point(147, 574);
            this.XuThreshold.Name = "XuThreshold";
            this.XuThreshold.Size = new System.Drawing.Size(100, 20);
            this.XuThreshold.TabIndex = 9;
            this.XuThreshold.Text = "0,011";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 577);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Regulerings tærskel";
            // 
            // XuAmplificationDown
            // 
            this.XuAmplificationDown.Location = new System.Drawing.Point(146, 644);
            this.XuAmplificationDown.Name = "XuAmplificationDown";
            this.XuAmplificationDown.Size = new System.Drawing.Size(100, 20);
            this.XuAmplificationDown.TabIndex = 11;
            this.XuAmplificationDown.Text = "100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 647);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Forstærkning ned";
            // 
            // XuAmplificationUp
            // 
            this.XuAmplificationUp.Location = new System.Drawing.Point(146, 609);
            this.XuAmplificationUp.Name = "XuAmplificationUp";
            this.XuAmplificationUp.Size = new System.Drawing.Size(100, 20);
            this.XuAmplificationUp.TabIndex = 13;
            this.XuAmplificationUp.Text = "125";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 612);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Forstærkning op";
            // 
            // XuSave
            // 
            this.XuSave.Location = new System.Drawing.Point(146, 700);
            this.XuSave.Name = "XuSave";
            this.XuSave.Size = new System.Drawing.Size(75, 23);
            this.XuSave.TabIndex = 14;
            this.XuSave.Text = "Save";
            this.XuSave.UseVisualStyleBackColor = true;
            this.XuSave.Click += new System.EventHandler(this.XuSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 874);
            this.Controls.Add(this.XuSave);
            this.Controls.Add(this.XuAmplificationUp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.XuAmplificationDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.XuThreshold);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.XuOffset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.XuStop);
            this.Controls.Add(this.XuTimer1);
            this.Controls.Add(this.XuAnalyzeDom);
            this.Controls.Add(this.XuSchema);
            this.Controls.Add(this.XuLogin);
            this.Controls.Add(this.XuWeBrowser);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser XuWeBrowser;
        private System.Windows.Forms.Button XuLogin;
        private System.Windows.Forms.Button XuSchema;
        private System.Windows.Forms.Button XuAnalyzeDom;
        private System.Windows.Forms.Timer XuTimer;
        private System.Windows.Forms.Button XuTimer1;
        private System.Windows.Forms.Button XuStop;
        private System.Windows.Forms.Timer XuDelay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox XuOffset;
        private System.Windows.Forms.TextBox XuThreshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox XuAmplificationDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox XuAmplificationUp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button XuSave;
    }
}

