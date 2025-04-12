using System;
using System.Collections.Generic;
using System.Text;

namespace PostLevelSummary.Models
{
	public class PlayerBlame
	{
		public string PlayerName { get; set; }
		public float ValueLost { get; set; }
		public int ItemsBroken { get; set; }
	}
}
