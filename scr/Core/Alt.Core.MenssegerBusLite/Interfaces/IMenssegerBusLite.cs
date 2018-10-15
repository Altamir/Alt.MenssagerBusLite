namespace Alt.Core.MenssegerBusLite.Interfaces
{
    public interface IMenssegerBusLite
    {
        void Subscrive(object reciver);
        void UnSubscriver(object reciver);
        void Post(object menssegerData);
    }
}
