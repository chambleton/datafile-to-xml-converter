using System;
using System.Data;
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
    /// This class validates a Remittance XML file against the Remittance schema file
    /// </summary>
    public class XmlValidator
    {
        private static bool m_bXmlFileValid = false;


        private static XmlSchemaException m_SchemaException = null;
        public static XmlSchemaException SchemaException
        {
            get { return m_SchemaException; }
        }

        private static XmlSeverityType m_SeverityType = XmlSeverityType.Warning;
        public static XmlSeverityType SeverityType
        {
            get { return m_SeverityType; }
        }

        private static string m_sValidationErrorMessage = string.Empty;
        public static string ValidationErrorMessage
        {
            get { return m_sValidationErrorMessage; }
        }

        /// <summary>
        /// Overload:  Validates an xmldocument according to the xmlschema
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="sXslSchemaPath"></param>
        /// <returns></returns>
        public static bool Validate(string sFilePath, string sXslSchemaPath)
        {
            XmlDocument xmlRaw = new XmlDocument();
            xmlRaw.Load(sFilePath);

            XmlSchema xmlSchema = null;

            try
            {
                // if schema file is empty, just return true (this is for types that cannot be validated)
                if (FileUtilities.ReadFileContents(sXslSchemaPath).Trim().Length < 20)
                    return true;

                XmlTextReader xmlReader = new XmlTextReader(sXslSchemaPath);
                xmlSchema = XmlSchema.Read(xmlReader, new ValidationEventHandler(SchemaReadError));
            }
            catch (Exception ex)
            {
                throw new Exception("Validate:LoadFileOrSchema", ex);
            }

            return Validate(xmlRaw, xmlSchema);
        }

        /// <summary>
        /// Overload:  Validates an xmldocument according to the xmlschema
        /// </summary>
        /// <param name="xmlRaw"></param>
        /// <param name="sXslSchemaPath"></param>
        /// <returns></returns>
        public static bool Validate(XmlDocument xmlRaw, string sXslSchemaPath)
        {
            XmlSchema xmlSchema = null;            

            try
            {
                // if schema file is empty, just return true (this is for types that cannot be validated)
                if (FileUtilities.ReadFileContents(sXslSchemaPath).Trim().Length < 20)
                    return true;

                XmlTextReader xmlReader = new XmlTextReader(sXslSchemaPath);
                xmlSchema = XmlSchema.Read(xmlReader, new ValidationEventHandler(SchemaReadError));
            }
            catch (Exception ex)
            {
                throw new Exception("Validate:LoadSchema", ex);
            }

            return Validate(xmlRaw, xmlSchema);
        }

        /// <summary>
        /// Validates an xmldocument according to the xmlschema
        /// </summary>
        /// <param name="xmlRaw"></param>
        /// <param name="xmlSchema"></param>
        /// <returns></returns>
        public static bool Validate(XmlDocument xmlRaw, XmlSchema xmlSchema)
        {
            m_bXmlFileValid = true; // set to true, since the error handler will set it to 'false'

            m_SchemaException = null;
            m_sValidationErrorMessage = string.Empty;
            m_SeverityType = XmlSeverityType.Warning;

            XmlValidatingReader xmlValidator = null;
            string sTempXmlFile = FileUtilities.GetUniqueTempFileName();
            
            try
            {
                if (File.Exists(sTempXmlFile) == true)
                    File.Delete(sTempXmlFile);
                
                xmlRaw.Save(sTempXmlFile);

                //Get Validator
                XmlTextReader xmlReader = new XmlTextReader(sTempXmlFile);
                xmlValidator = new XmlValidatingReader(xmlReader);

                //Assign Schema
                xmlValidator.Schemas.Add(xmlSchema);
                xmlValidator.ValidationType = ValidationType.Schema;
                xmlValidator.ValidationEventHandler += new ValidationEventHandler(ValidationError);

                //Validate the entire Document Node By Node
                while (xmlValidator.Read());
            }
            catch (Exception ex)
            {
                throw new Exception("Validation exception: ", ex);
            }
            finally
            {
                if(xmlValidator != null)
                    xmlValidator.Close();

                xmlValidator = null;

                if (File.Exists(sTempXmlFile) == true)
                    File.Delete(sTempXmlFile);
            }

            return m_bXmlFileValid;
        }

        /// <summary>
        /// Handles a XmlValidation exception/error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        private static void ValidationError(object sender, ValidationEventArgs arguments)
        {
            m_SchemaException = arguments.Exception;
            m_sValidationErrorMessage = arguments.Message;
            m_SeverityType = arguments.Severity;

            m_bXmlFileValid = false;            
        }

        /// <summary>
        /// Handles a Schema read exception/error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        private static void SchemaReadError(object sender, ValidationEventArgs arguments)
        {
            m_SchemaException = arguments.Exception;
            m_sValidationErrorMessage = arguments.Message;
            m_SeverityType = arguments.Severity;

            throw new Exception("SchemaReadError: " + arguments.Message);
        }

        /// <summary>
        /// Creates a schema file from an xmldocument object
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static string CreateXmlSchema(XmlDocument xmldoc)
        {
            string sXmlSchemaPath = string.Empty;

            try
            {
                string sXmlFilePath = FileUtilities.GetUniqueTempFileName();
                xmldoc.Save(sXmlFilePath);

                sXmlSchemaPath = CreateXmlSchema(sXmlFilePath);
                File.Delete(sXmlFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("CreateXmlSchema", ex);
            }

            return sXmlSchemaPath;
        }

        /// <summary>
        /// Creates a schema file from an xmldocument file
        /// </summary>
        /// <param name="sXmlFilePath"></param>
        /// <returns></returns>
        public static string CreateXmlSchema(string sXmlFilePath)
        {
            string sXmlSchemaPath = FileUtilities.GetUniqueTempFileName();

            try
            {
                DataSet dsXmlDoc = new DataSet();
                dsXmlDoc.ReadXml(sXmlFilePath);
                dsXmlDoc.WriteXmlSchema(sXmlSchemaPath);
                
                dsXmlDoc.Clear();
                dsXmlDoc = null;
            }
            catch (Exception ex)
            { 
                throw new Exception("CreateXmlSchema", ex);
            }

            return sXmlSchemaPath;
        }

    }
}
