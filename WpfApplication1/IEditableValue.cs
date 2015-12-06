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
    /// パラメータ
    /// </summary>
    public class EditableValue : ViewModelBase
    {

        private string m_bindableName;
        public string BindableName
        {
            get { return m_bindableName; }
            set
            {
                SetProperty(ref m_bindableName, value);
            }
        }

        private string m_dispName;
        /// <summary>
        /// パラメータ名(Angle的な)
        /// </summary>
        public string DispName 
        {
            get
            {
                return m_dispName;
            }
            set
            {
                SetProperty(ref m_dispName, value);
            }
        }

        private string m_name;
        /// <summary>
        /// パラメータ固有名(DirLight0Angle的な)
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                SetProperty(ref m_name, value);
            }
        }

        private int m_type;
        /// <summary>
        /// パラメータのタイプ(gparamxmlから引っ張ってきた値そのまま)
        /// </summary>
        public int Type 
        {
            get
            {
                return m_type;
            }
            set
            {
                SetProperty(ref m_type, value);
            }
        }


        private dynamic m_value;
        public dynamic Value
        {
            get
            {
                return m_value;
            }
            set
            {
                this.SetProperty(ref m_value, value, "Value");
            }
        }

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get
            {
                return m_isExpanded;
            }
            set
            {
                this.SetProperty(ref m_isExpanded, value);
            }
        }

        private int m_tabIndex = 0;
        public int TabIndex
        {
            get
            {
                return m_tabIndex;
            }
            set
            {
                this.SetProperty(ref m_tabIndex, value);
            }
        }

        private string m_filter;
        public string Filter
        {
            get
            {
                return m_filter;
            }
            set
            {
                this.SetProperty(ref m_filter, value);
            }
        }

        private bool m_isDirty = false;
        /// <summary>
        /// パラメータの値が変更されたか
        /// </summary>
        public virtual bool IsDirty
        {
            get
            {
                return m_isDirty;
            }
            set
            {
                this.SetProperty(ref m_isDirty, value);
            }
        }

    }

    public class EditableValueGroup : EditableValue, System.ComponentModel.ICustomTypeDescriptor
    {

        private List<System.ComponentModel.PropertyDescriptor> m_extendProperties = new List<System.ComponentModel.PropertyDescriptor>();

        public EditableValueGroup()
        {
            var collection = new ObservableCollection<EditableValue>();
            Value = collection;
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Value")
                {
                    foreach(EditableValue value in Value)
                    {
                        m_extendProperties.Add(new CustomPropertyDescriptor<EditableValue>(value.BindableName, value, typeof(EditableValueGroup)));
                    }
                }
            };
            collection.CollectionChanged += (sender, e) =>
            {

                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (EditableValue item in e.NewItems)
                    {
                        m_extendProperties.Add(new CustomPropertyDescriptor<EditableValue>(item.BindableName, item, typeof(EditableValueGroup)));
                    }
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    foreach (EditableValue item in e.NewItems)
                    {
                        m_extendProperties.Remove(new CustomPropertyDescriptor<EditableValue>(item.BindableName, item, typeof(EditableValueGroup)));
                    }
                }
                else
                {
                    var a = e.Action;
                }
            };
        }

        /// <summary>
        /// パラメータの値が変更されたか
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                // 自身に登録されている子の変更状態を確認
                foreach (var value in this.Value)
                {
                    // 一つでも変更されているものがあれば
                    if (value.IsDirty)
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
                // 自身に登録されている子の変更状態を設定
                foreach (var v in this.Value)
                {
                    v.Value = value;
                }
            }
        }

        #region ICustomTypeDescriptor メンバー

        public System.ComponentModel.AttributeCollection GetAttributes()
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

        public System.ComponentModel.TypeConverter GetConverter()
        {
            throw new NotImplementedException();
        }

        public System.ComponentModel.EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public System.ComponentModel.PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public System.ComponentModel.EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public System.ComponentModel.EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public System.ComponentModel.PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public System.ComponentModel.PropertyDescriptorCollection GetProperties()
        {
            Type t = typeof(EditableValueGroup);
            var properties = t.GetProperties();

           
            return new System.ComponentModel.PropertyDescriptorCollection(
                                (m_extendProperties).ToArray());
        }

        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    /// <summary>
    /// カテゴリ1つあたりの情報
    /// </summary>
    /// <remarks>
    /// Slot配列 + Edited(ID配列)
    /// ParamSet配列は外部で定義
    /// </remarks>
    /// <see cref="ObservableCollection ParameterCollection"/>
    public class ParameterCollectionViewModel : ViewModelBase
    {
        

        private string m_name;
        /// <summary>
        /// カテゴリ名
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                SetProperty(ref m_name, value);
            }
        }
        private string m_dispName;
        /// <summary>
        /// カテゴリ名
        /// </summary>
        public string DispName
        {
            get
            {
                return m_dispName;
            }
            set
            {
                SetProperty(ref m_dispName, value);
            }
        }

        private string m_bindableName;
        public string BindableName
        {
            get { return m_bindableName; }
            set
            {
                SetProperty(ref m_bindableName, value);
            }
        }


        private ObservableCollection<ParametersViewModel> m_parameters = null;
        /// <summary>
        /// ID群
        /// </summary>
        public ObservableCollection<ParametersViewModel> Parameters
        {
            get
            {
                return m_parameters = m_parameters ?? new ObservableCollection<ParametersViewModel>();
            }
            set
            {
                SetProperty(ref m_parameters, value);
            }
        }

        /// <summary>
        /// 情報が空のIDの作成
        /// </summary>
        /// <returns>
        /// 作成したID
        /// </returns>
        public ParametersViewModel CreateId()
        {
            return new ParametersViewModel(DispName);
        }

        /// <summary>
        /// 情報付きIDの作成
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">固有名</param>
        /// <param name="comment">コメント</param>
        /// <returns>
        /// 作成したID
        /// </returns>
        public ParametersViewModel CreateId(int id, string name, string comment)
        {
            return new ParametersViewModel(DispName) { ID = id, Name = name, Comment = comment };
        }
    }


    /// <summary>
    /// ParamSet配例
    /// </summary>
    public class _Collection : ObservableCollection<ParameterCollectionViewModel>
    { }

}
