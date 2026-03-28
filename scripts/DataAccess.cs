using HarmonyLib;
using Godot;
using System.Collections;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using BetterDrawing.scripts.ui;

namespace BetterDrawing.scripts;

public static class DrawingDataAccess
{
    public static Dictionary<ulong, Color> _playerColors = new();
    public static Dictionary<ulong, float> _playerWidths = new();
    public static INetGameService? _netService;
    public static NMapDrawings? _mapDrawings;
    public static Material? _eraserMaterial;
    public static WidthMarker? _widthMarker;
    public static List<EyeButton> _eyeButtons = [];

    public static IList GetStates()
    {
        if (_mapDrawings != null)
            return Traverse.Create(_mapDrawings)
                .Field("_drawingStates")
                .GetValue<IList>();
        return new List<object>();
    }

    public static object? GetLocalState()
    {
        return GetStateByPlayerId(_netService.NetId);
    }

    public static object? GetStateByPlayerId(ulong playerId)
    {
        foreach (var state in GetStates())
            if (GetPlayerIdByState(state) == playerId)
                return state;
        return null;
    }

    public static ulong GetPlayerIdByState(object state)
    {
        return Traverse.Create(state)
            .Field("playerId")
            .GetValue<ulong>();
    }

    public static SubViewport GetViewportByState(object state)
    {
        return Traverse.Create(state)
            .Field("drawViewport")
            .GetValue<SubViewport>();
    }

    public static bool GetIsDrawingByState(object state)
    {
        return Traverse.Create(state)
            .Field("overrideDrawingMode")
            .GetValue<DrawingMode?>() != null || 
            Traverse.Create(state)
            .Field("currentlyDrawingLine")
            .GetValue<Line2D?>() != null;
    }
}