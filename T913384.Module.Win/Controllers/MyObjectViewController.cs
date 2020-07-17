using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using T913384.Module.BusinessObjects;
namespace T913384.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class MyObjectViewController : ObjectViewController<ListView, MyClass>
    {
        public MyObjectViewController()
        {
            TargetObjectType = typeof(MyClass);
            contacts = new List<MyClass>();
            for (int i = 0; i < 20; i++)
            {
                contacts.Add(new MyClass() { ID = i, Name = "Name" + i });
            }
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
            // Perform various tasks depending on the target View.
        }

        private void View_CreateCustomCurrentObjectDetailView(object sender, CreateCustomCurrentObjectDetailViewEventArgs e)
        {
            if (e.ListViewCurrentObject == null) return;
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


            
        }
        private void os_ObjectsGetting(object sender, ObjectsGettingEventArgs e)
        {
         
                DynamicCollection collection = new DynamicCollection((IObjectSpace)sender, e.ObjectType, e.Criteria, e.Sorting, e.InTransaction);
                collection.FetchObjects -= DynamicCollection_FetchObjects;
                collection.FetchObjects += DynamicCollection_FetchObjects;
                e.Objects = collection;
            
        }
        private List<MyClass> contacts;
        private void DynamicCollection_FetchObjects(object sender, FetchObjectsEventArgs e)
        {
            if (View == null) return;


            var filterController = Frame.GetController<FilterController>();
            var selectedFilterItem = filterController.SetFilterAction.SelectedItem;
            if (selectedFilterItem == null) return;
            var caption = selectedFilterItem.Caption.ToLower();
            //var filterNum = ToDoListFilterEnum.IncludeTickedBeforeToday;
            //if (caption.Contains("green")) filterNum = ToDoListFilterEnum.GreenLightsOnly;
            //if (caption.Contains("hide")) filterNum = ToDoListFilterEnum.HideTickedBeforeToday;


            e.Objects =   contacts; // your collection of non-persistent objects.
                e.ShapeData = true; // set to true if the supplied collection is not already filtered and sorted.
           
        }

       
    }
}
