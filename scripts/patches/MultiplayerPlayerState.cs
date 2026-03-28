using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using BetterDrawing.scripts.ui;

namespace BetterDrawing.scripts.patches;

[HarmonyPatch(typeof(NMultiplayerPlayerState), "UpdateHighlightedState")]
class Patch_MutipleHover
{

    static void Prefix(ref bool ____isHighlighted, ref bool __state)
    {
        __state = ____isHighlighted;
    }

    static void Postfix(ref bool ____isHighlighted, NMultiplayerPlayerState __instance, bool __state)
    {
        bool newValue = ____isHighlighted;
        if (newValue == __state)
            return;

        ulong playerID = __instance.Player.NetId;
        foreach (var state in DrawingDataAccess.GetStates())
            if (DrawingDataAccess.GetPlayerIdByState(state) != playerID)
                foreach (Line2D line in DrawingDataAccess.GetViewportByState(state).GetChildren().Cast<Line2D>())
                    if (line.Material != DrawingDataAccess._eraserMaterial)
                        line.SelfModulate = new Color(1f, 1f, 1f, newValue ? 0.3f : 1f);
    }
}

[HarmonyPatch(typeof(NMultiplayerPlayerState), "_Ready")]
class Patch_AddEyeButton
{

    static void Postfix(NMultiplayerPlayerState __instance)
    {
        EyeButton eyeButton = new(__instance.Player.NetId);
        DrawingDataAccess._eyeButtons.Add(eyeButton);
        __instance.AddChild(eyeButton);
    }
}
