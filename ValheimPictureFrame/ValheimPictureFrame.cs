using BepInEx;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using System;
using UnityEngine;
using ValheimPictureFrame.Utils;

namespace ValheimPictureFrame
{
    [BepInPlugin("dfirst.ValheimPictureFrame", "Valheim Picture Frame", "1.1.0")]
    [BepInDependency(Main.ModGuid)]
    public class ValheimPictureFrame : BaseUnityPlugin
    {
        public static readonly TextureCache spriteCache = new TextureCache("ValheimPictureFrame/Assets/Images");
        public static readonly string TOKEN_NAME = "$piece_dfirst_pictureframe";
        public static readonly string TOKEN_NAME_VERTICAL = "$piece_dfirst_pictureframe_vertical";
        public static readonly string TOKEN_NAME_SQUARE = "$piece_dfirst_pictureframe_square";
        private static readonly string TOKEN_DESC = "$piece_dfirst_pictureframe_description";
        private readonly Harmony harmony = new Harmony("dfirst.ValheimPictureFrame");

        private void Awake()
        {
            AddPiece();
            harmony.PatchAll();
        }

        private void AddPiece()
        {
            var assetBundle = AssetBundleHelper.GetAssetBundleFromResources("pictureframe");
            string[] prefabNames = new string[] { "PictureFrame", "PictureFrameVertical", "PictureFrameSquare"};
            PieceConfig config = new PieceConfig()
            {
                PieceTable = "Hammer",
                Description = TOKEN_DESC,
                Requirements = new[]
    {
                    new RequirementConfig()
                    {
                        Item = "FineWood",
                        Amount = 6,
                        Recover = true
                    },
                    new RequirementConfig()
                    {
                        Item = "BronzeNails",
                        Amount = 2,
                        Recover = true
                    }
                }
            };
            
            foreach (var prefabName in prefabNames)
            {
                var prefab = assetBundle.LoadAsset<GameObject>($"Assets/Pieces/PictureFrame/{prefabName}.prefab");
                var piece = new CustomPiece(prefab, config);
                piece.FixReference = true;
                PieceManager.Instance.AddPiece(piece);

            }

            assetBundle.Unload(false);
        }
    }
}