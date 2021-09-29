using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCompare.Model;

namespace XmlCompare.Presenter
{
    interface IReportPresenter
    {
        event Action OnReportSaveError;

        void OnMakeReportClick(ICompare Compare);

    }
}
