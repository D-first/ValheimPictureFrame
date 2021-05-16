using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ValheimPictureFrame.Patches
{
    class SignTextPatch
    {
        [HarmonyPatch(typeof(Sign), nameof(Sign.SetText))]
        private class SignSetTextPatch
        {
            static void Prefix(string ___m_name, Text ___m_textWidget, ref string __state)
            {
                if (___m_name == ValheimPictureFrame.TOKEN_NAME)
                {
                    __state = ___m_textWidget.text;
                }
            }

            static void Postfix(Sign __instance, string ___m_name, Text ___m_textWidget, string __state)
            {
                UpdatePicture(__instance, ___m_name, ___m_textWidget, __state);
            }
        }

        [HarmonyPatch(typeof(Sign), "UpdateText")]
        private class SignUpdateTextPatch
        {
            static void Prefix(string ___m_name, Text ___m_textWidget, ref string __state)
            {
                if (___m_name == ValheimPictureFrame.TOKEN_NAME)
                {
                    __state = ___m_textWidget.text;
                }
            }

            static void Postfix(Sign __instance, string ___m_name, Text ___m_textWidget, string __state)
            {
                UpdatePicture(__instance, ___m_name, ___m_textWidget, __state);
            }
        }

        private static void UpdatePicture(Sign __instance, string ___m_name, Text ___m_textWidget, string beforeText)
        {
            var pictureFrames = new Dictionary<string, Vector3>()
            {
                { ValheimPictureFrame.TOKEN_NAME, new Vector3(0.729f, 0.4473f, 0) },
                { ValheimPictureFrame.TOKEN_NAME_VERTICAL, new Vector3(0.4473f, 0.729f, 0) },
                { ValheimPictureFrame.TOKEN_NAME_SQUARE, new Vector3(0.4473f, 0.4473f, 0) },
            };

            if ( !pictureFrames.Keys.Any(x => x == ___m_name))
            {
                return;
            }

            if (___m_textWidget.text == beforeText)
            {
                return;
            }

            var text = ___m_textWidget.text.Split(':');
            GameObject pictureFrame = __instance.gameObject;
            Transform pivotObject = pictureFrame.transform.Find("Pivot");
            pictureFrame.transform.localScale = Vector3.one;
            pivotObject.transform.localPosition = Vector3.zero;

            if (text.Length == 2)
            {
                string[] args = text[1].Split(' ');
                var options = ParseOptions(args);
                Vector3 pivotOffset = new Vector3(0, 0, 0);

                if (options.ContainsKey("pivot") || options.ContainsKey("p"))
                {
                    string key = options.ContainsKey("pivot") ? "pivot" : "p";
                    string[] pivots = options[key].Split(',');
                    foreach (var pivot in pivots)
                    {
                        switch (pivot)
                        {
                            case "t":
                            case "top":
                                pivotOffset += new Vector3(0, -pictureFrames[___m_name].y, 0);
                                break;
                            case "b":
                            case "bottom":
                                pivotOffset += new Vector3(0, pictureFrames[___m_name].y, 0);
                                break;
                            case "r":
                            case "right":
                                pivotOffset += new Vector3(pictureFrames[___m_name].x, 0, 0);
                                break;
                            case "l":
                            case "left":
                                pivotOffset += new Vector3(-pictureFrames[___m_name].x, 0, 0);
                                break;

                        }
                    }
                    pivotOffset += new Vector3(0, 0, 0.023f);
                }

                if (options.ContainsKey("scale") || options.ContainsKey("s"))
                {
                    string key = options.ContainsKey("scale") ? "scale" : "s";
                    float scale = Math.Max(Math.Min(float.Parse(options[key]), 10.0f), 0.1f);
                    pivotObject.transform.localPosition = pivotOffset;
                    pictureFrame.transform.localScale = Vector3.one * scale;
                    pivotObject.transform.localPosition -= pivotOffset / scale;
                }
            }

            Renderer pictureRenderer = __instance.transform.Find("Pivot/New/Picture").gameObject.GetComponent<Renderer>();
            var textureName = text[0].Trim();
            pictureRenderer.material.mainTexture = ValheimPictureFrame.textureCache.Load(textureName);
        }

        private static Dictionary<string, string> ParseOptions(string[] args)
        {
            var options = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                string[] option = arg.Split('=');
                if (option.Length == 2)
                {
                    options.Add(option[0], option[1]);
                }
            }

            return options;
        }
    }
}
