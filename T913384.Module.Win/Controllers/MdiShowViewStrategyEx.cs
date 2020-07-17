using System.Diagnostics;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win;
using T913384.Module.BusinessObjects;
namespace T913384.Module.Win.Controllers
{
    public class MdiShowViewStrategyEx : MdiShowViewStrategy
    {
        public MdiShowViewStrategyEx(XafApplication application) : base(application)
        {
            CustomCanCreateViewDocumentControlDescription -= MdiShowViewStrategyNP_CustomCanCreateViewDocumentControlDescription;
            CustomCanCreateViewDocumentControlDescription += MdiShowViewStrategyNP_CustomCanCreateViewDocumentControlDescription;
            CustomCanProcessDocumentControlDescription -= MdiShowViewStrategyNP_CustomCanProcessDocumentControlDescription;
            CustomCanProcessDocumentControlDescription += MdiShowViewStrategyNP_CustomCanProcessDocumentControlDescription;
        }
        protected override bool IsCompatibleWindow(WinWindow existingWindow, View view, ViewShortcut viewShortcut)
        {

            if (existingWindow.View != null && view.Id == Application.FindListViewId(typeof(MyClass)) && existingWindow.View.Tag != view.Tag)
            {
                return false;
            }
            return base.IsCompatibleWindow(existingWindow, view, viewShortcut);
        }

        void MdiShowViewStrategyNP_CustomCanProcessDocumentControlDescription(object sender, CustomCanProcessDocumentControlDescriptionEventArgs e)
        {
            var viewShortcut = ViewShortcut.FromString(e.DocumentControlDescription.SerializedControl);
            Debug.Print(viewShortcut["Tag"]);
            var modelView = base.Application.FindModelView(viewShortcut.ViewId);


            e.Result = IsTaskResultSerialisableListView(e.Result, modelView);

            e.Handled = true;
        }
        void MdiShowViewStrategyNP_CustomCanCreateViewDocumentControlDescription(object sender, CustomCanCreateViewDocumentControlDescriptionEventArgs e)
        {
            e.Result = IsTaskResultSerialisableListView(e.Result, e.View.Model);
            e.Handled = true;
        }


        private static bool IsTaskResultSerialisableListView(bool defaultResult, IModelView modelView)
        {
            if (!(modelView is IModelListView)) return defaultResult;
            var type = ((IModelListView)modelView).ModelClass.TypeInfo.Type;
            return type == typeof(MyClass);
        }
    }
}