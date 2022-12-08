using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para TarjetaActividades.xaml
    /// </summary>
    public partial class TarjetaActividades : UserControl
    {
        public TarjetaActividades()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        public int idAct { get; set; }
        public string nombreEmp { get; set; }
        public string fechaAct { get; set; }
        public string nombreGer { get; set; }
        public string nombreProf { get; set; }  
        public string horaAct {get; set; }
        public string lblAct { get; set; }
        public int? estadoAct { get; set; }
        public int? retraso { get; set; }
        public DateTime horaReal { get; set; }
        public DateTime fechaReal { get; set; }


        private void actualizarBD(string nombreTab, string nombreCol, string nuevoDato, int id)
        {
            try
            {
                using (BD_NMAEntities db = new BD_NMAEntities())
                {
                    db.crudUpdate(nombreTab, nombreCol, nuevoDato, id);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al modificar la base de datos \n" + ex.Message);
            }

        }

        private void btnAlerta_Click(object sender, RoutedEventArgs e)
        {
            if (retraso == 0)
            {
                actualizarBD("Actividad", "Retraso", "1", idAct);
            }

            else if (retraso == 1)
            {
                actualizarBD("Actividad", "Retraso", "0", idAct);
            }

            Administrador admi = new Administrador();
            Window.GetWindow(this).Close();
            admi.Show();
        }

        private void btnToggle_Click(object sender, RoutedEventArgs e)
        {
            if (imgTgNeg.Visibility == Visibility.Visible & estadoAct == 0)
            {
                imgTgNeg.Visibility = Visibility.Hidden;
                imgTgPurp.Visibility = Visibility.Visible;
                actualizarBD("Actividad", "Estado", "2", idAct);
            }

            else if (imgTgNeg.Visibility == Visibility.Visible & estadoAct == 1)
            {
                imgTgNeg.Visibility = Visibility.Hidden;
                imgTgPurp.Visibility = Visibility.Visible;
                actualizarBD("Actividad", "Estado", "2", idAct);
            }
            else if (imgTgPurp.Visibility == Visibility.Visible & estadoAct == 0)
            {
                imgTgNeg.Visibility = Visibility.Visible;
                imgTgPurp.Visibility = Visibility.Hidden;
                actualizarBD("Actividad", "Estado", "1", idAct);
            }
            else if (imgTgPurp.Visibility == Visibility.Visible & estadoAct == 2)
            {
                imgTgNeg.Visibility = Visibility.Visible;
                imgTgPurp.Visibility = Visibility.Hidden;
                actualizarBD("Actividad", "Estado", "1", idAct);
            }

            Administrador admi = new Administrador();
            Window.GetWindow(this).Close();
            admi.Show();
        }

        private void gridTarjAct_Initialized(object sender, EventArgs e)
        {
            //if (lblActividad.Content.ToString().ToUpper().Equals("ALERTA"))
            //{
            //    headerAlerta.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    headerAlerta.Visibility = Visibility.Hidden;
            //}
        }
    }
}
