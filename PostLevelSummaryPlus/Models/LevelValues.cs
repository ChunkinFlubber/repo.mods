using System.Collections.Generic;

namespace PostLevelSummaryPlus.Models
{
    public class LevelValues
    {
        public int TotalItems = 0;
        public float TotalValue = 0.0f;
        public readonly List<PlayerStats> PlayerStatsList = new();

        public int ItemsHit = 0;
        public float TotalValueLost = 0.0f;
        public int ItemsBroken = 0;
        public float ExtractedValue = 0.0f;
        public int ExtractedItems = 0;

        public float SessionExtractedValue = 0.0f;
        public float SessionTotalValueLost = 0.0f;
		public int SessionItemsBroken = 0;

        public void Clear()
        {
            PostLevelSummaryPlus.Logger.LogDebug("Clearing level values!");

            TotalItems = 0;
            TotalValue = 0.0f;
            PlayerStatsList.Clear();

            ItemsHit = 0;
            TotalValueLost = 0.0f;
            ItemsBroken = 0;
            ExtractedValue = 0.0f;
            ExtractedItems = 0;
        }

		public void CheckValueChange(ValuableObject val, float valueLost)
		{
			if (valueLost <= 0.0f) return;
			
			PostLevelSummaryPlus.Logger.LogDebug($"{val.name} lost {valueLost} value!");

			ItemsHit += 1;
			TotalValueLost += valueLost;
			SessionTotalValueLost += valueLost;

			PhysGrabObject grabObject = val.GetComponent<PhysGrabObject>();
			if (grabObject != null)
			{
				if (grabObject.playerGrabbing.Count > 0)
				{
					float individualValueLost = valueLost / (float)grabObject.playerGrabbing.Count;
					foreach (PhysGrabber physGrabber in grabObject.playerGrabbing)
					{
						PlayerStats playerStats = PlayerStatsList.Find(player => player.PlayerName == physGrabber.playerAvatar.playerName);
						if (playerStats == null)
						{
							playerStats = new PlayerStats(physGrabber.playerAvatar.playerName);
							PlayerStatsList.Add(playerStats);
						}
						playerStats.ValueLost += individualValueLost;
					}
				}
				else if(grabObject.lastPlayerGrabbing != null)
				{
					PlayerStats playerStats = PlayerStatsList.Find(player => player.PlayerName == grabObject.lastPlayerGrabbing.playerName);
					if (playerStats == null)
					{
						playerStats = new PlayerStats(grabObject.lastPlayerGrabbing.playerName);
						PlayerStatsList.Add(playerStats);
					}
					playerStats.ValueLost += valueLost;
				}
			}

			if (valueLost >= val.dollarValueCurrent)
			{
				ItemBroken(val);
			}
		}

		private void ItemBroken(ValuableObject val)
        {
			PostLevelSummaryPlus.Logger.LogDebug($"Broken {val.name}!");

			ItemsBroken += 1;
            SessionItemsBroken += 1;

			PhysGrabObject grabObject = val.GetComponent<PhysGrabObject>();
			if (grabObject != null)
			{
				if (grabObject.playerGrabbing.Count > 0)
				{
					foreach (PhysGrabber physGrabber in grabObject.playerGrabbing)
					{
						PlayerStats playerStats = PlayerStatsList.Find(player => player.PlayerName == physGrabber.playerAvatar.playerName);
						if (playerStats == null)
						{
							playerStats = new PlayerStats(physGrabber.playerAvatar.playerName);
							PlayerStatsList.Add(playerStats);
						}
						playerStats.ItemsBroken += 1;
					}
				}
				else if(grabObject.lastPlayerGrabbing != null)
				{
					PlayerStats playerStats = PlayerStatsList.Find(player => player.PlayerName == grabObject.lastPlayerGrabbing.playerName);
					if (playerStats == null)
					{
						playerStats = new PlayerStats(grabObject.lastPlayerGrabbing.playerName);
						PlayerStatsList.Add(playerStats);
					}
                    playerStats.ItemsBroken += 1;
				}
			}
		}

        public void Extracted(ExtractionPoint extract)
        {
			PostLevelSummaryPlus.Logger.LogDebug($"Haul of {extract.haulCurrent} extracted!");
			PostLevelSummaryPlus.Logger.LogDebug($"{extract.amountOfValuables} extracted!");
			
            ExtractedValue += extract.haulCurrent;
            ExtractedItems += extract.amountOfValuables;

            SessionExtractedValue += extract.haulCurrent;
        }

	}
}
