using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using BetterDrawing.scripts.ui;

namespace BetterDrawing.scripts.patches;

[HarmonyPatch(typeof(NMapScreen), "_Ready")]
class Patch_AddNodes
{
    static void Postfix(NMapScreen __instance)
    {
        NinePatchRect drawingTools = __instance.GetNode<NinePatchRect>("%DrawingTools");
        drawingTools.GetNode<NMapEraseButton>("%EraseButton").AddSibling(new UndoButton());
        drawingTools.GetChild<HBoxContainer>(0).AddChild(new WidthButton());
        drawingTools.GetChild<HBoxContainer>(0).AddChild(new ColorButton());

        drawingTools.Size += new Vector2(180f, 0f);
        drawingTools.GetNode<NMapEraseButton>("%EraseButton").FocusNeighborRight = "../UndoButton";
        drawingTools.GetNode<NMapClearButton>("%ClearButton").FocusNeighborLeft = "../UndoButton";
        drawingTools.GetNode<NMapClearButton>("%ClearButton").FocusNeighborRight = "../WidthButton";

        DrawingDataAccess._widthMarker = new WidthMarker();
        __instance.AddChild(DrawingDataAccess._widthMarker);
    }
}


[HarmonyPatch(typeof(NMapScreen), "ProcessScrollEvent")]
class Patch_ProcessScrollEvent
{
    static bool Prefix(NMapScreen __instance, InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton { ButtonIndex: var buttonIndex })
			return __instance.GetNode<BetterDrawingSystem>("BetterDrawingSystem").HandleCtrlMouseWheel(buttonIndex);
        return true;
    }
}

[HarmonyPatch(typeof(NMapScreen), "OnVisibilityChanged")]
class Patch_OnVisibilityChanged
{
    static void Postfix(NMapScreen __instance)
    {
        __instance.GetNode<BetterDrawingSystem>("BetterDrawingSystem")._enable = __instance.Visible;
        foreach (EyeButton eyeButton in DrawingDataAccess._eyeButtons)
            eyeButton.Visible = __instance.Visible;
    }
}

[HarmonyPatch(typeof(NMapScreen), "Close")]
class Patch_Close
{
    static void Prefix()
    {
        foreach (EyeButton eyeButton in DrawingDataAccess._eyeButtons)
            eyeButton.Visible = false;
    }
}