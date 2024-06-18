using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering;
using static CRM.Module.BusinessObjects.ProjectTasks;

namespace CRM.Win.Controllers.Tasks
{
    public class FindTaskController : ObjectViewController<ListView, ProjectTasks>
    {
        private System.ComponentModel.IContainer components = null;

        public FindTaskController()
        {
            InitializeComponent();

            //Find Task and open details window
            var findBySubjectAction = new ParametrizedAction(this, "FindTaskAction", PredefinedCategory.View, typeof(string))
            {
                ImageName = "Action_Search",
                NullValuePrompt = "Find task"
            };

            findBySubjectAction.Execute += FindBySubjectAction_Execute;

            //Filter Tasks on choose 
            var filterTasksAction = new SingleChoiceAction(this, "FilterTasks", PredefinedCategory.Edit)
            {
                Caption = "Filter",
                ImageName = "MasterFilter",
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
            };

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                var statusCaption = Enum.GetName(typeof(StatusEnum), status);
                filterTasksAction.Items.Add(new ChoiceActionItem(statusCaption, status));
            }

            filterTasksAction.Execute += FilterTasksAction_Execute;

            //Reset Filter
            var resetFilterAction = new SimpleAction(this, "ResetFilter", PredefinedCategory.Edit)
            {
                Caption = "Reset Filter",
                ImageName = "Action_Clear"
            };
            resetFilterAction.Execute += ResetFilterAction_Execute;

        }

        private void FilterTasksAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var selectedStatuses = e.SelectedObjects
                                   .OfType<ProjectTasks>()
                                   .Select(task => task.Status)
                                   .FirstOrDefault();

            var collectionSource = View.CollectionSource;
            collectionSource.Criteria["FilterByStatus"] = CriteriaOperator.Parse("[Status] In (?)", selectedStatuses);


            View.Refresh();
        }

        private void ResetFilterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            View.CollectionSource.Criteria.Clear();
            View.Refresh();
        }


        private void FindBySubjectAction_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            var objectType = ((ListView)View).ObjectTypeInfo.Type;
            var objectSpace = Application.CreateObjectSpace(objectType);
            var paramValue = e.ParameterCurrentValue as string;
            object obj = objectSpace.FirstOrDefault<ProjectTasks>(task => task.TaskName.Contains(paramValue));
            if (obj != null)
            {
                var detailView = Application.CreateDetailView(objectSpace, obj);
                detailView.ViewEditMode = ViewEditMode.Edit;
                e.ShowViewParameters.CreatedView = detailView;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }



        #region Component Designer generated code
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        #endregion
    }
}
