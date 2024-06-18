using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Company : BaseObject
    { 
        public Company(Session session)
            : base(session)
        {
        }

        public Company() { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string _CompanyName;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string CompanyName { 
            get { return _CompanyName; } 
            set { SetPropertyValue<string>(nameof(CompanyName), ref _CompanyName, value); }
        }

        private string _WebSite;
        public string WebSite
        {
            get { return _WebSite; }
            set { SetPropertyValue<string>(nameof(WebSite), ref _WebSite, value); }
        }

        private string _PhoneNumber;
        [RuleRequiredField]
        [RuleRegularExpression("RuleRegularExpression for PhoneNumber", DefaultContexts.Save, @"^\d{7}$")]

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { SetPropertyValue<string>(nameof(PhoneNumber), ref _PhoneNumber, value); }
        }

        private string _BillingAddress;
        public string BillingAddress
        {
            get { return _BillingAddress; }
            set { SetPropertyValue<string>(nameof(BillingAddress), ref _BillingAddress, value); }
        }

        private string _ShippingAddress;
        public string ShippingAddress
        {
            get { return _ShippingAddress; }
            set { SetPropertyValue<string>(nameof(ShippingAddress), ref _ShippingAddress, value); }
        }


        private bool _ShipToBilling;
        [ImmediatePostData] //немедленное сохранение в бд при обновлении
        public bool ShipToBilling
        {
            get { return _ShipToBilling; }
            set { SetPropertyValue<bool>(nameof(ShipToBilling), ref _ShipToBilling, value); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<Employee> Employee
        {
            get { return GetCollection<Employee>(nameof(Employee));}
        }

    }
}