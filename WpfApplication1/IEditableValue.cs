using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public interface IEditableValue
    {
        dynamic Value { get; set; }
    }
    /// <summary>
    /// パラメータ
    /// </summary>
    /// <typeparam name="ValueType"></typeparam>
    public class EditableValue<ValueType> : ViewModelBase, IEditableValue
    {
        /// <summary>
        /// パラメータ名(DirLight0Angle的な)
        /// </summary>
        private string m_name;
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


        private ValueType m_value;
        public dynamic Value
        {
            get
            {
                return m_value;
            }
            set
            {
                this.SetProperty(ref m_value, value);
            }
        }
    }

    /// <summary>
    /// IDとそのIDで登録されるパラメータ群
    /// </summary>
    /// <remarks>
    /// ID1つあたりの情報
    /// </remarks>
    public class ParametersViewModel : ViewModelBase
    {
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
    }

    /// <summary>
    /// Slot配列 + Edited(ID配列)
    /// </summary>
    /// <remarks>
    /// カテゴリ1つあたりの情報
    /// ParamSet配列は外部でObservableCollection<ParameterCollection>で定義
    /// </remarks>
    public class ParameterCollectionViewModel : ViewModelBase
    {
        /// <summary>
        /// カテゴリ名
        /// </summary>
        private string m_name;
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

        ObservableCollection<ParametersViewModel> m_parameters;
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

    }


    /// <summary>
    /// ParamSet配例
    /// </summary>
    public class _Collection : ObservableCollection<ParameterCollectionViewModel>
    { }

}
