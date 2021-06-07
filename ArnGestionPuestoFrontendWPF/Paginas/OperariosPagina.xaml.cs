using ArnGestionPuestoFrontendWPF.Ventanas;
using BDSQL;
using Entidades.EntidadesBD;
using JsonResolvers;
using MQTT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Turnos;

namespace ArnGestionPuestoFrontendWPF.Paginas
{
    /// <summary>
    /// Lógica de interacción para OperariosPagina.xaml
    /// </summary>
    public partial class OperariosPagina : Page, INotifyPropertyChanged
    {
        public string CodigoOperario { get; set; } = "";
        //public ObservableCollection<Operarios> Operarios
        //{
        //    get
        //    {
        //        return new ObservableCollection<Operarios>(Store.Operarios);
        //    }
        //}
        public Operarios Operario
        {
            get
            {
                return Store.OperarioEjecucion;
            }
        }

        public OperariosPagina()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnOperarioEntra += BusEventos_OnOperarioEntra;
            BusEventos.OnOperarioSale += BusEventos_OnOperarioSale; ;
        }

        private void BusEventos_OnOperarioSale(object sender, EventosTareas.OperarioSalidaEventArgs e)
        {
            Notifica();
        }

        private void BusEventos_OnOperarioEntra(object sender, EventosTareas.OperarioEntradaEventArgs e)
        {
            Notifica();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtNumeroClick(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            string text = bt.Name.Replace("Bt", "");
            this.CodigoOperario += text;
            Notifica("CodigoOperario");
        }

        private void BtBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (this.CodigoOperario.Length > 0)
            {
                this.CodigoOperario = this.CodigoOperario.Substring(0, this.CodigoOperario.Length - 1);
                Notifica("CodigoOperario");
            }
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.CodigoOperario.Length > 0)
            {
                Operarios o = Select.BuscarOperarioPorCodigo(this.CodigoOperario.Trim());
                if (o != null)
                {
                    if (!Store.Operarios.Any(x => x.Id == o.Id))
                    {
                        new Aviso(string.Format("¡{0}!", Horario.CalcularSaludoActual()), hablar: true).Show();

                        Store.Operarios.Clear();
                        Store.Operarios.Add(o);
                        BusEventos.OperarioEntra(o);
                        if (Store.Bancada.IdHermano != null)
                        {
                            ClienteMQTT.Publicar(string.Format("/puesto/loginHermano/{0}", Store.Bancada.IdHermano), JsonConvert.SerializeObject(o, new JsonSerializerSettings
                            {
                                ContractResolver = new CustomResolver(),
                                PreserveReferencesHandling = PreserveReferencesHandling.None,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Formatting = Formatting.Indented
                            }), 2);
                        }

                        NavegacionEventos.CargarNuevaPagina(NavegacionEventos.PaginaTarea);


                    }

                }
                this.CodigoOperario = "";
                Notifica("CodigoOperario");
            }
        }

        private void BtSalir_Click(object sender, RoutedEventArgs e)
        {
            if (Store.OperarioEjecucion != null)
            {
                Operarios o = Store.OperarioEjecucion;
                new Aviso(string.Format("¡Hasta pronto!"), hablar: true).Show();
                Store.Operarios.Clear();

                if (Store.Bancada.IdHermano != null)
                {
                    ClienteMQTT.Publicar(string.Format("/puesto/logoutHermano/{0}", Store.Bancada.IdHermano), JsonConvert.SerializeObject(o, new JsonSerializerSettings
                    {
                        ContractResolver = new CustomResolver(),
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented
                    }), 2);
                }

                BusEventos.OperarioSale(o);

            }
        }

        //private void BtOperarioSalir_Click(object sender, RoutedEventArgs e)
        //{
        //    if(this.TablaOperarios.SelectedItems.Count == 1)
        //    {
        //        Operarios o = this.TablaOperarios.SelectedItems[0] as Operarios;
        //        Store.Operarios.Remove(o);
        //        BusEventos.OperarioSale(o);
        //    }
        //}
    }
}
