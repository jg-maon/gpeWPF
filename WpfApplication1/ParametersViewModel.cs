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


        ObservableCollection<EditableValue> m_slots;
        /// <summary>
        /// 編集可能パラメータ(表示名と実値)
        /// </summary>
        public ObservableCollection<EditableValue> Slots
        {
            get
            {
                return m_slots = m_slots ?? new ObservableCollection<EditableValue>();
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
                        m_slotProperties.Add(new CustomPropertyDescriptor<EditableValue>(slot.BindableName, slot, typeof(ParametersViewModel)));
                    }
                }
            };

            Slots.CollectionChanged += (sender, e) =>
            {
                if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach(EditableValue item in e.NewItems)
                    {
                        m_slotProperties.Add(new CustomPropertyDescriptor<EditableValue>(item.BindableName, item, typeof(ParametersViewModel)));
                    }
                }
                else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                { 
                    foreach(EditableValue item in e.NewItems)
                    {
                        m_slotProperties.Remove(new CustomPropertyDescriptor<EditableValue>(item.BindableName, item, typeof(ParametersViewModel)));
                    }
                }
                else
                {
                    var a = e.Action;
                }
            };

        }


        private EditableValue _FindValue(ObservableCollection<EditableValue> valueCollection, string name)
        {
            foreach (var value in valueCollection)
            {
                if (value.DispName == name)
                {
                    return value;
                }
                var collection = value.Value as ObservableCollection<EditableValue>;
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


        #region ID詳細用 ParameterTab側からの変更が反映されるように

        #region LightSet
        #region Angle
        bool m_isAngleEventAdded = false;
        public dynamic Angle
        {
            get
            {
                var result = _FindValue(Slots, "Angle");
                if (null != result)
                {
                    if (!m_isAngleEventAdded)
                    {
                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.Angle);
                        m_isAngleEventAdded = true;
                    }
                }
                return result.Value;
            }
            set
            {
                var result = _FindValue(Slots, "Angle");
                if (null != result)
                {
                    result.Value = value;
                }
            }
        }
        #endregion  // Angle
        #region Sharpness
        bool m_isSharpnessEventAdded = false;
        public float Sharpness
        {
            get
            {
                var result = _FindValue(Slots, "Sharpness");
                if (null != result)
                {
                    if (!m_isSharpnessEventAdded)
                    {
                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.Sharpness);
                        m_isSharpnessEventAdded = true;
                    }
                }
                return result.Value;
            }
            set
            {
                var result = _FindValue(Slots, "Sharpness");
                if (null != result)
                {
                    result.Value = value;
                }
            }
        }
        #endregion  // Sharpness
        #region SsaoWeight
        bool m_isSsaoWeightEventAdded = false;
        public float SsaoWeight
        {
            get
            {
                var result = _FindValue(Slots, "SsaoWeight");
                if (null != result)
                {
                    if (!m_isSsaoWeightEventAdded)
                    {
                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.SsaoWeight);
                        m_isSsaoWeightEventAdded = true;
                    }
                }
                return result.Value;
            }
            set
            {
                var result = _FindValue(Slots, "SsaoWeight");
                if (null != result)
                {
                    result.Value = value;
                }
            }
        }
        #endregion  // SsaoWeight
        #region EnableLocalLight1
        bool m_isEnableLocalLight1EventAdded = false;
        public bool EnableLocalLight1
        {
            get
            {
                var result = _FindValue(Slots, "EnableLocalLight1");
                if (null != result)
                {
                    if (!m_isEnableLocalLight1EventAdded)
                    {
                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.EnableLocalLight1);
                        m_isEnableLocalLight1EventAdded = true;
                    }
                }
                return result.Value;
            }
            set
            {
                var result = _FindValue(Slots, "EnableLocalLight1");
                if (null != result)
                {
                    result.Value = value;
                }
            }
        }
        #endregion  // EnableLocalLight1
        #region EnableLocalLight2
        bool m_isEnableLocalLight2EventAdded = false;
        public bool EnableLocalLight2
        {
            get
            {
                var result = _FindValue(Slots, "EnableLocalLight2");
                if (null != result)
                {
                    if (!m_isEnableLocalLight2EventAdded)
                    {
                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.EnableLocalLight2);
                        m_isEnableLocalLight2EventAdded = true;
                    }
                }
                return result.Value;
            }
            set
            {
                var result = _FindValue(Slots, "EnableLocalLight2");
                if (null != result)
                {
                    result.Value = value;
                }
            }
        }
        #endregion  // EnableLocalLight2



        #endregion  // LightSet


        #region Fog

        #region FogHeight
        bool m_isFogHeightEventAdded = false;
        public float FogHeight
        {
            get
            {
                var result = _FindValue(Slots, "FogHeight");
                if (null != result)
                {
                    if (!m_isFogHeightEventAdded)
                    {
                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.FogHeight);
                        m_isFogHeightEventAdded = true;
                    }
                }
                return result.Value;
            }
            set
            {
                var result = _FindValue(Slots, "FogHeight");
                if (null != result)
                {
                    result.Value = value;
                }
            }
        }
        #endregion  // FogHeight
        #region check1
        bool m_ischeck1EventAdded = false;
        public bool check1
        {
            get
            {
                var result = _FindValue(Slots, "check1");
                if (null != result)
                {
                    if (!m_ischeck1EventAdded)
                    {
                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.check1);
                        m_ischeck1EventAdded = true;
                    }
                }
                return result.Value;
            }
            set
            {
                var result = _FindValue(Slots, "check1");
                if (null != result)
                {
                    result.Value = value;
                }
            }
        }
        #endregion  // check1


        #endregion  // Fog

        #endregion


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
