using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Contract.Service;
using log4net;
using SOAPServer.Configuration;
using SOAPServer.Properties;

namespace SOAPServer
{
    public class ControlServer : IServer, IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ControlServer));

        private readonly IServiceListener _service;
        private ServiceHost _clientServiceHost;

        public ControlServer()
        {
            _service = DocsServer.GetSingleton();
        }

        public void Start()
        {
            Func<Task> _startFunc = () => _start();
            // используем отложенный старт службы, чтобы служба стартовала
            // но в то же время можно было бесконечно долго проверять соединение с БД
            // new Timer(_start, null, 10, 0);
            Task.Run(_startFunc);
        }

        private async Task _start()
        {
            _log.Debug("Запуск сервера в интерактивном режиме");

            //InitIoC();
            
            //// после успешного соединения инициализируем общие настройки, если таковые не были проиницилизированы ранее
            //SettingsStore.InitializeCommonSettings();
            //SettingsStore.CheckConfigSettings();

            //using (var db = new Database())
            //{
            //    DataCache.InitDataCache(db);
            //}

            // еще нет соединения с клиентами - нужно организовать связь
            var binding = BindingHelper.GetTcpBinding();
            var baseAddress = new Uri(Settings.Default.BaseAddress);

            _clientServiceHost = new ServiceHost(_service, baseAddress);
            _clientServiceHost.AddServiceEndpoint(typeof(IService), binding, baseAddress);

            if (binding.Security.Mode == SecurityMode.Transport)
                InitializeCertificate();

            foreach (var endPoint in _clientServiceHost.Description.Endpoints)
                endPoint.Behaviors.Add(new ServiceEndpointBehavior());

            _clientServiceHost.Description.Behaviors.Add(new WCFErrorHandler(_log));
            _clientServiceHost.Open();
            
        }

        private void InitializeCertificate()
        {
            var certificatePath = AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\server.pfx";

            _clientServiceHost.Credentials.ServiceCertificate.Certificate = new X509Certificate2(certificatePath, string.Empty, X509KeyStorageFlags.MachineKeySet);
            _clientServiceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
