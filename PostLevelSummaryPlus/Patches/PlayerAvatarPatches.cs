using HarmonyLib;
using Photon.Pun;

namespace PostLevelSummaryPlus.Patches
{
	[HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.Awake))]
	public class PlayerAvatarPatches
	{
		private static void Postfix(PlayerAvatar __instance)
		{
			if (!PhotonNetwork.IsConnectedAndReady) return;
			
			PostLevelSummaryPlayerAttachment plsAttachment = __instance.GetComponent<PostLevelSummaryPlayerAttachment>();
			PhotonView playerView = __instance.GetComponent<PhotonView>();
			if (plsAttachment == null)
			{
				plsAttachment = __instance.gameObject.AddComponent<PostLevelSummaryPlayerAttachment>();
				if(playerView != null)
				{
					playerView.RPC(nameof(PostLevelSummaryPlayerAttachment.RequestPostLevelSummaryRPC), RpcTarget.MasterClient);
				}
				PostLevelSummaryPlus.Logger.LogDebug((object)("Added PLSAttachment component to PlayerAvatar: " + __instance.name));
			}
			if (playerView != null && playerView.IsMine && PhotonNetwork.IsMasterClient)
			{
				PostLevelSummaryPlus.Instance.MasterPhotonView = playerView;
				PostLevelSummaryPlus.Logger.LogDebug("Set MasterPhotonView for local PlayerAvatar: " + __instance.name);
			}
		}
	}
}
