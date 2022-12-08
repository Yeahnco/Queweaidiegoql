using Controladores;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para VentanaChecklist.xaml
    /// </summary>
    public partial class VentanaChecklist : MetroWindow
    {
        public VentanaChecklist()
        {
            InitializeComponent();
        }
        public string DisplayProfesionalAsignado;
        public string DisplayHoraActividad;
        public string DisplayTipoAct;
        public string DisplayFechaVisita;
        public string DisplayContadorChecklist;

        public void AgregarCheckbox(int idChecklistExistente)
        {
            ServiceChecklist serviceChecklist = new();
            char[] delimiterChars = { ',', '.', ':', '\t', ';' };
            try
            {
                string[] Aspectos = serviceChecklist.GetEntity(idChecklistExistente).Aspectos.Split(delimiterChars);
                int contador = 0;
                foreach (string Aspecto in Aspectos)
                {
                    contador++;
                    CheckBox checkBox = new()
                    {
                        Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                        Content = Aspecto
                    };
                    StackPanelChecklist.Children.Add(checkBox);
                }
                StackPanelChecklist.Children.RemoveAt(contador - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VentanaChecklist/Metodo/AgregarCheckbox: ", "Se ha producido un error en la carga de los aspectos del checklist. " + ex.Message);
            }


        }
        private void TileGuardar_Click(object sender, RoutedEventArgs e)
        {
        }
        private void TileAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
