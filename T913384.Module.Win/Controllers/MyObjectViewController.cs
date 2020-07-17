using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.Export;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using T913384.Module.BusinessObjects;
namespace T913384.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class MyObjectViewController : ObjectViewController<ListView, XMyClass>
    {
        public MyObjectViewController()
        {
            TargetObjectType = typeof(MyClass);
          
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            var os = (NonPersistentObjectSpace)ObjectSpace;
            os.ObjectsGetting += os_ObjectsGetting;
            View.CreateCustomCurrentObjectDetailView += View_CreateCustomCurrentObjectDetailView;
            ObjectSpace.Refresh();
            Frame.GetController<FilterController>()?.Active.SetItemValue("Workaround T890466", false);
            Frame.GetController<FilterController>()?.Active.RemoveItem("Workaround T890466");

            var refreshController = Frame.GetController<RefreshController>();
            refreshController.RefreshAction.Executing += RefreshAction_Executing;
            refreshController.RefreshAction.Executed += RefreshAction_Executed;
            // Perform various tasks depending on the target View.
        }

        private int? focusedRowHandle;
        private void RefreshAction_Executed(object sender, ActionBaseEventArgs e)
        {
          
            if (focusedRowHandle != null)
            {
                var gridEditor = View.Editor as GridListEditor;
                gridEditor.GridView.UnselectRow(0);
                gridEditor.GridView.FocusedRowHandle = (int)focusedRowHandle;
               
                gridEditor.GridView.SelectRow((int)focusedRowHandle);
            }

        }

        private void RefreshAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var gridEditor = View.Editor as GridListEditor;
            focusedRowHandle = gridEditor.GridView?.FocusedRowHandle;
        }

        private void View_CreateCustomCurrentObjectDetailView(object sender, CreateCustomCurrentObjectDetailViewEventArgs e)
        {
            if (e.ListViewCurrentObject == null) return;
            if (!(e.ListViewCurrentObject is XMyClass currentRec)) throw new Exception("Unexpected");
            // var os = Application.CreateObjectSpace(typeof(JobExt));

            //var connectionString = ConfigurationManager.ConnectionStrings["ConnectionJobTalk"].ConnectionString;
            //switch (currentRec.TemplateId)
            //{
            //    case 50:
            //    {
            //        if (currentRec.Pricing == null)
            //        {
            //            var snapInController = new VivUniversalController(connectionString);

            //            // var jobId = 127264;
            //            snapInController.Load(currentRec.JobId);
            //            currentRec.Pricing = snapInController.modelSpecLine.uniModel.PricingViv;
            //        }

            //        break;
            //    }
            //    case 61:
            //    {
            //        if (currentRec.QtyBreakPricing == null)
            //        {
            //            var snapInController = new VivQtyBreakController(connectionString);
            //            int staffId;
            //            using (var db = new JobTalkDbContext())
            //            {
            //                staffId = HandyBusinessFunctions.GetStaffIdForLoggedInUser(db, true);
            //            }

            //            snapInController.Load(currentRec.JobId, staffId);
            //            currentRec.QtyBreakPricing = snapInController.Model.ModelBo;
            //        }

            //        break;
            //    }
            //}
            // var os = Application.CreateObjectSpace(typeof(JobExt));
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            var os = (NonPersistentObjectSpace)ObjectSpace;
            os.ObjectsGetting -= os_ObjectsGetting;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();

            View.CreateCustomCurrentObjectDetailView -= View_CreateCustomCurrentObjectDetailView;
            var refreshController = Frame.GetController<RefreshController>();
            refreshController.RefreshAction.Executing -= RefreshAction_Executing;
            refreshController.RefreshAction.Executed -= RefreshAction_Executed;

        }
        private void os_ObjectsGetting(object sender, ObjectsGettingEventArgs e)
        {
                //var gridEditor = View.Editor as GridListEditor;
                //var focussedRowHandle = gridEditor.GridView?.FocusedRowHandle;
                DynamicCollection collection = new DynamicCollection((IObjectSpace)sender, e.ObjectType, e.Criteria, e.Sorting, e.InTransaction);
                collection.FetchObjects -= DynamicCollection_FetchObjects;
                collection.FetchObjects += DynamicCollection_FetchObjects;
                e.Objects = collection;
                //if (focussedRowHandle != null)
                //{
                //    gridEditor.GridView.FocusedRowHandle = (int)focussedRowHandle;
                //}
        }
        private List<XMyClass> contacts;
        private void DynamicCollection_FetchObjects(object sender, FetchObjectsEventArgs e)
        {
            MakeContacts();
            e.Objects = contacts;
            e.ShapeData = true;
            
        }

        private  void MakeContacts()
        {
            if (contacts == null)
            {
                contacts = new List<XMyClass>();
            }
            else
            {
                contacts.RemoveAll(x => x.ID > 0);
            }
             
          
            for (int i = 0; i < 20; i++)
            {
                contacts.Add(new XMyClass() { ID = i, Name = "Name" + i });
            }

            return ;
        }
    }
}
