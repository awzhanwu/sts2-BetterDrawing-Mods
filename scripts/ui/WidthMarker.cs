using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace BetterDrawing.scripts.ui;

public partial class WidthMarker : Line2D
{
    private DrawingMode _drawingMode = DrawingMode.None;
    private float normalWidth = 22f;
    private int CanVisible = 0;

    public WidthMarker()
    {
        Name = "WidthMarker";
        SelfModulate = new Color(1f, 1f, 1f, 0.15f);
        Visible = false;
        Closed = true;
        AddPoint(new Vector2(-0.1f, -0.1f));
        AddPoint(new Vector2(-0.1f, 0.1f));
        AddPoint(new Vector2(0.1f, 0.1f));
        AddPoint(new Vector2(0.1f, -0.1f));
        UpdateWidth();
    }

    public override void _Process(double _delta)
    {
        if (Visible)
            GlobalPosition = GetGlobalMousePosition();
    }

    public void UpdateVisible(int added = 0)
    {
        CanVisible += added;
        Visible = CanVisible > 0;
    }

    public void UpdateWidth(float newWidth = -1)
    {
        if (newWidth > 0) normalWidth = newWidth;
        Width = normalWidth + (_drawingMode == DrawingMode.Erasing ? 16f : 0f);
    }
    public void UpdateDrawingMode(DrawingMode drawingMode)
    {
        if (_drawingMode != drawingMode)
        {
            if (drawingMode == DrawingMode.Drawing || drawingMode == DrawingMode.Erasing) UpdateVisible(1);
            else UpdateVisible(-1);
            _drawingMode = drawingMode;
        }
        UpdateWidth();
    }
}