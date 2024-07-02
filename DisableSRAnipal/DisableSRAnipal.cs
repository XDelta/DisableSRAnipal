using HarmonyLib;
using ResoniteModLoader;
using UnityFrooxEngineRunner;

namespace DisableSRAnipal;

public partial class DisableSRAnipal : ResoniteMod {
	public override string Name => "DisableSRAnipal";
	public override string Author => "Delta";
	public override string Version => "1.0.0";
	public override string Link => "https://github.com/XDelta/DisableSRAnipal";

	[AutoRegisterConfigKey]
	private static readonly ModConfigurationKey<bool> Enabled = new("Enabled", "Should SRAnipal setup be enabled (restart required)", () => true);

	internal static ModConfiguration Config;

	public override void OnEngineInit() {
		Config = GetConfiguration();
		Config.Save(true);

		Harmony harmony = new("net.deltawolf.DisableSRAnipal");
		harmony.PatchAll();
	}

	[HarmonyPatch(typeof(UnityLaunchOptions), "ForceSRAnipal", MethodType.Getter)]
	private class UnityLaunchOptionsForceSRAnipalPatch {
		public static bool Prefix(ref bool __result) {
			if (Config.GetValue(Enabled)) {
				__result = false;
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(ViveProEyeTrackingDriver), "ShouldRegister", MethodType.Getter)]
	private class ViveProEyeTrackingDriverShouldRegisterPatch {
		public static bool Prefix(ref bool __result) {
			if (Config.GetValue(Enabled)) {
				__result = false;
				return false;
			}
			return true;
		}
	}
}
