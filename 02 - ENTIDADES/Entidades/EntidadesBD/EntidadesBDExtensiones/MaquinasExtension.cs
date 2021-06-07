using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesBD
{
    public partial class Maquinas : Notificable
    {
        public event EventHandler OnErrorTareaSinEjecutar;
        public event EventHandler OnColaTrabajoActualizada;
        public event EventHandler OnParesConsumidos;
        public event EventHandler OnInfoEjecucionActualizada;

        private const double COEF_VARIACION = 0.03;

        public string ModuloViejo
        {
            get
            {
                if (this.Nombre
                    != null)
                {
                    string nombreSust = this.Nombre.Replace("MOLDE ESPUMA ", "");
                    return nombreSust.Substring(0, 2);
                }
                return "";
            }
        }
        public string Cliente
        {
            get
            {


                if (TrabajoEjecucion != null)
                {
                    string nombre = TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.Campos_ERP.NOMBRECLI;
                    if (nombre == null)
                    {
                        return "ARNEPLANT S.L.";
                    }
                    else
                    {
                        return nombre;
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        public int IdOperacion
        {
            get
            {


                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.ID;

                }
                else
                {
                    return 0;
                }
            }
        }
        public string Utillaje
        {
            get
            {

                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.CodUtillaje;
                }
                else
                {
                    return "";
                }
            }
        }
        public string Modelo
        {
            get
            {
                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.Campos_ERP.DESCRIPCIONARTICULO;
                }
                else
                {
                    return "";
                }
            }
        }
        public int ParesFabricando
        {
            get
            {
                if (TrabajoEjecucion != null)
                {
                    return (int)TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.CantidadFabricar.Value +
                         (int)TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.CantidadSaldos.Value;
                }
                else
                {
                    return 0;
                }
            }
        }
        public string TallaUtillaje
        {
            get
            {

                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.IdUtillajeTalla;
                }
                else
                {
                    return "";
                }
            }
        }
        public string CodigoOrden
        {
            get
            {
                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.Codigo;
                }
                else
                {
                    return "";
                }
            }
        }

        public int IdOrden
        {
            get
            {
                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.ID;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int IdTarea { get; private set; }
        public string CodigoArticulo
        {
            get
            {
                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.CodigoArticulo;
                }
                else
                {
                    return "";
                }
            }
        }
        public int IdArticulo
        {
            get
            {
                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.IdArticulo??0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string TallaArticulo
        {
            get
            {
                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.Tallas;
                }
                else
                {
                    return "";
                }
            }
        }
        public double SgCiclo { get; private set; }
        public double ParesCiclo { get; set; }

        public double CantidadCaja
        {
            get
            {

                if (TrabajoEjecucion != null)
                {
                    return TrabajoEjecucion.CantidadEtiquetaFichada;
                }
                else
                {
                    return 0;
                }
            }
        }

        public double CantidadCajaRealizada
        {
            get
            {

                if (this.TrabajoEjecucion != null)
                {
                    var pulsos = this.Pulsos.Where(x => x.CodigoEtiqueta == this.TrabajoEjecucion.CodigoEtiquetaFichada);
                    if (pulsos.Count() > 0)
                    {
                        return pulsos.Sum(x => x.Pares);
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }
            }
        }

        public MaquinasColasTrabajo TrabajoEjecucion
        {
            get
            {
                if (IdTarea != 0 && this.MaquinasColasTrabajo.FirstOrDefault(x => x.IdTarea == IdTarea) != null)
                {
                    var trabajo = this.MaquinasColasTrabajo.FirstOrDefault(x => x.IdTarea == IdTarea);
                    return trabajo;

                }
                else
                {
                    if (this.MaquinasColasTrabajo.Count > 0)
                    {
                        var enEjecucion = this.MaquinasColasTrabajo.Where(x => x.Ejecucion).ToList();
                        if (enEjecucion.Count != 1)
                        {
                            if (enEjecucion.Count == 0)
                            {
                                return null;
                            }
                            else
                            {
                                // agrupacion
                                double maxPendientes = this.MaquinasColasTrabajo.Max(x => x.ParesPendientes);
                                return this.MaquinasColasTrabajo.FirstOrDefault(x => x.ParesPendientes == maxPendientes);
                            }
                        }
                        else
                        {
                            return enEjecucion[0];
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        public List<PulsoMaquina> Pulsos { get; private set; } = new List<PulsoMaquina>();

        public void ColaTrabajoActualizada()
        {
            if (OnColaTrabajoActualizada != null)
            {
                OnColaTrabajoActualizada(this, new EventArgs());
            }
        }

        public void InfoEjecucionActualizada()
        {
            if (OnInfoEjecucionActualizada != null)
            {
                OnInfoEjecucionActualizada(this, new EventArgs());
            }
        }

        public void ErrorTareaSinEjecutar()
        {
            if (OnErrorTareaSinEjecutar != null)
            {
                OnErrorTareaSinEjecutar(this, new EventArgs());
            }
        }

        public void ParesConsumidos()
        {
            if (OnParesConsumidos != null)
            {
                OnParesConsumidos(this, new EventArgs());
            }
        }

        public void AsignarColaTrabajo(List<MaquinasColasTrabajo> cola)
        {
            this.MaquinasColasTrabajo = cola.OrderBy(x => x.Posicion).ToList();
            ColaTrabajoActualizada();
            Notifica();
        }

        public void DesasignarTrabajo(MaquinasColasTrabajo trabajo)
        {
            var lista = this.MaquinasColasTrabajo.ToList();
            lista.RemoveAll(x => x.IdMaquina == trabajo.IdMaquina && x.Posicion == trabajo.Posicion);
            this.AsignarColaTrabajo(lista);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Maquinas o = (obj as Maquinas);
            return o.Posicion == this.Posicion && o.IpAutomata == this.IpAutomata;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }

        public void CargarInformacion(int idTarea)
        {
            this.IdTarea = idTarea;
            this.InfoEjecucionActualizada();
            Notifica();
        }

        public bool InsertarPares(MaquinasColasTrabajo trabajo, double pares)
        {
            MaquinasColasTrabajo t = this.MaquinasColasTrabajo.FirstOrDefault(x => x.Id == trabajo.Id && x.Ejecucion);
            if (t != null)
            {

                t.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionProductos.Add(new OrdenesFabricacionProductos
                {
                    Cantidad = pares,
                    FechaCreacion = DateTime.Now,
                    IdMaquina = this.ID,

                });
                this.Notifica();
                t.Notifica();
                this.ColaTrabajoActualizada();
                this.ParesConsumidos();
                return true;
            }
            else
            {
                this.ErrorTareaSinEjecutar();
                return false;
            }
        }

        public static Maquinas Default
        {
            get
            {
                return new Maquinas
                {
                    Nombre = "- SIN MAQUINA -",
                    CodSeccion = "- SIN SECCION -",
                    MaquinasColasTrabajo = new List<MaquinasColasTrabajo> {
                        new EntidadesBD.MaquinasColasTrabajo {
                            Ejecucion = true,
                            CodigoEtiquetaFichada = "SIN ETIQUETA",
                            OrdenesFabricacionOperacionesTallasCantidad = new EntidadesBD.OrdenesFabricacionOperacionesTallasCantidad
                            { CantidadFabricar = 0,
                                CantidadSaldos = 0,
                        OrdenesFabricacionOperacionesTallas = new OrdenesFabricacionOperacionesTallas {
                        Tallas = "SIN TALLAS", IdUtillajeTalla = "SIN TALLA", OrdenesFabricacionOperaciones = new OrdenesFabricacionOperaciones { CodUtillaje = "-SIN UTILLAJE-", CodSeccion = "- SIN SECCION-", OrdenesFabricacion = new OrdenesFabricacion { Codigo = "-SIN OF-", Campos_ERP = new Campos_ERP { NOMBRECLI = "-SIN CLIENTE-" } } } }
                    } }
                    }

                };
            }
        }
    }
}
