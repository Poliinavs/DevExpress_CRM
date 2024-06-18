using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[NavigationItem("Project Details")]
    public class Project : BaseObject
    {
        public Project(Session session)
            : base(session)
        {
        }

        public Project()  { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string _ProjectName;
        [RuleRequiredField]

        public string ProjectName
        {
            get { return _ProjectName; }
            set { SetPropertyValue<string>(nameof(ProjectName), ref _ProjectName, value); }
        }

        
        private double _Budget;
        public double Budget
        {
            get { return _Budget; }
            set { SetPropertyValue<double>(nameof(Budget), ref _Budget, value); }
        }

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { SetPropertyValue<DateTime>(nameof(StartTime), ref _StartTime, value); }
        }

        private Employee _User;
        [Association]
        public Employee User
        {
            get { return _User; }
            set { SetPropertyValue(nameof(User), ref _User, value); }
        }

        [Association]
        public XPCollection<ProjectTasks> Tasks
        {
            get { return GetCollection<ProjectTasks>(nameof(Tasks)); }
        }
    }
}