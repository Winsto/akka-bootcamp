namespace ChartApp
{
    partial class Main
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.sysChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.diskSelector = new System.Windows.Forms.Button();
            this.memorySelector = new System.Windows.Forms.Button();
            this.cpuSelector = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).BeginInit();
            this.SuspendLayout();
            // 
            // sysChart
            // 
            chartArea3.Name = "ChartArea1";
            this.sysChart.ChartAreas.Add(chartArea3);
            this.sysChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.sysChart.Legends.Add(legend3);
            this.sysChart.Location = new System.Drawing.Point(0, 0);
            this.sysChart.Name = "sysChart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.sysChart.Series.Add(series3);
            this.sysChart.Size = new System.Drawing.Size(684, 446);
            this.sysChart.TabIndex = 0;
            this.sysChart.Text = "sysChart";
            // 
            // diskSelector
            // 
            this.diskSelector.Location = new System.Drawing.Point(593, 379);
            this.diskSelector.Name = "diskSelector";
            this.diskSelector.Size = new System.Drawing.Size(75, 23);
            this.diskSelector.TabIndex = 1;
            this.diskSelector.Text = "DISK (OFF)";
            this.diskSelector.UseVisualStyleBackColor = true;
            this.diskSelector.Click += new System.EventHandler(this.diskSelector_Click);
            // 
            // memorySelector
            // 
            this.memorySelector.Location = new System.Drawing.Point(593, 350);
            this.memorySelector.Name = "memorySelector";
            this.memorySelector.Size = new System.Drawing.Size(75, 23);
            this.memorySelector.TabIndex = 2;
            this.memorySelector.Text = "MEMORY (OFF)";
            this.memorySelector.UseVisualStyleBackColor = true;
            this.memorySelector.Click += new System.EventHandler(this.memorySelector_Click);
            // 
            // cpuSelector
            // 
            this.cpuSelector.Location = new System.Drawing.Point(593, 321);
            this.cpuSelector.Name = "cpuSelector";
            this.cpuSelector.Size = new System.Drawing.Size(75, 23);
            this.cpuSelector.TabIndex = 3;
            this.cpuSelector.Text = "CPU (ON)";
            this.cpuSelector.UseVisualStyleBackColor = true;
            this.cpuSelector.Click += new System.EventHandler(this.cpuSelector_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 446);
            this.Controls.Add(this.cpuSelector);
            this.Controls.Add(this.memorySelector);
            this.Controls.Add(this.diskSelector);
            this.Controls.Add(this.sysChart);
            this.Name = "Main";
            this.Text = "System Metrics";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart sysChart;
        private System.Windows.Forms.Button diskSelector;
        private System.Windows.Forms.Button memorySelector;
        private System.Windows.Forms.Button cpuSelector;
    }
}

