﻿using DolphinDynamicInputTextureCreator.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DolphinDynamicInputTextureCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Data.DynamicInputTextureEvents.ImageNotExist = Dialogs.ImageNotExistMessage;
            InitializeComponent();
            this.Title += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            InputPack = Models.DefaultData.NewInputPack();
        }

        private DynamicInputPackViewModel InputPack
        {
            get => (DynamicInputPackViewModel)DataContext;
            set
            {
                if (value == null)
                    return;

                value.CheckImagePaths();
                _edit_host_devices_window?.Close();
                DataContext = value;
                ((PanZoomViewModel)PanZoom.DataContext).InputPack = value;
                UnsavedChanges = false;
            }
        }

        private string _saved_document = null;

        private Window _edit_emulated_devices_window;
        private Window _edit_default_host_devices_window;
        private Window _edit_host_devices_window;
        private Window _edit_metadata_window;

        private void QuitProgram_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditDefaultHostDevices_Click(object sender, RoutedEventArgs e)
        {
            _edit_default_host_devices_window?.Close();

            _edit_default_host_devices_window = new Window
            {
                Title = "Editing Default Host Devices",
                Icon = (ImageSource)FindResource("Image.HostDevices"),
                ResizeMode = ResizeMode.CanResize,
                SizeToContent = SizeToContent.Manual,
                Owner = Application.Current.MainWindow,
                Top = this.Top + 50, Left = this.Left + 70,
                Width = 620, Height = 550, MinWidth = 500, MinHeight = 400
            };

            UpdateEditWindows();
            _edit_default_host_devices_window.Show();
        }

        public ICommand EditHostDevicesCommand => new ViewModels.Commands.RelayCommand<Data.DynamicInputTexture>(EditHostDevicesS);

        private void EditHostDevicesS(Data.DynamicInputTexture texture)
        {
            _edit_host_devices_window?.Close();

            var user_control = new HostDeviceKeyViewModel { HostDevices = new ViewModels.Commands.UICollection<Data.HostDevice>(texture.HostDevices) };
            texture.HostDevices = user_control.HostDevices;

            _edit_host_devices_window = new Window
            {
                Title = "Editing Host Devices of " + texture.TextureHash,
                ResizeMode = ResizeMode.CanResize,
                SizeToContent = SizeToContent.Manual,
                Owner = Application.Current.MainWindow,
                Top = this.Top + 50, Left = this.Left + 70,
                Width = 620, Height = 550, MinWidth = 500, MinHeight = 400,
                Content = new Controls.EditHostDevices { DataContext = user_control }
            };

            _edit_host_devices_window.Show();
        }

        private void EditEmulatedDevices_Click(object sender, RoutedEventArgs e)
        {
            _edit_emulated_devices_window?.Close();

            _edit_emulated_devices_window = new Window
            {
                Title = "Editing Emulated Devices",
                Icon = (ImageSource)FindResource("Image.EmulatedDevices"),
                ResizeMode = ResizeMode.CanResize,
                SizeToContent = SizeToContent.Manual,
                Owner = Application.Current.MainWindow,
                Top = this.Top + 50, Left = this.Left + 70,
                Width = 620, Height = 550, MinWidth = 500, MinHeight = 400
            };

            UpdateEditWindows();
            _edit_emulated_devices_window.Show();
        }

        private void EditMetadata_Click(object sender, RoutedEventArgs e)
        {
            _edit_metadata_window?.Close();

            _edit_metadata_window = new Window
            {
                Title = "Editing Metadata",
                Icon = (ImageSource)FindResource("Image.Metadata"),
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight,
                Owner = Application.Current.MainWindow,
                Top = this.Top + 50, Left = this.Left + 70
            };

            UpdateEditWindows();
            _edit_metadata_window.Show();
        }

        public static RoutedUICommand SaveAsCmd = new RoutedUICommand("Save as...", "SaveAsCmd", typeof(MainWindow));

        #region NEW
        private void NewData_Click(object sender, RoutedEventArgs e)
        {
            InputPack = Models.DefaultData.NewInputPack();
            UpdateEditWindows();
            _saved_document = null;
        }

        private void NewData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region SAVE
        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            if (_saved_document == null)
            {
                SaveAsData_Click(sender, e);
            }
            else
            {
                string output = JsonConvert.SerializeObject(InputPack, Formatting.Indented);
                File.WriteAllText(_saved_document, output);
                UnsavedChanges = false;
            }
        }

        private void SaveData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region OPEN

        private void OpenData_Click(object sender, RoutedEventArgs e)
        {
            InputPack = Dialogs.DialogOpenDIT(ref _saved_document);
            UpdateEditWindows();
        }

        private void OpenData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region Export Import

        private void ExportData_Click(object sender, RoutedEventArgs e)
        {
            Dialogs.DialogExportToLocation(InputPack);
        }

        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            DynamicInputPackViewModel inputPack = Dialogs.DialogImportFromLocation(Models.DefaultData.NewInputPack());

            if (inputPack != null)
            {
                InputPack = inputPack;
                _saved_document = null;
                UpdateEditWindows();
            }
        }

        #endregion

        #region SAVE AS

        private void SaveAsData_Click(object sender, RoutedEventArgs e)
        {
            bool result = Dialogs.DialogSaveDIT(InputPack, ref _saved_document);
            if (result)
            {
                UnsavedChanges = false;
            }
        }

        private void SaveStartupSuggestions_Click(object sender, RoutedEventArgs e)
        {
            Models.DefaultData.SaveSettings();
        }

        private void SaveAsDefaultPack_Click(object sender, RoutedEventArgs e)
        {
            Models.DefaultData.SaveInputPack(InputPack);
        }

        private void SaveDataAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        private void UpdateEditWindows()
        {
            if (_edit_emulated_devices_window != null)
            {
                var user_control = new Controls.EditEmulatedDevices { DataContext = new EmulatedDeviceKeysViewModel { InputPack = InputPack } };
                _edit_emulated_devices_window.Content = user_control;
            }

            if (_edit_default_host_devices_window != null)
            {
                var user_control = new Controls.EditHostDevices { DataContext = new HostDeviceKeyViewModel { HostDevices = InputPack.HostDevices } };
                _edit_default_host_devices_window.Content = user_control;
            }

            if (_edit_metadata_window != null)
            {
                var user_control = new Controls.Metadata { DataContext = InputPack };
                _edit_metadata_window.Content = user_control;
            }
        }

        #region Closing

        public bool UnsavedChanges
        {
            get => unsavedChanges;
            set
            {
                unsavedChanges = value;

                if (unsavedChanges)
                {
                    InputPack.Textures.CollectionChanged -= ChangesObserved;
                    InputPack.HostDevices.CollectionChanged -= ChangesObserved;
                    InputPack.EmulatedDevices.CollectionChanged -= ChangesObserved;
                }
                else
                {
                    InputPack.Textures.CollectionChanged += ChangesObserved;
                    InputPack.HostDevices.CollectionChanged += ChangesObserved;
                    InputPack.EmulatedDevices.CollectionChanged += ChangesObserved;
                }
            }
        }
        private bool unsavedChanges = false;

        private void ChangesObserved(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => UnsavedChanges = true;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (unsavedChanges)
            {
               switch (MessageBox.Show("The project has unsaved changes, do you want to save?", "unsaved changes!", MessageBoxButton.YesNoCancel, MessageBoxImage.Information))
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        SaveData_Click(sender, new RoutedEventArgs());
                        e.Cancel = unsavedChanges;
                        break;
                }
            }
        }

        #endregion

    }
}
