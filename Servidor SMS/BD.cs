using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Servidor_SMS
{
    internal class BD
    {
        #region variables
        private string SetValuesUpdate { get; set; }
        private string Attributes { get; set; }
        private string IdAttribute { get; set; }
        private string Json { get; set; }
        private string TEST { get; set; }

        //RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        //int os = (int)Environment.OSVersion.Platform;
        //string parametrosConexion = "Data Source = /home/pi/Dropbox-Uploader/MiPVG/dbmiPV.db; Version = 3; New = False; Compress = True;";//ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        private string parametrosConexion;// = "URI = file:dbmiPV.db; Version = 3; New = False; Compress = True;";
        private SQLiteConnection conexionBD; // SQLiteConnection conexionBD;
        private SQLiteCommand queryCommand; //SQLiteCommand queryCommand;
        public SQLiteDataAdapter adapter; //public SQLiteDataAdapter adapter;
        public DataTable dt;
        private DataSet ds;

        #endregion

        #region abrir y cerrar conexion a la base de datos
        public bool abrirConexion()
        {
            if ((int)Environment.OSVersion.Platform == 2)
            {
                //System.Windows.Forms.MessageBox.Show("entre a la version test");
                parametrosConexion = "URI = file:baseCETI.db; Version = 3; New = False; Compress = True;";
            }
            else if ((int)Environment.OSVersion.Platform == 4)
            {
                //try
                //{
                //    File.ReadAllText("/home/pi/Dropbox-Uploader/MiPVGTest/test.json");

                //ruta de la base de datos en versión TEST
                //System.Windows.Forms.MessageBox.Show("entre a la version test");
                //    parametrosConexion = "Data Source = /home/pi/Dropbox-Uploader/MiPVGTest/dbmiPV.db; Version = 3; New = False; Compress = True;";
                //}
                //catch(FileNotFoundException) 
                //{
                //ruta de la base de datos en varsión RELEASE
                //System.Windows.Forms.MessageBox.Show("entre a la version release");
                parametrosConexion = "Data Source = /home/pi/Dropbox-Uploader/MiPVG/dbmiPV.db; Version = 3; New = False; Compress = True;";
                //}
            }

            conexionBD = new SQLiteConnection(parametrosConexion);
            ConnectionState estadoConexion;

            try
            {
                conexionBD.Open();
                estadoConexion = conexionBD.State;
                return true;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }
        private void cerrarConexion()
        {
            if (conexionBD.State == ConnectionState.Open)
            {
                conexionBD.Close();
            }
        }
        #endregion

        #region metodos
        public void attributes(string table)
        {
            bool flag = false;
            Attributes = "";

            abrirConexion();
            try
            {
                queryCommand = new SQLiteCommand($"pragma table_info({table})", conexionBD);
                SQLiteDataReader dr = queryCommand.ExecuteReader();

                while (dr.Read())
                {
                    if (flag)
                    {
                        Attributes += $"{dr["name"]}, ";
                    }
                    else
                    {
                        /*Nota importante para mi yo del futuro
                        este algoritmo serpara la columna con de la llave primaria
                        del resto de los atributos, para usarla a conveniencia de maneras separadas*/
                        flag = true;
                        IdAttribute = $"{dr["name"]},";
                    }
                }

                Attributes = Attributes.Trim().TrimEnd(',');
            }
            catch (System.Exception e)
            {
                System.Windows.Forms.MessageBox.Show($"{e}");
            }
            cerrarConexion();
        }

        public void setValuesUpdate(string values)
        {
            //se inicializa para poder realizar varias actualizaciones 
            SetValuesUpdate = "";

            string[] Attributes;
            string[] _Values;

            int items = 0;

            Attributes = this.Attributes.Split(',');// explode(",", self::$_attributes);
            _Values = values.Split(',');

            items = Attributes.Length;// count($attributes);

            for (int i = 0; i < items; i++)
            {
                SetValuesUpdate += $"{Attributes[i]} = {_Values[i]}, ";
            }

            SetValuesUpdate = SetValuesUpdate.Trim().TrimEnd(',');
        }

        public bool execute(string table, string values, string action, string where)
        {
            attributes(table);

            switch (action)
            {
                case "insert":
                    return insert($"INSERT INTO {table} ({Attributes}) VALUES ({values})");

                case "multiinsert":
                    return insert($"INSERT INTO {table} ({Attributes}) VALUES {values}");

                case "query":
                    return select($"SELECT {IdAttribute}{Attributes} FROM {table} {where}", table);

                case "update":
                    setValuesUpdate(values);
                    return insert($"UPDATE {table} SET {SetValuesUpdate} {where}");

                case "delete":
                    return insert($"DELETE FROM {table} {where}");

                case "free":
                    return select($"{where}", table);

                case "freeUpdate":
                    return insert($"{where}");

                default:
                    return false; // code...
            }
        }

        #region consulta
        public bool select(string queryConsulta, string table)
        {
            abrirConexion();

            ds = new DataSet();
            ds.Reset();

            adapter = new SQLiteDataAdapter(queryConsulta, conexionBD);

            try
            {
                adapter.Fill(ds, table);

                cerrarConexion();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Json = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    dt = ds.Tables[0];
                    //Json = $"[{JsonConvert.SerializeObject(ds, Formatting.Indented)}]";
                    Mensaje.getMessage(messageResponse.actionSuccess.ToString());
                    Mensaje.result_bool = true;
                    return true;
                }
                else
                {
                    Mensaje.getMessage(messageResponse.actionFail.ToString());
                    Mensaje.result_bool = false;
                    Json = null;
                    return false;
                }
            }
            catch (SQLiteException e)
            {
                Mensaje.getMessage(messageResponse.actionFail.ToString());
                Mensaje.result_bool = false;
                return false;
            }
        }
        #endregion

        #region ejecuta la instruccion sql insertar y update
        public bool insert(string query)
        {
            int filasAfectadas = 0;
            abrirConexion();

            queryCommand = new SQLiteCommand(query, conexionBD);

            filasAfectadas = queryCommand.ExecuteNonQuery();

            cerrarConexion();

            if (filasAfectadas > 0)
            {
                Mensaje.getMessage(messageResponse.actionSuccess.ToString());
                return true;
            }
            else
            {
                Mensaje.getMessage(messageResponse.actionFail.ToString());
                return false;
            }
        }

        public void alter(string query)
        {
            int filasAfectadas = 0;
            abrirConexion();

            queryCommand = new SQLiteCommand(query, conexionBD);

            try
            {
                filasAfectadas = queryCommand.ExecuteNonQuery();

                cerrarConexion();
            }
            catch (Exception e) { }
        }

        public string maxId(string tabla, string atributo)
        {
            //insert();
            return $"SELECT * FROM {tabla} ORDER BY {atributo} DESC LIMIT 1";

            //$"{SELECT} * {FROM} {tabla} {WHERE} {atributo} {igual} " +
            //$"{abrir_parentesis} {SELECT} {MAX_ID}{abrir_parentesis}{atributo}{cerrar_parentesis} {FROM} {tabla}";
        }
        #endregion

        #region Adaptador -> carga los datos de una consulta
        public SQLiteDataAdapter consultaAdaptador(string query)
        {
            abrirConexion();
            adapter = new SQLiteDataAdapter(query, conexionBD);
            cerrarConexion();
            return adapter;
        }
        #endregion

        public List<T> list<T>()
        {
            if (Json != "false" && Json != null)
            {
                return JsonConvert.DeserializeObject<List<T>>(Json);
            }
            return null;
        }

        public DataTable dataTable()
        {
            if (Json != "false")
            {
                dt = new DataTable();
                dt = (DataTable)JsonConvert.DeserializeObject(Json, typeof(DataTable));

                return dt;
            }
            else
            {
                return null;
            }
        }

        #region llena un listBox con una consulta sql
        public void llenarListBox(System.Windows.Forms.ListBox llenarListBox, string query, string columna, string tabla, ref int entra)
        {
            /*ds = new DataSet();
            ds.Reset();
            adapter = consultaAdaptador(query);
            adapter.Fill(ds, tabla);
            entra = 1;
            llenarListBox.DataSource = ds.Tables[tabla];
            llenarListBox.DisplayMember = columna;
            llenarListBox.SelectedIndex = -1;
            entra = 0;*/
        }

        public void llenarCombobox(System.Windows.Forms.ComboBox llenarListBox, string query, string columna, string tabla, ref int entra)
        {
            /*ds = new DataSet();
            ds.Reset();
            adapter = consultaAdaptador(query);
            adapter.Fill(ds, tabla);
            entra = 1;
            llenarListBox.DataSource = ds.Tables[tabla];
            llenarListBox.DisplayMember = columna;
            llenarListBox.SelectedIndex = -1;
            entra = 0;*/
        }
        #endregion

        #region llena un dataGridView con una consulta sql
        public void llenarDgv(System.Windows.Forms.DataGridView dgvLlenar, string query, string tabla)
        {
            ds = new DataSet();
            ds.Reset();

            adapter = consultaAdaptador(query);
            adapter.Fill(ds, tabla);

            dgvLlenar.DataSource = ds.Tables[0];
        }
        #endregion
        #endregion
    }
}
