using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Entities.Players;
using BetterDrawing.scripts.ui;

namespace BetterDrawing.scripts.patches;

[HarmonyPatch(typeof(NMapDrawings), "CreateLineForPlayer")]
class Patch_DrawingColor
{
    static void Postfix(Player player, bool isErasing, Line2D __result)
    {
        __result.DefaultColor = DrawingDataAccess._playerColors.GetValueOrDefault(player.NetId, player.Character.MapDrawingColor);
        __result.Width = DrawingDataAccess._playerWidths.GetValueOrDefault(player.NetId, 4f) + (isErasing ? 8f : 0f);
    }
}

[HarmonyPatch(typeof(NMapDrawings), "Initialize")]
class Patch_NetService
{
    static void Postfix(IPlayerCollection playerCollection, NMapDrawings __instance)
    {
        INetGameService _netService = Traverse.Create(__instance)
            .Field("_netService")
            .GetValue<INetGameService>();
        
        DrawingDataAccess._netService = _netService;

        BetterDrawingSystem sys = new();
        sys.Initialize(__instance);
        __instance.AddChild(sys);

        Color characterColor = playerCollection.GetPlayer(_netService.NetId).Character.MapDrawingColor;
        NinePatchRect drawingTools = __instance.GetNode<NinePatchRect>("%DrawingTools");
        drawingTools.GetChild<HBoxContainer>(0).GetNode<ColorButton>("ColorButton").Initialize(characterColor);
        _netService.SendMessage(new BetterDrawingMessage()
        {
            type = BetterDrawingEventType.SetColor,
            color = characterColor,
        });
        _netService.SendMessage(new BetterDrawingMessage()
        {
            type = BetterDrawingEventType.SetWidth,
            width = 4f,
        });

        DrawingDataAccess._mapDrawings = __instance;
    }
}