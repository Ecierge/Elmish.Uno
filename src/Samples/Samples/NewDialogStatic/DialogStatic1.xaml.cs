using Elmish.Uno.Samples.NewDialogStatic.Dialog1;

namespace Elmish.Uno.Samples.NewDialogStatic;

#pragma warning disable CA1010 // Generic interface should also be implemented
#pragma warning disable CA1501 // 'Dialog1Static1' has multiple object hierarchy levels deep within the defining module
public partial class DialogStatic1 : ContentDialog
{
    private Dialog1ViewModel ViewModel => (DataContext as Dialog1ViewModel)!;
    public DialogStatic1()
    {
        InitializeComponent();
    }
}
#pragma warning restore CA1010 // Generic interface should also be implemented
