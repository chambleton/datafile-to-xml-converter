using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace FileToXmlConverter
{

    public class XmlTranslator
    {                        

        /// <summary>
        /// Overload: Perform the xml translation 
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="sXslSheetPath"></param>
        /// <returns></returns>
        public static XmlDocument TransformXml(string sFilePath, string sXslSheetPath)
        {
            XmlDocument xmlRaw = new XmlDocument();
            xmlRaw.Load(sFilePath);  
          
            XslCompiledTransform xslSheet = new XslCompiledTransform();
            
            try
            {
                xslSheet.Load(sXslSheetPath);                
            }
            catch (Exception ex)
            {
                throw new Exception("TransformXml:Load", ex.InnerException);
            }

            return TransformXml(xmlRaw, xslSheet);
        }

        /// <summary>
        /// Perform the xml translation
        /// </summary>
        /// <param name="xmlRaw"></param>
        /// <param name="xslSheet"></param>
        /// <returns></returns>
        public static XmlDocument TransformXml(XmlDocument xmlRawClean, XslCompiledTransform xslSheet)
        {            
            XmlDocument xmlTransform = null;            
            XmlTextWriter xmlWriter = null;

            try
            {                
                XPathDocument xmlClean = ConvertXmlDocumentToXPathDocument(xmlRawClean);

                string sTempPath = FileUtilities.GetUniqueTempFileName();
                xmlWriter = new XmlTextWriter(sTempPath, null);
                xslSheet.Transform(xmlClean, null, xmlWriter);
                xmlWriter.Close();

                xmlTransform = new XmlDocument();
                xmlTransform.Load(sTempPath);

                // try to clean up the file we created
                try
                {
                    File.Delete(sTempPath);
                }
                catch
                {

                }
            }
            catch (Exception ex)
            {
                throw new Exception("TransformXml", ex.InnerException);
            }

            return xmlTransform;
        }

        /// <summary>
        /// Converts an XmlDocument to a read-only XPathDocument
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static XPathDocument ConvertXmlDocumentToXPathDocument(XmlDocument xmldoc)
        {
            XPathDocument xmlClean = null;
            string sTempClean = FileUtilities.GetUniqueTempFileName();

            // try to save, load, then clean up the file we created
            try
            {
                xmldoc.Save(sTempClean);
                //File.Copy(sTempClean, @"c:\temp\cleaned_output.txt");     // for debugging
                xmlClean = new XPathDocument(sTempClean);

                if (File.Exists(sTempClean) == true)
                    File.Delete(sTempClean);
            }
            catch (Exception ex)
            {
                throw new Exception("ConvertXmlDocument error: ", ex);
            }
            finally
            {
                if (File.Exists(sTempClean) == true)
                    File.Delete(sTempClean);
            }

            return xmlClean;
        }

    }
}
