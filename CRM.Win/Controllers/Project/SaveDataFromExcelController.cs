using CRM.Module.BusinessObjects.Models.Excel;
using CRM.Module.BusinessObjects;
using CRM.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using OfficeOpenXml;

namespace CRM.Win.Controllers.Proj
{
    public class SaveDataFromExcelController : ObjectViewController<ListView, Project>
    {
        private System.ComponentModel.IContainer components = null;
        private MappingConfiguration<ExcelProject, Project> excelMapping;


        public SaveDataFromExcelController()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            excelMapping = new MappingConfiguration<ExcelProject, Project>();

            var loadFile = new SimpleAction(this, "LoadFileProjAction", PredefinedCategory.Edit)
            {
                Caption = "Load Data",
                ImageName = "Open"
            };

            loadFile.Execute += LoadFileAction_Execute;
        }

        private void LoadFileAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
           // GenerateTasks();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    var headerIndexes = ExcelReader<ExcelProject>.GetHeaderIndexes(filePath);
                    var excelProj = ExcelReader<ExcelProject>.ReadFromExcel(filePath, (worksheet, row) =>
                    {
                        return new ExcelProject
                        {
                            ProjectName = worksheet.Cells[row, headerIndexes["ProjectName"]].Text,
                            Budget = worksheet.Cells[row, headerIndexes["Budget"]].Text,
                            UserName = worksheet.Cells[row, headerIndexes["UserName"]].Text
                        };
                    }); ;

                    CreateCompanyFromExcel(excelProj);
                    SaveChanges();
                    View.RefreshDataSource();

                }
            }
        }

       /* public void GenerateTasks(int taskCount = 100)
        {
            var objectSpace = View.ObjectSpace;
            Random random = new Random();

            for (int i = 0; i < taskCount; i++)
            {
                var newProj = objectSpace.CreateObject<ProjectTasks>();
                newProj.TaskName = $"Task {i + 1}";
                newProj.Description = $"Description for Task {i + 1}";
                newProj.Cost = random.Next(100, 1000); // Random cost between 100 and 1000
                newProj.StartDate = DateTime.Today.AddDays(random.Next(-30, 30)); // Random start date within +/- 30 days from today
                newProj.EndDate = DateTime.Today.AddDays(random.Next(31, 60)); // Random end date 1 to 60 days after today
                newProj.Status = (ProjectTasks.StatusEnum)random.Next(0, 6); // Random status
                newProj.Level = (ProjectTasks.LevelEnum)random.Next(0, 3); // Random level
                newProj.ProjectName = GetRandomProject(objectSpace); // Assuming GetRandomProject method
            }

            objectSpace.CommitChanges();
        }

        private static Project GetRandomProject(IObjectSpace objectSpace)
        {
            var projects = objectSpace.GetObjects<Project>().ToList();
            Random random = new Random();
            int index = random.Next(projects.Count);
            return projects.ElementAtOrDefault(index);
        }*/

        private void SaveData()
        {
            var objectSpace = View.ObjectSpace;
            for (int i = 0; i < 200; i++)
            {
                var newProj = objectSpace.CreateObject<Project>();
                newProj.ProjectName = $"Project {i + 1}";
                newProj.Budget = 10000 + i * 500;  // Пример бюджета
                newProj.StartTime = DateTime.Now.AddDays(-i);  // Пример времени начала
            }

            objectSpace.CommitChanges();
        }

        private void SaveChanges()
        {
            var objectSpace = View.ObjectSpace;
            objectSpace.CommitChanges();
        }

        private void CreateCompanyFromExcel(List<ExcelProject> excelCompanies)
        {
            var objectSpace = View.ObjectSpace;

            try
            {
                foreach (var excelCompany in excelCompanies)
                {
                    var newProj = objectSpace.CreateObject<Project>();
                    excelMapping.Map(excelCompany, newProj, objectSpace);
                }
            }
            catch (AutoMapper.AutoMapperMappingException ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка при маппинге данных из Excel: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                objectSpace.Rollback();
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
