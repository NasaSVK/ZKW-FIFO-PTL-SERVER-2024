using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KEPZET
{
    class tcp
    {
        static Thread thread = null;
        public static string dataRcvd = null, //prijate data - jedna premenna zdielana vsetkymi citackami
            ip = null; //adresa servera == 10.16.0.141
        public int port = 0; //port servera pre komunikaciu s vybranou citackou (pre 1S1 = 2000)
        private TcpListener listener = null;

        public Action UpdateProgress;

        //private EventLog mEventLog = new EventLog();
        private dgEventLogAdd EventLogAdd;

        private void LoadData()
        {
            dataRcvd = getData();
            this.UpdateProgress();
        }

        public void tcpSet(string ipAddr, int portNr, dgEventLogAdd pEventLogAdd)
        {
            ip = ipAddr;
            port = portNr; //port servera pre komunikaciu s vybranou citackou
            
            //tcpListener = new TCPListener(ip,portNr);
            //UpdateProgress = this.LoadData;


            /*
            if (!EventLog.SourceExists("zkwPBL"))
                EventLog.CreateEventSource("zkwPBL", "zkwPBLLog");
            mEventLog.Source = "zkwPBL";*/
            this.EventLogAdd = pEventLogAdd;
        }

        public string getData()
        {
            return dataRcvd;
        }

        public void startListener()
        {
            thread = new Thread(Listen);
            thread.Start();
        }

        public void stopListener()
        {
            //listener.EndAcceptTcpClient();
            listener.Stop();
            thread.Abort();
            thread = null;
        }

        private IPAddress getIp()
        {
            return IPAddress.Parse(ip);
        }


        public async void send(string ipAddress, int port, string text)
        {
            {
                try
                {
                    //IPAddress ipp = IPAddress.Parse(ipAddress);
                    String str = text;
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes(str);

                    TcpClient tcpclnt = new TcpClient();
                    tcpclnt.SendBufferSize = 8192;
                    ////mEventLog.WriteEntry("Connecting.....(" + ip.ToString() + ":" + port.ToString() + ")", EventLogEntryType.Information, 50);
                    bool result = tcpclnt.ConnectAsync(IPAddress.Parse(ipAddress), port).Wait(3000);
                    if (!result) {
                        this.EventLogAdd("Chyba pri udalosti: @" + text + "* during sending message to \\\\" + IPAddress.Parse(ipAddress) + ":" + port.ToString(), LogType.error, 50);
                        return;
                    }

                    ////mEventLog.WriteEntry("Connected(" + ip.ToString() + ":" + port.ToString() + ")", EventLogEntryType.Information, 50);


                    Stream stm = tcpclnt.GetStream();



                    //mEventLog.WriteEntry("Transmitting \"" + str + "\".....(" + ip.ToString() + ":" + port.ToString() + ")", EventLogEntryType.Information, 50);
                    this.EventLogAdd("Transmitting \"" + str + "\".....(" + IPAddress.Parse(ipAddress).ToString() + ":" + port.ToString() + ")", LogType.info, 50);

                    stm.Write(ba, 0, ba.Length);
                    stm.Close();

                    byte[] bb = new byte[100];
                    tcpclnt.Close();
                    //tcpclnt.EndConnect();
                    //mEventLog.WriteEntry("Closing.....(" + ip.ToString() + ":" + port.ToString() + ")", EventLogEntryType.Information, 50);

                }

                catch (Exception ex)
                {
                    ////Console.WriteLine("Error..... " + ex.StackTrace);
                    //mEventLog.WriteEntry("Chyba pri udalosti: " + text + "\n\n send error!(" + ip.ToString() + ":" + port.ToString() + "):" + ex.ToString(), EventLogEntryType.Error, 50);
                    this.EventLogAdd("Chyba pri udalosti: @" + text + "* send error! \\\\" + IPAddress.Parse(ipAddress).ToString() + ":" + port.ToString() , LogType.error, 50);
                }
            }
        }

        //samotny listener, ktory vsak nie je vizany na konkretnu IPcku ciacky (ZDROJA)
        public void Listen()
        {
            IPAddress localAdd = getIp(); //10.16.0.141
            listener = new TcpListener(localAdd, port);

            listener.Start();
            //mEventLog.WriteEntry("listener start at:" + localAdd + ":" + port.ToString() + "...", EventLogEntryType.Information, 30);
            EventLogAdd("listener start at:" + localAdd + ":" + port.ToString() + "...", LogType.info,30);

            while (listener.Server.IsBound)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream nwStream = client.GetStream();
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    //##################################################################
                    //mEventLog.WriteEntry("received at " + port + " : " + dataReceived, EventLogEntryType.Information, 30);
                    EventLogAdd("received at " + port + " : " + dataReceived, LogType.info,30);
                    //##################################################################

                    dataRcvd = dataReceived;
                    nwStream.Write(buffer, 0, bytesRead);

                    LoadData();

                    client.Close();
                    nwStream.Close();
                    nwStream.Dispose();
                }
                catch (Exception ex)
                {
                    //mEventLog.WriteEntry("listener error:" + ex.ToString(), EventLogEntryType.Error, 30);
                    this.EventLogAdd("listener error:" + ex.ToString(), LogType.error);
                }
            }
        }
        /// <summary>
        /// ZASLANIE DAT CITACKE (55x volana v MainForm-e)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="pn"></param>
        /// <param name="pos"></param>
        /// <param name="prev"></param>
        /// <param name="result"></param>
        /// <param name="multipack"></param>
        /// <param name="info"></param>
        public void sendToIntermec(string ip, int port, string pn, string pos, string prev, string result, string multipack, string info)
        {
            string text = null;

            text = pn;
            text += "\n";
            text += pos;
            text += "\n";
            text += prev;
            text += "\n";
            text += result;
            text += "\n";
            text += multipack;
            text += "\n";
            text += info;
            text += "\n";

            send(ip, port, text);
        }
    }
}
