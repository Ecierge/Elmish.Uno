namespace Elmish.Uno.Samples.NewWindow;

using Microsoft.FSharp.Core;
using Microsoft.UI.Xaml.Controls;
using Elmish.Uno;
using ElmishProgram = Elmish.Uno.Samples.NewWindow.AppModule;

public partial class NewWindowPage : Page
{
    public NewWindowPage()
    {
        InitializeComponent();
        var program = ElmishProgram.CreateProgram(() => new Window1(), () => new Window2());
        UnoProgram.StartElmishLoop(this, program);
    }
}
