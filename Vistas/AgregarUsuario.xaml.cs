using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using Controladores;
using ControlzEx.Standard;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PersistenciaBD;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para AgregarUsuario.xaml
    /// </summary>
    public partial class AgregarUsuario : MetroWindow
    {
        public AgregarUsuario()
        {
            InitializeComponent();
        }

        private void gridAgregarUsuario_Initialized(object sender, EventArgs e)
        {

        }
    }
}
