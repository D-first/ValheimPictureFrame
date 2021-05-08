using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ValheimPictureFrame.Patches
{
    class SignTextPatch
    {
        [HarmonyPatch(typeof(Sign), nameof(Sign.SetText))]
        private class SignSetTextPatch
        {
            static void Postfix(Sign __instance, string ___m_name, Text ___m_textWidget)
            {
                UpdatePicture(__instance, ___m_name, ___m_textWidget);
            }
        }

        [HarmonyPatch(typeof(Sign), "UpdateText")]
        private class SignUpdateTextPatch
        {
            static void Postfix(Sign __instance, string ___m_name, Text ___m_textWidget)
            {
                UpdatePicture(__instance, ___m_name, ___m_textWidget);
            }
        }

        private static void UpdatePicture(Sign __instance, string ___m_name, Text ___m_textWidget)
        {
            if (___m_name != ValheimPictureFrame.TOKEN_NAME)
            {
                return;
            }

            Renderer pictureRenderer = __instance.transform.Find("New/Picture").gameObject.GetComponent<Renderer>();
            if (pictureRenderer.material.mainTexture != null && pictureRenderer.material.mainTexture.name == ___m_textWidget.text)
            {
                return;
            }

            pictureRenderer.material.mainTexture = ValheimPictureFrame.spriteCache.Load(___m_textWidget.text);
        }
    }
}
