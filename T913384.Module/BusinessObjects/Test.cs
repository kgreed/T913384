﻿using DevExpress.ExpressApp.Data;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
