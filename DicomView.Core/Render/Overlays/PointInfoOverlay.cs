﻿using RT.Core.Utilities.RTMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomPanel.Core.Render.Overlays
{
    public class PointInfoOverlay:IOverlay
    {
        public Point3d Position { get; set; }
        private POIOverlay poiOverlay = new POIOverlay();
        private TextOverlay textOverlay = new TextOverlay("");
        public PointInfoOverlay()
        {
            Position = new Point3d();
            poiOverlay = new POIOverlay();
            poiOverlay.KeepSameSizeOnScreen = true;
            poiOverlay.RenderCircle = false;
        }

        public void Render(DicomPanelModel model, IRenderContext context)
        {
            poiOverlay.Position = Position;
            Position.CopyTo(textOverlay.Position);
            textOverlay.Position = Position - (model.Camera.RowDir * 25 / model.Camera.Scale);

            /*var HU = model?.Image?.Grid?.Interpolate(Position);
            var doseNorm = model?.Doses?.GetNormalisationAmount();
            var doseVoxel = model?.Doses?.Grid?.Interpolate(Position);*/

            textOverlay.Text = "";

            /*if (HU != null)
                textOverlay.Text = Math.Round(HU.Value) + " HU";
            if (doseVoxel != null)
            {
                textOverlay.Text += "\n" + Math.Round(100*(doseVoxel.Value / (double)doseNorm), 2) + "%";
            }*/

            textOverlay.Text = "("+Math.Round(Position.X,2)+", "+Math.Round(Position.Y,2)+", "+Math.Round(Position.Z,2)+") (mm)";

            poiOverlay.Render(model, context);
            //Use the poi fractin of oriignal size to set opacity of text
            textOverlay.Opacity = poiOverlay.Fraction;

            textOverlay.Render(model, context);
        }
    }
}
