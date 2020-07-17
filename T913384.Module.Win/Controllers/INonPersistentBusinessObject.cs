using System.Collections.Generic;
using DevExpress.ExpressApp;
namespace T913384.Module.Win.Controllers
{
    public interface INonPersistentBusinessObject
    {
        int Id { get; set; }
        List<INonPersistentBusinessObject> GetData(IObjectSpace space, object viewCurrentObject);


        void NPOnSaving(IObjectSpace persistentOs);
        string SearchText { get; set; }
        int CacheIndex { get; set; }
    }
}