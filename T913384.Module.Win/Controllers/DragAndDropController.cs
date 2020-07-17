using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Data.Filtering;
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
using DevExpress.Utils.Behaviors;
using DevExpress.Utils.DragDrop;
using DevExpress.XtraGrid.Views.Grid;
using T913384.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
namespace T913384.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DragAndDropController : ViewController<ListView>
    {
        private BehaviorManager behaviorManager1;
        DragDropBehavior behaviorField = null;
        public DragAndDropController()
        {
            InitializeComponent();
            TargetObjectType = typeof(MyClass);
            TargetViewType = ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            View.EditorChanged += View_EditorChanged;
            SetupEditor();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        private void View_EditorChanged(object sender, EventArgs e)
        {
            SetupEditor();

        }
        private void SetupEditor()
        {
            if (View.Editor == null) return;
            View.Editor.ControlsCreated += Editor_ControlsCreated;

        }

        private void Editor_ControlsCreated(object sender, EventArgs e)
        {

            if (!(View.Editor is GridListEditor editor) || editor.GridView == null) return;

            editor.Grid.AllowDrop = false;
            behaviorManager1 = new BehaviorManager();

            behaviorManager1.Attach(editor.GridView, MyBehaviorSettings());

        }
        private Action<DragDropBehavior> MyBehaviorSettings()
        {
            return behavior =>
            {
                behavior.Properties.AllowDrop = true;
                behavior.Properties.InsertIndicatorVisible = true;
                behavior.Properties.PreviewVisible = true;
                behavior.DragOver += Behavior_DragOver;
                behavior.DragDrop += Behavior_DragDrop;
                behavior.BeginDragDrop += Behavior_BeginDragDrop;
                behaviorField = behavior; // so we can release the events later
            };
        }

        private void Behavior_DragOver(object sender, DragOverEventArgs e)
        {
            var args = DragOverGridEventArgs.GetDragOverGridEventArgs(e);
            e.InsertType = args.InsertType;
            e.InsertIndicatorLocation = args.InsertIndicatorLocation;
            e.Action = args.Action;
            Cursor.Current = args.Cursor;
            args.Handled = true;
        }

        private void Behavior_BeginDragDrop(object sender, BeginDragDropEventArgs e)
        {
            if (!(View.Editor is GridListEditor editor) || editor.GridView == null) return;

            var sortedColumn = editor.GridView.SortedColumns.FirstOrDefault();
            if (sortedColumn != null &&
                (sortedColumn.Name != "Priority" || sortedColumn.SortOrder != ColumnSortOrder.Ascending))
                MessageBox.Show("You need to be sorted by Priority (Ascending) to use prioritization");
        }
        private void Behavior_DragDrop(object sender, DragDropEventArgs e)
        {
            //https://www.devexpress.com/Support/Center/Question/Details/T583428/how-to-use-new-dragdropbehavior-in-xaf

            try
            {
                var targetGrid = e.Target as GridView;
                if (e.Action == DragDropActions.None) return;
                if (targetGrid == null) return;
                var hitPoint = targetGrid.GridControl.PointToClient(Cursor.Position);
                var hitInfo = targetGrid.CalcHitInfo(hitPoint);
                var droppedOnRowHandle = hitInfo.RowHandle;
                var droppedOnRow = targetGrid.GetRow(droppedOnRowHandle);
                if (!(droppedOnRow is MyClass droppedOnTask)) return;

               // RePrioritiseIfNeeded(targetGrid);

                TagToSetPriority(e.InsertType, targetGrid, droppedOnTask);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }


        private void TagToSetPriority(InsertType insertType, GridView view, MyClass droppedOnTask)
        {
            int[] selectedRows = view.GetSelectedRows();
            var addnum = insertType == InsertType.After ? 1 : -1;
            addnum *= 10;
            if (insertType == InsertType.Before) Array.Reverse(selectedRows);



            var offset = addnum;
            foreach (var rowNum in selectedRows)
            {
                var row = view.GetRow(rowNum);
                if (!(row is MyClass task)) throw new Exception("Expected SalesOrderLineTask");
                task.Priority = droppedOnTask.Priority + offset;
                task.TagToSetPriority = true;
                //offset = offset + 10;
                offset += addnum;
            }
            // SaveTasksAndRefresh(view, true);
        }

        protected override void OnDeactivated()
        {
            if (behaviorField != null)
            {
                behaviorField.DragOver -= Behavior_DragOver;
                behaviorField.DragDrop -= Behavior_DragDrop;
                behaviorField.BeginDragDrop -= Behavior_BeginDragDrop;
            }

            View.EditorChanged -= View_EditorChanged;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
