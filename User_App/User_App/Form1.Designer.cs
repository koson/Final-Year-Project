namespace User_App
{
    partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.liveChartTab = new System.Windows.Forms.TabPage();
            this.liveChartSplitContainer = new System.Windows.Forms.SplitContainer();
            this.liveChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.liveChartAverage = new System.Windows.Forms.CheckBox();
            this.chamberPickerLabel = new System.Windows.Forms.Label();
            this.liveChartPicker = new System.Windows.Forms.ComboBox();
            this.customChartTab = new System.Windows.Forms.TabPage();
            this.customChartSplitContainer = new System.Windows.Forms.SplitContainer();
            this.customChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.customChartAverage = new System.Windows.Forms.CheckBox();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.customChartPicker = new System.Windows.Forms.ComboBox();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.exportToExcelBtn = new System.Windows.Forms.Button();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingChamberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newChamberMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewSensorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editExistingChamberButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editSensorMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteChamberMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSensorMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tenSecondsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twentySecondsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thirtySecondsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sixtySecondsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneHourRange = new System.Windows.Forms.ToolStripMenuItem();
            this.twoHourRange = new System.Windows.Forms.ToolStripMenuItem();
            this.sixHourRange = new System.Windows.Forms.ToolStripMenuItem();
            this.twelveHourRange = new System.Windows.Forms.ToolStripMenuItem();
            this.twentyFourHourRange = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.liveChartTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveChartSplitContainer)).BeginInit();
            this.liveChartSplitContainer.Panel1.SuspendLayout();
            this.liveChartSplitContainer.Panel2.SuspendLayout();
            this.liveChartSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveChart)).BeginInit();
            this.customChartTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customChartSplitContainer)).BeginInit();
            this.customChartSplitContainer.Panel1.SuspendLayout();
            this.customChartSplitContainer.Panel2.SuspendLayout();
            this.customChartSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customChart)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.liveChartTab);
            this.tabControl1.Controls.Add(this.customChartTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1370, 725);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 2;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // liveChartTab
            // 
            this.liveChartTab.Controls.Add(this.liveChartSplitContainer);
            this.liveChartTab.Location = new System.Drawing.Point(4, 22);
            this.liveChartTab.Margin = new System.Windows.Forms.Padding(5);
            this.liveChartTab.Name = "liveChartTab";
            this.liveChartTab.Size = new System.Drawing.Size(1362, 699);
            this.liveChartTab.TabIndex = 0;
            this.liveChartTab.Text = "Live Chart";
            this.liveChartTab.UseVisualStyleBackColor = true;
            // 
            // liveChartSplitContainer
            // 
            this.liveChartSplitContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.liveChartSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.liveChartSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveChartSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.liveChartSplitContainer.Name = "liveChartSplitContainer";
            // 
            // liveChartSplitContainer.Panel1
            // 
            this.liveChartSplitContainer.Panel1.Controls.Add(this.liveChart);
            this.liveChartSplitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.liveChartSplitContainer.Panel1MinSize = 0;
            // 
            // liveChartSplitContainer.Panel2
            // 
            this.liveChartSplitContainer.Panel2.Controls.Add(this.liveChartAverage);
            this.liveChartSplitContainer.Panel2.Controls.Add(this.chamberPickerLabel);
            this.liveChartSplitContainer.Panel2.Controls.Add(this.liveChartPicker);
            this.liveChartSplitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.liveChartSplitContainer.Panel2MinSize = 0;
            this.liveChartSplitContainer.Size = new System.Drawing.Size(1362, 699);
            this.liveChartSplitContainer.SplitterDistance = 1202;
            this.liveChartSplitContainer.TabIndex = 6;
            // 
            // liveChart
            // 
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Angle = -45;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.MajorGrid.Interval = 20D;
            chartArea1.AxisY.MajorTickMark.Interval = 10D;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY.Minimum = -40D;
            chartArea1.AxisY.Title = "Temperature (C) & Relative Humidity (%)";
            chartArea1.AxisY2.MajorTickMark.Interval = 200D;
            chartArea1.AxisY2.Maximum = 1000D;
            chartArea1.AxisY2.Minimum = 0D;
            chartArea1.AxisY2.Title = "Pressure (mbar) (a)";
            chartArea1.Name = "ChartArea1";
            this.liveChart.ChartAreas.Add(chartArea1);
            this.liveChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.liveChart.Legends.Add(legend1);
            this.liveChart.Location = new System.Drawing.Point(0, 0);
            this.liveChart.Name = "liveChart";
            this.liveChart.Size = new System.Drawing.Size(1200, 697);
            this.liveChart.TabIndex = 1;
            this.liveChart.Text = "chart1";
            // 
            // liveChartAverage
            // 
            this.liveChartAverage.AutoSize = true;
            this.liveChartAverage.Location = new System.Drawing.Point(12, 68);
            this.liveChartAverage.Name = "liveChartAverage";
            this.liveChartAverage.Size = new System.Drawing.Size(120, 17);
            this.liveChartAverage.TabIndex = 10;
            this.liveChartAverage.Text = "Per-Minute Average";
            this.liveChartAverage.UseVisualStyleBackColor = true;
            // 
            // chamberPickerLabel
            // 
            this.chamberPickerLabel.AutoSize = true;
            this.chamberPickerLabel.Location = new System.Drawing.Point(47, 10);
            this.chamberPickerLabel.Name = "chamberPickerLabel";
            this.chamberPickerLabel.Size = new System.Drawing.Size(49, 13);
            this.chamberPickerLabel.TabIndex = 5;
            this.chamberPickerLabel.Text = "Chamber";
            // 
            // liveChartPicker
            // 
            this.liveChartPicker.FormattingEnabled = true;
            this.liveChartPicker.Location = new System.Drawing.Point(12, 26);
            this.liveChartPicker.Name = "liveChartPicker";
            this.liveChartPicker.Size = new System.Drawing.Size(125, 21);
            this.liveChartPicker.TabIndex = 4;
            this.liveChartPicker.SelectedIndexChanged += new System.EventHandler(this.chamberPickerBox_SelectedIndexChanged);
            // 
            // customChartTab
            // 
            this.customChartTab.AutoScroll = true;
            this.customChartTab.Controls.Add(this.customChartSplitContainer);
            this.customChartTab.Location = new System.Drawing.Point(4, 22);
            this.customChartTab.Margin = new System.Windows.Forms.Padding(0);
            this.customChartTab.Name = "customChartTab";
            this.customChartTab.Size = new System.Drawing.Size(1362, 699);
            this.customChartTab.TabIndex = 1;
            this.customChartTab.Text = "Custom Chart";
            this.customChartTab.UseVisualStyleBackColor = true;
            // 
            // customChartSplitContainer
            // 
            this.customChartSplitContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.customChartSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customChartSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customChartSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.customChartSplitContainer.Name = "customChartSplitContainer";
            // 
            // customChartSplitContainer.Panel1
            // 
            this.customChartSplitContainer.Panel1.Controls.Add(this.customChart);
            this.customChartSplitContainer.Panel1MinSize = 900;
            // 
            // customChartSplitContainer.Panel2
            // 
            this.customChartSplitContainer.Panel2.Controls.Add(this.customChartAverage);
            this.customChartSplitContainer.Panel2.Controls.Add(this.endDatePicker);
            this.customChartSplitContainer.Panel2.Controls.Add(this.label1);
            this.customChartSplitContainer.Panel2.Controls.Add(this.startDatePicker);
            this.customChartSplitContainer.Panel2.Controls.Add(this.customChartPicker);
            this.customChartSplitContainer.Panel2.Controls.Add(this.startDateLabel);
            this.customChartSplitContainer.Panel2.Controls.Add(this.updateButton);
            this.customChartSplitContainer.Panel2.Controls.Add(this.endDateLabel);
            this.customChartSplitContainer.Panel2.Controls.Add(this.exportToExcelBtn);
            this.customChartSplitContainer.Panel2MinSize = 100;
            this.customChartSplitContainer.Size = new System.Drawing.Size(1362, 699);
            this.customChartSplitContainer.SplitterDistance = 1164;
            this.customChartSplitContainer.TabIndex = 17;
            // 
            // customChart
            // 
            chartArea2.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisX.IsLabelAutoFit = false;
            chartArea2.AxisX.LabelStyle.Angle = -45;
            chartArea2.AxisX.MajorGrid.Interval = 0D;
            chartArea2.AxisY.Interval = 20D;
            chartArea2.AxisY.IsStartedFromZero = false;
            chartArea2.AxisY.MajorGrid.Interval = 20D;
            chartArea2.AxisY.MajorTickMark.Interval = 20D;
            chartArea2.AxisY.Maximum = 100D;
            chartArea2.AxisY.Minimum = -40D;
            chartArea2.AxisY.Title = "Temperature (C) & Relative Humidity (%)";
            chartArea2.AxisY2.MajorGrid.Enabled = false;
            chartArea2.AxisY2.MajorTickMark.Interval = 200D;
            chartArea2.AxisY2.Maximum = 1000D;
            chartArea2.AxisY2.Minimum = 0D;
            chartArea2.AxisY2.Title = "Pressure (mbar) (a)";
            chartArea2.Name = "ChartArea1";
            this.customChart.ChartAreas.Add(chartArea2);
            this.customChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.customChart.Legends.Add(legend2);
            this.customChart.Location = new System.Drawing.Point(0, 0);
            this.customChart.Name = "customChart";
            this.customChart.Size = new System.Drawing.Size(1162, 697);
            this.customChart.TabIndex = 13;
            this.customChart.Text = "chart1";
            // 
            // customChartAverage
            // 
            this.customChartAverage.AutoSize = true;
            this.customChartAverage.Location = new System.Drawing.Point(29, 186);
            this.customChartAverage.Name = "customChartAverage";
            this.customChartAverage.Size = new System.Drawing.Size(120, 17);
            this.customChartAverage.TabIndex = 18;
            this.customChartAverage.Text = "Per-Minute Average";
            this.customChartAverage.UseVisualStyleBackColor = true;
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "dd:MM:yyyy HH:mm";
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatePicker.Location = new System.Drawing.Point(29, 148);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(121, 20);
            this.endDatePicker.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Chamber";
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "dd:MM:yyyy HH:mm";
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatePicker.Location = new System.Drawing.Point(29, 76);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(121, 20);
            this.startDatePicker.TabIndex = 8;
            // 
            // customChartPicker
            // 
            this.customChartPicker.FormattingEnabled = true;
            this.customChartPicker.Location = new System.Drawing.Point(29, 25);
            this.customChartPicker.Name = "customChartPicker";
            this.customChartPicker.Size = new System.Drawing.Size(121, 21);
            this.customChartPicker.TabIndex = 15;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(56, 60);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(55, 13);
            this.startDateLabel.TabIndex = 10;
            this.startDateLabel.Text = "Start Date";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(45, 226);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 14;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // endDateLabel
            // 
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Location = new System.Drawing.Point(59, 132);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(52, 13);
            this.endDateLabel.TabIndex = 11;
            this.endDateLabel.Text = "End Date";
            // 
            // exportToExcelBtn
            // 
            this.exportToExcelBtn.Location = new System.Drawing.Point(45, 270);
            this.exportToExcelBtn.Name = "exportToExcelBtn";
            this.exportToExcelBtn.Size = new System.Drawing.Size(75, 23);
            this.exportToExcelBtn.TabIndex = 12;
            this.exportToExcelBtn.Text = "Export";
            this.exportToExcelBtn.UseVisualStyleBackColor = true;
            this.exportToExcelBtn.Click += new System.EventHandler(this.exportToExcelBtn_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.existingChamberToolStripMenuItem,
            this.editExistingToolStripMenuItem,
            this.deleteExistingToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // existingChamberToolStripMenuItem
            // 
            this.existingChamberToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newChamberMenuButton,
            this.createNewSensorButton});
            this.existingChamberToolStripMenuItem.Name = "existingChamberToolStripMenuItem";
            this.existingChamberToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.existingChamberToolStripMenuItem.Text = "Create New";
            // 
            // newChamberMenuButton
            // 
            this.newChamberMenuButton.Name = "newChamberMenuButton";
            this.newChamberMenuButton.Size = new System.Drawing.Size(123, 22);
            this.newChamberMenuButton.Text = "Chamber";
            this.newChamberMenuButton.Click += new System.EventHandler(this.newChamberMenuButton_Click);
            // 
            // createNewSensorButton
            // 
            this.createNewSensorButton.Name = "createNewSensorButton";
            this.createNewSensorButton.Size = new System.Drawing.Size(123, 22);
            this.createNewSensorButton.Text = "Sensor";
            this.createNewSensorButton.Click += new System.EventHandler(this.createNewSensorButton_Click);
            // 
            // editExistingToolStripMenuItem
            // 
            this.editExistingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editExistingChamberButton,
            this.editSensorMenuButton});
            this.editExistingToolStripMenuItem.Name = "editExistingToolStripMenuItem";
            this.editExistingToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.editExistingToolStripMenuItem.Text = "Edit Existing";
            // 
            // editExistingChamberButton
            // 
            this.editExistingChamberButton.Name = "editExistingChamberButton";
            this.editExistingChamberButton.Size = new System.Drawing.Size(123, 22);
            this.editExistingChamberButton.Text = "Chamber";
            this.editExistingChamberButton.Click += new System.EventHandler(this.editExistingChamberButton_Click);
            // 
            // editSensorMenuButton
            // 
            this.editSensorMenuButton.Name = "editSensorMenuButton";
            this.editSensorMenuButton.Size = new System.Drawing.Size(123, 22);
            this.editSensorMenuButton.Text = "Sensor";
            this.editSensorMenuButton.Click += new System.EventHandler(this.editSensorMenuButton_Click);
            // 
            // deleteExistingToolStripMenuItem
            // 
            this.deleteExistingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteChamberMenuButton,
            this.deleteSensorMenuButton});
            this.deleteExistingToolStripMenuItem.Name = "deleteExistingToolStripMenuItem";
            this.deleteExistingToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.deleteExistingToolStripMenuItem.Text = "Delete Existing";
            // 
            // deleteChamberMenuButton
            // 
            this.deleteChamberMenuButton.Name = "deleteChamberMenuButton";
            this.deleteChamberMenuButton.Size = new System.Drawing.Size(123, 22);
            this.deleteChamberMenuButton.Text = "Chamber";
            this.deleteChamberMenuButton.Click += new System.EventHandler(this.deleteChamberMenuButton_Click);
            // 
            // deleteSensorMenuButton
            // 
            this.deleteSensorMenuButton.Name = "deleteSensorMenuButton";
            this.deleteSensorMenuButton.Size = new System.Drawing.Size(123, 22);
            this.deleteSensorMenuButton.Text = "Sensor";
            this.deleteSensorMenuButton.Click += new System.EventHandler(this.deleteSensorMenuButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1370, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.liveChartToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // liveChartToolStripMenuItem
            // 
            this.liveChartToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateIntervalToolStripMenuItem1,
            this.rangeToolStripMenuItem});
            this.liveChartToolStripMenuItem.Name = "liveChartToolStripMenuItem";
            this.liveChartToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.liveChartToolStripMenuItem.Text = "Live Chart";
            // 
            // updateIntervalToolStripMenuItem1
            // 
            this.updateIntervalToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tenSecondsItem,
            this.twentySecondsItem,
            this.thirtySecondsItem,
            this.sixtySecondsItem});
            this.updateIntervalToolStripMenuItem1.Name = "updateIntervalToolStripMenuItem1";
            this.updateIntervalToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.updateIntervalToolStripMenuItem1.Text = "Update Interval";
            // 
            // tenSecondsItem
            // 
            this.tenSecondsItem.Name = "tenSecondsItem";
            this.tenSecondsItem.Size = new System.Drawing.Size(133, 22);
            this.tenSecondsItem.Text = "10 Seconds";
            this.tenSecondsItem.Click += new System.EventHandler(this.tenSecondsItem_Click);
            // 
            // twentySecondsItem
            // 
            this.twentySecondsItem.Name = "twentySecondsItem";
            this.twentySecondsItem.Size = new System.Drawing.Size(133, 22);
            this.twentySecondsItem.Text = "20 Seconds";
            this.twentySecondsItem.Click += new System.EventHandler(this.twentySecondsItem_Click);
            // 
            // thirtySecondsItem
            // 
            this.thirtySecondsItem.Name = "thirtySecondsItem";
            this.thirtySecondsItem.Size = new System.Drawing.Size(133, 22);
            this.thirtySecondsItem.Text = "30 Seconds";
            this.thirtySecondsItem.Click += new System.EventHandler(this.thirtySecondsItem_Click);
            // 
            // sixtySecondsItem
            // 
            this.sixtySecondsItem.Name = "sixtySecondsItem";
            this.sixtySecondsItem.Size = new System.Drawing.Size(133, 22);
            this.sixtySecondsItem.Text = "60 Seconds";
            this.sixtySecondsItem.Click += new System.EventHandler(this.sixtySecondsItem_Click);
            // 
            // rangeToolStripMenuItem
            // 
            this.rangeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oneHourRange,
            this.twoHourRange,
            this.sixHourRange,
            this.twelveHourRange,
            this.twentyFourHourRange});
            this.rangeToolStripMenuItem.Name = "rangeToolStripMenuItem";
            this.rangeToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.rangeToolStripMenuItem.Text = "Range";
            // 
            // oneHourRange
            // 
            this.oneHourRange.Name = "oneHourRange";
            this.oneHourRange.Size = new System.Drawing.Size(116, 22);
            this.oneHourRange.Text = "1 Hour";
            this.oneHourRange.Click += new System.EventHandler(this.oneHourRange_Click);
            // 
            // twoHourRange
            // 
            this.twoHourRange.Name = "twoHourRange";
            this.twoHourRange.Size = new System.Drawing.Size(116, 22);
            this.twoHourRange.Text = "2 Hour";
            this.twoHourRange.Click += new System.EventHandler(this.twoHourRange_Click);
            // 
            // sixHourRange
            // 
            this.sixHourRange.Name = "sixHourRange";
            this.sixHourRange.Size = new System.Drawing.Size(116, 22);
            this.sixHourRange.Text = "6 Hour";
            this.sixHourRange.Click += new System.EventHandler(this.sixHourRange_Click);
            // 
            // twelveHourRange
            // 
            this.twelveHourRange.Name = "twelveHourRange";
            this.twelveHourRange.Size = new System.Drawing.Size(116, 22);
            this.twelveHourRange.Text = "12 Hour";
            this.twelveHourRange.Click += new System.EventHandler(this.twelveHourRange_Click);
            // 
            // twentyFourHourRange
            // 
            this.twentyFourHourRange.Name = "twentyFourHourRange";
            this.twentyFourHourRange.Size = new System.Drawing.Size(116, 22);
            this.twentyFourHourRange.Text = "24 Hour";
            this.twentyFourHourRange.Click += new System.EventHandler(this.twentyFourHourRange_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sensorcom";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.liveChartTab.ResumeLayout(false);
            this.liveChartSplitContainer.Panel1.ResumeLayout(false);
            this.liveChartSplitContainer.Panel2.ResumeLayout(false);
            this.liveChartSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveChartSplitContainer)).EndInit();
            this.liveChartSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.liveChart)).EndInit();
            this.customChartTab.ResumeLayout(false);
            this.customChartSplitContainer.Panel1.ResumeLayout(false);
            this.customChartSplitContainer.Panel2.ResumeLayout(false);
            this.customChartSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customChartSplitContainer)).EndInit();
            this.customChartSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customChart)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage liveChartTab;
        private System.Windows.Forms.TabPage customChartTab;
        private System.Windows.Forms.Label chamberPickerLabel;
        private System.Windows.Forms.ComboBox liveChartPicker;
        private System.Windows.Forms.Button exportToExcelBtn;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart customChart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox customChartPicker;
        private System.Windows.Forms.DataVisualization.Charting.Chart liveChart;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingChamberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newChamberMenuButton;
        private System.Windows.Forms.ToolStripMenuItem createNewSensorButton;
        private System.Windows.Forms.ToolStripMenuItem editExistingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editExistingChamberButton;
        private System.Windows.Forms.ToolStripMenuItem editSensorMenuButton;
        private System.Windows.Forms.ToolStripMenuItem deleteExistingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteChamberMenuButton;
        private System.Windows.Forms.ToolStripMenuItem deleteSensorMenuButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liveChartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tenSecondsItem;
        private System.Windows.Forms.ToolStripMenuItem twentySecondsItem;
        private System.Windows.Forms.ToolStripMenuItem thirtySecondsItem;
        private System.Windows.Forms.ToolStripMenuItem sixtySecondsItem;
        private System.Windows.Forms.ToolStripMenuItem rangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oneHourRange;
        private System.Windows.Forms.ToolStripMenuItem twoHourRange;
        private System.Windows.Forms.ToolStripMenuItem sixHourRange;
        private System.Windows.Forms.ToolStripMenuItem twelveHourRange;
        private System.Windows.Forms.ToolStripMenuItem twentyFourHourRange;
        private System.Windows.Forms.SplitContainer liveChartSplitContainer;
        private System.Windows.Forms.SplitContainer customChartSplitContainer;
        private System.Windows.Forms.CheckBox liveChartAverage;
        private System.Windows.Forms.CheckBox customChartAverage;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}

