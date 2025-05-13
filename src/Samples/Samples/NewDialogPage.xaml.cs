namespace Elmish.Uno.Samples.NewDialog;

using Microsoft.FSharp.Core;
using Microsoft.UI.Xaml.Controls;
using Elmish.Uno;
using ElmishProgram = Elmish.Uno.Samples.NewDialog.AppModule;

public partial class NewDialogPage : Page
{
    public NewDialogPage()
    {
        InitializeComponent();
        var program = ElmishProgram.CreateProgram(() => new Dialog1 { XamlRoot = this.XamlRoot }, () => new Dialog2 { XamlRoot = this.XamlRoot });
        UnoProgram.StartElmishLoop(this, program);
    }
}
