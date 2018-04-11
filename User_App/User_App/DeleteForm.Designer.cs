namespace User_App
{
    partial class DeleteForm
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
            this.submitButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.deleteChamberOption = new System.Windows.Forms.RadioButton();
            this.deleteSensorOption = new System.Windows.Forms.RadioButton();
            this.toDeletePicker = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(21, 87);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 0;
            this.submitButton.Text = "OK";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(150, 87);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // deleteChamberOption
            // 
            this.deleteChamberOption.AutoSize = true;
            this.deleteChamberOption.Location = new System.Drawing.Point(21, 13);
            this.deleteChamberOption.Name = "deleteChamberOption";
            this.deleteChamberOption.Size = new System.Drawing.Size(67, 17);
            this.deleteChamberOption.TabIndex = 2;
            this.deleteChamberOption.TabStop = true;
            this.deleteChamberOption.Text = "Chamber";
            this.deleteChamberOption.UseVisualStyleBackColor = true;
            this.deleteChamberOption.CheckedChanged += new System.EventHandler(this.deleteChamberOption_CheckedChanged);
            // 
            // deleteSensorOption
            // 
            this.deleteSensorOption.AutoSize = true;
            this.deleteSensorOption.Location = new System.Drawing.Point(167, 12);
            this.deleteSensorOption.Name = "deleteSensorOption";
            this.deleteSensorOption.Size = new System.Drawing.Size(58, 17);
            this.deleteSensorOption.TabIndex = 3;
            this.deleteSensorOption.TabStop = true;
            this.deleteSensorOption.Text = "Sensor";
            this.deleteSensorOption.UseVisualStyleBackColor = true;
            // 
            // toDeletePicker
            // 
            this.toDeletePicker.FormattingEnabled = true;
            this.toDeletePicker.Location = new System.Drawing.Point(62, 47);
            this.toDeletePicker.Name = "toDeletePicker";
            this.toDeletePicker.Size = new System.Drawing.Size(121, 21);
            this.toDeletePicker.TabIndex = 4;
            // 
            // DeleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 131);
            this.Controls.Add(this.toDeletePicker);
            this.Controls.Add(this.deleteSensorOption);
            this.Controls.Add(this.deleteChamberOption);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.submitButton);
            this.Name = "DeleteForm";
            this.Text = "DeleteForm";
            this.Load += new System.EventHandler(this.DeleteForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton deleteChamberOption;
        private System.Windows.Forms.RadioButton deleteSensorOption;
        private System.Windows.Forms.ComboBox toDeletePicker;
    }
}