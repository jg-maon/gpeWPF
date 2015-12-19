using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using WpfApplication1;

namespace IdInfoTableTabPageXamlGenerator
{


    class MainWindowViewModel : ViewModelBase
    {

        protected MainWindowViewModel()
        { 
        }

        private static MainWindowViewModel s_instance = new MainWindowViewModel();
        public static MainWindowViewModel Instance { get { return s_instance; } }

        GparamRoot m_gparam = null;
        public GparamRoot GParam
        {
            get { return m_gparam; }
            set { SetProperty(ref m_gparam, value); }
        }

        ObservableCollection<CategoryViewModel> m_categories = new ObservableCollection<CategoryViewModel>();
        public ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                return (m_categories);
            }
            private set
            {
                SetProperty(ref m_categories, value);
            }
        }


        #region OpenCommand
        DelegateCommand _openCommand = null;
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new DelegateCommand(OnOpen, CanOpen);
                }

                return _openCommand;
            }
        }

        private bool CanOpen()
        {
            return true;
        }

        private void OnOpen()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                var filePath = dlg.FileName;
                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(GparamRoot));
                //読み込むファイルを開く
                using (System.IO.StreamReader sr = new System.IO.StreamReader(
                    filePath, new System.Text.UTF8Encoding(false)))
                {
                    //XMLファイルから読み込み、逆シリアル化する
                    GParam = (GparamRoot)serializer.Deserialize(sr);

                }
                ObservableCollection<CategoryViewModel> tempCollection;
                if(!GParamToParameterParser.TryParse(GParam, out tempCollection, SpaceReplace))
                {
                    System.Windows.MessageBox.Show("ファイル読み込み失敗");
                }
                else
                {
                    Categories = tempCollection;
                }

            }
        }


        #endregion 

        private CategoryViewModel m_selectedParamSet = null;
        public CategoryViewModel SelectedParamSet
        {
            get
            {
                return m_selectedParamSet;
            }
            set
            {
                SetProperty(ref m_selectedParamSet, value);
                OnPropertyChanged(() => ColumnsText);
            }
        }

        //private GparamRoot._ParamSet m_selectedParamSet = null;
        //public GparamRoot._ParamSet SelectedParamSet
        //{
        //    get
        //    {
        //        return m_selectedParamSet;
        //    }
        //    set
        //    {
        //        SetProperty(ref m_selectedParamSet, value);
        //        OnPropertyChanged(() => ColumnsText);
        //    }
        //}

        public string ColumnsText
        {
            get
            {
                if (SelectedParamSet == null) { return ""; }

                return _GetColumnsText();
            }
        }

        private string _GetColumnsText()
        {
            var xmlDoc = new XmlDocument();



            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.OmitXmlDeclaration = true;

            // xml文書の構築
            _Initialize(xmlDoc);
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
            {
                xmlDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                var stringBuilder = stringWriter.GetStringBuilder();
                return stringBuilder.ToString();
            }
        }

        private string m_vectorToStringConverterKey = "ColorToStringConverter";
        public string VectorToStringConverterKey
        {
            get { return m_vectorToStringConverterKey; }
            set { SetProperty(ref m_vectorToStringConverterKey, value); OnPropertyChanged(() => ColumnsText); }
        }

        private string m_colorToStringConverterKey = "ColorToStringConverter";
        public string ColorToStringConverterKey
        {
            get { return m_colorToStringConverterKey; }
            set { SetProperty(ref m_colorToStringConverterKey, value); OnPropertyChanged(() => ColumnsText); }
        }

        private string m_textColumnExName = "DataGridTextColumnEx";
        public string TextColumnExName
        {
            get { return m_textColumnExName; }
            set { SetProperty(ref m_textColumnExName, value); OnPropertyChanged(() => ColumnsText); }
        }

        private string m_checkBoxColumnName = "DataGridTemplateColumn";
        public string CheckBoxColumnName
        {
            get { return m_checkBoxColumnName; }
            set { SetProperty(ref m_checkBoxColumnName, value); OnPropertyChanged(() => ColumnsText); }
        }

        private string m_proxyName = "proxy";
        public string ProxyName
        {
            get { return m_proxyName; }
            set { SetProperty(ref m_proxyName, value); OnPropertyChanged(() => ColumnsText); }
        }

        private string m_spaceReplace = "_";
        public string SpaceReplace
        {
            get { return m_spaceReplace; }
            set { SetProperty(ref m_spaceReplace, value); OnPropertyChanged(() => ColumnsText); GParamToParameterParser.TryParse(GParam, out m_categories, SpaceReplace); }
        }


        private void _Initialize(XmlDocument xmlDoc)
        {
            var dataGridElement = xmlDoc.CreateElement("DataGrid");
            foreach(var slot in SelectedParamSet.Parameters[0].Slots)
            {
                StringBuilder sb = new StringBuilder("{Binding Path=Tag");
                _Recursively(xmlDoc, dataGridElement, slot, sb);


            }
            xmlDoc.AppendChild(dataGridElement);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc">CreateElement用</param>
        /// <param name="dataGridElement">追加先親ノード</param>
        /// <param name="slot">追加するノード</param>
        /// <param name="sb">属性値</param>
        private void _Recursively(XmlDocument xmlDoc, XmlElement dataGridElement, IEditableValue slot, StringBuilder sb)
        {

            var bindableName = slot.BindableName;

            sb.Append("." + bindableName);

            var groupSlot = slot.Value as ObservableCollection<IEditableValue>;
            // グループ用スロットの場合
            if (null != groupSlot)
            {
                // .{BindableName} Recursibly
                // Value分再帰
                foreach (var childSlot in groupSlot)
                {
                    StringBuilder childrenSb = new StringBuilder(sb.ToString());
                    _Recursively(xmlDoc, dataGridElement, childSlot, childrenSb);
                }
            }
            // パラメータスロットの場合 sbの中身は {Binding Path=Tag.<GroupBindableName>.<GroupBindableName>
            else
            {
                // 自身のDispNameを結合してヘッダー属性値を作成
                var headerAttribute = string.Format("{0}.DispName, Source={{StaticResource {1}}}}}", sb.ToString(), ProxyName);
                // Tagを外したValueバインド用属性の頭
                var bindingAttributePrefix = sb.Replace("{Binding Path=Tag.", "{Binding Path=").ToString();
                var type = slot.Type;
                switch (type)
                {
                    case (int)GX_META_INFO_TYPE.INT8:
                    case (int)GX_META_INFO_TYPE.INT16:
                    case (int)GX_META_INFO_TYPE.INT32:
                    case (int)GX_META_INFO_TYPE.INT64:
                    case (int)GX_META_INFO_TYPE.UINT8:
                    case (int)GX_META_INFO_TYPE.UINT16:
                    case (int)GX_META_INFO_TYPE.UINT32:
                    case (int)GX_META_INFO_TYPE.UINT64:
                    case (int)GX_META_INFO_TYPE.FLOAT32:
                    case (int)GX_META_INFO_TYPE.FLOAT64:
                    case (int)GX_META_INFO_TYPE.STRINGW:
                    case (int)GX_META_INFO_TYPE.VECTOR2AL:
                    case (int)GX_META_INFO_TYPE.VECTOR3AL:
                    case (int)GX_META_INFO_TYPE.VECTOR4AL:
                    case (int)GX_META_INFO_TYPE.COLOR32:
                        {
                            var element = xmlDoc.CreateElement(TextColumnExName);

                            element.SetAttribute("Header", headerAttribute);

                            switch (type)
                            {
                                case (int)GX_META_INFO_TYPE.VECTOR2AL:
                                case (int)GX_META_INFO_TYPE.VECTOR3AL:
                                case (int)GX_META_INFO_TYPE.VECTOR4AL:
                                    element.SetAttribute("Binding", string.Format("{0}.Value, Mode=TwoWay, UpdateSourceTrigger=LostFocus, Converter={{StaticResource {1}}}}}", sb.ToString(), VectorToStringConverterKey));
                                    break;
                                case (int)GX_META_INFO_TYPE.COLOR32:
                                    element.SetAttribute("Binding", string.Format("{0}.Value, Mode=TwoWay, UpdateSourceTrigger=LostFocus, Converter={{StaticResource {1}}}}}", sb.ToString(), ColorToStringConverterKey));
                                    break;
                                default:
                                    element.SetAttribute("Binding", string.Format("{0}.Value, Mode=TwoWay, UpdateSourceTrigger=LostFocus}}", sb.ToString()));
                                    break;
                            }
                            dataGridElement.AppendChild(element);
                        }
                        break;
                    case (int)GX_META_INFO_TYPE.BOOL:
                        {
                            var element = xmlDoc.CreateElement(CheckBoxColumnName);
                            element.SetAttribute("Header", headerAttribute);
                            {
                                var templateElement = xmlDoc.CreateElement(string.Format("{0}.CellTemplate", CheckBoxColumnName));
                                {
                                    var dataTemplateElement = xmlDoc.CreateElement("DataTemplate");
                                    {
                                        var checkBoxElement = xmlDoc.CreateElement("CheckBox");

                                        checkBoxElement.SetAttribute("IsChecked", string.Format("{0}.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}}", sb.ToString()));

                                        dataTemplateElement.AppendChild(checkBoxElement);
                                    }
                                    templateElement.AppendChild(dataTemplateElement);
                                }
                                element.AppendChild(templateElement);
                            }
                            dataGridElement.AppendChild(element);


                        }
                        break;

                }
            }
        }




    }
}
