using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEPZET.sap
{
    public class DestinationConfig : IDestinationConfiguration
    {
        public bool ChangeEventsSupported()
        {
            return true;
        }

        #pragma warning disable 67
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destionationName)
        {
            //https://answers.sap.com/questions/13433249/sap-rfc-could-not-connect-in-net.html
            //https://serveanswer.com/questions/net-core-3-1-sap-connector-could-not-load-type-system-servicemodel-activation-virtualpathextension
            RfcConfigParameters parms = new RfcConfigParameters();
            //"Inner Exception 1: FileNotFoundException: Could not load file or assembly 'System.ServiceModel"
            //InnerException	{"Could not load type 'System.ServiceModel.Activation.VirtualPathExtension' from assembly 'System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.":"System.ServiceModel.Activation.VirtualPathExtension"}
            //"Could not load type 'System.ServiceModel.Activation.VirtualPathExtension' from assembly 'System.ServiceModel"

            /*
            if (destionationName.Equals(Constants.SapSystemHr))
            {
                parms.Add(RfcConfigParameters.AppServerHost, Constants.DestinationConfigHr.SapAppServerHost);
                parms.Add(RfcConfigParameters.SystemNumber, Constants.DestinationConfigHr.SapSystemNumber);
                parms.Add(RfcConfigParameters.SystemID, Constants.DestinationConfigHr.SapSystemID);
                parms.Add(RfcConfigParameters.User, Constants.DestinationConfigHr.SapUser);
                parms.Add(RfcConfigParameters.Password, Constants.DestinationConfigHr.SapPassword);
                parms.Add(RfcConfigParameters.Client, Constants.DestinationConfigHr.SapClient);
                parms.Add(RfcConfigParameters.Language, Constants.DestinationConfigHr.SapLanguage);
                parms.Add(RfcConfigParameters.PoolSize, Constants.DestinationConfigHr.SapPoolSize);                
            }*/

            
            if (destionationName.Equals(Constants.SapSystemErp))
            {
                parms.Add(RfcConfigParameters.AppServerHost, Constants.DestinationConfigErp.SapAppServerHost);
                parms.Add(RfcConfigParameters.SystemNumber, Constants.DestinationConfigErp.SapSystemNumber);
                parms.Add(RfcConfigParameters.SystemID, Constants.DestinationConfigErp.SapSystemID);
                parms.Add(RfcConfigParameters.User, Constants.DestinationConfigErp.SapUser);
                parms.Add(RfcConfigParameters.Password, Constants.DestinationConfigErp.SapPassword);
                parms.Add(RfcConfigParameters.Client, Constants.DestinationConfigErp.SapClient);
                parms.Add(RfcConfigParameters.Language, Constants.DestinationConfigErp.SapLanguage);
                parms.Add(RfcConfigParameters.PoolSize, Constants.DestinationConfigErp.SapPoolSize);
            }

            return parms;
        }
    }
}