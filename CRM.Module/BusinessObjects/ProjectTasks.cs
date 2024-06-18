using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [RuleCriteria("EndDateAfterStartDate", DefaultContexts.Save, "EndDate >= StartDate", "End date must be after start date.")]
    public class ProjectTasks : BaseObject
    {
        public ProjectTasks(Session session)
            : base(session)
        {
            /*    EnumProcessingHelper.RegisterEnum(typeof(StatusEnum));
                EnumProcessingHelper.RegisterEnum(typeof(LevelEnum));*/
        }

        public override void AfterConstruction()
        {

            base.AfterConstruction();
            Status = StatusEnum.ToDO; //по умолчанию при создании

        }

        private string _TaskName;
        [RuleRequiredField]
        public string TaskName
        {
            get { return _TaskName; }
            set { SetPropertyValue<string>(nameof(TaskName), ref _TaskName, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue<string>(nameof(Description), ref _Description, value); }
        }

        private int _Cost;
        public int Cost
        {
            get { return _Cost; }
            set { SetPropertyValue<int>(nameof(Cost), ref _Cost, value); }
        }

        private DateTime _StartDate;
        [RuleRequiredField]
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { SetPropertyValue<DateTime>(nameof(StartDate), ref _StartDate, value); }
        }

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { SetPropertyValue<DateTime>(nameof(EndDate), ref _EndDate, value); }
        }

        private StatusEnum _Status;
        public StatusEnum Status
        {
            get { return _Status; }
            set { SetPropertyValue<StatusEnum>(nameof(Status), ref _Status, value); }
        }

        private LevelEnum _Level;
        public LevelEnum Level
        {
            get { return _Level; }
            set { SetPropertyValue<LevelEnum>(nameof(Level), ref _Level, value); }
        }

        private Project _ProjectName;
        [Association]
        public Project ProjectName
        {
            get { return _ProjectName; }
            set { SetPropertyValue(nameof(ProjectName), ref _ProjectName, value); }
        }

        public enum LevelEnum
        {
            [ImageName("WhiteGreenColorScale")]
            Low = 0,
            [ImageName("YellowGreenColorScale")]
            Normal = 1,
            [ImageName("WhiteRedColorScale")]
            Hard = 2
        }

        public enum StatusEnum
        {
            [ImageName("State_Task_NotStarted")]
            ToDO = 0,
            [ImageName("State_Task_Deferred")]
            InProgress = 1,
            [ImageName("State_Validation_Valid")]
            Completed = 2,
            [ImageName("State_Validation_Valid")]
            Blocked = 3,
            [ImageName("State_Validation_Valid")]
            Review = 4,
            [ImageName("State_Validation_Valid")]
            Redo = 5,
        }
    }
}