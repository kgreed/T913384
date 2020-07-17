using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Win.Editors;
using T913384.Module.BusinessObjects;
namespace T913384.Module.Win.Controllers
{
    public partial class NonPersistentController : ObjectViewController<ListView, INonPersistentBusinessObject>
    {
        private DevExpress.ExpressApp.Actions.ParametrizedAction parametrizedAction1;
        private IContainer components;
        private static List<INonPersistentBusinessObject> objectsCache;
        private string SearchText;
        static NonPersistentController()
        {
        }
        public NonPersistentController()
            : base()
        {
            InitializeComponent();
            this.TargetObjectType = typeof(INonPersistentBusinessObject);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.parametrizedAction1 = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            // 
            // parametrizedAction1
            // 
            this.parametrizedAction1.Caption = "Text Search";
            this.parametrizedAction1.Category = "RecordsNavigation";
            this.parametrizedAction1.ConfirmationMessage = null;
            this.parametrizedAction1.Id = "TextSearch";
            this.parametrizedAction1.NullValuePrompt = null;
            this.parametrizedAction1.ShortCaption = null;
            this.parametrizedAction1.ToolTip = null;
            this.parametrizedAction1.Execute += new DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventHandler(this.ParametrizedAction1_Execute);
            // 
            // NonPersistentController
            // 
            this.Actions.Add(this.parametrizedAction1);
        }

        private void ParametrizedAction1_Execute(object sender, DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventArgs e)
        {
            SearchText = (String)e.ParameterCurrentValue;
            View.ObjectSpace.Refresh();
        }

        private void ObjectSpace_CustomRefresh(object sender, HandledEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            LoadObjectsCache(objectSpace);
            objectSpace.ReloadCollection(objectsCache);
        }

        private void NonPersistentObjectSpace_ObjectsGetting(Object sender, ObjectsGettingEventArgs e)
        {
            ITypeInfo info = XafTypesInfo.Instance.FindTypeInfo(e.ObjectType);

            if (!info.Implements<INonPersistentBusinessObject>()) return;
            IObjectSpace objectSpace = (IObjectSpace)sender;
            BindingList<INonPersistentBusinessObject> objects = new BindingList<INonPersistentBusinessObject>
            {
                AllowNew = false,
                AllowEdit = true,
                AllowRemove = false
            };

            LoadObjectsCache(objectSpace);
            foreach (INonPersistentBusinessObject obj in objectsCache)
            {
                objects.Add(objectSpace.GetObject(obj));
            }
            e.Objects = objects;
        }

        private void LoadObjectsCache(IObjectSpace objectSpace)
        {

            var npObj = (INonPersistentBusinessObject)Activator.CreateInstance(View.ObjectTypeInfo.Type);
            npObj.SearchText = SearchText;
            objectsCache = npObj.GetData(objectSpace, View.CurrentObject);
        }

        private void UpdateCacheFromObjectSpace() // causes view refresh
        {

            foreach (INonPersistentBusinessObject obj in ObjectSpace.ModifiedObjects)
            {
                var cacheObj = objectsCache.Find(x => x.Id == obj.Id);
                objectsCache[cacheObj.CacheIndex] = obj;
            }


        }

        private void NonPersistentObjectSpace_ObjectGetting(object sender, ObjectGettingEventArgs e)
        {
            if (e.SourceObject is IObjectSpaceLink)
            {
                ((IObjectSpaceLink)e.TargetObject).ObjectSpace = (IObjectSpace)sender;
            }
        }
        private void NonPersistentObjectSpace_ObjectByKeyGetting(object sender, ObjectByKeyGettingEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            foreach (Object obj in objectsCache)
            {
                if (obj.GetType() != e.ObjectType || !Equals(objectSpace.GetKeyValue(obj), e.Key)) continue;
                e.Object = objectSpace.GetObject(obj);


                break;
            }
        }

        private void NonPersistentObjectSpace_Committing(Object sender, CancelEventArgs e)
        {
            var currentobj = View.CurrentObject as IXafEntityObject;
            var objAsnp = currentobj as INonPersistentBusinessObject;


            Trace.WriteLine($"objAsNp id:{objAsnp?.Id} index {objAsnp?.CacheIndex}");


            IObjectSpace objectSpace = (IObjectSpace)sender;
            var persistentOS = ((NonPersistentObjectSpace)objectSpace).AdditionalObjectSpaces.FirstOrDefault();
            foreach (Object obj in objectSpace.ModifiedObjects)
            {
                if (!(obj is INonPersistentBusinessObject)) continue;
                if (objectSpace.IsNewObject(obj))
                {
                    objectsCache.Add((INonPersistentBusinessObject)obj);
                }
                else if (objectSpace.IsDeletedObject(obj))
                {
                    objectsCache.Remove((INonPersistentBusinessObject)obj);
                }
                else
                {
                    ((NonPersistentObjectSpace)objectSpace).GetObject(obj);  //https://www.devexpress.com/Support/Center/Question/Details/T750176/iobjectspace-link-additional-objectspace-not-working-for-saving-non-persistent-objects
                    ((INonPersistentBusinessObject)obj).NPOnSaving(persistentOS);

                }
            }
            persistentOS.CommitChanges();

        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            GridListEditor editor = View.Editor as GridListEditor;
            if (editor != null && editor.GridView != null && editor.GridView.FocusedRowHandle >= 0)
                editor.GridView.RefreshRow(editor.GridView.FocusedRowHandle);
        }


        protected override void OnActivated()
        {
            base.OnActivated();

            if (!(ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace)) return;
            nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpace_ObjectsGetting;
            nonPersistentObjectSpace.ObjectByKeyGetting += NonPersistentObjectSpace_ObjectByKeyGetting;
            nonPersistentObjectSpace.ObjectGetting += NonPersistentObjectSpace_ObjectGetting;
            nonPersistentObjectSpace.Committing += NonPersistentObjectSpace_Committing;
            var persistentOS = this.Application.CreateObjectSpace(typeof(MyClass));
            nonPersistentObjectSpace.AdditionalObjectSpaces.Add(persistentOS);

            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.CustomRefresh += ObjectSpace_CustomRefresh;
            View.CreateCustomCurrentObjectDetailView += NonPersistentController_CreateCustomCurrentObjectDetailView;


            ObjectSpace.Refresh();

            //Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }

        private void NonPersistentController_CreateCustomCurrentObjectDetailView(object sender, CreateCustomCurrentObjectDetailViewEventArgs e)
        {

            UpdateCacheFromObjectSpace();
            //var gridEditor = View.Editor as GridListEditor;
            //var objects = gridEditor.GetSelectedObjects();
            //var focussedObj = gridEditor.FocusedObject as INonPersistent;
            //var curObj = View.CurrentObject as INonPersistent;
            //if (focussedObj?.Id != curObj?.Id)
            //{

            //    Trace.WriteLine($"focus:{focussedObj?.Id} cur:{curObj?.Id}");
            //}
            //Trace.WriteLine($"Focussed obj is {focussedObj?.Id}");
            //foreach (INonPersistent npObj in objects)
            //{

            //    gridEditor.FocusedObject = npObj;
            //    Trace.WriteLine($"obj id:{npObj.Id} index {npObj.CacheIndex}");
            //    View.CurrentObject = npObj;
            //}

            var obj = View.CurrentObject as IXafEntityObject;
            var objAsnp = obj as INonPersistentBusinessObject;
            Trace.WriteLine($"objAsNp id:{objAsnp?.Id} index {objAsnp?.CacheIndex}");
            obj?.OnLoaded();
            Trace.WriteLine("");



        }

        protected override void OnDeactivated()
        {
            if (ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace)
            {
                nonPersistentObjectSpace.ObjectsGetting -= NonPersistentObjectSpace_ObjectsGetting;
                nonPersistentObjectSpace.ObjectByKeyGetting -= NonPersistentObjectSpace_ObjectByKeyGetting;
                nonPersistentObjectSpace.ObjectGetting -= NonPersistentObjectSpace_ObjectGetting;
                nonPersistentObjectSpace.Committing -= NonPersistentObjectSpace_Committing;
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                ObjectSpace.CustomRefresh -= ObjectSpace_CustomRefresh;
                View.CreateCustomCurrentObjectDetailView -= NonPersistentController_CreateCustomCurrentObjectDetailView;
                var persistentOS = nonPersistentObjectSpace.AdditionalObjectSpaces.FirstOrDefault();
                if (persistentOS != null)
                {
                    nonPersistentObjectSpace.AdditionalObjectSpaces.Remove(persistentOS);
                    persistentOS.Dispose();
                }
            }

            base.OnDeactivated();
            //Application.ObjectSpaceCreated -= Application_ObjectSpaceCreated;
        }

    }
}