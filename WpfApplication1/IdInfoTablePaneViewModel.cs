using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    [Obsolete("IdInfoTablePane2ViewModel")]
    class IdInfoTablePaneViewModel : DocumentViewModel
    {
        private ObservableCollection<LightSetViewModel> m_lightSetCollection = new ObservableCollection<LightSetViewModel>();
        public ObservableCollection<LightSetViewModel> LightSetCollection
        {
            get
            {
                return m_lightSetCollection;
            }
            set
            {
                if(value != m_lightSetCollection)
                {
                    m_lightSetCollection = value;
                    RaisePropertyChanged("LightSetCollection");
                }
            }
        }

        ParameterViewModelBase m_selectedItem;
        public ParameterViewModelBase SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
            set
            {
                if(m_selectedItem != value)
                {
                    m_selectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                    if(SelectedItemChanged != null)
                    {
                        SelectedItemChanged(this, SelectedItem);
                    }
                }
            }
        }
        public event EventHandler<ParameterViewModelBase> SelectedItemChanged;

        public IdInfoTablePaneViewModel()
            : base("ID詳細情報")
        {
            m_lightSetCollection.Add(new LightSetViewModel() { ID = 10, Comment = "id:10", Name = "teketo-", DirLight0 = new ParamLightSet.LightInfo() { Sharpness = 10 } });
            m_lightSetCollection.Add(new LightSetViewModel() { ID = 20, Comment = "id:20", Name = "ど", DirLight1 = new ParamLightSet.LightInfo() { Sharpness = 101 } });
            m_lightSetCollection.Add(new LightSetViewModel() { ID = 1, Comment = "id:1", Name = "よ" });
            m_lightSetCollection.Add(new LightSetViewModel() { ID = 2, Comment = "id:2", Name = "うひひ-", HemiColorDown = new ColorRGBI(0.5f) });

        }

        protected override void OnClose()
        {
            throw new NotImplementedException();
        }
    }
}
