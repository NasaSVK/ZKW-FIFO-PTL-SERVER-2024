using KEPZET.sap;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KEPZET
{
    class sql
    {
        //private SqlConnection DBConnection = new SqlConnection(System.IO.File.ReadAllText(@"c:\SimOPC\configDB.txt")); //new SqlConnection("server=10.184.144.110;database=VolvoSPA;User ID=ba_OPC;Password=ba_OPC01;Trusted_Connection=False");//povodny server 10.184.144.106
        //private SqlConnection DBConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=zkwPBL19;User Id=nasask;Password=nasask");
        //private readonly string connString = "server=(local)\\SQLEXPRESS;database=zkwPBL;User Id=nasask;Password=nasask";
        //private readonly string connString = "User ID=sknasa;Password=sknasa;Initial Catalog=zkwPBL19;Data Source=(local)\\SQLEXPRESS";
        
        private readonly string connString = "User ID=sknasa;Password=sknasa;Initial Catalog=zkwPBL;Data Source=(local)\\SQLEXPRESS";
        //private EventLog mEventLog = new EventLog();
        private dgEventLogAdd EventLogAdd;

        #region testing
        public object[] testQuery(string Line)
        {
            SqlConnection DBConnection = new SqlConnection(connString);
            try
            {
                string[] retValues;
                retValues = new string[10];

                SqlDataReader dr = null;
                string query = "select * from layout where posX like '4'";
                SqlCommand tq = new SqlCommand(query, DBConnection);
                DBConnection.Open();

                dr = tq.ExecuteReader();

                while (dr.Read())
                {
                    Console.WriteLine(dr["posX"]);
                    Console.WriteLine(dr["posY"]);
                    Console.WriteLine(dr["partNr"]);

                }
                DBConnection.Close();

                return retValues;
            }
            catch (Exception e)
            {
                DBConnection.Close();
                return null;
            }

        }
        #endregion

        public sql(dgEventLogAdd pEventLogAdd, int pHala)
        {
            /*
            if (!EventLog.SourceExists("zkwPBL19"))
                EventLog.CreateEventSource("zkwPBL19", "zkwPBL19Log");
            mEventLog.Source = "zkwPBL19";*/
            this.EventLogAdd = pEventLogAdd;

            if (pHala == 2)
                connString = "User ID=sknasa;Password=sknasa;Initial Catalog=zkwPBL;Data Source=(local)\\SQLEXPRESS";
            else
                if (pHala == 3)
                    connString = "User ID=sknasa;Password=sknasa;Initial Catalog=zkwPBL19;Data Source=(local)\\SQLEXPRESS";


        }
        #region private methods

        ///<summary>
        ///query w return values
        ///</summary>
        private string[] readerQuery(string query, int retValCount)
        {
            SqlConnection DBConnection = new SqlConnection(connString);
            try
            {
                string[] retValues;
                retValues = new string[retValCount];
                if (DBConnection.State != ConnectionState.Closed)
                {
                    DBConnection.Close();
                    //mEventLog.WriteEntry("dbconnection closed(" + DBConnection.State.ToString() + ")", EventLogEntryType.Error, 70);
                    this.EventLogAdd("dbconnection closed(" + DBConnection.State.ToString() + ")",LogType.error);
                }
                SqlDataReader dr = null;
                SqlCommand tq = new SqlCommand(query, DBConnection);
                DBConnection.Open();

                dr = tq.ExecuteReader();

                while (dr.Read())
                {
                    for (int i = 0; i < retValCount; i++)
                    { retValues[i] = dr[i].ToString(); }

                }
                DBConnection.Close();
                dr.Close();
                dr.Dispose();
                tq.Dispose();


                return retValues;
            }
            catch (Exception e)
            {                
                //mEventLog.WriteEntry("readerQuery:" + query + "\nError:" + e.ToString(), EventLogEntryType.Error, 10);
                this.EventLogAdd("readerQuery:" + query + "\nError:" + e.ToString(),LogType.error);
                return null;
            }
            finally
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }
        }

        ///<summary>
        ///query w return values and multiple rows
        ///</summary>
        private string[] readerQuery(string query, int retValCount, int retRowCount)
        {
            SqlConnection DBConnection = new SqlConnection(connString);
            try
            {
                List<string> retValues = new List<string>();
                if (DBConnection.State != ConnectionState.Closed)
                {
                    DBConnection.Close();
                    //mEventLog.WriteEntry("dbconnection closed(" + DBConnection.State.ToString() + ")", EventLogEntryType.Error, 71);
                    this.EventLogAdd("dbconnection closed(" + DBConnection.State.ToString() + ")", LogType.error);
                }
                SqlDataReader dr = null;
                SqlCommand tq = new SqlCommand(query, DBConnection);
                DBConnection.Open();

                dr = tq.ExecuteReader();

                do
                {
                    int count = dr.FieldCount;
                    while (dr.Read())
                    {
                        for (int i = 0; i < count; i++)
                        {
                            retValues.Add(dr.GetValue(i).ToString().Trim());

                        }
                    }
                } while (dr.NextResult());

                DBConnection.Close();
                dr.Close();
                dr.Dispose();
                tq.Dispose();

                return retValues.ToArray();
            }
            catch (Exception e)
            {                
                //mEventLog.WriteEntry("readerQuery2:" + query + "\nError:" + e.ToString(), EventLogEntryType.Error, 10);
                this.EventLogAdd("readerQuery2:" + query + "\nError:" + e.ToString(), LogType.error);
                return null;
            }
            finally
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }
        }

        ///<summary>
        ///query w/o return values
        ///</summary>
        private int readerQuery(string query)
        {
            SqlConnection DBConnection = new SqlConnection(connString);
            try
            {
                int retValue = -1;
                if (DBConnection.State != ConnectionState.Closed)
                {
                    DBConnection.Close();
                    //mEventLog.WriteEntry("dbconnection closed(" + DBConnection.State.ToString() + ")", EventLogEntryType.Error, 72);
                    this.EventLogAdd("dbconnection closed(" + DBConnection.State.ToString() + ")", LogType.error);
                }
                SqlCommand tq = new SqlCommand(query, DBConnection);
                DBConnection.Open();

                tq.ExecuteNonQuery();

                DBConnection.Close();
                tq.Dispose();
                retValue = 0;
                return retValue;
            }
            catch (Exception e)
            {                
                //mEventLog.WriteEntry("readerQuery3:" + query + "\nError:" + e.ToString(), EventLogEntryType.Error, 10);
                this.EventLogAdd("readerQuery3:" + query + "\nError:" + e.ToString(), LogType.error);
                return -2;
            }
            finally
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }
        }

        #endregion

        #region public methods
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        ///<summary>
        ///returns object array of values for selected pallet
        ///</summary>
        public /*object[]*/ PN_DATE_TIME getRecord(string paletteNr)
        {
            PN_DATE_TIME result = new PN_DATE_TIME();
            object[] o = readerQuery("SELECT [partNr], [posX], [posY], [FIFODatetime] FROM [dbo].[warehouseDB] WHERE [paletteNr] like '" + paletteNr + "%'", 4);
            //string[] test = (string[])o;
            result.PN = o[0].ToString().Trim();
            result.posX = o[1] == null ? -1 : Convert.ToInt32(o[1]);
            result.posY = o[2] == null ? -1 : Convert.ToInt32(o[2]);
            result.GrDate = (Convert.ToDateTime(o[3])).Date.ToString().Trim();
            result.GrTime = (Convert.ToDateTime(o[3])).TimeOfDay.ToString().Trim();
            //return o;
            return result;

        }
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        ///<summary>
        ///returns object array of values for selected position
        ///</summary>
        public object[] getPosAgeData(string x, string y)
        {
            object[] o = readerQuery("SELECT [aLTime],[aLCheck],[aLBPTime],[aLBPStart],[aLBP] FROM [dbo].[layout] WHERE [posX] = " + x + " AND [posY] = " + y + ";", 5);
            string[] test = (string[])o;
            return o;

        }
        ///<summary>
        ///returns packCount for selected position
        ///</summary>
        public object[] getPackCount(string x, string y)
        {
            object[] o = readerQuery("SELECT [pack] FROM [dbo].[layout] WHERE [posX] = " + x + " AND [posY] = " + y, 1);
            string[] test = (string[])o;
            return o;

        }
        ///<summary>
        ///returns channel for selected position
        ///</summary>
        public object[] getChannel(string x, string y)
        {
            object[] o = readerQuery("SELECT [channel] FROM [dbo].[layout] WHERE [posX] = " + x + " AND [posY] = " + y, 1);
            string[] test = (string[])o;
            return o;

        }
        ///<summary>
        ///returns object array of sixpack values in selected position
        ///</summary>
        public string[] getPack(string paletteNr, string partNr, int pack)
        {
            //string[] o = readerQuery(
            //    "declare @row int;\ndeclare @position int;\ndeclare @FromRange int;\ndeclare @ToRange int;\nwith temp as(\n"
            //    + "SELECT ROW_NUMBER() OVER (ORDER BY [id]) as row , id,paletteNr \nFROM dbo.warehouseDB where [partNr] like '" + partNr + "%' )\n"
            //    + " select @row = [row] from temp \nwhere [paletteNr] like '" + paletteNr + "%' \nset @position = (@row-1)/6; \nset @FromRange = 1+(6*@position)"
            //    + " \nset @ToRange = 6+(6*@position) \nSELECT top(@ToRange-@FromRange+1) [paletteNr]"
            //    + " \nFROM [dbo].[warehouseDB] \nwhere partNr like '" + partNr + "%' and [id] not in (select top (@FromRange-1) [id] from [dbo].[warehouseDB]) \nORDER BY [FIFODatetime],[id]", 1, 6);
            //oprava chyby vyberu z db - chyba spociva v nefunkčnom vyhladavani paliet ktoré nie sú na rade po oprave je možné skenovať aj palety vnutri regálu je to vhodné?
            string[] o = readerQuery(
                            "declare @row int; declare @position int; declare @FromRange int; declare @ToRange int; declare @pack int; declare @pn nchar(20); declare @pallet nchar(20); " +
                            "set @pn = N'" + partNr + "'; set @pallet = N'" + paletteNr + "'; set @pack = " + pack + ";" +
                            "with temp as(\nSELECT ROW_NUMBER() OVER (ORDER BY [id]) as row , id,paletteNr\nFROM dbo.warehouseDB where [partNr] = @pn )\nselect @row = [row] from temp " +
                            "where [paletteNr] = @pallet\nset @position = (@row-1)/@pack;\nset @FromRange = 1+(@pack*@position)\nset @ToRange = @pack+(@pack*@position)" +
                            "SELECT top(@ToRange-@FromRange+1) [paletteNr]\nFROM [dbo].[warehouseDB]\nwhere partNr = @pn and [id] not in " +
                            "(select top (@FromRange-1) [id] from [dbo].[warehouseDB] where partNr = @pn ORDER BY [FIFODatetime],[id])\nORDER BY [FIFODatetime],[id]"
                            , 1, pack);

            string[] test = (string[])o;
            return o;

        }

        ///<summary>
        ///returns object array of pack values in set by paletteNr
        ///</summary>
        public string[] getPackByPn(string paletteNr, string partNr, int count, int number)
        {

            string[] o = readerQuery(
                            "declare @row int;\ndeclare @position int;\ndeclare @FromRange int;\ndeclare @ToRange int;\nwith temp as(\n"
                            + "SELECT ROW_NUMBER() OVER (ORDER BY [id]) as row , id,paletteNr \nFROM dbo.warehouseDB where [partNr] like '" + partNr + "%' )\n"
                            + " select @row = [row] from temp \nwhere [paletteNr] like '" + paletteNr + "%' \nset @position = (@row-1)/" + count + "; \nset @FromRange = 1+(" + count + "*@position)"
                            + " \nset @ToRange = " + count + "+(" + count + "*@position) \nSELECT top(@ToRange-@FromRange+1) [paletteNr]"
                            + " \nFROM [dbo].[warehouseDB] \nwhere partNr = N'" + partNr + "' and [id] not in (select top (@FromRange-1) [id] from [dbo].[warehouseDB] where partNr = N'" + partNr + "' ORDER BY [FIFODatetime],[id]) \nORDER BY [FIFODatetime],[id]", 1, count);

            string[] test = (string[])o;
            return o;

        }

        ///<summary>
        ///returns object array of pack values in set by paletteNr
        ///</summary>
        public string[] getPosByPos(int x, int y, string field)
        {

            string[] o = readerQuery(
                            "SELECT [" + field + "]\nFROM [dbo].[warehouseDB]\nWhere [posX] = " + x + " and [posY] = " + y + "ORDER BY [FIFODatetime],[id]", 0, 0);

            string[] test = (string[])o;
            return o;

        }
        ///<summary>
        ///returns object array of pack values in set by position
        ///</summary>
        public string[] getPackByPos(int posX, int posY, int posZ, int count)
        {

            string[] o = readerQuery(
                "declare @FromRange int;\ndeclare @ToRange int;\ndeclare @count int;\ndeclare @posX int;\ndeclare @posY int;\ndeclare @posZ int;\nset @count = " + count + ";\n" +
                "set @posX = " + posX + ";\nset @posY = " + posY + ";\nset @posZ = " + posZ + ";\nset @FromRange = 1+(@count*(@posZ-1))\nset @ToRange = @count+(@count*(@posZ-1))\n" +
                "SELECT top(@ToRange-@FromRange+1) [paletteNr]\nFROM [dbo].[warehouseDB]\n" +
                "where (posX = @posX and posY = @posY) and [id] not in (select top (@FromRange-1) [id] from [dbo].[warehouseDB]" +
                "\nwhere (posX = @posX and posY = @posY) ORDER BY [FIFODatetime],[id])\nORDER BY [FIFODatetime],[id]"
                            , 1, count);

            string[] test = (string[])o;
            return o;

        }
        ///<summary>
        ///returns object array of pack values in set by position
        ///</summary>
        public string[] getPackTimeByPos(int posX, int posY, int posZ, int count)
        {

            string[] o = readerQuery(
                "declare @FromRange int;\ndeclare @ToRange int;\ndeclare @count int;\ndeclare @posX int;\ndeclare @posY int;\ndeclare @posZ int;\nset @count = " + count + ";\n" +
                "set @posX = " + posX + ";\nset @posY = " + posY + ";\nset @posZ = " + posZ + ";\nset @FromRange = 1+(@count*(@posZ-1))\nset @ToRange = @count+(@count*(@posZ-1))\n" +
                "SELECT top(@ToRange-@FromRange+1) [FIFODatetime]\nFROM [dbo].[warehouseDB]\n" +
                "where (posX = @posX and posY = @posY) and [id] not in (select top (@FromRange-1) [id] from [dbo].[warehouseDB]" +
                "\nwhere (posX = @posX and posY = @posY) ORDER BY [FIFODatetime],[id])\nORDER BY [FIFODatetime],[id]"
                            , 1, count);

            string[] test = (string[])o;
            return o;

        }

        ///<summary>
        ///returns object array of sixpack values in selected position
        ///</summary>
        public string getPnByPalletNr(string paletteNr)
        {
            string[] o = readerQuery(
                "SELECT partNr FROM dbo.warehouseDB where [paletteNr] = N'" + paletteNr + "'", 1);
            string test = (string)o[0];
            return test;

        }

        ///<summary>
        ///returns object array of sixpack values in selected position
        ///</summary>
        public string getLastPalletinDb(string partNr)
        {
            string[] o = readerQuery(
                "SELECT TOP 1 [paletteNr] FROM [warehouseDB] WHERE [partNr] = N'" + partNr + "' ORDER BY [FIFODatetime] DESC, [id] DESC", 1);
            if (o == null)
                o = readerQuery(
                "SELECT TOP 1 [paletteNr] FROM [warehouseDB] WHERE [partNr] = N'" + partNr + "' ORDER BY [FIFODatetime] DESC, [id] DESC", 1);
            if (o == null)
            {
                this.EventLogAdd("getLastPalletError:" + partNr + "\nError:", LogType.error);
                return "";
            }
            string test = (string)o[0];
            if (test == null)
            {
                this.EventLogAdd("getLastPalletError2:" + partNr + "\nError:", LogType.error);
                return "";
            }


            return test;

        }

        ///<summary>
        ///returns coords of first free record
        ///</summary>
        public object[] getFreePos(string pn)
        {
            object[] o = readerQuery("SELECT TOP 1 [posX],[posY],[partNr],[channel],[count],[maxcount],[pack] FROM [dbo].[layout] WHERE [partNr] like '" + pn + "%' and [count] < [maxcount] ORDER BY [channel]", 7);
            return o;

        }

        ///<summary>
        ///text existencie PNka
        ///</summary>
        public bool existPNinDB (string pn)
        {
            object[] o = readerQuery("SELECT TOP 1 [posX],[posY],[partNr],[channel],[count],[maxcount],[pack] FROM [dbo].[layout] WHERE [partNr] like '" + pn + "%'", 7);
            if ((o == null) || (o[0] == null))
                return false;
            else return true;

        }

        ///<summary>
        ///gets pn at selected position
        ///</summary>
        public string getPosPn(string x, string y)
        {
            object[] o = readerQuery("SELECT [partNr] FROM [dbo].[layout] where posX like '" + x + "' and posY like '" + y + "'", 1);
            string temp = "";
            try
            {
                temp = o[0].ToString().Trim();
            }
            catch { }

            return temp;

        }

        ///<summary>
        ///update record by paletteNr
        ///</summary>
        public int updatePosByPos(string x, string y, string newPartNr, string newChannel, string newPack)
        {
            //return readerQuery("UPDATE [dbo].[layout]\nSET [partNr] = N'" + newPartNr + "'\n,[channel] = " + newChannel + "\n,[pack] = " + newPack + "\nWHERE [posX] = " + x + " AND [posY] = " + y + "");
            return readerQuery("declare @pack int;\nset @pack = " + newPack + ";\nUPDATE[dbo].[layout]\nSET[partNr] = N'" + newPartNr + "'\n,[channel] = " + newChannel + "\n,[pack] = @pack\n,[maxcount] = @pack*6\nWHERE[posX] = " + x + " AND[posY] = " + y);
        }

        ///<summary>
        ///update record by paletteNr
        ///</summary>
        public int updateRecordByPalletNr(string currentPalletNr, string newPalletNr, string newFifoDatetime)
        {
            return readerQuery("UPDATE [dbo].[warehouseDB]\nSET [paletteNr] = N'" + newPalletNr + "',\n[FIFODatetime] = '" + newFifoDatetime + "'\nWHERE [paletteNr] = N'" + currentPalletNr + "'");

        }

        ///<summary>
        ///set record by id
        ///</summary>
        public int setRecordByPos(int x, int y, string partNr, string paletteNr, string fifoDatetime)
        {
            return readerQuery("declare @partNrFixed nvarchar(20), @channel int, @posX int, @posY int, @count int; set @posX = " + x + "; set	@posY = " + y + "; set @count = 0;"
                + " SELECT @partNrFixed = [partNr], @channel = [channel] FROM [dbo].[layout] WHERE [posX] like @posX and [posY] like @posY "
                + "INSERT INTO [dbo].[warehouseDB]([partNr], [partNrFixed], [channel], [paletteNr], [posX], [posY], [FIFODatetime]) "
                + "VALUES('" + partNr + "', @partNrFixed, @channel, '" + paletteNr + "', @posX, @posY,CONVERT(datetime,'" + fifoDatetime + "')) "
                + "SELECT @count = COUNT(*) FROM [dbo].[warehouseDB] WHERE [posX] like @posX and [posY] like @posY "
                + "UPDATE [dbo].[layout] SET [count] = @count WHERE [posX] like @posX and [posY] like @posY");

        }

        ///<summary>
        ///clear record by paletteNr
        ///</summary>
        public int clearRecord(string paletteNr)
        {
            string query = "declare @posX int, @posY int, @count int; set @posX = 0; set	@posY = 0; set @count = 0; "
                    + "SELECT @posX = [posX], @posY = [posY] FROM [dbo].[warehouseDB] WHERE [paletteNr] = N'" + paletteNr + "' DELETE from [dbo].[warehouseDB]"
                    + " where [paletteNr] = N'" + paletteNr + "' SELECT @count = COUNT(*) FROM [dbo].[warehouseDB] WHERE [posX] like @posX and [posY] like @posY"
                    + " UPDATE [dbo].[layout] SET [count] = @count WHERE [posX] like @posX and [posY] like @posY";
            int retVal = readerQuery(query);
            if (checkRecord(paletteNr))
            {
                this.EventLogAdd("clearRecordError(" + paletteNr + ")", LogType.error);
                return -3;
            }

            else
            {
                //mEventLog.WriteEntry("sql.clearRecord:" + paletteNr, EventLogEntryType.Information, 10);
                this.EventLogAdd("sql.clearRecord:" + paletteNr, LogType.info);
                return retVal;
            }

        }

        ///<summary>
        ///check record in db
        ///</summary>
        public bool checkRecord(string paletteNr)
        {
            int count = 0;

            if (paletteNr == "")
                return false;

            //DOROBENE DODATOCNE
            //return false;
            
            object[] o = readerQuery("SELECT count(*) FROM [dbo].[warehouseDB] WHERE [paletteNr] = '" + paletteNr + "'", 1);
            try
            {
                //Tu CHYBA pri citani z DB
                //#############################################
                //#############################################
                count = Convert.ToInt32(o[0].ToString());
                //#############################################
                //#############################################
            }
            catch (Exception ex)
            {
                //mEventLog.WriteEntry("checkRecord(" + paletteNr + ")=" + count.ToString() + " error:" + ex.ToString(), EventLogEntryType.Error, 10);
                this.EventLogAdd("checkRecord(" + paletteNr + ")=" + count.ToString() + " error:" + ex.ToString(), LogType.error);
            }
            if (count >= 1)
                return true;
            else
                return false;

        }

        #endregion

        #region helper methods
        public string getSQLDateTimeFromFIFO(string fifo)
        {
            //20160422 73335
            //20160202 35 00:00:35

            string retVal = fifo, tempDate = null, year = null, month = null, day = null, hour = null, minute = null, second = null;
            try
            {
                year = fifo.Substring(6, 4);
                month = fifo.Substring(3, 2);
                day = fifo.Substring(0, 2);
                //73335
                second = fifo.Substring(fifo.Length - 2);
                minute = fifo.Substring(fifo.Length - 5, 2);
                hour = fifo.Substring(fifo.Length - 8, 2);
            }
            catch (Exception ex)
            {
                //mEventLog.WriteEntry("sqldatetime conversion error(" + fifo + "):" + ex.ToString(), EventLogEntryType.Error, 10);
               this.EventLogAdd("sqldatetime conversion error(" + fifo + "):" + ex.ToString(), LogType.error);

            }


            retVal = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;


            return retVal;
        }
        #endregion
    }
}
