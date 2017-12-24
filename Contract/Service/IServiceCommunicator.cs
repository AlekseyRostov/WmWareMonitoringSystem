using System;
using System.Collections.Generic;

namespace Contract.Service
{
    /// <summary>
    ///     Интерфейс класса для взаимодействия с клиентами
    /// </summary>
    public interface IServiceCommunicator
    {
        /// <summary>
        ///     Добавить клиент
        /// </summary>
        /// <param name="userGuid">идентификатор пользователя</param>
        /// <param name="clientGuid">идентификатор клиента</param>
        /// <param name="computerName"></param>
        /// <param name="clientCallback">канал для связи с клиентом</param>
        void KeepClient(Guid userGuid, Guid clientGuid, string computerName, IServiceCallback clientCallback);

        /// <summary>
        ///     Добавить клиент
        /// </summary>
        /// <param name="userGuid">идентификатор пользователя</param>
        /// <param name="clientGuid">идентификатор клиента</param>
        /// ///
        /// <param name="computerName">имя компьютера клиента</param>
        /// <param name="clientCallback">канал для связи с клиентом</param>
        void AddClient(Guid userGuid, Guid clientGuid, string computerName, IServiceCallback clientCallback);

        /// <summary>
        ///     Получение идентификатора клиента
        /// </summary>
        /// <param name="clientCallback">канал для связи с клиентом</param>
        /// <returns></returns>
        Guid GetClientGuid(IServiceCallback clientCallback);

        /// <summary>
        ///     Получение идентификатора пользователя
        /// </summary>
        /// <param name="clientCallback">канал для связи с клиентом</param>
        /// <returns></returns>
        Guid GetUserGuid(IServiceCallback clientCallback);        
       
    }
}