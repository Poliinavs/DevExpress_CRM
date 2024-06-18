using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using OfficeOpenXml;
using CRM.Utils;
using CRM.Module.BusinessObjects.Models.Excel;
using DevExpress.XtraEditors.DXErrorProvider;

namespace CRM.Win.Controllers.Compan
{
    public class SaveDataFromExcelController : ObjectViewController <ListView, Company>
    {
        private System.ComponentModel.IContainer components = null;
        private MappingConfiguration<ExcelCompany, Company> excelToCompanyMapping;


        public SaveDataFromExcelController()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            excelToCompanyMapping = new MappingConfiguration<ExcelCompany, Company>();

            var loadFile = new SimpleAction(this, "LoadFileAction", PredefinedCategory.Edit)
            {
                Caption = "Load Data",
                ImageName = "Open"
            };

            loadFile.Execute += LoadFileAction_Execute;
        }

        private void LoadFileAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    ProcessExcelFile(filePath);
                }
            }
        }


        public void ProcessExcelFile(string filePath)
        {
            try
            {
                var headerIndexes = ExcelReader<ExcelCompany>.GetHeaderIndexes(filePath);
                var excelCompanies = ExcelReader<ExcelCompany>.ReadFromExcel(filePath, (worksheet, row) =>
                {
                    return new ExcelCompany
                    {
                        CompanyName = worksheet.Cells[row, headerIndexes["CompanyName"]].Text,
                        WebSite = worksheet.Cells[row, headerIndexes["WebSite"]].Text,
                        PhoneNumber = worksheet.Cells[row, headerIndexes["PhoneNumber"]].Text,
                        BillingAddress = worksheet.Cells[row, headerIndexes["BillingAddress"]].Text,
                        ShippingAddress = worksheet.Cells[row, headerIndexes["ShippingAddress"]].Text,
                        ShipToBilling = worksheet.Cells[row, headerIndexes["ShipToBilling"]].Text
                    };
                });

                CreateCompanyFromExcel(excelCompanies);
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка при чтении данных " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SaveChanges();
            View.RefreshDataSource();
        }

        public void SaveChanges()
        {
            var objectSpace = View.ObjectSpace;
            objectSpace.CommitChanges();
        }

        public void CreateCompanyFromExcel(List<ExcelCompany> excelCompanies)
        {
            var objectSpace = View.ObjectSpace;

            try
            {
                foreach (var excelCompany in excelCompanies)
                {
                    var newCompany = objectSpace.CreateObject<Company>();
                    excelToCompanyMapping.Map(excelCompany, newCompany, objectSpace);
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
