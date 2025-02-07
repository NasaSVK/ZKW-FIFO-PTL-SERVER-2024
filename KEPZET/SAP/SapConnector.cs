using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Diagnostics;
using System.Windows.Forms;

namespace KEPZET.sap
{


    public delegate void DelLog(string message, LogType pType, int pEventID = -1);

    //klucova trieda dodavajuca metody controleru suvisiace s pristupom do SAP_DB
    public class SapConnector
    {
        private const string DateFormat = "yyyyMMdd";

        static public DelLog delLog;

        internal static RfcDestination destination = null;
        internal static DestinationConfig destinationConfig = null;

        /// <summary>
        /// Loads First Pallet of definite from SAP
        /// </summary>
        /// <param name="pPalletID"></param>        
        /// <returns>PartNumber of the Palet</returns>
        public static PN_DATE_TIME getPNFromPalletId(string pPalletID)
        {
                                    
            //String[] retVal = new String[10];
            PN_DATE_TIME result = new PN_DATE_TIME();
           
            try
            {
                
                //RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                //destination = RfcDestinationManager.GetDestination(Constants.SapSystemHr);

                RfcRepository repo = destination.Repository;
                IRfcFunction function = repo.CreateFunction(Constants.FunctionGetPNFromPalletID);

                // passing parameters
                function.SetValue(Constants.ImportParameterCompany, Constants.CompanyValue);
                function.SetValue(Constants.ImportParameterSite, Constants.SiteValue);
                function.SetValue(Constants.ImportParameterPalletID2, pPalletID);               
                function.Invoke(destination);

                // loading export parameter and converting Object value to string
                result.PN = function.GetValue(Constants.SapExportParameterPartNumber).ToString();
                result.GrDate = function.GetValue(Constants.SapExportParameterDate).ToString();
                result.GrTime = function.GetValue(Constants.SapExportParameterTime).ToString();

                if (result.PN.Trim() == "Box instead of pallet scanned") return result;
                                

                //ADAM FORCE
                //result.PN = "1277.305.0000.99";
                //result.GrDate = "08/16/2022";
                //result.GrTime = "00:00:00"; 

                //RfcSessionManager.EndContext(destination);
                //RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
            }
            catch (Exception ex)
            {
                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
                Thread.Sleep(1000);
                //MessageBox.Show("SAP command execution error:/n" + ex.ToString()) ;
                delLog("\n *** SAP command execution error:/n" + ex.ToString()+ " *** \n",LogType.error);
                //mEventLog.WriteEntry("Xpert command execution error:" + ex.ToString(), EventLogEntryType.Error);
                //if (String.IsNullOrEmpty(result.PN) || String.IsNullOrEmpty(result.PN) || String.IsNullOrEmpty(result.PN))
                //    new NullReferenceException("No data returned from SAP");
            }

            return result;
        }



        /// <summary>
        /// Loads First Pallet of definite from SAP
        /// </summary>
        /// <param name="pPalletID"></param>        
        /// <returns>PartNumber of the Palet</returns>
        internal static string getFirstPallet(string pPN, string pGRDate, string pGRTime, string pID, string pLPPID)
        {
                        
            //RfcDestination destination = null;
            //String[] retVal = new String[10];
            string result = null; 

            try
            {                
                //RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                //destination = RfcDestinationManager.GetDestination(Constants.SapSystemHr);               
                RfcRepository repo = destination.Repository;

                IRfcFunction function = repo.CreateFunction(Constants.FunctionGetFirstPallet);

                // passing parameters
                function.SetValue(Constants.ImportParameterCompany, Constants.CompanyValue);
                function.SetValue(Constants.ImportParameterSite, Constants.SiteValue);
                function.SetValue(Constants.ImportParameterPalletID2, pID);
                function.SetValue(Constants.ImportParameterPartNumber, pPN);                
                function.SetValue(Constants.ImportParameterLastProcessedPalletID, pLPPID);
                function.SetValue(Constants.ImportParameterGRDate, pGRDate);
                function.SetValue(Constants.ImportParameterGRTIME, pGRTime);

                function.Invoke(destination);

                // loading export parameter and converting Object value to string
                
                result = Helpers.odstranNuly(function.GetValue(Constants.SapExportParameterFirst).ToString());

                //object obj = function.GetValue(Constants.SapExportParameterPartNumber);

                //RfcSessionManager.EndContext(destination);
                //RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
            }
            catch (Exception ex)
            {
                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
                Thread.Sleep(1000);
                //MessageBox.Show("SAP command execution error:/n" + ex.ToString());
                delLog("\n  ** SAP command execution error:/n" + ex.ToString() + " *** \n", LogType.error);
                //mEventLog.WriteEntry("Xpert command execution error:" + ex.ToString(), EventLogEntryType.Error);
            }

            return result;
        }


        /// <summary>
        /// Loads First Pallet of definite from SAP
        /// </summary>
        /// <param name="pPalletID"></param>        
        /// <returns>PartNumber of the Palet</returns>
        internal static string getPreviousPallet(string pPN, string pGRDate, string pGRTime, string pID)
        {
            
            //RfcDestination destination = null;
            //String[] retVal = new String[10];
            string result = null;

            try
            {
                //destinationConfig = new DestinationConfig();
                //RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                //destination = RfcDestinationManager.GetDestination(Constants.SapSystemHr);
                RfcRepository repo = destination.Repository;

                IRfcFunction function = repo.CreateFunction(Constants.FunctionGetPreviousPallet);

                // passing parameters
                function.SetValue(Constants.ImportParameterCompany, Constants.CompanyValue);
                function.SetValue(Constants.ImportParameterSite, Constants.SiteValue);
                function.SetValue(Constants.ImportParameterPalletID2, pID);
                function.SetValue(Constants.ImportParameterPartNumber, pPN);                
                function.SetValue(Constants.ImportParameterGRDate, pGRDate);
                function.SetValue(Constants.ImportParameterGRTIME, pGRTime);

                function.Invoke(destination);


                //result = "3000003204"; //ADAM FORCE
                // loading export parameter and converting Object value to string                

                Object resultO = function.GetValue(Constants.SapExportParameterPrevious);
                result = Helpers.odstranNuly(resultO.ToString());
                

                //object obj = function.GetValue(Constants.SapExportParameterPartNumber);

                //RfcSessionManager.EndContext(destination);
                //RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
            }
            catch (Exception ex)
            {
                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
                Thread.Sleep(1000);
                //MessageBox.Show("SAP command execution error:/n" + ex.ToString());
                delLog("\n *** SAP command execution error:/n" + ex.ToString() + " ***\n", LogType.error);
                //mEventLog.WriteEntry("Xpert command execution error:" + ex.ToString(), EventLogEntryType.Error);
            }

            return result;
        }




        /// <summary>
        /// Loads projects from SAP which have been created or changed in the given date range
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>Projects</returns>
        /*
        internal static List<Project> LoadSapProjects(DateTime dateFrom, DateTime dateTo)
        {
            List<Project> projectList = new List<Project>();
            DestinationConfig destinationConfig = null;
            RfcDestination destination = null;

            try
            {
                destinationConfig = new DestinationConfig();
                RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                destination = RfcDestinationManager.GetDestination(Constants.SapSystemErp);

                RfcRepository repo = destination.Repository;
                IRfcFunction function = repo.CreateFunction(Constants.SapFunctionGetProjects);

                // passing parameters
                function.SetValue(Constants.SapImportParameterDateFromEN, dateFrom.ToString(DateFormat));
                function.SetValue(Constants.SapImportParameterDateToEN, dateTo.ToString(DateFormat));
                function.Invoke(destination);                
                
                // loading export parameter
                var exObject = function.GetTable(Constants.SapExportParameterProjects);
                var dataTable = RfcTableExtension.ToDataTable(exObject, Constants.SapExportParameterProjects);

                // converting dataTable to List<Project>
                projectList = dataTable.AsEnumerable().Select(m => new Project()
                {
                    PspID = m.Field<string>(Constants.SapTableColumnPspID),
                    Description = m.Field<string>(Constants.SapTableColumnDescription)
                }).ToList();

                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
            }
            catch (Exception)
            {
                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
                Thread.Sleep(1000);
                throw;
            }

            return projectList;
        }*/



        /// <summary>
        /// Opens connection to SAP and performs a remote function call (RFC).
        /// Loads all booked ours from SAP CATS withing the passed range and returns them in a convenient list of booked hours.
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        /*
        internal static List<CatsInfo> LoadCatsInfo(DateTime dateFrom, DateTime dateTo)
        {
            List<CatsInfo> catsInfoList = new List<CatsInfo>();
            DestinationConfig destinationConfig = null;
            RfcDestination destination = null;

            try
            {
                destinationConfig = new DestinationConfig();
                RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                destination = RfcDestinationManager.GetDestination(Constants.SapSystemErp);

                RfcRepository repo = destination.Repository;
                IRfcFunction function = repo.CreateFunction(Constants.SapFunctionGetBookedHours);

                // passing parameters
                function.SetValue(Constants.SapImportParameterDateFromEN, dateFrom.ToString(DateFormat));
                function.SetValue(Constants.SapImportParameterDateToEN, dateTo.ToString(DateFormat));

                // passing table to filter costcentres 

                IRfcTable tblItems = function.GetTable("IT_SELECTION");

                foreach (var costcentre in Constants.SapImportCostCentreCollection)
                {
                    tblItems.Append();
                    tblItems.SetValue("SELNAME", "LDBSKOST");
                    tblItems.SetValue("KIND", "S");
                    tblItems.SetValue("SIGN", "I");
                    tblItems.SetValue("OPTION", "EQ");
                    tblItems.SetValue("LOW", costcentre);
                }

                function.Invoke(destination);

                // loading export parameter
                var exObject = function.GetTable(Constants.SapExportParameterBookedHours);
                var dataTable = RfcTableExtension.ToDataTable(exObject, Constants.SapExportParameterBookedHours);

                catsInfoList = dataTable.AsEnumerable().Select(m => new CatsInfo()
                {
                    PersonnelNumber = int.Parse(m.Field<string>(Constants.SapTableColumnPersonnelNumber)),
                    Name = m.Field<string>(Constants.SapTableColumnEmployeeName),
                    Date = DateTime.ParseExact(m.Field<string>(Constants.SapTableColumnWorkDate), DateFormat, null),
                    Hours = (double)m.Field<decimal>(Constants.SapTableColumnCatsHours),
                    CostReceiver = m.Field<string>(Constants.SapTableColumnCostReceiverNumber),
                    CostReceiverTypeID = GetCostReceiverTypeID(m.Field<string>(Constants.SapTableColumnCostReceiverType)),
                    CostReceiverText = m.Field<string>(Constants.SapTableColumnCostReceiverText),
                    UserName = m.Field<string>(Constants.SapTableColumnUserName),
                    OrderNumber = m.Field<string>(Constants.SapTableColumnOrderNumberAlt),
                    OperationNumber = m.Field<string>(Constants.SapTableColumnOperationNumber),
                    CostCentre = m.Field<string>(Constants.SapTableColumnCostCentre)
                }).ToList();

                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
            }
            catch (Exception)
            {
                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
                Thread.Sleep(1000);
                throw;
            }

            return catsInfoList;
        }*/

        /// <summary>
        /// Returns cost receiver type id from name
        /// </summary>
        /// <param name="costReceiverTypeName"></param>
        /// <returns></returns>
        /*
        private static int GetCostReceiverTypeID(string costReceiverTypeName)
        {
            switch (costReceiverTypeName)
            {
                case "Network activity":
                    return CostReceiverType.NetworkActivity;
                case "Order":
                    return CostReceiverType.IoNumber;
                case "WBS element":
                    return CostReceiverType.PspElement;
                case "Cost center":
                    return CostReceiverType.CostCentre;
            }
            return 0;
        }*/
    }
}