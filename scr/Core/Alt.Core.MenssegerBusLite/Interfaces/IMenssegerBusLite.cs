using System;

namespace Alt.Core.MenssegerBusLite.Interfaces
{
    public interface IMenssegerBusLite
    {
        void RegisterEventType<T>();

        void Subscrive(object reciver);
        void UnSubscriver(object reciver);
        void Post(object menssegerData);
    }
}
