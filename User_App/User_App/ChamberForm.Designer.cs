namespace User_App
{
    partial class ChamberForm
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
            this.chamberSubmitButton = new System.Windows.Forms.Button();
            this.newChamberName = new System.Windows.Forms.TextBox();
            this.newChamberDescription = new System.Windows.Forms.TextBox();
            this.newChamberOption = new System.Windows.Forms.RadioButton();
            this.existingChamberOption = new System.Windows.Forms.RadioButton();
            this.chamberIDPicker = new System.Windows.Forms.ComboBox();
            this.chamberCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chamberSubmitButton
            // 
            this.chamberSubmitButton.Location = new System.Drawing.Point(20, 380);
            this.chamberSubmitButton.Name = "chamberSubmitButton";
            this.chamberSubmitButton.Size = new System.Drawing.Size(75, 23);
            this.chamberSubmitButton.TabIndex = 0;
            this.chamberSubmitButton.Text = "OK";
            this.chamberSubmitButton.UseVisualStyleBackColor = true;
            this.chamberSubmitButton.Click += new System.EventHandler(this.chamberSubmitButton_Click);
            // 
            // newChamberName
            // 
            this.newChamberName.Location = new System.Drawing.Point(13, 165);
            this.newChamberName.Name = "newChamberName";
            this.newChamberName.Size = new System.Drawing.Size(100, 20);
            this.newChamberName.TabIndex = 1;
            this.newChamberName.Text = "Name";
            // 
            // newChamberDescription
            // 
            this.newChamberDescription.Location = new System.Drawing.Point(13, 208);
            this.newChamberDescription.Name = "newChamberDescription";
            this.newChamberDescription.Size = new System.Drawing.Size(100, 20);
            this.newChamberDescription.TabIndex = 2;
            this.newChamberDescription.Text = "Description";
            // 
            // newChamberOption
            // 
            this.newChamberOption.AutoSize = true;
            this.newChamberOption.Location = new System.Drawing.Point(13, 39);
            this.newChamberOption.Name = "newChamberOption";
            this.newChamberOption.Size = new System.Drawing.Size(81, 17);
            this.newChamberOption.TabIndex = 3;
            this.newChamberOption.TabStop = true;
            this.newChamberOption.Text = "Create New";
            this.newChamberOption.UseVisualStyleBackColor = true;
            this.newChamberOption.CheckedChanged += new System.EventHandler(this.newChamberOption_CheckedChanged);
            // 
            // existingChamberOption
            // 
            this.existingChamberOption.AutoSize = true;
            this.existingChamberOption.Location = new System.Drawing.Point(13, 63);
            this.existingChamberOption.Name = "existingChamberOption";
            this.existingChamberOption.Size = new System.Drawing.Size(82, 17);
            this.existingChamberOption.TabIndex = 4;
            this.existingChamberOption.TabStop = true;
            this.existingChamberOption.Text = "Edit Existing";
            this.existingChamberOption.UseVisualStyleBackColor = true;
            // 
            // chamberIDPicker
            // 
            this.chamberIDPicker.FormattingEnabled = true;
            this.chamberIDPicker.Location = new System.Drawing.Point(143, 39);
            this.chamberIDPicker.Name = "chamberIDPicker";
            this.chamberIDPicker.Size = new System.Drawing.Size(121, 21);
            this.chamberIDPicker.TabIndex = 5;
            // 
            // chamberCancelButton
            // 
            this.chamberCancelButton.Location = new System.Drawing.Point(179, 380);
            this.chamberCancelButton.Name = "chamberCancelButton";
            this.chamberCancelButton.Size = new System.Drawing.Size(75, 23);
            this.chamberCancelButton.TabIndex = 6;
            this.chamberCancelButton.Text = "Cancel";
            this.chamberCancelButton.UseVisualStyleBackColor = true;
            // 
            // ChamberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 415);
            this.Controls.Add(this.chamberCancelButton);
            this.Controls.Add(this.chamberIDPicker);
            this.Controls.Add(this.existingChamberOption);
            this.Controls.Add(this.newChamberOption);
            this.Controls.Add(this.newChamberDescription);
            this.Controls.Add(this.newChamberName);
            this.Controls.Add(this.chamberSubmitButton);
            this.Name = "ChamberForm";
            this.Text = "ChamberForm";
            this.Load += new System.EventHandler(this.ChamberForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button chamberSubmitButton;
        private System.Windows.Forms.TextBox newChamberName;
        private System.Windows.Forms.TextBox newChamberDescription;
        private System.Windows.Forms.RadioButton newChamberOption;
        private System.Windows.Forms.RadioButton existingChamberOption;
        private System.Windows.Forms.ComboBox chamberIDPicker;
        private System.Windows.Forms.Button chamberCancelButton;
    }
}