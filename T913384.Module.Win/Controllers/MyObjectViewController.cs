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
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            var os = (NonPersistentObjectSpace)ObjectSpace;
            os.ObjectsGetting += os_ObjectsGetting;
            // Perform various tasks depending on the target View.
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
                collection.FetchObjects += DynamicCollection_FetchObjects;
                e.Objects = collection;
            
        }
        private List<MyClass> contacts;
        private void DynamicCollection_FetchObjects(object sender, FetchObjectsEventArgs e)
        {
       
                e.Objects =   contacts; // your collection of non-persistent objects.
                e.ShapeData = true; // set to true if the supplied collection is not already filtered and sorted.
           
        }

       
    }
}
