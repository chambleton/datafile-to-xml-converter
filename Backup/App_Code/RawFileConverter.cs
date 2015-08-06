using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace FileToXmlConverter
{
    /// <summary>
    /// This class converts a raw file into a raw, .NET compatible XML file
    /// </summary>
    public class RawFileConverter
    {
        private static string[] REQUIRED_ELEMENTS = { "Column0", "Column1" };        // TODO:  add/remove from this list for custom validation
        private const int MINIMUM_NUMBER_CSV_COLUMNS = 7;    // TODO:  set the minimum number of acceptable columns for your own file

        private static string NEWDATASET_TABLE_ROOT = "/NewDataSet/Table";
        private const string XML_PRESERVE_WHITESPACE_ATTRIBUTE = "xml:space=\"preserve\"";
        private const string QUOTED_COMMA_DELIMITER = "\",\"";
        
        private static char[] ALLOWED_DELIMITER_CHARACTERS = new char[] { '|', '\t', ',' }; // keep the delimiters ordered so the comma goes LAST


        private static bool m_bPerformValidationCleanup = true;
        public static bool PerformValidationCleanup
        {
            get { return RawFileConverter.m_bPerformValidationCleanup; }
            set { RawFileConverter.m_bPerformValidationCleanup = value; }
        }


        private static string m_sDebugOutputDirectory = string.Empty;
        public static string DebugOutputDirectory
        {
            get { return RawFileConverter.m_sDebugOutputDirectory; }
            set { RawFileConverter.m_sDebugOutputDirectory = value; }
        }

        private static string m_sExcelWorksheetName = string.Empty;
        public static string ExcelWorksheetName
        {
            get { return RawFileConverter.m_sExcelWorksheetName; }
            set { RawFileConverter.m_sExcelWorksheetName = value; }
        }

        private static string m_sAccessDatabaseQuery = string.Empty;
        public static string AccessDatabaseQuery
        {
            get { return RawFileConverter.m_sAccessDatabaseQuery; }
            set { RawFileConverter.m_sAccessDatabaseQuery = value; }
        }


        /// <summary>
        /// Converts the specified file to it's equivalent XmlDocument
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public static XmlDocument ConvertFileToRawXml(string sFilePath)
        {
            XmlDocument xmlRawFile = null;

            // copy the file to a temp file and read from that (some providers are picky)
            System.IO.FileInfo fiInfo = new FileInfo(sFilePath);
            string sFileExtension = fiInfo.Extension.Trim().ToLower();
            if (sFileExtension == string.Empty)
                sFileExtension = ".txt";

            string sTempFile = FileUtilities.GetUniqueTempFileName(sFileExtension);

            try
            {
                // copy the file to a temp file and read from that (some providers are picky)
                if (File.Exists(sTempFile) == true)
                    File.Delete(sTempFile);

                File.Copy(sFilePath, sTempFile);

                switch (sFileExtension)
                {
                    case ".xls":
                        xmlRawFile = ConvertExcelFile(sTempFile);
                        break;

                    case ".xml":
                        xmlRawFile = ConvertXmlFile(sTempFile);
                        break;

                    case ".mdb":
                        xmlRawFile = ConvertAccessDatabaseFile(sTempFile);
                        break;
                    
                    case ".txt":                    
                        xmlRawFile = ConvertTextFile(sTempFile);
                        break;
                    
                    case ".csv":
                        xmlRawFile = ConvertCsvFile(sFilePath);
                        break;

                    default:
                        throw new Exception("Unknown file extension encountered: " + sFilePath);
                        break;
                }
            }
            catch (Exception ex)
            {
                string sErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    sErrorMessage = sErrorMessage + "  Details: " + ex.InnerException.Message;

                throw new Exception("ConvertFileToRawXml exception: " + sErrorMessage);
            }
            finally
            {
                try
                {
                    if (File.Exists(sTempFile) == true)
                        File.Delete(sTempFile);
                }
                catch
                { 
                    // error deleting the temp file - bummer
                }
            }            

            return xmlRawFile;
        }

        public static string[] GetExcelWorksheets(string sFilePath)
        {            
            System.Data.OleDb.OleDbConnection oleConnection = null;
            string[] sWorksheets = null;

            try
            {
                oleConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + sFilePath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"");
                oleConnection.Open();

                //select just TABLE in the Object array of restrictions.
                //Remove TABLE and insert Null to see tables, views, and other objects.
                object[] objArrRestrict = new object[] { null, null, null, "TABLE" };
                DataTable schemaTbl = oleConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, objArrRestrict);

                ArrayList aryTableNames = new ArrayList();
                // Display the table name from each row in the schema
                foreach (DataRow row in schemaTbl.Rows)
                {
                    string sTable = Convert.ToString(row["TABLE_NAME"]).Trim();
                    if (sTable.IndexOf("$_") < 0)
                        aryTableNames.Add(sTable);
                }
                oleConnection.Close();

                if (aryTableNames.Count <= 0)
                    throw new Exception("Excel spreadsheet contains no valid worksheets!");

                sWorksheets = (string[])aryTableNames.ToArray(typeof(string));
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ConvertExcelFile", ex);
            }
            finally
            {
                oleConnection.Close();
            }

            return sWorksheets;
        }

        /// <summary>
        /// Converts the specified Excel file to it's equivalent XmlDocument
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="sExcelFileQuery"></param>
        /// <returns></returns>
        private static XmlDocument ConvertExcelFile(string sFilePath)
        {
            XmlDocument xmlRaw = null;
            System.Data.OleDb.OleDbConnection oleConnection = null;

            try
            {
                oleConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + sFilePath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"");
                string[] sWorksheets = GetExcelWorksheets(sFilePath);

                if (sWorksheets.Length <= 0)
                    throw new Exception("Excel spreadsheet contains no valid worksheets!");
                
                string sWorksheetName = string.Empty;
                foreach (string sTableName in sWorksheets)
                {
                    // initially set the data worksheet to the first/0th worksheet
                    if (sWorksheetName == string.Empty)
                        sWorksheetName = sTableName;

                    // loop thru until we find the one called 'DataEntry', in case it's not the first one
                    if (sTableName.Trim().ToUpper() == ExcelWorksheetName.ToUpper())
                    {
                        sWorksheetName = sTableName;
                        break;
                    }
                }
                               
                if (m_sDebugOutputDirectory.Trim() != string.Empty)
                {
                    FileInfo fiDebug = new FileInfo(sFilePath);
                    string sDebugFilePath = Path.Combine(m_sDebugOutputDirectory, fiDebug.Name);
                    if (File.Exists(sDebugFilePath) == true)
                        FileUtilities.DeleteFile(sDebugFilePath);

                    File.Copy(sFilePath, sDebugFilePath);
                }

                string sExcelDataQuery = string.Format("SELECT * FROM [{0}]", sWorksheetName);
                System.Data.OleDb.OleDbDataAdapter oleCommand = new System.Data.OleDb.OleDbDataAdapter(sExcelDataQuery, oleConnection);

                System.Data.DataSet dsExcel = new System.Data.DataSet();
                oleCommand.Fill(dsExcel);

                xmlRaw = CleanupRawXml(dsExcel.GetXml());                
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ConvertExcelFile", ex);                
            }
            finally
            {
                oleConnection.Close();
            }
            
            return xmlRaw;
        }

        /// <summary>
        /// Converts the specified XML file to it's equivalent XmlDocument
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        private static XmlDocument ConvertXmlFile(string sFilePath)
        {
            XmlDocument xmlRaw = null;
          
            try
            {
                string sXmlContents = FileUtilities.ReadFileContents(sFilePath);
                sXmlContents = sXmlContents.Replace("&", "and");
                
                xmlRaw = new XmlDocument();
                xmlRaw.LoadXml(sXmlContents);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ConvertXmlFile", ex);
            }
            finally
            {                
            }

            return xmlRaw;
        }

        /// <summary>
        /// Converts the specified AccessDB file to it's equivalent XmlDocument
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="sAccessDbQuery"></param>
        /// <returns></returns>
        private static XmlDocument ConvertAccessDatabaseFile(string sFilePath)
        {
            XmlDocument xmlRaw = null;
            System.Data.OleDb.OleDbConnection oleConnection = null;

            try
            {                
                string sProviderInfo = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + sFilePath + ";";

                oleConnection = new System.Data.OleDb.OleDbConnection(sProviderInfo);
                System.Data.OleDb.OleDbDataAdapter oleCommand = new System.Data.OleDb.OleDbDataAdapter(AccessDatabaseQuery, oleConnection);

                System.Data.DataSet dsAccess = new System.Data.DataSet();
                oleCommand.Fill(dsAccess);

                xmlRaw = CleanupRawXml(dsAccess.GetXml());                
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ConvertAccessDatabaseFile", ex);
            }
            finally
            {
                oleConnection.Close();
            }

            return xmlRaw;
        }

        /// <summary>
        /// Converts the specified CSV file to it's equivalent XmlDocument
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        private static XmlDocument ConvertCsvFile(string sFilePath)
        {
            XmlDocument xmlRaw = null;
            System.Data.OleDb.OleDbConnection oleConnection = null;

            try
            {
                FileInfo fiInfo = new FileInfo(sFilePath);

                oleConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fiInfo.DirectoryName + ";Extended Properties=\"Text;HDR=no;FMT=Delimited\";");
                System.Data.OleDb.OleDbDataAdapter oleCommand = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + fiInfo.Name + "]", oleConnection);

                System.Data.DataSet dsCsvFile = new System.Data.DataSet();
                oleCommand.Fill(dsCsvFile);

                xmlRaw = CleanupRawXml(dsCsvFile.GetXml());                             
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ConvertCsvFile", ex);
            }
            finally
            {
                oleConnection.Close();
            }

            return xmlRaw;
        }

        /// <summary>
        /// Creates the datatable columns based upon a string array, or creates a numbered set if no valid names exist
        /// </summary>
        /// <param name="sColumns"></param>
        /// <param name="dtTable"></param>
        /// <param name="bUseNamedColumns"></param>
        private static bool InitializeTableColumns(string[] sColumns, ref DataTable dtTable, bool bUseNamedColumns)
        {
            bool bHeaderIsDataRow = false;

            // clear out all the data, since it's probably a header or something
            if ((dtTable.Rows.Count > 1) && (dtTable.Rows[0].ItemArray.Length > MINIMUM_NUMBER_CSV_COLUMNS))
                throw new Exception("InitializeTableColumns: valid data may be present!");

            dtTable.Rows.Clear();
            dtTable.Columns.Clear();

            // ensure we can use named columns
            if (bUseNamedColumns == true)
            {
                foreach (string sColName in sColumns)
                {
                    // if any of the columns is a number, then it's not valid to use named columns
                    string sColumnName = sColName.Trim();
                    if (Validation.IsNumeric(sColumnName) == true)
                    {
                        bUseNamedColumns = false;
                        break;
                    }
                }
            }

            // add/name all the columns
            if (bUseNamedColumns == true)
            {
                foreach (string sCol in sColumns)
                {
                    // rename each header column properly
                    string sColumn = sCol.Trim();
                    sColumn = sColumn.Replace("#", "No");
                    sColumn = sColumn.Replace("%", "");
                    sColumn = sColumn.Replace(" ", "");
                    sColumn = sColumn.Replace(@"\", "_");
                    sColumn = sColumn.Replace(@"/", "_");

                    dtTable.Columns.Add(sColumn);
                }
            }
            else
            {
                int iColumnIndex = 0;
                foreach (string sCol in sColumns)
                {
                    dtTable.Columns.Add("Column" + iColumnIndex.ToString());
                    iColumnIndex += 1;
                }
            }

            bHeaderIsDataRow = (bUseNamedColumns == false);
            return bHeaderIsDataRow;
        }

        /// <summary>
        /// Converts the specified TEXT file to it's equivalent XmlDocument
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        private static XmlDocument ConvertTextFile(string sFilePath)
        {
            XmlDocument xmlRaw = null;
            StreamReader oSR = null;             

            try
            {               
                DataSet dsTextFile = new DataSet();
		        DataTable dtTextFile = new DataTable();
		        DataRow drRows = null;

                // check and pre-process the text file if it's a non-standard text file
                sFilePath = PreprocessNonStandardFiles(sFilePath);

                // find the correct delimiter for the file (some files have multiple delimiting chars, but only one is correct)
                char chrDelimiter = GetDelimiterCharacter(sFilePath);

                //Open the file and go to the top of the file		        
                oSR = new StreamReader(sFilePath);                
		        oSR.BaseStream.Seek(0, SeekOrigin.Begin);

                // read the first line 
                string sFirstLine = oSR.ReadLine();
                bool bHeaderIsDataRow = false;

                // init the columns if the file has a valid, parsible header
                string[] sColumns = sFirstLine.Split(chrDelimiter);
                if(sColumns.Length > MINIMUM_NUMBER_CSV_COLUMNS)
                {                    
                    bHeaderIsDataRow = InitializeTableColumns(sColumns, ref dtTextFile, true);
                    if (bHeaderIsDataRow == true)
                    {
                        oSR.BaseStream.Seek(0, SeekOrigin.Begin);
                        oSR.Close();
                        oSR = new StreamReader(sFilePath);
                        oSR.BaseStream.Seek(0, SeekOrigin.Begin);
                    }
                }		      

		        // add in the Rows for the datatable/file
                dsTextFile.DataSetName = "NewDataSet";
		        dsTextFile.Tables.Clear();
                dtTextFile.TableName = "Table";
                dsTextFile.Tables.Add(dtTextFile);

                // iterate thru the file and process each line
		        while (oSR.Peek() > -1)
		        {			        
                    int iFieldIndex = 0;
                    string sLine = oSR.ReadLine();
                    string sLineTrimmed = sLine.Trim();     // used to make sure there's no crap in the tab delimited file
                    string[] sLineFields = sLine.Split(chrDelimiter);

                    if ((sLineFields.Length <= 0) || (sLineTrimmed.Length < MINIMUM_NUMBER_CSV_COLUMNS))
                    {
                        continue;
                    }

                    // if the number of fields is less that the minimum, skip the field
                    if (sLineFields.Length <= MINIMUM_NUMBER_CSV_COLUMNS)
                    {
                        continue;
                    }

                    // if we suddenly have more fields than columns, we're in a header or something, so re-init the columns
                    if ((sLineFields.Length > dtTextFile.Columns.Count) && (sLineFields.Length > MINIMUM_NUMBER_CSV_COLUMNS))
                    {
                        //note: there may be a bug here - header/inconsistent file delimiting problems?  
                        if (dtTextFile.Rows.Count <= 0)
                        {
                            InitializeTableColumns(sLineFields, ref dtTextFile, false);
                        }
                    }

                    drRows = dtTextFile.NewRow();
                    foreach (string strField in sLineFields)
			        {
                        string sField = strField.Trim();
                        sField = sField.Replace("\"", "");
                        sField = sField.Replace("'", "");
                        sField = sField.Replace("$", "");
                        sField = sField.Replace("%", "");
                        sField = sField.Replace("-0-", "0");
                        sField = sField.Replace("&", "and");

                        //note: there may be a bug here - header/inconsistent file delimiting problems?  
                        if (dtTextFile.Columns.Count <= iFieldIndex)
                            break;

                        drRows[iFieldIndex] = sField;
                        iFieldIndex = iFieldIndex + 1;
			        }
                    
			        dtTextFile.Rows.Add(drRows);
		        }		

                // load the dataset to an xmldocument                
                xmlRaw = CleanupRawXml(dsTextFile.GetXml());                
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ConvertTextFile", ex);
            }
            finally
            {
                oSR.Close();
            }

            return xmlRaw;
        }

        /// <summary>
        /// Scan the first line of the file and if it needs preprocessing, preprocess and rewrite the file
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        private static string PreprocessNonStandardFiles(string sFilePath)
        {
            string sNewFilePath = sFilePath;

            StreamReader oSR = null;

            try
            {
                //Open the file and go to the top of the file		        
                oSR = new StreamReader(sFilePath);
                oSR.BaseStream.Seek(0, SeekOrigin.Begin);

                string sFirstLine = oSR.ReadLine().Trim().ToLower();
                oSR.Close();

                if (sFirstLine.IndexOf(QUOTED_COMMA_DELIMITER) > -1)
                {
                    sNewFilePath = PreprocessQuotedCommaDelimiterFile(sFilePath);                
                }
                else
                {
                    sNewFilePath = sFilePath;
                }                
            }
            catch (Exception ex)
            {                
                throw new Exception("Error: ConvertTextFile", ex);
            }

            return sNewFilePath;
        }

        /// <summary>
        /// Some files are delimited by "," strings, so we need to replace them with pipes all thru the file
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        private static string PreprocessQuotedCommaDelimiterFile(string sFilePath)
        {
            string sNewFilePath = sFilePath;

            StreamReader oSR = null;

            try
            {
                //Open the file and go to the top of the file		        
                oSR = new StreamReader(sFilePath);
                oSR.BaseStream.Seek(0, SeekOrigin.Begin);

                StringBuilder sbFileContents = new StringBuilder();

                // iterate thru the file and process each line
                while (oSR.Peek() > -1)
                {
                    string sLine = oSR.ReadLine().Trim();
                    if (sLine.Length <= 0)
                    {
                        continue;
                    }

                    sLine = sLine.Replace("\" ,\"", "|");
                    sLine = sLine.Replace("\",\"", "|");
                    sLine = sLine.Replace("\", \"", "|");
                    sLine = sLine.Replace("\"", string.Empty);

                    sbFileContents.AppendLine(sLine);
                }

                FileInfo fiInfo = new FileInfo(sFilePath);
                sNewFilePath = fiInfo.FullName.Replace(fiInfo.Extension, "-preproc" + fiInfo.Extension);

                FileUtilities.WriteFileContents(sNewFilePath, sbFileContents.ToString(), true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: PreprocessQuotedCommaDelimiterFile", ex);
            }
            finally
            {
                oSR.Close();
            }

            return sNewFilePath;
        }


        /// <summary>
        /// Scan the file and attempt to find the correct delimiter character, then return it (or throw exception)
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public static char GetDelimiterCharacter(string sFilePath)
        {
            bool bDelimiterFound = false;
            char chrDelimiter = ',';
            StreamReader oSR = null;

            // foreach char type, read every line in the file to ensure they dimit properly
            // if every line delimits to the same number of columns, then set the return char delimiter (we found it)
            // if there's any mismatch, then the file is not delimited properly and so try the next delimiter
            foreach (char chrTest in ALLOWED_DELIMITER_CHARACTERS)
            { 
                try
                {
                    //Go to the top of the file		
                    oSR = new StreamReader(sFilePath);
                    oSR.BaseStream.Seek(0, SeekOrigin.Begin);

                    int nLineCount = 0;
                    int nSplitLineCount = 0;
                    int nColumnCount = 0;

                    while (oSR.Peek() > -1)
                    {
                        string sFileLine = oSR.ReadLine();
                        if (sFileLine.Trim() == string.Empty)
                            continue;

                        string[] sSplitFields = sFileLine.Split(chrTest);

                        // line split ok, so check the number of fields
                        if (sSplitFields.Length > 1)
                        {
                            // line is valid, so bump our total line count
                            nLineCount += 1;

                            // set to the first split count we find
                            if (nColumnCount == 0)
                                nColumnCount = sSplitFields.Length;

                            // if the newcolumncount is greater, then we've moved past the header so keep going
                            // increment the splitcount 
                            if (sSplitFields.Length >= nColumnCount)
                            {
                                if (sSplitFields.Length > MINIMUM_NUMBER_CSV_COLUMNS)
                                    // bump the splitline count
                                    nSplitLineCount += 1;
                            }
                            else
                            {
                                // if the new split is less that the original count, then its' the wrong delimiter
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    // if the number of lines match the number split, the current delimiter is the one for the file
                    if ((nSplitLineCount > 0) && (nLineCount >= nSplitLineCount))
                    {
                        chrDelimiter = chrTest;     
                        bDelimiterFound = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error: GetDelimiterCharacter", ex);
                }
                finally
                {
                    oSR.Close();
                }                
            }

            if (bDelimiterFound == false)
                throw new Exception("GetDelimiterCharacter:  Could not properly discover the delimiting character in the file (missing chars?)");

            return chrDelimiter;
        }


        /// <summary>
        /// Remove all the junk tables at the end of the spreadsheet/xml document
        /// </summary>
        /// <param name="xmlRaw"></param>
        /// <returns></returns>
        public static XmlDocument CleanupRawXml(string sXmlRaw)
        {
            sXmlRaw = sXmlRaw.Replace(XML_PRESERVE_WHITESPACE_ATTRIBUTE, string.Empty);

            XmlDocument xmlRaw = new XmlDocument();
            xmlRaw.LoadXml(sXmlRaw);

            // if not allowed to validate, return
            if (m_bPerformValidationCleanup == false)
            {
                return xmlRaw;
            }

            foreach (XmlNode xmlnode in xmlRaw.SelectNodes(NEWDATASET_TABLE_ROOT))
            {
                bool bFoundRequired = false;
                foreach (string sRequired in REQUIRED_ELEMENTS)
                {
                    XmlNode xnode = xmlnode.SelectSingleNode(sRequired);
                    if ((xnode != null) && (xnode.InnerText.Trim() != string.Empty))
                    {
                        // some excel files put totals at the bottom, so if there's a column0, there MUST be a column1
                        if (sRequired == "Column0")
                        {
                            XmlNode xnode1 = xmlnode.SelectSingleNode("Column1");
                            if ((xnode1 != null) && (xnode1.InnerText.Trim() != string.Empty))
                            {
                                bFoundRequired = true;
                                break;
                            }
                        }
                        else
                        {
                            bFoundRequired = true;
                            break;
                        }
                    }
                }

                if (bFoundRequired == false)
                {
                    xmlnode.RemoveAll();
                    xmlnode.ParentNode.RemoveChild(xmlnode);
                }
            }

            return xmlRaw;
        }

    }
}
