using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery
{
    class DBConnectivity
    {
        //Getting connection to the MS Access database
        public static OleDbConnection GetConnection()
        {
            string connString;
            connString = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source= B:\Systems Programming\Lottery_Newest1_5_5\lotto.mdb"; //Database location
            return new OleDbConnection(connString);
        }
        public static void SaveNumber(string n, string n1, string n2, string n3, string n4, string n5, string dno, string am)
        {
            OleDbConnection myConnection = GetConnection();
            string myQuery = "INSERT INTO LottoTable(Name, No1, No2, No3, No4, No5,DrawNo, Amount) VALUES('" + n + "','" + n1 + "','" + n2 + "','" + n3 + "','" + n4 + "','" + n5 + "','" + dno + "','" + am + "')"; //Insert statement to add string name to the TeacherName column of YogaTeacher table
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

            try
            {
                myConnection.Open(); //Opening the connection
                myCommand.ExecuteNonQuery(); //Executing nonQuery
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
            }
            finally
            {
                myConnection.Close(); //Close the connection to database
            }
        }


        public static void LoadDraws(DataGridView datagridview, string dn)
        {
            
            OleDbConnection myConnection = GetConnection();

            try
            {
                myConnection.Open(); //Opening the connection
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = myConnection;
                string myQuery = "SELECT Name, No1, No2, No3, No4, No5 FROM LottoTable WHERE DrawNo = "+dn+"";
                myCommand.CommandText = myQuery;

                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                datagridview.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in DBHandler"+ex);
             
            }
            finally
            {
                myConnection.Close();
            }
        }

        public static string LoadlastDrawNo()
        {
            
            OleDbConnection myConnection = GetConnection();
            
            try
            {
                myConnection.Open(); //Opening the connection
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = myConnection;
                string myQuery = "SELECT MAX(DrawNo) AS LastDraw FROM LottoTable";
                myCommand.CommandText = myQuery;

                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                   string ldno = myReader["LastDraw"].ToString();
                   return ldno;
                }
                return null;   
            }
            catch (Exception ex)
            {

                MessageBox.Show("Exception in DBHandler"+ex);
                return null;
            }
            finally
            {
                myConnection.Close();
            }
            
        }
        public static double LoadPrice (int dn)
            {

            OleDbConnection myConnection = GetConnection();

            try
                {
                myConnection.Open(); //Opening the connection
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = myConnection;
                string myQuery = "SELECT SUM(Amount) AS TotalAmount FROM LottoTable WHERE DrawNo ="+dn+"";
                myCommand.CommandText = myQuery;

                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                    {
                    string total_str = myReader["TotalAmount"].ToString();
                    double price = int.Parse(total_str) * 0.5;
                    return price;
                    }
                return 0;
                }
            catch (Exception)
                {

                //MessageBox.Show("Exception in DBHandler" + ex);
                return 0;
                }
            finally
                {
                myConnection.Close();
                }

            }
        public static string LoadlWinner(int dn, string no1, string no2, string no3, string no4, string no5)
            {

            OleDbConnection myConnection = GetConnection();

            try
                {
                myConnection.Open(); //Opening the connection
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = myConnection;
                string myQuery = "SELECT Name AS Winner FROM Lottotable WHERE (((Lottotable.No1) = '" + no1 + "') AND((Lottotable.No2) = '" + no2 + "') AND((Lottotable.No3) = '" + no3 + "') AND((Lottotable.No4) = '" + no4 + "') AND((Lottotable.No5) = '" + no5 + "') AND((Lottotable.DrawNo) = " + dn + "))";
                myCommand.CommandText = myQuery;

                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                    {
                    string ldno = myReader["Winner"].ToString();
                    return ldno;
                    }
                return null;
                }
            catch (Exception ex)
                {

                MessageBox.Show("Exception in DBHandler" + ex);
                return null;
                }
            finally
                {
                myConnection.Close();
                }

            }

        }
}
