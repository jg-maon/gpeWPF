using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public interface IUndoRedoCommand
    {
        /// <summary>
        /// 呼び出し
        /// </summary>
        void Invoke();

        /// <summary>
        /// 元に戻す
        /// </summary>
        void Undo();

        /// <summary>
        /// やり直し
        /// </summary>
        void Redo();

    }
}
