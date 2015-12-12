using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{

    /// <summary>
    /// IDとそのIDで登録されるパラメータ群
    /// </summary>
    /// <remarks>
    /// ID1つあたりの情報
    /// </remarks>
    public class ParametersViewModel : ViewModelBase, ICustomTypeDescriptor
    {
        List<PropertyDescriptor> m_slotProperties = new List<PropertyDescriptor>();

        private readonly string m_categoryName;
        public string CategoryName
        {
            get
            {
                return m_categoryName;
            }
        }

        private int m_id;
        public int ID
        {
            get
            {
                return m_id;
            }
            set
            {
                SetProperty(ref m_id, value);
            }
        }

        private string m_name;
        /// <summary>
        /// 固有名
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                this.SetProperty(ref m_name, value);
            }
        }

        private string m_comment;
        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get
            {
                return m_comment;
            }
            set
            {
                this.SetProperty(ref m_comment, value);
            }
        }


        ObservableCollection<IEditableValue> m_slots;
        /// <summary>
        /// 編集可能パラメータ(表示名と実値)
        /// </summary>
        public ObservableCollection<IEditableValue> Slots
        {
            get
            {
                return m_slots = m_slots ?? new ObservableCollection<IEditableValue>();
            }
            set
            {
                SetProperty(ref m_slots, value);
            }
        }

        public ParametersViewModel(string categoryName)
        {
            m_categoryName = categoryName;

            PropertyChanged += (sender, e) =>
            {
                if(e.PropertyName == "Slots")
                {
                    foreach(var slot in Slots)
                    {
                        m_slotProperties.Add(new CustomPropertyDescriptor<IEditableValue>(slot.BindableName, slot, typeof(ParametersViewModel)));
                    }
                }
            };

            Slots.CollectionChanged += (sender, e) =>
            {
                if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach(IEditableValue item in e.NewItems)
                    {
                        m_slotProperties.Add(new CustomPropertyDescriptor<IEditableValue>(item.BindableName, item, typeof(ParametersViewModel)));
                    }
                }
                else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                { 
                    foreach(IEditableValue item in e.NewItems)
                    {
                        m_slotProperties.Remove(new CustomPropertyDescriptor<IEditableValue>(item.BindableName, item, typeof(ParametersViewModel)));
                    }
                }
                else
                {
                    var a = e.Action;
                }
            };

        }


        private IEditableValue _FindValue(ObservableCollection<IEditableValue> valueCollection, string name)
        {
            foreach (var value in valueCollection)
            {
                if (value.DispName == name)
                {
                    return value;
                }
                var collection = value.Value as ObservableCollection<IEditableValue>;
                if (null != collection)
                {
                    var result = _FindValue(collection, name);
                    if (null != result)
                    {
                        return result;
                    }
                }
            }
            return null;
        }



        #region ICustomTypeDescriptor メンバー

        public AttributeCollection GetAttributes()
        {
            throw new NotImplementedException();
        }

        public string GetClassName()
        {
            throw new NotImplementedException();
        }

        public string GetComponentName()
        {
            throw new NotImplementedException();
        }

        public TypeConverter GetConverter()
        {
            throw new NotImplementedException();
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(m_slotProperties.ToArray());
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
