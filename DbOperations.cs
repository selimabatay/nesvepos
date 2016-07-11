using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using XFramework;
using static System.Windows.Forms.Control;


public class DbOperations
    {
        public static string ip = ".", database = "database", user = "sa", pass = "sa12345";
        public static SqlDbType GetSqlType(string type)
        {
            SqlDbType tp = new SqlDbType();
            if (type.Equals("string") || type.Equals("String"))
            {
                tp = SqlDbType.NVarChar;
            }
            else if (type.Equals("nvarchar") || type.Equals("Nvarchar"))
            {
                tp = SqlDbType.NVarChar;
            }
            else if (type.Equals("int") || type.Equals("Int"))
            {
                tp = SqlDbType.Int;
            }
            else if (type.Equals("double") || type.Equals("Double"))
            {
                tp = SqlDbType.Float;
            }
            else if (type.Equals("datetime") || type.Equals("Datetime"))
            {
                tp = SqlDbType.DateTime;
            }
            else if (type.Equals("varbinary") || type.Equals("Varbinary"))
            {
                tp = SqlDbType.VarBinary;
            }
            else
            {
                tp = SqlDbType.NVarChar;
            }
            return tp;
        }



        #region Connection String
        public static String GetConStr()
        {
            return string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", ip, database, user, pass);
        }
        #endregion

        #region Generate Sql Query
        public static DataTable GenerateSqlQuery(string sql)
        {
            var dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(GetConStr()))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    connection.Close();
                    var dp = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    dp.Fill(ds);
                    dt = ds.Tables[0];
                }
            }

            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return dt;
        }
        #endregion

        #region Generate Query String
        public static void GenerateQueryString(string sql)
        {
            try
            {
                using (var connection = new SqlConnection(GetConStr()))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Ekleme veya Güncelleme İşlemi
        public static void InsertAndUpdateOperations(Dictionary<string, SqlDbType> sqlTypes, Dictionary<string, string> valuesTypes, string sql, List<string> sqlValueList)
        {
            try
            {
                using (var connection = new SqlConnection(GetConStr()))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    for (int i = 0; i < sqlValueList.Count; i++)
                    {
                        command.Parameters.Add(sqlValueList[i], sqlTypes[sqlValueList[i]]).Value = valuesTypes[sqlValueList[i]];
                    }
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            catch (SqlException ex)
            {
                //
            }
        }
        #endregion

        public static bool Insert(ControlCollection controls)
        {
            bool result = false;
            try
            {

                Dictionary<string, SqlDbType> sqlTypes = new Dictionary<string, SqlDbType>();
                Dictionary<string, string> valuesTypes = new Dictionary<string, string>();
                List<string> sqlValueList = new List<string>();

                string values = "";
                string sql = "";
                sql += "INSERT INTO " + controls.Owner.Name + "(";

                foreach (Control item in controls)
                {
                    if (item is XTextEdit)
                    {

                        string tipi = ((XTextEdit)item)._sqlDataType;
                  
                        string fieldName = ((XTextEdit)item)._fieldName;

                        sqlTypes.Add("@" + fieldName, GetSqlType((tipi)));
                        valuesTypes.Add("@" + fieldName, ((XTextEdit)item).Text);
                        sqlValueList.Add("@" + fieldName);
                        values += "@" + fieldName + ",";
                        sql += "" + fieldName + ",";

                    }

                    if (item is XLookUpEdit)
                    {
                        if (!string.IsNullOrEmpty(((XLookUpEdit)item).Name))
                        {
                            string primaryField = ((XLookUpEdit)item)._primaryField;

                            string tipi = ((XLookUpEdit)item)._sqlDataType;

                            string fieldName = ((XLookUpEdit)item)._fieldName;

                            sqlTypes.Add("@" + fieldName, GetSqlType((tipi)));
                            valuesTypes.Add("@" + fieldName, ((XLookUpEdit)item).GetColumnValue(primaryField).ToString());
                            sqlValueList.Add("@" + fieldName);
                            values += "@" + fieldName + ",";
                            sql += "" + fieldName + ",";
                        }
                    }

                }
                values = values.Substring(0, values.Length - 1);
                sql = sql.Substring(0, sql.Length - 1);
                sql += ") VALUES(" + values + ")";

                InsertAndUpdateOperations(sqlTypes, valuesTypes, sql, sqlValueList);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool Update(int index, string valueName, ControlCollection controls)
        {

            try
            {

                Dictionary<string, SqlDbType> sqlTypes = new Dictionary<string, SqlDbType>();
                Dictionary<string, string> valuesTypes = new Dictionary<string, string>();
                List<string> sqlValueList = new List<string>();

                string values = "";
                string sql = "";
                sql += "UPDATE " + controls.Owner.Name + " SET ";
                foreach (Control item in controls)
                {
                    if (item is TextEdit)
                    {
                        string tipi = ((TextEdit)item).EditValue.GetType().ToString().Remove(0, 7);
                        string kolonadi = ((TextEdit)item).Name;


                        sqlTypes.Add("@" + kolonadi, GetSqlType((tipi)));
                        valuesTypes.Add("@" + kolonadi, ((TextEdit)item).Text);
                        sqlValueList.Add("@" + kolonadi);
                        values += "@" + kolonadi + ",";
                        sql += string.Concat(kolonadi, "=", "@", kolonadi, ",");
                    }
                }

                sqlValueList.Add("@" + valueName);
                sqlTypes.Add("@" + valueName, GetSqlType("nvarchar"));
                valuesTypes.Add("@" + valueName, index.ToString());
                values = values.Substring(0, values.Length - 1);
                sql = sql.Substring(0, sql.Length - 1);
                sql += " WHERE " + valueName + "=@" + valueName;

                InsertAndUpdateOperations(sqlTypes, valuesTypes, sql, sqlValueList);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static void Delete(int index, String valueName, string message, string tableName)
        {
            DialogResult dialog;
            try
            {
                string sql = "DELETE " + tableName;
                sql += " WHERE " + valueName + " = '" + index + "'";
                dialog = MessageBox.Show(message + "Onaylıyor Musunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.Yes)
                {
                    GenerateQueryString(sql);
                }

            }
            catch (Exception ex)
            {
            }
        }

        public static void GetRecord(int index, string valueName, ControlCollection controls)
        {
            try
            {
                string sql = string.Concat("SELECT * FROM ", controls.Owner.Name, " WHERE ", valueName, "=", "'", index, "'");


                BindingSource bs = new BindingSource();
                bs.DataSource = GenerateSqlQuery(sql);
                DataRow dr = ((DataRowView)bs.Current).Row;

                foreach (Control item in controls)
                {
                    if (item is TextEdit)
                    {
                        ((TextEdit)item).Text = dr[((TextEdit)item).Name].ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Exec Procedure
        public static void ExecuteProcedure(string procedure)
        {
            try
            {
                using (var connection = new SqlConnection(GetConStr()))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Exec Procedure
        public static DataTable ExecProcedure(string procedure)
        {
            var dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(GetConStr()))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    connection.Close();
                    var dp = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    dp.Fill(ds);
                    dt = ds.Tables[0];
                }
            }

            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return dt;
        }
        #endregion

        #region Exec Procedure 1 Parameter
        public static DataTable ExecProcedure1Parameter(string procedure, string pName, string parameter)
        {
            var dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(GetConStr()))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.Parameters.Add("@" + pName, SqlDbType.NVarChar).Value = parameter;
                    connection.Close();
                    var dp = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    dp.Fill(ds);
                    dt = ds.Tables[0];
                }
            }

            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return dt;
        }
        #endregion

        #region Exec Procedure 2 Parameter
        public static DataTable ExecProcedure2Parameter(string procedure, string pName, string parameter, string pName2, string parameter2)
        {
            var dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(GetConStr()))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.Parameters.Add("@" + pName, SqlDbType.NVarChar).Value = parameter;
                    command.Parameters.Add("@" + pName2, SqlDbType.NVarChar).Value = parameter2;
                    connection.Close();
                    var dp = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    dp.Fill(ds);
                    dt = ds.Tables[0];
                }
            }

            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return dt;
        }
        #endregion

    }
