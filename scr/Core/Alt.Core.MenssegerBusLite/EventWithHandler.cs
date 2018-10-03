using System;

namespace Alt.Core.MenssegerBusLite
{
    public class EventWithHandler
    {
        public EventWithHandler(Type @event, MenssagerHandler handler)
        {
            this.Event = @event;
            this.Handler = handler;
        }
      
        public Type Event { get; protected set; }       
        public MenssagerHandler Handler { get; protected set; }
    }
}
