using Elmish.Uno.Samples.NewDialogStatic.Dialog1;

namespace Elmish.Uno.Samples.NewDialogStatic;

public partial class DialogStatic1 : ContentDialog
{
    private Dialog1ViewModel ViewModel => (DataContext as Dialog1ViewModel)!;
    public DialogStatic1()
    {
        InitializeComponent();
    }
}
