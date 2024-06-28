using Microsoft.UI;
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
using Windows.Storage.Pickers;
using Windows.Storage;
using WinUI3FormApp;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppMicroform
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ListViewOutput.ItemsSource = DataAccess.GetData();

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this); // m_window in App.cs
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
        }

        private void AddData(object sender, RoutedEventArgs e)
        {
            DataAccess.AddData(Input_Box.Text);

            ListViewOutput.ItemsSource = DataAccess.GetData();
        }

        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            DataAccess.DeleteAllData();
            ListViewOutput.ItemsSource = DataAccess.GetData();
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
                var lines = await FileIO.ReadLinesAsync(file);
                foreach (string line in lines)
                {
                    DataAccess.AddData(line);
                }
                ListViewOutput.ItemsSource = DataAccess.GetData();
            }
            else
            {
            }

        }
    }
}
