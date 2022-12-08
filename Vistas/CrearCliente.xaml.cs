using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using Controladores;
using ControlzEx.Standard;
using PersistenciaBD;
using Microsoft.Win32;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para CrearCliente.xaml
    /// </summary>
    public partial class CrearCliente : UserControl
    {
        public CrearCliente()
        {
            InitializeComponent();
        }

        OpenFileDialog openFile = new OpenFileDialog();
        private async void crearCliente(string Rut_emp, string Nombre_emp, string Direccion_emp, int Profesional_id_prof, string Fono_cliente)
        {
            try
            {
                using (BD_NMAEntities db = new BD_NMAEntities())
                {
                    db.CREATE_CLIENTE(Rut_emp, Nombre_emp, Direccion_emp, Profesional_id_prof, Fono_cliente);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al actualizar la Base de Datos. \n" + ex.Message);
            }
        }

        private async void crearGerente(string Nombre_gerente, string Mail_gerente, int? Contrato_id_contrato, int Cliente_id_clien)
        {
            try
            {
                using (BD_NMAEntities db = new BD_NMAEntities())
                {
                    db.CREATE_GERENTE(Nombre_gerente, Mail_gerente, Contrato_id_contrato, Cliente_id_clien);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al actualizar la Base de Datos. \n" + ex.Message);
            }
        }

        private async void crearContrato(DateTime Vencimiento_cont, byte[] Archivo_pdf, int Gerente_id_gerente, string nombreArc, int plan)
        {
            try
            {
                using (BD_NMAEntities db = new BD_NMAEntities())
                {
                    db.CREATE_CONTRATO(Vencimiento_cont, Archivo_pdf, Gerente_id_gerente, nombreArc, plan);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al actualizar la Base de Datos. \n" + ex.Message);
            }
        }

        private async void crearUsuario(string usuario, string clave, int rol, int? Profesional_id_prof, int? Cliente_id_emp, int? Administrador_id_adm)
        {
            try
            {
                using (BD_NMAEntities db = new BD_NMAEntities())
                {
                    db.CREATE_USUARIO(usuario, clave, rol, Profesional_id_prof, Cliente_id_emp, Administrador_id_adm);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al crear usuario en la Base de Datos. \n" + ex.Message);
            }

        }

        private async void crearPago(string Estado_pago, string Mes_pago, int Valor_extra, int Total_a_pagar, int Plan_id_plan, int idContrato)
        {
            try
            {
                using (BD_NMAEntities db = new BD_NMAEntities())
                {
                    db.CREATE_PAGO(Estado_pago, Mes_pago, Valor_extra, Total_a_pagar, Plan_id_plan, idContrato);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al crear usuario en la Base de Datos. \n" + ex.Message);
            }

        }


        private void cbProfesional_Initialized(object sender, EventArgs e)
        {
            ServiceProfesional sp = new ServiceProfesional();
            cbProfesional.ItemsSource = sp.GetEntities();

        }

        private void cbPlan_Initialized(object sender, EventArgs e)
        {
            try
            {
                List<string> nombrePlanes = new List<string>();
                nombrePlanes.Add("BASICO");
                nombrePlanes.Add("FULL");
                nombrePlanes.Add("OP");
                cbPlan.ItemsSource = nombrePlanes;
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR: ", "Se ha producido un error al cargar planes usuario \n" + ex.Message);
            }

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceCliente sc = new ServiceCliente();
                ServiceContrato sco = new ServiceContrato();
                ServiceGerente sg = new ServiceGerente();
                int contadorCli = 0;
                int contadorCo = 0;
                int contadorG = 0;
                byte[] a = Encoding.ASCII.GetBytes("aaaasda");

                if (string.IsNullOrEmpty(txbContraseña.Text) || string.IsNullOrEmpty(txbContrato.Text) || string.IsNullOrEmpty(txbMailGer.Text) || string.IsNullOrEmpty(txbDirecEmp.Text) || string.IsNullOrEmpty(txbNombreEmp.Text) || string.IsNullOrEmpty(txbNombreUs.Text) || string.IsNullOrEmpty(txbNomGer.Text) || string.IsNullOrEmpty(txbRutEmp.Text) || string.IsNullOrEmpty(txbTlfEmp.Text) || string.IsNullOrEmpty(cbPlan.Text) || string.IsNullOrEmpty(cbProfesional.Text))
                {
                    MessageBox.Show("Debes rellenar todos los campos");
                }

                else
                {
                    if (txbRutEmp.Text.Contains("-" + 0) || txbRutEmp.Text.Contains("-" + 1) || txbRutEmp.Text.Contains("-" + 2) || txbRutEmp.Text.Contains("-" + 3) || txbRutEmp.Text.Contains("-" + 4) || txbRutEmp.Text.Contains("-" + 5) || txbRutEmp.Text.Contains("-" + 6) || txbRutEmp.Text.Contains("-" + 6) || txbRutEmp.Text.Contains("-" + 7) || txbRutEmp.Text.Contains("-" + 8) || txbRutEmp.Text.Contains("-" + 9) || txbRutEmp.Text.Contains("-" + "k"))
                    {
                        if (txbRutEmp.Text.Length > 8)
                        {
                            ServiceUsuarios su = new ServiceUsuarios();
                            int validacionUs = 0;
                            int validacionRut = 0;

                            foreach(Usuarios u in su.GetEntities())
                            {
                                if (txbNombreUs.Text.Contains(u.usuario))
                                {
                                    validacionUs++;
                                }
                            }

                            foreach(Cliente c in sc.GetEntities())
                            {
                                if (txbRutEmp.Text.ToUpper().Contains(c.Rut_emp))
                                {
                                    validacionRut++;
                                }
                            }

                            if (validacionRut > 0)
                            {
                                MessageBox.Show("Ya existe un Cliente con el rut: " + txbRutEmp.Text.ToUpper() + " registrado en el sistema");
                            }

                            else if (validacionUs > 0)
                            {
                                MessageBox.Show("El nombre de usuario: " + txbNombreUs.Text + ", No está disponible");
                            }

                            else
                            {
                                if (txbTlfEmp.Text.Contains("+569"))
                                {
                                    if (txbMailGer.Text.Contains("@") & txbMailGer.Text.Contains("."))
                                    {
                                        crearCliente(txbRutEmp.Text, txbNombreEmp.Text, txbDirecEmp.Text, Convert.ToInt32                       (cbProfesional.SelectedValue.ToString()), txbTlfEmp.Text);

                                        foreach (Cliente c in sc.GetEntities())
                                        {

                                            contadorCli = c.id_emp;

                                        }

                                        crearGerente(txbNomGer.Text, txbMailGer.Text, null, contadorCli);

                                        foreach (Gerente g in sg.GetEntities())
                                        {

                                            contadorG = g.id_gerente;

                                        }

                                        byte[] file = null;
                                        Stream myStream = openFile.OpenFile();
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            myStream.CopyTo(ms);
                                            file = ms.ToArray();
                                        }

                                        ServicePlan spl = new ServicePlan();
                                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es_ES");
                                        int tipoPlan = 0;
                                        if (cbPlan.Text.Equals("BASICO"))
                                        {
                                            tipoPlan = 1;
                                        }
                                        else if (cbPlan.Text.Equals("OP"))
                                        {
                                            tipoPlan = 2;
                                        }
                                        if (cbPlan.Text.Equals("FULL"))
                                        {
                                            tipoPlan = 3;
                                        }

                                        crearContrato(Convert.ToDateTime("11-11-11"), file, contadorG, openFile.SafeFileName, tipoPlan);

                                        foreach (Contrato co in sco.GetEntities())
                                        {

                                            contadorCo = co.id_contrato;

                                        }

                                        crearUsuario(txbNombreUs.Text, txbContraseña.Text, 3, null, contadorCli, null);



                                        string mes = "";
                                        if (DateTime.Today.Month == 1)
                                        {
                                            mes = "Enero";
                                        }
                                        if (DateTime.Today.Month == 2)
                                        {
                                            mes = "Febrero";
                                        }
                                        if (DateTime.Today.Month == 3)
                                        {
                                            mes = "Marzo";
                                        }
                                        if (DateTime.Today.Month == 4)
                                        {
                                            mes = "Abril";
                                        }
                                        if (DateTime.Today.Month == 5)
                                        {
                                            mes = "Mayo";
                                        }
                                        if (DateTime.Today.Month == 6)
                                        {
                                            mes = "Junio";
                                        }
                                        if (DateTime.Today.Month == 7)
                                        {
                                            mes = "Julio";
                                        }
                                        if (DateTime.Today.Month == 8)
                                        {
                                            mes = "Agosto";
                                        }
                                        if (DateTime.Today.Month == 9)
                                        {
                                            mes = "Septiembre";
                                        }
                                        if (DateTime.Today.Month == 10)
                                        {
                                            mes = "Octubre";
                                        }
                                        if (DateTime.Today.Month == 11)
                                        {
                                            mes = "Noviembre";
                                        }
                                        if (DateTime.Today.Month == 12)
                                        {
                                            mes = "Diciembre";
                                        }


                                        Debug.WriteLine("TIPO PLAN, VALOR PLAN:  " + tipoPlan, " ", Convert.ToInt32(spl.GetEntity(tipoPlan).Valor_plan));

                                        crearPago("PENDIENTE", mes, 0, Convert.ToInt32(spl.GetEntity(tipoPlan).Valor_plan), tipoPlan, contadorCo);




                                        MessageBox.Show("CLIENTE CREADO");
                                        Window.GetWindow(this).Close();
                                        var parent = Application.Current.Windows.OfType<Administrador>().FirstOrDefault();
                                        if (parent != null)
                                        {
                                            Administrador adm = new Administrador();
                                            adm.Show();
                                            parent.Close();

                                        }
                                    }

                                    else
                                    {
                                        MessageBox.Show("El mail ingresado no es válido.");
                                    }

                                }

                                else
                                {
                                    MessageBox.Show("Debes ingresar un número de teléfono válido.");
                                }
                            }
                        }
                        else if (txbRutEmp.Text.Length < 9 || txbRutEmp.Text.Contains("kk") || txbRutEmp.Text.Contains("k-k"))
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

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "ERROR: ", "Se ha producido un error al crear usuario \n");
            }




        }


        private void btnContrato_Click(object sender, RoutedEventArgs e)
        {
            openFile.Filter = "Todos los archivos (*.*)|*.*";
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == true)
            {
                txbContrato.Text = openFile.FileName;
            }


        }

        private void txbRutEmp_KeyDown(object sender, KeyEventArgs e)
        {
            int cant = txbRutEmp.Text.Length;
            txbRutEmp.IsReadOnly = true;
            if (e.Key == Key.D1 | e.Key == Key.D2 | e.Key == Key.K | e.Key == Key.D3 | e.Key == Key.D4 | e.Key == Key.D5 | e.Key == Key.D6 | e.Key == Key.D7 | e.Key == Key.D8 | e.Key == Key.D9 | e.Key == Key.D0 | e.Key == Key.OemMinus | e.Key == Key.Delete)
            {
                txbRutEmp.IsReadOnly = false;

            }
        }

        private void txbNomGer_KeyDown(object sender, KeyEventArgs e)
        {
            txbNomGer.IsReadOnly = true;
            if ((e.Key >= Key.A && e.Key <= Key.Z) | e.Key == Key.OemMinus) { txbNomGer.IsReadOnly = false; }
        }

        private void txbTlfEmp_KeyDown(object sender, KeyEventArgs e)
        {
            txbTlfEmp.IsReadOnly = true;
            if (e.Key == Key.D1 | e.Key == Key.D2 | e.Key == Key.D3 | e.Key == Key.D4 | e.Key == Key.D5 | e.Key == Key.D6 | e.Key == Key.D7 | e.Key == Key.D8 | e.Key == Key.D9 | e.Key == Key.D0 | e.Key == Key.OemMinus | e.Key == Key.OemPlus) { txbTlfEmp.IsReadOnly = false; }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
