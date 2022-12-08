using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
using static Vistas.Administrador;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para Administrador.xaml
    /// </summary>
    public partial class Administrador : MetroWindow
    {




        //////////////////////// Variables Globales //////////////////////////
        int n = 0;
        ServiceCliente sc = new ServiceCliente();
        ServiceVisita sv = new ServiceVisita();
        ServiceActividad sa = new ServiceActividad();
        ServiceProfesional sp = new ServiceProfesional();
        ServiceGerente sg = new ServiceGerente();
        ServiceContrato sco = new ServiceContrato();
        ServicePlan spl = new ServicePlan();
        ServicePago spa = new ServicePago();
        ServiceSolicitud sso = new ServiceSolicitud();

        public class Pagos
        {
            public string Estado { get; set; }
            public string Mes { get; set; }
            public string Plan { get; set; }
            public int ValorPlan { get; set; }
            public int ValorExtra { get; set; }
            public int Total { get; set; }
            public int idPago { get; set; }
            public int idCliente { get; set; }
            public int? idComprobante { get; set; }
        }




        //////////////////////// Variables Globales //////////////////////////
        public Administrador()
        {
            InitializeComponent();
        }

        public int idVentana;

        int aa = 0;
        
        private async void actualizarBD(string nombreTab, string nombreCol, string nuevoDato, int id)
        {
            try
            {
                using (BD_NMAEntities db = new BD_NMAEntities())
                {
                    db.crudUpdate(nombreTab, nombreCol, nuevoDato, id);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al actualizar la Base de Datos. \n" + ex.Message);
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
        //////////////////////////////////////////////// METODOS CREADOS //////////////////////////////////////////////////////////////

        private TarjetaActividades CrearTarjetaActividades(int idAct, int idCli, int idProf)
        {
            TarjetaActividades tarjetaActividades = new TarjetaActividades();

            tarjetaActividades.idAct = idAct;
            tarjetaActividades.nombreEmp = sc.GetEntity(idCli).Nombre_emp;
            tarjetaActividades.fechaAct = sa.GetEntity(idAct).Fecha_act.ToShortDateString();
            tarjetaActividades.nombreGer = sg.GetEntity(idCli).Nombre_gerente;
            tarjetaActividades.nombreProf = sp.GetEntity(idProf).Nombre_prof + ' ' + sp.GetEntity(idProf).Apellido_prof;
            tarjetaActividades.horaAct = new DateTime(sa.GetEntity(idAct).Hora_act.Ticks).ToString("hh:mm tt").ToUpper();
            tarjetaActividades.lblAct = sa.GetEntity(idAct).Tipo_actividad.ToUpper();
            tarjetaActividades.estadoAct = sa.GetEntity(idAct).Estado;
            tarjetaActividades.retraso = sa.GetEntity(idAct).Retraso;

            if(tarjetaActividades.lblAct == "Accidente")
            {
                tarjetaActividades.headerAlerta.Visibility = Visibility.Visible;
            }

            tarjetaActividades.fechaReal = sa.GetEntity(idAct).Fecha_act;  

            var nowTime = DateTime.Parse(tarjetaActividades.horaAct);

            if (tarjetaActividades.fechaAct == DateTime.Today.ToShortDateString())
            {
                if (nowTime < DateTime.Now)
                {
                    int a = 1;
                    int b = 2;
                    int c = 0;

                    if (tarjetaActividades.estadoAct == a)
                    {
                        tarjetaActividades.imgTgNeg.Visibility = Visibility.Visible;
                        tarjetaActividades.imgTgPurp.Visibility = Visibility.Hidden;
                    }
                    else if (tarjetaActividades.estadoAct == b)
                    {
                        tarjetaActividades.imgTgNeg.Visibility = Visibility.Hidden;
                        tarjetaActividades.imgTgPurp.Visibility = Visibility.Visible;
                    }
                    else if (tarjetaActividades.estadoAct == c)
                    {
                        tarjetaActividades.imgTgNeg.Visibility = Visibility.Hidden;
                        tarjetaActividades.imgTgPurp.Visibility = Visibility.Visible;
                    }
                }
                else if (nowTime > DateTime.Now)
                {
                    int a = 1;
                    int b = 2;
                    int c = 0;

                    if (tarjetaActividades.estadoAct == a)
                    {
                        tarjetaActividades.imgTgNeg.Visibility = Visibility.Visible;
                        tarjetaActividades.imgTgPurp.Visibility = Visibility.Hidden;
                    }
                    else if (tarjetaActividades.estadoAct == b)
                    {
                        tarjetaActividades.imgTgNeg.Visibility = Visibility.Hidden;
                        tarjetaActividades.imgTgPurp.Visibility = Visibility.Visible;
                    }
                    else if (tarjetaActividades.estadoAct == c)
                    {
                        tarjetaActividades.imgTgNeg.Visibility = Visibility.Visible;
                        tarjetaActividades.imgTgPurp.Visibility = Visibility.Hidden;
                    }
                }
            }

            else if (tarjetaActividades.fechaReal > DateTime.Today)
            {
                int a = 1;
                int b = 2;
                int c = 0;

                if (tarjetaActividades.estadoAct == a)
                {
                    tarjetaActividades.imgTgNeg.Visibility = Visibility.Visible;
                    tarjetaActividades.imgTgPurp.Visibility = Visibility.Hidden;
                }
                else if (tarjetaActividades.estadoAct == b)
                {
                    tarjetaActividades.imgTgNeg.Visibility = Visibility.Hidden;
                    tarjetaActividades.imgTgPurp.Visibility = Visibility.Visible;
                }
                else if (tarjetaActividades.estadoAct == c)
                {
                    tarjetaActividades.imgTgNeg.Visibility = Visibility.Visible;
                    tarjetaActividades.imgTgPurp.Visibility = Visibility.Hidden;
                }
            }
            else if (tarjetaActividades.fechaReal < DateTime.Today)
            {
                int a = 1;
                int b = 2;
                int c = 0;

                if (tarjetaActividades.estadoAct == a)
                {
                    tarjetaActividades.imgTgNeg.Visibility = Visibility.Visible;
                    tarjetaActividades.imgTgPurp.Visibility = Visibility.Hidden;
                }
                else if (tarjetaActividades.estadoAct == b)
                {
                    tarjetaActividades.imgTgNeg.Visibility = Visibility.Hidden;
                    tarjetaActividades.imgTgPurp.Visibility = Visibility.Visible;
                }
                else if (tarjetaActividades.estadoAct == c)
                {
                    tarjetaActividades.imgTgNeg.Visibility = Visibility.Hidden;
                    tarjetaActividades.imgTgPurp.Visibility = Visibility.Visible;
                }
            }

            if (tarjetaActividades.retraso == 1)
            {
                tarjetaActividades.imgAlerNeg.Visibility = Visibility.Hidden;
                tarjetaActividades.imgAlerRoj.Visibility = Visibility.Visible;
            }

            else if (tarjetaActividades.retraso == 0)
            {
                tarjetaActividades.imgAlerRoj.Visibility = Visibility.Hidden;
                tarjetaActividades.imgAlerNeg.Visibility = Visibility.Visible;
            }

            return tarjetaActividades;
        }


        private async void TarjetasHoy()
        {
            try
            {
                List<Actividad> actividad = new List<Actividad>();
                foreach (Actividad c in sa.GetEntities())
                {

                    if (c.Fecha_act == DateTime.Today)
                    {
                        stackActHoy.Children.Add(CrearTarjetaActividades(c.id_act, c.Cliente_id_emp, c.Prof_id_profe));
                    }

                }
            }

            catch(Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al cargar la información de hoy. \n" + ex.Message);
            }
            
        }

        private async void TarjetasSemana()
        {
            try
            {
                DateTime fechaHoy = DateTime.Today;
                DateTime fechaSemana = fechaHoy.AddDays(7);

                List<Actividad> actividad = new List<Actividad>();
                actividad.OrderBy(actividad => actividad.Hora_act);
                foreach (Actividad c in sa.GetEntities())
                {
                    if ((c.Fecha_act > fechaHoy) && (c.Fecha_act <= fechaSemana))
                    {
                        stackSemana.Children.Add(CrearTarjetaActividades(c.id_act, c.Cliente_id_emp, c.Prof_id_profe));
                    }
                }
            }

            catch(Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al cargar la información de la semana. \n" + ex.Message);
            }
            
        }

        private async void TarjetasCerradas()
        {
            try
            {
                DateTime fechaHoy = DateTime.Today;
                DateTime fechaSemana = fechaHoy.AddDays(7);

                List<Actividad> actividad = new List<Actividad>();
                foreach (Actividad c in sa.GetEntities())
                {
                    if (c.Fecha_act < fechaHoy)
                    {
                        stackCerradas.Children.Add(CrearTarjetaActividades(c.id_act, c.Cliente_id_emp, c.Prof_id_profe));
                    }
                }
            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al cargar la información de actividades completas. \n" + ex.Message);
            }

        }

        private async void FiltrarStackActHoy()
        {
            try
            {
                string filtro = txbBuscAct.Text.ToLower();
                stackActHoy.Children.Clear();

                foreach (Actividad c in sa.GetEntities())
                {

                    if (c.Fecha_act == DateTime.Today)
                    {
                        string nC = sc.GetEntity(c.Cliente_id_emp).Nombre_emp;
                        string nP = sp.GetEntity(c.Prof_id_profe).Nombre_prof + ' ' + sp.GetEntity(c.Prof_id_profe).Apellido_prof;
                        string nG = sg.GetEntity(sc.GetEntity(c.Cliente_id_emp).id_emp).Nombre_gerente;
                        string nA = sa.GetEntity(c.id_act).Tipo_actividad;

                        if (nC.ToLower().Contains(filtro) | nP.ToLower().Contains(filtro) | nG.ToLower().Contains(filtro) | nA.ToLower().Contains(filtro))
                        {
                            stackActHoy.Children.Add(CrearTarjetaActividades(c.id_act, c.Cliente_id_emp, c.Prof_id_profe));
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al filtrar. \n" + ex.Message);
            }
        }

        private async void FiltrarStackActSemana()
        {
            try
            {
                string filtro = txbBuscAct.Text.ToLower();
                stackSemana.Children.Clear();
                DateTime fechaHoy = DateTime.Today;
                DateTime fechaSemana = fechaHoy.AddDays(7);

                foreach (Actividad c in sa.GetEntities())
                {

                    if ((c.Fecha_act >= fechaHoy) && (c.Fecha_act <= fechaSemana))
                    {
                        string nC = sc.GetEntity(c.Cliente_id_emp).Nombre_emp;
                        string nP = sp.GetEntity(c.Prof_id_profe).Nombre_prof + ' ' + sp.GetEntity(c.Prof_id_profe).Apellido_prof;
                        string nG = sg.GetEntity(sc.GetEntity(c.Cliente_id_emp).id_emp).Nombre_gerente;
                        string nA = sa.GetEntity(c.id_act).Tipo_actividad;
                        string fA = sa.GetEntity(c.id_act).Fecha_act.ToShortDateString();

                        if (nC.ToLower().Contains(filtro) | nP.ToLower().Contains(filtro) | nG.ToLower().Contains(filtro) | nA.ToLower().Contains(filtro) | fA.Contains(filtro))
                        {
                            stackSemana.Children.Add(CrearTarjetaActividades(c.id_act, c.Cliente_id_emp, c.Prof_id_profe));
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al filtrar. \n" + ex.Message);
            }
        }

        private async void FiltrarStackActCerradas()
        {
            try
            {
                string filtro = txbBuscAct.Text.ToLower();
                stackCerradas.Children.Clear();
                DateTime fechaHoy = DateTime.Today;
                DateTime fechaSemana = fechaHoy.AddDays(7);

                foreach (Actividad c in sa.GetEntities())
                {

                    if (c.Fecha_act < fechaHoy)
                    {
                        string nC = sc.GetEntity(c.Cliente_id_emp).Nombre_emp;
                        string nP = sp.GetEntity(c.Prof_id_profe).Nombre_prof + ' ' + sp.GetEntity(c.Prof_id_profe).Apellido_prof;
                        string nG = sg.GetEntity(sc.GetEntity(c.Cliente_id_emp).id_emp).Nombre_gerente;
                        string nA = sa.GetEntity(c.id_act).Tipo_actividad;
                        string fA = sa.GetEntity(c.id_act).Fecha_act.ToShortDateString();

                        if (nC.ToLower().Contains(filtro) | nP.ToLower().Contains(filtro) | nG.ToLower().Contains(filtro) | nA.ToLower().Contains(filtro) | fA.Contains(filtro))
                        {
                            stackCerradas.Children.Add(CrearTarjetaActividades(c.id_act, c.Cliente_id_emp, c.Prof_id_profe));
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al filtrar. \n" + ex.Message);
            }
        }
        // ------------------------------------------------ ADM CLIENTE------------------------------------------------------ //
        public ClienteTarjetaCompleta CrearTarjeta(int idc, int idg, int idp)
        {
            ClienteTarjetaCompleta clienteTarjetaCompleta = new ClienteTarjetaCompleta();

            var rut = sc.GetEntity(idc).Rut_emp.ToString();

            clienteTarjetaCompleta.DisplayEmpresa = sc.GetEntity(idc).Nombre_emp;
            clienteTarjetaCompleta.DisplayRutEmpresa = rut;
            clienteTarjetaCompleta.DisplayGerente = sg.GetEntity(idg).Nombre_gerente;
            clienteTarjetaCompleta.DisplayProfNombre = sp.GetEntity(idp).Nombre_prof + ' ' + sp.GetEntity(idp).Apellido_prof;
            clienteTarjetaCompleta.DisplayMailGerente = sg.GetEntity(idg).Mail_gerente;
            clienteTarjetaCompleta.DisplayTelefonoEmpresa = sc.GetEntity(idc).Fono_cliente.ToString();
            clienteTarjetaCompleta.DisplayDireccion = sc.GetEntity(idc).Direccion_emp;
            clienteTarjetaCompleta.idClient = sc.GetEntity(idc).id_emp;

            clienteTarjetaCompleta.BtnAgregarActividadMejora.Visibility = Visibility.Hidden;
            clienteTarjetaCompleta.BtnAgregarAsesoria.Visibility = Visibility.Hidden;
            clienteTarjetaCompleta.BtnAgregarCapacitacion.Visibility = Visibility.Hidden; 
            clienteTarjetaCompleta.BtnAgregarVisita.Visibility = Visibility.Hidden;

            

            return clienteTarjetaCompleta;
        }

        private async void TarjetasClientes()
        {
            try
            {
                List<Cliente> cliente = new List<Cliente>();
                int idge = 0;
                foreach (Cliente c in sc.GetEntities())
                {
                    foreach(Gerente g in sg.GetEntities())
                    {
                        if (g.Cliente_id_clien == c.id_emp)
                        {
                            idge = g.id_gerente;
                            stackCliAdm.Children.Add(CrearTarjeta(c.id_emp, g.id_gerente, c.Profesional_id_prof));
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al cargar clientes. \n" + ex.Message);
            }


        }

        private async void FiltrarStackClientes()
        {
            try
            {
                string filtro = txbBuscAct.Text.ToLower();
                stackCliAdm.Children.Clear();

                foreach (Cliente c in sc.GetEntities())
                {
                    var rut = sc.GetEntity(c.id_emp).Rut_emp.ToString();
                    if (c.Nombre_emp.ToLower().Contains(filtro) | sp.GetEntity(c.Profesional_id_prof).Nombre_prof.ToLower().Contains(filtro) | sp.GetEntity(c.Profesional_id_prof).Apellido_prof.ToLower().Contains(filtro) | sg.GetEntity(c.id_emp).Nombre_gerente.ToLower().Contains(filtro) | c.Direccion_emp.ToLower().Contains(filtro) | sc.GetEntity(c.id_emp).Fono_cliente.ToString().ToLower().Contains(filtro) | sg.GetEntity(c.id_emp).Mail_gerente.ToLower().Contains(filtro) | rut.ToLower().Contains(filtro))
                    {

                        stackCliAdm.Children.Add(CrearTarjeta(c.id_emp, sg.GetEntity(c.id_emp).id_gerente, c.Profesional_id_prof));

                    }

                }

            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al filtrar. \n" + ex.Message);
            }
        }
        // ------------------------------------------------ADM PAGOS------------------------------------------------------ //
        public TarjetaPagos CrearTarjetaPagos(int idc, int idg)
        {
            TarjetaPagos tarjetaPagos = new TarjetaPagos();
            var rut = sc.GetEntity(idc).Rut_emp.ToString();

            tarjetaPagos.nombreEmp = sc.GetEntity(idc).Nombre_emp;
            tarjetaPagos.rutEmp = rut;

            List <Pagos> items = new List<Pagos>();
            foreach (Pago p in spa.GetEntities())
            {
                
                if (p.Contrato_id_cont == sco.GetEntity(idg).id_contrato)
                {
                    int contadorAsTotal = 0;
                    int contadorAsMes = 0;
                    int contadorAsHor = 0;
                    

                    int contadorCapTotal = 0;
                    int contadorCapMes = 0;

                    int contadorCheckTotal = 0;
                    int contadorCheckMens = 0;

                    int contadorCheckEX = 0;
                    int contadorAsEX = 0;
                    int contadorCapEX = 0;
                    int contadorInfEX = 0;



                    int? valorExCheck = spl.GetEntity(p.Plan_id_plan).Valor_ExtraChecklist;
                    int? valorExAs = spl.GetEntity(p.Plan_id_plan).Valor_ExtraAsesoria;
                    int? valorExCap = spl.GetEntity(p.Plan_id_plan).Valor_ExtraCapacitaciones;
                    int? ValorExInforme = spl.GetEntity(p.Plan_id_plan).Valor_ExtraInforme;

                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-ES", false);
                    foreach (Actividad a in sa.GetEntities())
                    {
                        if (a.Cliente_id_emp == idc && a.Tipo_actividad == "Asesoria")
                        {
                            contadorAsTotal++;
                            if (a.Fecha_act.Month == DateTime.Now.Month)
                            {
                                contadorAsMes++;
                                var horaActiv = DateTime.Parse(new DateTime(a.Hora_act.Ticks).ToString("hh:mm tt").ToUpper());
                                var horaTem = DateTime.Parse("09:00 AM");
                                var horaTar = DateTime.Parse("06:00 PM");
                                if ( horaActiv > horaTar && horaTem > horaActiv )
                                {
                                    contadorAsHor++;
                                    
                                }
                            }

                        }

                        if (contadorAsTotal > 1)
                        {
                            int dif = contadorAsTotal - (contadorAsMes + contadorAsHor);
                            if (dif >= 1)
                            {
                                contadorAsEX = contadorAsMes + contadorAsHor;
                            }

                            if (dif == 0)
                            {
                                contadorAsEX = (contadorAsMes + contadorAsHor) - 1;
                            }

                        }


                        if (a.Cliente_id_emp == idc && a.Tipo_actividad == "Capacitación")
                        {
                            contadorCapTotal++;
                        }

                        if (a.Cliente_id_emp == idc && a.Tipo_actividad == "Capacitación" && p.Mes_pago.ToUpper() == a.Fecha_act.ToString("MMMM", culture).ToUpper()) //AGREGAR MES 
                        {
                            contadorCapMes++;                           
                        }

                        

                        if (contadorCapTotal > 1)
                        {
                            int dif = contadorCapTotal - contadorCapMes;
                            if (dif >= 1)
                            {
                                contadorCapEX = contadorCapMes;
                            }

                            if (dif == 0)
                            {
                                contadorCapEX = contadorCapMes - 1;
                            }

                        }
                    }

                    foreach (Solicitud s in sso.GetEntities())
                    {
                        if (s.Gerente_id_gerente == idg & s.Razon_soli == "CHECKLIST" & s.Estado_solicitud == "APROBADA")
                        {
                            contadorCheckTotal++;
                        }

                        if (s.Gerente_id_gerente == idg & s.Razon_soli == "CHECKLIST" & s.Estado_solicitud == "APROBADA" & s.Fecha_CreacionSolicitud.Month == DateTime.Now.Month)
                        {
                            contadorCheckMens++;
                        }

                        if (s.Gerente_id_gerente == idg & s.Razon_soli == "INFORME" & s.Estado_solicitud == "APROBADA")
                        {
                            contadorInfEX++;
                        }
                    }

                    if (contadorCheckTotal > 2)
                    {
                        int dif = contadorCheckTotal - contadorCheckMens;
                        if (dif >= 2)
                        {
                            contadorCheckEX = contadorCheckMens;
                        }

                        if (dif == 1)
                        {
                            contadorCheckEX = contadorCheckMens - 1;
                        }

                        if (dif == 0)
                        {
                            contadorCheckEX = contadorCheckMens - 2;
                        }

                    }

                    valorExCheck = valorExCheck * contadorCheckEX;
                    valorExAs = valorExAs * contadorAsEX;
                    valorExCap = valorExCap * contadorCapEX;
                    ValorExInforme = ValorExInforme * contadorInfEX;

                    int? valorExtra = valorExCheck + valorExAs + valorExCap + ValorExInforme;

                    Debug.WriteLine("Mes pago, fecha hoy: " + p.Mes_pago.ToUpper() + ", " + DateTime.Today.Month.ToString("MMMM", culture).ToUpper());

                    if (p.Mes_pago.ToUpper() == DateTime.Today.ToString("MMMM", culture).ToUpper())
                    {
                        actualizarBD("pago", "Valor_extra", Convert.ToInt32(valorExtra).ToString(), p.id_pago);
                        actualizarBD("pago", "Total_a_pagar", (valorExtra + Convert.ToInt32(spl.GetEntity(p.Plan_id_plan).Valor_plan)).ToString(), p.id_pago);
                        //AGREGAR TOTAL
                    }

                    items.Add(new Pagos() { Estado = p.Estado_pago, Mes = p.Mes_pago, Plan = spl.GetEntity(p.Plan_id_plan).Tipo_plan, ValorPlan = Convert.ToInt32(spl.GetEntity(p.Plan_id_plan).Valor_plan), ValorExtra = Convert.ToInt32(valorExtra), Total = Convert.ToInt32(p.Total_a_pagar), idPago = p.id_pago, idCliente = idc, idComprobante = p.Comprobante_id_com });


                    

                    

                    Debug.WriteLine("Total asesoria, total capac, total check: " + contadorAsTotal.ToString() + contadorCapMes.ToString() + contadorCheckTotal.ToString());

                    Debug.WriteLine("Valor extra chek, valor extra asesoria, valor extra cap, valor extra informe: " + valorExCheck.ToString() + "," +  valorExAs.ToString() + "," + valorExCap.ToString()+ "," + ValorExInforme.ToString());
                    
                }
            }

            foreach (Contrato co in sco.GetEntities())
            {
                int contadorPago = 0;
                int contadorMes = 0;
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-ES", false);
                foreach (Pago p in spa.GetEntities())
                {
                    if (co.id_contrato == p.Contrato_id_cont)
                    {
                        contadorPago++;
                        if (DateTime.ParseExact(p.Mes_pago, "MMMM", culture).Month
                        .Equals(DateTime.Today.Month))
                        {
                            contadorMes++;
                        }
                    }
                }

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

                if (contadorPago == 0)
                {
                    crearPago("PENDIENTE", mes, 0, Convert.ToInt32(spl.GetEntity(co.Plan_id_plan).Valor_plan), Convert.ToInt32(co.Plan_id_plan), Convert.ToInt32(co.id_contrato));
                }

                if (contadorPago >= 1)
                {
                    if (contadorMes == 0)
                    {
                        crearPago("PENDIENTE", mes, 0, Convert.ToInt32(spl.GetEntity(co.Plan_id_plan).Valor_plan), Convert.ToInt32(co.Plan_id_plan), Convert.ToInt32(co.id_contrato));
                    }
                }
                
            }

            tarjetaPagos.lvPagos.ItemsSource = items;
      

            return tarjetaPagos;
        }

        private async void TarjetaPagos()
        {
            try
            {
                int idge = 0;
                foreach (Cliente c in sc.GetEntities())
                {
                    foreach (Gerente g in sg.GetEntities())
                    {
                        if (g.Cliente_id_clien == c.id_emp)
                        {
                            idge = g.id_gerente;
                            stackPagos.Children.Add(CrearTarjetaPagos(c.id_emp, idge));
                        }
                    }
                }
            }

            catch(Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al cargar los pagos. \n" + ex.Message);
            }

            
        }

        private async void FiltrarStackPagos()
        {
            try
            {
                string filtro = txbBuscAct.Text.ToLower();
                stackPagos.Children.Clear();

                foreach (Cliente c in sc.GetEntities())
                {

                    if (c.Nombre_emp.ToLower().Contains(filtro) | c.Rut_emp.ToLower().Contains(filtro))
                    {
                        stackPagos.Children.Add(CrearTarjetaPagos(c.id_emp, sg.GetEntity(c.id_emp).id_gerente));
                    }

                }

            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al filtrar. \n" + ex.Message);
            }
        }  

        // ------------------------------------------------ADM PROF------------------------------------------------------ //
            
        public TarjetaProfesionales CrearTarjetaProfesional(int idp)
        {
            TarjetaProfesionales tarjetaProfesionales = new TarjetaProfesionales();

            var rut = sp.GetEntity(idp).Rut_prof.ToString();

            tarjetaProfesionales.rutProf = rut;
            tarjetaProfesionales.nombreProfesional = sp.GetEntity(idp).Nombre_prof;
            tarjetaProfesionales.apellidoProfesional = sp.GetEntity(idp).Apellido_prof;
            tarjetaProfesionales.telefonoProfesional = sp.GetEntity(idp).Telefono;
            tarjetaProfesionales.direccProf = sp.GetEntity(idp).Direccion;
            tarjetaProfesionales.mailProf = sp.GetEntity(idp).Mail_prof;

            tarjetaProfesionales.idProfe = sp.GetEntity(idp).id_prof;

            int contador = 0;
            foreach (Actividad a in sa.GetEntities())
            {
                if (a.Prof_id_profe == idp && a.Tipo_actividad == "Capacitación")
                {
                    contador++;
                }
            }
            tarjetaProfesionales.nCap = contador.ToString();

            contador = 0;
            foreach (Actividad a in sa.GetEntities())
            {
                if (a.Prof_id_profe == idp && a.Tipo_actividad == "Act Mejora")
                {
                    contador++;
                }
            }
            tarjetaProfesionales.nMejoras = contador.ToString();

            contador = 0;
            foreach (Actividad a in sa.GetEntities())
            {
                if (a.Prof_id_profe == idp && a.Tipo_actividad == "Visita")
                {
                    contador++;
                }
            }
            tarjetaProfesionales.nVisitas = contador.ToString();

            contador = 0;
            foreach (Actividad a in sa.GetEntities())
            {
                if (a.Prof_id_profe == idp && a.Tipo_actividad == "Asesoria")
                {
                    contador++;
                }
            }
            tarjetaProfesionales.nAsesorias = contador.ToString();

            contador = 0;
            foreach (Actividad a in sa.GetEntities())
            {
                if (a.Prof_id_profe == idp && a.Tipo_actividad == "Casos")
                {
                    contador++;
                }
            }
            tarjetaProfesionales.nCasos = contador.ToString();

            contador = 0;
            foreach (Cliente c in sc.GetEntities())
            {
                if (c.Profesional_id_prof == idp)
                {
                    contador++;
                }
            }
            tarjetaProfesionales.nClientes = contador.ToString();

            return tarjetaProfesionales;
        }

        private async void TarjetaProfesional()
        {
            try
            {
                foreach (Profesional p in sp.GetEntities())
                {
                    stackProf.Children.Add(CrearTarjetaProfesional(p.id_prof));
                }
            }

            catch(Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al cargar los profesionales. \n" + ex.Message);
            }
            
        }

        private async void FiltrarStackProf()
        {
            try
            {
                string filtro = txbBuscAct.Text.ToLower();
                stackProf.Children.Clear();

                foreach (Profesional p in sp.GetEntities())
                {

                    if (p.Rut_prof.ToLower().Contains(filtro) | p.Nombre_prof.ToLower().Contains(filtro) | p.Mail_prof.ToLower().Contains(filtro) | p.Apellido_prof.ToLower().Contains(filtro))
                    {

                        stackProf.Children.Add(CrearTarjetaProfesional(p.id_prof));

                    }

                }

            }

            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR: ", "Se ha producido un error al filtrar. \n" + ex.Message);
            }
        }
        //////////////////////////////////////////////// FIN METODOS CREADOS //////////////////////////////////////////////////////////////


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            this.Close();
            login.ShowDialog();
        }

        private void stackActHoy_Initialized(object sender, EventArgs e)
        {
            TarjetasHoy();
        }

        private void stackSemana_Initialized(object sender, EventArgs e)
        {
            TarjetasSemana();
        }

        private void stackCerradas_Initialized(object sender, EventArgs e)
        {
            TarjetasCerradas();
        }

        private void stackCliAdm_Initialized(object sender, EventArgs e)
        {
            TarjetasClientes();
            
        }

        private void txbBuscAct_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarStackActHoy();
            FiltrarStackActSemana();
            FiltrarStackActCerradas();

            FiltrarStackClientes();
            FiltrarStackProf();
            FiltrarStackPagos();
        }

        private void stackPagos_Initialized(object sender, EventArgs e)
        {
            TarjetaPagos();
        }

        private void stackProf_Initialized(object sender, EventArgs e)
        {
            TarjetaProfesional();
        }

        private void stackCliAdm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Administrador adm = new Administrador();
                this.Close();
                adm.Show();
            }
            
        }

        private void TabControlAct2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Administrador adm = new Administrador();
                this.Close();
                adm.Show();
            }
        }

        private void stackPagos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Vadmin_Closed(object sender, EventArgs e)
        {
           
        }

        private void TabItemHoyAct_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TabitemClientes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgAgUs.Visibility = Visibility.Visible;
            btnAgregarCli.IsEnabled = true;
            btnAgregarCli.Width = 21;
            btnAgregarCli.Height = 25;
            btnAgregarProf.IsEnabled = false;
            btnAgregarProf.Width = 0;
            btnAgregarProf.Height = 0;
        }

        private void TabitemProfesionales_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgAgUs.Visibility = Visibility.Visible;
            btnAgregarProf.IsEnabled = true;
            btnAgregarProf.Width = 21;
            btnAgregarProf.Height = 25;
            btnAgregarCli.IsEnabled = false;
            btnAgregarCli.Width = 0;
            btnAgregarCli.Height = 0;
        }

        private void TabitemActividades_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgAgUs.Visibility = Visibility.Hidden;
            btnAgregarProf.IsEnabled = false;
            btnAgregarCli.IsEnabled = false;
        }

        private void btnAgregarCli_Click(object sender, RoutedEventArgs e)
        {
            AgregarUsuario agregarUsuario = new AgregarUsuario();
            CrearCliente cliente = new CrearCliente();
            agregarUsuario.stackUsuario.Children.Add(cliente);
            agregarUsuario.lblUsuario.Content = "Nuevo Cliente";
            agregarUsuario.Show();
            
        }

        private void btnAgregarProf_Click(object sender, RoutedEventArgs e)
        {
            AgregarUsuario agregarUsuario = new AgregarUsuario();
            CrearProfesional crearProfesional = new CrearProfesional(); 
            agregarUsuario.stackUsuario.Children.Add(crearProfesional);
            agregarUsuario.lblUsuario.Content = "Nuevo Profesional";
            agregarUsuario.Show();
            
        }
    }
}

