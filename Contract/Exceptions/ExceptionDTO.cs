using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Contract.Exceptions
{
    [DataContract]
    [KnownType("GetTypes")]
    public class ExceptionDTO
    {
        [DataMember]
        public Exception Exception { get; set; } = new Exception();

        [DataMember]
        public string Message { get; set; }

        static Type[] GetTypes()
        {
            return new Type[]
                   {
                       typeof(IDictionary).Assembly.GetType("System.Collections.ListDictionaryInternal"),
                       typeof(Exception),
                   };
        }
    }
}