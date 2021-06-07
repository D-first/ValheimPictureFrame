﻿using UnityEngine;

namespace ValheimPictureFrame
{
    class PictureFrameSquare : PictureFrameBase
    {
        public override Vector3 PivotOffset { get; set; } = new Vector3(0.4473f, 0.4473f, 0);
        public override string Name { get; set; } = "$piece_dfirst_pictureframe_square";
    }
}
