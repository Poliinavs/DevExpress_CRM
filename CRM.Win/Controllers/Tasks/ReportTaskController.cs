using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.XtraReports.UI;
using CRM.Module.BusinessObjects;
using System.IO;

namespace CRM.Win.Controllers.Task
{
    public class ReportTaskController : ObjectViewController<ListView, ProjectTasks>
    {
        private System.ComponentModel.IContainer components = null;

        public ReportTaskController()
        {
            InitializeComponent();

            var showReportAction = new SimpleAction(this, "ShowReport", PredefinedCategory.Edit)
            {
                Caption = "Show Report",
                ImageName = "BO_Report"
            };

            showReportAction.Execute += ShowReportAction_Execute;

        }

        private void ShowReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var objectSpace = Application.CreateObjectSpace();
            string reportFilePath = "..\\..\\..\\Resourse\\Report\\Tasks.repx";

            if (File.Exists(reportFilePath))
            {
                var report = XtraReport.FromFile(reportFilePath, true);
                var dataCollection = objectSpace.GetObjects<ProjectTasks>();
                report.DataSource = dataCollection;
                var printTool = new ReportPrintTool(report);
                printTool.ShowPreview();
            }
            else
            {
                throw new UserFriendlyException($"Report '{reportFilePath}' not found.");
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
