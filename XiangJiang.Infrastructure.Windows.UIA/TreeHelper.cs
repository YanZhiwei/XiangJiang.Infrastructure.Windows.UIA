﻿using System;
using System.Linq;
using System.Windows.Automation;
using XiangJiang.Core;
using XiangJiang.Infrastructure.Windows.UIA.Core;

namespace XiangJiang.Infrastructure.Windows.UIA
{
    /// <summary>
    ///     Tree 控件
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class TreeHelper
    {
        /// <summary>
        ///     选中节点
        /// </summary>
        /// <param name="treeView">AutomationElement</param>
        /// <param name="childNodes">节点文本递归；eg aa--bb--cc，选中cc节点</param>
        /// <returns>选中节点</returns>
        /// <exception cref="ArgumentException">
        ///     AutomationId '{treeView.Current.AutomationId}' and Name '{treeView.Current.Name}'
        ///     控件类型不是 Tree.
        /// </exception>
        /// <exception cref="ArgumentNullException">未能找到 {nextNode} 节点.</exception>
        public static AutomationElement SelectNode(this AutomationElement treeView, params string[] childNodes)
        {
            Checker.Begin().NotNull(treeView, nameof(treeView))
                .SupportedType(treeView, ControlType.Tree)
                .NotNull(childNodes, nameof(childNodes));
            if (childNodes?.Any() ?? false)
            {
                var selectedNode = childNodes.Last();
                var rootNode = childNodes.First();
                var aeParentNode = treeView.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.NameProperty, rootNode));
                foreach (var nextNode in childNodes)
                {
                    if (nextNode == rootNode) continue;
                    var aeChildNode = GetChildNode(aeParentNode, nextNode);
                    if (aeChildNode == null) continue;
                    if (selectedNode == aeChildNode.Current.Name)
                    {
                        var spChildNode = aeChildNode.GetSelectionItemPattern();
                        spChildNode.Select();
                        return aeChildNode;
                    }

                    aeParentNode = aeChildNode;
                }
            }

            return null;
        }

        private static AutomationElement GetChildNode(this AutomationElement aeParentNode, string nextNode)
        {
            if (aeParentNode != null)
            {
                var ecpNode = aeParentNode.GetExpandCollapsePattern();
                ecpNode.Expand();
                return aeParentNode.FindFirst(TreeScope.Subtree,
                    new PropertyCondition(AutomationElement.NameProperty, nextNode));
            }

            return null;
        }
    }
}