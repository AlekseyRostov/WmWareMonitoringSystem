using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Contract.Exceptions;
using log4net;

namespace SOAPServer.Configuration
{
    internal class WCFErrorHandler : IErrorHandler, IServiceBehavior
    {
        private readonly ILog _logger;

        public WCFErrorHandler(ILog logger)
        {
            _logger = logger;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is FaultException)
            {
                return;
            }

            ExceptionDTO dtoEx = new ExceptionDTO();
            dtoEx.Exception = error;
            dtoEx.Message = error.Message;

            FaultException<ExceptionDTO> faultException = new FaultException<ExceptionDTO>(dtoEx, error.Message);
            MessageFault faultMessage = faultException.CreateMessageFault();
            fault = Message.CreateMessage(version, faultMessage, "http://tempuri.org/IService/FaultException");
        }

        public bool HandleError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            throw new NotImplementedException();
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler = new WCFErrorHandler(_logger);

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;

                if (channelDispatcher != null)
                {
                    channelDispatcher.ErrorHandlers.Add(errorHandler);
                }
            }
        }
    }
}