using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using PostLevelSummary.Helpers;
using PostLevelSummary.Models;
using WebSocketSharp;

namespace PostLevelSummary
{
    public static class UI
    {
        public static void Init()
        {
            GameObject hud = GameObject.Find("Game Hud");
            GameObject haul = GameObject.Find("Tax Haul");

            TMP_FontAsset font = haul.GetComponent<TMP_Text>().font;

            PostLevelSummary.TextInstance = new GameObject();
            PostLevelSummary.TextInstance.SetActive(false);
            PostLevelSummary.TextInstance.name = "Summary HUD";
            PostLevelSummary.TextInstance.AddComponent<TextMeshProUGUI>();

            PostLevelSummary.ValueText = PostLevelSummary.TextInstance.GetComponent<TextMeshProUGUI>();
            PostLevelSummary.ValueText.font = font;
            PostLevelSummary.ValueText.color = new Vector4(0.7882f, 0.9137f, 0.902f, 1);
            PostLevelSummary.ValueText.fontSize = 18f;
            PostLevelSummary.ValueText.enableWordWrapping = false;
            PostLevelSummary.ValueText.alignment = TextAlignmentOptions.BaselineRight;
            PostLevelSummary.ValueText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            PostLevelSummary.ValueText.verticalAlignment = VerticalAlignmentOptions.Baseline;

            PostLevelSummary.TextInstance.transform.SetParent(hud.transform, false);

            RectTransform component = PostLevelSummary.TextInstance.GetComponent<RectTransform>();

            component.pivot = new Vector2(1f, 1f);
            component.anchoredPosition = new Vector2(1f, -1f);
            component.anchorMin = new Vector2(0f, 0f);
            component.anchorMax = new Vector2(1f, 0f);
            component.sizeDelta = new Vector2(0f, 0f);
            component.offsetMax = new Vector2(0, 225f);
            component.offsetMin = new Vector2(0f, 225f);

            PostLevelSummary.Logger.LogDebug("HUD generated");
        }
        public static void Update()
        {
            RectTransform component = PostLevelSummary.TextInstance.GetComponent<RectTransform>();

            component.pivot = new Vector2(1f, 1f);
            component.anchoredPosition = new Vector2(1f, -1f);
            component.anchorMin = new Vector2(0f, 0f);
            component.anchorMax = new Vector2(1f, 0f);
            component.sizeDelta = new Vector2(0f, 0f);
            component.offsetMax = new Vector2(0, 225f);
            component.offsetMin = new Vector2(0f, 225f);

            PostLevelSummary.ValueText.lineSpacing = -50f;

            string playerMostLostValue = "";
            string playerMostBrokenItems = "";
            float mostValueLost = 0.0f;
            int mostItemsBroken = 0;
            foreach(PlayerBlame playerBlame in PostLevelSummary.Level.PlayerBlames)
            {
                if(playerBlame.ValueLost > mostValueLost)
                {
                    mostValueLost = playerBlame.ValueLost;
                    playerMostLostValue = playerBlame.PlayerName;
                }
                if(playerBlame.ItemsBroken > mostItemsBroken)
                {
                    mostItemsBroken = playerBlame.ItemsBroken;
                    playerMostBrokenItems = playerBlame.PlayerName;
                }
            }

            PostLevelSummary.ValueText.SetText($@"
                    Session Totals:
                        Extracted Value ${NumberFormatter.FormatToK(PostLevelSummary.Level.SessionExtractedValue)}
                        Lost ${NumberFormatter.FormatToK(PostLevelSummary.Level.SessionTotalValueLost)}
                        Broken Items {PostLevelSummary.Level.SessionItemsBroken}

                    Level Totals:
                        Extracted Value ${NumberFormatter.FormatToK(PostLevelSummary.Level.ExtractedValue)} out of ${NumberFormatter.FormatToK(PostLevelSummary.Level.TotalValue)}
                        {PostLevelSummary.Level.ExtractedItems} items out of {PostLevelSummary.Level.TotalItems}
                        Lost ${NumberFormatter.FormatToK(PostLevelSummary.Level.TotalValueLost)}
                        {PostLevelSummary.Level.ItemsBroken} {string.Format("item{0}", PostLevelSummary.Level.ItemsBroken == 1 ? "" : "s")} broken ({PostLevelSummary.Level.ItemsHit} hits)

                    Congratulations {string.Format(playerMostBrokenItems.IsNullOrEmpty() ? "for not breaking anything!" : string.Format("to {0} for breaking the most items!", playerMostBrokenItems))}
                    Congratulations {string.Format(playerMostLostValue.IsNullOrEmpty() ? "for not losing any value!" : string.Format("to {0} for losing the most value!", playerMostLostValue))}
                ");
        }
    }
}
