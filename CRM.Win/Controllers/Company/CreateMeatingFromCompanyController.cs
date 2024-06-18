using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;

namespace CRM.Win.Controllers.Compan
{
    public class CreateMeatingFromCompanyController : ObjectViewController<ListView, Company>
    {
        private System.ComponentModel.IContainer components = null;

        public CreateMeatingFromCompanyController()
        {
            InitializeComponent();

            PopupWindowShowAction showNotesAction = new PopupWindowShowAction(this, "Create Meeting", PredefinedCategory.Edit)
            {
                Caption = "Create Meeting",
                ImageName = "BO_Scheduler"
            };

            showNotesAction.CustomizePopupWindowParams += ShowNotesAction_CustomizePopupWindowParams;
        }

        private void ShowNotesAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var company = (Company)View.CurrentObject;
            var objectSpace = View.ObjectSpace.CreateNestedObjectSpace();
            var meeting = objectSpace.CreateObject<Meeting>();
            meeting.Company = objectSpace.GetObject<Company>(company);
            e.View = Application.CreateDetailView(objectSpace, meeting);
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
