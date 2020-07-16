using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using T913384.Module.BusinessObjects;

namespace T913384.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            string name = "MyName";
            Test theObject = ObjectSpace.FindObject<Test>(CriteriaOperator.Parse("Name=?", name));
            if (theObject == null) {
                theObject = ObjectSpace.CreateObject<Test>();
                theObject.Name = name;
                for (int i = 0; i < 20; i++) {
                    theObject = ObjectSpace.CreateObject<Test>();
                    theObject.Name = name + i;
                }
            }

            ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
