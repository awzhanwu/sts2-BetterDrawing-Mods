using Godot;
using HarmonyLib;
using System.Collections;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Multiplayer.Game;

namespace BetterDrawing.scripts;

public static class DrawingDataAccess
{
    public static Dictionary<ulong, Color> _playerColors = new();
    public static Dictionary<ulong, float> _playerWidths = new();
    public static INetGameService _netService;
    public static NMapDrawings _mapDrawings;
    public static Material _eraserMaterial;

    public static IList GetStates()
    {
        return Traverse.Create(_mapDrawings)
            .Field("_drawingStates")
            .GetValue<IList>();
    }

    public static ulong GetPlayerId(object state)
    {
        return Traverse.Create(state)
            .Field("playerId")
            .GetValue<ulong>();
    }

    public static SubViewport GetViewport(object state)
    {
        return Traverse.Create(state)
            .Field("drawViewport")
            .GetValue<SubViewport>();
    }

    public static bool GetIsDrawing(object state)
    {
        return Traverse.Create(state)
            .Field("overrideDrawingMode")
            .GetValue<DrawingMode?>() != null || 
            Traverse.Create(state)
            .Field("currentlyDrawingLine")
            .GetValue<Line2D?>() != null;
    }
}