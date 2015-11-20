using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class GParamToParameterParser
    {
        internal static bool TryParse(GparamRoot gparam, ref System.Collections.ObjectModel.ObservableCollection<ParameterCollectionViewModel> destCollection)
        {
            System.Collections.ObjectModel.ObservableCollection<ParameterCollectionViewModel> tempCollection = new System.Collections.ObjectModel.ObservableCollection<ParameterCollectionViewModel>();
            try
            {
                int n = 0;
                foreach (var paramset in gparam.ParamSet)
                {
                    string category = paramset.DispName;
                    var collection = new ParameterCollectionViewModel() { Name = category };
                    var parameters = collection.Parameters;
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
                        var param = new ParametersViewModel() { ID = id, Comment = comment, Name = "name" + (++n).ToString() };
                        var slots = param.Slots;
                        foreach (var slot in paramset.Slot)
                        {
                            string paramName = slot.DispName;
                            string text = slot.Value.FirstOrDefault(v => v.Id == id).Text;
                            IEditableValue value;
                            switch (slot.Type)
                            {
                                case 1:
                                    value = new EditableValue<float>();
                                    value.Value = float.Parse(text);
                                    break;
                                case 2:
                                    value = new EditableValue<bool>();
                                    value.Value = bool.Parse(text);
                                    break;
                                case 4:
                                    value = new EditableValue<float[]>();
                                    {
                                        var values = text.Split(',');

                                        value.Value = Array.ConvertAll<string, float>(values, new Converter<string, float>((s) => float.Parse(s)));
                                    }
                                    break;
                                default:
                                    value = new EditableValue<object>();
                                    value.Value = text;
                                    break;
                            }


                            slots.Add(value);
                        }

                        parameters.Add(param);
                    }


                    // 自身のコレクションに追加
                    tempCollection.Add(collection);
                }
            }
            catch(Exception)
            {
                return false;
            }

            destCollection = tempCollection;

            return true;
        }
    }
}