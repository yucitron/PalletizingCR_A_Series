namespace CSharpTcpDemo
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
            this.label1 = new System.Windows.Forms.Label();
            this.pltbtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainbtn = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.boxbtn = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.enbbtn = new System.Windows.Forms.Button();
            this.rstbtn = new System.Windows.Forms.Button();
            this.clrErrbtn = new System.Windows.Forms.Button();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.richTextBoxErrInfo = new System.Windows.Forms.RichTextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(673, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ana Sayfa";
            // 
            // pltbtn
            // 
            this.pltbtn.Location = new System.Drawing.Point(13, 150);
            this.pltbtn.Name = "pltbtn";
            this.pltbtn.Size = new System.Drawing.Size(121, 87);
            this.pltbtn.TabIndex = 1;
            this.pltbtn.Text = "Palletizing";
            this.pltbtn.UseVisualStyleBackColor = true;
            this.pltbtn.Click += new System.EventHandler(this.pltbtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mainbtn);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.boxbtn);
            this.panel1.Controls.Add(this.pltbtn);
            this.panel1.Location = new System.Drawing.Point(12, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(149, 1003);
            this.panel1.TabIndex = 6;
            // 
            // mainbtn
            // 
            this.mainbtn.Location = new System.Drawing.Point(14, 24);
            this.mainbtn.Name = "mainbtn";
            this.mainbtn.Size = new System.Drawing.Size(121, 87);
            this.mainbtn.TabIndex = 6;
            this.mainbtn.Text = "MainPage";
            this.mainbtn.UseVisualStyleBackColor = true;
            this.mainbtn.Click += new System.EventHandler(this.mainbtn_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(13, 752);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(121, 87);
            this.button5.TabIndex = 5;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 603);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(121, 87);
            this.button4.TabIndex = 4;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 452);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 87);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // boxbtn
            // 
            this.boxbtn.Location = new System.Drawing.Point(13, 295);
            this.boxbtn.Name = "boxbtn";
            this.boxbtn.Size = new System.Drawing.Size(121, 87);
            this.boxbtn.TabIndex = 2;
            this.boxbtn.Text = "Boxes";
            this.boxbtn.UseVisualStyleBackColor = true;
            this.boxbtn.Click += new System.EventHandler(this.boxbtn_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(274, 72);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(182, 61);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // enbbtn
            // 
            this.enbbtn.Location = new System.Drawing.Point(615, 75);
            this.enbbtn.Margin = new System.Windows.Forms.Padding(5);
            this.enbbtn.Name = "enbbtn";
            this.enbbtn.Size = new System.Drawing.Size(182, 61);
            this.enbbtn.TabIndex = 8;
            this.enbbtn.Text = "Enable";
            this.enbbtn.UseVisualStyleBackColor = true;
            this.enbbtn.Click += new System.EventHandler(this.enbbtn_Click);
            // 
            // rstbtn
            // 
            this.rstbtn.Location = new System.Drawing.Point(905, 72);
            this.rstbtn.Margin = new System.Windows.Forms.Padding(5);
            this.rstbtn.Name = "rstbtn";
            this.rstbtn.Size = new System.Drawing.Size(182, 61);
            this.rstbtn.TabIndex = 9;
            this.rstbtn.Text = "Reset";
            this.rstbtn.UseVisualStyleBackColor = true;
            this.rstbtn.Click += new System.EventHandler(this.rstbtn_Click);
            // 
            // clrErrbtn
            // 
            this.clrErrbtn.Location = new System.Drawing.Point(1166, 72);
            this.clrErrbtn.Margin = new System.Windows.Forms.Padding(5);
            this.clrErrbtn.Name = "clrErrbtn";
            this.clrErrbtn.Size = new System.Drawing.Size(182, 61);
            this.clrErrbtn.TabIndex = 10;
            this.clrErrbtn.Text = "Clear Error";
            this.clrErrbtn.UseVisualStyleBackColor = true;
            this.clrErrbtn.Click += new System.EventHandler(this.clrErrbtn_Click);
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.richTextBoxLog);
            this.groupBoxLog.Location = new System.Drawing.Point(983, 186);
            this.groupBoxLog.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Padding = new System.Windows.Forms.Padding(5);
            this.groupBoxLog.Size = new System.Drawing.Size(365, 467);
            this.groupBoxLog.TabIndex = 11;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "Log";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(10, 30);
            this.richTextBoxLog.Margin = new System.Windows.Forms.Padding(5);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(341, 424);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
            // 
            // richTextBoxErrInfo
            // 
            this.richTextBoxErrInfo.Location = new System.Drawing.Point(736, 221);
            this.richTextBoxErrInfo.Margin = new System.Windows.Forms.Padding(5);
            this.richTextBoxErrInfo.Name = "richTextBoxErrInfo";
            this.richTextBoxErrInfo.Size = new System.Drawing.Size(225, 328);
            this.richTextBoxErrInfo.TabIndex = 4;
            this.richTextBoxErrInfo.Text = "";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(732, 198);
            this.label26.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(76, 20);
            this.label26.TabIndex = 3;
            this.label26.Text = "Error Info";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(554, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 46);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1398, 1024);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBoxErrInfo);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.clrErrbtn);
            this.Controls.Add(this.rstbtn);
            this.Controls.Add(this.enbbtn);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.groupBoxLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button pltbtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button boxbtn;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button enbbtn;
        private System.Windows.Forms.Button rstbtn;
        private System.Windows.Forms.Button clrErrbtn;
        private System.Windows.Forms.GroupBox groupBoxLog;
        public System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.RichTextBox richTextBoxErrInfo;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button mainbtn;
        private System.Windows.Forms.Button button1;
    }
}