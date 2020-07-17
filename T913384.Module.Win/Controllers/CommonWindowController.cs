using System;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Templates.Ribbon;
namespace T913384.Module.Win.Controllers
{
    public partial class CommonWindowController : WindowController
    {
        public CommonWindowController()
        {
            //InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            Window.ViewChanged -= Window_ViewChanged;
            Window.ViewChanged += Window_ViewChanged;
        }

        void Window_ViewChanged(object sender, EventArgs e)
        {
            var v2 = Window.Template as DetailRibbonFormV2;
            if (v2 != null)
            {
                v2.Shown -= form_Shown;
                v2.Shown += form_Shown;
            }

        }

        private void form_Shown(object sender, EventArgs e)
        {
            if (!(Window.View is DetailView) || !(sender is Form)) return;
            var form = (Form)sender;
            form.WindowState = FormWindowState.Maximized;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}