using Controladores;
using ControlzEx.Standard;
using PersistenciaBD;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para TarjetaProfesionales.xaml
    /// </summary>
    public partial class TarjetaProfesionales : UserControl
    {
        public TarjetaProfesionales()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string rutProf { get; set; }
        public string nombreProfesional { get; set; }
        public string apellidoProfesional { get; set; }
        public string telefonoProfesional { get; set; }
        public string direccProf { get; set; }
        public string mailProf { get; set; }
        public string nMejoras { get; set; }
        public string nAsesorias { get; set; }
        public string nCasos { get; set; }
        public string nCap { get; set; }
        public string nVisitas { get; set; }
        public string nClientes { get; set; }
        public int idProfe { get; set; }

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
            
            catch(Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al modificar la base de datos \n" + ex.Message);
            }
            
        }

        ServiceProfesional sp = new ServiceProfesional();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dockPanelMedio.Visibility == Visibility.Collapsed)
            {
                dockPanelMedio.Visibility = Visibility.Visible;
                ucVentanaProfesionales.Height.Equals(300);
                
                
            }
            else if (dockPanelMedio.Visibility == Visibility.Visible)
            {
                ucVentanaProfesionales.Height.Equals(72.4137931034483);
                dockPanelMedio.Visibility = Visibility.Collapsed;
                
            }
        }

        private void gridVentanaCompleta_Initialized(object sender, EventArgs e)
        {
            dockPanelMedio.Visibility = Visibility.Collapsed;
            ucVentanaProfesionales.Height.Equals(72.4137931034483);
        }

        private void txtblockRutProf_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarRut.Visibility = Visibility.Visible;
        }

        private void txtblockRutProf_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarRut.Visibility=Visibility.Hidden;
        }

        private void gridNombres_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarNombre.Visibility = Visibility.Visible;
        }

        private void gridNombres_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarNombre.Visibility=Visibility.Hidden;
        }

        private void gridApellidos_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarApellidos.Visibility = Visibility.Visible;
        }

        private void gridApellidos_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarApellidos.Visibility=Visibility.Hidden;
        }

        private void gridTelef_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarTelef.Visibility = Visibility.Visible;
        }

        private void gridTelef_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarTelef.Visibility = Visibility.Hidden;
        }

        private void gridDirec_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarDirec.Visibility = Visibility.Visible;
        }

        private void gridDirec_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarDirec.Visibility = Visibility.Hidden;
        }

        private void gridMail_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarMail.Visibility = Visibility.Visible;  
        }

        private void gridMail_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarMail.Visibility = Visibility.Hidden;
        }

        private void txtblockRutProf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbRut.Visibility = Visibility.Visible;
            txbRut.Text = txtblockRutProf.Text;
        }

        private void txbRut_KeyDown(object sender, KeyEventArgs e)
        {
            int cant = txbRut.Text.Length;
            txbRut.IsReadOnly = true;
            if (e.Key == Key.D1 | e.Key == Key.D2 | e.Key == Key.K | e.Key == Key.D3 | e.Key == Key.D4 | e.Key == Key.D5 | e.Key == Key.D6 | e.Key == Key.D7 | e.Key == Key.D8 | e.Key == Key.D9 | e.Key == Key.D0 | e.Key == Key.OemMinus | e.Key == Key.Delete | e.Key == Key.Enter)
            {
                txbRut.IsReadOnly = false;

            }

            ServiceProfesional sp = new ServiceProfesional();
            if (e.Key == Key.Enter & txbRut.Visibility == Visibility.Visible)
            {
                if (txbRut.Text.Contains("-" + 0) || txbRut.Text.Contains("-" + 1) || txbRut.Text.Contains("-" + 2) || txbRut.Text.Contains("-" + 3) || txbRut.Text.Contains("-" + 4) || txbRut.Text.Contains("-" + 5) || txbRut.Text.Contains("-" + 6) || txbRut.Text.Contains("-" + 6) || txbRut.Text.Contains("-" + 7) || txbRut.Text.Contains("-" + 8) || txbRut.Text.Contains("-" + 9) || txbRut.Text.Contains("-" + "k"))
                {
                    if (txbRut.Text.Length > 8)
                    {
                        int validacionRut = 0;

                        foreach (Profesional p in sp.GetEntities())
                        {
                            if (txbRut.Text.ToUpper().Contains(p.Rut_prof))
                            {
                                validacionRut++;
                            }
                        }

                        if (validacionRut > 0)
                        {
                            MessageBox.Show("Ya existe un Cliente con el rut: " + txbRut.Text.ToUpper() + " registrado en el sistema");
                        }

                        else
                        {
                            txbRut.Visibility = Visibility.Hidden;
                            actualizarBD("profesional", "Rut_prof", txbRut.Text.ToUpper(), sp.GetEntity(idProfe).id_prof);
                        }
                    }
                    else if (txbRut.Text.Length < 9 || txbRut.Text.Contains("kk") || txbRut.Text.Contains("k-k"))
                    {
                        MessageBox.Show("El formato del rut es incorrecto");
                    }

                }
                else
                {
                    MessageBox.Show("Debes ingresar el dígito verificador");
                }
            }
        }

        private void txtblockNombreProf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbNombres.Visibility = Visibility.Visible;
            txbNombres.Text = txtblockNombreProf.Text;
        }

        private void txbNombres_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbNombres.Visibility == Visibility.Visible)
            {
                txbNombres.Visibility = Visibility.Hidden;
                actualizarBD("profesional", "Nombre_prof", txbNombres.Text, sp.GetEntity(idProfe).id_prof);
            }
        }

        private void txtblockProfApellido_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbApellidos.Visibility = Visibility.Visible;
            txbApellidos.Text = txtblockProfApellido.Text;
        }

        private void txbApellidos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbApellidos.Visibility == Visibility.Visible)
            {
                txbApellidos.Visibility = Visibility.Hidden;
                actualizarBD("profesional", "Apellido_prof", txbApellidos.Text, sp.GetEntity(idProfe).id_prof);
            }
        }

        private void txtblockTfProf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbTelef.Visibility = Visibility.Visible;
            txbTelef.Text = txtblockTfProf.Text;
        }

        private void txbTelef_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbTelef.Visibility == Visibility.Visible)
            {
                txbTelef.Visibility = Visibility.Hidden;
                actualizarBD("profesional", "Telefono", txbTelef.Text, sp.GetEntity(idProfe).id_prof);
            }
        }

        private void txtblockDirecProf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbDirec.Visibility = Visibility.Visible;
            txbDirec.Text = txtblockDirecProf.Text;
        }

        private void txbDirec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbDirec.Visibility == Visibility.Visible)
            {
                txbDirec.Visibility = Visibility.Hidden;
                actualizarBD("profesional", "Direccion", txbDirec.Text, sp.GetEntity(idProfe).id_prof);
            }
        }

        private void txtblockMailProf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txbMail.Visibility = Visibility.Visible;
            txbMail.Text = txtblockMailProf.Text;
        }

        private void txbMail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & txbMail.Visibility == Visibility.Visible)
            {
                txbMail.Visibility = Visibility.Hidden;
                actualizarBD("profesional", "Mail_prof", txbMail.Text.ToLower(), sp.GetEntity(idProfe).id_prof);
            }
        }

        private void txtblockMailProf_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEditarMail.Visibility = Visibility.Visible;
        }

        private void txtblockMailProf_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEditarMail.Visibility = Visibility.Hidden;
        }

        private void txbRut_MouseLeave(object sender, MouseEventArgs e)
        {
            txbRut.Visibility = Visibility.Hidden;
        }
    }
}
