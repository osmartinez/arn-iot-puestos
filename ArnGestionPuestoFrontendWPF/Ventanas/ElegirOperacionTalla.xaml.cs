﻿using Entidades.EntidadesBD;
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
using System.Windows.Shapes;

namespace ArnGestionPuestoFrontendWPF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para ElegirOperacionTalla.xaml
    /// </summary>
    public class BotonOfot : Button
    {
        public OrdenesFabricacionOperacionesTallas Ofot { get; set; }
        public BotonOfot(OrdenesFabricacionOperacionesTallas ofot)
        {
            this.Ofot = ofot;
            this.Style = Application.Current.TryFindResource("BotonOperacionMatematica") as Style;
            this.Content = new TextBlock { Text = ofot.IdUtillajeTalla };
        }

    }
    public partial class ElegirOperacionTalla : Window
    {
        public OrdenesFabricacionOperacionesTallas OfotElegida { get; set; }
        public ElegirOperacionTalla(List<OrdenesFabricacionOperacionesTallas> ofots)
        {
            InitializeComponent();
            double max_filas = 3;
            double max_columnas = 4;
            for (int i = 0; i < max_filas; i++)
            {
                this.Grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < max_columnas; i++)
            {
                this.Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int fila = 0;
            int columna = 0;
            foreach (var ofot in ofots)
            {
                BotonOfot boton = new BotonOfot(ofot);
                boton.Click += (s, e) =>
                {
                    OfotElegida = ofot;
                    this.Close();
                };
                Grid.SetRow(boton, fila);
                Grid.SetColumn(boton, columna);
                if (columna == max_columnas - 1)
                {
                    fila++;
                    columna = 0;
                }
                else
                {
                    columna++;
                }
                this.Grid.Children.Add(boton);
            }

        }

    }
}
