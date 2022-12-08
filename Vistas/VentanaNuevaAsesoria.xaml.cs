using Controladores;
using MahApps.Metro.Controls;
using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para VentanaNuevaAsesoria.xaml
    /// </summary>
    public partial class VentanaNuevaAsesoria : MetroWindow
    {
        public VentanaNuevaAsesoria()
        {
            InitializeComponent();
            datePickerFechaAsesoria.DisplayDateStart = DateTime.Now.AddDays(2);
            timePickerHoraAsesoria.SourceHours = horas;
            timePickerHoraAsesoria.SourceMinutes = minutos;
            FormatoCalendario();
        }
        public int idCliente;
        public int idProfesional;
        int[] horas = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
        int[] minutos = { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
        public void FormatoCalendario()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-CL");
            datePickerFechaAsesoria.Language = XmlLanguage.GetLanguage("es-CL");
        }
        public bool ValidarCampos()
        {
            if (ComboBoxAsesorias.Text == " "
                || datePickerFechaAsesoria.SelectedDate == null
                || timePickerHoraAsesoria.SelectedDateTime == null
                && ComboBoxAsesorias.Text == " "
                && datePickerFechaAsesoria.SelectedDate == null
                && timePickerHoraAsesoria.SelectedDateTime == null)
            {
                MessageBox.Show("Se encontraron campos vacíos en la ventana o no se ha seleccionado una asesoría, favor de rellenar todos los campos y seleccionar una asesoría.", "Validación de campos");
                return false;
            }
            else
            {
                return true;
            }
        }
        public string DisplayHoraCreacionSolicitud { get; set; }
        public string DisplayDescripcionCaso { get; set; }
        public void CargarRazonAsesorias(int idProfesional, int idCliente)
        {
            ServiceSolicitud serviceSolicitud = new();
            foreach (Solicitud _solicitud in serviceSolicitud.GetEntities())
            {
                if (_solicitud.Profesional_id_prof == idProfesional
                    && _solicitud.Gerente_id_gerente == idCliente
                    && _solicitud.Estado_solicitud == "Aceptada")
                {
                    ComboBoxItem comboBoxItem = new();
                    comboBoxItem.Content = _solicitud.Nombre_solicitud;
                    ComboBoxAsesorias.Items.Add(comboBoxItem);
                }
            }
        }
        private void ComboBoxAsesorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string HeaderCombobox = ComboBoxAsesorias.SelectedItem.ToString().Split(':')[1].Trim();
            ServiceSolicitud serviceSolicitud = new();
            foreach (Solicitud _solicitud in serviceSolicitud.GetEntities())
            {
                if (HeaderCombobox == _solicitud.Nombre_solicitud)
                {
                    TxtBoxDescripcionCaso.Text = _solicitud.Descripcion;
                    var date = _solicitud.Fecha_CreacionSolicitud;
                    var formattedDate = date.ToString("dd/MM/yyyy");
                    var finalDate = Convert.ToDateTime(formattedDate);
                    datePickerFechaCreacionSolicitud.SelectedDate = finalDate;
                    var datetime = _solicitud.Hora_CreacionSolicitud;
                    var hora = datetime.TimeOfDay.Hours;
                    var minuto = datetime.TimeOfDay.Minutes;
                    timePickerHoraCreacionSolicitud.SelectedDateTime = datetime;
                }
            }
        }
        public static int Ultimo_idActividad()
        {
            ServiceActividad serviceActividad = new();
            int ultimoID = 0;
            foreach (Actividad _actividad in serviceActividad.GetEntities())
            {
                if (ultimoID < _actividad.id_act)
                {
                    ultimoID = _actividad.id_act;
                }
            }
            return ultimoID;
        }
        public static int Ultimo_valorContador(int idCliente)
        {
            ServiceActividad serviceActividad = new();
            int ultimoContador = 0;
            foreach (Actividad _actividad in serviceActividad.GetEntities())
            {
                if (ultimoContador < _actividad.Contador
                    && idCliente == _actividad.Cliente_id_emp
                    && _actividad.Tipo_actividad == "Asesoria"
                    && _actividad.Fecha_act.Month == DateTime.Now.Month
                    && _actividad.Fecha_act.Year == DateTime.Now.Year)
                {
                    ultimoContador = _actividad.Contador;
                }
            }
            Debug.WriteLine(ultimoContador);
            return ultimoContador;
        }
        private bool ValidadorDeActividades()
        {
            int contadorActividades = 0;
            ServiceActividad serviceActividad = new();
            contadorActividades = serviceActividad.GetEntities().Count;
            if (contadorActividades >= 0)
            {
                return true;
            }
            return false;
        }
        int valorContador = 0;
        private bool ValidadorDeAsesorias(int idCliente)
        {
            ServiceActividad serviceActividad = new();
            int actividades = serviceActividad.GetEntities().Count;
            if (actividades == 0)
            {
                return true;
            }
            else if (actividades > 0)
            {
                int contador = 0;
                foreach (Actividad _actividades in serviceActividad.GetEntities())
                {
                    if (_actividades.Tipo_actividad == "Asesoría"
                        && _actividades.Cliente_id_emp == idCliente
                        && _actividades.Fecha_act.Month == DateTime.Now.Month
                        && _actividades.Fecha_act.Year == DateTime.Now.Year)
                    {
                        contador++;
                    }
                }
                valorContador = contador;
                if (contador < 10)
                {
                    return true;
                }
                else if (contador >= 10)
                {
                    MessageBox.Show("Llegaste al limite de asesorías");
                    this.Close();
                    return false;
                }
            }
            return false;
        }
        public void CrearActividad(int idProfesional, int idCliente)
        {
            ServiceActividad serviceActividad = new();
            if (ValidadorDeActividades() == false)
            {
                MessageBox.Show("ERROR: Algo salió mal al leer las actividades");
            }
            else if (ValidadorDeActividades() == true)
            {
                Debug.WriteLine("Podemos crear actividades");
                if (ValidadorDeAsesorias(idCliente) == true)
                {
                    Debug.WriteLine("Podemos crear visitas");
                    if (Ultimo_valorContador(idCliente) <= 10)
                    {
                        Debug.WriteLine("Tienes menos de 10 asesorías");
                        Debug.WriteLine(valorContador);
                        using BD_NMAEntities contextActividad = new();
                        contextActividad.CREATE_ACTIVIDAD
                            (
                                fecha_act: datePickerFechaAsesoria.SelectedDate,
                                hora_act: timePickerHoraAsesoria.SelectedDateTime,
                                contador: valorContador + 1,
                                prof_id_profe: idProfesional,
                                cliente_id_emp: idCliente,
                                tipo_actividad: "Asesoría",
                                estado: 0,
                                retraso: 0
                            );
                        contextActividad.SaveChanges();
                    }
                }
            }

        }
        public void CrearAsesoria(int idActividad)
        {
            var nombre = ComboBoxAsesorias.SelectedItem.ToString().Split(':')[1].Trim();
            int idSolicitud;
            ServiceSolicitud serviceSolicitud = new();
            ServiceProfesional serviceProfesional = new();
            foreach (Solicitud _solicitud in serviceSolicitud.GetEntities())
            {
                if (_solicitud.Nombre_solicitud == nombre) //Filtro de creación
                {
                    idSolicitud = _solicitud.id_solicitud;
                    using BD_NMAEntities contextAsesoria = new();
                    contextAsesoria.CREATE_ASESORIA
                        (
                            razon_ases: nombre,
                            estado_caso: "Abierto",
                            diligencia: TxtBoxDiligencia.Text,
                            evento_ases: "null",
                            solicitud_id_solicitud: idSolicitud,
                            asesoriaActividad_id_act: idActividad,
                            fecha_incidente: datePickerFechaCreacionSolicitud.SelectedDate,
                            descripcion: TxtBoxDescripcionCaso.Text
                        );
                    contextAsesoria.SaveChanges();
                    using BD_NMAEntities contextActualizar = new();
                    var nombreProf = serviceProfesional.GetEntity(idProfesional).Nombre_prof;
                    var apellidoProf = serviceProfesional.GetEntity(idProfesional).Apellido_prof;
                    var fullNombreProf = nombreProf + " " + apellidoProf;
                    contextActualizar.crudUpdate
                        (
                            nombreTabla: "Solicitud",
                            nombreColumna: "Estado_solicitud",
                            nuevoDato: "Tomada por: " + fullNombreProf,
                            id: idSolicitud
                        );
                    contextActualizar.SaveChanges();
                }
            }
        }
        private void TileGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("La información a continuación será ingresada. \n¿Esta seguro(a) que la información ingresada es correcta?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (ValidarCampos() == true
                    && ValidadorDeAsesorias(idCliente) == true)
                {
                    CrearActividad(idProfesional, idCliente);
                    CrearAsesoria(Ultimo_idActividad());
                    MessageBox.Show("Asesoría creada correctamente.");
                    this.Close();
                }
                else if (ValidarCampos() == false
                    && ValidadorDeAsesorias(idCliente) == false)
                {
                    MessageBox.Show("No se pudo crear la asesoría.");
                }

            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }
        }
        private void TileAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
