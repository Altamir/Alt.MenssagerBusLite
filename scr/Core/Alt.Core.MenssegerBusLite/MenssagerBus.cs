using Alt.Core.MenssegerBusLite.Attributes;
using Alt.Core.MenssegerBusLite.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
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
#if NET45
                new Thread(new ThreadStart(() =>
                {

                    Parallel.ForEach(result.ToList(), item =>
                    {
                        item.Handler.HandleEvent(menssegerData);
                    });

                })).Start();
#elif NETSTANDARD2_0
                new Thread(new ParameterizedThreadStart(a =>
                {
                    Parallel.ForEach(result.ToList(), item =>
                    {
                        item.Handler.HandleEvent(menssegerData);
                    });
                })).Start();
#endif
            }
        }      

        public void Subscrive(object reciver)
        {
            IEnumerable<MethodInfo> metodos = GetMarkedMethods(reciver);
            Type type = reciver.GetType();

            if (this.subscrivers.ContainsKey(type))
            {
                HashSet<MenssagerHandler> values = this.subscrivers[type];
                lock (this.trava)
                {
                    foreach (MethodInfo item in metodos)
                    {
                        values.Add(new MenssagerHandler(reciver, item));
                    }
                }
            }
            else
            {
                HashSet<MenssagerHandler> values = new HashSet<MenssagerHandler>();
                foreach (MethodInfo item in metodos)
                {
                    values.Add(new MenssagerHandler(reciver, item));
                }
                lock (this.trava)
                {
                    this.subscrivers.Add(type, values);
                }
            }
        }

        public void UnSubscriver(object reciver)
        {
            Type type = reciver.GetType();
            if (this.subscrivers.ContainsKey(type))
            {
                lock (this.trava)
                {
                    HashSet<MenssagerHandler> elements = this.subscrivers[type];
                    int coaunrt = elements.RemoveWhere(x => x.Target == reciver);
                }
            }
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