using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    interface IEditableValue<ValueType>
    {
        string Name { get; set; }
        ValueType Value { get; set; }
        
    }
}
