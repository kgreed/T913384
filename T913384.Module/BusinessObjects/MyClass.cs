using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
namespace T913384.Module.BusinessObjects
{
    [DomainComponent, DefaultClassOptions]
    [ListViewFilter("Green lights or soon only", "true", "Green Lights Only", true, Index = 0)]
    [ListViewFilter("Hide ticked before today", "true", "Hide Ticked Before Today", false, Index = 1)]
    [ListViewFilter("Include ticked before today", "true", "Include Ticked Before Today", false, Index = 2)]
    public class MyClass {

        string name;
        int iD;
        [DevExpress.ExpressApp.Data.Key]
        public int ID {
            get => iD;
            set => iD = value;
        }
        
        public string Name {
            get => name;
            set => name = value;
        }
        public decimal Priority { get; set; }
        public bool TagToSetPriority { get; set; }
    }
}