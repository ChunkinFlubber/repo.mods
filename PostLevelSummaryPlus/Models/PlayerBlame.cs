﻿
namespace PostLevelSummaryPlus.Models
{
	public class PlayerBlame
	{
		public PlayerBlame(string playerName)
		{
			PlayerName = playerName;
			ValueLost = 0.0f;
			ItemsBroken = 0;
		}

		public string PlayerName { get; set; }
		public float ValueLost { get; set; }
		public int ItemsBroken { get; set; }
	}
}
