//  Copyright (C) 2019 Ben Staniford
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace AutoHostfileSettings
{
    partial class AutoHostfileSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoHostfileSettingsForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtFriendlyName = new System.Windows.Forms.TextBox();
            this.lblFriendlyName = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtSharedKey = new System.Windows.Forms.TextBox();
            this.lblSharedKey = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.linkLblSupport = new System.Windows.Forms.LinkLabel();
            this.linkLblUpdates = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(25, 237);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(146, 42);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(25, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(190, 190);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(177, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(146, 42);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(236, 28);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(158, 20);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Auto hostfile settings";
            // 
            // txtFriendlyName
            // 
            this.txtFriendlyName.Location = new System.Drawing.Point(324, 63);
            this.txtFriendlyName.Name = "txtFriendlyName";
            this.txtFriendlyName.Size = new System.Drawing.Size(239, 20);
            this.txtFriendlyName.TabIndex = 3;
            // 
            // lblFriendlyName
            // 
            this.lblFriendlyName.AutoSize = true;
            this.lblFriendlyName.Location = new System.Drawing.Point(237, 65);
            this.lblFriendlyName.Name = "lblFriendlyName";
            this.lblFriendlyName.Size = new System.Drawing.Size(74, 13);
            this.lblFriendlyName.TabIndex = 4;
            this.lblFriendlyName.Text = "Friendly Name";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(324, 89);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(239, 20);
            this.txtPort.TabIndex = 3;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(237, 93);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port";
            // 
            // txtSharedKey
            // 
            this.txtSharedKey.Location = new System.Drawing.Point(324, 115);
            this.txtSharedKey.Name = "txtSharedKey";
            this.txtSharedKey.Size = new System.Drawing.Size(239, 20);
            this.txtSharedKey.TabIndex = 3;
            // 
            // lblSharedKey
            // 
            this.lblSharedKey.AutoSize = true;
            this.lblSharedKey.Location = new System.Drawing.Point(237, 118);
            this.lblSharedKey.Name = "lblSharedKey";
            this.lblSharedKey.Size = new System.Drawing.Size(62, 13);
            this.lblSharedKey.TabIndex = 4;
            this.lblSharedKey.Text = "Shared Key";
            // 
            // lblHelp
            // 
            this.lblHelp.Location = new System.Drawing.Point(237, 157);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(326, 61);
            this.lblHelp.TabIndex = 5;
            this.lblHelp.Text = resources.GetString("lblHelp.Text");
            // 
            // linkLblSupport
            // 
            this.linkLblSupport.AutoSize = true;
            this.linkLblSupport.Location = new System.Drawing.Point(521, 252);
            this.linkLblSupport.Name = "linkLblSupport";
            this.linkLblSupport.Size = new System.Drawing.Size(42, 13);
            this.linkLblSupport.TabIndex = 6;
            this.linkLblSupport.TabStop = true;
            this.linkLblSupport.Text = "Donate";
            this.linkLblSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblSupport_LinkClicked);
            // 
            // linkLblUpdates
            // 
            this.linkLblUpdates.AutoSize = true;
            this.linkLblUpdates.Location = new System.Drawing.Point(468, 252);
            this.linkLblUpdates.Name = "linkLblUpdates";
            this.linkLblUpdates.Size = new System.Drawing.Size(47, 13);
            this.linkLblUpdates.TabIndex = 6;
            this.linkLblUpdates.TabStop = true;
            this.linkLblUpdates.Text = "Updates";
            this.linkLblUpdates.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblUpdates_LinkClicked);
            // 
            // AutoHostfileSettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(591, 303);
            this.Controls.Add(this.linkLblUpdates);
            this.Controls.Add(this.linkLblSupport);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.lblSharedKey);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblFriendlyName);
            this.Controls.Add(this.txtSharedKey);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtFriendlyName);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoHostfileSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Hostfile Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AutoHostfileSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtFriendlyName;
        private System.Windows.Forms.Label lblFriendlyName;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtSharedKey;
        private System.Windows.Forms.Label lblSharedKey;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.LinkLabel linkLblSupport;
        private System.Windows.Forms.LinkLabel linkLblUpdates;
    }
}

