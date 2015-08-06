using System;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FileToXmlConverter
{
    public partial class Form1 : Form
    {
        private string m_sXmlFilePath = string.Empty;
        private string m_sXmlTransformFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetBehavior();
        }

        private void SetBehavior()
        {
            bool bFileExists = ((txtFile.Text.Trim() != string.Empty) && File.Exists(txtFile.Text));
            bool bXsltFileExists = ((txtXsltFile.Text.Trim() != string.Empty) && File.Exists(txtXsltFile.Text));
            bool bXsdFileExists = ((txtXsdFile.Text.Trim() != string.Empty) && File.Exists(txtXsdFile.Text));

            btnConvertToXml.Enabled = bFileExists;            
            btnViewFile.Enabled = bFileExists;

            btnViewXsltFile.Enabled = bXsltFileExists;
            btnTransformXML.Enabled = bXsltFileExists && File.Exists(m_sXmlFilePath);

            btnViewXsdFile.Enabled = bXsdFileExists;
            btnValidateXML.Enabled = bXsdFileExists;

            btnGetWorksheets.Enabled = bFileExists && (txtFile.Text.EndsWith(".xls") == true);
            cboExcelWorksheet.Enabled = bFileExists && (txtFile.Text.EndsWith(".xls") == true);
            
            txtAccessQuery.Enabled = bFileExists;
            btnSaveXml.Enabled = ((m_sXmlFilePath != string.Empty) && (File.Exists(m_sXmlFilePath) == true));

            if (chkTableView.Checked == true)
            {
                xmlXmlOutputGridView.ViewMode = XmlGridView.VIEW_MODE.TABLE;
                xmlXsltOutputGridView.ViewMode = XmlGridView.VIEW_MODE.TABLE;
            }
            else
            {
                xmlXmlOutputGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
                xmlXsltOutputGridView.ViewMode = XmlGridView.VIEW_MODE.XML;
            }

            xmlXmlOutputGridView.DataSetTableIndex = 0;
            xmlXsltOutputGridView.DataSetTableIndex = 0;     
        }

        private void ResetInputFiles()
        {            
            xmlXmlOutputGridView.DataFilePath = string.Empty;
            xmlXsltOutputGridView.DataFilePath = string.Empty;
            cboExcelWorksheet.Items.Clear();

            SetBehavior();
        }


        private void ViewFile(string sFilePath)
        {
            if (File.Exists(sFilePath) == true)
            {
                FileInfo fiInfo = new FileInfo(sFilePath);
                if (fiInfo.Extension.Trim() == string.Empty)
                {
                    ProcessStartInfo psi = new ProcessStartInfo("notepad.exe", sFilePath);
                    Process.Start(psi);
                }
                else
                {
                    Process.Start(sFilePath);
                }
            }
        }

        private void cmbTableIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetBehavior();
        }

        private void chkTableView_CheckedChanged(object sender, EventArgs e)
        {
            SetBehavior();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string sOpenPath = FileUtilities.GetOpenFilePath();
            if (sOpenPath != string.Empty)
            {
                txtFile.Text = sOpenPath;

                ResetInputFiles();
                SetBehavior();
            }
        }

        private void btnViewFile_Click(object sender, EventArgs e)
        {
            ViewFile(txtFile.Text);
        }

        private void btnConvertToXml_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtFile.Text) == false)
            {
                return;
            }

            XmlDocument xmlRawFile = null;
            try
            {
                Cursor = Cursors.WaitCursor;

                if(cboExcelWorksheet.SelectedIndex > -1)
                    RawFileConverter.ExcelWorksheetName = cboExcelWorksheet.SelectedItem.ToString();

                RawFileConverter.AccessDatabaseQuery = txtAccessQuery.Text.Trim();
                RawFileConverter.PerformValidationCleanup = chkCleanupXml.Checked;

                xmlRawFile = RawFileConverter.ConvertFileToRawXml(txtFile.Text);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error converting file! " + ex.Message);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            m_sXmlFilePath = FileUtilities.GetUniqueTempFileName(".xml"); // add the xml extension so the browser can read it
            xmlRawFile.Save(m_sXmlFilePath);

            ApplyOverrideElements(m_sXmlFilePath, txtRootElement.Text, txtRowElement.Text);           
            xmlXmlOutputGridView.DataFilePath = m_sXmlFilePath;

            tabOutput.SelectTab(tabXmlOutput);
            SetBehavior();
        }

        private void ApplyOverrideElements(string sFilePath, string sNewRootElement, string sNewRowElement)
        {
            if (File.Exists(sFilePath) == false)
                return;

            sNewRootElement = sNewRootElement.Trim();
            sNewRowElement = sNewRowElement.Trim();

            string sXmlContents = FileUtilities.ReadFileContents(sFilePath);

            if (sNewRootElement != string.Empty)
            {
                string sReplaceRoot = sNewRootElement + ">";
                sXmlContents = sXmlContents.Replace("NewDataSet>", sReplaceRoot);
            }

            if (sNewRowElement != string.Empty)
            {
                string sReplaceRow = sNewRowElement + ">";
                sXmlContents = sXmlContents.Replace("Table>", sReplaceRow);
            }

            FileUtilities.WriteFileContents(sFilePath, sXmlContents, true);
        }

        private void btnGetWorksheets_Click(object sender, EventArgs e)
        {
            cboExcelWorksheet.Items.Clear();
            cboExcelWorksheet.Items.AddRange(RawFileConverter.GetExcelWorksheets(txtFile.Text.Trim()));
            cboExcelWorksheet.DroppedDown = true;
        }

        private void chkCleanupXml_CheckedChanged(object sender, EventArgs e)
        {
            SetBehavior();
        }

        private void btnSaveXml_Click(object sender, EventArgs e)
        {
            if ((m_sXmlFilePath == string.Empty) || (File.Exists(m_sXmlFilePath) == false))
                return;

            string sDestFile = FileUtilities.GetSaveFilePath();
            if (sDestFile.Trim() == string.Empty)
                return;

            if (tabOutput.SelectedTab == tabXsltOutput)
            {
                if(File.Exists(m_sXmlTransformFilePath))
                    File.Copy(m_sXmlTransformFilePath, sDestFile, true);
            }
            else
            {
                File.Copy(m_sXmlFilePath, sDestFile, true);
            }
        }

        private void btnTransformXML_Click(object sender, EventArgs e)
        {
            if (File.Exists(m_sXmlFilePath) == false)
                return;

            if (File.Exists(txtXsltFile.Text) == false)
                return;

            m_sXmlTransformFilePath = FileUtilities.GetUniqueTempFileName(".xml");

            try
            {
                XmlDocument xmltrans = XmlTranslator.TransformXml(m_sXmlFilePath, txtXsltFile.Text);
                xmltrans.Save(m_sXmlTransformFilePath);
                xmlXsltOutputGridView.DataFilePath = m_sXmlTransformFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XSLT Transformation error for file:  " + m_sXmlTransformFilePath + System.Environment.NewLine + System.Environment.NewLine + ex.Message, "XSLT Transform Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tabOutput.SelectTab(tabXsltOutput);
        }

        private void btnOpenXsltFile_Click(object sender, EventArgs e)
        {
            string sOpenPath = FileUtilities.GetOpenFilePath();
            if (sOpenPath != string.Empty)
            {
                txtXsltFile.Text = sOpenPath;
                
                SetBehavior();
            }
        }

        private void btnViewXsltFile_Click(object sender, EventArgs e)
        {
            ViewFile(txtXsltFile.Text);
        }

        private void btnOpenXsdFile_Click(object sender, EventArgs e)
        {
            string sOpenPath = FileUtilities.GetOpenFilePath();
            if (sOpenPath != string.Empty)
            {
                txtXsdFile.Text = sOpenPath;

                SetBehavior();
            }
        }

        private void btnViewXsdFile_Click(object sender, EventArgs e)
        {
            ViewFile(txtXsdFile.Text);
        }

        private void btnValidateXML_Click(object sender, EventArgs e)
        {
            if (File.Exists(m_sXmlFilePath) == false)
                return;

            if (File.Exists(txtXsdFile.Text) == false)
                return;
            
            string sFileToValidate = string.Empty;
            if (tabOutput.SelectedTab == tabXsltOutput)
            {
                if (File.Exists(m_sXmlTransformFilePath))
                    sFileToValidate = m_sXmlTransformFilePath;                    
            }
            else
            {
                sFileToValidate = m_sXmlFilePath;
            }

            bool bvalid = false;
            try
            {
                bvalid = XmlValidator.Validate(sFileToValidate, txtXsdFile.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation error for file:  " + sFileToValidate + System.Environment.NewLine + System.Environment.NewLine + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (bvalid == true)
                MessageBox.Show("Validation succeeded for file:  " + sFileToValidate, "Validation Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Validation FAILED for file:  " + sFileToValidate + System.Environment.NewLine + System.Environment.NewLine + XmlValidator.ValidationErrorMessage, "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}