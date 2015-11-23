using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FindEditableValuePropertyGenerator
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Windows;
    using WpfApplication1;
    class MainWindowViewModel : BindableBase
    {
        GparamRoot gparam;
        DelegateCommand m_openFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                return m_openFileCommand = m_openFileCommand ?? new DelegateCommand(_OnOpenFile);
            }
        }

        private void _OnOpenFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if(dialog.ShowDialog().GetValueOrDefault())
            {
                try
                {

                    string name = dialog.FileName;

                    //XmlSerializerオブジェクトを作成
                    System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(GparamRoot));
                    //読み込むファイルを開く
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(
                        name, new System.Text.UTF8Encoding(false)))
                    {
                        //XMLファイルから読み込み、逆シリアル化する
                        gparam = (GparamRoot)serializer.Deserialize(sr);

                    }

                    foreach (var paramset in gparam.ParamSet)
                    {
                        string cateName = paramset.DispName;
                        Category cate = new Category(cateName);
                        foreach (var slot in paramset.Slot)
                        {
                            string paramName = slot.DispName;
                            cate.Properties.Add(new ParameterProperty(paramName));
                        }
                        Categories.Add(cate);
                    }
                    FilePath = name;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.StackTrace + "\n" + e.Message, "error");
                }

            }
        }

        private string m_filePath;
        public string FilePath
        {
            get
            {
                return m_filePath;
            }
            set
            {
                SetProperty(ref m_filePath, value);
            }
        }

        private ObservableCollection<Category> m_categories;
        public ObservableCollection<Category> Categories
        {
            get
            {
                return m_categories = m_categories ?? new ObservableCollection<Category>();
            }
        }

        public class ParameterProperty : BindableBase
        {
            private readonly string m_propertyName;

            public string Code
            {
                get
                {
                    return string.Format(
"        #region {0}\n"+
"        bool m_is{0}EventAdded = false;\n"+
"        public dynamic {0}\n"+
"        {{\n"+
"            get\n"+
"            {{\n"+
"                var result = _FindValue(Slots, \"{0}\");\n"+
"                if (null != result)\n"+
"                {{\n"+
"                    if (!m_is{0}EventAdded)\n"+
"                    {{\n"+
"                        result.PropertyChanged += (sender, e) => OnPropertyChanged(() => this.{0});\n"+
"                        m_is{0}EventAdded = true;\n"+
"                    }}\n"+
"                }}\n"+
"                return result.Value;\n"+
"            }}\n"+
"            set\n"+
"            {{\n"+
"                var result = _FindValue(Slots, \"{0}\");\n"+
"                if (null != result)\n"+
"                {{\n"+
"                    result.Value = value;\n"+
"                }}\n"+
"            }}\n"+
"        }}\n"+
"        #endregion  // {0}\n", m_propertyName);
                }
            }
            public ParameterProperty(string name)
            {
                m_propertyName = name;
            }
        }

        public class Category : BindableBase
        {
            private readonly string m_categoryName;
            public string CategoryName
            {
                get
                {
                    return m_categoryName;
                }
            }
            public Category(string name)
            {
                m_categoryName = name;
            }
            ObservableCollection<ParameterProperty> m_param = new ObservableCollection<ParameterProperty>();
            public ObservableCollection<ParameterProperty> Properties
            {
                get
                {
                    return m_param;
                }
            }
        }

    }
}
