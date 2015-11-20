using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ParameterTab2ViewModel : ToolViewModel , IEquatable<ParameterTab2ViewModel>
    {
        public ParameterTab2ViewModel()
            :base("aa")
        {

        }
        public bool Equals(ParameterTab2ViewModel other)
        {
            return false;
        }
    }
}
