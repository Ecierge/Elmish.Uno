namespace Elmish.Uno.Samples.NewDialogStatic;

using Elmish.Uno.Samples.NewDialogStatic;
using Microsoft.UI.Xaml.Controls;
using System;

public partial class NewDialogStaticPage : Page
{

    public NewDialogStaticPage()
    {
        InitializeComponent();
        Func<ContentDialog> createDialog1 = () => new DialogStatic1() { XamlRoot = this.XamlRoot };
        Func<ContentDialog> createDialog2 = () => new DialogStatic2() { XamlRoot = this.XamlRoot };
        DataContext = new NewDialogStaticViewModel(createDialog1, createDialog2, this.DispatcherQueue);
    }
}
