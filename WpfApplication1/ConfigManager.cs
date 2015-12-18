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
                return s_instance = s_instance ?? new ConfigManager();
            }
        }
        
        private ConfigManager()
        {
            _LoadDefaultGParam();
        }


        #region デフォルトgparamの読み込み

        private static readonly string s_defaultGparamFilePath = @"config/default.gparamxml";

        private CategoriesAccessor m_defaultGParam = new CategoriesAccessor();
        /// <summary>
        /// デフォルトGParamファイルの取得
        /// </summary>
        public CategoriesAccessor DefaultGParam
        {
            get
            {
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

                    if (!GParamToParameterParser.TryParse(gparam, ref m_categories))
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

    }
}
