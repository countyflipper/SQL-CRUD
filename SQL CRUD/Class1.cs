using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQL_CRUD
{
    public class People
    {
        public int nID;
        public string Name;
        public string Country;

        public People() 
        {

      
    }

        //----------------------------------------------------------------------------				
        public bool PullFromReader(ref SqlDataReader Sqlreader)
        {
            bool bReturn = true;

            try
            {
                if (!Sqlreader.IsDBNull(Sqlreader.GetOrdinal("nID")))
                {
                    nID = Sqlreader.GetInt32(Sqlreader.GetOrdinal("nID"));
                }

                if (!Sqlreader.IsDBNull(Sqlreader.GetOrdinal("Name")))
                {
                    Name = Sqlreader.GetString(Sqlreader.GetOrdinal("Name")).Trim();
                }

                if (!Sqlreader.IsDBNull(Sqlreader.GetOrdinal("Country")))
                {
                    Country = Sqlreader.GetString(Sqlreader.GetOrdinal("Country")).Trim();
                }



                //Unsanitize();
            }
            catch (Exception xpt)
            {
                // MessageBox.Show("Problem reading user account information...", CDefines.MSG_PROBLEM_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                bReturn = false;
            }
            return bReturn;
        }
        //-----------------------------------------------------------------
    }
    public class Class1
    {
        public static string m_szConn = "Integrated Security=True;encrypt=false;Initial Catalog=DGE;Data Source=SQLEXPRESS;User ID=dt;pwd=PASSWORD;";

        People m_People = new People();
        protected void Create(object sender, EventArgs e, People pData)
        {

            string query = "INSERT INTO Customers VALUES(@Name, @Country)";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@Name", pData.Name);
                    cmd.Parameters.AddWithValue("@Country", pData.Country);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            
        }
        //----------------------------------------------------------------
        
        public virtual bool Read()
        {
            SqlConnection SqlConn = null;
            SqlCommand SqlCmd = null;
            string szQuery = "";
            bool bReturn = false;

            try
            {
                szQuery = string.Format("select * from {0};", 1);
                SqlConn = new SqlConnection(m_szConn);
                SqlCmd = new SqlCommand(szQuery);
                SqlCmd.Connection = SqlConn;
                SqlConn.Open();

                if (SqlConn.State == ConnectionState.Open)
                {
                    SqlDataReader Sqlreader = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (Sqlreader.Read())
                    {
                        m_People.PullFromReader(ref Sqlreader);
                        bReturn = true;
                    }
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
            }
            catch (SqlException x1)
            {
                //MessageBox.Show(x1.Message);
            }
            finally
            {
                if (SqlConn != null)
                {
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                        SqlConn.Dispose();
                    }
                }
            }
            return bReturn;
        }
        //---------------------------------------------------------------------------

        protected void Updating(object sender, People pData)
        {

            string query = "UPDATE Customers SET Name=@Name, Country=@Country WHERE CustomerId=@CustomerId";
            using (SqlConnection con = new SqlConnection(m_szConn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", pData.nID);
                    cmd.Parameters.AddWithValue("@Name", pData.Name);
                    cmd.Parameters.AddWithValue("@Country", pData.Country);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

        }
        //-------------------------------------------------------------

        protected void Deleting(object sendere, People pData)
        {
            string query = "DELETE FROM Customers WHERE CustomerId=@CustomerId";
            using (SqlConnection con = new SqlConnection(m_szConn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", pData.nID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }



    }


}
