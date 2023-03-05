using LoreCore.Enums;
using System.Collections.Generic;

namespace LoreCore.SaveManagment;

public class LocalSettings
{
	#region Properties

	public Dictionary<Traveller, int> Stages { get; set; }

	public bool CanRead { get; set; } = true;

	public bool CanListen { get; set; } = true;

	#endregion
}
