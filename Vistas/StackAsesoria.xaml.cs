﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para StackAsesoria.xaml
    /// </summary>
    public partial class StackAsesoria : UserControl
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public StackAsesoria()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            InitializeComponent();
            DataContext = this;
            FormatoCalendario();
        }
        public string DisplayRazonAsesoria { get; set; }
        public string DisplayFechaIncidente { get; set; }
        public string DisplayFechaAsesoria { get; set; }
        public string DisplayHoraAsesoria { get; set; }
        public void FormatoCalendario()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-CL");
            datePickerFechaAsesoria.Language = XmlLanguage.GetLanguage("es-CL");
            datePickerFechaIncidente.Language = XmlLanguage.GetLanguage("es-CL");
        }

        public void ToggleBtnAsesoria_Checked(object sender, RoutedEventArgs e)
        {
        }
    }
}
