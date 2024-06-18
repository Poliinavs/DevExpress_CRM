﻿/*using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.Persistent.Validation;
using System.Reflection;
using Xpand.ExpressApp.Security.Permissions;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.Security;

namespace CRM.Win.Controllers.Permission
{
    public class CriteriaActionStateController : ViewController<ObjectView>
    {

        private const string HideIfCriteriaDoNotFit = "HideIfCriteriaDoNotFit";

        ChangeStateActionController _changeStateActionController;



        protected override void OnActivated()
        {

            base.OnActivated();

            _changeStateActionController = Frame.GetController<ChangeStateActionController>();

            _changeStateActionController.RequestActiveState += ChangeStateActionControllerOnRequestActiveStateAction;

        }



        public override void CustomizeTypesInfo(DevExpress.ExpressApp.DC.ITypesInfo typesInfo)
        {

            base.CustomizeTypesInfo(typesInfo);

            var typeInfo = typesInfo.FindTypeInfo(typeof(XpoState));

            typeInfo.CreateMember(HideIfCriteriaDoNotFit, typeof(bool));

        }



        protected override void OnDeactivated()
        {

            base.OnDeactivated();

            _changeStateActionController.RequestActiveState -= ChangeStateActionControllerOnRequestActiveStateAction;

        }



        void ChangeStateActionControllerOnRequestActiveStateAction(object sender, ChoiceActionItemArgs choiceActionItemArgs)
        {

            var key = typeof(CriteriaActionStateController).Name;

            choiceActionItemArgs.Active[key] = IsActive(choiceActionItemArgs.Transition);

        }



        bool IsActive(XpoTransition xpoTransition)
        {

            var hideIfCriteriaDoNotFit = xpoTransition.TargetState.GetMemberValue(HideIfCriteriaDoNotFit) as bool?;

            if (hideIfCriteriaDoNotFit.HasValue && hideIfCriteriaDoNotFit.Value)
            {

                var stateMachineLogic = new StateMachineLogic(ObjectSpace);

                var ruleSetValidationResult = RuleSetValidationResult(xpoTransition, stateMachineLogic);

                return ruleSetValidationResult.State != ValidationState.Invalid;

            }

            return true;

        }



        [Obsolete("in 13.2 the ValidateTransition will be public")]

        RuleSetValidationResult RuleSetValidationResult(XpoTransition xpoTransition, StateMachineLogic stateMachineLogic)
        {

            var methodInfo = stateMachineLogic.GetType().GetMethod("ValidateTransition", BindingFlags.NonPublic | BindingFlags.Instance);

            return (RuleSetValidationResult)methodInfo.Invoke(stateMachineLogic, new[] { xpoTransition.TargetState, View.CurrentObject });

        }

    }


}
*/