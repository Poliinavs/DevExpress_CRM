﻿/*using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp.Utils;
using Xpand.Extensions.XAF.ObjectExtensions;
using Xpand.Persistent.Base.General;
using Xpand.Utils.Linq;

namespace CRM.Win.Controllers.Permission
{
    public class ChangeStateActionController : ViewController<ObjectView>
    {
        private StateMachineController _stateMachineController;

        public event EventHandler<ChoiceActionItemArgs> RequestActiveState;

        protected virtual void OnRequestActiveState(ChoiceActionItemArgs e)
        {
            var handler = RequestActiveState;
            handler?.Invoke(this, e);
        }

        bool IsActive(ChoiceActionItem choiceActionItem)
        {
            var iStateMachine = (choiceActionItem.Data as IStateMachine);
            if (iStateMachine != null)
            {
                var boolList = new BoolList(true, BoolListOperatorType.Or);
                boolList.BeginUpdate();
                foreach (var item in choiceActionItem.Items)
                {
                    var iTransition = ((XpoTransition)item.Data);
                    var choiceActionItemArgs = new ChoiceActionItemArgs(iTransition, item.Active);
                    OnRequestActiveState(choiceActionItemArgs);
                    boolList.SetItemValue(ObjectSpace.GetKeyValueAsString(iTransition), item.Active.ResultValue);
                }
                boolList.EndUpdate();
                return boolList.ResultValue;
            }
            var transition = choiceActionItem.Data as XpoTransition;
            if (transition != null)
            {
                var choiceActionItemArgs = new ChoiceActionItemArgs(transition, choiceActionItem.Active);
                OnRequestActiveState(choiceActionItemArgs);
                return choiceActionItem.Active;
            }
            throw new NotImplementedException();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            Frame.GetController<StateMachineCacheController>(controller => {
                _stateMachineController = Frame.GetController<StateMachineController>();
                _stateMachineController.ChangeStateAction.ItemsChanged += ChangeStateActionOnItemsChanged;
                ObjectSpace.ObjectChanged += ObjectSpaceOnObjectChanged;
            });
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (_stateMachineController != null)
                _stateMachineController.ChangeStateAction.ItemsChanged -= ChangeStateActionOnItemsChanged;
            ObjectSpace.ObjectChanged -= ObjectSpaceOnObjectChanged;
        }

        private void ObjectSpaceOnObjectChanged(object sender, ObjectChangedEventArgs objectChangedEventArgs)
        {
            Frame.GetController<StateMachineCacheController>(controller => {
                if (controller.CachedStateMachines.Any())
                {
                   // _stateMachineController.UpdateActionState();
                    foreach (var item in _stateMachineController.ChangeStateAction.Items.GetItems<ChoiceActionItem>(@base => @base.Items))
                    {
                        UpdatePanelActions(item.Id, item.Active[typeof(ChangeStateActionController).Name]);
                    }
                }
            });
        }

        void ChangeStateActionOnItemsChanged(object sender, ItemsChangedEventArgs itemsChangedEventArgs)
        {
            foreach (ChoiceActionItem item in GetItems(itemsChangedEventArgs.ChangedItemsInfo))
            {
                var isActive = IsActive(item);
                item.Active[typeof(ChangeStateActionController).Name] = isActive;
                UpdatePanelActions(item.Id, isActive);
            }
        }

        private void UpdatePanelActions(string itemId, bool isActive)
        {
            var detailView = View as DetailView;

            if (detailView != null)
            {
                foreach (var key in _stateMachineController.Actions)
                {
                    var actionContainer = detailView.FindItem(key.Id) as ActionContainerViewItem;
                    var action = actionContainer?.Actions.FirstOrDefault(@base => @base.Caption == itemId);
                    if (action != null) action.Active[typeof(ChangeStateActionController).Name] = isActive;
                }
            }
        }

        IEnumerable<ChoiceActionItem> GetItems(Dictionary<object, ChoiceActionItemChangesType> changedItemsInfo)
        {
            return changedItemsInfo.Where(pair => pair.Value == ChoiceActionItemChangesType.Add).Select(pair => pair.Key as ChoiceActionItem).Where(item => item != null);
        }

    }

    public class ChoiceActionItemArgs : EventArgs
    {
        public ChoiceActionItemArgs(XpoTransition transition, BoolList active)
        {
            Transition = transition;
            Active = active;
        }

        public XpoTransition Transition { get; }

        public BoolList Active { get; set; }
    }
}
*/