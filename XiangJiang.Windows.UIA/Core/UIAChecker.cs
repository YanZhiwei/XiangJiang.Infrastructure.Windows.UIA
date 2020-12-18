using System.Windows.Automation;
using XiangJiang.Core;
using XiangJiang.Windows.UIA.Localization;
using XiangJiang.Model;

namespace XiangJiang.Windows.UIA.Core
{
    public static class UIAChecker
    {
        public static Validation SupportedType(this Validation validation, AutomationElement automation,
            ControlType targetControlType)
        {
            return validation.Check<UIAException>(() => Equals(automation.Current.ControlType, targetControlType),
                string.Format(Resource.Typenotsupported,
                    automation.Current.AutomationId,
                    automation.Current.Name,
                    targetControlType.LocalizedControlType));
        }

        public static Validation SupportedType(this Validation validation, AutomationElement automation,
            AutomationPattern targetControlType)
        {
            return validation.Check<UIAException>(() => automation.TryGetCurrentPattern(targetControlType, out var _),
                string.Format(Resource.Typenotsupported,
                    automation.Current.AutomationId,
                    automation.Current.Name,
                    targetControlType.ProgrammaticName));
        }
    }
}