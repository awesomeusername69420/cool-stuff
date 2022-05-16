namespace Krypt.Tools
{
    partial class FrequencyAnalysis
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Analyze = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.AnalyiticalText = new System.Windows.Forms.TextBox();
            this.Output = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Output, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.35227F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.64773F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(462, 352);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Analyze);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.AnalyiticalText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(456, 216);
            this.panel1.TabIndex = 0;
            // 
            // Analyze
            // 
            this.Analyze.Location = new System.Drawing.Point(372, 185);
            this.Analyze.Name = "Analyze";
            this.Analyze.Size = new System.Drawing.Size(75, 23);
            this.Analyze.TabIndex = 2;
            this.Analyze.Text = "Analyze";
            this.Analyze.UseVisualStyleBackColor = true;
            this.Analyze.Click += new System.EventHandler(this.Analyze_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Text";
            // 
            // AnalyiticalText
            // 
            this.AnalyiticalText.Location = new System.Drawing.Point(9, 30);
            this.AnalyiticalText.Multiline = true;
            this.AnalyiticalText.Name = "AnalyiticalText";
            this.AnalyiticalText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AnalyiticalText.Size = new System.Drawing.Size(438, 149);
            this.AnalyiticalText.TabIndex = 0;
            // 
            // Output
            // 
            this.Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Output.Location = new System.Drawing.Point(3, 225);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output.Size = new System.Drawing.Size(456, 124);
            this.Output.TabIndex = 1;
            // 
            // FrequencyAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 352);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrequencyAnalysis";
            this.ShowIcon = false;
            this.Text = "Frequency Analysis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverrideClose);
            this.Load += new System.EventHandler(this.FrequencyAnalysis_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Analyze;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AnalyiticalText;
        private System.Windows.Forms.TextBox Output;
    }
}