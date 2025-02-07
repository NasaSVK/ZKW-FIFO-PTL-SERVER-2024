//using ResourcePlanning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEPZET.sap
{
    public class Constants
    {


        #region SAP

        //internal static bool InsertNewUsersFromCats = true;
        internal const string SapSystemErp = "P10";
        //internal const string SapSystemHr = "Q10";

        //test
        /*
        internal class DestinationConfigHr
        {
            //internal const string SapAppServerHost = "atibsvsapq101.intranet.zkw.at:3312";
            internal const string SapAppServerHost = "atibsvsapq101.intranet.zkw.at";
            internal const string SapSystemNumber = "12";
            internal const string SapSystemID = "P10";
            internal const string SapUser = "RFC_NASA";
            internal const string SapPassword = "3wjsWBSOuOXdcYePuJyA";
            internal const string SapClient = "100";
            internal const string SapLanguage = "EN";
            internal const string SapPoolSize = "20";
            internal const string SapPort = "3312";
        }*/

        //productive
        
        internal class DestinationConfigErp
        {
            internal const string SapAppServerHost = "at1svsapp10.intranet.zkw.at";
            internal const string SapSystemNumber = "00";
            internal const string SapSystemID = "P10";
            internal const string SapUser = "RFC_NASA";
            internal const string SapPassword = "3wjsWBSOuOXdcYePuJyA";
            internal const string SapClient = "100";
            internal const string SapLanguage = "EN";
            internal const string SapPoolSize = "20";
            internal const string SapPort = "3300";
        }


        internal const string FunctionGetFirstPallet = "Z_EWM_NASA_GET_FIRST_HU";
        internal const string FunctionGetPNFromPalletID = "Z_EWM_NASA_GET_MNR_FROM_HU";
        internal const string FunctionGetPreviousPallet = "Z_EWM_NASA_GET_PREV_HU";

        internal const string SapFunctionGetAbsences = "ZHR_PEO_ABWES_RFC";
        internal const string SapFunctionGetProjects = "ZPS_PROJ_GET_LIST";
        internal const string SapFunctionGetProjectDetails = "ZPS_PROJ_GET_INFO";
        internal const string SapFunctionGetBookedHours = "ZPS_CATS_GET_INFO";

        internal const string SapImportParameterDateFromDE = "IV_BEGDA";
        internal const string SapImportParameterDateToDE = "IV_ENDDA";
        internal const string SapImportParameterCostCentreDE = "IRA_KOSTL";

        //internal const string ImportParameterPalletID = "";
        internal const string ImportParameterCompany = "IV_COMPANY";
        internal const string ImportParameterSite = "IV_SITE";
        internal const string ImportParameterPartNumber = "IV_PARTNUMBER";        
        internal const string ImportParameterPalletID = "IV_PRESENTPALLETNUMER";
        internal const string ImportParameterPalletID2 = "IV_PRESENTPALLETNUMBER";
        internal const string ImportParameterGRDate = "IV_GRDATE";
        internal const string ImportParameterGRTIME = "IV_GRTIME";
        internal const string ImportParameterLastProcessedPalletID = "IV_LASTPROCESSEDPALLETNUMBER";

        internal const string SapExportParameterPartNumber = "EV_PARTNUMBER";
        internal const string SapExportParameterDate = "EV_GRDATE";
        internal const string SapExportParameterTime = "EV_GRTIME";
        internal const string SapExportParameterFirst = "EV_PALLETNUMBEROFFIRSTPALLET";
        internal const string SapExportParameterPrevious = "EV_PALLETNUMBEROFPREVIOUSPAL";




        internal const string SapImportParameterDateFromEN = "IV_DATE_FROM";
        internal const string SapImportParameterDateToEN = "IV_DATE_TO";
        internal const string SapImportParameterProjects = "IT_SEL";
        internal const string SapImportParameterCostCentreTableSign = "SIGN";
        internal const string SapImportParameterCostCentreTableOption = "OPTION";
        internal const string SapImportParameterCostCentreTableLow = "LOW";
        internal const string SapImportParameterCostCentreTableHigh = "HIGH";
        internal const string SapImportParameterCostCentreTableSignValue = "I";
        internal const string SapImportParameterCostCentreTableOptionValue = "EQ";
        internal static class SapImportParameterCostCentreSelection
        {
            internal const string ProjectManagement = "XXXXXX";
            internal const string SoftwareDevelopment = "XXXXXX";
            internal const string Construction = "XXXXXX";
            internal const string ControlTechnology = "XXXXXX";
            internal const string Workshop = "XXXXXX";
            internal const string TradeApprentice = "XXXXXX";
            internal const string BusinessApprentice = "XXXXXX";
        };
        public static string[] SapImportCostCentreCollection =
            {
                SapImportParameterCostCentreSelection.ProjectManagement,
                SapImportParameterCostCentreSelection.SoftwareDevelopment,
                SapImportParameterCostCentreSelection.Construction,
                SapImportParameterCostCentreSelection.ControlTechnology,
                SapImportParameterCostCentreSelection.Workshop,
                SapImportParameterCostCentreSelection.TradeApprentice,
                SapImportParameterCostCentreSelection.BusinessApprentice
            };

        internal const string SapExportParameterAbsences = "ET_PEO";
        internal const string SapExportParameterProjects = "ET_PROJ";
        internal const string SapExportParameterProjectDetails = "ET_PROJ_INFO";
        internal const string SapExportParameterBookedHours = "ET_CATS_INFO";

        internal const string SapTableColumnPersonnelNumber = "PERNR";
        internal const string SapTableColumnDate = "DATUM";
        internal const string SapTableColumnWorkDate = "WORKDATE";
        internal const string SapTableColumnHours = "STDAZ";
        internal const string SapTableColumnPspID = "PSPID";
        internal const string SapTableColumnPosID = "POSID";
        internal const string SapTableColumnOrderNumber = "AUFNR";
        internal const string SapTableColumnOrderNumberAlt = "RNPLNR";
        internal const string SapTableColumnOperationNumber = "VORNR";
        internal const string SapTableColumnDescription = "POST1";
        internal const string SapTableColumnWork = "ARBEI";
        internal const string SapTableColumnType = "ZKIND";
        internal const string SapTableColumnEmployeeName = "ENAME";
        internal const string SapTableColumnCatsHours = "CATSQUANTITY";
        internal const string SapTableColumnCostReceiverNumber = "RACCOBJ_GENREC";
        internal const string SapTableColumnCostReceiverType = "RACCOBJ";
        internal const string SapTableColumnCostReceiverText = "RACCOBJ_TEXT";
        internal const string SapTableColumnUserName = "ERNAM";
        internal const string SapTableColumnCostCentre = "ARBPL";



        public const string CompanyValue = "4";
        public const string SiteValue = "1030";



        public const int SapAbsencesSyncDaysPast = 35;
        public const int SapAbsencesSyncDaysFuture = 365;

        public const int SapProjectsSyncDaysPast = 14;
        public const int SapProjectsSyncDaysFuture = 0;

        public const int SapCatsSyncDaysPast = 100;
        public const int SapCatsSyncDaysFuture = 0;

        #endregion



    }
}