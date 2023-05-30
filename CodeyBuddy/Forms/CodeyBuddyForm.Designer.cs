namespace CodeyBuddy.Forms
{
    partial class CodeyBuddyForm
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
            this.lblUserInput = new System.Windows.Forms.Label();
            this.txtUserInput = new System.Windows.Forms.TextBox();
            this.txtResposne = new System.Windows.Forms.TextBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.btnAsk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbBusyIndicator = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbBusyIndicator)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUserInput
            // 
            this.lblUserInput.AutoSize = true;
            this.lblUserInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserInput.Location = new System.Drawing.Point(59, 31);
            this.lblUserInput.Name = "lblUserInput";
            this.lblUserInput.Size = new System.Drawing.Size(81, 20);
            this.lblUserInput.TabIndex = 0;
            this.lblUserInput.Text = "UserInput";
            // 
            // txtUserInput
            // 
            this.txtUserInput.Location = new System.Drawing.Point(62, 63);
            this.txtUserInput.Multiline = true;
            this.txtUserInput.Name = "txtUserInput";
            this.txtUserInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUserInput.Size = new System.Drawing.Size(656, 129);
            this.txtUserInput.TabIndex = 1;
            // 
            // txtResposne
            // 
            this.txtResposne.Location = new System.Drawing.Point(62, 239);
            this.txtResposne.Multiline = true;
            this.txtResposne.Name = "txtResposne";
            this.txtResposne.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResposne.Size = new System.Drawing.Size(656, 149);
            this.txtResposne.TabIndex = 2;
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResponse.Location = new System.Drawing.Point(59, 211);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(84, 20);
            this.lblResponse.TabIndex = 3;
            this.lblResponse.Text = "Resposne";
            // 
            // btnAsk
            // 
            this.btnAsk.Location = new System.Drawing.Point(217, 405);
            this.btnAsk.Name = "btnAsk";
            this.btnAsk.Size = new System.Drawing.Size(120, 42);
            this.btnAsk.TabIndex = 4;
            this.btnAsk.Text = "Ask";
            this.btnAsk.UseVisualStyleBackColor = true;
            this.btnAsk.Click += new System.EventHandler(this.btnAsk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(403, 405);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 42);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbBusyIndicator
            // 
            this.pbBusyIndicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBusyIndicator.Image = global::CodeyBuddy.Properties.Resources.busyIndicator;
            this.pbBusyIndicator.Location = new System.Drawing.Point(302, 165);
            this.pbBusyIndicator.Name = "pbBusyIndicator";
            this.pbBusyIndicator.Size = new System.Drawing.Size(172, 118);
            this.pbBusyIndicator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbBusyIndicator.TabIndex = 2;
            this.pbBusyIndicator.TabStop = false;
            this.pbBusyIndicator.Visible = false;
            // 
            // CodeyBuddyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 480);
            this.Controls.Add(this.pbBusyIndicator);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAsk);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.txtResposne);
            this.Controls.Add(this.txtUserInput);
            this.Controls.Add(this.lblUserInput);
            this.Name = "CodeyBuddyForm";
            this.Text = "CodeyBuddyForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeyBuddyForm_FormClosing);
            this.Load += new System.EventHandler(this.CodeyBuddyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbBusyIndicator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserInput;
        private System.Windows.Forms.TextBox txtUserInput;
        private System.Windows.Forms.TextBox txtResposne;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.Button btnAsk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pbBusyIndicator;
    }
}