using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.SystemModule;
namespace T913384.Module.Win.Controllers
{
    public class MyWinWindowTemplateController : WinWindowTemplateController
    {
        protected override void OnDocumentActivated(DevExpress.XtraBars.Docking2010.Views.DocumentEventArgs e)
        {
            base.OnDocumentActivated(e);
            if (!(e.Document.Form is IViewHolder)) return;
            // View activeView = ((IViewHolder)e.Document.Form).View; // does not build
            var activeView = ((IViewHolder)e.Document.Form).View;
            activeView.RefreshDataSource();
        }
    }
}