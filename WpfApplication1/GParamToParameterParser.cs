using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class GParamToParameterParser
    {
        static readonly string s_groupDefinitionFile = "config/group.config";
        static readonly string s_groupLabel = "group";
        static readonly string s_argumentsLabel = "Args";
        static readonly string s_namesLabel = "Names";

        //static Dictionary<string, GroupPattern> s_groupDefinitions = null;


        class GroupPattern
        {
            /// <summary>
            /// 収拾する名前
            /// </summary>
            public List<string> Args { get; set; }
            /// <summary>
            /// グループ名
            /// </summary>
            public List<string> Names { get; set; }
            public GroupPattern()
            {
                Args = new List<string>();
                Names = new List<string>();
            }
        }

        internal static bool TryParse(GparamRoot gparam, ref System.Collections.ObjectModel.ObservableCollection<ParameterCollectionViewModel> destCollection)
        {
            
            ObservableCollection<ParameterCollectionViewModel> tempCollection = new ObservableCollection<ParameterCollectionViewModel>();
            try
            {
                using (var reader = new StreamReader(s_groupDefinitionFile))
                {
                    var json = DynamicJson.Parse(reader.ReadToEnd());
                    Console.WriteLine(json.ToString());

                    
                    int n = 0;  // #TODO: IDのName用
                    foreach (var paramset in gparam.ParamSet)
                    {
                        string categoryName = paramset.DispName;

                        int tabIndex = 0;  // TextBox.TabIndex用カウンタ

                        // グループ情報の格納
                        #region グループ情報の格納

                        // グループのグループ化するパラメータとグループ後の名前のコレクション
                        List<GroupPattern> groupPatterns = new List<GroupPattern>();

                        if (json.IsDefined(categoryName) && json[categoryName].IsObject)
                        {
                            var jsonCategory = json[categoryName];
                            // カテゴリ(Object)内にグループがあり、Arrayの場合
                            if (jsonCategory.IsDefined(s_groupLabel) && jsonCategory[s_groupLabel].IsArray)
                            {
                                var jsonGroups = jsonCategory[s_groupLabel];
                                // グループの配列分組み合わせと名前を収拾
                                foreach (var groupObject in jsonGroups)
                                {
                                    if (groupObject.IsDefined(s_argumentsLabel) && groupObject.IsDefined(s_namesLabel))
                                    {
                                        GroupPattern groupPattern = new GroupPattern();
                                        foreach (string arg in groupObject[s_argumentsLabel])
                                        {
                                            groupPattern.Args.Add(arg);
                                        }
                                        foreach (string arg in groupObject[s_namesLabel])
                                        {
                                            groupPattern.Names.Add(arg);
                                        }
                                        groupPatterns.Add(groupPattern);
                                    }
                                }
                            }
                        }
                        #endregion

                        var parameterCollection = new ParameterCollectionViewModel() { Name = categoryName };
                        var parameters = parameterCollection.Parameters;
                        paramset.Edited.Id.Sort(); // 昇順ソート
                        foreach (var id in paramset.Edited.Id)
                        {
                            var commentValue = paramset.Comment.Value.FirstOrDefault(v => v.Id == id);
                            if (null == commentValue)
                            {
                                System.Windows.MessageBox.Show(string.Format("id:{0} not found", id), "");
                                continue;
                            }
                            var comment = commentValue.Text;
                            var parameter = parameterCollection.CreateId(id, "name" + (++n).ToString(), comment);
                            var slots = parameter.Slots;
                            // 全追加用グループコレクション
                            var groupCollection = new ObservableCollection<EditableValue>();
                            // グループ配列用リスト
                            var groupList = new List<ObservableCollection<EditableValue>>();
                            foreach (var gparamSlot in paramset.Slot)
                            {
                                // パラメータ名
                                string parameterName = gparamSlot.DispName;
                                // IDが一致している値部
                                string gparamTextById = gparamSlot.Value.FirstOrDefault(v => v.Id == id).Text;
                                // パラメータコレクションに追加させる値
                                var slotValue = new EditableValue();
                                switch (gparamSlot.Type)
                                {
                                    case 1:
                                        slotValue.Value = float.Parse(gparamTextById);
                                        break;
                                    case 2:
                                        slotValue.Value = bool.Parse(gparamTextById);
                                        break;
                                    case 4:
                                        {
                                            var values = gparamTextById.Split(',');

                                            slotValue.Value = Array.ConvertAll<string, float>(values, new Converter<string, float>((s) => float.Parse(s)));
                                        }
                                        break;
                                    default:
                                        slotValue.Value = gparamTextById;
                                        break;
                                }

                                slotValue.Name = parameterName;

                                // patternのargsを蓄積したものを一気に追加させる
                                bool isAdded = false;
                                foreach(var pattern in groupPatterns)
                                {
                                    // 配列が空の場合、全ての要素を追加する
                                    if(pattern.Args.Count == 0)
                                    {
                                        groupCollection.Add(slotValue);
                                        // 全ての要素を集めきったら
                                        if(groupCollection.Count == paramset.Slot.Count)
                                        {
                                            // 名前をグループ名に変更し、値にグループコレクションを入れてスロットに追加させる
                                            EditableValueGroup value = new EditableValueGroup();
                                            value.Name = pattern.Names[0];
                                            value.Value = groupCollection;
                                            value.TabIndex = tabIndex++;
                                            slots.Add(value);
                                        }
                                        isAdded = true;
                                    }
                                    // patternのArgsに同じparamNameが存在する場合
                                    else if(pattern.Args.Contains(parameterName))
                                    {
                                        // group用のコレクションに追加する
                                        // その際、既にコレクション内にparamNameの値が追加されていたら
                                        // 別のコレクションを探す
                                        
                                        // 追加先コレクション
                                        ObservableCollection<EditableValue> targetCollection = null;
                                        // グループコレクションの確認
                                        foreach(var group in groupList)
                                        {
                                            // コレクション内に追加されたパラメータを1つずつ見る
                                            foreach(var groupCollectionValue in group)
                                            {
                                                // 既にあるコレクションの中で自身と同じ名前のパラメータが登録されていたら
                                                if(parameterName == groupCollectionValue.Name)
                                                {
                                                    // このグループには追加させないようにしてパラメータ検索終了
                                                    targetCollection = null;
                                                    break;
                                                }
                                                    // 同じ名前のパラメータでない場合
                                                else if (null == targetCollection)
                                                {
                                                    // このコレクションにパラメータを追加させるように登録
                                                    targetCollection = group;
                                                }
                                            }
                                            // 追加先が決まった場合はループ終了
                                            if(null != targetCollection)
                                            {
                                                break;
                                            }
                                        }

                                        // 全てのコレクションに存在していたら新しいコレクションを作成し、追加を行う
                                        if(null == targetCollection)
                                        {
                                            targetCollection = new ObservableCollection<EditableValue>();
                                            groupList.Add(targetCollection);
                                        }

                                        targetCollection.Add(slotValue);
                                        isAdded = true;
                                        // グループのパラメータ数分集まったら
                                        if (targetCollection.Count == pattern.Args.Count)
                                        {
                                            // 名前をグループ名に変更し、値にグループコレクションを入れてスロットに追加させる
                                            EditableValueGroup value = new EditableValueGroup();
                                            value.Name = pattern.Names[(groupList.Count - 1) % pattern.Names.Count];
                                            value.Value = targetCollection;
                                            value.TabIndex = tabIndex++;
                                            slots.Add(value);
                                        }
                                    }
                                }

                                if (!isAdded)
                                {
                                    slotValue.TabIndex = tabIndex++;
                                    slots.Add(slotValue);
                                }
                            }

                            parameters.Add(parameter);
                        }


                        // 自身のコレクションに追加
                        tempCollection.Add(parameterCollection);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            destCollection = tempCollection;

            return true;
        }
    }
}