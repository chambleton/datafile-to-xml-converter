namespace FileToXmlConverter
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
            this.btnConvertToXml = new System.Windows.Forms.Button();
            this.chkTableView = new System.Windows.Forms.CheckBox();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnViewFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAccessQuery = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboExcelWorksheet = new System.Windows.Forms.ComboBox();
            this.btnGetWorksheets = new System.Windows.Forms.Button();
            this.chkCleanupXml = new System.Windows.Forms.CheckBox();
            this.btnSaveXml = new System.Windows.Forms.Button();
            this.btnTransformXML = new System.Windows.Forms.Button();
            this.txtXsltFile = new System.Windows.Forms.TextBox();
            this.btnViewXsltFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOpenXsltFile = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRowElement = new System.Windows.Forms.TextBox();
            this.txtRootElement = new System.Windows.Forms.TextBox();
            this.txtXsdFile = new System.Windows.Forms.TextBox();
            this.btnViewXsdFile = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnOpenXsdFile = new System.Windows.Forms.Button();
            this.btnValidateXML = new System.Windows.Forms.Button();
            this.xmlXmlOutputGridView = new FileToXmlConverter.XmlGridView();
            this.xmlXsltOutputGridView = new FileToXmlConverter.XmlGridView();
            this.tabOutput = new System.Windows.Forms.TabControl();
            this.tabXmlOutput = new System.Windows.Forms.TabPage();
            this.tabXsltOutput = new System.Windows.Forms.TabPage();
            this.tabOutput.SuspendLayout();
            this.tabXmlOutput.SuspendLayout();
            this.tabXsltOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConvertToXml
            // 
            this.btnConvertToXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConvertToXml.Location = new System.Drawing.Point(653, 12);
            this.btnConvertToXml.Name = "btnConvertToXml";
            this.btnConvertToXml.Size = new System.Drawing.Size(99, 23);
            this.btnConvertToXml.TabIndex = 25;
            this.btnConvertToXml.Text = "Convert to XML";
            this.btnConvertToXml.UseVisualStyleBackColor = true;
            this.btnConvertToXml.Click += new System.EventHandler(this.btnConvertToXml_Click);
            // 
            // chkTableView
            // 
            this.chkTableView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTableView.AutoSize = true;
            this.chkTableView.Location = new System.Drawing.Point(540, 125);
            this.chkTableView.Name = "chkTableView";
            this.chkTableView.Size = new System.Drawing.Size(79, 17);
            this.chkTableView.TabIndex = 21;
            this.chkTableView.Text = "Table View";
            this.chkTableView.UseVisualStyleBackColor = true;
            this.chkTableView.CheckedChanged += new System.EventHandler(this.chkTableView_CheckedChanged);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(68, 12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(468, 20);
            this.txtFile.TabIndex = 1;
            // 
            // btnViewFile
            // 
            this.btnViewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewFile.Location = new System.Drawing.Point(574, 12);
            this.btnViewFile.Name = "btnViewFile";
            this.btnViewFile.Size = new System.Drawing.Size(50, 23);
            this.btnViewFile.TabIndex = 3;
            this.btnViewFile.Text = "View";
            this.btnViewFile.UseVisualStyleBackColor = true;
            this.btnViewFile.Click += new System.EventHandler(this.btnViewFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Input File:";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFile.Location = new System.Drawing.Point(540, 12);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(28, 23);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Excel Worksheet:";
            // 
            // txtAccessQuery
            // 
            this.txtAccessQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccessQuery.Location = new System.Drawing.Point(113, 151);
            this.txtAccessQuery.Name = "txtAccessQuery";
            this.txtAccessQuery.Size = new System.Drawing.Size(511, 20);
            this.txtAccessQuery.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "MS Access Query:";
            // 
            // cboExcelWorksheet
            // 
            this.cboExcelWorksheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExcelWorksheet.FormattingEnabled = true;
            this.cboExcelWorksheet.Location = new System.Drawing.Point(113, 121);
            this.cboExcelWorksheet.Name = "cboExcelWorksheet";
            this.cboExcelWorksheet.Size = new System.Drawing.Size(172, 21);
            this.cboExcelWorksheet.TabIndex = 18;
            // 
            // btnGetWorksheets
            // 
            this.btnGetWorksheets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetWorksheets.Location = new System.Drawing.Point(322, 121);
            this.btnGetWorksheets.Name = "btnGetWorksheets";
            this.btnGetWorksheets.Size = new System.Drawing.Size(51, 23);
            this.btnGetWorksheets.TabIndex = 19;
            this.btnGetWorksheets.Text = "Query";
            this.btnGetWorksheets.UseVisualStyleBackColor = true;
            this.btnGetWorksheets.Click += new System.EventHandler(this.btnGetWorksheets_Click);
            // 
            // chkCleanupXml
            // 
            this.chkCleanupXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCleanupXml.AutoSize = true;
            this.chkCleanupXml.Location = new System.Drawing.Point(428, 125);
            this.chkCleanupXml.Name = "chkCleanupXml";
            this.chkCleanupXml.Size = new System.Drawing.Size(90, 17);
            this.chkCleanupXml.TabIndex = 20;
            this.chkCleanupXml.Text = "Cleanup XML";
            this.chkCleanupXml.UseVisualStyleBackColor = true;
            this.chkCleanupXml.CheckedChanged += new System.EventHandler(this.chkCleanupXml_CheckedChanged);
            // 
            // btnSaveXml
            // 
            this.btnSaveXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveXml.Location = new System.Drawing.Point(653, 93);
            this.btnSaveXml.Name = "btnSaveXml";
            this.btnSaveXml.Size = new System.Drawing.Size(99, 23);
            this.btnSaveXml.TabIndex = 28;
            this.btnSaveXml.Text = "Save XML";
            this.btnSaveXml.UseVisualStyleBackColor = true;
            this.btnSaveXml.Click += new System.EventHandler(this.btnSaveXml_Click);
            // 
            // btnTransformXML
            // 
            this.btnTransformXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransformXML.Location = new System.Drawing.Point(653, 39);
            this.btnTransformXML.Name = "btnTransformXML";
            this.btnTransformXML.Size = new System.Drawing.Size(99, 23);
            this.btnTransformXML.TabIndex = 26;
            this.btnTransformXML.Text = "Transform XML";
            this.btnTransformXML.UseVisualStyleBackColor = true;
            this.btnTransformXML.Click += new System.EventHandler(this.btnTransformXML_Click);
            // 
            // txtXsltFile
            // 
            this.txtXsltFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXsltFile.Location = new System.Drawing.Point(68, 39);
            this.txtXsltFile.Name = "txtXsltFile";
            this.txtXsltFile.Size = new System.Drawing.Size(468, 20);
            this.txtXsltFile.TabIndex = 5;
            // 
            // btnViewXsltFile
            // 
            this.btnViewXsltFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewXsltFile.Location = new System.Drawing.Point(574, 39);
            this.btnViewXsltFile.Name = "btnViewXsltFile";
            this.btnViewXsltFile.Size = new System.Drawing.Size(50, 23);
            this.btnViewXsltFile.TabIndex = 7;
            this.btnViewXsltFile.Text = "View";
            this.btnViewXsltFile.UseVisualStyleBackColor = true;
            this.btnViewXsltFile.Click += new System.EventHandler(this.btnViewXsltFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "XSLT File:";
            // 
            // btnOpenXsltFile
            // 
            this.btnOpenXsltFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenXsltFile.Location = new System.Drawing.Point(540, 39);
            this.btnOpenXsltFile.Name = "btnOpenXsltFile";
            this.btnOpenXsltFile.Size = new System.Drawing.Size(28, 23);
            this.btnOpenXsltFile.TabIndex = 6;
            this.btnOpenXsltFile.Text = "...";
            this.btnOpenXsltFile.UseVisualStyleBackColor = true;
            this.btnOpenXsltFile.Click += new System.EventHandler(this.btnOpenXsltFile_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Overrides:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(90, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Root Element:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(328, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Row Element:";
            // 
            // txtRowElement
            // 
            this.txtRowElement.Location = new System.Drawing.Point(401, 94);
            this.txtRowElement.Name = "txtRowElement";
            this.txtRowElement.Size = new System.Drawing.Size(135, 20);
            this.txtRowElement.TabIndex = 16;
            // 
            // txtRootElement
            // 
            this.txtRootElement.Location = new System.Drawing.Point(165, 94);
            this.txtRootElement.Name = "txtRootElement";
            this.txtRootElement.Size = new System.Drawing.Size(120, 20);
            this.txtRootElement.TabIndex = 14;
            // 
            // txtXsdFile
            // 
            this.txtXsdFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXsdFile.Location = new System.Drawing.Point(68, 68);
            this.txtXsdFile.Name = "txtXsdFile";
            this.txtXsdFile.Size = new System.Drawing.Size(468, 20);
            this.txtXsdFile.TabIndex = 9;
            // 
            // btnViewXsdFile
            // 
            this.btnViewXsdFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewXsdFile.Location = new System.Drawing.Point(574, 68);
            this.btnViewXsdFile.Name = "btnViewXsdFile";
            this.btnViewXsdFile.Size = new System.Drawing.Size(50, 23);
            this.btnViewXsdFile.TabIndex = 11;
            this.btnViewXsdFile.Text = "View";
            this.btnViewXsdFile.UseVisualStyleBackColor = true;
            this.btnViewXsdFile.Click += new System.EventHandler(this.btnViewXsdFile_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "XSD File:";
            // 
            // btnOpenXsdFile
            // 
            this.btnOpenXsdFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenXsdFile.Location = new System.Drawing.Point(540, 68);
            this.btnOpenXsdFile.Name = "btnOpenXsdFile";
            this.btnOpenXsdFile.Size = new System.Drawing.Size(28, 23);
            this.btnOpenXsdFile.TabIndex = 10;
            this.btnOpenXsdFile.Text = "...";
            this.btnOpenXsdFile.UseVisualStyleBackColor = true;
            this.btnOpenXsdFile.Click += new System.EventHandler(this.btnOpenXsdFile_Click);
            // 
            // btnValidateXML
            // 
            this.btnValidateXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidateXML.Location = new System.Drawing.Point(653, 66);
            this.btnValidateXML.Name = "btnValidateXML";
            this.btnValidateXML.Size = new System.Drawing.Size(99, 23);
            this.btnValidateXML.TabIndex = 27;
            this.btnValidateXML.Text = "Validate XML";
            this.btnValidateXML.UseVisualStyleBackColor = true;
            this.btnValidateXML.Click += new System.EventHandler(this.btnValidateXML_Click);
            // 
            // xmlXmlOutputGridView
            // 
            this.xmlXmlOutputGridView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xmlXmlOutputGridView.DataFilePath = "";
            this.xmlXmlOutputGridView.DataSetTableIndex = 0;
            this.xmlXmlOutputGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmlXmlOutputGridView.Location = new System.Drawing.Point(3, 3);
            this.xmlXmlOutputGridView.Name = "xmlXmlOutputGridView";
            this.xmlXmlOutputGridView.Size = new System.Drawing.Size(723, 470);
            this.xmlXmlOutputGridView.TabIndex = 24;
            this.xmlXmlOutputGridView.ViewMode = FileToXmlConverter.XmlGridView.VIEW_MODE.XML;
            // 
            // xmlXsltOutputGridView
            // 
            this.xmlXsltOutputGridView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xmlXsltOutputGridView.DataFilePath = "";
            this.xmlXsltOutputGridView.DataSetTableIndex = 0;
            this.xmlXsltOutputGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmlXsltOutputGridView.Location = new System.Drawing.Point(3, 3);
            this.xmlXsltOutputGridView.Name = "xmlXsltOutputGridView";
            this.xmlXsltOutputGridView.Size = new System.Drawing.Size(512, 269);
            this.xmlXsltOutputGridView.TabIndex = 29;
            this.xmlXsltOutputGridView.ViewMode = FileToXmlConverter.XmlGridView.VIEW_MODE.TABLE;
            // 
            // tabOutput
            // 
            this.tabOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOutput.Controls.Add(this.tabXmlOutput);
            this.tabOutput.Controls.Add(this.tabXsltOutput);
            this.tabOutput.Location = new System.Drawing.Point(15, 190);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.SelectedIndex = 0;
            this.tabOutput.Size = new System.Drawing.Size(737, 502);
            this.tabOutput.TabIndex = 30;
            // 
            // tabXmlOutput
            // 
            this.tabXmlOutput.Controls.Add(this.xmlXmlOutputGridView);
            this.tabXmlOutput.Location = new System.Drawing.Point(4, 22);
            this.tabXmlOutput.Name = "tabXmlOutput";
            this.tabXmlOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabXmlOutput.Size = new System.Drawing.Size(729, 476);
            this.tabXmlOutput.TabIndex = 0;
            this.tabXmlOutput.Text = "XML Output";
            this.tabXmlOutput.UseVisualStyleBackColor = true;
            // 
            // tabXsltOutput
            // 
            this.tabXsltOutput.Controls.Add(this.xmlXsltOutputGridView);
            this.tabXsltOutput.Location = new System.Drawing.Point(4, 22);
            this.tabXsltOutput.Name = "tabXsltOutput";
            this.tabXsltOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabXsltOutput.Size = new System.Drawing.Size(518, 275);
            this.tabXsltOutput.TabIndex = 1;
            this.tabXsltOutput.Text = "XSLT Output";
            this.tabXsltOutput.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 704);
            this.Controls.Add(this.tabOutput);
            this.Controls.Add(this.btnValidateXML);
            this.Controls.Add(this.txtXsdFile);
            this.Controls.Add(this.btnViewXsdFile);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnOpenXsdFile);
            this.Controls.Add(this.txtRootElement);
            this.Controls.Add(this.txtRowElement);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtXsltFile);
            this.Controls.Add(this.btnViewXsltFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnOpenXsltFile);
            this.Controls.Add(this.btnTransformXML);
            this.Controls.Add(this.btnSaveXml);
            this.Controls.Add(this.chkCleanupXml);
            this.Controls.Add(this.btnGetWorksheets);
            this.Controls.Add(this.cboExcelWorksheet);
            this.Controls.Add(this.txtAccessQuery);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnViewFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnConvertToXml);
            this.Controls.Add(this.chkTableView);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File to XML Converter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabOutput.ResumeLayout(false);
            this.tabXmlOutput.ResumeLayout(false);
            this.tabXsltOutput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private XmlGridView xmlXmlOutputGridView;
        private System.Windows.Forms.Button btnConvertToXml;
        private System.Windows.Forms.CheckBox chkTableView;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnViewFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAccessQuery;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboExcelWorksheet;
        private System.Windows.Forms.Button btnGetWorksheets;
        private System.Windows.Forms.CheckBox chkCleanupXml;
        private System.Windows.Forms.Button btnSaveXml;
        private System.Windows.Forms.Button btnTransformXML;
        private System.Windows.Forms.TextBox txtXsltFile;
        private System.Windows.Forms.Button btnViewXsltFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOpenXsltFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRowElement;
        private System.Windows.Forms.TextBox txtRootElement;
        private System.Windows.Forms.TextBox txtXsdFile;
        private System.Windows.Forms.Button btnViewXsdFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnOpenXsdFile;
        private System.Windows.Forms.Button btnValidateXML;
        private XmlGridView xmlXsltOutputGridView;
        private System.Windows.Forms.TabControl tabOutput;
        private System.Windows.Forms.TabPage tabXmlOutput;
        private System.Windows.Forms.TabPage tabXsltOutput;
    }
}

