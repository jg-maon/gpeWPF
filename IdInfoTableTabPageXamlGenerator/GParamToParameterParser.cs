﻿using Codeplex.Data;
using IdInfoTableTabPageXamlGenerator;
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
        static readonly string s_groupDefinitionFile = "config/group.json";
        static readonly string s_groupLabel = "group";
        static readonly string s_argumentsLabel = "Args";
        static readonly string s_namesLabel = "Names";
        static readonly string s_isExpandedLabel = "IsExpanded";
        static readonly string s_spaceReplaceLabel = "SpaceReplace";

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
            public string Name { get; set; }
            /// <summary>
            /// グループ名
            /// </summary>
            public string DispName { get; set; }
            /// <summary>
            /// 初期展開状態
            /// </summary>
            public bool IsExpanded { get; set; }
            public GroupPattern()
            {
                Args = new List<string>();
            }
        }


        internal static bool TryParse(GparamRoot gparam, out System.Collections.ObjectModel.ObservableCollection<CategoryViewModel> destCollection)
        {
            return TryParse(gparam, out destCollection, "_");
        }
        internal static bool TryParse(GparamRoot gparam, out System.Collections.ObjectModel.ObservableCollection<CategoryViewModel> destCollection, string spaceReplace)
        {
            
            ObservableCollection<CategoryViewModel> tempCollection = new ObservableCollection<CategoryViewModel>();
            try
            {
                using (var reader = new StreamReader(s_groupDefinitionFile))
                {
                    var json = DynamicJson.Parse(reader.ReadToEnd());
                    Console.WriteLine(json.ToString());


                    var GroupPatterns = new Dictionary<string, List<GroupPattern>>();
                    
                    int n = 0;  // #TODO: IDのName用
                    foreach (var paramset in gparam.ParamSet)
                    {
                        string categoryDispName = paramset.DispName;
                        string categoryName = paramset.Name;
                        string categoryBindableName = categoryDispName.Trim();

                        int tabIndex = 0;  // TextBox.TabIndex用カウンタ

                        categoryBindableName = categoryBindableName.Replace(" ", spaceReplace);

                        // グループ情報の格納
                        #region グループ情報の格納
                        List<GroupPattern> groupPatterns;
                        foreach(var jsonCategoryObject in json.Categories)
                        {
                            // グループのグループ化するパラメータとグループ後の名前のコレクション
                            groupPatterns = new List<GroupPattern>();
                            //groupPatterns.Add
                            foreach(var jsonGroupObject in jsonCategoryObject.Group)
                            {
                                GroupPattern pattern = new GroupPattern()
                                {
                                    Name = jsonGroupObject.Name,
                                    DispName = jsonGroupObject.DispName,
                                    IsExpanded = (bool)(jsonGroupObject.IsExpanded),
                                };
                                foreach(var jsonArgs in jsonGroupObject.Args)
                                {
                                    pattern.Args.Add(jsonArgs);
                                }
                                groupPatterns.Add(pattern);
                            }
                            GroupPatterns[jsonCategoryObject.Name] = groupPatterns;

                        }
                        groupPatterns = GroupPatterns[categoryName];
                        //if (json.IsDefined(categoryDispName) && json[categoryDispName].IsObject)
                        //{
                        //    var jsonCategory = json[categoryDispName];
                        //    // カテゴリ(Object)内にグループがあり、Arrayの場合
                        //    if (jsonCategory.IsDefined(s_groupLabel) && jsonCategory[s_groupLabel].IsArray)
                        //    {
                        //        var jsonGroups = jsonCategory[s_groupLabel];
                        //        // グループの配列分組み合わせと名前を収拾
                        //        foreach (var groupObject in jsonGroups)
                        //        {
                        //            if (groupObject.IsDefined(s_argumentsLabel) && groupObject.IsDefined(s_namesLabel))
                        //            {
                        //                GroupPattern groupPattern = new GroupPattern();
                        //                foreach (string arg in groupObject[s_argumentsLabel])
                        //                {
                        //                    groupPattern.Args.Add(arg);
                        //                }
                        //                foreach (string arg in groupObject[s_namesLabel])
                        //                {
                        //                    groupPattern.Name.Add(arg);
                        //                }
                        //                foreach(bool arg in groupObject[s_isExpandedLabel])
                        //                {
                        //                    groupPattern.IsExpanded.Add(arg);
                        //                }
                        //                groupPatterns.Add(groupPattern);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        var parameterCollection = new CategoryViewModel() {
                            DispName = categoryDispName, Name = categoryName, BindableName = categoryBindableName };
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
                            var groupCollection = new ObservableCollection<IEditableValue>();
                            // グループ配列用リスト
                            
                            var groupDic = new Dictionary<GroupPattern, List<ObservableCollection<IEditableValue>>>();
                            //groupDic[id] = new List<ObservableCollection<IEditableValue>>();
                            List<ObservableCollection<IEditableValue>> groupList = null;



                            // 追加済みグループカウンタ
                            var addedGroupCountDictionary = new Dictionary<GroupPattern, int>();

                            foreach (var gparamSlot in paramset.Slot)
                            {
                                // パラメータ名
                                string parameterDispName = gparamSlot.DispName;
                                // バインディング用
                                string parameterBindableName = parameterDispName.Trim();
                                parameterBindableName = parameterBindableName.Replace(" ", spaceReplace);
                                // 固有名
                                string uniqueName = gparamSlot.Name;
                                // IDが一致している値部
                                string gparamTextById = gparamSlot.Value.FirstOrDefault(v => v.Id == id).Text;
                                // パラメータコレクションに追加させる値
                                IEditableValue slotValue = null;
                                
                                switch (gparamSlot.Type)
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
                                        {
                                            var unitValue = new TUnitValue<double>();
                                            unitValue.Value = double.Parse(gparamTextById);
                                            slotValue = unitValue;
                                        }
                                        break;
                                    case (int)GX_META_INFO_TYPE.STRINGW:
                                        {
                                            var unitValue = new TUnitValue<string>();
                                            unitValue.Value = gparamTextById;

                                            slotValue = unitValue;
                                        }
                                        break;
                                    case (int)GX_META_INFO_TYPE.VECTOR2AL:
                                    case (int)GX_META_INFO_TYPE.VECTOR3AL:
                                    case (int)GX_META_INFO_TYPE.VECTOR4AL:
                                        {
                                            var values = gparamTextById.Split(',');
                                            var unitValue = new TUnitValue<TArrayValue<float>>();
                                            var arrayValue = new TArrayValue<float>(Array.ConvertAll<string, float>(values, (s => float.Parse(s))));
                                            unitValue.Value = arrayValue;
                                            slotValue = unitValue;
                                        }
                                        break;
                                    case (int)GX_META_INFO_TYPE.COLOR32:
                                        {
                                            var values = gparamTextById.Split(',');
                                            var unitValue = new TUnitValue<TArrayValue<byte>>();
                                            var arrayValue = new TArrayValue<byte>(Array.ConvertAll<string, byte>(values, (s => byte.Parse(s))));
                                            unitValue.Value = arrayValue;
                                            slotValue = unitValue;
                                        }
                                        break;
                                    case (int)GX_META_INFO_TYPE.BOOL:
                                        {
                                            var unitValue = new TUnitValue<bool>();
                                            unitValue.Value = bool.Parse(gparamTextById);
                                            slotValue = unitValue;
                                        }
                                        break;

                                }
                                slotValue.Type = gparamSlot.Type;
                                
                                slotValue.DispName = parameterDispName;
                                slotValue.Name = uniqueName;
                                slotValue.BindableName = parameterBindableName;

                                // patternのargsを蓄積したものを一気に追加させる
                                bool isAdded = false;
                                bool isGroupChanged = false;
                                do
                                {
                                    isAdded = false;
                                    isGroupChanged = false;
                                    foreach (var pattern in groupPatterns)
                                    {
                                        if(!groupDic.ContainsKey(pattern))
                                        {
                                            groupDic[pattern] = new List<ObservableCollection<IEditableValue>>();
                                        }
                                        groupList = groupDic[pattern];
                                        // 配列が空の場合、全ての要素を追加する
                                        if (pattern.Args.Count == 0)
                                        {
                                            groupCollection.Add(slotValue);
                                            // 全ての要素を集めきったら
                                            if (groupCollection.Count == paramset.Slot.Count)
                                            {
                                                // 名前をグループ名に変更し、値にグループコレクションを入れてスロットに追加させる
                                                EditableValueGroup value = new EditableValueGroup();
                                                value.Name = pattern.Name;
                                                value.DispName = pattern.DispName;
                                                value.BindableName = pattern.DispName.Trim().Replace(" ", spaceReplace);
                                                value.Value = groupCollection;
                                                value.TabIndex = tabIndex++;
                                                value.IsExpanded = pattern.IsExpanded;
                                                slots.Add(value);
                                            }
                                            isAdded = true;
                                        }
                                        // patternのArgsに同じparamNameが存在する場合
                                        else if (pattern.Args.Contains(slotValue.Name))
                                        {
                                            // group用のコレクションに追加する
                                            // その際、既にコレクション内にparamNameの値が追加されていたら
                                            // 別のコレクションを探す

                                            // 追加先コレクション
                                            ObservableCollection<IEditableValue> targetCollection = null;
                                            // グループコレクションの確認
                                            foreach (var group in groupList)
                                            {
                                                // コレクション内に追加されたパラメータを1つずつ見る
                                                foreach (var groupCollectionValue in group)
                                                {
                                                    // 既にあるコレクションの中で自身と同じ名前のパラメータが登録されていたら
                                                    if (slotValue.Name == groupCollectionValue.Name)
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
                                                if (null != targetCollection)
                                                {
                                                    break;
                                                }
                                            }

                                            // 全てのコレクションに存在していたら新しいコレクションを作成し、追加を行う
                                            if (null == targetCollection)
                                            {
                                                targetCollection = new ObservableCollection<IEditableValue>();
                                                groupList.Add(targetCollection);
                                            }

                                            targetCollection.Add(slotValue);
                                            isAdded = true;
                                            // グループのパラメータ数分集まったら
                                            if (targetCollection.Count == pattern.Args.Count)
                                            {
                                                // 名前をグループ名に変更し、値にグループコレクションを入れてスロットに追加させる
                                                EditableValueGroup value = new EditableValueGroup();
                                                value.Name = pattern.Name;
                                                value.DispName = pattern.DispName;
                                                value.BindableName = value.DispName.Trim().Replace(" ", spaceReplace);
                                                value.Value = targetCollection;
                                                value.TabIndex = tabIndex++;
                                                value.IsExpanded = pattern.IsExpanded;
                                                //slots.Add(value);
                                                //targetCollection.Add(slotValue);
                                                slotValue = value;  // スロットの値をグループにしてからもう一度グループパターンのチェックをする
                                                isGroupChanged = true;

                                                break;
                                            }
                                        }
                                    }
                                    if(!isGroupChanged)
                                    {
                                        //isAdded = false;
                                    }
                                } while (isGroupChanged /*&& !isAdded*/);

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
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "");
                System.Diagnostics.Debug.Assert(false, e.StackTrace);
                destCollection = new ObservableCollection<CategoryViewModel>();
                return false;
            }

            destCollection = tempCollection;

            return true;
        }


        internal static bool TryDeserialize(ReadOnlyObservableCollection<CategoryViewModel> parameterCollection, out GparamRoot outGparam)
        {
            GparamRoot gparam = new GparamRoot();
            try
            {
                gparam.ParamSet = new List<GparamRoot._ParamSet>();
                foreach (var categories in parameterCollection)
                {
                    string categoryDispName = categories.DispName;
                    string categoryName = categories.Name;

                    GparamRoot._ParamSet._Comment gparamComments = new GparamRoot._ParamSet._Comment() { Value = new List<GparamRoot._ParamSet._Value>() };
                    GparamRoot._ParamSet._Edited gparamIds = new GparamRoot._ParamSet._Edited() { Id = new List<int>() };
                    
                    // @name, スロット
                    Dictionary<string, GparamRoot._ParamSet._Slot> gparamSlotDictionary = new Dictionary<string, GparamRoot._ParamSet._Slot>();
                    int slotCount = 0;
                    foreach(var paramset in categories.Parameters)
                    {
                        int id = paramset.ID;
                        string comment = paramset.Comment;
                        gparamIds.Id.Add(id);
                        gparamComments.Value.Add(new GparamRoot._ParamSet._Value() { Id = id, Text = comment, Index = slotCount });


                        // スロットの収拾
                        _Recursively(id, slotCount, paramset.Slots, ref gparamSlotDictionary);

                        ++slotCount;
                    }

                    GparamRoot._ParamSet gparamParamset = new GparamRoot._ParamSet()
                    {
                        DispName = categoryDispName,
                        Name = categoryName,
                        Comment = gparamComments,
                        Edited = gparamIds,
                        Slot = gparamSlotDictionary.Values.ToList()
                    };

                    gparam.ParamSet.Add(gparamParamset);

                }
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace, "");
                Console.WriteLine(e.StackTrace);
                outGparam = new GparamRoot();
                return false;
            }
            outGparam = gparam;
            return true;

        }

        private static void _Recursively(int id, int slotCount, ObservableCollection<IEditableValue> paramsetSlots, ref Dictionary<string, GparamRoot._ParamSet._Slot> gparamSlotDictionary)
        {
            foreach (var slot in paramsetSlots)
            {
                // パラメータ配列の場合
                //var group = slot as EditableValueGroup;
                var group = slot.Value as ObservableCollection<IEditableValue>;
                if (null != group)
                {
                    _Recursively(id, slotCount, group, ref gparamSlotDictionary);
                }
                else
                {
                    // 通常のパラメータ
                    string paramName = slot.Name;
                    string paramDispName = slot.DispName;
                    int paramType = slot.Type;

                    GparamRoot._ParamSet._Slot gparamSlot;
                    // 名前に対応したスロット配列の取得
                    if (!gparamSlotDictionary.TryGetValue(paramName, out gparamSlot))
                    {
                        // 取得できなかった場合、新規作成
                        gparamSlot = new GparamRoot._ParamSet._Slot();
                        gparamSlot.Value = new List<GparamRoot._ParamSet._Value>();
                        gparamSlotDictionary.Add(paramName, gparamSlot);
                    }

                    gparamSlot.DispName = paramDispName;
                    gparamSlot.Name = paramName;
                    gparamSlot.Type = paramType;
                    var value = new GparamRoot._ParamSet._Value() { Id = id, Index = slotCount };
                    

                    switch (gparamSlot.Type)
                    {
                        case 4: // float2
                        case 5: // float3
                        case 6: // float4   (とする)
                            {
                                var array = slot.Value as object[];
                                if(null != array)
                                {
                                    value.Text = string.Join(",", array);
                                }
                                else
                                {
                                    value.Text = string.Join(",", slot.Value);
                                }
                                //slotValue.Value = Array.ConvertAll<string, float>(values, new Converter<string, float>((s) => float.Parse(s)));
                            }
                            break;
                        default:
                            value.Text = slot.Value.ToString(); // #TODO: 配列の変換
                            break;
                    }


                    gparamSlot.Value.Add(value);

                }
            }
        }


    }
}