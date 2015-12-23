using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1
{
    class ConfigManager
    {
        private static ConfigManager s_instance = null;

        public static ConfigManager Instance
        {
            get
            {
                return s_instance ?? (s_instance = new ConfigManager());
            }
        }
        
        private ConfigManager()
        {
            _LoadParameterInfo();
        }


        #region デフォルトgparamの読み込み

        private static readonly string s_defaultGparamFilePath = @"config/default.gparamxml";

        private CategoriesAccessor m_defaultGParam = null;
        /// <summary>
        /// デフォルトGParamファイルの取得
        /// </summary>
        public CategoriesAccessor DefaultGParam
        {
            get
            {
                if(null == m_defaultGParam)
                {
                    m_defaultGParam = new CategoriesAccessor();

                    _LoadDefaultGParam();
                }
                return m_defaultGParam;
            }
        }

        /// <summary>
        /// デフォルトgparamファイルの読み込み
        /// </summary>
        private void _LoadDefaultGParam()
        {
            if(!m_defaultGParam.LoadFile(s_defaultGparamFilePath))
            {
                MessageBox.Show("デフォルトファイルの読み込みに失敗", "");
            }
        }

        /// <summary>
        /// カテゴリ一覧アクセスクラス
        /// </summary>
        public class CategoriesAccessor : ICustomTypeDescriptor
        {
            List<PropertyDescriptor> m_slotProperties = new List<PropertyDescriptor>();

            public CategoriesAccessor()
            { }

            private ObservableCollection<CategoryViewModel> m_categories = new ObservableCollection<CategoryViewModel>();
            private ReadOnlyObservableCollection<CategoryViewModel> m_readonlyCategories = null;
            /// <summary>
            /// カテゴリ一覧の取得
            /// </summary>
            public ReadOnlyObservableCollection<CategoryViewModel> Categories
            {
                get
                {
                    if (null == m_readonlyCategories)
                    {
                        m_readonlyCategories = new ReadOnlyObservableCollection<CategoryViewModel>(m_categories);
                    }
                    return m_readonlyCategories;
                }
            }

            /// <summary>
            /// ファイル読み込み
            /// </summary>
            /// <param name="filePath">ファイルパス</param>
            /// <returns>読み込めたか</returns>
            public bool LoadFile(string filePath)
            {
                try
                {

                    GparamRoot gparam;

                    //XmlSerializerオブジェクトを作成
                    System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(GparamRoot));
                    //読み込むファイルを開く
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(
                        filePath, new System.Text.UTF8Encoding(false)))
                    {
                        //XMLファイルから読み込み、逆シリアル化する
                        gparam = (GparamRoot)serializer.Deserialize(sr);

                    }

                    if (!GParamToParameterParser.TryParse(gparam, out m_categories))
                    {
                        System.Windows.MessageBox.Show("gparamファイルのパースに失敗", "");
                        return false;
                    }
                }
                catch(Exception e)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("gparamファイルの読み込みに失敗");
                    sb.AppendLine("ファイル：" + filePath);
                    sb.AppendLine(e.Message);
                    Debug.Print(e.StackTrace);
                    MessageBox.Show(sb.ToString(), "");
                    return false;
                }

                // カテゴリコレクションからプロパティを生成
                m_slotProperties.Clear();
                foreach(var category in m_categories)
                {
                    foreach(PropertyDescriptor p in category.Parameters[0].GetProperties())
                    {
                        m_slotProperties.Add(p);
                    }
                    //m_slotProperties.Add()
                }

                return true;
            }
            /// <summary>
            /// カテゴリの取得
            /// </summary>
            /// <param name="categoryName">カテゴリ識別名</param>
            /// <returns></returns>
            public CategoryViewModel GetCategoryFromName(string categoryName)
            {
                return m_categories.FirstOrDefault(c => c.Name == categoryName);
            }
            /// <summary>
            /// カテゴリの取得
            /// </summary>
            /// <param name="categoryDispName">カテゴリ表示名</param>
            /// <returns></returns>
            public CategoryViewModel GetCategoryFromDispName(string categoryDispName)
            {
                return m_categories.FirstOrDefault(c => c.DispName == categoryDispName);
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
        #endregion


        #region パラメータ詳細定義

        private static readonly string s_parameterInfoFile = @"config/Parameter.json";
        private static readonly string s_parameterInfoParametersLabel = "Parameters";
        private static readonly string s_parameterInfoParameterNameLabel = "Name";
        private static readonly string s_parameterInfoParameterIsSaveLabel = "IsSave";
        private static readonly string s_parameterInfoParameterIsVisibleLabel = "IsVisible";
        private static readonly string s_parameterInfoParameterEditorTypeLabel = "EditorType";
        private static readonly string s_parameterInfoSliderLabel = "Slider";
        private static readonly string s_parameterInfoSliderRangeNameLabel = "Name";
        private static readonly string s_parameterInfoSliderRangeLabel = "Range";
        private static readonly string s_parameterInfoSliderRangeMinLabel = "Min";
        private static readonly string s_parameterInfoSliderRangeMaxLabel = "Max";
        private static readonly string s_parameterInfoSliderRangeTickLabel = "Tick";
        private static readonly string s_parameterInfoSliderRangeSnapToTickLabel = "SnapToTick";
        private static readonly string s_parameterInfoSliderRangeSmallChangeLabel = "SmallChange";
        private static readonly string s_parameterInfoComboBoxLabel = "ComboBox";
        private static readonly string s_parameterInfoComboBoxTextLabel = "Text";
        private static readonly string s_parameterInfoComboBoxValuesLabel = "Values";

        public class SliderInfo
        {
            public class RangeInfo
            {
                public string Name { get; set; }
                public double Min { get; set; }
                public double Max { get; set; }
                public double Tick { get; set; }
                public bool SnapToTick { get; set; }
                public double SmallChange { get; set; }
            }
            public List<RangeInfo> Units { get; set; }
            public SliderInfo() { Units = new List<RangeInfo>(); }
        }

        public class ComboBoxInfo
        {
            public List<string> Text { get; set; }
            public List<dynamic> Values { get; set; }
            public ComboBoxInfo() { Text = new List<string>(); Values = new List<dynamic>(); }
        }

        class SliderValue<ValueType> : TUnitValue<ValueType>
        {
            public interface IRange
            {
                string Name { get; set; }
            }
            class TRange : IRange
            {
                public string Name { get; set; }
                public ValueType Min { get; set; }
                public ValueType Max { get; set; }
                public ValueType Tick { get; set; }
                public bool SnapToTick { get; set; }
                public ValueType SmallChange { get; set; }
            }

            private ObservableCollection<IRange> m_units = new ObservableCollection<IRange>();
            private ReadOnlyObservableCollection<IRange> m_readonlyUnits = null;
            public ReadOnlyObservableCollection<IRange> Units
            {
                get
                {
                    return m_readonlyUnits ?? (m_readonlyUnits = new ReadOnlyObservableCollection<IRange>(m_units));
                }
            }
            private IRange m_activeUnit = null;
            public IRange ActiveUnit
            {
                get { return m_activeUnit; }
                set { SetProperty(ref m_activeUnit, value); }
            }
        }
        

        
        public class ParameterInfo
        {
            public bool IsVisible { get; private set; }
            public bool IsSave { get; private set; }

            public int? EditorType { get; private set; }

            public enum EditorKind
            {
                Default,
                Slider,
                ComboBox,
            }
            public EditorKind Kind { get; private set; }

            public object Editor { get; private set; }
            
            public ParameterInfo(EditorKind kind, object editor, double? editorType, bool isVisible, bool isSave)
            {
                IsVisible = isVisible;
                IsSave = isSave;
                EditorType = (int?)(editorType);
                Kind = kind;
                Editor = editor;
            }

        }
        private Dictionary<string, ParameterInfo> m_parameterInfos = new Dictionary<string, ParameterInfo>();
        private ReadOnlyDictionary<string, ParameterInfo> m_readonlyParameterInfos = null;
        public ReadOnlyDictionary<string, ParameterInfo> ParameterInfos
        {
            get { return m_readonlyParameterInfos ?? (m_readonlyParameterInfos = new ReadOnlyDictionary<string, ParameterInfo>(m_parameterInfos)); }
        }

        /// <summary>
        /// パラメータ詳細定義の読み込み
        /// </summary>
        private void _LoadParameterInfo()
        {
            try
            {
                using (var reader = new System.IO.StreamReader(s_parameterInfoFile))
                {
                    var json = DynamicJson.Parse(reader.ReadToEnd());

                    foreach(var jsonParameterObject in json[s_parameterInfoParametersLabel])
                    {
                        string name = jsonParameterObject[s_parameterInfoParameterNameLabel];
                        ParameterInfo parameterInfo = null;
                        double? editorType = (jsonParameterObject.IsDefined(s_parameterInfoParameterEditorTypeLabel)) ? (jsonParameterObject[s_parameterInfoParameterEditorTypeLabel]) : null;
                        bool isVisible = (jsonParameterObject.IsDefined(s_parameterInfoParameterIsVisibleLabel)) ? jsonParameterObject[s_parameterInfoParameterIsVisibleLabel] : true;
                        bool isSave = (jsonParameterObject.IsDefined(s_parameterInfoParameterIsSaveLabel)) ? jsonParameterObject[s_parameterInfoParameterIsSaveLabel] : true;
                        if(jsonParameterObject.IsDefined(s_parameterInfoSliderLabel))
                        {
                            SliderInfo slider = new SliderInfo();
                            
                            foreach(var jsonSliderObject in jsonParameterObject[s_parameterInfoSliderLabel])
                            {
                                string unitName = (jsonSliderObject.IsDefined(s_parameterInfoSliderRangeNameLabel)) ? jsonSliderObject[s_parameterInfoSliderRangeNameLabel] : "Unit";
                                var jsonRangeObject = (jsonSliderObject.IsDefined(s_parameterInfoSliderRangeLabel)) ? jsonSliderObject[s_parameterInfoSliderRangeLabel] : null;
                                if(null != jsonRangeObject)
                                {
                                    double tick = (jsonRangeObject.IsDefined(s_parameterInfoSliderRangeTickLabel)) ? jsonRangeObject[s_parameterInfoSliderRangeTickLabel] : 0.01;
                                    bool snapToTick = (jsonRangeObject.IsDefined(s_parameterInfoSliderRangeSnapToTickLabel)) ? jsonRangeObject[s_parameterInfoSliderRangeSnapToTickLabel] : true;
                                    double smallChange = (jsonRangeObject.IsDefined(s_parameterInfoSliderRangeSmallChangeLabel)) ? jsonRangeObject[s_parameterInfoSliderRangeSmallChangeLabel] : 0.01;

                                    double min = (jsonRangeObject.IsDefined(s_parameterInfoSliderRangeMinLabel)) ? jsonRangeObject[s_parameterInfoSliderRangeMinLabel] : 0.0;
                                    double max = (jsonRangeObject.IsDefined(s_parameterInfoSliderRangeMaxLabel)) ? jsonRangeObject[s_parameterInfoSliderRangeMaxLabel] : 100.0;

                                    slider.Units.Add(new SliderInfo.RangeInfo()
                                    {
                                        Min = min,
                                        Max = max,
                                        Tick = tick,
                                        SnapToTick = snapToTick,
                                        SmallChange = smallChange
                                    });
                                }
                            }

                            parameterInfo = new ParameterInfo(ParameterInfo.EditorKind.Slider, slider, editorType, isVisible, isSave);

                        }
                        else if(jsonParameterObject.IsDefined(s_parameterInfoComboBoxLabel))
                        {
                            var jsonComboBoxObject = jsonParameterObject[s_parameterInfoComboBoxLabel];
                            ComboBoxInfo comboBox = new ComboBoxInfo();
                            foreach (var text in jsonComboBoxObject[s_parameterInfoComboBoxTextLabel])
                            {
                                comboBox.Text.Add(text);
                            }
                            if (jsonComboBoxObject.IsDefined(s_parameterInfoComboBoxValuesLabel))
                            {
                                foreach (var value in jsonComboBoxObject[s_parameterInfoComboBoxValuesLabel])
                                {
                                    comboBox.Values.Add(value);
                                }
                            }
                            else
                            {
                                for(int i=0; i<comboBox.Text.Count; ++i)
                                {
                                    comboBox.Values.Add(i);
                                }
                            }
                            parameterInfo = new ParameterInfo(ParameterInfo.EditorKind.ComboBox, comboBox, editorType, isVisible, isSave);
                        }
                        else
                        {
                            parameterInfo = new ParameterInfo(ParameterInfo.EditorKind.Default, null, editorType, isVisible, isSave);   
                        }

                        m_parameterInfos[name] = parameterInfo;
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
        }
        #endregion

    }
}
