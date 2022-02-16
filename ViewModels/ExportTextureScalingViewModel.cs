using DolphinDynamicInputTexture.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace DolphinDynamicInputTextureCreator.ViewModels
{
    public class ExportTextureScalingViewModel : Other.PropertyChangedBase
    {

        /// <summary>
        /// Possible texture scaling modes.
        /// </summary>
        internal enum Modes
        {
            None, NearestNeighbor, Bicubic, Bilinear
        }

        /// <summary>
        /// The currently selected export texture scaling mode.
        /// </summary>
        internal Modes SelectedScalingMode = Modes.None;

        /// <summary>
        /// Scaling factor that the exported image should have.
        /// </summary>
        public int SelectedScalingFactor { get; set; } = 4;

        /// <summary>
        /// Number of pixels that the smallest region should have at least after exporting, when the Dynamic mode is used.
        /// </summary>
        public int SelectedRegioneSize { get; set; } = 96;

        public bool UseDynamicSize
        {
            get => _use_dynamic_size;
            set
            {
                _use_dynamic_size = value;
                OnPropertyChanged(nameof(UseDynamicSize));
            }
        }

        private bool _use_dynamic_size = false;

        #region UI Helper

        public string[] ScalingModesHelper
        {
            get => Enum.GetNames(typeof(Modes));
        }

        public string SelectedScalingModeHelper
        {
            get => SelectedScalingMode.ToString();
            set
            {
                SelectedScalingMode = Enum.Parse<Modes>(value);
                SetExportTextureScaling();
                OnPropertyChanged(nameof(SelectedScalingModeHelper));
                OnPropertyChanged(nameof(SelectedScalingMode));
            }
        }

        public int[] ScalingFactorHelper
        {
            get => new int[] { 1, 2, 3, 4, 5, 6, 8, 10 };
        }

        public int[] RegioneSizeHelper
        {
            get => new int[] { 32, 64, 96, 128, 160, 192, 256, 320, 512 };
        }

        #endregion

        /// <summary>
        /// set the export scale for each texture.
        /// </summary>
        private void SetExportTextureScaling()
        {
            switch (SelectedScalingMode)
            {
                case Modes.None:
                    DynamicInputTextureEvents.DynamicInputTextureExportProcessor = null;
                    break;
                case Modes.NearestNeighbor:
                case Modes.Bicubic:
                case Modes.Bilinear:
                    DynamicInputTextureEvents.DynamicInputTextureExportProcessor = DefaultScalingProcessor;
                    break;
                default:
                    break;
            }
        }

        public bool DefaultScalingProcessor(string savepath, DynamicInputTexture dynamicinputtexture)
        {
            int Scaling = SelectedScalingFactor;

            if (UseDynamicSize)
            {
                Scaling = GetDynamicExportTextureScaling(dynamicinputtexture);
            }

            //Is this scaling already the current one?
            if (Scaling == dynamicinputtexture.ImageWidthScaling)
            {
                return false;
            }

            using (Bitmap newImage = new Bitmap(dynamicinputtexture.HashProperties.ImageWidth * Scaling, dynamicinputtexture.HashProperties.ImageHeight * Scaling))
            {
                using (Bitmap Image = new Bitmap(dynamicinputtexture.TexturePath))
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    switch (SelectedScalingMode)
                    {
                        case Modes.NearestNeighbor:
                            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                            break;
                        case Modes.Bicubic:
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            break;
                        case Modes.Bilinear:
                            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                            break;
                        default:
                            break;
                    }

                    graphics.DrawImage(Image, 0, 0, newImage.Width, newImage.Height);
                }
                newImage.Save(savepath, System.Drawing.Imaging.ImageFormat.Png);

            }

            return true;
        }

        /// <summary>
        /// Calculates a good texture scaling based on region size.
        /// </summary>
        /// <param name="texture"></param>
        private int GetDynamicExportTextureScaling(DynamicInputTexture texture)
        {
            // if there are no regions.
            if (texture.Regions.Count <= 0)
            {
                return 1;
            }

            //calculate a scaling based on the regions.
            int smallest_region = texture.ImageWidth;
            foreach (InputRegion region in texture.Regions)
            {
                int regionsize = (int)(region.RegionRect.Height + region.RegionRect.Width) / 2;
                if (smallest_region > regionsize)
                {
                    smallest_region = regionsize;
                }
            }
            smallest_region = (int)(smallest_region / ((texture.ImageHeightScaling + texture.ImageWidthScaling) / 2));
            int scaling = (int)Math.Round((double)SelectedRegioneSize / smallest_region, 0, MidpointRounding.AwayFromZero);

            //the image scaling must be more than zero.
            if (scaling <= 0)
            {
                return 1;
            }

            //unlikely however try to avoid too large textures.
            int pixelsize = texture.HashProperties.ImageWidth > texture.HashProperties.ImageHeight ? texture.HashProperties.ImageWidth : texture.HashProperties.ImageHeight;
            if (pixelsize * scaling > 4096)
            {
                scaling = (int)Math.Round((double)4096 / pixelsize, MidpointRounding.ToZero);
            }

            return scaling;
        }

    }
}
