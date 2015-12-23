using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication1
{
    public interface ISliderValue
    {
        ReadOnlyObservableCollection<RangeInfo> Units { get; }
        RangeInfo ActiveUnit { get; set; }
        bool CanChangeUnit { get; }
        ICommand ChangeUnitCommand { get; }
    }
    /// <summary>
    /// スライダー用
    /// </summary>
    /// <typeparam name="ValueType">値の型</typeparam>
    [Serializable]
    class SliderValue<ValueType> : TUnitValue<ValueType>, ISliderValue, ISerializable
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sliderInfo">スライダー情報</param>
        public SliderValue(SliderInfo sliderInfo)
        {
            m_units = new ObservableCollection<RangeInfo>(sliderInfo.Units);
            ActiveUnit = m_units.FirstOrDefault();
        }

        protected SliderValue(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_innerUnits = (RangeInfo[])info.GetValue("m_innerUnits", typeof(RangeInfo[]));
            m_activeUnit = (RangeInfo)info.GetValue("m_activeUnit", typeof(RangeInfo));
        }

        private RangeInfo[] m_innerUnits;
        [OnDeserialized]
        private void _OnDeserialized(StreamingContext context)
        {
            m_units = new ObservableCollection<RangeInfo>(m_innerUnits);
            m_innerUnits = null;
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_innerUnits", m_units.ToArray());
            info.AddValue("m_activeUnit", m_activeUnit);
        }



        #region ISliderValue メンバー

        private ObservableCollection<RangeInfo> m_units = new ObservableCollection<RangeInfo>();
        private ReadOnlyObservableCollection<RangeInfo> m_readonlyUnits = null;
        public ReadOnlyObservableCollection<RangeInfo> Units
        {
            get { return m_readonlyUnits ?? (m_readonlyUnits = new ReadOnlyObservableCollection<RangeInfo>(m_units)); }
        }

        private RangeInfo m_activeUnit = null;
        public RangeInfo ActiveUnit
        {
            get { return m_activeUnit; }
            set { SetProperty(ref m_activeUnit, value); }
        }


        /// <summary>
        /// 他の単位に設定変更できるか
        /// </summary>
        public bool CanChangeUnit
        {
            get
            {
                return 2 <= Units.Count;
            }
        }

        public DelegateCommand<RangeInfo> m_changeUnitCommand = null;
        public ICommand ChangeUnitCommand
        {
            get { return m_changeUnitCommand ?? (m_changeUnitCommand = new DelegateCommand<RangeInfo>(_OnChangeUnit)); }
        }

        private void _OnChangeUnit(RangeInfo newUnit)
        {
            if(ActiveUnit == newUnit)
            { 
                return; 
            }
            var oldValue = Value;
            var oldRange = (ActiveUnit.Max + ActiveUnit.Min);
            ActiveUnit = newUnit;
            
            //Value = oldValue.Multiply<ValueType, double, ValueType>((ActiveUnit.Max + ActiveUnit.Min) / (oldRange));
        }
        
        #endregion
    }
}
