using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class Meeting : Event
    { 
        public Meeting(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private Company _Company;
        public Company Company
        {
            get { return _Company; }
            set { SetPropertyValue(nameof(Company), ref _Company, value); }
        }

        private Employee _PrimaryCompany;
        [DataSourceProperty("Company.Contacts")]
        public Employee PrimaryCompany
        {
            get { return _PrimaryCompany; }
            set { SetPropertyValue(nameof(PrimaryCompany), ref _PrimaryCompany, value); }
        }
    }
}