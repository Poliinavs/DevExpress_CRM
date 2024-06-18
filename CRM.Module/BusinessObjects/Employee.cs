using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("FullName")]

    public class Employee : BaseObject
    {
        public Employee(Session session)
           : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public string FullName
        {
            get
            {
                return ObjectFormatter.Format("{FirstName} {LastName}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
            }
        }

        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { SetPropertyValue<string>(nameof(FirstName), ref _FirstName, value); }
        }

        private string _LastName;
        [RuleRequiredField]

        public string LastName
        {
            get { return _FirstName; }
            set { SetPropertyValue<string>(nameof(LastName), ref _LastName, value); }
        }

        private string _PhoneNumber;
/*        [RuleRegularExpression("PhoneNumber", DefaultContexts.Save, @"^\d{7}$")]
*/        public string PhoneNumber
        {
            get { return _FirstName; }
            set { SetPropertyValue<string>(nameof(PhoneNumber), ref _PhoneNumber, value); }
        }

        private string _EmailAdress;
        /*        [RuleRegularExpression("RuleRegularExpression for Email", DefaultContexts.Save, @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b", CustomMessageTemplate = "Invalid email address.")]
        */
        public string EmailAdress
        {
            get { return _FirstName; }
            set { SetPropertyValue<string>(nameof(EmailAdress), ref _EmailAdress, value); }
        }

        private byte[] _Image;
        [ImageEditor]
        public byte[] Image
        {
            get { return _Image; }
            set { SetPropertyValue<byte[]>(nameof(Image), ref _Image, value); }
        }


        private Company _Company;
        [Association]
        public Company Company
        {
            get { return _Company; }
            set { SetPropertyValue(nameof(Company), ref _Company, value); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<Project> Project
        {
            get { return GetCollection<Project>(nameof(Project)); }
        }
    }
}