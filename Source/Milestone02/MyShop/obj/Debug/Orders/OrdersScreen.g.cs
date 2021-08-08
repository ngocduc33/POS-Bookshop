﻿#pragma checksum "..\..\..\Orders\OrdersScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3D5238C4D8ADAC7EF469DAD10EB380CDC5F1CFAC55BD3014B8E02126FFBEF512"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using MyShop;
using MyShop.Orders;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MyShop.Orders {
    
    
    /// <summary>
    /// OrdersScreen
    /// </summary>
    public partial class OrdersScreen : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 17 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid screenOrders;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Create_Order;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker fromDayDatePicker;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker toDayDatePicker;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox statusFilterComboBox;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox searchTextBox;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listOrders;
        
        #line default
        #line hidden
        
        
        #line 388 "..\..\..\Orders\OrdersScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox pagingComboBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MyShop;component/orders/ordersscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Orders\OrdersScreen.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\..\Orders\OrdersScreen.xaml"
            ((MyShop.Orders.OrdersScreen)(target)).Initialized += new System.EventHandler(this.UserControl_Initialized);
            
            #line default
            #line hidden
            return;
            case 2:
            this.screenOrders = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            
            #line 38 "..\..\..\Orders\OrdersScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.backWard_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Create_Order = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\Orders\OrdersScreen.xaml"
            this.Create_Order.Click += new System.Windows.RoutedEventHandler(this.Create_Order_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.fromDayDatePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 78 "..\..\..\Orders\OrdersScreen.xaml"
            this.fromDayDatePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.fromDayDatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.toDayDatePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 91 "..\..\..\Orders\OrdersScreen.xaml"
            this.toDayDatePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.toDayDatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.statusFilterComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 102 "..\..\..\Orders\OrdersScreen.xaml"
            this.statusFilterComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboFilter_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 103 "..\..\..\Orders\OrdersScreen.xaml"
            this.statusFilterComboBox.DropDownOpened += new System.EventHandler(this.ComboFilter_DropDownOpened);
            
            #line default
            #line hidden
            
            #line 104 "..\..\..\Orders\OrdersScreen.xaml"
            this.statusFilterComboBox.DropDownClosed += new System.EventHandler(this.ComboFilter_DropDownClosed);
            
            #line default
            #line hidden
            return;
            case 8:
            this.searchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 131 "..\..\..\Orders\OrdersScreen.xaml"
            this.searchTextBox.SelectionChanged += new System.Windows.RoutedEventHandler(this.searchTextBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.listOrders = ((System.Windows.Controls.ListView)(target));
            return;
            case 12:
            
            #line 380 "..\..\..\Orders\OrdersScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PreviousPaging_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.pagingComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 397 "..\..\..\Orders\OrdersScreen.xaml"
            this.pagingComboBox.DropDownOpened += new System.EventHandler(this.ComboFilter_DropDownOpened);
            
            #line default
            #line hidden
            
            #line 398 "..\..\..\Orders\OrdersScreen.xaml"
            this.pagingComboBox.DropDownClosed += new System.EventHandler(this.ComboFilter_DropDownClosed);
            
            #line default
            #line hidden
            
            #line 399 "..\..\..\Orders\OrdersScreen.xaml"
            this.pagingComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.pageIndexComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 408 "..\..\..\Orders\OrdersScreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NextPaging_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 10:
            
            #line 329 "..\..\..\Orders\OrdersScreen.xaml"
            ((System.Windows.Controls.Border)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.editOrderMouseUp_MouseUp);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 352 "..\..\..\Orders\OrdersScreen.xaml"
            ((System.Windows.Controls.Border)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.deleteOrderMouseUp_MouseUp);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

