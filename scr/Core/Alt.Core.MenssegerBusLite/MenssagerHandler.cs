using System;
using System.Reflection;

namespace Alt.Core.MenssegerBusLite
{
    public class MenssagerHandler
    {
        public object Target { get; protected set; }
        public MethodInfo MethodInfo { get; protected set; }

        public MenssagerHandler(object target, MethodInfo methodInfo)
        {
            this.Target = target;
            this.MethodInfo = methodInfo;
        }

        public void HandleEvent(object @event)
        {
            try
            {
                MethodInfo.Invoke(Target, new Object[] { @event });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is MenssagerHandler that)
            {
                return Target == that.Target && MethodInfo.Equals(that.MethodInfo);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return MethodInfo.GetHashCode() ^ Target.GetHashCode();
        }
    }
}
