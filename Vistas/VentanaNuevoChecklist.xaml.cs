using Controladores;
using MahApps.Metro.Controls;
using PersistenciaBD;
using System;
using System.Collections;
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
    /// Lógica de interacción para VentanaNuevoChecklist.xaml
    /// </summary>
    public partial class VentanaNuevoChecklist : MetroWindow
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public VentanaNuevoChecklist()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            InitializeComponent();
            DataContext = this;
            DatePickerFechaVisitaAuto.DisplayDateStart = DateTime.Now;
            FormatoCalendario();
        }
        public int idCliente;
        public int idProfesional;
        public int idActvidad_Seleccionada;
        public string DisplayNombreCliente { get; set; }
        public string DisplayFechaVisita { get; set; }
        public string DisplayHoraVisita { get; set; }
        public List<TextBox> listaItems = new List<TextBox>();
        public List<int> listaInt = new List<int>();
        public List<string> listaTextItem = new List<string>();
        public void FormatoCalendario()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-CL");
            DatePickerFechaVisitaAuto.Language = XmlLanguage.GetLanguage("es-CL");
        }
        public bool ValidarCampos(List<TextBox> listaTextBoxObj)
        {
            TextBox data;
            string datalast;
            try
            {
                int j = listaTextBoxObj.Count();
                for (int i = 0; i < j; i++)
                {
                    data = listaTextBoxObj[i];
                    datalast = listaTextBoxObj.Last().Text;
                    if (data.Text == "")
                    {
                        MessageBox.Show("Se encontraron campos vacíos en la ventana o la lista está vacía, favor asegurarse de crear una lista y rellenar todos los campos correspondientes.", "Validación de campos");
                        return false;
                    }
                    else if (datalast == "")
                    {
                        MessageBox.Show("Se encontraron campos vacíos en la ventana o la lista está vacía, favor asegurarse de crear una lista y rellenar todos los campos correspondientes.", "Validación de campos");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VentanaNuevoChecklist/Metodo/ValidarCampos: ", "Se ha producido un error en la validación de los datos. \n" + ex.Message);
                return false;
            }

        }
        private TextBox CreateItems(List<TextBox> listaTextBoxObj)
        {
            TextBox txtboxItem_creado = new()
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                BorderThickness = new Thickness(0, 0, 0, 2)
            };
            this.StckPanelChecklist.Children.Add(txtboxItem_creado);
            listaTextBoxObj.Add(txtboxItem_creado);
            return txtboxItem_creado;
        }
        private List<TextBox> DeleteItems(List<TextBox> listaTextBoxObj)
        {
            if (listaTextBoxObj.Count != 0)
            {
                var ultimoObj = listaTextBoxObj.Last();
                this.StckPanelChecklist.Children.Remove(ultimoObj);
                listaTextBoxObj.Remove(ultimoObj);
            }
            else if (listaTextBoxObj.Count == 0)
            {
                MessageBox.Show("La lista esta vacía.");
            }
            return listaTextBoxObj;
        }
        private string leerTextBoxList(List<TextBox> listaTextBoxObj)
        {
            string data;
            string alldata = "";
            foreach (TextBox elements in listaTextBoxObj)
            {
                data = elements.Text;
                alldata = alldata + data + ";";
            }
            return alldata;
        }
        private void TileAgregarItem_Click(object sender, RoutedEventArgs e)
        {
            CreateItems(listaItems);
        }
        private void TileQuitarItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteItems(listaItems);
        }
        public static int Ultimo_idChecklist()
        {
            ServiceChecklist serviceChecklist = new();
            int ultimoID = 0;
            try
            {
                foreach (Checklist _checklist in serviceChecklist.GetEntities())
                {
                    if (ultimoID < _checklist.id_checklist)
                    {
                        ultimoID = _checklist.id_checklist;
                    }
                }
                return ultimoID;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VentanaNuevoChecklist/Metodo/Ultimo_idChecklist: ", "Se ha producido un error, imposible obtener el último idChecklist. \n" + ex.Message);
                return -1;
            }
        }
        public void CrearChecklist()
        {
            try
            {
                using BD_NMAEntities contextChecklist = new();
                contextChecklist.CREATE_CHECKLIST
                    (
                        aspectos: leerTextBoxList(listaItems),
                        reporte: "Reporte pendiente",
                        contador: 0
                    );
                contextChecklist.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VentanaNuevoChecklist/Metodo/CrearChecklist: ", "Se ha producido un error al crear el nuevo checklist. \n" + ex.Message);
            }

        }
        public void ActualizarVisita(int idChecklist, int idActividad)
        {
            try
            {
                using BD_NMAEntities contextVisita = new();
                contextVisita.crudUpdate
                    (
                        nombreTabla: "Visita",
                        nombreColumna: "Checklist_id_check",
                        nuevoDato: idChecklist.ToString(),
                        id: idActividad
                    );
                contextVisita.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR_MODULO:VentanaNuevoChecklist/Metodo/ActualizarVisita: ", "Se ha producido un error al actualizar la visita. \n" + ex.Message);
            }
        }
        private void TileGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("La información a continuación será ingresada. \n¿Esta seguro(a) que la información ingresada es correcta?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (ValidarCampos(listaItems) == true)
                {
                    CrearChecklist();
                    ActualizarVisita(Ultimo_idChecklist(), idActvidad_Seleccionada);
                    MessageBox.Show("Checklist creado correctamente.");
                    this.Close();
                }
                else if (ValidarCampos(listaItems) == false)
                {
                    MessageBox.Show("No se pudo crear el checklist.");
                }
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }
        }
        private void TileAtras_Click(object sender, RoutedEventArgs e)
        {
            //ValidarCampos(listaItems);
            this.Close();
        }
    }
}
