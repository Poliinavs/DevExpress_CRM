using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine.NonPersistent;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.Actions;
using CRM.Module.BusinessObjects;
using static CRM.Module.BusinessObjects.ProjectTasks;
using DevExpress.Persistent.Base.Security;
using DevExpress.XtraLayout.Customization;

namespace CRM.Win.Controllers.Permission
{
    public class CustomStateMachineController : StateMachineController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            CustomizeStateMachineActionItems();
            ChangeStateAction.ItemsChanged += ChangeStateAction_ItemsChanged;
        }

        private void ChangeStateAction_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            CustomizeStateMachineActionItems();
        }

        private void CustomizeStateMachineActionItems()
        {
            var security = (ISecurityStrategyBase)SecuritySystem.Instance;

            ApplicationUser user = (ApplicationUser)security.User;
            if (user.Roles.Any(r => r.Name == "User"))
            {
                if (ChangeStateAction != null)
                {
                    foreach (ChoiceActionItem item in ChangeStateAction.Items)
                    {
                        if (item.Data is ITransition transition)
                        {
                            if (transition.TargetState.Caption == StatusEnum.Completed.ToString())
                            {
                                ChangeStateAction.Active["ByUserRole"] = false;
                                ChangeStateAction.Enabled["ByUserRole"] = false;
                            }
                        }
                    }
                }
            }
        }

    }
}
