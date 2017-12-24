using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOAPServer
{
    interface IServer
    {
        /// <summary>
        /// Запуск 
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка
        /// </summary>
        void Stop();
    }
}
