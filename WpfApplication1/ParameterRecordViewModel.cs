using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
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
    [Serializable]
    public class ParameterRecordViewModel : ViewModelBase, ICustomTypeDescriptor, ISerializable
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

        IEditableValue[] m_innerSlots;

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

        public event EventHandler<IEditableValue> ValueChanging;

        protected void RaiseValueChanging(IEditableValue value)
        {
            if (null != ValueChanging)
            {
                ValueChanging(this, value);
            }
        }
        public event EventHandler<IEditableValue> ValueChanged;

        protected void RaiseValueChanged(IEditableValue value)
        {
            if(null != ValueChanged)
            {
                ValueChanged(this, value);
            }
        }

        public ParameterRecordViewModel(string categoryName)
        {
            m_categoryName = categoryName;

            PropertyChanged += (sender, e) =>
            {
                if(e.PropertyName == "Slots")
                {
                    foreach(var slot in Slots)
                    {
                        if (null != slot)
                        {
                            slot.ValueChanging -= slot_ValueChanging;
                            slot.ValueChanging += slot_ValueChanging;
                            slot.ValueChanged -= slot_ValueChanged;
                            slot.ValueChanged += slot_ValueChanged;
                            m_slotProperties.Add(new CustomPropertyDescriptor<IEditableValue>(slot.BindableName, slot, typeof(ParameterRecordViewModel)));
                        }

                    }
                }
            };

            Slots.CollectionChanged += (sender, e) =>
            {
                if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach(IEditableValue item in e.NewItems)
                    {
                        item.ValueChanging -= slot_ValueChanging;
                        item.ValueChanging += slot_ValueChanging;
                        item.ValueChanged -= slot_ValueChanged;
                        item.ValueChanged += slot_ValueChanged;
                        m_slotProperties.Add(new CustomPropertyDescriptor<IEditableValue>(item.BindableName, item, typeof(ParameterRecordViewModel)));
                    }
                }
                else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                { 
                    foreach(IEditableValue item in e.OldItems)
                    {
                        m_slotProperties.Remove(new CustomPropertyDescriptor<IEditableValue>(item.BindableName, item, typeof(ParameterRecordViewModel)));
                    }
                }
                else
                {
                    var a = e.Action;
                }
            };

        }

        void slot_ValueChanging(object sender, CancelEventArgs e)
        {
            RaiseValueChanging(sender as IEditableValue);
        }

        void slot_ValueChanged(object sender, EventArgs e)
        {

            RaiseValueChanged(sender as IEditableValue);
        }

        
        private ParameterRecordViewModel(SerializationInfo info, StreamingContext context)
        {
            //m_slotProperties = (List<PropertyDescriptor>)info.GetValue("m_slotProperties", typeof(List<PropertyDescriptor>));
            m_categoryName = info.GetString("m_categoryName");
            m_id = info.GetInt32("m_id");
            m_name = info.GetString("m_name");
            m_comment = info.GetString("m_comment");
            m_slots = (ObservableCollection<IEditableValue>)info.GetValue("m_slots", typeof(ObservableCollection<IEditableValue>));

        }
        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            //info.AddValue("m_slotProperties", m_slotProperties);
            info.AddValue("m_categoryName", m_categoryName);
            info.AddValue("m_id", m_id);
            info.AddValue("m_name", m_name);
            info.AddValue("m_comment", m_comment);
            info.AddValue("m_slots", m_slots, typeof(ObservableCollection<IEditableValue>));
        }

        public ParameterRecordViewModel CreateCopy()
        {
            var ret = new ParameterRecordViewModel(this.CategoryName)
            { 
                ID = this.ID,
                Name = this.Name,
                Comment = this.Comment,
                Slots = DeepCopyExtensions.DeepCopy(Slots)
            };

            foreach (var slot in ret.Slots)
            {
                if(null != slot)
                    ret.m_slotProperties.Add(new CustomPropertyDescriptor<IEditableValue>(slot.BindableName, slot, typeof(ParameterRecordViewModel)));
            }

            return ret;
        }
        [OnDeserialized]
        private void _SetValuesOnDesesrialized(StreamingContext context)
        {
            Slots = new ObservableCollection<IEditableValue>(this.m_innerSlots);
            m_innerSlots = null;
            foreach (var slot in Slots)
            {
                if (null != slot)
                    m_slotProperties.Add(new CustomPropertyDescriptor<IEditableValue>(slot.BindableName, slot, typeof(ParameterRecordViewModel)));
            }
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
