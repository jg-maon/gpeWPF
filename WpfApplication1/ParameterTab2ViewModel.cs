using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ParameterTab2ViewModel : ToolViewModel , IEquatable<ParameterTab2ViewModel>
    {
        private readonly CategoryTreePaneViewModel m_file;
        public CategoryTreePaneViewModel File
        {
            get
            {
                return m_file;
            }
        }

        public override string Title
        {
            get
            {
                return Parameters.CategoryName + ":ID: " + Parameters.ID + " - " + Path.GetFileNameWithoutExtension(File.Title);
            }
        }

        private readonly ParametersViewModel m_parameter;
        public ParametersViewModel Parameters
        {
            get
            {
                return m_parameter;
            }
        }

        private bool m_isInfoGroupExpanded = true;
        public bool IsInfoGroupExpanded
        {
            get
            {
                return m_isInfoGroupExpanded;
            }
            set
            {
                SetProperty(ref m_isInfoGroupExpanded, value);
            }
        }


        private string m_searchText;
        public string SearchText
        {
            get
            {
                return m_searchText;
            }
            set
            {
                SetProperty(ref m_searchText, value);
            }
        }


        public ParameterTab2ViewModel(CategoryTreePaneViewModel categoryTreePaneViewModel, ParametersViewModel parameterIdViewModel)
            : base("")
        {
            m_file = categoryTreePaneViewModel;

            m_parameter = parameterIdViewModel;


        }
        public bool Equals(ParameterTab2ViewModel other)
        {
            if (m_file != other.m_file)
            { return false; }
            if (m_parameter != other.m_parameter)
            {
                return false;
            }
            return true;
        }

    }
}
