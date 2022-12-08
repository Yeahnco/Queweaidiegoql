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
    /// Lógica de interacción para TarjetaAsesoria.xaml
    /// </summary>
    public partial class TarjetaAsesoria : UserControl
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public TarjetaAsesoria()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            InitializeComponent();
            DataContext = this;
        }
        public string DisplayNombreEmpresa { get; set; }
        public string DisplayRutEmpresa { get; set; }
        public string DisplayNombreGerente { get; set; }
        public string DisplayNombreProfesional { get; set; }
        public string DisplayRazon { get; set; }
        public string DisplayCaso { get; set; }
        public string DisplayDescripcionAsesoria { get; set; }
        public string DisplayNombreAsesoria { get; set; }

        private void GridPrincipal_Initialized(object sender, EventArgs e)
        {
            dockPanelMedio.Visibility = Visibility.Collapsed;
            ucTarjetaAsesoria.Height.Equals(97.1134020618556);
        }

        private void BtnUpdDown_Click(object sender, RoutedEventArgs e)
        {
            if (dockPanelMedio.Visibility == Visibility.Collapsed)
            {
                dockPanelMedio.Visibility = Visibility.Visible;
                ucTarjetaAsesoria.Height.Equals(315);
            }
            else if (dockPanelMedio.Visibility == Visibility.Visible)
            {
                ucTarjetaAsesoria.Height.Equals(97.1134020618556);
                dockPanelMedio.Visibility = Visibility.Collapsed;
            }
        }
    }
}
