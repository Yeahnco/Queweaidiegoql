using Controladores;
using MahApps.Metro.Controls;
using PersistenciaBD;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Lógica de interacción para VistaProfesional.xaml
    /// </summary>
    public partial class VistaProfesional : MetroWindow
    {
        public VistaProfesional()
        {
            InitializeComponent();
            Height = 500;
            Width = 1000;
            TabItemOpciones1.Visibility = Visibility.Collapsed;
            TabItemOpciones2.Visibility = Visibility.Collapsed;
            TabItemOpciones3.Visibility = Visibility.Collapsed;
            scrlViewerPrincipal.Visibility = Visibility.Visible;
            BtnCheckList.Visibility = Visibility.Collapsed;
            BtnCasoAsesoria.Visibility = Visibility.Collapsed;
            BtnCasoAsesoria.IsEnabled = false;
            BtnCheckList.IsEnabled = false;
        }
        public int idClienteSeleccionado;
        public int idProfesionalPerfilActual;
        public int idActividadSeleccionada;
        private void TileSalir_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new();
            this.Close();
            login.ShowDialog();
        }


        private void TabItemClientes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BtnCheckList.Visibility = Visibility.Collapsed;
            BtnCasoAsesoria.Visibility = Visibility.Collapsed;
            TabItemOpciones1.Header = "CLIENTE";
            TabItemOpciones2.Header = "ASESORIAS";
            TabItemOpciones3.Header = "SOLICITUDES";
            TabItemOpciones1.Visibility = Visibility.Visible;
            TabItemOpciones2.Visibility = Visibility.Visible;
            TabItemOpciones3.Visibility = Visibility.Visible;
            imgClientesPurpura.Visibility = Visibility.Visible;
            imgProfPurpura.Visibility = Visibility.Hidden;
            stckPanelTarjetas.Children.Clear();
        }
        private void TabitemProfesionales_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BtnCheckList.Visibility = Visibility.Collapsed;
            BtnCasoAsesoria.Visibility = Visibility.Collapsed;
            TabItemOpciones1.Header = "REVISIÓN";
            TabItemOpciones1.Visibility = Visibility.Visible;
            TabItemOpciones2.Visibility = Visibility.Collapsed;
            TabItemOpciones3.Visibility = Visibility.Collapsed;
            imgProfPurpura.Visibility = Visibility.Visible;
            imgClientesPurpura.Visibility = Visibility.Hidden;
            stckPanelTarjetas.Children.Clear();
        }
        public static ClienteTarjetaCompleta CrearTarjetaCliente(int idCliente)
        {
            ServiceActividad serviceActividad = new();
            ServiceActMejora serviceActMejora = new();
            ServiceCapacitacion serviceCapacitacion = new();
            ServiceAsesoria serviceAsesoria = new();
            ServiceCliente serviceCliente = new();
            ServiceGerente serviceGerente = new();
            ServiceProfesional serviceProfesional = new();

            try
            {
                int? idGerente = serviceGerente.GetEntity(idCliente).id_gerente;
                int? idProfesional = serviceCliente.GetEntity(idCliente).Profesional_id_prof;
                ClienteTarjetaCompleta clienteTarjetaCompleta = new()
                {
                    ReceiveIdCliente = serviceCliente.GetEntity(idCliente).id_emp,
                    ReceiveIdProfesional = serviceProfesional.GetEntity(idProfesional).id_prof,

                    DisplayEmpresa = serviceCliente.GetEntity(idCliente).Nombre_emp,
                    DisplayRutEmpresa = serviceCliente.GetEntity(idCliente).Rut_emp,
                    DisplayGerente = serviceGerente.GetEntity(idGerente).Nombre_gerente
                };
                string nombre = serviceProfesional.GetEntity(idProfesional).Nombre_prof;
                string apellido = serviceProfesional.GetEntity(idProfesional).Apellido_prof;
                string nombreProf = nombre + " " + apellido;
                clienteTarjetaCompleta.DisplayProfNombre = nombreProf;
                clienteTarjetaCompleta.DisplayMailGerente = serviceGerente.GetEntity(idGerente).Mail_gerente;
                clienteTarjetaCompleta.DisplayTelefonoEmpresa = serviceCliente.GetEntity(idCliente).Fono_cliente;
                clienteTarjetaCompleta.DisplayDireccion = serviceCliente.GetEntity(idCliente).Direccion_emp;
                clienteTarjetaCompleta.btnDescContrato.Visibility = Visibility.Hidden;
                //---------------------------------------------------------------------------------------------
                foreach (Actividad _actividades in serviceActividad.GetEntities())
                {
                    if (idCliente == _actividades.Cliente_id_emp
                        && _actividades.Fecha_act > DateTime.Now)
                    {
                        var tipoAct = _actividades.Tipo_actividad;
                        if (tipoAct == "Visita")
                        {
                            StackVisita stackVisita = new()
                            {
                                DisplayFechaVisita = _actividades.Fecha_act.ToString(),
                                DisplayHoraVisita = _actividades.Hora_act.ToString(),
                                idCliente = serviceCliente.GetEntity(idCliente).id_emp,
                                idProfesional = serviceProfesional.GetEntity(idProfesional).id_prof,
                                idVisita_Actividad = _actividades.id_act
                            };
                            clienteTarjetaCompleta.StckVisita.Children.Add(stackVisita);
                        }
                        if (tipoAct == "Capacitación")
                        {
                            StackCapacitacion stackCapacitacion = new()
                            {
                                DisplayNombreCapacitacion = serviceCapacitacion.GetEntity(_actividades.id_act).Nombre_cap,
                                DisplayFechaCapacitacion = _actividades.Fecha_act.ToString(),
                                DisplayHoraCapacitacion = _actividades.Hora_act.ToString()
                            };
                            clienteTarjetaCompleta.StckCapacitacion.Children.Add(stackCapacitacion);
                        }
                        if (tipoAct == "Actividad de mejora"
                            && serviceActMejora.GetEntity(_actividades.id_act).Estado_actividad != "Revisada")
                        {
                            StackActMejora stackActMejora = new()
                            {
                                DisplayActMejora = serviceActMejora.GetEntity(_actividades.id_act).Nombre_act_mejora
                            };
                            clienteTarjetaCompleta.StckActMejora.Children.Add(stackActMejora);
                        }
                        if (tipoAct == "Asesoría")
                        {
                            StackAsesoria stackAsesoria = new()
                            {
                                DisplayRazonAsesoria = serviceAsesoria.GetEntity(_actividades.id_act).Razon_ases,
                                DisplayFechaAsesoria = _actividades.Fecha_act.ToString(),
                                DisplayHoraAsesoria = _actividades.Hora_act.ToString(),
                                DisplayFechaIncidente = serviceAsesoria.GetEntity(_actividades.id_act).Fecha_incidente.ToString()
                            };
                            clienteTarjetaCompleta.StckAsesoria.Children.Add(stackAsesoria);
                        }
                    }
                }
                return clienteTarjetaCompleta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VistaProfesional/Metodo/ClienteTarjetaCompleta: ", "Se ha producido un error en la carga de los datos. \n" + ex.ToString());
                ClienteTarjetaCompleta clienteTarjetaCompleta_Empty = new();
                return clienteTarjetaCompleta_Empty;
            }
        }
        public void MostrarClientes()
        {
            ServiceCliente serviceCliente = new();
            try
            {
                foreach (Cliente _cliente in serviceCliente.GetEntities())
                {
                    if (_cliente.Profesional_id_prof == MainWindow.IdProfesional)
                    {
                        stckPanelTarjetas.Children.Add(CrearTarjetaCliente(_cliente.id_emp));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO_MostrarClientes: ", "Se ha producido un error insesperado. \n" + ex.ToString());
            }
        }
        public static TarjetaAsesoria CrearTarjetaAsesoria(int idActividad)
        {
            ServiceActividad serviceActividad = new();
            ServiceCliente serviceCliente = new();
            ServiceProfesional serviceProfesional = new();
            ServiceGerente serviceGerente = new();
            ServiceAsesoria serviceAsesoria = new();
            try
            {
                var idCliente = serviceActividad.GetEntity(idActividad).Cliente_id_emp;
                var idProfesional = serviceActividad.GetEntity(idActividad).Prof_id_profe;
                TarjetaAsesoria tarjetaAsesoria = new()
                {
                    DisplayNombreEmpresa = serviceCliente.GetEntity(idCliente).Nombre_emp,
                    DisplayRutEmpresa = serviceCliente.GetEntity(idCliente).Rut_emp,
                    DisplayNombreGerente = serviceGerente.GetEntity(idCliente).Nombre_gerente
                };
                string nombre = serviceProfesional.GetEntity(idProfesional).Nombre_prof;
                string apellido = serviceProfesional.GetEntity(idProfesional).Apellido_prof;
                string nombreProf = nombre + " " + apellido;
                tarjetaAsesoria.DisplayNombreProfesional = nombreProf;
                tarjetaAsesoria.DisplayRazon = serviceAsesoria.GetEntity(idActividad).Razon_ases;
                tarjetaAsesoria.DisplayCaso = serviceAsesoria.GetEntity(idActividad).Estado_caso;
                tarjetaAsesoria.DisplayDescripcionAsesoria = serviceAsesoria.GetEntity(idActividad).Descripcion;
                return tarjetaAsesoria;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VistaProfesional/Metodo/CrearTarjetaAsesoria: ", "Se ha producido un error en la carga de los datos. \n" + ex.Message);
                TarjetaAsesoria tarjetaAsesoria_Empty = new();
                return tarjetaAsesoria_Empty;
            }

        }
        public void MostrarAsesorias()
        {
            ServiceAsesoria serviceAsesoria = new();
            ServiceActividad serviceActividad = new();
            try
            {
                foreach (Actividad actividad in serviceActividad.GetEntities())
                {
                    Debug.WriteLine(idProfesionalPerfilActual);
                    if (actividad.Prof_id_profe == MainWindow.IdProfesional)
                    {
                        foreach (Asesoria asesorias in serviceAsesoria.GetEntities())
                        {
                            if (actividad.id_act == asesorias.Actividad_id_act)
                            {
                                stckPanelTarjetas.Children.Add(CrearTarjetaAsesoria(asesorias.Actividad_id_act));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO_MostrarAsesorias: ", "Se ha producido un error insesperado. \n" + ex.Message);
            }
        }
        public static TarjetaSolicitud CrearTarjetSolicitud(int idSolicitud)
        {

            ServiceSolicitud serviceSolicitud = new();
            ServiceCliente serviceCliente = new();
            ServiceGerente serviceGerente = new();
            ServiceProfesional serviceProfesional = new();
            try
            {
                var idGerente = serviceSolicitud.GetEntity(idSolicitud).Gerente_id_gerente;
                TarjetaSolicitud tarjetaSolicitud = new()
                {
                    idSolicitud = serviceSolicitud.GetEntity(idSolicitud).id_solicitud,
                    DisplayNombreEmpresa = serviceCliente.GetEntity(idGerente).Nombre_emp,
                    DisplayRutEmpresa = serviceCliente.GetEntity(idGerente).Rut_emp,
                    DisplayNombreGerente = serviceGerente.GetEntity(idGerente).Nombre_gerente
                };
                string nombre = serviceProfesional.GetEntity(MainWindow.IdProfesional).Nombre_prof;
                string apellido = serviceProfesional.GetEntity(MainWindow.IdProfesional).Apellido_prof;
                string nombreProf = nombre + " " + apellido;
                tarjetaSolicitud.DisplayNombreProfesional = nombreProf;
                tarjetaSolicitud.DisplayRazonSolicitud = serviceSolicitud.GetEntity(idSolicitud).Razon_soli;
                tarjetaSolicitud.DisplayDescripcionSolicitud = serviceSolicitud.GetEntity(idSolicitud).Descripcion;
                tarjetaSolicitud.DisplayFechaSolicitud = serviceSolicitud.GetEntity(idSolicitud).Fecha_CreacionSolicitud.ToString("dd/MM/yyyy");
                tarjetaSolicitud.DisplayHoraSolicitud = new DateTime(serviceSolicitud.GetEntity(idSolicitud).Hora_CreacionSolicitud.Ticks).ToString("HH:mm");
                tarjetaSolicitud.DisplayNombreSolicitud = serviceSolicitud.GetEntity(idSolicitud).Nombre_solicitud;
                return tarjetaSolicitud;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VistaProfesional/Metodo/CrearTarjetSolicitud: ", "Se ha producido un error en la carga de los datos. \n" + ex.Message);
                TarjetaSolicitud tarjetaSolicitud_Empty = new();
                return tarjetaSolicitud_Empty;
            }
        }
        public void MostrarSolicitudes()
        {
            ServiceSolicitud serviceSolicitud = new();
            try
            {
                Debug.WriteLine("id prof: " + MainWindow.IdProfesional);
                foreach (Solicitud _solicitudes in serviceSolicitud.GetEntities())
                {
                    if (_solicitudes.Profesional_id_prof == MainWindow.IdProfesional
                        && _solicitudes.Estado_solicitud == "Pendiente")
                    {
                        Debug.WriteLine("prof id prof: " +  _solicitudes.Profesional_id_prof);
                        stckPanelTarjetas.Children.Add(CrearTarjetSolicitud(_solicitudes.id_solicitud));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO_MostrarSolicitudes: ", "Se ha producido un error insesperado. \n" + ex.Message);
            }

        }
        public static TarjetaRevision CrearTarjetaRevision(int idActividad)
        {
            ServiceActMejora serviceActMejora = new();
            ServiceActividad serviceActividad = new();
            ServiceCliente serviceCliente = new();
            ServiceGerente serviceGerente = new();
            ServiceProfesional serviceProfesional = new();
            try
            {
                var idCliente = serviceActividad.GetEntity(idActividad).Cliente_id_emp;
                var idProfesional = serviceActividad.GetEntity(idActividad).Prof_id_profe;
                string nombre = serviceProfesional.GetEntity(idProfesional).Nombre_prof;
                string apellido = serviceProfesional.GetEntity(idProfesional).Apellido_prof;
                string nombreProf = nombre + " " + apellido;
                TarjetaRevision tarjetaRevision = new()
                {
                    idActividadMejora = idActividad,
                    idProf_Emisor = (int)serviceActMejora.GetEntity(idActividad).Prof_emisor_id,
                    DisplayNombreEmpresa = serviceCliente.GetEntity(idCliente).Nombre_emp,
                    DisplayRutEmpresa = serviceCliente.GetEntity(idCliente).Rut_emp,
                    DisplayNombreGerente = serviceGerente.GetEntity(idCliente).Nombre_gerente,
                    DisplayNombreProfesional = nombreProf,
                    DisplayNombreActMejora = serviceActMejora.GetEntity(idActividad).Nombre_act_mejora,
                    DisplayFechaActMejora = serviceActividad.GetEntity(idActividad).Fecha_act.ToString("dd/MM/yyyy"),
                    DisplayHoraActMejora = new DateTime(serviceActividad.GetEntity(idActividad).Hora_act.Ticks).ToString("HH:mm"),
                    DisplayDescripcionActMejora = serviceActMejora.GetEntity(idActividad).Descripcion_act_mejora.ToString(),
                    DisplayProfAsignado = serviceActMejora.GetEntity(idActividad).Estado_asignacion
                };
                tarjetaRevision.BloqueoTileTomar(MainWindow.IdProfesional, idProfesional);
                tarjetaRevision.ActividadTomada(serviceActMejora.GetEntity(idActividad).Estado_actividad);
                return tarjetaRevision;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VistaProfesional/Metodo/CrearTarjetaRevision: ", "Se ha producido un error insesperado. \n" + ex.Message);
                TarjetaRevision tarjetaRevision_Empty = new();
                return tarjetaRevision_Empty;
            }

        }
        public void MostrarRevisiones()
        {
            ServiceActMejora serviceActMejora = new();
            foreach (Act_de_mejora _acts_de_mejora in serviceActMejora.GetEntities())
            {
                if (_acts_de_mejora.Estado_actividad == "Revisada")
                {

                }
                else if (_acts_de_mejora.Estado_actividad == "Tomada"
                    || _acts_de_mejora.Estado_actividad == "Sin asignación")
                {
                    stckPanelTarjetas.Children.Add(CrearTarjetaRevision(_acts_de_mejora.Actividad_id_act));
                }
            }
        }
        private void TabItemOpciones1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TabItemOpciones1.Header.Equals("CLIENTE"))
            {
                BtnCheckList.Visibility = Visibility.Visible;
                BtnCasoAsesoria.Visibility = Visibility.Visible;
                stckPanelTarjetas.Children.Clear();
                MostrarClientes();
            }
            else if (TabItemOpciones1.Header.Equals("REVISIÓN"))
            {
                BtnCheckList.Visibility = Visibility.Collapsed;
                BtnCasoAsesoria.Visibility = Visibility.Collapsed;
                stckPanelTarjetas.Children.Clear();
                MostrarRevisiones();
            }
            else
            {
                MessageBox.Show("Work in progress");
            }
        }
        private void TabItemOpciones2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BtnCheckList.Visibility = Visibility.Collapsed;
            BtnCasoAsesoria.Visibility = Visibility.Collapsed;
            if (TabItemOpciones2.Header.Equals("ASESORIAS"))
            {
                stckPanelTarjetas.Children.Clear();
                MostrarAsesorias();
            }
            else
            {
                MessageBox.Show("Work in progress");
            }
        }
        private void TabItemOpciones3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BtnCheckList.Visibility = Visibility.Collapsed;
            BtnCasoAsesoria.Visibility = Visibility.Collapsed;
            if (TabItemOpciones3.Header.Equals("SOLICITUDES"))
            {
                stckPanelTarjetas.Children.Clear();
                MostrarSolicitudes();
            }
            else
            {
                MessageBox.Show("Work in progress");
            }
        }
        public void ActivarChecklist()
        {
            this.BtnCheckList.IsEnabled = true;
        }
        public void DesactivarChecklist()
        {
            this.BtnCheckList.IsEnabled = false;
        }
        private void BtnCheckList_Click(object sender, RoutedEventArgs e)
        {
            ServiceCliente serviceCliente = new();
            ServiceActividad serviceActividad = new();
            ServiceChecklist serviceChecklist = new();
            ServiceVisita serviceVisita = new();
            ServiceProfesional serviceProfesional = new();
            int? idChecklist = 0;
            if (serviceVisita.GetEntity(idActividadSeleccionada).Checklist_id_check == null)
            {
                VentanaNuevoChecklist ventanaNuevoChecklist = new();
                ventanaNuevoChecklist.idCliente = idClienteSeleccionado;
                ventanaNuevoChecklist.idProfesional = idProfesionalPerfilActual;
                ventanaNuevoChecklist.idActvidad_Seleccionada = idActividadSeleccionada;
                ventanaNuevoChecklist.DisplayNombreCliente = serviceCliente.GetEntity(idClienteSeleccionado).Nombre_emp;
                Actividad actividad = (serviceActividad.GetEntity(idActividadSeleccionada));
                ventanaNuevoChecklist.DisplayFechaVisita = actividad.Fecha_act.ToString("dd/MM/yyyy");
                ventanaNuevoChecklist.DisplayHoraVisita = actividad.Hora_act.ToString("HH:mm");
                ventanaNuevoChecklist.ShowDialog();
            }
            else
            {
                foreach (Visita _visita in serviceVisita.GetEntities())
                {
                    if (_visita.Actividad_id_act == idActividadSeleccionada
                        && _visita.Checklist_id_check != null)
                    {
                        idChecklist = serviceVisita.GetEntity(idActividadSeleccionada).Checklist_id_check;
                    }
                }
                VentanaChecklist ventanaChecklist = new();
                ventanaChecklist.lblNombreCliente.Content = serviceCliente.GetEntity(idClienteSeleccionado).Nombre_emp;
                ventanaChecklist.lblTipoAct.Content = serviceActividad.GetEntity(idActividadSeleccionada).Tipo_actividad;
                ventanaChecklist.lblFechaVisita.Content = serviceActividad.GetEntity(idActividadSeleccionada).Fecha_act.ToString("dd/MM/yyyy");
                ventanaChecklist.lblHoraVisita.Content = serviceActividad.GetEntity(idActividadSeleccionada).Hora_act.ToString("HH:mm");
                ventanaChecklist.lblContador.Content = serviceChecklist.GetEntity(idChecklist).Contador.ToString();
                ventanaChecklist.AgregarCheckbox((int)idChecklist);
                ventanaChecklist.ShowDialog();
            }
        }
    }
}