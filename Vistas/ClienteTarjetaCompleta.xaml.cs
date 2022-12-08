using Controladores;
using PersistenciaBD;
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
using System.Net;
using System.Net.Http;
using Path = System.IO.Path;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para ClienteTarjetaCompleta.xaml
    /// </summary>
    public partial class ClienteTarjetaCompleta : UserControl
    {
        public ClienteTarjetaCompleta()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public int ReceiveIdProfesional { get; set; }
        public int ReceiveIdCliente { get; set; }


        public string DisplayEmpresa { get; set; }
        public string DisplayRutEmpresa { get; set; }
        public string DisplayGerente { get; set; }
        public string DisplayProfNombre { get; set; }
        public string DisplayMailGerente { get; set; }
        public string DisplayTelefonoEmpresa { get; set; }
        public string DisplayDireccion { get; set; }
        public int idClient { get; set; }



        private void actualizarBD(string nombreTab, string nombreCol, string nuevoDato, int id)
        {
            using (BD_NMAEntities db = new BD_NMAEntities())
            {
                db.crudUpdate(nombreTab, nombreCol, nuevoDato, id);
                db.SaveChanges();
            }
        }

        ServiceGerente sg = new ServiceGerente();
        ServiceContrato sco = new ServiceContrato();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dockPanelMedio.Visibility == Visibility.Collapsed)
            {
                dockPanelMedio.Visibility = Visibility.Visible;
                ucVentana.Height.Equals(300);
            }
            else if (dockPanelMedio.Visibility == Visibility.Visible)
            {
                ucVentana.Height.Equals(72.4137931034483);
                dockPanelMedio.Visibility = Visibility.Collapsed;
            }
        }

        private void gridVentanaCompleta_Initialized(object sender, EventArgs e)
        {
            dockPanelMedio.Visibility = Visibility.Collapsed;
            ucVentana.Height.Equals(72.4137931034483);
            btnDescContrato.Visibility = Visibility.Visible;
        }

        private void gridGerente_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarGerente.Visibility = Visibility.Visible;
        }

        private void gridGerente_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarGerente.Visibility = Visibility.Hidden;
        }

        private void gridMail_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarMail.Visibility = Visibility.Visible;
        }

        private void gridMail_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarMail.Visibility = Visibility.Hidden;
        }

        private void gridTelefono_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarTelef.Visibility = Visibility.Visible;
        }

        private void gridTelefono_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarTelef.Visibility = Visibility.Hidden;
        }

        private void gridDireccion_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarDirecc.Visibility = Visibility.Visible;
        }

        private void gridDireccion_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarDirecc.Visibility = Visibility.Hidden;
        }

        private void txtblockNombreGerente_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbEditarGer.Visibility = Visibility.Visible;
            txbEditarGer.Text = txtblockNombreGerente.Text;
        }

        private void txbEditarGer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbEditarGer.Visibility == Visibility.Visible)
            {
                txbEditarGer.Visibility = Visibility.Hidden;
                actualizarBD("gerente", "Nombre_gerente", txbEditarGer.Text, sg.GetEntity(idClient).id_gerente);
            }
        }

        private void gridMail_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbEditarMail.Visibility = Visibility.Visible;
            txbEditarMail.Text = txtblockMailGerente.Text;
        }

        private void txbEditarMail_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbEditarMail.Visibility == Visibility.Visible)
            {
                txbEditarMail.Visibility = Visibility.Hidden;
                actualizarBD("gerente", "Mail_cliente", txbEditarMail.Text, sg.GetEntity(idClient).id_gerente);
            }
        }

        private void gridTelefono_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbEditarTelef.Visibility = Visibility.Visible;
            txbEditarTelef.Text = txtblockTelefonoEmpresa.Text;
        }

        private void txbEditarTelef_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbEditarTelef.Visibility == Visibility.Visible)
            {
                txbEditarTelef.Visibility = Visibility.Hidden;
                actualizarBD("cliente", "Fono_cliente", txbEditarTelef.Text.ToUpper(), sg.GetEntity(idClient).id_gerente);
            }
        }

        private void gridDireccion_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbEditarDirecc.Visibility = Visibility.Visible;
            txbEditarDirecc.Text = txtblockDirec.Text;
        }

        private void txbEditarDirecc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbEditarDirecc.Visibility == Visibility.Visible)
            {
                txbEditarDirecc.Visibility = Visibility.Hidden;
                actualizarBD("cliente", "Direccion_emp", txbEditarDirecc.Text, idClient);
            }
        }

        private void txtblockRutEmpresa_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbEditarRutEmp.Visibility = Visibility.Visible;
            txbEditarRutEmp.Text = txtblockRutEmpresa.Text;
        }

        private void txbEditarRutEmp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbEditarRutEmp.Visibility == Visibility.Visible)
            {
                txbEditarRutEmp.Visibility = Visibility.Hidden;
                actualizarBD("cliente", "Rut_emp", txbEditarRutEmp.Text.ToUpper(), idClient);
            }
        }

        private void lblEmpresa_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbEditarNombreEmp.Visibility = Visibility.Visible;
            txbEditarNombreEmp.Text = lblEmpresa.Text.ToString();
        }

        private void txbEditarNombreEmp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbEditarNombreEmp.Visibility == Visibility.Visible)
            {
                txbEditarNombreEmp.Visibility = Visibility.Hidden;
                actualizarBD("cliente", "Nombre_emp", txbEditarNombreEmp.Text.ToUpper(), idClient);
            }
        }

        private void lblEmpresa_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarNombreEmp.Visibility = Visibility.Visible;
        }

        private void lblEmpresa_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarNombreEmp.Visibility = Visibility.Hidden;
        }

        private void txtblockRutEmpresa_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarRutEmp.Visibility = Visibility.Visible;
        }

        private void txtblockRutEmpresa_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarRutEmp.Visibility = Visibility.Hidden;
        }


        private void btnDescContrato_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Path.GetTempPath();
                byte[] binaryData;
                binaryData = sco.GetEntity(sg.GetEntity(idClient).id_gerente).Archivo_pdf.ToArray();
                string nombreArc = sco.GetEntity(sg.GetEntity(idClient).id_gerente).Nombre_archivo;


                System.IO.File.WriteAllBytes(@path +"//"+ nombreArc, binaryData);
                MessageBox.Show("Se ha descargado el archivo " + nombreArc + " correctamente");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Se ha producido un error al descargar el archivo. \n" + ex.Message, "ERROR: ");
            }
                

        }

        private void BtnAgregarVisita_Click(object sender, RoutedEventArgs e)
        {
            VentanaNuevaVisita ventanaNuevaVisita = new();
            ventanaNuevaVisita.idCliente = ReceiveIdCliente;
            ventanaNuevaVisita.idProfesional = ReceiveIdProfesional;
            ventanaNuevaVisita.ShowDialog();
        }

        private void BtnAgregarCapacitacion_Click(object sender, RoutedEventArgs e)
        {
            VentanaNuevaCapacitacion ventanaNuevaCapacitacion = new();
            ventanaNuevaCapacitacion.idCliente = ReceiveIdCliente;
            ventanaNuevaCapacitacion.idProfesional = ReceiveIdProfesional;
            ventanaNuevaCapacitacion.ShowDialog();

        }

        private void BtnAgregarAsesoria_Click(object sender, RoutedEventArgs e)
        {
            VentanaNuevaAsesoria ventanaNuevaAsesoria = new();
            ventanaNuevaAsesoria.idCliente = ReceiveIdCliente;
            ventanaNuevaAsesoria.idProfesional = ReceiveIdProfesional;
            ventanaNuevaAsesoria.CargarRazonAsesorias(ReceiveIdProfesional, ReceiveIdCliente);
            ventanaNuevaAsesoria.ShowDialog();
        }

        private void BtnAgregarActividadMejora_Click(object sender, RoutedEventArgs e)
        {
            VentanaNuevaActMejora ventanaNuevaActMejora = new();
            ventanaNuevaActMejora.idCliente = ReceiveIdCliente;
            ventanaNuevaActMejora.idProfesional = ReceiveIdProfesional;
            ventanaNuevaActMejora.ShowDialog();
        }
    }
}
