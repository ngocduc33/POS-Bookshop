using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyShop
{
    static class VisibilityExtension
    {
        public static bool IsCollapsed(this StackPanel stk)
        {
            return stk.Visibility == Visibility.Collapsed;
        }

        public static void Show(this StackPanel stk)
        {
            stk.Visibility = Visibility.Visible;
        }

        public static void Collapse(this StackPanel stk)
        {
            stk.Visibility = Visibility.Collapsed;
        }
    }
}
