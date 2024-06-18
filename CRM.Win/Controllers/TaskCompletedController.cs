using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using static CRM.Module.BusinessObjects.ProjectTasks;

namespace CRM.Win.Controllers
{
    public class TaskCompletedController : ViewController
    {
        SimpleAction complete;

        public TaskCompletedController()
        {
            TargetObjectType = typeof(ProjectTasks);

            complete = new SimpleAction(this, "Complete", PredefinedCategory.Edit);
            complete.ImageName = "ApplyChanges";
            complete.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            complete.Execute += Complete_Execute;
        }

        private void Complete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var task = (ProjectTasks)e.CurrentObject;
            if (task != null)
            {
                task.Status = StatusEnum.Completed;
            }

            if (this.ObjectSpace.IsModified)
            {
                this.ObjectSpace.CommitChanges();
            }
        }


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }

    }
}
