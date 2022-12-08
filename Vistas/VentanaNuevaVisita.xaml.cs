using Controladores;
using MahApps.Metro.Controls;
using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Lógica de interacción para VentanaNuevaVisita.xaml
    /// </summary>
    public partial class VentanaNuevaVisita : MetroWindow
    {
        public VentanaNuevaVisita()
        {
            InitializeComponent();
            DatepickerVisita.DisplayDateStart = DateTime.Now.AddDays(2);
            TimepickerVisita.SourceHours = horas;
            TimepickerVisita.SourceMinutes = minutos;
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
            DatepickerVisita.Language = XmlLanguage.GetLanguage("es-CL");
        }
        public bool ValidarCampos()
        {
            if (DatepickerVisita.SelectedDate == null
                || TimepickerVisita.SelectedDateTime == null
                && DatepickerVisita.SelectedDate == null
                && TimepickerVisita.SelectedDateTime == null)
            {
                MessageBox.Show("Se encontraron campos vacíos en la ventana, favor de rellenar todos los campos.", "Validación de campos");
                return false;
            }
            else
            {
                return true;
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
                    && _actividad.Tipo_actividad == "Visita"
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
        private bool ValidadorDeVisitas(int idCliente)
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
                    if (_actividades.Tipo_actividad == "Visita"
                        && _actividades.Cliente_id_emp == idCliente
                        && _actividades.Fecha_act.Month == DateTime.Now.Month
                        && _actividades.Fecha_act.Year == DateTime.Now.Year)
                    {
                        contador++;
                    }
                }
                valorContador = contador;
                if (contador < 2)
                {
                    return true;
                }
                else if (contador >= 2)
                {
                    MessageBox.Show("Llegaste al limite de visitas");
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
                if (ValidadorDeVisitas(idCliente) == true)
                {
                    Debug.WriteLine("Podemos crear visitas");
                    if (Ultimo_valorContador(idCliente) <= 2)
                    {
                        Debug.WriteLine("Tienes menos de 2 visitas");
                        Debug.WriteLine(valorContador);
                        using BD_NMAEntities contextactividad = new();
                        contextactividad.CREATE_ACTIVIDAD
                            (
                                fecha_act: DatepickerVisita.SelectedDate,
                                hora_act: TimepickerVisita.SelectedDateTime,
                                contador: valorContador + 1,
                                prof_id_profe: idProfesional,
                                cliente_id_emp: idCliente,
                                tipo_actividad: "Visita",
                                estado: 0,
                                retraso: 0
                            );
                        contextactividad.SaveChanges();
                    }
                }
            }
        }
        public void CrearVisita(int idActividad)
        {
            using BD_NMAEntities contextvisita = new();
            contextvisita.CREATE_VISITA
                (
                    checklist_id_check: null,
                    visitaActividad_id_act: idActividad,
                    test: null
                );
            contextvisita.SaveChanges();
        }
        private void TileGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("La información a continuación será ingresada. \n¿Esta seguro(a) que la información ingresada es correcta?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (ValidarCampos() == true
                    && ValidadorDeVisitas(idCliente) == true)
                {
                    CrearActividad(idProfesional, idCliente);
                    CrearVisita(Ultimo_idActividad());
                    MessageBox.Show("Visita creada correctamente.");
                    this.Close();
                }
                else if (ValidarCampos() == false
                    && ValidadorDeVisitas(idCliente) == false)
                {
                    MessageBox.Show("No se pudo crear la visita.");
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
