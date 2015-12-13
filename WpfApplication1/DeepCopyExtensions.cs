using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public static class DeepCopyExtensions
    {
        /// <summary>
        /// オブジェクトのディープコピーの作成を行います。
        /// </summary>
        /// <see cref="http://d.hatena.ne.jp/tekk/20100131/1264913887"/>
        /// <remarks>
        /// クラスに [Serializable] 属性があるもののみ可能です。
        /// </remarks>
        /// <param name="target">ディープコピーを行うクラス　[Serializable]属性付き</param>
        /// <returns>コピーを行った結果</returns>
        public static object DeepCopy(this object target)
        {
            object result;
            BinaryFormatter b = new BinaryFormatter();

            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = b.Deserialize(mem);
            }
            finally
            {
                mem.Close();
            }

            return result;

        }

        /// <summary>
        /// 型付きでディープコピーの作成を行います。
        /// 内容はobject版のDeepCopyと同じです。
        /// </summary>
        /// <see cref="DeepCopy"/>
        /// <typeparam name="T">コピー後の型</typeparam>
        /// <param name="target">コピー元</param>
        /// <returns>コピー結果</returns>
        public static T DeepCopy<T>(T target)
        {

            T result;
            BinaryFormatter b = new BinaryFormatter();

            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = (T)b.Deserialize(mem);
            }
            finally
            {
                mem.Close();
            }

            return result;

        }
    }
}
