using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using TMPro;
using UnityEngine;
using PostLevelSummary.Helpers;

namespace PostLevelSummary.Patches
{
    [HarmonyPatch(typeof(MenuManager))]
    class RoundDirectorPatches
    {
        [HarmonyPostfix, HarmonyPatch(typeof(MenuManager), nameof(MenuManager.Awake))]
        public static void Awake()
        {
            if (PostLevelSummary.TextInstance == null)
            {
                UI.Init();
            }

            UI.Update();
        }
    }
}
