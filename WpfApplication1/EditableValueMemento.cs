using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// EditableValueを保存するMemento
    /// </summary>
    public class EditableValueMemento : Memento<IEditableValue, ParameterRecordViewModel>, INotifyPropertyChanged
    {
        public EditableValueMemento(IEditableValue value, ParameterRecordViewModel parameters) : base(value)
        {
            Target = parameters;
        }

        public override void SetMemento(IEditableValue state)
        {
            if(state != null)
            {
                this.State = DeepCopyExtensions.DeepCopy(state);
            
                var slots = Target.Slots;
                for (int i = 0; i < slots.Count; ++i)
                {
                    if (slots[i].Name == State.Name)
                    {
                        slots[i] = State;
                        break;
                    }
                }
            }
            else
            {
                this.State = state;
            }
            OnPropertyChanged("State");
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region INotifyPropertyChanged メンバー

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
