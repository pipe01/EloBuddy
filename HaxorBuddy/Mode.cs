using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy
{
    abstract class Mode
    {
        public abstract bool Init();
        public abstract void Stop();
        public abstract string GetID();
        public abstract bool DefaultEnabled();
        public abstract void CreateMenu();

        public static Dictionary<string, Type> GetAllModes()
        {
            var list = new Dictionary<string, Type>();
            var t = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var item in t)
            {
                if (item.BaseType != typeof(Mode)) continue;
                Console.WriteLine(item.Name);
                var c = Activator.CreateInstance(item);
                var m = item.GetMethod("GetID");
                if (m != null)
                    list.Add((string)m.Invoke(c, null), item);
            }
            return list;
        }
    }
}
