using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using TMPro;
using UnityEngine;
using PostLevelSummaryPlus.Helpers;
using UnityEngine.SceneManagement;

namespace PostLevelSummaryPlus.Patches
{
    [HarmonyPatch(typeof(MenuManager))]
    class RoundDirectorPatches
    {
        [HarmonyPostfix, HarmonyPatch(typeof(MenuManager), nameof(MenuManager.Awake))]
        public static void Awake()
        {
            if (PostLevelSummaryPlus.Instance == null) return;

            if (PostLevelSummaryPlus.Instance.TextInstance == null)
            {
                UI.Init();
            }
            UI.Update();
        }
    }
}
