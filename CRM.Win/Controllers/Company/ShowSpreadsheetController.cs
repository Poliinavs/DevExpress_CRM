using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.XtraSpreadsheet;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;

namespace CRM.Win.Controllers.Compan
{
    public class ShowSpreadsheetCompanyController : ObjectViewController<ListView, Company>
    {
        private System.ComponentModel.IContainer components = null;

        public ShowSpreadsheetCompanyController()
        {
            var showSpreadsheetAction = new SimpleAction(this, "ShowSpreadsheetCompany", PredefinedCategory.View)
            {
                Caption = "Show Spreadsheet"
            };

            showSpreadsheetAction.Execute += ShowSpreadsheetAction_Execute;
        }

        private void ShowSpreadsheetAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            var allCompanies = ObjectSpace.GetObjects<Company>();

            var _spreadsheetControl = new SpreadsheetControl();
            IWorkbook workbook = _spreadsheetControl.Document;

            workbook.LoadDocument("Temp.xlsx");
            var sheet = workbook.Worksheets[0];

            sheet.ClearContents(sheet.GetUsedRange());

            sheet.Cells[0, 1].Value = "id";

            for (int i = 0; i < 6; i++)
            {
                string propertyName = typeof(Company).GetProperties()[i].Name;

                sheet.Cells[0, i+2].Value = propertyName;
            }

            sheet.DataBindings.BindToDataSource(allCompanies, 1, 0);

            _spreadsheetControl.DataContext = workbook;
            _spreadsheetControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _spreadsheetControl.Size = new Size(800, 600);
            _spreadsheetControl.CellValueChanged += SpreadsheetControl_CellValueChanged;


            var form = new System.Windows.Forms.Form();
            _spreadsheetControl.Location = new Point((form.ClientSize.Width - _spreadsheetControl.Width) / 2,
                                            (form.ClientSize.Height - _spreadsheetControl.Height) / 2);
            form.Size = new Size(900, 700);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Controls.Add(_spreadsheetControl);
            form.ShowDialog();

        }

        private void SpreadsheetControl_CellValueChanged(object sender, SpreadsheetCellEventArgs e)
        {
            var worksheet = e.Worksheet;
            var rowIndex = e.RowIndex;
            var columnIndex = e.ColumnIndex;
            var newValue = e.Value;
            try
            {
                if (columnIndex != 1)
                {
                    var idCell = worksheet.Cells[rowIndex, 1];
                    if (idCell.Value != null)
                    {
                        var companyId = Guid.Parse(idCell.Value.ToString());
                        var company = ObjectSpace.GetObjectByKey<Company>(companyId);
                        if (company != null)
                        {
                            var propertyIndex = columnIndex - 2;
                            var properties = typeof(Company).GetProperties();

                            if (propertyIndex >= 0 && propertyIndex < properties.Length)
                            {
                                var propertyName = properties[propertyIndex].Name;
                                var property = typeof(Company).GetProperty(propertyName);

                                property.SetValue(company, GetValueFromCellValue(newValue));

                                ObjectSpace.CommitChanges();
                            }
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Cannot modify the ID column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        private object GetValueFromCellValue(object value)
        {
            if (value is CellValue cellValue)
            {
                if (cellValue.IsEmpty)
                {
                    return null; 
                }
                else if (cellValue.IsBoolean)
                {
                    return cellValue.BooleanValue; 
                }
                else if (cellValue.IsDateTime)
                {
                    return cellValue.DateTimeValue; 
                }
                else if (cellValue.IsNumeric)
                {
                    return cellValue.NumericValue; 
                }
                else if (cellValue.IsText)
                {
                    return cellValue.TextValue; 
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return value; // Return value as is if not a CellValue object
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
