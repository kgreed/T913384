using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
namespace T913384.Module.Win.Controllers
{
    public partial class CommonListViewController : ViewController
    {
        private const string cActionDisableReason = "CommonListViewController disabled";
        private const string cActionEnableReason = "CommonListViewController enabled";

        public CommonListViewController()
        {
            // InitializeComponent();
        }

        

        DevExpress.XtraEditors.Controls.Localizer defaultLocalizer;

        protected override void OnActivated()
        {
            base.OnActivated();


            var ct2 = Frame.GetController<FilterController>();
            //   ct2?.FullTextFilterAction.Active.SetItemValue(cActionDisableReason, false);
            ct2?.FullTextFilterAction.Active.SetItemValue(cActionEnableReason, true);
            ct2?.FullTextFilterAction.Enabled.SetItemValue(cActionEnableReason, true);
            defaultLocalizer = DevExpress.XtraEditors.Controls.Localizer.Active as DevExpress.XtraEditors.Controls.Localizer;
            DevExpress.XtraEditors.Controls.Localizer.Active = new MyEditorLocalizer();

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();



            //var ct2 = Frame.GetController<FilterController>();
            //ct2?.FullTextFilterAction.Active.RemoveItem(cActionDisableReason);
            var ct2 = Frame.GetController<FilterController>();
            ct2?.FullTextFilterAction.Active.RemoveItem(cActionEnableReason);
            ct2?.FullTextFilterAction.Enabled.RemoveItem(cActionEnableReason);

            DevExpress.XtraEditors.Controls.Localizer.Active = defaultLocalizer;
        }
        public class MyEditorLocalizer : DevExpress.XtraEditors.Controls.Localizer
        {
            public override string GetLocalizedString(DevExpress.XtraEditors.Controls.StringId id)
            {
                return id == DevExpress.XtraEditors.Controls.StringId.DataEmpty ? "" : base.GetLocalizedString(id);
            }
        }



    }
}