using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ParameterTabViewModel : ToolViewModel, IEquatable<ParameterTabViewModel>
    {

        readonly CategoryTreePaneViewModel m_file;
        public CategoryTreePaneViewModel File
        {
            get
            {
                return m_file;
            }
        }

        readonly ParameterViewModelBase m_parameter;
        public ParameterViewModelBase Parameter
        {
            get
            {
                return m_parameter;
            }
        }

        public ReadOnlyObservableCollection<ParameterNode> ParameterCollection
        {
            get
            {
                return m_parameter.ParameterCollection;
            }
        }

        public ParameterTabViewModel(CategoryTreePaneViewModel file, ParameterViewModelBase parameter)
            : base(parameter.CategoryName + ":ID: " + parameter.ID + " - " + file.Title)
        {
            m_file = file;
            m_parameter = parameter;
        }

        public bool Equals(ParameterTabViewModel other)
        {
            if(m_file != other.m_file)
            { return false;}
            if(m_parameter != other.m_parameter)
            {
                return false;
            }
            return true;
        }
    }
}
