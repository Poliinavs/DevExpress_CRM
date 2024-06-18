using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using CRM.Module.BusinessObjects;
using DevExpress.XtraReports.UI;
using System.IO;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;

namespace CRM.Win.Controllers.Proj
{
    public class ShowReportController : ObjectViewController<ListView, Project>
    {
        private System.ComponentModel.IContainer components = null;

        public ShowReportController()
        {
            InitializeComponent();

            var showAction = new SingleChoiceAction(this, "ShowReportOrDashboard", PredefinedCategory.Edit)
            {
                Caption = "Show Report or Dashboard",
                ImageName = "BO_Report",
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };

            showAction.Items.Add(new ChoiceActionItem("Show Report", "Report"));
            showAction.Items.Add(new ChoiceActionItem("Show Dashboard", "Dashboard"));

            showAction.Execute += ShowAction_Execute;

        }

        private void ShowAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var choice = e.SelectedChoiceActionItem.Data as string;
            var objectSpace = Application.CreateObjectSpace();

            if (choice == "Report")
            {
                string reportFilePath = "..\\..\\..\\Resourse\\Report\\Proj.repx";

                if (File.Exists(reportFilePath))
                {
                    var report = XtraReport.FromFile(reportFilePath, true);
                    var dataCollection = objectSpace.GetObjects<Project>();
                    report.DataSource = dataCollection;
                    var printTool = new ReportPrintTool(report);
                    printTool.ShowPreview();
                }
                else
                {
                    throw new UserFriendlyException($"Report '{reportFilePath}' not found.");
                }
            }
            else if (choice == "Dashboard")
            {
                string dashboardFilePath = "..\\..\\..\\Resourse\\Chart\\diagram.xml";

                if (File.Exists(dashboardFilePath))
                {
                    Dashboard dashboard = new Dashboard();
                    dashboard.LoadFromXml(dashboardFilePath);

                    Form dashboardForm = new Form
                    {
                        Text = "Dashboard Viewer",
                        Width = 800,
                        Height = 600
                    };

                    DashboardViewer dashboardViewer = new DashboardViewer
                    {
                        Dock = DockStyle.Fill,
                        Dashboard = dashboard
                    };

                    dashboardForm.Controls.Add(dashboardViewer);
                    dashboardForm.ShowDialog();
                }
                else
                {
                    throw new UserFriendlyException($"Dashboard '{dashboardFilePath}' not found.");
                }
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
