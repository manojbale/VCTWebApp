using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using System.Threading;
using System.Collections.Generic;

namespace VCTWeb.Core.Domain
{
    public class Helper
    {
        
        public string GetAppSettingsValue(string key)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                return ConfigurationManager.AppSettings[key];
            }
            else
            {
                return string.Empty;
            }
        }

        public bool UpdateAppSettings(string key, string value)
        {
            try
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (config.AppSettings.Settings[key] != null)
                {
                    config.AppSettings.Settings.Remove(key);
                }

                config.AppSettings.Settings.Add(key, value);

                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CopyLocalFile(string localPath, string destinationPath, string fileName, out string copiedFilePath, params string[] validExtensions)
        {
            return CopyLocalFile(localPath, Path.Combine(destinationPath, fileName), out copiedFilePath, validExtensions);
        }

        public bool CopyLocalFile(string localPath, string destinationPath, out string copiedFilePath, params string[] validExtensions)
        {
            copiedFilePath = string.Empty;

            if (File.Exists(localPath))
            {
                FileInfo localFileInfo = new FileInfo(localPath);
                FileInfo destinationFileInfo = new FileInfo(destinationPath);

                string currentFileExtension = localFileInfo.Extension.Substring(1);

                bool validFileExtension = IsValidFile(validExtensions, currentFileExtension);

                if (validFileExtension)
                {
                    return CopyFile(localPath, destinationPath, destinationFileInfo, currentFileExtension, out copiedFilePath);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool CopyFile(string localPath, string destinationPath, FileInfo destinationFileInfo, string currentFileExtension, out string copiedFilePath)
        {
            if (!Directory.Exists(destinationFileInfo.DirectoryName))
            {
                Directory.CreateDirectory(destinationFileInfo.DirectoryName);
            }

            copiedFilePath = destinationPath + "." + currentFileExtension;

            File.Copy(localPath, copiedFilePath, true);

            return true;
        }

        private bool IsValidFile(string[] validExtensions, string currentFileExtension)
        {
            bool validFileExtension = false;

            if (validExtensions != null && validExtensions.Length > 0)
            {
                foreach (string validExtension in validExtensions)
                {
                    if (object.Equals(currentFileExtension.ToUpper(CultureInfo.InvariantCulture), validExtension.ToUpper(CultureInfo.InvariantCulture)))
                    {
                        validFileExtension = true;
                        break;
                    }
                }
            }
            else
            {
                validFileExtension = true;
            }
            return validFileExtension;
        }

        public string GetString(object[] obj)
        {
            try
            {
                string XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();

                XmlSerializer xs = new XmlSerializer(typeof(object[]));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;

                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

                return XmlizedString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string GetStringMasterLookUp(object obj)
        {
            try
            {
                string XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();

                XmlSerializer xs = new XmlSerializer(typeof(object));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;

                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

                return XmlizedString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public double? GetNullableDouble(string value)
        {
            double dValue;

            if (double.TryParse(value, out dValue))
            {
                return dValue;
            }
            else
            {
                return null;
            }
        }

        public int? GetNullableInteger(string value)
        {
            int iValue;

            if (int.TryParse(value, out iValue))
            {
                return iValue;
            }
            else
            {
                return null;
            }
        }

        public int GetInteger(string value)
        {
            int iValue;

            if (int.TryParse(value, out iValue))
            {
                return iValue;
            }
            else
            {
                return 0;
            }
        }

        public int GetIfPositiveInteger(string value)
        {
            int iValue;

            if (int.TryParse(value, out iValue))
            {
                if (iValue > 0)
                    return iValue;
                else
                    return -1;
            }
            else
            {
                return -1;
            }
        }

        public uint GetUnsignedInteger(string value)
        {
            uint iValue;

            if (uint.TryParse(value, out iValue))
            {
                return iValue;
            }
            else
            {
                return 0;
            }
        }

        public double GetDouble(string value)
        {
            double dValue;

            if (double.TryParse(value, out dValue))
            {
                return dValue;
            }
            else
            {
                return -1;
            }
        }

        public decimal GetDecimal(string value)
        {
            decimal dValue;

            if (decimal.TryParse(value, out dValue))
            {
                return dValue;
            }
            else
            {
                return -1M;
            }
        }

        /// <summary>
        /// Determines whether [is valid decimal] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid decimal] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidDecimal(decimal value)
        {
            if (value != -1M)
            {
                string strValue = value.ToString().Replace("-", "");
                if (strValue.IndexOf('.') > 4)
                    return false;
                else if (strValue.IndexOf('.') == -1 && strValue.Length > 4)
                {
                    return false;
                }
            }

            return true;
        }

        private string UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters, 0, characters.Length);
            return (constructedString.Trim());
        }

        public void LogError(Exception exceptionObject)
        {
            ILog logger = LogManager.GetLogger("VCTWeb");
            logger.Error(exceptionObject.Message, exceptionObject);
        }

        public void LogInformation(string userName, string moduleName, string informationString)
        {
            ILog logger = LogManager.GetLogger("VCTWeb");
            logger.Info("User : " + userName + " || Module : " + moduleName + " ==> " + informationString);
        }

        public void LogInformationHandheld(string userName, string moduleName, string informationString)
        {
            ILog handheldLogger = LogManager.GetLogger("Handheld");
            handheldLogger.Info("User : " + userName + " || Module : " + moduleName + " ==> " + informationString);
        }
        public void LogInformationRFID(string userName, string moduleName, string informationString)
        {
            ILog rfidLogger = LogManager.GetLogger(Constants.RFID_USER);
            rfidLogger.Info("User : " + userName + " || Module : " + moduleName + " ==> " + informationString);
        }
        public void LogInformationALESubscriber(string userName, string moduleName, string informationString)
        {
            ILog rfidLogger = LogManager.GetLogger("ALESubscriber");
            rfidLogger.Info("User : " + userName + " || Module : " + moduleName + " ==> " + informationString);
        }
        public void LogInformationSVTagEvntSubscriber(string userName, string moduleName, string informationString)
        {
            ILog rfidLogger = LogManager.GetLogger("SVTagEventSubscriber");
            rfidLogger.Info("User : " + userName + " || Module : " + moduleName + " ==> " + informationString);
        }
        public void LogErrorALE(Exception ex)
        {
            ILog rfidLogger = LogManager.GetLogger("ALESubscriber");
            rfidLogger.Error(ex.Message, ex);
        }
        public void LogErrorSVTagEvent(Exception ex)
        {
            ILog rfidLogger = LogManager.GetLogger("SVTagEventSubscriber");
            rfidLogger.Error(ex.Message, ex);
        }

        public void LogErrorHandheld(Exception ex)
        {
            ILog handheldLogger = LogManager.GetLogger("Handheld");
            handheldLogger.Error(ex.Message, ex);
        }
        public void LogErrorRFID(Exception ex)
        {
            ILog rfidLogger = LogManager.GetLogger(Constants.RFID_USER);
            rfidLogger.Error(ex.Message, ex);
        }

        //public string PrintDefectedTicket()
        //{
        //    //return ConfigurationManager.AppSettings["PrintDefectiveTicket"].ToString();
        //    return ConfigSettings.GetValue(ConfigSettings.PRINT_DEFECTIVE_TICKET);
        //}

        public String DBError(System.Data.SqlClient.SqlException databaseException)
        {
            Int32 IntErrorNumber = ((System.Data.SqlClient.SqlException)databaseException).Number;
            string errMsg = string.Empty;

            if (IntErrorNumber == 2601 || IntErrorNumber == 2627)
            {
                errMsg = "msgValExists";
            }
            else if (IntErrorNumber == 50000)
            {
                errMsg = "msgValIsInUse";
            }
            else
            {
                errMsg = "msgDatabaseError";
            }
            return errMsg;
        }

        public string GetTempFolder()
        {
            // Get the folder for temporary image files from Web.Config.
            string imageTempFolder = ConfigurationManager.AppSettings["ImageTempFolder"];
            if (string.IsNullOrEmpty(imageTempFolder))
                imageTempFolder = "/Temp";

            return imageTempFolder;

        }

        public string GetEntityXml<T>(List<T> lstObjects)
        {
            if (lstObjects.Count > 0)
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("root"));
                        serializer.Serialize(xmlWriter, lstObjects);
                        for (int i = 0; i < lstObjects.Count; i++)
                            ((IDisposable)lstObjects[i]).Dispose();
                        return stringWriter.ToString();
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }

        //public  string GetHelpFile()
        //{
        //    return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ScanWare 2009.chm");
        //}
    }

}
