using System;

namespace Punch.Utils
{
    public
        static class TypeSwitch
    {
        public class CaseInfo
        {
            public bool IsDefault { get; set; }
            public Type Target { get; set; }
            public Action<object> Action { get; set; }
        }

        public static void Do(object source, params CaseInfo[] cases)
        {
            var type = source.GetType();
            foreach (var entry in cases)
            {
                if (entry.IsDefault || type == entry.Target)
                {
                    entry.Action(source);
                    break;
                }
            }
        }

        public static CaseInfo Case<TC>(Action action)
        {
            return new CaseInfo
                       {
                           Action = x => action(),
                           Target = typeof(TC)
                       };
        }

        public static CaseInfo Case<TC>(Action<TC> action)
        {
            return new CaseInfo
                       {
                           Action = (x) => action((TC)x),
                           Target = typeof(TC)
                       };
        }

        public static CaseInfo Default(Action action)
        {
            return new CaseInfo
                       {
                           Action = x => action(),
                           IsDefault = true
                       };
        }
    }
}