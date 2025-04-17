using System;
using System.Collections.Generic;
using System.Text;

namespace PostLevelSummaryPlus.Models
{
	public struct PostLevelSummaryEventData
	{
		public string Text;
		public bool ShouldShowText;

		public PostLevelSummaryEventData(string text, bool shouldShowText)
		{
			Text = text;
			ShouldShowText = shouldShowText;
		}
	}
}
