using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class LightSetViewModel : ParameterViewModelBase
    {
        ParamLightSet m_parameter = new ParamLightSet();

        private ObservableCollection<ParameterNode> m_parameterCollection = new ObservableCollection<ParameterNode>();
        private ReadOnlyObservableCollection<ParameterNode> m_readonlyParameterCollection = null;
        public override ReadOnlyObservableCollection<ParameterNode> ParameterCollection
        {
            get
            {
                if (null == m_readonlyParameterCollection)
                {
                    m_readonlyParameterCollection = new ReadOnlyObservableCollection<ParameterNode>(m_parameterCollection);
                }
                return m_readonlyParameterCollection;
            }
        }

        public override string CategoryName
        {
            get
            {
                return "LightSet";
            }            
        }

        public override uint ID
        {
            get
            {
                return m_parameter.id;
            }
            set
            {
                if(value != ID)
                {
                    m_parameter.id = value;
                    RaisePropertyChanged("ID");
                }
            }
        }
        public override string Comment
        {
            get
            {
                return m_parameter.comment;
            }
            set
            {
                if(value != Comment)
                {
                    m_parameter.comment = value;
                    RaisePropertyChanged("Comment");
                }
            }
        }


        public ParamLightSet.LightInfo DirLight0
        {
            get
            {
                return m_parameter.DirLight0;
            }
            set
            {
                if (value != m_parameter.DirLight0)
                {
                    m_parameter.DirLight0 = value;
                    RaisePropertyChanged("DirLight0");
                }
            }
        }

        public ParamLightSet.LightInfo DirLight1
        {
            get
            {
                return m_parameter.DirLight1;
            }
            set
            {
                if (value != m_parameter.DirLight1)
                {
                    m_parameter.DirLight1 = value;
                    RaisePropertyChanged("DirLight1");
                }
            }
        }

        public ParamLightSet.LightInfo LocalLight1
        {
            get
            {
                return m_parameter.LocalLight1;
            }
            set
            {
                if (value != m_parameter.LocalLight1)
                {
                    m_parameter.LocalLight1 = value;
                    RaisePropertyChanged("LocalLight1");
                }
            }
        }

        public ParamLightSet.LightInfo LocalLight2
        {
            get
            {
                return m_parameter.LocalLight2;
            }
            set
            {
                if (value != m_parameter.LocalLight2)
                {
                    m_parameter.LocalLight2 = value;
                    RaisePropertyChanged("LocalLight2");
                }
            }
        }
        public bool EnableLocalLight1
        {
            get
            {
                return m_parameter.EnableLocalLight1;
            }
            set
            {
                if (value != m_parameter.EnableLocalLight1)
                {
                    m_parameter.EnableLocalLight1 = value;
                    RaisePropertyChanged("EnableLocalLight1");
                }
            }
        }

        public bool EnableLocalLight2
        {
            get
            {
                return m_parameter.EnableLocalLight2;
            }
            set
            {
                if (value != m_parameter.EnableLocalLight2)
                {
                    m_parameter.EnableLocalLight2 = value;
                    RaisePropertyChanged("EnableLocalLight2");
                }
            }
        }

        public ColorRGBI HemiColorUp
        {
            get
            {
                return m_parameter.HemiColorUp;
            }
            set
            {
                if (value != m_parameter.HemiColorUp)
                {
                    m_parameter.HemiColorUp = value;
                    RaisePropertyChanged("HemiColorUp");
                }
            }
        }
        public ColorRGBI HemiColorDown
        {
            get
            {
                return m_parameter.HemiColorDown;
            }
            set
            {
                if (value != m_parameter.HemiColorDown)
                {
                    m_parameter.HemiColorDown = value;
                    RaisePropertyChanged("HemiColorDown");
                }
            }
        }
        public LightSetViewModel()
        {
            Type t = typeof(LightSetViewModel);
            var properties = t.GetProperties();
            foreach(var property in properties)
            {
                var p = property.GetValue(this);
                if(p == ParameterCollection)
                {
                    continue;
                }
                string s = p as string;
                if(null!=s && s==CategoryName)
                {
                    continue;
                }
                m_parameterCollection.Add(new ParameterNode(p, property.Name, property.PropertyType));
            }
        }
    }
}
