using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using CRM.Module.BusinessObjects;
using DevExpress.XtraSpreadsheet;
using System.IO;
using DevExpress.Persistent.BaseImpl;
using Xpand.Extensions.XAF.Xpo.ObjectSpaceExtensions;

namespace CRM.Win.Controllers.FileAttach
{
    public class ShowsSpreadsheetController : ObjectViewController<DetailView, FileAttachment>
    {
        private System.ComponentModel.IContainer components = null;
        private bool _changesMade = false;

        public ShowsSpreadsheetController()
        {
            var showSpreadsheetAction = new SimpleAction(this, "ShowSpreadsheet", PredefinedCategory.View)
            {
                Caption = "Show Spreadsheet",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
                ImageName = "Action_Excel_Import" // Имя картинки из ресурсов
            };

            showSpreadsheetAction.Execute += ShowSpreadsheetAction_Execute;

            var closeSpreadsheetAction = new SimpleAction(this, "CloseSpreadsheet", PredefinedCategory.View)
            {
                Caption = "Close Spreadsheet"
            };
            closeSpreadsheetAction.Execute += CloseSpreadsheetAction_Execute;
        }

        private void CloseSpreadsheetAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var detailView = View as DetailView;
            var controlContainer = detailView.Control as System.Windows.Forms.Control;
            if (controlContainer == null) return;

            var spreadsheetControl = FindSpreadsheetControl(controlContainer.Controls);

            var fileAttachment = (FileAttachment)e.CurrentObject;

            if (_changesMade)
            {
                var result = MessageBox.Show("Хотите сохранить изменения?", "Сохранение", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    SaveChangesToFile(fileAttachment, spreadsheetControl);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
    

            if (spreadsheetControl != null)
            {
                controlContainer.Controls.Remove(spreadsheetControl);
                spreadsheetControl.Dispose();
            }

        }

        private Control FindSpreadsheetControl(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is SpreadsheetControl)
                {
                    return control;
                }
            }
            return null;
        }

        private void SaveChangesToFile(FileAttachment fileAttachment, Control spreadsheetControl)
        {
            var workbook = (SpreadsheetControl)spreadsheetControl;
            var document = workbook.Document;

            using (var stream = new MemoryStream())
            {
                document.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                stream.Seek(0, SeekOrigin.Begin);
                var fileContent = stream.ToArray();

                var newFileData = new FileData(ObjectSpace.Session());
                newFileData.LoadFromStream("NewFile.xlsx", new MemoryStream(fileContent));

                fileAttachment.File = newFileData;
                ObjectSpace.CommitChanges();
            }

            _changesMade = false;
        }

        private void ShowSpreadsheetAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            FileAttachment fileAttachment = (FileAttachment)e.CurrentObject;
            if (fileAttachment != null && fileAttachment.File != null )
            {
                ShowSpreadsheet(fileAttachment);
            }
            else
            {
                throw new UserFriendlyException("The selected file is not a valid Excel file.");
            }
        }

        private void ShowSpreadsheet(FileAttachment fileAttachment)
        {
            var detailView = View as DetailView;
            if (detailView == null) return;

            var controlContainer = detailView.Control as System.Windows.Forms.Control;
            if (controlContainer == null) return;

            var spreadsheetControl = new SpreadsheetControl();
            spreadsheetControl.Size = new System.Drawing.Size(controlContainer.Width - 20, controlContainer.Height / 2);
            spreadsheetControl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            controlContainer.Controls.Add(spreadsheetControl);

            spreadsheetControl.Left = (controlContainer.ClientSize.Width - spreadsheetControl.Width) / 2;
            spreadsheetControl.Top = (controlContainer.ClientSize.Height - spreadsheetControl.Height) / 2;

            using (var stream = new System.IO.MemoryStream(fileAttachment.File.Content))
            {
                spreadsheetControl.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            }

            spreadsheetControl.CellValueChanged += SpreadsheetControl_CellValueChanged;
        }

        private void SpreadsheetControl_CellValueChanged(object sender, SpreadsheetCellEventArgs e)
        {
            _changesMade = true;
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
