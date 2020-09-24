using System;
using System.Windows.Automation;
using XiangJiang.Core;
using XiangJiang.Windows.UIA.Core;

namespace XiangJiang.Windows.UIA
{
    /// <summary>
    ///     ComboBox 控件
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ComboBoxHelper
    {
        /// <summary>
        ///     设置选中项
        /// </summary>
        /// <param name="comboBox">AutomationElement</param>
        /// <param name="item">需要选中的文本</param>
        /// <exception cref="ArgumentException">
        ///     Element with AutomationId '{comboBox.Current.AutomationId}' and Name
        ///     '{comboBox.Current.Name}' ControlType is not ComboBox.
        /// </exception>
        public static bool SetSelectedItem(this AutomationElement comboBox, string item)
        {
            Checker.Begin().NotNull(comboBox, nameof(comboBox))
                .NotNullOrEmpty(item, nameof(item))
                .SupportedType(comboBox, ControlType.ComboBox);
            var listBox = comboBox.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List));
            if (listBox == null) return false;

            var listItem = listBox.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, item));
            if (listItem == null) return false;

            if (!listItem.TryGetCurrentPattern(SelectionItemPatternIdentifiers.Pattern, out var sipCombox))
                return false;
            var selectionItemPattern = sipCombox as SelectionItemPattern;
            selectionItemPattern?.Select();
            return true;
        }
    }
}