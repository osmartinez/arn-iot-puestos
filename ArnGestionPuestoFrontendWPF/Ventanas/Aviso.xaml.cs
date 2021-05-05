using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
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
using System.Windows.Threading;

namespace ArnGestionPuestoFrontendWPF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Aviso.xaml
    /// </summary>
    public partial class Aviso : Window
    {
        public string Texto { get; private set; }
        private DispatcherTimer timer;
        public Aviso(string texto, int ms = 800, bool hablar = false)
        {
            InitializeComponent();

            if (!hablar)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Assets/beep_error.wav");
                player.Play();
            }
            else
            {

                var synthesizer = new SpeechSynthesizer();
                var pbuilder = new PromptBuilder();
                string codIdioma = "es-ES";
                string idioma = "Spanish";
                var culture = CultureInfo.GetCultureInfo(codIdioma);
                var voices = synthesizer.GetInstalledVoices(culture).Where(x=>x.VoiceInfo.Description.Contains(idioma));
                if (voices.Count() > 0)
                {
                    synthesizer.SelectVoice(voices.ElementAt(0).VoiceInfo.Name); //you need to call this API
                    pbuilder.AppendText(texto);
                    synthesizer.SpeakAsync(pbuilder);
                }



            }

            this.DataContext = this;
            this.Texto = texto;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, ms);
            timer.Tick += Timer_Tick;
            this.timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.Close();
        }
    }
}
