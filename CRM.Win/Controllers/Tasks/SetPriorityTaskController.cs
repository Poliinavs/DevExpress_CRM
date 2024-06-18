using CRM.Module.BusinessObjects;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using static CRM.Module.BusinessObjects.ProjectTasks;

namespace CRM.Win.Controllers.Tasks
{
    public class SetPriorityTaskController : ObjectViewController<DetailView, ProjectTasks>
    {
        private System.ComponentModel.IContainer components = null;
        private ChoiceActionItem setStatusItem;
        private ChoiceActionItem seDifficultItem;

        public SetPriorityTaskController()
        {
            InitializeComponent();


            SingleChoiceAction SetTaskAction = new SingleChoiceAction(this, "SetStatusAction", PredefinedCategory.Edit)
            {
                Caption = "Set status",
                ItemType = SingleChoiceActionItemType.ItemIsOperation,
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
            };

            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(ProjectTasks), "Status"), null);
            seDifficultItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(ProjectTasks), "difficulty level"), null);

            SetTaskAction.Items.Add(setStatusItem);
            SetTaskAction.Items.Add(seDifficultItem);

            FillItemWithEnumValues(setStatusItem, typeof(StatusEnum));
            FillItemWithEnumValues(seDifficultItem, typeof(LevelEnum));

            SetTaskAction.Execute += SetTaskAction_Execute;
        }

        private void SetTaskAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var tasks = (ProjectTasks)e.CurrentObject;
            if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                tasks.Status = (StatusEnum)e.SelectedChoiceActionItem.Data;
            }
            else if (e.SelectedChoiceActionItem.ParentItem == seDifficultItem)
            {
                tasks.Level = (LevelEnum)e.SelectedChoiceActionItem.Data;
            }

            if (this.ObjectSpace.IsModified)
            {
                this.ObjectSpace.CommitChanges();
            }

            View.ObjectSpace.Refresh();
        }

        private void FillItemWithEnumValues(ChoiceActionItem parentItem, Type enumType)
        {
            var ed = new EnumDescriptor(enumType);
            foreach (object current in ed.Values)
            {
                var item = new ChoiceActionItem(ed.GetCaption(current), current);
                item.ImageName = ImageLoader.Instance.GetEnumValueImageName(current);
                parentItem.Items.Add(item);
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
