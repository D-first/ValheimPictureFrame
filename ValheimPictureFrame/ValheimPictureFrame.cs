using BepInEx;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using ValheimPictureFrame.Utils;

namespace ValheimPictureFrame
{
    [BepInPlugin("dfirst.ValheimPictureFrame", "Valheim Picture Frame", "1.0.0")]
    [BepInDependency(Main.ModGuid)]
    public class ValheimPictureFrame : BaseUnityPlugin
    {
        public static readonly TextureCache spriteCache = new TextureCache("ValheimPictureFrame/Assets/Images");
        public static readonly string TOKEN_NAME = "$piece_dfirst_pictureframe";
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
            var asset = assetBundle.LoadAsset<GameObject>($"Assets/Pieces/PictureFrame/PictureFrame.prefab");

            Sign sign = asset.GetComponent<Sign>();
            sign.m_name = TOKEN_NAME;

            CustomPiece piece = new CustomPiece(asset,
                new PieceConfig()
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
                });

            piece.FixReference = true;
            PieceManager.Instance.AddPiece(piece);
            assetBundle.Unload(false);
        }
    }
}