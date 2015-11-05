using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication1
{
    public class ParameterNode : ViewModelBase
    {
        private object m_object;
        public object Object
        {
            get
            { return m_object; }
            set
            {
                if (value != m_object)
                {
                    m_object = value;
                    RaisePropertyChanged("Object");
                }
            }
        }
        private string m_name;
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (value != m_name)
                {
                    m_name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        private Type m_type;
        public Type Type
        {
            get
            {
                return m_type;
            }
            set
            {
                if (value != m_type)
                {
                    m_type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }
        public ParameterNode(object param, string name, Type t)
        {
            Object = param;
            Name = name;
            if(param==null)
            {
                Type = t;
                return;
            }
            Type = param.GetType();
        }
    }
    public class ParameterViewModelBase : ViewModelBase
    {
        public virtual string CategoryName { get; set; }

        public virtual uint ID
        {
            get
            {
                throw new NotImplementedException("ID getter"); 
            }
            set
            {
                throw new NotImplementedException("ID setter"); 
            }
        }
        public virtual string Comment
        {
            get
            {
                throw new NotImplementedException("Comment getter");
            }
            set
            {
                throw new NotImplementedException("Comment setter");
            }
        }

        public virtual ReadOnlyObservableCollection<ParameterNode> ParameterCollection { get; set; }

        private string m_name="";
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if(value != m_name)
                {
                    m_name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        private uint? m_parentId = null;
        public uint? ParentId
        {
            get
            {
                return m_parentId;
            }
            set
            {
                if(value != m_parentId)
                {
                    m_parentId = value;
                    RaisePropertyChanged("ParentId");
                }
            }
        }

        private DateTime m_timeStamp = new DateTime();
        public DateTime TimeStamp
        {
            get
            {
                return m_timeStamp;
            }
            set
            {
                if(value != m_timeStamp)
                {
                    m_timeStamp = value;
                    RaisePropertyChanged("DateTime");
                }
            }
        }

        private string m_log = "";
        public string Log
        {
            get
            {
                return m_log;
            }
            set
            {
                if(value != m_log)
                {
                    m_log = value;
                    RaisePropertyChanged("Log");
                }
            }
        }






    }
}
