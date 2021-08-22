using DolphinDynamicInputTextureCreator.Other;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;

namespace DolphinDynamicInputTextureCreator.Data
{
    public class RegionBrush : Other.PropertyChangedBase
    {
        #region PROPERTIES
        /// <summary>
        /// The currently selected emulated device for this "brush"
        /// </summary>
        private EmulatedDevice _selected_emulated_device;
        public EmulatedDevice SelectedEmulatedDevice
        {
            get { return _selected_emulated_device; }
            set
            {
                _selected_emulated_device = value;
                OnPropertyChanged(nameof(SelectedEmulatedDevice));
            }
        }

        /// <summary>
        /// The currently selected emulated key for this "brush"
        /// </summary>
        private EmulatedKey _selected_emulated_key;
        public EmulatedKey SelectedEmulatedKey
        {
            get { return _selected_emulated_key; }
            set
            {
                _selected_emulated_key = value;
                OnPropertyChanged(nameof(SelectedEmulatedKey));
            }
        }

        /// <summary>
        /// whether subpixels are used.
        /// </summary>
        private bool _subpixel;
        public bool Subpixel
        {
            get { return _subpixel; }
            set
            {
                _subpixel = value;
                RectRegion.DecimalPlaces = value ? 1 : 0;
                OnPropertyChanged(nameof(Subpixel));
            }
        }

        /// <summary>
        /// Canvas grid.
        /// </summary>
        private CanvasGrid _grid;
        public CanvasGrid Grid
        {
            get 
            {
                if (_grid == null)
                    Grid = new CanvasGrid(0, 0, 1, 1);
                return _grid; 
            }
            set
            {
                _grid = value;
                _grid.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(UpdateCanvasGrid);
                OnPropertyChanged(nameof(Grid));
            }
        }

        private void UpdateCanvasGrid(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CanvasGrid))
            {
                OnPropertyChanged(nameof(Grid));
                RectRegion.Grid = _grid;
            }
        }

        /// <summary>
        /// sets the current RectRegion to the grid.
        /// </summary>
        private ICommand _set_grid_command;
        [JsonIgnore]
        public ICommand SetGridCommand
        {
            get
            {
                if (_set_grid_command == null)
                {
                    _set_grid_command = new RelayCommand(param => SetGrid((RectRegion)param));
                }
                return _set_grid_command;
            }
        }

        private void SetGrid(RectRegion region)
        {
            Grid = (Rect)region;
        }
        #endregion
    }
}
