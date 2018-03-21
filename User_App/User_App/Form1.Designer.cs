namespace User_App
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.liveChartTab = new System.Windows.Forms.TabPage();
            this.liveChartSplitContainer = new System.Windows.Forms.SplitContainer();
            this.liveChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.chamberPickerLabel = new System.Windows.Forms.Label();
            this.chamberPickerBox = new System.Windows.Forms.ComboBox();
            this.customChartTab = new System.Windows.Forms.TabPage();
            this.customChartSplitContainer = new System.Windows.Forms.SplitContainer();
            this.customChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.exportToExcelBtn = new System.Windows.Forms.Button();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingChamberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chamberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chamberToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sensorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chamberToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sensorToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondsToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.rangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hourToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hourToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.hourToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hourToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.debugBox = new System.Windows.Forms.RichTextBox();
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
            this.tabControl1.Size = new System.Drawing.Size(1664, 782);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 2;
            // 
            // liveChartTab
            // 
            this.liveChartTab.Controls.Add(this.liveChartSplitContainer);
            this.liveChartTab.Location = new System.Drawing.Point(4, 22);
            this.liveChartTab.Margin = new System.Windows.Forms.Padding(5);
            this.liveChartTab.Name = "liveChartTab";
            this.liveChartTab.Size = new System.Drawing.Size(1656, 756);
            this.liveChartTab.TabIndex = 0;
            this.liveChartTab.Text = "Live Chart";
            this.liveChartTab.UseVisualStyleBackColor = true;
            this.liveChartTab.Click += new System.EventHandler(this.liveChartTab_Click);
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
            this.liveChartSplitContainer.Panel1MinSize = 1342;
            // 
            // liveChartSplitContainer.Panel2
            // 
            this.liveChartSplitContainer.Panel2.Controls.Add(this.debugBox);
            this.liveChartSplitContainer.Panel2.Controls.Add(this.label2);
            this.liveChartSplitContainer.Panel2.Controls.Add(this.checkedListBox1);
            this.liveChartSplitContainer.Panel2.Controls.Add(this.chamberPickerLabel);
            this.liveChartSplitContainer.Panel2.Controls.Add(this.chamberPickerBox);
            this.liveChartSplitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.liveChartSplitContainer.Panel2MinSize = 300;
            this.liveChartSplitContainer.Size = new System.Drawing.Size(1656, 756);
            this.liveChartSplitContainer.SplitterDistance = 1350;
            this.liveChartSplitContainer.TabIndex = 6;
            // 
            // liveChart
            // 
            chartArea1.Name = "ChartArea1";
            this.liveChart.ChartAreas.Add(chartArea1);
            this.liveChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.liveChart.Legends.Add(legend1);
            this.liveChart.Location = new System.Drawing.Point(0, 0);
            this.liveChart.Name = "liveChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.liveChart.Series.Add(series1);
            this.liveChart.Size = new System.Drawing.Size(1348, 754);
            this.liveChart.TabIndex = 1;
            this.liveChart.Text = "chart1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Data Series";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(25, 163);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(120, 94);
            this.checkedListBox1.TabIndex = 6;
            // 
            // chamberPickerLabel
            // 
            this.chamberPickerLabel.AutoSize = true;
            this.chamberPickerLabel.Location = new System.Drawing.Point(120, 18);
            this.chamberPickerLabel.Name = "chamberPickerLabel";
            this.chamberPickerLabel.Size = new System.Drawing.Size(49, 13);
            this.chamberPickerLabel.TabIndex = 5;
            this.chamberPickerLabel.Text = "Chamber";
            // 
            // chamberPickerBox
            // 
            this.chamberPickerBox.FormattingEnabled = true;
            this.chamberPickerBox.Location = new System.Drawing.Point(6, 45);
            this.chamberPickerBox.Name = "chamberPickerBox";
            this.chamberPickerBox.Size = new System.Drawing.Size(291, 21);
            this.chamberPickerBox.TabIndex = 4;
            this.chamberPickerBox.SelectedIndexChanged += new System.EventHandler(this.chamberPickerBox_SelectedIndexChanged);
            // 
            // customChartTab
            // 
            this.customChartTab.AutoScroll = true;
            this.customChartTab.Controls.Add(this.customChartSplitContainer);
            this.customChartTab.Location = new System.Drawing.Point(4, 22);
            this.customChartTab.Margin = new System.Windows.Forms.Padding(0);
            this.customChartTab.Name = "customChartTab";
            this.customChartTab.Size = new System.Drawing.Size(1656, 756);
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
            this.customChartSplitContainer.Panel1MinSize = 1342;
            // 
            // customChartSplitContainer.Panel2
            // 
            this.customChartSplitContainer.Panel2.Controls.Add(this.endDatePicker);
            this.customChartSplitContainer.Panel2.Controls.Add(this.label1);
            this.customChartSplitContainer.Panel2.Controls.Add(this.startDatePicker);
            this.customChartSplitContainer.Panel2.Controls.Add(this.comboBox1);
            this.customChartSplitContainer.Panel2.Controls.Add(this.startDateLabel);
            this.customChartSplitContainer.Panel2.Controls.Add(this.updateButton);
            this.customChartSplitContainer.Panel2.Controls.Add(this.endDateLabel);
            this.customChartSplitContainer.Panel2.Controls.Add(this.exportToExcelBtn);
            this.customChartSplitContainer.Panel2MinSize = 300;
            this.customChartSplitContainer.Size = new System.Drawing.Size(1656, 756);
            this.customChartSplitContainer.SplitterDistance = 1350;
            this.customChartSplitContainer.TabIndex = 17;
            // 
            // customChart
            // 
            chartArea2.Name = "ChartArea1";
            this.customChart.ChartAreas.Add(chartArea2);
            this.customChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.customChart.Legends.Add(legend2);
            this.customChart.Location = new System.Drawing.Point(0, 0);
            this.customChart.Name = "customChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.customChart.Series.Add(series2);
            this.customChart.Size = new System.Drawing.Size(1348, 754);
            this.customChart.TabIndex = 13;
            this.customChart.Text = "chart1";
            this.customChart.Click += new System.EventHandler(this.customChart_Click);
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "dd:MM:yyyy HH:mm:ss";
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatePicker.Location = new System.Drawing.Point(156, 108);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(137, 20);
            this.endDatePicker.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Chamber";
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "dd:MM:yyyy HH:mm:ss";
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatePicker.Location = new System.Drawing.Point(23, 151);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(137, 20);
            this.startDatePicker.TabIndex = 8;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(84, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 15;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(54, 132);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(55, 13);
            this.startDateLabel.TabIndex = 10;
            this.startDateLabel.Text = "Start Date";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(60, 194);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 14;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            // 
            // endDateLabel
            // 
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Location = new System.Drawing.Point(190, 89);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(52, 13);
            this.endDateLabel.TabIndex = 11;
            this.endDateLabel.Text = "End Date";
            // 
            // exportToExcelBtn
            // 
            this.exportToExcelBtn.Location = new System.Drawing.Point(170, 151);
            this.exportToExcelBtn.Name = "exportToExcelBtn";
            this.exportToExcelBtn.Size = new System.Drawing.Size(75, 23);
            this.exportToExcelBtn.TabIndex = 12;
            this.exportToExcelBtn.Text = "Export";
            this.exportToExcelBtn.UseVisualStyleBackColor = true;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.quitToolStripMenuItem.Text = "Quit";
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
            this.chamberToolStripMenuItem,
            this.sensorToolStripMenuItem});
            this.existingChamberToolStripMenuItem.Name = "existingChamberToolStripMenuItem";
            this.existingChamberToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.existingChamberToolStripMenuItem.Text = "Create New";
            // 
            // chamberToolStripMenuItem
            // 
            this.chamberToolStripMenuItem.Name = "chamberToolStripMenuItem";
            this.chamberToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.chamberToolStripMenuItem.Text = "Chamber";
            // 
            // sensorToolStripMenuItem
            // 
            this.sensorToolStripMenuItem.Name = "sensorToolStripMenuItem";
            this.sensorToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.sensorToolStripMenuItem.Text = "Sensor";
            // 
            // editExistingToolStripMenuItem
            // 
            this.editExistingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chamberToolStripMenuItem1,
            this.sensorToolStripMenuItem1});
            this.editExistingToolStripMenuItem.Name = "editExistingToolStripMenuItem";
            this.editExistingToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.editExistingToolStripMenuItem.Text = "Edit Existing";
            // 
            // chamberToolStripMenuItem1
            // 
            this.chamberToolStripMenuItem1.Name = "chamberToolStripMenuItem1";
            this.chamberToolStripMenuItem1.Size = new System.Drawing.Size(123, 22);
            this.chamberToolStripMenuItem1.Text = "Chamber";
            // 
            // sensorToolStripMenuItem1
            // 
            this.sensorToolStripMenuItem1.Name = "sensorToolStripMenuItem1";
            this.sensorToolStripMenuItem1.Size = new System.Drawing.Size(123, 22);
            this.sensorToolStripMenuItem1.Text = "Sensor";
            // 
            // deleteExistingToolStripMenuItem
            // 
            this.deleteExistingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chamberToolStripMenuItem2,
            this.sensorToolStripMenuItem2});
            this.deleteExistingToolStripMenuItem.Name = "deleteExistingToolStripMenuItem";
            this.deleteExistingToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.deleteExistingToolStripMenuItem.Text = "Delete Existing";
            // 
            // chamberToolStripMenuItem2
            // 
            this.chamberToolStripMenuItem2.Name = "chamberToolStripMenuItem2";
            this.chamberToolStripMenuItem2.Size = new System.Drawing.Size(123, 22);
            this.chamberToolStripMenuItem2.Text = "Chamber";
            // 
            // sensorToolStripMenuItem2
            // 
            this.sensorToolStripMenuItem2.Name = "sensorToolStripMenuItem2";
            this.sensorToolStripMenuItem2.Size = new System.Drawing.Size(123, 22);
            this.sensorToolStripMenuItem2.Text = "Sensor";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1664, 24);
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
            this.secondsToolStripMenuItem,
            this.secondsToolStripMenuItem1,
            this.secondsToolStripMenuItem2,
            this.secondsToolStripMenuItem3});
            this.updateIntervalToolStripMenuItem1.Name = "updateIntervalToolStripMenuItem1";
            this.updateIntervalToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.updateIntervalToolStripMenuItem1.Text = "Update Interval";
            // 
            // secondsToolStripMenuItem
            // 
            this.secondsToolStripMenuItem.Name = "secondsToolStripMenuItem";
            this.secondsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.secondsToolStripMenuItem.Text = "10 Seconds";
            // 
            // secondsToolStripMenuItem1
            // 
            this.secondsToolStripMenuItem1.Name = "secondsToolStripMenuItem1";
            this.secondsToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.secondsToolStripMenuItem1.Text = "20 Seconds";
            // 
            // secondsToolStripMenuItem2
            // 
            this.secondsToolStripMenuItem2.Name = "secondsToolStripMenuItem2";
            this.secondsToolStripMenuItem2.Size = new System.Drawing.Size(133, 22);
            this.secondsToolStripMenuItem2.Text = "30 Seconds";
            // 
            // secondsToolStripMenuItem3
            // 
            this.secondsToolStripMenuItem3.Name = "secondsToolStripMenuItem3";
            this.secondsToolStripMenuItem3.Size = new System.Drawing.Size(133, 22);
            this.secondsToolStripMenuItem3.Text = "60 Seconds";
            // 
            // rangeToolStripMenuItem
            // 
            this.rangeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hourToolStripMenuItem,
            this.hourToolStripMenuItem1,
            this.hourToolStripMenuItem4,
            this.hourToolStripMenuItem2,
            this.hourToolStripMenuItem3});
            this.rangeToolStripMenuItem.Name = "rangeToolStripMenuItem";
            this.rangeToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.rangeToolStripMenuItem.Text = "Range";
            // 
            // hourToolStripMenuItem
            // 
            this.hourToolStripMenuItem.Name = "hourToolStripMenuItem";
            this.hourToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.hourToolStripMenuItem.Text = "1 Hour";
            // 
            // hourToolStripMenuItem1
            // 
            this.hourToolStripMenuItem1.Name = "hourToolStripMenuItem1";
            this.hourToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.hourToolStripMenuItem1.Text = "2 Hour";
            // 
            // hourToolStripMenuItem4
            // 
            this.hourToolStripMenuItem4.Name = "hourToolStripMenuItem4";
            this.hourToolStripMenuItem4.Size = new System.Drawing.Size(116, 22);
            this.hourToolStripMenuItem4.Text = "6 Hour";
            // 
            // hourToolStripMenuItem2
            // 
            this.hourToolStripMenuItem2.Name = "hourToolStripMenuItem2";
            this.hourToolStripMenuItem2.Size = new System.Drawing.Size(116, 22);
            this.hourToolStripMenuItem2.Text = "12 Hour";
            // 
            // hourToolStripMenuItem3
            // 
            this.hourToolStripMenuItem3.Name = "hourToolStripMenuItem3";
            this.hourToolStripMenuItem3.Size = new System.Drawing.Size(116, 22);
            this.hourToolStripMenuItem3.Text = "24 Hour";
            // 
            // debugBox
            // 
            this.debugBox.Location = new System.Drawing.Point(25, 291);
            this.debugBox.Name = "debugBox";
            this.debugBox.Size = new System.Drawing.Size(255, 252);
            this.debugBox.TabIndex = 9;
            this.debugBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1664, 806);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MinimumSize = new System.Drawing.Size(1680, 845);
            this.Name = "Form1";
            this.Text = "Sensorcom";
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
        private System.Windows.Forms.ComboBox chamberPickerBox;
        private System.Windows.Forms.Button exportToExcelBtn;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart customChart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart liveChart;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingChamberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chamberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editExistingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chamberToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sensorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteExistingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chamberToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem sensorToolStripMenuItem2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liveChartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem rangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hourToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hourToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem hourToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem hourToolStripMenuItem3;
        private System.Windows.Forms.SplitContainer liveChartSplitContainer;
        private System.Windows.Forms.SplitContainer customChartSplitContainer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.RichTextBox debugBox;
    }
}

