using Alt.Core.MenssegerBusLite.Attributes;
using Alt.Core.MenssegerBusLite.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Alt.Core.MenssegerBusLite
{
    public class MenssagerBus : IMenssegerBusLite
    {

        protected static MenssagerBus instance;
        public static MenssagerBus Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenssagerBus();
                }
                return instance;
            }
        }

        private readonly object trava = new object();

        private readonly Dictionary<Type, HashSet<MenssagerHandler>> subscrivers;

        protected MenssagerBus()
        {
            this.subscrivers = new Dictionary<Type, HashSet<MenssagerHandler>>();
        }



        public void Post(object menssegerData)
        {
            Type type = menssegerData.GetType();

            HashSet<EventWithHandler> result = Get(type);

            if (result.Count > 0)
            {
                Parallel.ForEach(result.ToList(), item =>
                {
                    item.Handler.HandleEvent(menssegerData);
                });

            }
        }



        public void RegisterEventType<T>()
        {
            Type type = typeof(T);
            lock (this.trava)
            {
                if (!this.subscrivers.ContainsKey(type))
                {
                    this.subscrivers.Add(type, new HashSet<MenssagerHandler>());
                }
            }
        }

        public void Subscrive(object reciver)
        {
            IEnumerable<MethodInfo> metodos = GetMarkedMethods(reciver);
            Type type = reciver.GetType();


            if (this.subscrivers.ContainsKey(type))
            {
                HashSet<MenssagerHandler> values = this.subscrivers[type];

                foreach (MethodInfo item in metodos)
                {
                    values.Add(new MenssagerHandler(reciver, item));
                }
            }
            else
            {
                HashSet<MenssagerHandler> values = new HashSet<MenssagerHandler>();
                foreach (MethodInfo item in metodos)
                {
                    values.Add(new MenssagerHandler(reciver, item));
                }

                this.subscrivers.Add(type, values);
            }
        }

        public void UnSubscriver(object reciver)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<MethodInfo> GetMarkedMethods(object @class)
        {
            Type typeOfClass = @class.GetType();
            return typeOfClass.GetMethods().Where<MethodInfo>((method) =>
            {
                Attribute attribute = method.GetCustomAttribute(typeof(Subscribe));
                return attribute == null ? false : true;
            });
        }

        private HashSet<EventWithHandler> Get(Type type)
        {
            HashSet<EventWithHandler> result = new HashSet<EventWithHandler>();
            foreach (Type key in this.subscrivers.Keys)
            {
                HashSet<MenssagerHandler> values = this.subscrivers[key];
                foreach (MenssagerHandler item in values)
                {
                    ParameterInfo[] parametro = item.MethodInfo.GetParameters();
                    if (parametro.Length == 1)
                    {
                        if (parametro[0].ParameterType == type)
                        {
                            result.Add(new EventWithHandler(type, item));
                        }
                    }
                }
            }
            return result;
        }
    }
}