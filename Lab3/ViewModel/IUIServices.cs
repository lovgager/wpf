using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public interface IUIServices
    {
        bool ConfirmDelete();
        bool ConfirmSave();
        string SaveDialog();
        string OpenDialog();
        void UpdateBinding();
        void AddElement();
        bool NoErrorsAdd();
        bool NoErrorsDraw();
        void DrawElement();
    }
}
