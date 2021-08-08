﻿#pragma checksum "..\..\..\Category\CategoriesSreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "36EE553A0261DC0A3CA310447A4D842F0F44A2802C1E04D53F293856FE6F9FF1"
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
using MyShop.Category;
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


namespace MyShop.Category {
    
    
    /// <summary>
    /// CategoriesSreen
    /// </summary>
    public partial class CategoriesSreen : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\Category\CategoriesSreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid categoriesScreen;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Category\CategoriesSreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listProductType;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\..\Category\CategoriesSreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox categoryNameTextBox;
        
        #line default
        #line hidden
        
        
        #line 175 "..\..\..\Category\CategoriesSreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox categoryIdTextBox;
        
        #line default
        #line hidden
        
        
        #line 191 "..\..\..\Category\CategoriesSreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddCategory;
        
        #line default
        #line hidden
        
        
        #line 208 "..\..\..\Category\CategoriesSreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUpdateCategory;
        
        #line default
        #line hidden
        
        
        #line 226 "..\..\..\Category\CategoriesSreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDeleteCategory;
        
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
            System.Uri resourceLocater = new System.Uri("/MyShop;component/category/categoriessreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Category\CategoriesSreen.xaml"
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
            this.categoriesScreen = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            
            #line 50 "..\..\..\Category\CategoriesSreen.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.backWard_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.listProductType = ((System.Windows.Controls.ListView)(target));
            
            #line 66 "..\..\..\Category\CategoriesSreen.xaml"
            this.listProductType.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListProductType_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.categoryNameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.categoryIdTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnAddCategory = ((System.Windows.Controls.Button)(target));
            
            #line 190 "..\..\..\Category\CategoriesSreen.xaml"
            this.btnAddCategory.Click += new System.Windows.RoutedEventHandler(this.BtnAddCategory_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnUpdateCategory = ((System.Windows.Controls.Button)(target));
            
            #line 207 "..\..\..\Category\CategoriesSreen.xaml"
            this.btnUpdateCategory.Click += new System.Windows.RoutedEventHandler(this.BtnUpdateCategory_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnDeleteCategory = ((System.Windows.Controls.Button)(target));
            
            #line 225 "..\..\..\Category\CategoriesSreen.xaml"
            this.btnDeleteCategory.Click += new System.Windows.RoutedEventHandler(this.BtnDeleteCategory_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

