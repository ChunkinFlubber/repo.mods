using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using Photon.Pun;
using PostLevelSummaryPlus.Helpers;
using PostLevelSummaryPlus.Models;
using SingularityGroup.HotReload;

namespace PostLevelSummaryPlus
{
    public static class UI
    {
        public static void Init()
        {
            if (PostLevelSummaryPlus.Instance == null) return;

            GameObject hud = GameObject.Find("Game Hud");
            GameObject haul = GameObject.Find("Tax Haul");

            TMP_FontAsset font = haul.GetComponent<TMP_Text>().font;
            
            PostLevelSummaryPlus.Instance.TextInstance = new GameObject();
            PostLevelSummaryPlus.Instance.TextInstance.SetActive(false);
            PostLevelSummaryPlus.Instance.TextInstance.name = "Summary HUD";
            PostLevelSummaryPlus.Instance.TextInstance.AddComponent<TextMeshProUGUI>();

            PostLevelSummaryPlus.Instance.ValueText = PostLevelSummaryPlus.Instance.TextInstance.GetComponent<TextMeshProUGUI>();
            PostLevelSummaryPlus.Instance.ValueText.font = font;
            PostLevelSummaryPlus.Instance.ValueText.color = new Vector4(0.7882f, 0.9137f, 0.902f, 1);
            PostLevelSummaryPlus.Instance.ValueText.fontSize = 18f;
            PostLevelSummaryPlus.Instance.ValueText.enableWordWrapping = false;
            PostLevelSummaryPlus.Instance.ValueText.alignment = TextAlignmentOptions.BaselineRight;
            PostLevelSummaryPlus.Instance.ValueText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            PostLevelSummaryPlus.Instance.ValueText.verticalAlignment = VerticalAlignmentOptions.Baseline;

            PostLevelSummaryPlus.Instance.TextInstance.transform.SetParent(hud.transform, false);

            RectTransform component = PostLevelSummaryPlus.Instance.TextInstance.GetComponent<RectTransform>();

            component.pivot = new Vector2(1f, 1f);
            component.anchoredPosition = new Vector2(1f, -1f);
            component.anchorMin = new Vector2(0f, 0f);
            component.anchorMax = new Vector2(1f, 0f);
            component.sizeDelta = new Vector2(0f, 0f);
            component.offsetMax = new Vector2(0, 225f);
            component.offsetMin = new Vector2(0f, 225f);

            PostLevelSummaryPlus.Logger.LogDebug("HUD generated");
        }
        public static void Update()
        {
			if (PostLevelSummaryPlus.Instance == null) return;

			RectTransform component = PostLevelSummaryPlus.Instance.TextInstance.GetComponent<RectTransform>();

            component.pivot = new Vector2(1f, 1f);
            component.anchoredPosition = new Vector2(1f, -1f);
            component.anchorMin = new Vector2(0f, 0f);
            component.anchorMax = new Vector2(1f, 0f);
            component.sizeDelta = new Vector2(0f, 0f);
            component.offsetMax = new Vector2(0, 225f);
            component.offsetMin = new Vector2(0f, 225f);

            PostLevelSummaryPlus.Instance.ValueText.lineSpacing = -50f;

            string playerMostLostValue = "";
            string playerMostBrokenItems = "";
            float mostValueLost = 0.0f;
            int mostItemsBroken = 0;
            foreach (PlayerBlame playerBlame in PostLevelSummaryPlus.Instance.Level.PlayerBlames)
            {
                if (playerBlame.ValueLost > mostValueLost)
                {
                    mostValueLost = playerBlame.ValueLost;
                    playerMostLostValue = playerBlame.PlayerName;
                }
                if (playerBlame.ItemsBroken > mostItemsBroken)
                {
                    mostItemsBroken = playerBlame.ItemsBroken;
                    playerMostBrokenItems = playerBlame.PlayerName;
                }
            }

            PostLevelSummaryPlus.Instance.ValueText.SetText($@"
                    Session Totals:
                        Extracted ${NumberFormatter.FormatToK(PostLevelSummaryPlus.Instance.Level.SessionExtractedValue)}
                        Lost ${NumberFormatter.FormatToK(PostLevelSummaryPlus.Instance.Level.SessionTotalValueLost)}
                        Broken Items {PostLevelSummaryPlus.Instance.Level.SessionItemsBroken}

                    Level Totals:
                        Extracted ${NumberFormatter.FormatToK(PostLevelSummaryPlus.Instance.Level.ExtractedValue)} out of ${NumberFormatter.FormatToK(PostLevelSummaryPlus.Instance.Level.TotalValue)}
                        {PostLevelSummaryPlus.Instance.Level.ExtractedItems} items out of {PostLevelSummaryPlus.Instance.Level.TotalItems}
                        Lost ${NumberFormatter.FormatToK(PostLevelSummaryPlus.Instance.Level.TotalValueLost)}
                        {PostLevelSummaryPlus.Instance.Level.ItemsBroken} {$"item{(PostLevelSummaryPlus.Instance.Level.ItemsBroken == 1 ? "" : "s")}"} broken ({PostLevelSummaryPlus.Instance.Level.ItemsHit} hits)

                    Congratulations {string.Format(playerMostBrokenItems.IsNullOrEmpty() ? "for not breaking anything!" : $"to {playerMostBrokenItems} for breaking the most items ({mostItemsBroken})!")}
                    Congratulations {string.Format(playerMostLostValue.IsNullOrEmpty() ? "for not losing any value!" : $"to {playerMostLostValue} for losing the most value ({mostValueLost})!")}
                ");
        }
    }
}
