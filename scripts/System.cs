using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;


namespace BetterDrawing.scripts;

partial class BetterDrawingSystem : Node
{
    private NMapDrawings _mapDrawings;

    private bool ZPressed = false;

    public void Initialize(NMapDrawings mapDrawings)
	{
		_mapDrawings = mapDrawings;
        DrawingDataAccess._netService.RegisterMessageHandler<BetterDrawingMessage>(HandleBetterDrawingMessage);
	}

    public override void _Process(double _delta)
    {
        // Do not register InputAction, the amount of changes is too large
        // 不进行InputAction注册，修改量过大
        // Wait for a better input mapping basemod before refactoring
        // 可以等待有更好的输入映射basemod后再进行重构
        if (_mapDrawings.Visible && (Input.IsKeyPressed(Key.Ctrl) || Input.IsKeyPressed(Key.Meta)))
        {
            if (Input.IsKeyPressed(Key.Z) && !ZPressed)
            {
                ZPressed = true;
                RequestUndo();
            }
            else if (!Input.IsKeyPressed(Key.Z) && ZPressed)
                ZPressed = false;
        }
    }

    private void RequestUndo()
    {
        foreach (var state in DrawingDataAccess.GetStates())
        {
            if (DrawingDataAccess.GetPlayerId(state) == DrawingDataAccess._netService.NetId)
            {
                if (DrawingDataAccess.GetIsDrawing(state))
                    return;

                var lines = DrawingDataAccess
                    .GetViewport(state)
                    .GetChildren()
                    .Cast<Line2D>()
                    .ToList();
                if (lines.Count == 0)
                    return;

                var lastLine = lines[^1];
                lastLine.QueueFree();

                lines.RemoveAt(lines.Count - 1);
                DrawingDataAccess._netService.SendMessage(new BetterDrawingMessage
                {
                    type = BetterDrawingEventType.Undo,
                });
                return;
            }
        }
    }

    private void HandleBetterDrawingMessage(BetterDrawingMessage message, ulong senderId)
    {
        if (message.type == BetterDrawingEventType.Undo)
        {
            foreach (var state in DrawingDataAccess.GetStates())
            {
                if (DrawingDataAccess.GetPlayerId(state) == senderId)
                {
                    var lines = DrawingDataAccess
                        .GetViewport(state)
                        .GetChildren()
                        .Cast<Line2D>()
                        .ToList();
                    if (lines.Count == 0)
                        return;

                    var lastLine = lines[^1];
                    lastLine.QueueFree();
                    return;
                }
            }
        }
        else if (message.type == BetterDrawingEventType.SetColor)
            DrawingDataAccess._playerColors[senderId] = message.color;
        else if (message.type == BetterDrawingEventType.SetWidth)
            DrawingDataAccess._playerWidths[senderId] = message.width;
    }
}