using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace MQTT
{
    public static class ClienteMQTT
    {
        public static List<Topic> Topics { get; private set; } = new List<Topic>();
        private static MqttClient client;
        private static string topicConsumo = "";
        private static bool cierreForzado = false;
        private static string codigoOperario = "";
        public static bool Conectado { get; set; }
        static ClienteMQTT()
        {

        }

        public static void Iniciar(string codigoOperario)
        {
            ClienteMQTT.codigoOperario = codigoOperario;
            client = new MqttClient("192.168.0.104");
            string clientId = string.Format("{0}_{1}", "arn-iot-puesto", codigoOperario);
            client.Connect(clientId, "", "", true, 10);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            Conectado = true;


            client.ConnectionClosed += Client_ConnectionClosed;

            Suscribir();
        }

        private static void Client_ConnectionClosed(object sender, EventArgs e)
        {
            if (!cierreForzado)
            {
                Iniciar(ClienteMQTT.codigoOperario);
            }
            else
            {
                cierreForzado = false;
            }
        }

        public static void Publicar(string topic, string msg, int qos)
        {
            if (Conectado)
            {
                client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), (byte)qos, false);
            }
        }

        public static void Suscribir()
        {

            foreach (Topic topic in Topics)
            {
                client.Subscribe(new string[] { topic.Nombre }, new byte[] { topic.QOS });
            }
        }


        private static void Desuscribir()
        {
            foreach (Topic topic in Topics)
            {
                client.Unsubscribe(new string[] { topic.Nombre });
            }
        }

        public static void Cerrar()
        {
            try
            {
                cierreForzado = true;
                Desuscribir();
                client.Disconnect();
            }
            catch (Exception ex)
            {

            }

        }

        private static void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            foreach (Topic topic in Topics)
            {
                if (topic.Expresion.IsMatch(e.Topic))
                {
                    for (int i = 0; i < topic.Callbacks.Count; i++)
                    {
                        Action<string, string, Topic> callback = topic.Callbacks[i];
                        callback(System.Text.Encoding.UTF8.GetString(e.Message), e.Topic, topic);
                    }
                }
            }
        }

    }
}
