using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy.Utility
{
    class RangeIndicator : Mode
    {
        public override void CreateMenu()
        {
            
        }

        public override bool DefaultEnabled()
        {
            return false;
        }

        public override string GetID()
        {
            return "Range Indicator";
        }

        public override bool Init()
        {
            return true;
        }

        public override void Stop()
        {
            
        }
    }
}
