using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPivotGrid;

namespace CRM.Win.Controllers.Tasks
{
    public class PivotTableController : ObjectViewController<ListView, ProjectTasks>
    {
        private System.ComponentModel.IContainer components = null;

       public PivotTableController()
        {
            InitializeComponent();

            var groupTasksAction = new SimpleAction(this, "GroupTasks", PredefinedCategory.Edit)
            {
                Caption = "Group Tasks",
                ImageName = "Action_Group"
            };

            groupTasksAction.Execute += GroupTasksAction_Execute;
        }

        private void GroupTasksAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            using (IObjectSpace objectSpace = Application.CreateObjectSpace())
            {
                var tasks = objectSpace.GetObjects<ProjectTasks>();
                var groupedTasks = tasks
                    .GroupBy(t => new { t.Status, t.Level })
                    .Select(g => new
                    {
                        Status = g.Key.Status,
                        Level = g.Key.Level,
                        TaskCount = g.Count(),
                        TotalCost = g.Sum(t => t.Cost)
                    })
                    .ToList();

                ShowPivotTable(groupedTasks);
            }
        }

        private void ShowPivotTable(object groupedTasks)
        {
            var form = CreateForm();
            var pivotGrid = CreatePivotGrid(groupedTasks);
            var comboBox = CreateCheckedComboBoxFields(form, pivotGrid);
            var comboBoxSummaryType = CreateCheckedComboBoxSummaryType(form, pivotGrid);

            form.Controls.AddRange(new Control[] { pivotGrid,  comboBoxSummaryType });

            form.ShowDialog();
        }

        private XtraForm CreateForm()
        {
            var form = new XtraForm
            {
                Text = "Grouped Tasks Pivot Table",
                Width = 1000,
                Height = 800,
                StartPosition = FormStartPosition.CenterScreen,
                LookAndFeel = { Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat, UseDefaultLookAndFeel = false },
                Appearance = { BackColor = System.Drawing.Color.White, Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular) }
            };

            return form;
        }

        private PivotGridControl CreatePivotGrid(object dataSource)
        {
            var pivotGrid = new PivotGridControl
            {
                DataSource = dataSource,
                Dock = DockStyle.Fill,
                LookAndFeel = { Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat, UseDefaultLookAndFeel = false },
                Appearance = {
                    FieldHeader = { BackColor = System.Drawing.Color.LightBlue, Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold) },
                    DataHeaderArea = { BackColor = System.Drawing.Color.LightGray, Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular) }
                },

                OptionsView = { ShowDataHeaders = true, ShowFilterHeaders = true, ShowColumnHeaders = true }
            };

            pivotGrid.Fields.Add(new PivotGridField("Status", PivotArea.RowArea));
            pivotGrid.Fields.Add(new PivotGridField("Level", PivotArea.RowArea));
            pivotGrid.Fields.Add(new PivotGridField("TaskCount", PivotArea.DataArea));
            pivotGrid.Fields.Add(new PivotGridField("TotalCost", PivotArea.DataArea));

            return pivotGrid;
        }

        private CheckedComboBoxEdit CreateCheckedComboBoxFields(XtraForm parentForm, PivotGridControl pivotGrid)
        {
            var checkedComboBox = new CheckedComboBoxEdit
            {
                Properties = {
                    Items = { "Level", "Status" }
                },

                Dock = DockStyle.Top,
                Parent = parentForm
            };

            checkedComboBox.Properties.DropDownRows = checkedComboBox.Properties.Items.Count;

            checkedComboBox.EditValueChanged += (sender, e) =>
            {
                pivotGrid.BeginUpdate();
                pivotGrid.Fields.Clear();

                foreach (CheckedListBoxItem item in checkedComboBox.Properties.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        var selectedField = item.ToString();

                        if (selectedField == "Level")
                        {
                            pivotGrid.Fields.Add(new PivotGridField("Level", PivotArea.RowArea));
                        }
                        else if (selectedField == "Status")
                        {
                            pivotGrid.Fields.Add(new PivotGridField("Status", PivotArea.RowArea));
                        }
                    }
                }

                pivotGrid.Fields.Add(new PivotGridField("TaskCount", PivotArea.DataArea));
                pivotGrid.Fields.Add(new PivotGridField("TotalCost", PivotArea.DataArea));

                pivotGrid.EndUpdate();
            };

            return checkedComboBox;
        }


        private CheckedComboBoxEdit CreateCheckedComboBoxSummaryType(XtraForm parentForm, PivotGridControl pivotGrid)
        {
            var checkedComboBoxSummaryType = new CheckedComboBoxEdit
            {
                Properties = { Items = { "TotalCost", "TaskCount" } },
                Dock = DockStyle.Top,
                Parent = parentForm
            };

            checkedComboBoxSummaryType.Properties.EditValueChanged += (sender, e) =>
            {
                pivotGrid.BeginUpdate();

                List<PivotGridField> fieldsToRemove = new List<PivotGridField>();
                foreach (PivotGridField field in pivotGrid.Fields)
                {
                    if (field.Area == PivotArea.DataArea)
                    {
                        fieldsToRemove.Add(field);
                    }
                }

                foreach (PivotGridField fieldToRemove in fieldsToRemove)
                {
                    pivotGrid.Fields.Remove(fieldToRemove);
                }

                foreach (CheckedListBoxItem item in checkedComboBoxSummaryType.Properties.Items)
                {
                    string fieldName = item.Value.ToString();
                    if (item.CheckState == CheckState.Checked && pivotGrid.Fields[fieldName] == null)
                    {
                        PivotGridField dataField = new PivotGridField(fieldName, PivotArea.DataArea);
                        pivotGrid.Fields.Add(dataField);
                    }
                }

                pivotGrid.EndUpdate();
            };

            return checkedComboBoxSummaryType;
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
