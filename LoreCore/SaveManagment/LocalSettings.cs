using LoreCore.Enums;
using System.Collections.Generic;

namespace LoreCore.SaveManagment;

public class LocalSettings
{
	#region Properties

	public Dictionary<Traveller, int> Stages { get; set; }

	#endregion
}
