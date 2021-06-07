using BepInEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ValheimPictureFrame
{
    public class PictureFrame : PictureFrameBase
    {
        public override Vector3 PivotOffset { get; set; } = new Vector3(0.729f, 0.4473f, 0);
        public override string Name { get; set; } = "$piece_dfirst_pictureframe";
    }
}
