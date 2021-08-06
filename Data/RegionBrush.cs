using System;
using System.Collections.Generic;
using System.Text;

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
        /// Whether to fill the entire image with the region
        /// </summary>
        private bool _fill_region;
        public bool FillRegion
        {
            get { return _fill_region; }
            set
            {
                _fill_region = value;
                OnPropertyChanged(nameof(FillRegion));
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
                RectRegion.DecimalPlaces = value ? 1 : 0;
                _subpixel = value;
                OnPropertyChanged(nameof(Subpixel));
            }
        }

        /// <summary>
        /// Canvas grid width.
        /// </summary>
        private int _grid_width = 1;
        public int GridWidth
        {
            get { return _grid_width; }
            set
            {
                _grid_width = value;
                if (_grid_width > 512) _grid_width = 512;
                if (_grid_width < 1) _grid_width = 1;
                RectRegion.GridWidth = _grid_width;
                OnPropertyChanged(nameof(GridWidth));
            }
        }

        /// <summary>
        /// Canvas grid height.
        /// </summary>
        private int _grid_height = 1;
        public int GridHeight
        {
            get { return _grid_height; }
            set
            {
                _grid_height = value;
                if (_grid_height > 512) _grid_height = 512;
                if (_grid_height < 1) _grid_height = 1;
                RectRegion.GridHeight = _grid_height;
                OnPropertyChanged(nameof(GridHeight));
            }
        }
        #endregion
    }
}
