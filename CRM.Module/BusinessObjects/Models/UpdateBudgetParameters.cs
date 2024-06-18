using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CRM.Module.BusinessObjects.Models
{
    [NonPersistent]
    public class UpdateBudgetParameters : BaseObject
    {
        public UpdateBudgetParameters(Session session) : base(session) { }

        private double _NewBudget;
        public double NewBudget
        {
            get { return _NewBudget; }
            set { SetPropertyValue(nameof(NewBudget), ref _NewBudget, value); }
        }
    }
}