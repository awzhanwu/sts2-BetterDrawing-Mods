using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace BetterDrawing.scripts;

[ModInitializer("Init")]
public class Entry
{
    private static Harmony? _harmony;

    public static void Init()
    {
        _harmony = new Harmony("sts2.better.drawing");
        _harmony.PatchAll();
        Log.Info("Better Drawing Mod initialized!");
    }
}
