//#define NASATest
#define XIXTEST

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KEPZET.sap;
using OpcLabs.BaseLib.ComInterop;
using OpcLabs.EasyOpc.DataAccess;
using SAP.Middleware.Connector;

namespace KEPZET
{
    public partial class MainForm : Form
    {

        private int _itemHandle/* = 0*/;
        private bool IsItemsSubscribed;
        private bool _isItemsSubscribedValue;

        #region SAP_variables
        private RfcDestination destination;
        bool destinationIsInitialized = false;
        DestinationConfig destinationConfig;
        //SAPConnectorInterface SapConInter;
        #endregion


        #region common_variables
        tcp tcp1S1 = new tcp();
        tcp tcp2S1 = new tcp();
        tcp tcp1S2 = new tcp();
        tcp tcp2S2 = new tcp();

        sql db;
        SapConnector sap;
        SAPConnectorInterface SAPConInterface = new SAPConnectorInterface();


#if (!NASATest)
        private string serverIP = "10.16.0.141";
#else
        private string serverIP = "10.0.0.59";
#endif
        string
            input1posX = null, input1posY = null, input1pn = null, input1pallette = null, input1fifo = null, input1ip = "0.0.0.0", input1LastPn = null,
            input2posX = null, input2posY = null, input2pn = null, input2pallette = null, input2fifo = null, input2ip = "0.0.0.0", input2LastPn = null,
            input3posX = null, input3posY = null, input3pn = null, input3pallette = null, input3fifo = null, input3ip = "0.0.0.0", input3LastPn = null,
            input4posX = null, input4posY = null, input4pn = null, input4pallette = null, input4fifo = null, input4ip = "0.0.0.0", input4LastPn = null,
            input5posX = null, input5posY = null, input5pn = null, input5pallette = null, input5fifo = null, input5ip = "0.0.0.0", input5LastPn = null,
            input6posX = null, input6posY = null, input6pn = null, input6pallette = null, input6fifo = null, input6ip = "0.0.0.0", input6LastPn = null,

            output1posX = null, output1posY = null, output1pn = null, output1pallette = null, output1fifo = null, output1ip = null, output1LastPn = null,
            output2posX = null, output2posY = null, output2pn = null, output2pallette = null, output2fifo = null, output2ip = null, output2LastPn = null,
            output3posX = null, output3posY = null, output3pn = null, output3pallette = null, output3fifo = null, output3ip = null, output3LastPn = null,
            output4posX = null, output4posY = null, output4pn = null, output4pallette = null, output4fifo = null, output4ip = null, output4LastPn = null,
            output5posX = null, output5posY = null, output5pn = null, output5pallette = null, output5fifo = null, output5ip = null, output5LastPn = null,
            output6posX = null, output6posY = null, output6pn = null, output6pallette = null, output6fifo = null, output6ip = null, output6LastPn = null;

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (destinationIsInitialized)
            {
                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfig);
            }
        }

        int input1port = 0, input2port = 0, input3port = 0, input4port = 0, input5port = 0, input6port = 0,
            output1port = 0, output2port = 0, output3port = 0, output4port = 0, output5port = 0, output6port = 0,
            input1SetPort = 0, input2SetPort = 0, input3SetPort = 0, input4SetPort = 0, input5SetPort = 0, input6SetPort = 0,
            output1SetPort = 0, output2SetPort = 0, output3SetPort = 0, output4SetPort = 0, output5SetPort = 0, output6SetPort = 0;

        private EventLog mEventLog = new EventLog();


        List<palletSet> packInput1PalletSet = new List<palletSet>();
        List<palletSet> packInput2PalletSet = new List<palletSet>();
        List<palletSet> packInput3PalletSet = new List<palletSet>();
        List<palletSet> packInput4PalletSet = new List<palletSet>();
        List<palletSet> packInput5PalletSet = new List<palletSet>();
        List<palletSet> packInput6PalletSet = new List<palletSet>();

        List<palletSet> packOutput1PalletSet = new List<palletSet>();
        List<palletSet> packOutput2PalletSet = new List<palletSet>();
        List<palletSet> packOutput3PalletSet = new List<palletSet>();
        List<palletSet> packOutput4PalletSet = new List<palletSet>();
        List<palletSet> packOutput5PalletSet = new List<palletSet>();
        List<palletSet> packOutput6PalletSet = new List<palletSet>();

        List<palletSet> display1PalletSet = new List<palletSet>();
        List<palletSet> display2PalletSet = new List<palletSet>();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        const UInt32 WM_CLOSE = 0x0010;

        Timer t = new Timer();


        //variables
        UInt16 input1X, input1Y, input2X, input2Y, input3X, input3Y,input4X, input4Y, input5X, input5Y, input6X, input6Y,
            output1X, output1Y, output2X, output2Y, output3X, output3Y, output4X, output4Y, output5X, output5Y, output6X, output6Y;

        bool showInput1, showInput2, showInput3, showInput4, showInput5, showInput6, 
            showOutput1, showOutput2, showOutput3, showOutput4, showOutput5, showOutput6,
            input1Rst, input2Rst, input3Rst, input4Rst, input5Rst, input6Rst, output1Rst, 
            output2Rst, output3Rst, output4Rst, output5Rst, output6Rst;

        public static int hala { get; private set; }



        #endregion
        private void MainForm_Load(object sender, EventArgs e)
        { 
            #region codeTesting
            //object[] a = db.getRecord(1, 2, 3, 1);
            //int x = db.getFreePosCount("796.03.000.00");
            //object[] o = db.getFreePos("796.03.000.00");
            //db.setRecordByPos(Convert.ToInt32(o[0]), Convert.ToInt32(o[1]), (string)o[2], "123", "2016-04-25 13:00");
            //db.setRecordId(34,"test","123","2016-04-23 22:15");
            //db.clearRecordId(34);

            //object[] o = xp.getPnFromPalletId("04036367");
            //o = xp.getPreviousPallet((string)o[3], (string)o[4], (string)o[5], "04036367");

            //tcp1S1.sendToIntermec("10.0.0.69", 2004, "palNr", "pos", "prevPal", "");
            //tcp1S1.sendToIntermec("10.0.0.69", 2004, "palNr", "pos", "prevPal", "OK");
            //tcp1S1.sendToIntermec("10.0.0.69", 2004, "palNr", "pos", "prevPal", "NOK");
            //dataRcvd1S1();
            //test();
            //object[] o = db.getPack(1, 3, 2, 6);
            //object o2 = axOPCComms1.Read("Group1", "Data");
            #endregion

            #region subscribe_plc_variables          

            
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input1RstN, "false");
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input2RstN, "false");
            if (Constant.Hala == 3)
            {
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input3RstN, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input4RstN, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input5RstN, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input6RstN, "false");
            }

            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output1RstN, "false");
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output2RstN, "false");            
            if (Constant.Hala == 3)
            {
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output3RstN, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output4RstN, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output5RstN, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output6RstN, "false");
            }

            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput1N, "false");
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput2N, "false");
            if (Constant.Hala == 3)
            {
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput3N, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput4N, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput5N, "false");
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput6N, "false");
            }



            Constant.ih_input1Rst = easyDAClient1.SubscribeItem(
                Constant.ServerIP,
                Constant.ServerClass,
                Constant.input1RstN,
                VarTypes.Bool,
                Constant.requestedUpdateRate, "input1RstN");

            Constant.ih_input2Rst = easyDAClient1.SubscribeItem(
                  Constant.ServerIP,
                  Constant.ServerClass,
                  Constant.input2RstN,
                  VarTypes.Bool,
                  Constant.requestedUpdateRate, "input2RstN");

            if (Constant.Hala == 3) { 
                Constant.ih_input3Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.input3RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "input3RstN");

               Constant.ih_input4Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.input4RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "input4RstN");

                Constant.ih_input5Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.input5RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "input5RstN");

                Constant.ih_input6Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.input6RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "input6RstN");
            }



            Constant.ih_output1Rst = easyDAClient1.SubscribeItem(
                   Constant.ServerIP,
                   Constant.ServerClass,
                   Constant.output1RstN,
                   VarTypes.Bool,
                   Constant.requestedUpdateRate, "output1RstN");

            Constant.ih_output2Rst = easyDAClient1.SubscribeItem(
                  Constant.ServerIP,
                  Constant.ServerClass,
                  Constant.output2RstN,
                  VarTypes.Bool,
                  Constant.requestedUpdateRate, "output2RstN");


            if (Constant.Hala == 3)
            {
                Constant.ih_output3Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.output3RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "output3RstN");


                Constant.ih_output4Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.output4RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "output4RstN");

                Constant.ih_output5Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.output5RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "output5RstN");

                Constant.ih_output6Rst = easyDAClient1.SubscribeItem(
                       Constant.ServerIP,
                       Constant.ServerClass,
                       Constant.output6RstN,
                       VarTypes.Bool,
                       Constant.requestedUpdateRate, "output6RstN");
            }


        
                Constant.ih_showInput1 = easyDAClient1.SubscribeItem(
                  Constant.ServerIP,
                  Constant.ServerClass,
                  Constant.showInput1N,
                  VarTypes.Bool,
                  Constant.requestedUpdateRate, "showInput1N");

                Constant.ih_showInput2 = easyDAClient1.SubscribeItem(
                    Constant.ServerIP,
                    Constant.ServerClass,
                    Constant.showInput2N,
                    VarTypes.Bool,
                    Constant.requestedUpdateRate, "showInput2N");

            if (Constant.Hala == 3)
            {
                Constant.ih_showInput3 = easyDAClient1.SubscribeItem(
                    Constant.ServerIP,
                    Constant.ServerClass,
                    Constant.showInput3N,
                    VarTypes.Bool,
                    Constant.requestedUpdateRate, "showInput3N");

                Constant.ih_showInput4 = easyDAClient1.SubscribeItem(
                    Constant.ServerIP,
                    Constant.ServerClass,
                    Constant.showInput4N,
                    VarTypes.Bool,
                    Constant.requestedUpdateRate, "showInput4N");

                Constant.ih_showInput5 = easyDAClient1.SubscribeItem(
                    Constant.ServerIP,
                    Constant.ServerClass,
                    Constant.showInput5N,
                    VarTypes.Bool,
                    Constant.requestedUpdateRate, "showInput5N");

                Constant.ih_showInput6 = easyDAClient1.SubscribeItem(
                    Constant.ServerIP,
                    Constant.ServerClass,
                    Constant.showInput6N,
                    VarTypes.Bool,
                    Constant.requestedUpdateRate, "showInput6N");
            }

            IsItemsSubscribed = true;

            #endregion

            //tbxReadItem.Text = easyDAClient1.ReadItemValue(Constant.ServerIP, "OPCLabs.KitServer.2", "Demo.Single").ToString();
            tbxReadItem.Text = easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input1RstN).ToString();


            //##################################################
            tcp1S1.UpdateProgress = this.dataRcvd1S1;
            tcp1S1.tcpSet(serverIP, Constant.Port11, AddLog);
            tcp1S1.startListener();
            //##################################################

            tcp1S2.UpdateProgress = this.dataRcvd1S2;
            tcp1S2.tcpSet(serverIP, Constant.Port12, AddLog);
            tcp1S2.startListener();

            //##################################################
            tcp2S1.UpdateProgress = this.dataRcvd2S1;
            tcp2S1.tcpSet(serverIP, Constant.Port21, AddLog);
            tcp2S1.startListener();
            //##################################################

            tcp2S2.UpdateProgress = this.dataRcvd2S2;
            tcp2S2.tcpSet(serverIP, Constant.Port22, AddLog);
            tcp2S2.startListener();



#if (NASATest)
            string tempip = "10.0.0.48";
            tbIP1S1.Text = tempip;
            tbIP1S2.Text = tempip;
            tbIP2S1.Text = tempip;
            tbIP2S2.Text = tempip;
#endif

            #region SAP_constructor
            //listView1.View = View.Details;
            //###############################

            /*
            if (!new SAPConnectorInterface().TestConnection(Constants.SapSystemHr))
            {
                MessageBox.Show("Nepodarilo sa vytvorit spojenie so SAP serverom!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }*/


            destinationConfig = new DestinationConfig();
            destinationConfig.GetParameters(Constants.SapSystemErp);

            if (RfcDestinationManager.TryGetDestination(Constants.SapSystemErp) == null)
            {
                //nevyhodi vynimku, ani ked nie je konektivita na SAP 
                RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);

            }

            //az tu sa ukaze, ci sa naozaj podarilo vytvorit spojenie so SAPom
            try
            {
                //new SAPConnectorInterface().TestConnection(Constants.SapSystemHr);
                SAPConInterface.TestConnection(Constants.SapSystemErp);

                destinationIsInitialized = true;
                destination = RfcDestinationManager.GetDestination(Constants.SapSystemErp);
                SapConnector.destination = destination;
                SapConnector.destinationConfig = destinationConfig;
            }
            catch
            {
                MessageBox.Show("Nepodarilo sa vytvorit spojenie so SAP serverom!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Application.ExitThread();
                Application.Exit();
                this.Close();
                Environment.Exit(0);
            }
            #endregion


        }


        public MainForm()
        {
            InitializeComponent();

            //Constant.Hala = 3;
            nacitajConfig();

            if (Constant.Hala == 3)
            {
                //EventLog.DeleteEventSource("KEPZET");
                //EventLog.Delete("KEPZETLog","localhost");
                if (!EventLog.SourceExists("ZKWptl_19"))
                {


                    EventLog.CreateEventSource("ZKWptl_19", "ZKWptl19_Log");
                    //mEventLog.Source = "ZKWptl_19";
                    mEventLog.Log = "ZKWptl19_Log";


                    //mEventLog.Source = "KEPZET";
                    //mEventLog.Log = "zkwPBLLog";
                }
                mEventLog.Source = "ZKWptl_19";
                mEventLog.Log = "ZKWptl19_Log";
            }
            else {
                
                if (!EventLog.SourceExists("ZKWptl_16"))
                {
                    EventLog.CreateEventSource("ZKWptl_16", "ZKWptl16_Log");                
                    mEventLog.Log = "ZKWptl16_Log";                    
                }
                mEventLog.Source = "ZKWptl_16";
                mEventLog.Log = "ZKWptl16_Log";

                groupBox2.Text = "SKKRHSA00229"; groupBox4.Text = "SKKRHSA00230"; groupBox5.Text = "SKKRHSA00231"; groupBox6.Text = "SKKRHSA00232";
            }



            db = new sql(this.AddLog, Constant.Hala);
            
            //db.testQuery("4");
#if (!NASATest)
            sap = new SapConnector();
            SapConnector.delLog = this.AddLog;

#endif


            MainForm.CheckForIllegalCrossThreadCalls = false;
            t.Enabled = true;
            t.Interval = 10000;
            t.Tick += t_Tick;

            this.AddLog("Program started \n\n", LogType.info);
        }

        void t_Tick(object sender, EventArgs e)
        {
            //once per set time check for opc communication errors
            closeErrors("OPC Communication Client Error");
        }

        /// <summary>
        ///metoda spracovava dorucene a odchytene data z citaciek
        /// </summary>
        //zaskladnovanie 1
        protected void dataRcvd1S1()//SKIN00012
        {
            bool tempTLeader = cbtl1S1.Checked,
                tempTeach = cbTeach1S1.Checked,
                tempRemove = cbRemove1S1.Checked;
            //#####################################
            //tbIP1S1.Text == 10.16.109.81            
            //####################################
            //datarcvdHandle(tcp1S1, tbIP1S1.Text, 2004, tcp1S1.getData(), ref tempTLeader, ref tempTeach, ref tempRemove, ref input1LastPn);
            datarcvdHandle(tcp1S1, tbIP1S1.Text, int.Parse(tbxPort11.Text), tcp1S1.getData(), ref tempTLeader, ref tempTeach, ref tempRemove, ref input1LastPn);
            cbtl1S1.Checked = tempTLeader;
            cbTeach1S1.Checked = tempTeach;
            cbRemove1S1.Checked = tempRemove;
        }

        //zaskladnovanie 2
        protected void dataRcvd1S2()
        {
            bool tempTLeader = cbtl1S2.Checked,
                tempTeach = cbTeach1S2.Checked,
                tempRemove = cbRemove1S2.Checked;
            //#####################################
            //tbIP1S1.Text == 10.16.109.81            
            //####################################            
            datarcvdHandle(tcp1S2, tbIP1S2.Text, int.Parse(tbxPort12.Text), tcp1S2.getData(), ref tempTLeader, ref tempTeach, ref tempRemove, ref input2LastPn);
            cbtl1S2.Checked = tempTLeader;
            cbTeach1S2.Checked = tempTeach;
            cbRemove1S2.Checked = tempRemove;
        }


        //vyskladovanie 1
        protected void dataRcvd2S1()//SKIN00012 
        {
            bool tempTLeader = cbtl2S1.Checked,
                tempTeach = cbTeach2S1.Checked,
                tempRemove = cbRemove2S1.Checked;
            //#####################################
            //tbIP2S1.Text == 10.16.109.178            
            //####################################
            datarcvdHandle(tcp2S1, tbIP2S1.Text, int.Parse(tbxPort21.Text), tcp2S1.getData(), ref tempTLeader, ref tempTeach, ref tempRemove, ref output1LastPn);
            //datarcvdHandle(tcp2S1, tbIP2S1.Text, 2006, tcp2S1.getData(), ref tempTLeader, ref tempTeach, ref tempRemove, ref input1LastPn);

            cbtl2S1.Checked = tempTLeader;
            cbTeach2S1.Checked = tempTeach;
            cbRemove2S1.Checked = tempRemove;
        }

        //vyskladovanie 2
        protected void dataRcvd2S2()//SKIN00012 
        {
            bool tempTLeader = cbtl2S2.Checked,
                tempTeach = cbTeach2S2.Checked,
                tempRemove = cbRemove2S2.Checked;
            //#####################################
            //tbIP2S1.Text == 10.16.109.178            
            //####################################
            datarcvdHandle(tcp2S2, tbIP2S2.Text, int.Parse(tbxPort22.Text), tcp2S2.getData(), ref tempTLeader, ref tempTeach, ref tempRemove, ref output2LastPn);

            cbtl2S2.Checked = tempTLeader;
            cbTeach2S2.Checked = tempTeach;
            cbRemove2S2.Checked = tempRemove;
        }

        //##################################################################################################################################################################################
        //##################################################################################################################################################################################
        /// <summary>
        /// funkcia spracuva data dorucene z citaciek
        /// </summary>
        /// <param name="tcpSender">SERVER</param>
        /// <param name="ip">IP adresa ZDROJA dat (CITACKY)</param>
        /// <param name="port">PORT ZDROJA dat (CITACKY)</param>
        /// <param name="rcvdBc">DODANY KOD zo ZDROJA DAT (CITACKY)</param>
        /// <param name="tLeader"></param>
        /// <param name="teach"></param>
        /// <param name="remove"></param>
        /// <param name="lastPN"></param>
        void datarcvdHandle(tcp tcpSender, string ip, int port, string rcvdBc, ref bool tLeader, ref bool teach, ref bool remove, ref string lastPN)
        {
            {

                {

                    #region init
                    int section = 1, imP = port; //PORT CITACKY Z KTOREJ DATA PRISLI
                    int x = 0, y = 0;
                    string imip = ip;   //IP CITACKY Z KTOREJ DATA PRISLI 

                    /*object[] o = null;*/
                    object[] b = null;
                    PN_DATE_TIME pn_date_time = null;

                    bool packDone = false, curRecordInDb = false, prevRecordInDb = false,
                        pack = false, curPackInSet = false, prevPackInSet = false;
                    palletSet currentPal = new palletSet(null, null), previousPal = new palletSet(null, null), firstPal = new palletSet(null, null);
                    List<palletSet> tempPackPalletSet = new List<palletSet>();

                    bool palletBc = rcvdBc.StartsWith("1") || rcvdBc.StartsWith("2") || rcvdBc.StartsWith("3") || rcvdBc.StartsWith("4") || rcvdBc.StartsWith("5") || rcvdBc.StartsWith("6") || rcvdBc.StartsWith("7") || rcvdBc.StartsWith("8") || rcvdBc.StartsWith("9");
                    bool picklistBc = rcvdBc.StartsWith("S");
                    bool tLeaderBc = rcvdBc.StartsWith("TLeader");
                    bool ignoreListReq = rcvdBc.StartsWith("I");
                    bool resetReq = rcvdBc.StartsWith("TReset");
                    bool systemBc = rcvdBc.StartsWith("T");


                    //this.AddLog("Data na porte:" + tcpSender.port + ": " + rcvdBc + "\n", LogType.info, tcpSender.port);

                    #endregion

                    #region serviceMenu

                    if (palletBc && teach && tLeader)
                    {
                        //rcvdBc = rcvdBc.Remove(0, 1);// remove "B"
                        curRecordInDb = db.checkRecord(rcvdBc);
                        if (!curRecordInDb)
                            db.setRecordByPos(Convert.ToInt32(tbTeachX1S1.Text), Convert.ToInt32(tbTeachY1S1.Text), tbTeachPn1S1.Text, rcvdBc, db.getSQLDateTimeFromFIFO("2000010100000000"));
                    }

                    //===================================================================================================================================================================
                    //odstranenie palety v CLEAR MODE => CM musi byt aktivovany
                    //===================================================================================================================================================================
                    else if (palletBc && remove && tLeader)
                    {
                        //rcvdBc = rcvdBc.Remove(0, 1);// remove "B"
                        curRecordInDb = db.checkRecord(rcvdBc);
                        if (curRecordInDb)
                        {
                            int a = db.clearRecord(rcvdBc);
                            if (a != 0)
                                a = db.clearRecord(rcvdBc);
                            remove = false;
                            tLeader = false;
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "OK", "", rcvdBc + " removed, clear mode reset, teamLeader logged out");
                        }
                        else
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", rcvdBc + " selected pallet not in db");

                    }
                    //====================================================================================================================================================================
                    //aktivovanie/deaktivovanie LEADER MODU
                    //====================================================================================================================================================================
                    else if (tLeaderBc)// TLeader1
                    {
                        rcvdBc = rcvdBc.Remove(0, 7);
                        if (rcvdBc.StartsWith("1") && !tLeader)
                        {
                            tLeader = true;
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "teamLeader logged in");
                        }
                        else if (rcvdBc.StartsWith("1") && tLeader)
                        {
                            tLeader = false;
                            remove = false;
                            teach = false;
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "teamLeader logged out");
                        }
                        else
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "teamLeader unknown");
                        }
                    }
                    //====================================================================================================================================================================
                    //RESETOVANIE SYSTEMU VSTUP/VYSTUP
                    //====================================================================================================================================================================
                    else if (resetReq && tLeader)// TResetI/TResetO
                    {
                        rcvdBc = rcvdBc.Remove(0, 6);
                        if (rcvdBc.StartsWith("I"))
                        {
                            //resetovanie premennych suvisiacich z PLC
                            btnClearInput_Click(null, null);
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "OK", "input sets reset");
                            //tLeader = false;
                        }
                        else if (rcvdBc.StartsWith("O"))
                        {
                            //resetovanie premennych suvisiacich z PLC
                            btnClearOutput_Click(null, null);                          
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "OK", "output sets reset");
                            //tLeader = false;
                        }

                        else
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "unknown mode");
                            //tLeader = false;
                        }
                        tLeader = false;
                        return;
                    }

                    //====================================================================================================================================================================
                    //nacitany jeden zo 6-tich SYSTEMOVYCH KODOV pri aktivovanom LEADER MODE
                    //====================================================================================================================================================================
                    // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD
                    // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD // LEN AK JE AKTIVOVANY LEADER MOD
                    else if (systemBc && tLeader)// LEN AK JE AKTIVOVANY LEADER MOD
                    {
                        rcvdBc = rcvdBc.Remove(0, 1);
                        if (rcvdBc.Length > 5)
                            rcvdBc.Remove(5, 1);
                        //AKTIVOVANIE CLEAR MODU
                        if (rcvdBc == "Clear" && !remove)
                        {
                            remove = true;
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "clear mode set");
                        }
                        //DEAKTIVOVANIE CLER MODU
                        else if (rcvdBc == "Clear" && remove)
                        {
                            remove = false;
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "clear mode reset");
                        }
                        //AKTIVOVANIE TEACH MODU
                        else if (rcvdBc == "Teach" && !teach)
                        {
                            teach = true;
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "teach mode set");
                        }
                        //DEAKTIVOVANIE TEACH MODU
                        else if (rcvdBc == "Teach" && teach)
                        {
                            teach = false;
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "teach mode reset");
                        }
                        //PRIJATIE ACCEPTU v LEADER MODE == resetovanie PLC premennych vysvietenej sekcie 
                        else if (rcvdBc == "Accept")
                        {
                            if (input1port == imP)
                            {

                                resetHandle(
                                "input1", ref input1pallette, ref input1posX, ref input1posY, ref input1pn,
                                ref input1fifo, ref input1ip, ref input1port, tcp1S1, true, ref input1SetPort
                                );
                            }
                            else if (input2port == imP)
                            {

                                resetHandle(
                                "input2", ref input2pallette, ref input2posX, ref input2posY, ref input2pn,
                                ref input2fifo, ref input2ip, ref input2port, tcp1S2, true, ref input2SetPort
                                );
                            }
                            else if (input3port == imP)
                            {

                                resetHandle(
                                "input3", ref input3pallette, ref input3posX, ref input3posY, ref input3pn,
                                ref input3fifo, ref input3ip, ref input3port, tcp1S1, true, ref input3SetPort
                                );
                            }
                            else if (input4port == imP)
                            {

                                resetHandle(
                                "input4", ref input4pallette, ref input4posX, ref input4posY, ref input4pn,
                                ref input4fifo, ref input4ip, ref input4port, tcp1S2, true, ref input4SetPort
                                );
                            }
                            else if (input5port == imP)
                            {

                                resetHandle(
                                "input5", ref input5pallette, ref input5posX, ref input5posY, ref input5pn,
                                ref input5fifo, ref input5ip, ref input5port, tcp1S1, true, ref input5SetPort
                                );
                            }
                            else if (input6port == imP)
                            {

                                resetHandle(
                                "input6", ref input6pallette, ref input6posX, ref input6posY, ref input6pn,
                                ref input6fifo, ref input6ip, ref input6port, tcp1S2, true, ref input6SetPort
                                );
                            }


                            else if (output1port == imP)
                            {
                                resetHandle(
                                "output1", ref output1pallette, ref output1posX, ref output1posY, ref output1pn,
                                ref output1fifo, ref output1ip, ref output1port, tcp2S1, true, ref output1SetPort
                                );
                            }
                            else if (output2port == imP)
                            {
                                resetHandle(
                                "output2", ref output2pallette, ref output2posX, ref output2posY, ref output2pn,
                                ref output2fifo, ref output2ip, ref output2port, tcp2S2, true, ref output2SetPort
                                );
                            }
                            else if (output3port == imP)
                            {
                                resetHandle(
                                "output3", ref output3pallette, ref output3posX, ref output3posY, ref output3pn,
                                ref output3fifo, ref output3ip, ref output3port, tcp2S1, true, ref output3SetPort
                                );
                            }
                            else if (output4port == imP)
                            {
                                resetHandle(
                                "output4", ref output4pallette, ref output4posX, ref output4posY, ref output4pn,
                                ref output4fifo, ref output4ip, ref output4port, tcp2S2, true, ref output4SetPort
                                );
                            }
                            else if (output5port == imP)
                            {
                                resetHandle(
                                "output5", ref output5pallette, ref output5posX, ref output5posY, ref output5pn,
                                ref output5fifo, ref output5ip, ref output5port, tcp2S1, true, ref output5SetPort
                                );
                            }
                            else if (output6port == imP)
                            {
                                resetHandle(
                                "output6", ref output6pallette, ref output6posX, ref output6posY, ref output6pn,
                                ref output6fifo, ref output6ip, ref output6port, tcp2S2, true, ref output6SetPort
                                );
                            }
                            else
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " NOTHING TO DO - NO CHANNEL OPEN");
                        }
                        else
                        {
                            //NEBOL PRIJATY TClear, Tteach, TAccept, TResetI, TResetO v aktivovanom LEADER MODE  
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "unknown mode");
                        }
                    }  
                    //=========================================================================================================================================
                    //===================================================== koniec leader modu ================================================================
                    //=========================================================================================================================================
                    #endregion

                    #region palletBarcode
                    else

                    if (palletBc)
                    {

                        if (!SAPConInterface.TestConnectionQuiet())
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "SAP NEDOSTUPNY, SKUSTE NESKOR");
                            tempPackPalletSet.Clear();
                            lastPN = "";//force pack reset on xpert error
                            return;
                        }


                        //rcvdBc = rcvdBc.Remove(0, 1);// remove "B"
                        int packCount = 0;

                        // check db
                        curRecordInDb = db.checkRecord(rcvdBc);

                        if (curRecordInDb)
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " record already in db");
                            return;
                        }
                        //#######################################################
                        //#######################################################
                        //#######################################################
                        // get/check xpert
#if (!NASATest)
                        //try
                        //{
                        pn_date_time = SapConnector.getPNFromPalletId(rcvdBc); //Box instead of pallet scanned                             
                        //}
                        //ak SAP vrati prazdne hodnoty P/N, GRDate, GRTime potom:
                        /*
                        catch (NullReferenceException ex) {
                            this.AddLog("#getPNFromPalletId" + ex.Message  + "\n", LogType.error, port);
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", ex.Message);
                            tempPackPalletSet.Clear();
                            lastPN = "";//force pack reset on xpert error
                        }*/
                        if (pn_date_time == null || String.IsNullOrEmpty(pn_date_time.GrDate) || String.IsNullOrEmpty(pn_date_time.PN) || String.IsNullOrEmpty(pn_date_time.GrTime))
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "SAP record missing - FAILED record recieved");
                            tempPackPalletSet.Clear();
                            lastPN = "";//force pack reset on xpert error
                            return;
                        }
                        DateTime GrTime = DateTime.Parse(pn_date_time.GrTime);
                        pn_date_time.dateTime = DateTime.Parse(pn_date_time.GrDate).AddHours(GrTime.Hour).AddMinutes(GrTime.Minute).AddSeconds(GrTime.Second);


                        this.AddLog("#getPNFromPalletId  rcvdBc: " + rcvdBc + " ||| P/N: " + pn_date_time.PN + "   GRDate: " + pn_date_time.GrDate + "   GRTime: " + pn_date_time.GrTime + "\n", LogType.info, 0);
#else
                        string[] t = { "", "", "", "796.41.000.00", "20160422", "73335" };
                        o = (object[])t;
                        //o = null;
#endif
                        //#######################################################
                        //#######################################################
                        //#######################################################



                        //get/check free position
                        b = db.getFreePos(pn_date_time.PN.Trim());

                        if (b == null || b[0] == null)
                        {
                            //tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "part(" + pn_date_time.PN.Trim() + ") not in db or channel is full");
                            if (pn_date_time.PN.Trim() == "Box instead of pallet scanned")
                                tcpSender.sendToIntermec(imip, imP, "BOX SCANNED", "", "", "NOK", "", "NOK, BOX SCANNED");
                            else
                                if (db.existPNinDB(pn_date_time.PN.Trim()))
                                //tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "part(" + pn_date_time.PN.Trim() + ") not in db or channel is full");
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "part(" + pn_date_time.PN.Trim() + ") channel is full");
                            else
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "part(" + pn_date_time.PN.Trim() + ") not in db");

                            tempPackPalletSet.Clear();
                            return;
                        }

                        try
                        {
                            packCount = Convert.ToInt32(b[6]);

                        }
                        catch (Exception ex)
                        {
                            //mEventLog.WriteEntry("conversion error:" + ex.ToString(), EventLogEntryType.Error, imP);
                            AddLog("conversion error:" + ex.ToString() + "\n", LogType.error, imP);

                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "Conversion Error");
                        }


                        //convert to db coords
                        x = Convert.ToInt32(b[0]) - 1;
                        y = Convert.ToInt32(b[1]) - 1;
                        //x = pn_date_time.posX - 1;//&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                        //y = pn_date_time.posY - 1;//&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

                        //predtym 
                        //if ((y + 1) > 16) section = 2;
                        section = getSection(ref y);

                        currentPal.palletNr = rcvdBc;
                        currentPal.fifoTime = pn_date_time.GrDate + "00000" + pn_date_time.GrTime;

                        //#######################################################
                        //#######################################################
#if (!NASATest)
                        //previousPal.palletNr = SapConnector.getPreviousPallet((string)o[3], (string)o[4], (string)o[5], rcvdBc);
                        previousPal.palletNr = SapConnector.getPreviousPallet(pn_date_time.PN, pn_date_time.GrDate, pn_date_time.GrTime, rcvdBc);
                        this.AddLog("#getPreviousPallet   PN: " + pn_date_time.PN + "   PrPalNum: " + rcvdBc + "  ||  " + " PrevPaletNum: " + previousPal.palletNr, LogType.info, port);

                        //getLastPalletinDb - vrati naposledny zaskladnenu paletu z daneho typu (nutne sa sleduje stav lokalnej DB) 
                        //getFirst palet - vrati PRVU NEzaskladnenu (sleduje sa aj stav lokalneho SQL); vola sa len ak PREVOIUS PALET NEBOLA zaskladnena
                        //previus palet - paleta ktora mala/ma byt zaskladnena pred aktualnou paletou (NESLEDUJE SA SQL)

                        /*
                        string zSQL = db.getLastPalletinDb(pn_date_time.PN);
                        firstPal.palletNr = SapConnector.getFirstPallet(pn_date_time.PN, pn_date_time.GrDate, pn_date_time.GrTime, rcvdBc, zSQL).Trim();
                        this.AddLog("FIRST PALETE: " + firstPal.palletNr, LogType.info, -1);*/
#else

                        if (currentPal.palletNr == "02598650" || currentPal.palletNr == "02598656")
                            previousPal.palletNr = "";
                        else
                            previousPal.palletNr = "02598650";
#endif



                        previousPal.palletNr = previousPal.palletNr.Trim();

                        //#######################################################
                        //#######################################################
                        //dorobene na ADAMOVU ZIADOST
                        if (previousPal.palletNr.Length <= 8)
                            prevRecordInDb = true;
                        else
                            prevRecordInDb = db.checkRecord(previousPal.palletNr);

                        pack = packCount > 1 ? true : false;

                        if (section == 1)
                        {
                            if (input1SetPort == imP || input1SetPort == 0)
                            {
                                tempPackPalletSet = packInput1PalletSet;
                                input1SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }

                        }
                        else if (section == 2)
                        {
                            if (input2SetPort == imP || input2SetPort == 0)
                            {
                                tempPackPalletSet = packInput2PalletSet;
                                input2SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else if (section == 3)
                        {
                            if (input3SetPort == imP || input3SetPort == 0)
                            {
                                tempPackPalletSet = packInput3PalletSet;
                                input3SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else if (section == 4)
                        {
                            if (input4SetPort == imP || input4SetPort == 0)
                            {
                                tempPackPalletSet = packInput4PalletSet;
                                input4SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else if (section == 5)
                        {
                            if (input5SetPort == imP || input5SetPort == 0)
                            {
                                tempPackPalletSet = packInput5PalletSet;
                                input5SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else if (section == 6)
                        {
                            if (input6SetPort == imP || input6SetPort == 0)
                            {
                                tempPackPalletSet = packInput6PalletSet;
                                input6SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }

                        //if (lastPN != o[3].ToString().Trim())
                        if (lastPN != pn_date_time.PN.Trim())
                        {
                            tempPackPalletSet.Clear();
                            //mEventLog.WriteEntry("pn changed set cleared:", EventLogEntryType.Warning, imP);
                            AddLog("pn changed set cleared:", LogType.warning, imP);
                        }

                        if (pack && tempPackPalletSet.Count >= packCount)
                        {
                            tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "R: " + b[0].ToString() + " S: " + b[1].ToString(), "", "", "", "pallet Set complete");
                            return;
                        }
                        curPackInSet = tempPackPalletSet.Contains(currentPal);
                        prevPackInSet = tempPackPalletSet.Contains(previousPal);
                        if (!pack)
                            tempPackPalletSet.Clear();

                        //if ((prevRecordInDb || previousPal.palleteNr == "") && (sixPack || threePack))
                        //{
                        //    tempPalletSet.Clear();

                        //}

                        bool notPack = !pack,
                            expectedPallete = (prevRecordInDb || previousPal.palletNr == "") && notPack,
                            curPackOk = !curPackInSet && prevPackInSet,
                            expectedPackPalletSet = pack && (prevRecordInDb || previousPal.palletNr == "" || curPackOk);

                        if (expectedPallete || expectedPackPalletSet)
                        {

                            //pack
                            if (pack)
                            {
                                //lastPN = o[3].ToString().Trim();
                                lastPN = pn_date_time.PN.Trim();
                                if (!curPackInSet)
                                {
                                    tempPackPalletSet.Add(currentPal);
                                }
                                else
                                {
                                    tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "record already in set");
                                    return;
                                }

                                if (tempPackPalletSet.Count > packCount)
                                {
                                    packDone = false;
                                    tempPackPalletSet.Clear();
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "NOK", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "multipack error, scan again");
                                    return;
                                }

                                else if (tempPackPalletSet.Count < packCount)
                                {
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "");

                                }

                                else if (tempPackPalletSet.Count == packCount)
                                    packDone = true;

                            }

                            if (
                                ((prevRecordInDb && notPack) || packDone || (previousPal.palletNr == "" && notPack))
                                ) //no previous pallet "                         "
                            {
                                if (b[0] != null)// no free pos/pn not in db
                                {

                                    x = Convert.ToInt32(b[0]) - 1;
                                    y = Convert.ToInt32(b[1]) - 1;
                                    //x = pn_date_time.posX - 1;
                                    //y = pn_date_time.posY - 1;
                                    section = getSection(ref y);


                                    if (section == 2)
                                    {
                                        if (input2port == imP || input2port == 0)
                                        {
                                            if (Constant.Hala == 2) y = y - 16;
                                            input2posX = b[0].ToString();
                                            input2posY = b[1].ToString();
                                            input2pallette = rcvdBc;
                                            input2pn = pn_date_time.PN;
                                            lastPN = input2pn;
                                            input2fifo = pn_date_time.GrDate + "00000" + pn_date_time.GrTime;
                                            input2ip = imip;
                                            input2port = imP;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }

                                    }
                                    else if (section == 3)
                                    {
                                        if (input3port == imP || input3port == 0)
                                        {
                                            input3posX = b[0].ToString();
                                            input3posY = b[1].ToString();
                                            input3pallette = rcvdBc;
                                            input3pn = pn_date_time.PN;
                                            lastPN = input3pn;
                                            input3fifo = pn_date_time.GrDate + "00000" + pn_date_time.GrTime;
                                            input3ip = imip;
                                            input3port = imP;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }

                                    }
                                    else if (section == 4)
                                    {
                                        if (input4port == imP || input4port == 0)
                                        {
                                            input4posX = b[0].ToString();
                                            input4posY = b[1].ToString();
                                            input4pallette = rcvdBc;
                                            input4pn = pn_date_time.PN;
                                            lastPN = input4pn;
                                            input4fifo = pn_date_time.GrDate + "00000" + pn_date_time.GrTime;
                                            input4ip = imip;
                                            input4port = imP;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }

                                    }
                                    else if (section == 5)
                                    {
                                        if (input5port == imP || input5port == 0)
                                        {
                                            input5posX = b[0].ToString();
                                            input5posY = b[1].ToString();
                                            input5pallette = rcvdBc;
                                            input5pn = pn_date_time.PN;
                                            lastPN = input5pn;
                                            input5fifo = pn_date_time.GrDate + "00000" + pn_date_time.GrTime;
                                            input5ip = imip;
                                            input5port = imP;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }

                                    }
                                    else if (section == 6)
                                    {
                                        if (input6port == imP || input6port == 0)
                                        {
                                            input6posX = b[0].ToString();
                                            input6posY = b[1].ToString();
                                            input6pallette = rcvdBc;
                                            input6pn = pn_date_time.PN;
                                            lastPN = input6pn;
                                            input6fifo = pn_date_time.GrDate + "00000" + pn_date_time.GrTime;
                                            input6ip = imip;
                                            input6port = imP;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (input1port == imP || input1port == 0)
                                        {
                                            input1posX = b[0].ToString();
                                            input1posY = b[1].ToString();
                                            input1pallette = rcvdBc;
                                            input1pn = pn_date_time.PN;
                                            lastPN = input1pn;
                                            input1fifo = pn_date_time.GrDate + "00000" + pn_date_time.GrTime;
                                            input1ip = imip;
                                            input1port = imP;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }

                                    int[] xy = new int[2];
                                    xy[1] = x;//.ToString();
                                    xy[0] = y;//.ToString();

                                    if (pn_date_time == null || pn_date_time.PN == null || pn_date_time.PN == "")
                                        tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", "not found");
                                    else
                                    {
                                        tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "R: " + b[0].ToString() + " S: " + b[1].ToString(), "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "");


                                        if (section == 2)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input2N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput2N, true);
                                            mEventLog.WriteEntry("showInput2 set true", EventLogEntryType.Information, port);
                                        }
                                        else
                                        if (section == 3)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input3N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput3N, true);
                                            mEventLog.WriteEntry("showInput3 set true", EventLogEntryType.Information, port);
                                        }
                                        else
                                        if (section == 4)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input4N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput4N, true);
                                            mEventLog.WriteEntry("showInput4 set true", EventLogEntryType.Information, port);
                                        }
                                        else
                                        if (section == 5)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input5N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput5N, true);
                                            mEventLog.WriteEntry("showInput5 set true", EventLogEntryType.Information, port);
                                        }
                                        else
                                        if (section == 6)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input6N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput6N, true);
                                            mEventLog.WriteEntry("showInput6 set true", EventLogEntryType.Information, port);
                                        }
                                        else
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.input1N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput1N, true);
                                            mEventLog.WriteEntry("showInput1 set true", EventLogEntryType.Information, port);
                                        }
                                    }
                                }
                                else
                                {
                                    if (pack)
                                    {
                                        //tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "part(" + pn_date_time.PN.Trim() + ") not in db or channel is full");
                                        if (db.existPNinDB(pn_date_time.PN.Trim()))
                                            tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "STOP", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "part(" + pn_date_time.PN.Trim() + ") channel is full");
                                        else
                                            tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "NOK", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "part(" + pn_date_time.PN.Trim() + ") not in db");
                                    }

                                    else
                                    {
                                        //tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "", "", "part(" + pn_date_time.PN.Trim() + ") not in db or channel is full");

                                        if (db.existPNinDB(pn_date_time.PN.Trim()))
                                            tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "STOP", "", "part(" + pn_date_time.PN.Trim() + ") channel is full");
                                        else
                                            tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "NOK", "", "part(" + pn_date_time.PN.Trim() + ") not in db");
                                    }
                                    //###########################################################################################################################################################################################
                                    //###########################################################################################################################################################################################
                                }
                            }
                        }
                        else
                        {
                            if (curPackInSet)
                                tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "record already in set");
                            else
                            {
                                //#######################################################
                                //#######################################################
#if (!NASATest)
                                string LastPallet = db.getLastPalletinDb(pn_date_time.PN);
                                firstPal.palletNr = SapConnector.getFirstPallet(pn_date_time.PN, pn_date_time.GrDate, pn_date_time.GrTime, rcvdBc, LastPallet).Trim();
#else

                                firstPal.palletNr = "002598635";
#endif
                                //#######################################################
                                //#######################################################
                                this.AddLog("#getFirstPallet PartNum: " + pn_date_time.PN + "   GrDate: " + pn_date_time.GrDate + "   GrTime: " + pn_date_time.GrTime + "   LastPalletinDBformPN: " + LastPallet + " ||| FirstPalletNum: " + firstPal.palletNr, LogType.info);
                                tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", firstPal.palletNr, "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet), "previous pallet: " + previousPal.palletNr);
                            }

                            if (notPack && section == 1)
                            {

                                input1SetPort = 0;

                            }
                            else if (notPack && section == 2)
                            {
                                input2SetPort = 0;

                            }
                            else if (notPack && section == 3)
                            {
                                input3SetPort = 0;

                            }
                            else if (notPack && section == 4)
                            {
                                input4SetPort = 0;

                            }
                            else if (notPack && section == 5)
                            {
                                input5SetPort = 0;

                            }
                            else if (notPack && section == 6)
                            {
                                input6SetPort = 0;

                            }
                        }
                    }
                    #endregion
      
                    #region 2S1PicklistBarcode

                    else if (picklistBc)
                    {

                        rcvdBc = rcvdBc.Remove(0, 1);// remove "S"
                        if (rcvdBc.Length == 7)
                            rcvdBc = "0" + rcvdBc;
                        if (rcvdBc.Length == 9)
                            rcvdBc = rcvdBc.Remove(0, 1);// remove "0"
                        int packCount = 0, ageLockTime = 0, ageLockBPTime = 0;
                        bool ageLockBypass = false, ageLockCheck = false;
                        DateTime ageBypassStart = new DateTime();
                        DateTime palletFIFO = new DateTime();
                        string[] packOutputSet = null;

                        // get/check record in db
                        curRecordInDb = db.checkRecord(rcvdBc);

                        if (!curRecordInDb)
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", "record not in db");
                            return;
                        }


                        pn_date_time = db.getRecord(rcvdBc);

                        //if (o == null)
                        if (pn_date_time == null)
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "pallet error, scan again");
                            return;
                        }
                        try
                        {
                            //object[] k = db.getPackCount(o[1].ToString(), o[2].ToString());
                            object[] k = db.getPackCount(pn_date_time.posX.ToString(), pn_date_time.posY.ToString());
                            packCount = Convert.ToInt32(k[0]);

                        }
                        catch (Exception ex)
                        {
                            //mEventLog.WriteEntry("conversion error(pack):" + ex.ToString(), EventLogEntryType.Error, imP);
                            this.AddLog("conversion error(pack):" + ex.ToString(), LogType.error, imP);
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", "Conversion Error");
                        }
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        try
                        {
                            //object[] a = db.getPosAgeData(o[1].ToString(), o[2].ToString());
                            object[] a = db.getPosAgeData(pn_date_time.posX.ToString(), pn_date_time.posY.ToString());
                            ageLockTime = Convert.ToInt32(a[0].ToString());//vek palety
                            ageLockCheck = Convert.ToBoolean(a[1].ToString());//vypnutie/zapnutie kontorly pre pozíciu
                            ageLockBPTime = Convert.ToInt32(a[2].ToString());//cas bypass povolenia
                            //ageBypassStart = a[3] != null ? DateTime.Parse(a[3].ToString()):ageBypassStart;//cas zaciatku bypass povolenia
                            DateTime.TryParse(a[3].ToString(), out ageBypassStart);//cas zaciatku bypass povolenia
                            ageLockBypass = Convert.ToBoolean(a[4].ToString());//vypnutie/zapnutie bypass povolenia
                            //palletFIFO = DateTime.Parse(o[3].ToString());
                            palletFIFO = pn_date_time.GetGrDateTime;
                        }
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        catch (Exception ex)
                        {
                            //mEventLog.WriteEntry("conversion error(age):" + ex.ToString(), EventLogEntryType.Error, imP);
                            AddLog("conversion error(age):" + ex.ToString(), LogType.error, imP);
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", " Conversion Error(age)");
                        }

                        if (ageLockCheck)
                        {
                            bool ageLockBypassOk = ageLockBypass && DateTime.Now < ageBypassStart.AddMinutes(ageLockBPTime);
                            bool palletAgeOk = DateTime.Now > palletFIFO.AddHours(ageLockTime);
                            if (!palletAgeOk && !ageLockBypassOk)
                            {

                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " pallet too young, call teamleader");
                                //mEventLog.WriteEntry("pallet age stop, ageLockCheck:" + ageLockBypass.ToString() + " ageLockStart:" + ageBypassStart.ToString() + " ageLockTime:" + ageLockTime.ToString() + "", EventLogEntryType.Warning, imP);
                                AddLog("pallet age stop, ageLockCheck: " + ageLockBypass.ToString() + "  ageLockStart: " + ageBypassStart.ToString() + "  ageLockTime: " + ageLockTime.ToString() + " ", LogType.warning, imP);
                                return;

                            }
                        }

                        // convert to db coords
                        //x = Convert.ToInt32(o[1]) - 1;
                        //y = 41 - Convert.ToInt32(o[2]);
                        x = pn_date_time.posX - 1;
                        y = pn_date_time.posY - 1;

                        //predtym 
                        //if ((y + 1) > 16) section = 2;
                        section = getSection(ref y);

                        if (section == 2)
                        {
                            if (output2SetPort == imP || output2SetPort == 0)
                            {
                                tempPackPalletSet = packOutput2PalletSet;
                                output2SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else
                        if (section == 3)
                        {
                            if (output3SetPort == imP || output3SetPort == 0)
                            {
                                tempPackPalletSet = packOutput3PalletSet;
                                output3SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else if (section == 4)
                        {
                            if (output4SetPort == imP || output4SetPort == 0)
                            {
                                tempPackPalletSet = packOutput4PalletSet;
                                output4SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else if (section == 5)
                        {
                            if (output5SetPort == imP || output5SetPort == 0)
                            {
                                tempPackPalletSet = packOutput5PalletSet;
                                output5SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }

                        }
                        else if (section == 6)
                        {
                            if (output6SetPort == imP || output6SetPort == 0)
                            {
                                tempPackPalletSet = packOutput6PalletSet;
                                output6SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }
                        }
                        else
                        {
                            if (output1SetPort == imP || output1SetPort == 0)
                            {
                                tempPackPalletSet = packOutput1PalletSet;
                                output1SetPort = imP;
                            }
                            else
                            {
                                tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                tempPackPalletSet.Clear();
                                return;
                            }

                        }


                        //if (lastPN != o[0].ToString().Trim())
                        if (lastPN != pn_date_time.PN)
                        {
                            tempPackPalletSet.Clear();
                            //mEventLog.WriteEntry("pn changed set cleared:", EventLogEntryType.Warning, imP);
                            this.AddLog("pn changed set cleared:", LogType.warning, imP);

                        }
                        currentPal.palletNr = rcvdBc;
                        pack = packCount > 1 ? true : false;
                        curPackInSet = tempPackPalletSet.Contains(currentPal);

                        if (tempPackPalletSet.Count < packCount && !curPackInSet)
                        {
                            if (tempPackPalletSet.Count >= packCount)
                            {
                                string tempPackPalletSetData = "";
                                foreach (palletSet p in tempPackPalletSet)
                                {
                                    tempPackPalletSetData += p.palletNr + ",";
                                }
                                //mEventLog.WriteEntry("packCount error:\n" + "palletset.count=" + tempPackPalletSet.Count.ToString() + ",packCount=" + packCount.ToString() + ",currentPal.palletNr=" + currentPal.palletNr + "\ntemppackPalletSet:" + tempPackPalletSetData, EventLogEntryType.Error, 77);
                                this.AddLog("packCount error:\n" + "palletset.count=" + tempPackPalletSet.Count.ToString() + ", packCount=" + packCount.ToString() + ", currentPal.palletNr=" + currentPal.palletNr + "\ntemppackPalletSet: " + tempPackPalletSetData, LogType.error, 77);
                            }
                            // pack
                            if (pack)
                            {
                                //lastPN = o[0].ToString().Trim();
                                //string pn = o[0].ToString().Trim();//db.getpn(currentPal.palleteNr);
                                lastPN = pn_date_time.PN;
                                string pn = pn_date_time.PN;//db.getpn(currentPal.palleteNr);
                                packOutputSet = null;
                                if (tempPackPalletSet.Count == 0)
                                    packOutputSet = db.getPack(currentPal.palletNr, pn.Trim(), packCount);
                                else
                                    packOutputSet = db.getPack(tempPackPalletSet[0].palletNr, pn.Trim(), packCount);
                                lbxLogs.DataSource = packOutputSet;
                                bool palletInOutputSet = false;
                                try
                                {
                                    palletInOutputSet = packOutputSet.Contains(currentPal.palletNr);
                                }
                                catch (Exception ex)
                                {
                                    //tcpSender.sendToIntermec(imip, imP, o[0].ToString(), "", "", "NOK", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), "pallet not from correct set1");
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "NOK", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), " pallet not from correct set1");
                                    //mEventLog.WriteEntry("pallet not in output set Error:" + ex.ToString(), EventLogEntryType.Error, imP);
                                    AddLog("pallet not in output set Error: " + ex.ToString(), LogType.error, imP);
                                }

                                if (palletInOutputSet)
                                {
                                    palletSet temp = new palletSet(currentPal.palletNr, "");
                                    if (!tempPackPalletSet.Contains(temp))
                                    {
                                        tempPackPalletSet.Add(temp);
                                    }

                                    else
                                    {
                                        //tcpSender.sendToIntermec(imip, imP, o[0].ToString(), "", "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), "pallet already in set");
                                        tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "STOP", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), " pallet already in set");
                                        return;
                                    }

                                }
                                else
                                {
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "NOK", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), " pallet not from correct set");
                                    return;
                                }

                                if (tempPackPalletSet.Count > packCount)
                                {
                                    packDone = false;
                                    tempPackPalletSet.Clear();
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "NOK", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), " multipack error, scan again");

                                }

                                if (tempPackPalletSet.Count < packCount)
                                {
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "STOP", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), " BEZ HLASKY");
                                }

                                if (tempPackPalletSet.Count == packCount)
                                    packDone = true;

                            }


                            if (packDone || !pack) //no previous pallet "                         "
                            {
                                //b = db.getRecord(currentPal.palleteNr);
                                //if (o[1] != null)// pn not in db
                                if (pn_date_time.posX != 0)
                                {
                                    x = Convert.ToInt32(pn_date_time.posX) - 1;
                                    y = Convert.ToInt32(pn_date_time.posY) - 1;

                                    section = getSection(ref y);

                                    //if ((y + 1) > 16)
                                    if (section == 2)
                                    {
                                        if (output2port == imP || output2port == 0)
                                        {

                                            if (Constant.Hala == 2) y = y - 16;
                                            output2posX = pn_date_time.posX.ToString();
                                            output2posY = pn_date_time.posY.ToString();
                                            output2pallette = rcvdBc;
                                            output2ip = imip;
                                            output2port = imP;
                                            output2pn = pn_date_time.PN.Trim();
                                            lastPN = output2pn;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }
                                    else
                                    if (section == 3)
                                    {
                                        if (output3port == imP || output3port == 0)
                                        {
                                            output3posX = pn_date_time.posX.ToString();
                                            output3posY = pn_date_time.posY.ToString();
                                            output3pallette = rcvdBc;
                                            output3ip = imip;
                                            output3port = imP;
                                            output3pn = pn_date_time.PN.Trim();
                                            lastPN = output3pn;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }
                                    else
                                    if (section == 4)
                                    {
                                        if (output4port == imP || output4port == 0)
                                        {
                                            output4posX = pn_date_time.posX.ToString();
                                            output4posY = pn_date_time.posY.ToString();
                                            output4pallette = rcvdBc;
                                            output4ip = imip;
                                            output4port = imP;
                                            output4pn = pn_date_time.PN.Trim();
                                            lastPN = output4pn;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }
                                    else
                                    if (section == 5)
                                    {
                                        if (output5port == imP || output5port == 0)
                                        {
                                            output5posX = pn_date_time.posX.ToString();
                                            output5posY = pn_date_time.posY.ToString();
                                            output5pallette = rcvdBc;
                                            output5ip = imip;
                                            output5port = imP;
                                            output5pn = pn_date_time.PN.Trim();
                                            lastPN = output5pn;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }
                                    else
                                    if (section == 6)
                                    {
                                        if (output6port == imP || output6port == 0)
                                        {
                                            output6posX = pn_date_time.posX.ToString();
                                            output6posY = pn_date_time.posY.ToString();
                                            output6pallette = rcvdBc;
                                            output6ip = imip;
                                            output6port = imP;
                                            output6pn = pn_date_time.PN.Trim();
                                            lastPN = output6pn;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (output1port == imP || output1port == 0)
                                        {
                                            output1posX = pn_date_time.posX.ToString();
                                            output1posY = pn_date_time.posY.ToString();
                                            output1pallette = rcvdBc;
                                            output1ip = imip;
                                            output1port = imP;
                                            output1pn = pn_date_time.PN.ToString().Trim();
                                            lastPN = output1pn;
                                        }
                                        else
                                        {
                                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " warehouse section occupied");
                                            tempPackPalletSet.Clear();
                                            return;
                                        }
                                    }


                                    //string[] xy = new string[2];
                                    int[] xy = new int[2];
                                    xy[1] = x;
                                    xy[0] = y;

                                    //if (o == null)
                                    //ak sa nenajdu data o zaskladneni palety
                                    if (pn_date_time == null)
                                        tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", " not found");
                                    else
                                    {
                                        tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "R: " + pn_date_time.posX + "  S: " + pn_date_time.posY, "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), "");

                                        if (section == 2)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output2N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput2, true);
                                            mEventLog.WriteEntry("showOutput2 set true", EventLogEntryType.Information, port);
                                        }
                                        else if (section == 3)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output3N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput3, true);
                                            mEventLog.WriteEntry("showOutput3 set true", EventLogEntryType.Information, port);
                                        }
                                        else if (section == 4)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output4N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput4, true);
                                            mEventLog.WriteEntry("showOutput4 set true", EventLogEntryType.Information, port);
                                        }
                                        else if (section == 5)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output5N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput5, true);
                                            mEventLog.WriteEntry("showOutput5 set true", EventLogEntryType.Information, port);
                                        }
                                        else if (section == 6)
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output6N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput6, true);
                                            mEventLog.WriteEntry("showOutput6 set true", EventLogEntryType.Information, port);
                                        }
                                        else
                                        {
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.output1N, xy);
                                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput1, true);
                                            mEventLog.WriteEntry("showOutput1 set true", EventLogEntryType.Information, port);
                                        }

                                    }
                                }
                                else
                                    //tcpSender.sendToIntermec(imip, imP, pn_date_time.PN.ToString(), "", "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), "part(" + pn_date_time.dateTime.ToString().Trim() + ") not in db or channel is full");
                                    if (db.existPNinDB(pn_date_time.PN.Trim()))
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN.ToString(), "", "", "STOP", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), "part(" + pn_date_time.dateTime.ToString().Trim() + ") channel is full");
                                else
                                    tcpSender.sendToIntermec(imip, imP, pn_date_time.PN.ToString(), "", "", "NOK", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), "part(" + pn_date_time.dateTime.ToString().Trim() + ") not in db");
                            }

                        }
                        else
                        {
                            //packOutputSet = db.getPack(currentPal.palletNr, o[0].ToString().Trim(), packCount);
                            //tcpSender.sendToIntermec(imip, imP, o[0].ToString(), "", "", "", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), "record already in set");
                            packOutputSet = db.getPack(currentPal.palletNr, pn_date_time.PN, packCount);
                            tcpSender.sendToIntermec(imip, imP, pn_date_time.PN, "", "", "STOP", Helpers.getPackDisplayText(tempPackPalletSet.Count, packCount, tempPackPalletSet, packOutputSet), " record already in set");
                        }
                        if (!pack && section == 1)
                        {

                            output1SetPort = 0;

                        }
                        else if (!pack && section == 2)
                        {
                            output2SetPort = 0;

                        }
                        else if (!pack && section == 3)
                        {
                            output3SetPort = 0;

                        }
                        else if (!pack && section == 4)
                        {
                            output4SetPort = 0;

                        }
                        else if (!pack && section == 5)
                        {
                            output5SetPort = 0;

                        }
                        else if (!pack && section == 6)
                        {
                            output6SetPort = 0;

                        }

                    }

                    #endregion

                    #region IGNORE_LIST
                    else if (ignoreListReq)// internal barcode
                    {
                        rcvdBc = rcvdBc.Remove(0, 1);// remove "I"

                        if (rcvdBc.Length < 7 || rcvdBc.Length > 8)//solve in bcr
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", " wrong format");

                            return;
                        }

                        if (rcvdBc.Length == 7)
                            rcvdBc = "0" + rcvdBc;

                        if (!curRecordInDb)
                        {
                            db.setRecordByPos(0, 0, "abc", rcvdBc, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "", "", rcvdBc + " added");
                            return;
                        }
                        else
                        {
                            tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", rcvdBc + " already in db");
                            return;
                        }
                    }
                    #endregion

                    #region INTERNAL
                    else if (rcvdBc.StartsWith("1J"))// internal barcode
                    {
                        tcpSender.sendToIntermec(imip, imP, "", "", "", "STOP", "", " wrong barcode scanned");
                    }
                    else
                    if (rcvdBc.Trim() == "") {
                        tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", " EMPTY STRING RECIEVED");
                    }
                    else
                    {
                        tcpSender.sendToIntermec(imip, imP, "", "", "", "NOK", "", " unknown barcode");
                        //unknown barcode
                    }
                    #endregion

                    #region tempPack
                    if (palletBc && section == 1)
                    {
                        packInput1PalletSet = tempPackPalletSet;

                    }
                    else if (palletBc && section == 2)
                    {
                        packInput2PalletSet = tempPackPalletSet;

                    }
                    else if (palletBc && section == 3)
                    {
                        packInput3PalletSet = tempPackPalletSet;

                    }
                    else if (palletBc && section == 4)
                    {
                        packInput4PalletSet = tempPackPalletSet;

                    }
                    else if (palletBc && section == 5)
                    {
                        packInput5PalletSet = tempPackPalletSet;

                    }
                    else if (palletBc && section == 6)
                    {
                        packInput6PalletSet = tempPackPalletSet;

                    }
                    else if (picklistBc && section == 1)
                    {
                        packOutput1PalletSet = tempPackPalletSet;

                    }
                    else if (picklistBc && section == 2)
                    {
                        packOutput2PalletSet = tempPackPalletSet;

                    }
                    else if (picklistBc && section == 3)
                    {
                        packOutput3PalletSet = tempPackPalletSet;

                    }
                    else if (picklistBc && section == 4)
                    {
                        packOutput4PalletSet = tempPackPalletSet;

                    }
                    else if (picklistBc && section == 5)
                    {
                        packOutput5PalletSet = tempPackPalletSet;

                    }
                    else if (picklistBc && section == 6)
                    {
                        packOutput6PalletSet = tempPackPalletSet;

                    }
                    #endregion

                }

            }
        }
        //##################################################################################################################################################################################
        //##################################################################################################################################################################################

        void resetHandle(
            string side, ref string pallette, ref string posX, ref string posY, ref string pn,
            ref string fifo, ref string ip, ref int port, tcp tcpSender, bool plcOverride, ref int setPort
            )

        {
            mEventLog.WriteEntry("fifo: "+fifo + " (from: resetHandle)", EventLogEntryType.Information, setPort);
            int packCount = 0;
            try
            {
                object[] k = db.getPackCount(posX, posY);
                packCount = Convert.ToInt32(k[0]);
            }
            catch (Exception ex)
            {
                packCount = 0;
            }
            bool pack = packCount > 1 ? true : false;

            List<palletSet> tempPackPalletSet = new List<palletSet>();

            #region packInputOutput

            if (side.StartsWith("input1"))
            {
                tempPackPalletSet = packInput1PalletSet;

            }
            else if (side.StartsWith("input2"))
            {
                tempPackPalletSet = packInput2PalletSet;

            }
            else if (side.StartsWith("input3"))
            {
                tempPackPalletSet = packInput3PalletSet;

            }
            else if (side.StartsWith("input4"))
            {
                tempPackPalletSet = packInput4PalletSet;

            }
            else if (side.StartsWith("input5"))
            {
                tempPackPalletSet = packInput5PalletSet;

            }
            else if (side.StartsWith("input6"))
            {
                tempPackPalletSet = packInput6PalletSet;

            }
            else if (side.StartsWith("output1"))
            {
                tempPackPalletSet = packOutput1PalletSet;

            }
            else if (side.StartsWith("output2"))
            {
                tempPackPalletSet = packOutput2PalletSet;

            }
            else if (side.StartsWith("output3"))
            {
                tempPackPalletSet = packOutput3PalletSet;

            }
            else if (side.StartsWith("output4"))
            {
                tempPackPalletSet = packOutput4PalletSet;

            }
            else if (side.StartsWith("output5"))
            {
                tempPackPalletSet = packOutput5PalletSet;

            }
            else if (side.StartsWith("output6"))
            {
                tempPackPalletSet = packOutput6PalletSet;

            }
            #endregion

            #region input
            if (side.StartsWith("input"))
            {
                try
                {
                    string log = " ";
                    //bool plc = (bool)axOPCComms1.Read("Group2", side + "RstN");
                    bool plc = (bool)easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, Constant.inputXRstN(side));

                    if ((plc || plcOverride) && !db.checkRecord(pallette))
                    {
                        if (tempPackPalletSet.Count == packCount && pack)
                        {
                            foreach (palletSet value in tempPackPalletSet)
                            {
                                db.setRecordByPos(Convert.ToInt32(posX), Convert.ToInt32(posY), pn, value.palletNr, db.getSQLDateTimeFromFIFO(value.fifoTime));
                                log += value.palletNr + " ";
                            }
                            tempPackPalletSet.Clear();
                        }

                        else
                        {
                            db.setRecordByPos(Convert.ToInt32(posX), Convert.ToInt32(posY), pn, pallette, db.getSQLDateTimeFromFIFO(fifo));
                            log = pallette;
                        }
                        switch (side)
                        {
                            case "input1":
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput1N, false);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput1N, false);
                                    mEventLog.WriteEntry("input1 show cleared", EventLogEntryType.Information, port);
                                    break;
                                }
                            case "input2":
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput2N, false);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput2N, false);
                                    mEventLog.WriteEntry("input2 show cleared", EventLogEntryType.Information, port);
                                    break;
                                }
                            case "input3":
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput3N, false);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput3N, false);
                                    mEventLog.WriteEntry("input3 show cleared", EventLogEntryType.Information, port);
                                    break;
                                }
                            case "input4":
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput4N, false);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput4N, false);
                                    mEventLog.WriteEntry("input4 show cleared", EventLogEntryType.Information, port);
                                    break;
                                }
                            case "input5":
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput5N, false);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput5N, false);
                                    mEventLog.WriteEntry("input5 show cleared", EventLogEntryType.Information, port);
                                    break;
                                }
                            case "input6":
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput6N, false);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput6N, false);
                                    mEventLog.WriteEntry("input6 show cleared", EventLogEntryType.Information, port);
                                    break;
                               }

                        }

                        tcpSender.sendToIntermec(ip, port, "", "", "", "OK", "", " record added to db");

                        //==================================================================================================================================================================
                        //==================================================== ODZNACENIE VSETKYCH CHECKBOXOV po pridani palety do DB ======================================================   
                        //==================================================================================================================================================================
                        if (port == Constant.Port11)
                        {
                            cbtl1S1.Checked = false; cbRemove1S1.Checked = false; cbRemove1S1.Checked = false;
                        }
                        else
                        if (port == Constant.Port12)
                        {
                            cbtl1S2.Checked = false; cbRemove1S2.Checked = false; cbRemove1S2.Checked = false;
                        }
                        else
                        if (port == Constant.Port21)
                        {
                            cbtl2S1.Checked = false; cbRemove2S1.Checked = false; cbRemove2S1.Checked = false;
                        }
                        else
                        if (port == Constant.Port22)
                        {
                            cbtl2S2.Checked = false; cbRemove2S2.Checked = false; cbRemove2S2.Checked = false;
                        }
                        //==================================================================================================================================================================
                        //==================================================================================================================================================================


                        mEventLog.WriteEntry(side + "RstN" + " triggered, pallette added:" + log + "@X: " + posX + " Y: " + posY + " fifo: " + fifo, EventLogEntryType.Information, port);
                    }                 
                    AddLog("rstHandle Done " + "@X: " + posX + " Y: " + posY + " fifo: " + fifo, LogType.warning, port);


                    //axOPCComms1.Write("Group2", side + "RstN", false, WriteSetting.WaitUntilComplete);
                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.inputXRstN(side), false);

                    posX = "0";
                    posY = "0";
                    pn = "0";
                    pallette = "0";
                    fifo = "0";
                    ip = "0.0.0.0";
                    port = 0;
                    setPort = 0;
                }
                catch (Exception ex)
                {
                    mEventLog.WriteEntry("plc read error" + ex.ToString(), EventLogEntryType.Error, port);
                    tcpSender.sendToIntermec(ip, port, "", "", "", "NOK", "", "plc read error");
                }
            }
            #endregion

            #region output
            else if (side.StartsWith("output"))
            {
                try
                {
                    //bool plc = (bool)axOPCComms1.Read("Group1", side + "Rst");
                    bool plc = (bool)easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, Constant.outputXRstN(side));

                    if ((plc || plcOverride) && db.checkRecord(pallette))
                    {
                        string log = " ";
                        if (tempPackPalletSet.Count == packCount && pack)
                            foreach (palletSet value in tempPackPalletSet)
                            {
                                int a = db.clearRecord(value.palletNr);
                                if (a != 0)
                                    a = db.clearRecord(value.palletNr);
                                log += value.palletNr + " ";
                            }
                        else
                        {
                            int a = db.clearRecord(pallette);
                            if (a != 0)
                                a = db.clearRecord(pallette);
                            log = pallette;

                        }

                        tempPackPalletSet.Clear();

                        switch (side)
                        {
                            case "output1":
                                //axOPCComms1.Write("Group1", "showOutput1", false, WriteSetting.WaitUntilComplete);                                
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput1, false);
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput1, false);
                                mEventLog.WriteEntry("output1 show cleared", EventLogEntryType.Information, port);
                                break;
                            case "output2":
                                //axOPCComms1.Write("Group1", "showOutput2", false, WriteSetting.WaitUntilComplete);                                
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput2, false);
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput2, false);
                                mEventLog.WriteEntry("output2 show cleared", EventLogEntryType.Information, port);
                                break;
                            case "output3":
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput3, false);
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput3, false);
                                mEventLog.WriteEntry("output3 show cleared", EventLogEntryType.Information, port);
                                break;
                            case "output4":
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput4, false);
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput4, false);
                                mEventLog.WriteEntry("output4 show cleared", EventLogEntryType.Information, port);
                                break;
                            case "output5":
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput5, false);
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput5, false);
                                mEventLog.WriteEntry("output5 show cleared", EventLogEntryType.Information, port);
                                break;
                            case "output6":
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput6, false);
                                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput6, false);
                                mEventLog.WriteEntry("output6 show cleared", EventLogEntryType.Information, port);
                                break;

                        }
                        tcpSender.sendToIntermec(ip, port, "", "", "", "OK", "", "record removed from db");


                        //==================================================================================================================================================================
                        //==================================================== ODZNACENIE VSETKYCH CHECKBOXOV po odpbrani palety z DB ======================================================   
                        //==================================================================================================================================================================
                        
                        if (port == Constant.Port11) { cbtl1S1.Checked = false; cbRemove1S1.Checked = false; cbRemove1S1.Checked = false;}
                        else if (port == Constant.Port12) {cbtl1S2.Checked = false; cbRemove1S2.Checked = false; cbRemove1S2.Checked = false;}
                        else if (port == Constant.Port21) {cbtl2S1.Checked = false; cbRemove2S1.Checked = false; cbRemove2S1.Checked = false; }
                        else if (port == Constant.Port22) { cbtl2S2.Checked = false; cbRemove2S2.Checked = false; cbRemove2S2.Checked = false; }
                        
                        //==================================================================================================================================================================
                        //==================================================================================================================================================================

                        //mEventLog.WriteEntry(side + "Rst triggered, pallette removed:" + log, EventLogEntryType.Information, port);
                        AddLog("rstHandle Done" + "@X: " + posX + " Y: " + posY + " fifo: " + fifo, LogType.warning, port);

                    }

                    //mEventLog.WriteEntry("rstHandle Done" + "@X: " + posX + " Y: " + posY + " fifo: " + fifo, EventLogEntryType.Warning, port);
                    AddLog("rstHandle Done" + "@X: " + posX + " Y: " + posY + " fifo: " + fifo, LogType.warning, port);
                 
                    //easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, side.Trim() + "RstN", false);
                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.outputXRstN(side), false);


                    posX = "0";
                    posY = "0";
                    pn = "0";
                    pallette = "0";
                    fifo = "0";
                    ip = "0.0.0.0";
                    port = 0;
                    setPort = 0;
                }

                catch (Exception ex)
                {

                    tcpSender.sendToIntermec(ip, port, "", "", "", "NOK", "", "PLC OUTPUT ERROR");
                    AddLog("PLC OUTPUT ERROR:"+ ex.ToString(), LogType.error, port);
                    
                }
            }
            #endregion

            #region tempPack

            if (side.StartsWith("input1"))
            {
                packInput1PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("input2"))
            {
                packInput2PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("input3"))
            {
                packInput3PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("input4"))
            {
                packInput4PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("input5"))
            {
                packInput5PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("input6"))
            {
                packInput6PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("output1"))
            {
                packOutput1PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("output2"))
            {
                packOutput2PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("output3"))
            {
                packOutput3PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("output4"))
            {
                packOutput4PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("output5"))
            {
                packOutput5PalletSet = tempPackPalletSet;

            }
            else if (side.StartsWith("output6"))
            {
                packOutput6PalletSet = tempPackPalletSet;

            }
            #endregion

        }

        private static int getSection(ref int y)
        {
            int section = 1; //1-11
            if (Constant.Hala == 3)
            {
                if (y >= 11 && y <= 22)  //12-23
                    section = 2;
                else if (y >= 23 && y <= 30) //24-31
                    section = 3;
                else if (y >= 31 && y <= 36) //32-37
                    section = 4;
                else if (y >= 37 && y <= 48) //38-49
                    section = 5;
                else if (y >= 49) //50-55
                    section = 6;
                return section;

            }
            else
            if (Constant.Hala == 2)
            {
                y = 41 - Convert.ToInt32(y + 1); //pripocitamvam +1 lebo podla vzoru z haly 3 pred volanim getSection() bolo y = y -1
                if ((y + 1) > 16)
                {
                    //y = y - 16; //prepocet zapracovane do druhej sekcie po volani getSection()
                    section = 2;
                }

                return section;
            }

            throw new ArgumentException("@getSection()\\: DODANE NESPRAVNE CISLO HALY: " + Constant.Hala );
        }


        void AddLog(string pText, LogType pType, int pEventID = 0)
        {
            if (lbxLogs!=null) lbxLogs.Items.Add(pText + "\n");
            mEventLog.WriteEntry(pText, Helpers.getEventLogEntryType(pType), pEventID);
        }


        private void btnClearOutput_Click(object sender, EventArgs e)
        {
            //axOPCComms1.Write("Group1", "showOutput1", "false");
            //axOPCComms1.Write("Group1", "showOutput2", "false");
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput1, false);
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput2, false);
            if (Constant.Hala == 3)
            {
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput3, false);
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput4, false);
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput5, false);
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showOutput6, false);
            }

            packOutput1PalletSet.Clear();
            packOutput2PalletSet.Clear();
            if (Constant.Hala == 3)
            {
                packOutput3PalletSet.Clear();
                packOutput4PalletSet.Clear();
                packOutput5PalletSet.Clear();
                packOutput6PalletSet.Clear();
            }

            output1port = 0;
            output1SetPort = 0;
            
            output2port = 0;
            output2SetPort = 0;
            if (Constant.Hala == 3)
            {
                output3port = 0;
                output3SetPort = 0;

                output4port = 0;
                output4SetPort = 0;

                output5port = 0;
                output5SetPort = 0;

                output6port = 0;
                output6SetPort = 0;
            }
        }

        private void btnClearInput_Click(object sender, EventArgs e)
        {
            //axOPCComms1.Write("Group1", "showInput1N", "false");
            //axOPCComms1.Write("Group1", "showInput2N", "false");
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput1N, false);
            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput2N, false);
            if (Constant.Hala == 3)
            {
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput3N, false);
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput4N, false);
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput5N, false);
                easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, Constant.showInput6N, false);
            }

            packInput1PalletSet.Clear();
            packInput2PalletSet.Clear();
            if (Constant.Hala == 3)
            {
                packInput3PalletSet.Clear();
                packInput4PalletSet.Clear();
                packInput5PalletSet.Clear();
                packInput6PalletSet.Clear();
            }

            input1port = 0;
            input1SetPort = 0;

            input2port = 0;
            input2SetPort = 0;

            if (Constant.Hala == 3)
            {
                input3port = 0;
                input3SetPort = 0;

                input4port = 0;
                input4SetPort = 0;

                input5port = 0;
                input5SetPort = 0;

                input6port = 0;
                input6SetPort = 0;
            }
        }

        //sracky suvisiace s OPC serverom
        private string closeErrors(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;

            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Equals(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }

            if (hWnd.ToString() != "0")
            {
                SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                //mEventLog.WriteEntr("OPC Communication Client Error - error window closed", EventLogEntryType.Warning, 9999);
                AddLog("OPC Communication Client Error - error window closed", LogType.warning, 9999);

            }
            return hWnd.ToString();


        }

        #region PLC_PLC_PLC
        void easyDAClient1_ItemChanged(object sender, OpcLabs.EasyOpc.DataAccess.OperationModel.EasyDAItemChangedEventArgs e)
        {

            // Obtain the integer state we have passed in.
            
            //premenna, ktorej stav sa zmenil
            var stateAsString = e.Arguments.State.ToString();

            // Display the data
            if (e.Succeeded)
            {
                if (!e.Vtq.HasValue) { this.AddLog($"{stateAsString} *** NEPLATNA HODNOTA", LogType.error); return; }
                
                //this.AddLog($"{stateAsString}: {e.Vtq}", LogType.info);
            }
            else
            {
                Console.WriteLine($"{stateAsString} *** FAILURE: {e.ErrorMessageBrief}");
                this.AddLog($"{stateAsString} *** FAILURE: {e.ErrorMessageBrief}", LogType.error);
                // v pripade chyby predcase KONCIM
                return;
            }

        
            
            string valueString = e.Vtq.DisplayValue();
            object value = e.Vtq.Value;
            switch (stateAsString)
            {
                #region unused values
                case "input1N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            input1X = tempArray[0];
                            input1Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "input2N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            input2X = tempArray[0];
                            input2Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "input3N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            input3X = tempArray[0];
                            input3Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "input4N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            input4X = tempArray[0];
                            input4Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "input5N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            input5X = tempArray[0];
                            input5Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "input6N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            input6X = tempArray[0];
                            input6Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "output1N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            output1X = tempArray[0];
                            output1Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "output2N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            output2X = tempArray[0];
                            output2Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "output3N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            output3X = tempArray[0];
                            output3Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "output4N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            output4X = tempArray[0];
                            output4Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "output5N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            output5X = tempArray[0];
                            output5Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "output6N":
                    {
                        try
                        {
                            UInt16[] tempArray = new UInt16[2];
                            tempArray = (UInt16[])value;
                            output6X = tempArray[0];
                            output6Y = tempArray[1];

                        }
                        catch { }
                        break;
                    }
                case "showInput1N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showInput1 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showInput2N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showInput2 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showInput3N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showInput3 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showInput4N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showInput4 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showInput5N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showInput5 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showInput6N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showInput6 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showOutput1N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showOutput1 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showOutput2N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showOutput2 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showOutput3N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showOutput3 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showOutput4N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showOutput4 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showOutput5N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showOutput5 = tempBool;

                        }
                        catch { }
                        break;
                    }
                case "showOutput6N":
                    {
                        try
                        {
                            bool tempBool = false;
                            tempBool = (bool)value;
                            showOutput6 = tempBool;

                        }
                        catch { }
                        break;
                    }
                #endregion

                #region rst
                case "input1RstN":
                    {

                        //mEventLog.WriteEntry("##### PLC input1Rst volanie #####");
                        //axOPCComms1.Write("Group2", "input1RstN", "false");                         
                         if ((bool)value == true)
                             resetHandle(
                                 "input1", ref input1pallette, ref input1posX, ref input1posY, ref input1pn,
                                 ref input1fifo, ref input1ip, ref input1port, tcp1S1, false, ref input1SetPort
                                 );

                        break;
                    }
                case "input2RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC input2Rst volanie #####");
                        //axOPCComms1.Write("Group2", "input2RstN", "false");                        
                         if ((bool)value == true)
                             resetHandle(
                                 "input2", ref input2pallette, ref input2posX, ref input2posY, ref input2pn,
                                 ref input2fifo, ref input2ip, ref input2port, tcp1S2, false, ref input2SetPort
                                 );
                        break;
                    }
                case "input3RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC input3Rst volanie #####");
                        //axOPCComms1.Write("Group2", "input3RstN", "false");                        
                         if ((bool)value == true)
                             resetHandle(
                                 "input3", ref input3pallette, ref input3posX, ref input3posY, ref input3pn,
                                 ref input3fifo, ref input3ip, ref input3port, tcp1S1, false, ref input3SetPort
                                 );
                        break;
                    }
                case "input4RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC input4Rst volanie #####");
                        //axOPCComms1.Write("Group2", "input4RstN", "false");                        
                         if ((bool)value == true)
                             resetHandle(
                                 "input4", ref input4pallette, ref input4posX, ref input4posY, ref input4pn,
                                 ref input4fifo, ref input4ip, ref input4port, tcp1S2, false, ref input4SetPort
                                 );
                        break;
                    }
                case "input5RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC input5Rst volanie #####");
                        //axOPCComms1.Write("Group2", "input5RstN", "false");                        
                         if ((bool)value == true)
                             resetHandle(
                                 "input5", ref input5pallette, ref input5posX, ref input5posY, ref input5pn,
                                 ref input5fifo, ref input5ip, ref input5port, tcp1S1, false, ref input5SetPort
                                 );
                        break;
                    }
                case "input6RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC input6Rst volanie #####");
                        //axOPCComms1.Write("Group2", "input6RstN", "false");                        
                         if ((bool)value == true)
                             resetHandle(
                                 "input6", ref input6pallette, ref input6posX, ref input6posY, ref input6pn,
                                 ref input6fifo, ref input6ip, ref input6port, tcp1S2, false, ref input6SetPort
                                 );
                        break;
                    }
                case "output1RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC output1Rst volanie #####");
                        //axOPCComms1.Write("Group2", "output1RstN", "false");                        
                         if ((bool)value == true)
                         resetHandle(
                             "output1", ref output1pallette, ref output1posX, ref output1posY, ref output1pn,
                             ref output1fifo, ref output1ip, ref output1port, tcp2S1, false, ref output1SetPort
                             );
                        break;
                    }
                case "output2RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC output2Rst volanie #####");
                        //axOPCComms1.Write("Group2", "output2RstN", "false");

                        
                         if ((bool)value == true)
                             resetHandle(
                                 "output2", ref output2pallette, ref output2posX, ref output2posY, ref output2pn,
                                 ref output2fifo, ref output2ip, ref output2port, tcp2S2, false, ref output2SetPort
                                 );
                        break;
                    }
                case "output3RstN":
                    {
                        ///mEventLog.WriteEntry("##### PLC output3Rst volanie #####");
                        //axOPCComms1.Write("Group2", "output3RstN", "false");

                        
                         if ((bool)value == true)
                             resetHandle(
                                 "output3", ref output3pallette, ref output3posX, ref output3posY, ref output3pn,
                                 ref output3fifo, ref output3ip, ref output3port, tcp2S1, false, ref output3SetPort
                                 );
                        break;
                    }
                case "output4RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC output4Rst volanie #####");
                        //axOPCComms1.Write("Group2", "output4RstN", "false");

                        
                         if ((bool)value == true)
                             resetHandle(
                                 "output4", ref output4pallette, ref output4posX, ref output4posY, ref output4pn,
                                 ref output4fifo, ref output4ip, ref output4port, tcp2S2, false, ref output4SetPort
                                 );
                        break;
                    }
                case "output5RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC output5Rst volanie #####");
                        //axOPCComms1.Write("Group2", "output5RstN", "false");
                         if ((bool)value == true)
                             resetHandle(
                                 "output5", ref output5pallette, ref output5posX, ref output5posY, ref output5pn,
                                 ref output5fifo, ref output5ip, ref output5port, tcp2S1, false, ref output5SetPort
                                 );
                        break;
                    }
                case "output6RstN":
                    {
                        //mEventLog.WriteEntry("##### PLC output6Rst volanie #####");
                        //axOPCComms1.Write("Group2", "output6RstN", "false");                      
                         if ((bool)value == true)
                             resetHandle(
                                 "output6", ref output6pallette, ref output6posX, ref output6posY, ref output6pn,
                                 ref output6fifo, ref output6ip, ref output6port, tcp2S2, false, ref output6SetPort
                                 );
                        break;
                    }
                #endregion

                #region hmi
                case "controllN":
                    {
                        int ivalue = Convert.ToInt32(value);
                        if (ivalue == 0)
                            break;
                        bool[] select1 = new bool[16];
                        bool[] select2 = new bool[16];

                        //if(value != 0)
                        bool btnShow1 = Convert.ToBoolean(ivalue & (1 << 0));
                        bool btnSave1 = Convert.ToBoolean(ivalue & (1 << 1));
                        bool btnRemove1 = Convert.ToBoolean(ivalue & (1 << 2));

                        bool btnShow2 = Convert.ToBoolean(ivalue & (1 << 8));
                        bool btnSave2 = Convert.ToBoolean(ivalue & (1 << 9));
                        bool btnRemove2 = Convert.ToBoolean(ivalue & (1 << 10));


                        #region save1
                        if (btnSave1)
                        {
                            int x = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posx1N"));
                            int y = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posy1N"));
                            int z = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posz1N"));
                            string pn = db.getPosPn(x.ToString(), y.ToString()).Trim();
                            object[] a = db.getPackCount(x.ToString(), y.ToString());
                            int packCount = Convert.ToInt32(a[0]);
                            a = db.getChannel(x.ToString(), y.ToString());
                            int channel = Convert.ToInt32(a[0]);

                            int select = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "select1N"));

                            string log = "btnSave1 triggered on pos X:" + x.ToString() + ",Y:" + y.ToString() + ",Z:"
                                + z.ToString() + ",PN:" + pn + ",pack:" + packCount.ToString() + ",channel:" + channel.ToString() + ".\n" +
                                "select1:" + select.ToString() + "(";

                            for (int i = 0; i < 16; i++)
                            {
                                select1[i] = Convert.ToBoolean(select & (1 << i));
                                log += select1[i].ToString();
                            }
                            log += ")\n updated pn:";
                            if (packCount > 1)
                            {
                                for (int i = 0; i < packCount; i++)
                                {
                                    if (select1[i] == true)
                                    {
                                        string newpalletNr = easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "palA" + i + "N").ToString();
                                        db.updateRecordByPalletNr(display1PalletSet[i].palletNr, newpalletNr, display1PalletSet[i].fifoTime);
                                        log += ")\n updated pn:" + newpalletNr + "@" + display1PalletSet[i].palletNr + ",";
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < display1PalletSet.Count(); i++)
                                {
                                    if (select1[i] == true)
                                    {
                                        string newpalletNr = easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "palA" + i + "N").ToString();
                                        db.updateRecordByPalletNr(display1PalletSet[i].palletNr, newpalletNr, display1PalletSet[i].fifoTime);
                                        log += ")\n updated pn:" + newpalletNr + "@" + display1PalletSet[i].palletNr + ",";
                                    }
                                }
                            }
                            log += "\n";
                            db.updatePosByPos(x.ToString(), y.ToString(), easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "partNrAN").ToString(), easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "channelAN").ToString(), easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "packAN").ToString());


                            //reset button
                            ivalue &= ~(1 << 1);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "controllN", ivalue);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select1N", 0);
                            mEventLog.WriteEntry(log, EventLogEntryType.Information, 90);

                        }
                        #endregion

                        #region remove1
                        if (btnRemove1)
                        {
                            int select = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "select1N"));
                            for (int i = 0; i < 16; i++)
                            {
                                select1[i] = Convert.ToBoolean(select & (1 << i));
                            }
                            int x = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posx1N"));
                            int y = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posy1N"));
                            int z = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posz1N"));

                            string pn = db.getPosPn(x.ToString(), y.ToString()).Trim();
                            object[] a = db.getPackCount(x.ToString(), y.ToString());
                            int packCount = Convert.ToInt32(a[0]);
                            string log = "btnRemove1 triggered on pos X:" + x.ToString() + ",Y:" + y.ToString() + ",Z:"
                                + z.ToString() + ",PN:" + pn + ",pack:" + packCount.ToString() + ".\n" +
                                "select1:" + select.ToString() + "\n";
                            if (packCount > 1)
                            {
                                for (int i = 0; i < packCount; i++)
                                {
                                    if (select1[i] == true)
                                    {
                                        int c = db.clearRecord(display1PalletSet[i].palletNr);
                                        if (c != 0)
                                            c = db.clearRecord(display1PalletSet[i].palletNr);
                                    }
                                }

                            }
                            else
                            {

                                for (int i = 0; i < display1PalletSet.Count(); i++)
                                {
                                    if (select1[i] == true)
                                    {
                                        int c = db.clearRecord(display1PalletSet[i].palletNr);
                                        if (c != 0)
                                            c = db.clearRecord(display1PalletSet[i].palletNr);
                                    }
                                }

                            }
                            //reset button
                            ivalue &= ~(1 << 2);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "controllN", value);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select1N", 0);
                            mEventLog.WriteEntry(log, EventLogEntryType.Information, 90);
                            btnShow1 = true;

                        }
                        #endregion

                        #region show1
                        if (btnShow1)
                        {
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select1N", 0);
                            display1PalletSet.Clear();

                            int x = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posx1N"));
                            int y = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posy1N"));
                            int z = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posz1N"));

                            string pn = db.getPosPn(x.ToString(), y.ToString()).Trim();
                            object[] a = db.getPackCount(x.ToString(), y.ToString());
                            int packCount = Convert.ToInt32(a[0]);
                            a = db.getChannel(x.ToString(), y.ToString());
                            int channel = Convert.ToInt32(a[0]);
                            int rows = 0;
                            object[] b = null, c = null;
                            string log = "btnShow1 triggered on pos X:" + x.ToString() + ",Y:" + y.ToString() + ",Z:"
                                + z.ToString() + ",PN:" + pn + ",pack:" + packCount.ToString() + ",channel:" + channel.ToString() + ".\n";
                            if (packCount > 1)
                            {
                                b = db.getPackByPos(x, y, z, packCount);
                                c = db.getPackTimeByPos(x, y, z, packCount);

                                rows = packCount;
                            }
                            else
                            {
                                b = db.getPosByPos(x, y, "paletteNr");
                                c = db.getPosByPos(x, y, "FIFODatetime");

                                rows = b.Length;
                            }
                            log += "\ndisplay1pallet set:";

                            if (b.Length != 0)
                            {
                                if (packCount > 1)
                                {
                                    for (int i = 0; i < b.Count(); i++)
                                    {
                                        palletSet temp = new palletSet((string)b[i], (string)c[i]);
                                        display1PalletSet.Add(temp);
                                        log += (string)b[i] + ",";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < b.Count(); i++)
                                    {
                                        DateTime dt = Convert.ToDateTime(c[i].ToString());
                                        palletSet temp = new palletSet((string)b[i], dt.ToString("yyyy-MM-dd HH:mm:ss"));
                                        display1PalletSet.Add(temp);
                                        log += (string)b[i] + ",";
                                    }
                                }
                                for (int i = 0; i < rows; i++)
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "palA" + i.ToString() + "N", display1PalletSet[i].palletNr);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "timeA" + i.ToString() + "N", display1PalletSet[i].fifoTime);

                                }
                                for (int i = rows; i < 11; i++)
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "palA" + i.ToString() + "N", "0");
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "timeA" + i.ToString() + "N", "0");

                                }

                            }

                            else
                            {
                                for (int i = 0; i < 11; i++)
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "palA" + i.ToString() + "N", "0");
                                    //easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "timeA" + i.ToString(), "0");

                                }

                            }

                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "partNrAN", pn);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "packAN", packCount);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "channelAN", channel);

                            //reset button
                            ivalue &= ~(1 << 0);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "controllN", value);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select1N", 0);
                            mEventLog.WriteEntry(log, EventLogEntryType.Information, 90);
                        }
                        #endregion


                        #region save2
                        if (btnSave2)
                        {
                            int x = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posx2N"));
                            int y = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posy2N"));
                            int z = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posz2N"));
                            string pn = db.getPosPn(x.ToString(), y.ToString()).Trim();
                            object[] a = db.getPackCount(x.ToString(), y.ToString());
                            int packCount = Convert.ToInt32(a[0]);
                            a = db.getChannel(x.ToString(), y.ToString());
                            int channel = Convert.ToInt32(a[0]);

                            int select = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "select2N"));

                            string log = "btnSave2 triggered on pos X:" + x.ToString() + ",Y:" + y.ToString() + ",Z:"
                                + z.ToString() + ",PN:" + pn + ",pack:" + packCount.ToString() + ",channel:" + channel.ToString() + ".\n" +
                                "select2:" + select.ToString() + "(";

                            for (int i = 0; i < 16; i++)
                            {
                                select2[i] = Convert.ToBoolean(select & (1 << i));
                                log += select2[i].ToString();
                            }
                            log += ")\n updated pn:";
                            if (packCount > 1)
                            {
                                for (int i = 0; i < packCount; i++)
                                {
                                    if (select2[i] == true)
                                    {
                                        string newpalletNr = easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "pal" + i + "N").ToString();
                                        db.updateRecordByPalletNr(display2PalletSet[i].palletNr, newpalletNr, display2PalletSet[i].fifoTime);
                                        log += ")\n updated pn:" + newpalletNr + "@" + display2PalletSet[i].palletNr + ",";
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < display2PalletSet.Count(); i++)
                                {
                                    if (select2[i] == true)
                                    {
                                        string newpalletNr = easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "pal" + i + "N").ToString();
                                        db.updateRecordByPalletNr(display2PalletSet[i].palletNr, newpalletNr, display2PalletSet[i].fifoTime);
                                        log += ")\n updated pn:" + newpalletNr + "@" + display2PalletSet[i].palletNr + ",";
                                    }
                                }
                            }
                            log += "\n";
                            db.updatePosByPos(x.ToString(), y.ToString(), easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "partNrN").ToString(), easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "channelN").ToString(), easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "packN").ToString());


                            //reset button
                            ivalue &= ~(1 << 9);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "controllN", value);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select2N", 0);
                            mEventLog.WriteEntry(log, EventLogEntryType.Information, 90);

                        }
                        #endregion

                        #region remove2
                        if (btnRemove2)
                        {
                            int select = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "select2N"));
                            for (int i = 0; i < 16; i++)
                            {
                                select2[i] = Convert.ToBoolean(select & (1 << i));
                            }
                            int x = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posx2N"));
                            int y = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posy2N"));
                            int z = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posz2N"));

                            string pn = db.getPosPn(x.ToString(), y.ToString()).Trim();
                            object[] a = db.getPackCount(x.ToString(), y.ToString());
                            int packCount = Convert.ToInt32(a[0]);
                            string log = "btnRemove2 triggered on pos X:" + x.ToString() + ",Y:" + y.ToString() + ",Z:"
                                + z.ToString() + ",PN:" + pn + ",pack:" + packCount.ToString() + ".\n" +
                                "select2:" + select.ToString() + "\n";
                            if (packCount > 1)
                            {
                                for (int i = 0; i < packCount; i++)
                                {
                                    if (select2[i] == true)
                                    {
                                        int c = db.clearRecord(display2PalletSet[i].palletNr);
                                        if (c != 0)
                                            c = db.clearRecord(display2PalletSet[i].palletNr);
                                    }
                                }

                            }
                            else
                            {

                                for (int i = 0; i < display2PalletSet.Count(); i++)
                                {
                                    if (select2[i] == true)
                                    {
                                        int c = db.clearRecord(display2PalletSet[i].palletNr);
                                        if (c != 0)
                                            c = db.clearRecord(display2PalletSet[i].palletNr);
                                    }
                                }

                            }
                            //reset button
                            ivalue &= ~(1 << 10);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "controllN", value);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select2N", 0);
                            mEventLog.WriteEntry(log, EventLogEntryType.Information, 90);
                            btnShow2 = true;

                        }
                        #endregion

                        #region show2
                        if (btnShow2)
                        {
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select2N", 0);
                            display2PalletSet.Clear();

                            int x = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posx2N"));
                            int y = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posy2N"));
                            int z = Convert.ToInt32(easyDAClient1.ReadItemValue(Constant.ServerIP, Constant.ServerClass, "posz2N"));

                            string pn = db.getPosPn(x.ToString(), y.ToString()).Trim();
                            object[] a = db.getPackCount(x.ToString(), y.ToString());
                            int packCount = Convert.ToInt32(a[0]);
                            a = db.getChannel(x.ToString(), y.ToString());
                            int channel = Convert.ToInt32(a[0]);
                            int rows = 0;
                            object[] b = null, c = null;
                            string log = "btnShow2 triggered on pos X:" + x.ToString() + ",Y:" + y.ToString() + ",Z:"
                                + z.ToString() + ",PN:" + pn + ",pack:" + packCount.ToString() + ",channel:" + channel.ToString() + ".\n";
                            if (packCount > 1)
                            {
                                b = db.getPackByPos(x, y, z, packCount);
                                c = db.getPackTimeByPos(x, y, z, packCount);

                                rows = packCount;
                            }
                            else
                            {
                                b = db.getPosByPos(x, y, "paletteNr");
                                c = db.getPosByPos(x, y, "FIFODatetime");

                                rows = b.Length;
                            }
                            log += "\ndisplay2pallet set:";

                            if (b.Length != 0)
                            {
                                if (packCount > 1)
                                {
                                    for (int i = 0; i < b.Count(); i++)
                                    {
                                        palletSet temp = new palletSet((string)b[i], (string)c[i]);
                                        display2PalletSet.Add(temp);
                                        log += (string)b[i] + ",";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < b.Count(); i++)
                                    {
                                        DateTime dt = Convert.ToDateTime(c[i].ToString());
                                        palletSet temp = new palletSet((string)b[i], dt.ToString("yyyy-MM-dd HH:mm:ss"));
                                        display2PalletSet.Add(temp);
                                        log += (string)b[i] + ",";
                                    }
                                }
                                for (int i = 0; i < rows; i++)
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "pal" + i.ToString() + "N", display2PalletSet[i].palletNr);
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "time" + i.ToString() + "N", display2PalletSet[i].fifoTime);

                                }
                                for (int i = rows; i < 11; i++)
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "pal" + i.ToString() + "N", "0");
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "time" + i.ToString() + "N", "0");

                                }

                            }

                            else
                            {
                                for (int i = 0; i < 11; i++)
                                {
                                    easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "pal" + i.ToString() + "N", "0");
                                    //easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "time" + i.ToString(), "0");

                                }

                            }

                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "partNrN", pn);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "packN", packCount);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "channelN", channel);

                            //reset button
                            ivalue &= ~(1 << 8);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "controllN", value);
                            easyDAClient1.WriteItemValue(Constant.ServerIP, Constant.ServerClass, "select2N", 0);
                            mEventLog.WriteEntry(log, EventLogEntryType.Information, 90);
                        }
                        #endregion

                        break;
                    }
                #endregion
                default:
                    break;
            }
            //DisplayValue((object)e.value);
        }
        #endregion


        void nacitajConfig() {

            Constant.Hala =  Properties.Settings.Default.hala;
            
            lbHallNumber.Text = Constant.Hala.ToString();

            Constant.Port11 = Properties.Settings.Default.Port11;
            Constant.Port12 = Properties.Settings.Default.Port12;
            Constant.Port21 = Properties.Settings.Default.Port21;
            Constant.Port22 = Properties.Settings.Default.Port22;


            Constant.cPort11 = Properties.Settings.Default.cPort11;
            Constant.cPort12 = Properties.Settings.Default.cPort12;
            Constant.cPort21 = Properties.Settings.Default.cPort21;
            Constant.cPort22 = Properties.Settings.Default.cPort22;
                                                           

           Constant.cIP11 = Properties.Settings.Default.cIP11;
           Constant.cIP12 = Properties.Settings.Default.cIP12;
           Constant.cIP21 = Properties.Settings.Default.cIP21;
           Constant.cIP22 = Properties.Settings.Default.cIP22;


            tbIP1S1.Text = Constant.cIP11;
            tbIP1S2.Text = Constant.cIP12;
            tbIP2S1.Text = Constant.cIP21;
            tbIP2S2.Text = Constant.cIP22;


            tbxPort11.Text = Constant.cPort11.ToString();
            tbxPort12.Text = Constant.cPort12.ToString();
            tbxPort21.Text = Constant.cPort21.ToString();
            tbxPort22.Text = Constant.cPort22.ToString();

            if (Constant.Hala == 2)
            {
                Constant.ComChannel = "ZKWPBL2016";
                Constant.VarTable = "2016H2";
            }
            else
            {
                Constant.ComChannel = "ZKWPBL2019";
                Constant.VarTable = "2019H3";
            }

            Constant.AktualizujPLCCety();
    }
    }
}
