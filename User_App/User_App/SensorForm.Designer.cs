namespace User_App
{
    partial class SensorForm
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
            this.sensorPicker = new System.Windows.Forms.ComboBox();
            this.chamberPicker = new System.Windows.Forms.ComboBox();
            this.typePicker = new System.Windows.Forms.ComboBox();
            this.registerPicker = new System.Windows.Forms.ComboBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.createNewButton = new System.Windows.Forms.RadioButton();
            this.editExistingButton = new System.Windows.Forms.RadioButton();
            this.sensorNameBox = new System.Windows.Forms.TextBox();
            this.sensorIPBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.scaleBox = new System.Windows.Forms.TextBox();
            this.offsetBox = new System.Windows.Forms.TextBox();
            this.enabledBox = new System.Windows.Forms.CheckBox();
            this.registerLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.chamberLabel = new System.Windows.Forms.Label();
            this.sensorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sensorPicker
            // 
            this.sensorPicker.FormattingEnabled = true;
            this.sensorPicker.Location = new System.Drawing.Point(12, 83);
            this.sensorPicker.Name = "sensorPicker";
            this.sensorPicker.Size = new System.Drawing.Size(121, 21);
            this.sensorPicker.TabIndex = 0;
            this.sensorPicker.SelectedValueChanged += new System.EventHandler(this.sensorPicker_SelectedValueChanged);
            // 
            // chamberPicker
            // 
            this.chamberPicker.FormattingEnabled = true;
            this.chamberPicker.Location = new System.Drawing.Point(151, 83);
            this.chamberPicker.Name = "chamberPicker";
            this.chamberPicker.Size = new System.Drawing.Size(121, 21);
            this.chamberPicker.TabIndex = 1;
            // 
            // typePicker
            // 
            this.typePicker.FormattingEnabled = true;
            this.typePicker.Items.AddRange(new object[] {
            "Temperature",
            "Humidity",
            "Pressure"});
            this.typePicker.Location = new System.Drawing.Point(12, 148);
            this.typePicker.Name = "typePicker";
            this.typePicker.Size = new System.Drawing.Size(121, 21);
            this.typePicker.TabIndex = 2;
            this.typePicker.Text = "Sensor Type";
            this.typePicker.SelectedValueChanged += new System.EventHandler(this.typePicker_SelectedValueChanged);
            // 
            // registerPicker
            // 
            this.registerPicker.FormattingEnabled = true;
            this.registerPicker.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.registerPicker.Location = new System.Drawing.Point(151, 148);
            this.registerPicker.Name = "registerPicker";
            this.registerPicker.Size = new System.Drawing.Size(121, 21);
            this.registerPicker.TabIndex = 3;
            this.registerPicker.Text = "Register/Channel";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(29, 395);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 4;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(182, 395);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // createNewButton
            // 
            this.createNewButton.AutoSize = true;
            this.createNewButton.Location = new System.Drawing.Point(31, 15);
            this.createNewButton.Name = "createNewButton";
            this.createNewButton.Size = new System.Drawing.Size(81, 17);
            this.createNewButton.TabIndex = 6;
            this.createNewButton.TabStop = true;
            this.createNewButton.Text = "Create New";
            this.createNewButton.UseVisualStyleBackColor = true;
            this.createNewButton.CheckedChanged += new System.EventHandler(this.createNewOption_CheckedChanged);
            // 
            // editExistingButton
            // 
            this.editExistingButton.AutoSize = true;
            this.editExistingButton.Location = new System.Drawing.Point(167, 15);
            this.editExistingButton.Name = "editExistingButton";
            this.editExistingButton.Size = new System.Drawing.Size(82, 17);
            this.editExistingButton.TabIndex = 7;
            this.editExistingButton.TabStop = true;
            this.editExistingButton.Text = "Edit Existing";
            this.editExistingButton.UseVisualStyleBackColor = true;
            // 
            // sensorNameBox
            // 
            this.sensorNameBox.Location = new System.Drawing.Point(12, 190);
            this.sensorNameBox.Name = "sensorNameBox";
            this.sensorNameBox.Size = new System.Drawing.Size(121, 20);
            this.sensorNameBox.TabIndex = 8;
            this.sensorNameBox.Text = "Name";
            // 
            // sensorIPBox
            // 
            this.sensorIPBox.Location = new System.Drawing.Point(12, 225);
            this.sensorIPBox.Name = "sensorIPBox";
            this.sensorIPBox.Size = new System.Drawing.Size(121, 20);
            this.sensorIPBox.TabIndex = 9;
            this.sensorIPBox.Text = "IP Address";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(12, 260);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(121, 20);
            this.portBox.TabIndex = 10;
            this.portBox.Text = "Network Port";
            // 
            // scaleBox
            // 
            this.scaleBox.Location = new System.Drawing.Point(12, 296);
            this.scaleBox.Name = "scaleBox";
            this.scaleBox.Size = new System.Drawing.Size(121, 20);
            this.scaleBox.TabIndex = 11;
            this.scaleBox.Text = "Scale";
            // 
            // offsetBox
            // 
            this.offsetBox.Location = new System.Drawing.Point(13, 336);
            this.offsetBox.Name = "offsetBox";
            this.offsetBox.Size = new System.Drawing.Size(120, 20);
            this.offsetBox.TabIndex = 12;
            this.offsetBox.Text = "Offset";
            // 
            // enabledBox
            // 
            this.enabledBox.AutoSize = true;
            this.enabledBox.Location = new System.Drawing.Point(184, 262);
            this.enabledBox.Name = "enabledBox";
            this.enabledBox.Size = new System.Drawing.Size(65, 17);
            this.enabledBox.TabIndex = 13;
            this.enabledBox.Text = "Enabled";
            this.enabledBox.UseVisualStyleBackColor = true;
            // 
            // registerLabel
            // 
            this.registerLabel.AutoSize = true;
            this.registerLabel.Location = new System.Drawing.Point(167, 132);
            this.registerLabel.Name = "registerLabel";
            this.registerLabel.Size = new System.Drawing.Size(90, 13);
            this.registerLabel.TabIndex = 14;
            this.registerLabel.Text = "Register/Channel";
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(37, 132);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(67, 13);
            this.typeLabel.TabIndex = 15;
            this.typeLabel.Text = "Sensor Type";
            // 
            // chamberLabel
            // 
            this.chamberLabel.AutoSize = true;
            this.chamberLabel.Location = new System.Drawing.Point(190, 67);
            this.chamberLabel.Name = "chamberLabel";
            this.chamberLabel.Size = new System.Drawing.Size(49, 13);
            this.chamberLabel.TabIndex = 16;
            this.chamberLabel.Text = "Chamber";
            // 
            // sensorLabel
            // 
            this.sensorLabel.AutoSize = true;
            this.sensorLabel.Location = new System.Drawing.Point(48, 67);
            this.sensorLabel.Name = "sensorLabel";
            this.sensorLabel.Size = new System.Drawing.Size(40, 13);
            this.sensorLabel.TabIndex = 17;
            this.sensorLabel.Text = "Sensor";
            // 
            // SensorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 425);
            this.Controls.Add(this.sensorLabel);
            this.Controls.Add(this.chamberLabel);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.registerLabel);
            this.Controls.Add(this.enabledBox);
            this.Controls.Add(this.offsetBox);
            this.Controls.Add(this.scaleBox);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.sensorIPBox);
            this.Controls.Add(this.sensorNameBox);
            this.Controls.Add(this.editExistingButton);
            this.Controls.Add(this.createNewButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.registerPicker);
            this.Controls.Add(this.typePicker);
            this.Controls.Add(this.chamberPicker);
            this.Controls.Add(this.sensorPicker);
            this.Name = "SensorForm";
            this.Text = "SensorForm";
            this.Load += new System.EventHandler(this.SensorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox sensorPicker;
        private System.Windows.Forms.ComboBox chamberPicker;
        private System.Windows.Forms.ComboBox typePicker;
        private System.Windows.Forms.ComboBox registerPicker;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton createNewButton;
        private System.Windows.Forms.RadioButton editExistingButton;
        private System.Windows.Forms.TextBox sensorNameBox;
        private System.Windows.Forms.TextBox sensorIPBox;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.TextBox scaleBox;
        private System.Windows.Forms.TextBox offsetBox;
        private System.Windows.Forms.CheckBox enabledBox;
        private System.Windows.Forms.Label registerLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label chamberLabel;
        private System.Windows.Forms.Label sensorLabel;
    }
}