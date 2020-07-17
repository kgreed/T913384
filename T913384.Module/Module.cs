using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using T913384.Module.BusinessObjects;

namespace T913384.Module {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class T913384Module : ModuleBase {
        public T913384Module() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            contacts = new List<MyClass>();
            for (int i = 0; i < 20; i++) {
                contacts.Add(new MyClass() { ID = i, Name = "Name" + i });
            }
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.SetupComplete += Application_SetupComplete;
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
        private List<MyClass> contacts;
        private void Application_SetupComplete(object sender, EventArgs e) {
          //  Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }
        //private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e) {
        //    NonPersistentObjectSpace nonPersistentObjectSpace = e.ObjectSpace as NonPersistentObjectSpace;
        //    if (nonPersistentObjectSpace != null) {
        //        nonPersistentObjectSpace.ObjectsGetting += ObjectSpace_ObjectsGetting;
        //    }
        //}
        //private void ObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e) {
        //    if (e.ObjectType == typeof(MyClass)) {
        //        DynamicCollection collection = new DynamicCollection((IObjectSpace)sender, e.ObjectType, e.Criteria, e.Sorting, e.InTransaction);
        //        collection.FetchObjects += DynamicCollection_FetchObjects;
        //        e.Objects = collection;
        //    }
        //}
        //private void DynamicCollection_FetchObjects(object sender, FetchObjectsEventArgs e) {
        //    if (e.ObjectType == typeof(MyClass)) {
        //        e.Objects = contacts; // your collection of non-persistent objects.
        //        e.ShapeData = true; // set to true if the supplied collection is not already filtered and sorted.
        //    }
        //}
    }
}
