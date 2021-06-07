using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesDTO
{
    public class AgrupacionStock:Notificable
    {
        public List<TallaCantidad> TallasCantidades
        {
            get
            {
                List<TallaCantidad> res = new List<TallaCantidad>();

                Dictionary<string, double> dic = new Dictionary<string, double>();
                foreach(var stock in Stocks)
                {
                    foreach(var sat in stock.StockArticulosTallas)
                    {
                        if (!dic.ContainsKey(sat.Talla))
                        {
                            dic.Add(sat.Talla, sat.Cantidad??0);
                        }
                        else
                        {
                            dic[sat.Talla] += sat.Cantidad ?? 0;
                        }
                    }
                }

                foreach(var par in dic)
                {
                    res.Add(new TallaCantidad { Cantidad = par.Value, Talla = par.Key });
                }

                return res;
            }
        }

        public List<StockArticulos> Stocks { get; private set; } = new List<StockArticulos>();

        /// <summary>
        /// recibe una talla y el balance +- pares que hay que añadir o quitar de esa talla
        /// </summary>
        /// <param name="talla"></param>
        /// <param name="paresOperacion"></param>
        public void Actualizar(string talla, double paresOperacion)
        {
            double paresAplicados = 0;
            foreach(var stock in this.Stocks)
            {
                foreach(var sat in stock.StockArticulosTallas.Where(x=>x.Talla==talla))
                {
                    if(paresAplicados>= Math.Abs(paresOperacion))
                    {
                        Notifica("TallasCantidades");
                        return;
                    }

                    if (paresOperacion < 0)
                    {
                        if(sat.Cantidad+paresOperacion > 0)
                        {
                            paresAplicados += Math.Abs(paresOperacion);

                            sat.Cantidad += paresOperacion;
                        }
                        else
                        {
                            paresAplicados += Math.Abs(sat.Cantidad??0);
                            sat.Cantidad = 0;
                        }
                    }
                    else
                    {
                        paresAplicados += Math.Abs(paresOperacion);
                        sat.Cantidad += paresOperacion;

                    }
                }
            }
            Notifica("TallasCantidades");
        }

        public AgrupacionStock(List<StockArticulos> stocks)
        {
            Stocks = stocks;
        }
    }
}
