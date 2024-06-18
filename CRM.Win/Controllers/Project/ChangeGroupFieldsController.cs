using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using CRM.Module.BusinessObjects.Models;

namespace CRM.Win.Controllers.Proj
{
    public class ChangeGroupFieldsController : ObjectViewController<ListView, Project>
    {
        private System.ComponentModel.IContainer components = null;

        public ChangeGroupFieldsController()
        {
            InitializeComponent();

            PopupWindowShowAction showNotesAction = new PopupWindowShowAction(this, "Change budjet", PredefinedCategory.Edit)
            {
                Caption = "Change budjet",
                ImageName = "BO_Invoice"
            };

            showNotesAction.CustomizePopupWindowParams += ShowNotesAction_CustomizePopupWindowParams;

        }

        private void ShowNotesAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var selectedProjects = ((ListView)View).SelectedObjects.Cast<Project>().ToList();

            if (!selectedProjects.Any())
            {
                throw new UserFriendlyException("No projects selected.");
            }

            var objectSpace = Application.CreateObjectSpace();
            var parameters = objectSpace.CreateObject<UpdateBudgetParameters>();

          

            var detailView = Application.CreateDetailView(objectSpace, parameters);
            detailView.ViewEditMode = ViewEditMode.Edit;

            e.View = detailView;
            e.DialogController.SaveOnAccept = true;
            e.DialogController.Accepting += (s, ea) => UpdateBudgetForSelectedProjects(selectedProjects, parameters.NewBudget);
        }

        private void UpdateBudgetForSelectedProjects(List<Project> projects, double newBudget)
        {
            var objectSpace = View.ObjectSpace; 
            foreach (var project in projects)
            {
                var projectToUpdate = objectSpace.GetObjectByKey<Project>(project.Oid);
                if (projectToUpdate != null)
                {
                    projectToUpdate.Budget = newBudget;
                }
            }

            objectSpace.CommitChanges(); 
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
