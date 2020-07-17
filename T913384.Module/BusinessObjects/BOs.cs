using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.SystemModule;
namespace T913384.Module.BusinessObjects {
    [DefaultClassOptions]

    public class Test : BaseObject {
        public Test(Session session) : base(session) { }


        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
    }
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
