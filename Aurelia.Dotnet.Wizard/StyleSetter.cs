using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Aurelia.DotNet.Wizard
{
    public static class StyleSetter
    {

        public static void SetStyle(this FrameworkElement root)
        {
            WalkDownLogicalTree(root);
        }

        internal static void WalkDownLogicalTree(FrameworkElement root, object current = null)
        {
            current = current ?? root;
            if (!(current is DependencyObject depObj))
            {
                return;
            }

            var found = false;
            if (current == root)
            {
                root.SetRootStyles();
            }

            found |= root.SetElementStyle(depObj as TextBox, VsResourceKeys.TextBoxStyleKey);
            found |= root.SetElementStyle(depObj as Label, VsResourceKeys.ThemedDialogLabelStyleKey);
            found |= root.SetElementStyle(depObj as Button, VsResourceKeys.ThemedDialogButtonStyleKey);
            found |= root.SetElementStyle(depObj as ListView, VsResourceKeys.ThemedDialogListViewStyleKey);
            found |= root.SetElementStyle(depObj as ListViewItem, VsResourceKeys.ThemedDialogListViewItemStyleKey);
            found |= root.SetElementStyle(depObj as ListBox, VsResourceKeys.ThemedDialogListBoxStyleKey);
            found |= root.SetElementStyle(depObj as RadioButton, VsResourceKeys.ThemedDialogRadioButtonStyleKey);
            found |= root.SetElementStyle(depObj as ComboBox, VsResourceKeys.ThemedDialogComboBoxStyleKey);
            found |= root.SetElementStyle(depObj as CheckBox, VsResourceKeys.ThemedDialogCheckBoxStyleKey);
            found |= root.SetElementStyle(depObj as TreeView, VsResourceKeys.ThemedDialogTreeViewStyleKey);
            found |= root.SetElementStyle(depObj as TreeViewItem, VsResourceKeys.ThemedDialogTreeViewItemStyleKey);
            found |= root.SetElementStyle(depObj as Window, VsResourceKeys.ThemedDialogDefaultStylesKey);

            foreach (var logicalChild in LogicalTreeHelper.GetChildren(depObj))
            {
                WalkDownLogicalTree(root, logicalChild);
            }
        }

        private static void SetRootStyles(this FrameworkElement root)
        {
            var window = root as Window;
            if (window == null) { return; }
            if (root.TryFindResource(VsBrushes.ToolWindowBackgroundKey) is Brush background && root.TryFindResource(VsBrushes.ToolWindowBorderKey) is Brush border)
            {
                window.Background = background;
                window.BorderBrush = border;
            }
        }

        private static bool SetElementStyle(this FrameworkElement root, FrameworkElement element, object resourceKey)
        {
            var style = root.TryFindResource(resourceKey) as Style;
            if (element == null || style == null) { return false; }
            element.Style = style;
            return true;
        }
    }
}
