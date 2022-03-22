using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Servidor_SMS
{
    internal class Sms
    {
        public string sUrl { get; set; }
        private string token { get; set; }
        private string puerto { get => ":8766"; }
        private string ipa { get; set; }
        public string gateway { get; set; } = "http://192.168.2.164:8766";
        private RestClient client { get; set; } = new RestClient();
        private RestRequest request { get; set; } = new RestRequest(Method.GET);

        public Sms()
        {
            token = "";
            //getGateWayList();
        }

        public string enviar(string number, string message)
        {
            //if (Properties.Settings.Default.gateWay.Count != 0)
            //{
            // foreach (string geteway in Properties.Settings.Default.gateWay)
            // {
            sUrl = $@"{gateway}/?number={number}&message={message}&token={token}";
            client = new RestClient(sUrl);

            IRestResponse response = client.Execute(request);

            if (response.Content != "")
            {
                return "SMS ok";
            }
            else
            {
                return "no se envio";
            }

            // }
            //   return "";
            // }
            // else
            // {
            //     return "No se encontro ningun dispositivo conectado a la red";
            // }
        }

        public void enviar()
        {
            string gateway = "http://192.116.2.164:8766";
            //request.AddParameter("number", number);
            //request.AddParameter("message", message);
            //request.AddParameter("token", token);
            sUrl = $@"{gateway}/?number=3313480007&message=Daniel Iñiguez genero un registro a las 9:00 a.m. 22/03/2020&token={token}";
            client = new RestClient(sUrl);
            //client.BaseUrl = new  Uri(sUrl);
            //request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
        }

        public string getBaseGateWay()
        {
            findIP();

            string baseGateWay = $"{ ipa.Substring(0, (ipa.Length - ipa.Split('.')[3].Length))}";

            return baseGateWay;
        }

        public void getGateWayList()
        {
            string _geteway = getBaseGateWay();

            for (int i = 2; i < 150; i++)
            {
                sUrl = $@"http://{_geteway}{i}{puerto}/?number=3326547068&message=SUCCESS&token={token}";
                client = new RestClient(sUrl);
                IRestResponse response = client.Execute(request);

                if (response.Content != "")
                {
                   // Properties.Settings.Default.gateWay.Add($"http://{_geteway}{i}{puerto}");
                    //listGateWay.Add($"http://{_geteway}{i}{puerto}");
                }
            }
        }

        public void findIP()
        {
            string host = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostAddresses(host);

            foreach (IPAddress IPA in ip)
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ipa = $"{IPA.ToString()}";
                    break;
                }
            }
        }


        /*public static string GetIpCliente(System.Web.HttpContext contexto)
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(contexto.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork" || IPA.AddressFamily.ToString() == "InterNetworkV6")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != "127.0.0.1" && IP4Address != "::1") //String.Empty
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            return IP4Address;
        }*/
    }
}
