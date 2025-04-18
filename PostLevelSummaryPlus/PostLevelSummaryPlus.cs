using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;
using Photon.Pun;
using PostLevelSummaryPlus.Models;
using PostLevelSummaryPlus.Patches;

namespace PostLevelSummaryPlus;

[BepInPlugin("ChunkinFlubber.PostLevelSummaryPlus", "PostLevelSummaryPlus", "1.1")]
public class PostLevelSummaryPlus : BaseUnityPlugin
{
    internal static PostLevelSummaryPlus Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    public LevelValues Level = new();

    public GameObject TextInstance = null!;
    public TextMeshProUGUI ValueText = null!;
	public PhotonView MasterPhotonView = null!;

    private void Awake()
    {
        Instance = this;
        
        // Prevent the plugin from being deleted
        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll(typeof(LevelGeneratorPatches));
        Harmony.PatchAll(typeof(PhysGrabObjectImpactDetectorPatches));
        Harmony.PatchAll(typeof(RoundDirectorPatches));
        Harmony.PatchAll(typeof(ExtractionPointPatches));
        Harmony.PatchAll(typeof(PlayerAvatarPatches));

		Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    internal void Unpatch()
    {
	    Harmony?.UnpatchSelf();
    }

    public void ResetValues()
    {
		if (Instance == null) return;
		Instance.Level.Clear();
    }

	public void ReceivePostLevelSummary(string text)
	{
		if (Instance == null) return;

		RectTransform component = Instance.TextInstance.GetComponent<RectTransform>();

		component.pivot = new Vector2(1f, 1f);
		component.anchoredPosition = new Vector2(1f, -1f);
		component.anchorMin = new Vector2(0f, 0f);
		component.anchorMax = new Vector2(1f, 0f);
		component.sizeDelta = new Vector2(0f, 0f);
		component.offsetMax = new Vector2(0, 225f);
		component.offsetMin = new Vector2(0f, 225f);

		Instance.ValueText.lineSpacing = -50f;

		Instance.ValueText.SetText(text);
		Instance.TextInstance.SetActive(true);
		
		Logger.LogDebug($"Received Post Level Summary: {text}");
	}

	public void RemovePostLevelSummary()
	{
		if (Instance == null) return;

		Logger.LogDebug("Removing Post Level Summary Text");
		Instance.ValueText.SetText("");
		Instance.TextInstance.SetActive(false);
	}

	public static void LevelGenerationDone()
	{
		if (Instance == null || !PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.IsMasterClient) return;

		Logger.LogDebug($"Done generating new level {RunManager.instance.levelCurrent.name}");

		bool isInShop = SemiFunc.RunIsShop();
		if (isInShop)
		{
			UI.Update();
		}
		else if(SemiFunc.RunIsLevel() || SemiFunc.RunIsArena())
		{
			Instance.ResetValues();
		}

		Instance.Level.TotalItems = ValuableDirector.instance.valuableList.Count;
		ValuableDirector.instance.valuableList.ForEach(valuable => { Instance.Level.TotalValue += valuable.dollarValueCurrent; });
		Logger.LogDebug($"Items {Instance.Level.TotalItems} with total value {Instance.Level.TotalValue}!");

		if (isInShop)
		{
			Logger.LogDebug("Sending summary text");
			Instance.MasterPhotonView.RPC(nameof(PostLevelSummaryPlayerAttachment.ReceivePostLevelSummaryRPC), RpcTarget.All, (object)Instance.ValueText.text);
		}
		else
		{
			Logger.LogDebug("Sending remove summary");
			Instance.MasterPhotonView.RPC(nameof(PostLevelSummaryPlayerAttachment.RemovePostLevelSummaryRPC), RpcTarget.All);
		}
	}

	internal static void RequestPostLevelSummary(Photon.Realtime.Player requestor)
	{
		if (Instance == null || Instance.MasterPhotonView == null) return;
		if (!PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnectedAndReady) return;
		
		Logger.LogDebug("Received client request for summary");

		if (!SemiFunc.RunIsShop() && PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnectedAndReady)
		{
			Logger.LogDebug("Sending remove summary");
			Instance.MasterPhotonView.RPC(nameof(PostLevelSummaryPlayerAttachment.RemovePostLevelSummaryRPC), requestor);
		}
		else if(SemiFunc.RunIsShop())
		{
			Logger.LogDebug("Sending summary text");
			Instance.MasterPhotonView.RPC(nameof(PostLevelSummaryPlayerAttachment.ReceivePostLevelSummaryRPC), requestor, (object)Instance.ValueText.text);
		}
	}
}

public class PostLevelSummaryPlayerAttachment : MonoBehaviour
{
	[PunRPC]
	public void ReceivePostLevelSummaryRPC(string summary)
	{
		PostLevelSummaryPlus.Instance.ReceivePostLevelSummary(summary);
	}

	[PunRPC]
	public void RemovePostLevelSummaryRPC()
	{
		PostLevelSummaryPlus.Instance.RemovePostLevelSummary();
	}

	[PunRPC]
	public void RequestPostLevelSummaryRPC(PhotonMessageInfo info)
	{
		PostLevelSummaryPlus.RequestPostLevelSummary(info.Sender);
	}
}