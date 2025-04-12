using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace PostLevelSummary.Patches
{
    [HarmonyPatch(typeof(LevelGenerator))]
    class LevelGeneratorPatches
    {
        public static LevelGenerator? Instance;

        [HarmonyPostfix, HarmonyPatch(MethodType.Constructor)]
        public static void CaptureInstance(LevelGenerator __instance)
        {
            Instance = __instance;
            PostLevelSummary.Logger.LogDebug("Captured LevelGenerator instance.");
        }

        [HarmonyPrefix, HarmonyPatch(nameof(LevelGenerator.StartRoomGeneration))]
        public static void StartRoomGenerationPrefix()
        {
            if (Instance == null) return;

            PostLevelSummary.Logger.LogDebug($"Generating new level: {Instance.Level.name}");

            if (Instance.Level.NarrativeName == "Main Menu")
            {
                PostLevelSummary.InMenu = true;
            } else
            {
                PostLevelSummary.InMenu = false;
            }

            if (Instance.Level.HasEnemies)
            {
                PostLevelSummary.InGame = true;
            }
            else
            {
                PostLevelSummary.InGame = false;
            }

            if (Instance.Level.name.ToLower().Contains("shop"))
            {
                PostLevelSummary.InShop = true;
            } else
            {
                PostLevelSummary.InShop = false;
            }
        }

        [HarmonyPostfix, HarmonyPatch(nameof(LevelGenerator.GenerateDone))]
        public static void GenerateDonePostfix()
        {
            if (!PostLevelSummary.InShop)
            {
                PostLevelSummary.Logger.LogDebug("Done generating new level");
                PostLevelSummary.Logger.LogDebug($"Total value: {PostLevelSummary.Level.TotalValue}");
            }
        }
    }
}
