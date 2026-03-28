using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Entities.Players;
using BetterDrawing.scripts.ui;

namespace BetterDrawing.scripts.patches;

[HarmonyPatch(typeof(NMapDrawings), "CreateLineForPlayer")]
class Patch_CreateLineForPlayer
{
    static void Postfix(Player player, bool isErasing, Line2D __result)
    {
        __result.DefaultColor = DrawingDataAccess._playerColors.GetValueOrDefault(player.NetId, player.Character.MapDrawingColor);
        __result.Width = DrawingDataAccess._playerWidths.GetValueOrDefault(player.NetId, 4f) + (isErasing ? 8f : 0f);
    }
}

[HarmonyPatch(typeof(NMapDrawings), "Initialize")]
class Patch_Initialize
{
    static void Postfix(IPlayerCollection playerCollection, NMapDrawings __instance)
    {
        INetGameService _netService = Traverse.Create(__instance)
            .Field("_netService")
            .GetValue<INetGameService>();
        Material _eraserMaterial = Traverse.Create(__instance)
            .Field("_eraserMaterial")
            .GetValue<Material>();
        DrawingDataAccess._netService = _netService;
        DrawingDataAccess._eraserMaterial = _eraserMaterial;

        Color characterColor = playerCollection.GetPlayer(_netService.NetId).Character.MapDrawingColor;
        NinePatchRect drawingTools = __instance.GetNode<NinePatchRect>("%DrawingTools");
        drawingTools.GetChild<HBoxContainer>(0).GetNode<ColorButton>("ColorButton").GetNode<ColorPicker>("ColorPicker").Color = characterColor;
        DrawingDataAccess._widthMarker.DefaultColor = characterColor;

        __instance.GetParent<Control>().GetParent<NMapScreen>().AddChild(new BetterDrawingSystem(
            drawingTools.GetChild<HBoxContainer>(0).GetNode<WidthButton>("WidthButton"),
            drawingTools.GetChild<HBoxContainer>(0).GetNode<UndoButton>("UndoButton")
        ));
        
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

[HarmonyPatch(typeof(NMapDrawings), "UpdateLocalCursor")]
class Patch_UpdateLocalCursor
{
    static void Prefix()
    {
        DrawingDataAccess._widthMarker.UpdateDrawingMode(
            Traverse.Create(DrawingDataAccess.GetLocalState())
                .Field("drawingMode")
                .GetValue<DrawingMode>()
        );
    }
}