using System;
using System.ServiceModel;

namespace SOAPServer.Configuration
{
    public static class BindingHelper
    {
        public static NetTcpBinding GetTcpBinding()
        {
            var b = new NetTcpBinding
            {
                Security =
                {
                    Message = {ClientCredentialType = MessageCredentialType.None},
                    Transport = {ClientCredentialType = TcpClientCredentialType.None},
                    Mode = SecurityMode.None
                },
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas = 
                {
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                    MaxDepth = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                },

                CloseTimeout = new TimeSpan(0, 0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0)
            };

            return b;
        }
    }
}