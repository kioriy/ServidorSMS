using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servidor_SMS
{
    public partial class frmMain : Form
    {
        Alumno alumno = new Alumno();
        Sms sms = new Sms();

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnRunServer_Click(object sender, EventArgs e)
        {
            string entrada_salida = (Convert.ToInt32(DateTime.Now.ToString("HH")) < 12) ? "entrada" : "salida";
            string hora;// = (Convert.ToInt32(DateTime.Now.ToString("HH")) < 12) ? "entrada" : "salida";

            alumno.free("SELECT " +
                "A.nombre_alumno, " +
                "A.telefono, ES.Fecha, " +
                "ES.hora_entrada, " +
                "ES.hora_salida " +
                "FROM ALUMNO AS A " +
                "INNER JOIN Entradas_salidas AS ES ON A.id_alumnos = ES.fk_id_alumnos " +
                $"WHERE ES.Fecha = \"{DateTime.Now.ToString("dd/MM/yyyy")} \"");

            int i = 0;

            foreach (var a in alumno.list<Alumno>())
            {
                if (!a.telefonoIsEmpty(a.telefono))
                {
                    hora = (Convert.ToInt32(DateTime.Now.ToString("HH")) < 12) ? a.hora_entrada : a.hora_salida;

                    sms.enviar(a.telefono, $"El alumno {a.nombre_alumno} registro su {entrada_salida}" /*{hora}"*/);
                    Thread.Sleep(5000);
                }
                i++;
            }
        }
    }
}
