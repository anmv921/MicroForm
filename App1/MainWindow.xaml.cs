// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            Output.ItemsSource = DataAccess.GetData();

            //OutputTextBlock.Text = "teste";
        }

        //private async void myButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var welcomeDialog = new ContentDialog()
        //    {
        //        Title = "Database path:",
        //        Content = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db"),
        //        CloseButtonText = "Ok",
        //        XamlRoot = myButton.XamlRoot
        //    };
        //    await welcomeDialog.ShowAsync();
        //}

        private void AddData(object sender, RoutedEventArgs e)
        {
            DataAccess.AddData(Input_Box.Text);

            Output.ItemsSource = DataAccess.GetData();
        }

        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            DataAccess.DeleteAllData();
            Output.ItemsSource = DataAccess.GetData();
        }
        
        private async void AbrirJanelaFicheiro(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();

            // Get the current window's HWND by passing in the Window object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                // OutputTextBlock.Text = "Picked photo: " + file.Name;
                var lines = await FileIO.ReadLinesAsync(file);
                foreach (string line in lines)
                {
                    //OutputTextBlock.Text += " " + line;
                    DataAccess.AddData(line);
                }
                Output.ItemsSource = DataAccess.GetData();
            }
            else
            {
                //OutputTextBlock.Text = "Operation cancelled.";
            }

        }
    }
}
