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
using System.Runtime.InteropServices;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Controladores;
using PersistenciaBD;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        

        
        public MainWindow()
        {
            InitializeComponent();
        }

       

        ServiceUsuarios su = new ServiceUsuarios();

        public static int IdProfesional { get; set; }

        private async void ValidarLogin()
        {
            ServiceProfesional serviceProfesional = new();
            try
            {
                int tipo = 0;

                foreach (Usuarios u in su.GetEntities())
                {
                    if (usuarioLogin.Text.Equals(u.usuario) && contrasenaLogin.Password.Equals(u.clave) && u.idrol.Equals(1))
                    {
                        tipo++;
                    }
                    else if (usuarioLogin.Text.Equals(u.usuario) && contrasenaLogin.Password.Equals(u.clave) && u.idrol.Equals(2))
                    {
                        tipo = tipo + 2;
                        foreach (Profesional _profesional in serviceProfesional.GetEntities())
                        {
                            if (_profesional.id_prof == u.Profesional_id_prof)
                            {
                                IdProfesional = _profesional.id_prof;
                            }
                        }
                    }
                }
                if (tipo == 1)
                {
                    Administrador admini = new Administrador();
                    this.Close();
                    admini.ShowDialog();
                }
                else if (tipo == 2)
                {
                    VistaProfesional prof = new();
                    StackVisita.objetoVistaProfesionalExistente = prof;
                    this.Close();
                    var Nombreprof = serviceProfesional.GetEntity(IdProfesional).Nombre_prof;
                    var Apellidoprof = serviceProfesional.GetEntity(IdProfesional).Apellido_prof;
                    prof.lblNombrePerfil.Content = Nombreprof + " " + Apellidoprof;
                    prof.ShowDialog();
                }
                else if (usuarioLogin.Text.Equals("") && tipo == 0)
                {
                    await this.ShowMessageAsync("ERROR :", "Debe ingresar un usuario.");
                }
                else if (contrasenaLogin.Password.Equals("") && tipo == 0)
                {
                    await this.ShowMessageAsync("ERROR :", "Debe ingresar una contraseña.");
                }
                else if (tipo == 0)
                {
                    await this.ShowMessageAsync("ERROR :", "Usuario no encontrado o Contraseña incorrecta");
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al validar usuario. \n" + ex.Message);
            }
        }
        private void botonIngresar_Click(object sender, RoutedEventArgs e)
        {
            ValidarLogin();
        }
    }
}
