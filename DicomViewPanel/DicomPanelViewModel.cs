﻿using DicomPanel.Core.Render;
using DicomPanel.Core;
using DicomPanel.Wpf.Rendering;
using DicomPanel.Wpf.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpGL;

namespace DicomPanel.Wpf
{
    public class DicomPanelViewModel:BindableItem
    {

        public DicomPanelModel Model { get; set; }

        public WriteableBitmap ImageBase { get { return _imageBase; } set { SetField(ref _imageBase, value); } }
        private WriteableBitmap _imageBase;

        public WriteableBitmap ImageTop { get { return _imageTop; } set { SetField(ref _imageTop, value); } }
        private WriteableBitmap _imageTop;

        public WriteableBitmapRenderContext ImageBaseRenderContext { get; set; }
        public WriteableBitmapRenderContext ImageTopRenderContext { get; set; }

        public Canvas Canvas { set { OverlayRenderContext.Canvas = value; } }
        public CanvasRenderContext OverlayRenderContext { get; set; }

        public DicomPanelViewModel()
        {
            ImageBaseRenderContext = new WriteableBitmapRenderContext(ImageBase);
            ImageTopRenderContext = new WriteableBitmapRenderContext(ImageBase);
            OverlayRenderContext = new CanvasRenderContext(null);
        }

        public void SetModel(DicomPanelModel model)
        {
            Model = model;
            Model.SetImageRenderContext(ImageBaseRenderContext);
            Model.SetDoseRenderContext(ImageBaseRenderContext);
            Model.SetRoiRenderContext(ImageBaseRenderContext);
            Model.SetOverlayContext(OverlayRenderContext);
            Model?.Invalidate();
        }

        public void OnSizeChanged(double newWidth, double newHeight)
        {
            createRenderTargets((int)Math.Round(newWidth), (int)Math.Round(newHeight));
            Model?.OnResize(newWidth, newHeight);
            Model?.Invalidate();
        }

        private void createRenderTargets(double width, double height)
        {
            if (width <= 1 || height <= 1)
                return;

            double imgWidth = width, imgHeight = height;
            if (width >= 512)
            {
                imgWidth = 512;
                imgHeight *= (512/width);
            }
            else if (height >= 512)
            {
                imgHeight = 512;
                imgWidth *= (512/height);
            }

            ImageBase = new WriteableBitmap((int)Math.Round(imgWidth), (int)Math.Round(imgHeight), 96, 96, PixelFormats.Bgr32, null);
            //ImageTop = new WriteableBitmap((int)Math.Round(width), (int)Math.Round(height),96,96,PixelFormats.Bgra32,null);
            ImageBaseRenderContext.Resize(ImageBase, (int)Math.Round(imgWidth), (int)Math.Round(imgHeight));
            ImageTopRenderContext.Resize(ImageBase, (int)Math.Round(width), (int)Math.Round(height));
            OverlayRenderContext.Canvas.Clip = new RectangleGeometry(new System.Windows.Rect(0, 0, width, height));
            OverlayRenderContext.Width = (int)width;
            OverlayRenderContext.Height = (int)height;
        }
        
        public void Draw(OpenGL gl)
        {
            
        }
    }
}
