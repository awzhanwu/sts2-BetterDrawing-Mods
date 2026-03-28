using Godot;
using BetterDrawing.scripts.ui;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;


namespace BetterDrawing.scripts;

partial class BetterDrawingSystem : Node
{
    public bool _enable {private get; set;}
    private bool ZPressed = false;
    private bool CtrlPressed = false;
    private WidthButton _widthButton;
    private VSlider _widthSlider;

    public BetterDrawingSystem(WidthButton widthButton, UndoButton undoButton)
	{
        Name = "BetterDrawingSystem";
        _widthButton = widthButton;
        _widthSlider = widthButton.GetNode<VSlider>("WidthSlider");
        undoButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(RequestUndo));
        DrawingDataAccess._netService.RegisterMessageHandler<BetterDrawingMessage>(HandleBetterDrawingMessage);
	}

    public override void _Process(double _delta)
    {
        // Do not register InputAction, the amount of changes is too large
        // 不进行InputAction注册，修改量过大
        // Wait for a better input mapping basemod before refactoring
        // 可以等待有更好的输入映射basemod后再进行重构
        if (_enable && (Input.IsKeyPressed(Key.Ctrl) || Input.IsKeyPressed(Key.Meta)))
        {
            if (!CtrlPressed)
            {
                CtrlPressed = true;
                DrawingDataAccess._widthMarker.UpdateVisible(1);
                _widthButton.OpenOrCloseByCtrl(true);
            }

            if (Input.IsKeyPressed(Key.Z) && !ZPressed)
            {
                ZPressed = true;
                RequestUndo();
            }
            else if (!Input.IsKeyPressed(Key.Z) && ZPressed)
                ZPressed = false;
        }
        else if (CtrlPressed)
        {
            CtrlPressed = false;
            DrawingDataAccess._widthMarker.UpdateVisible(-1);
            _widthButton.OpenOrCloseByCtrl(false);
        }
    }

    public bool HandleCtrlMouseWheel(MouseButton mouseButton)
    {
        if (CtrlPressed)
        {
            switch (mouseButton)
            {
                case MouseButton.WheelUp:
                    _widthSlider.Value += _widthSlider.Step;
                    return false;
                case MouseButton.WheelDown:
                    _widthSlider.Value -= _widthSlider.Step;
                    return false;
            }
        }
        return true;
    }

    private static void RequestUndo(NButton _button) { RequestUndo();}
    private static void RequestUndo()
    {
        object? state = DrawingDataAccess.GetLocalState();
        if (state == null || DrawingDataAccess.GetIsDrawingByState(state))
            return;

        var lines = DrawingDataAccess
            .GetViewportByState(state)
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
    }

    private void HandleBetterDrawingMessage(BetterDrawingMessage message, ulong senderId)
    {
        if (message.type == BetterDrawingEventType.Undo)
        {
            object? state = DrawingDataAccess.GetStateByPlayerId(senderId);
            if (state == null)
                return;

            var lines = DrawingDataAccess
                .GetViewportByState(state)
                .GetChildren()
                .Cast<Line2D>()
                .ToList();
            if (lines.Count == 0)
                return;

            var lastLine = lines[^1];
            lastLine.QueueFree();
            return;
        }
        else if (message.type == BetterDrawingEventType.SetColor)
            DrawingDataAccess._playerColors[senderId] = message.color;
        else if (message.type == BetterDrawingEventType.SetWidth)
            DrawingDataAccess._playerWidths[senderId] = message.width;
    }
}