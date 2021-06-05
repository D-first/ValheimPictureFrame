using BepInEx;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using System.Collections.Generic;
using UnityEngine;
using ValheimPictureFrame.Utils;

namespace ValheimPictureFrame
{
    [BepInPlugin("dfirst.ValheimPictureFrame", "Valheim Picture Frame", "1.2.0")]
    [BepInDependency(Main.ModGuid)]
    public class ValheimPictureFrame : BaseUnityPlugin
    {
        public static readonly string TOKEN_NAME = "$piece_dfirst_pictureframe";
        public static readonly string TOKEN_NAME_VERTICAL = "$piece_dfirst_pictureframe_vertical";
        public static readonly string TOKEN_NAME_SQUARE = "$piece_dfirst_pictureframe_square";
        private static readonly string TOKEN_DESC = "$piece_dfirst_pictureframe_description";

        public static readonly TextureCache textureCache = new TextureCache("ValheimPictureFrame/Assets/Images");
        public static readonly Dictionary<string, Vector3> pictureFrames = new Dictionary<string, Vector3>()
        {
            { TOKEN_NAME, new Vector3(0.729f, 0.4473f, 0) },
            { TOKEN_NAME_VERTICAL, new Vector3(0.4473f, 0.729f, 0) },
            { TOKEN_NAME_SQUARE, new Vector3(0.4473f, 0.4473f, 0) },
        };
        private readonly Harmony harmony = new Harmony("dfirst.ValheimPictureFrame");

        private void Awake()
        {
            AddPiece();
            harmony.PatchAll();
        }

        private void AddPiece()
        {
            var assetBundle = AssetBundleHelper.GetAssetBundleFromResources("pictureframe");
            string[] prefabNames = new string[] { "PictureFrame", "PictureFrameVertical", "PictureFrameSquare" };

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
                prefab.AddComponent<PictureFrame>();
                var piece = new CustomPiece(prefab, config)
                {
                    FixReference = true
                };
                PieceManager.Instance.AddPiece(piece);
            }

            assetBundle.Unload(false);
        }
    }
}