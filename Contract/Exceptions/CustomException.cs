using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contract.Exceptions
{
    [Serializable]
    public class CustomException : CommunicationException
    {
        [NonSerialized]
        protected readonly ILog Logger = null;
        public CustomException()
            : base()
        {
            Logger = log4net.LogManager.GetLogger(this.GetType());
            LogError("(текст исключения отсутствует)");
        }

        public CustomException(string message)
            : base(message)
        {
            Logger = log4net.LogManager.GetLogger(this.GetType());
            LogError(message);
        }

        public CustomException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            Logger = log4net.LogManager.GetLogger(this.GetType());
            LogError(format, args);
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {
            Logger = log4net.LogManager.GetLogger(this.GetType());
            LogError(message);
        }

        public CustomException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            Logger = log4net.LogManager.GetLogger(this.GetType());
            LogError(format, args);
        }

        /// <summary>
        /// Для десериалиации
        /// Перегрузка данного метода обязательна, если хотите передавать исключение по сети
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        protected void LogError(string message)
        {
            if (Logger.IsErrorEnabled)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('=', 80);
                sb.AppendLine();
                sb.AppendLine("ВЫЗВАНО ИСКЛЮЧЕНИЕ");
                sb.AppendLine(message);
                sb.AppendLine();
                sb.Append('=', 80);
                sb.AppendLine();

                Logger.Error(sb.ToString(), this);
            }
        }

        protected void LogError(string format, params object[] args)
        {
            LogError(string.Format(format, args));
        }
    }
}