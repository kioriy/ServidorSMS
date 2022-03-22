using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor_SMS
{
    internal class Alumno : BD
    {
        public string nombre_alumno { get; set; }
        public string telefono { get; set; }
        public string fecha { get; set; }
        public string hora_entrada { get; set; }
        public string hora_salida { get; set; }

        public string table { get; set; }

        public Alumno()
        {
            table = "ALUMNO";
        }

        public void insert()
        {
            execute(table, values(), action.insert.ToString(), "");
        }

        public void update(string where)
        {
            execute(table, values(), action.update.ToString(), where);
        }

        public void query(string where)
        {
            execute(table, "", action.query.ToString(), where);
        }

        public void free(string query)
        {
            execute(table, "", action.free.ToString(), query);
        }

        public void delete(string where)
        {
            execute(table, "", action.delete.ToString(), where);
        }

        public bool telefonoIsEmpty(string telefonos)
        {
            if (telefonos != null && telefonos != "")
            {
                string[] _telefonos = telefono.Split('-');
                this.telefono = _telefonos[0];
                return false;
            }
            else
            {
                return true;
            }
        }

        public string values()
        {
            return
                $"\"{nombre_alumno.Trim()}\"," +
                $"\"{telefono}\"," +
                $"\"{fecha}\"," +
                $"\"{hora_entrada.ToUpper().Trim()}\"," +
                $"\"{hora_salida}\"";
        }
    }
}
