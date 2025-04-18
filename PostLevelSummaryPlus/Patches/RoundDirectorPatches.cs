using HarmonyLib;

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
