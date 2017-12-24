using System;

namespace Contract.Service
{
    public interface IServiceListener
    {
        IServiceCommunicator Communicator { get; }

        /// <summary>
        ///     Событие запроса имени сервера
        /// </summary>
        event EventHandler ServerNameRequested;
    }
}