using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy
{
    abstract class Mode
    {
        public abstract void Init();
        public abstract void Stop();
        //public abstract string GetID();
    }
}
