using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MsBox.Avalonia.Enums;
using System;
using System.IO;

namespace VCDiffGUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
            return;
        ButtonPatcher.Content = "Patching! Please Wait...";
        ButtonPatcher.IsEnabled = false;

        var sourceFile = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Pick original file",
            AllowMultiple = false
        });

        if (sourceFile is null)
        {
            ButtonPatcher.IsEnabled = true;
            ButtonPatcher.Content = "Create Patch";
            return;
        }

        var modifiedFile = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Pick modified file",
            AllowMultiple = false
        });

        if (modifiedFile is null)
        {
            ButtonPatcher.IsEnabled = true;
            ButtonPatcher.Content = "Create Patch";
            return;
        }

        var outFile = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save patch file",
            DefaultExtension = "xdelta",
            FileTypeChoices = [FilePickerFileTypes.All]
        });

        if (outFile is null)
        {
            ButtonPatcher.IsEnabled = true;
            ButtonPatcher.Content = "Create Patch";
            return;
        }

        try
        {
            using FileStream source = new FileStream(sourceFile[0].Path.LocalPath, FileMode.Open, FileAccess.Read);
            using FileStream target = new FileStream(modifiedFile[0].Path.LocalPath, FileMode.Open, FileAccess.Read);
            using FileStream output = new FileStream(outFile.Path.LocalPath, FileMode.Create, FileAccess.Write);

            using var coder = new VCDiff.Encoders.VcEncoder(source, target, output, 16, 32);
            var result = coder.Encode();
            _ = Utils.ShowSimpleMessage("Success", "DONE! You can now use your file!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            source.Dispose();
            target.Dispose();
            output.Dispose();
        }
        catch (Exception ex)
        {
            _ = Utils.ShowSimpleMessage("Error", ex.Message + Environment.NewLine + ex.StackTrace, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
        }
        ButtonPatcher.Content = "Create Patch";
        ButtonPatcher.IsEnabled = true;
    }
}
