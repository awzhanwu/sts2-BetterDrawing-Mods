using Godot;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace BetterDrawing.scripts.ui;

partial class UndoButton : NButton
{
	private Control _drawingToolHolder;

	private TextureRect _icon;

	private HoverTip _hoverTip;

	private Tween? _tween;

	private static readonly Color _activeColor = new Color(1f, 1f, 1f);

	private static readonly Color _inactiveColor = new Color(0.4f, 0.4f, 0.4f);

	public UndoButton() : base()
	{
		Name = "UndoButton";
		CustomMinimumSize = new Vector2(60, 60);
		FocusNeighborLeft = "../EraseButton";
		FocusNeighborRight = "../ClearButton";
		FocusMode = FocusModeEnum.All;

		_icon = new TextureRect(){
            Name = "Icon",
            Texture = GD.Load<AtlasTexture>("res://images/atlases/compressed.sprites/back_button_arrow.tres"),
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            Size = new Vector2(50f, 50f),
            Position = new Vector2(3f, 3f),
            SelfModulate = new Color(0.3f, 0.3f, 0.3f),
			MouseFilter = MouseFilterEnum.Ignore
        };

		AddChild(_icon);
	}

	public override void _Ready()
	{
		ConnectSignals();
		_drawingToolHolder = (Control)GetParent();
		NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(OnControllerUpdated));
		NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(OnControllerUpdated));
		OnControllerUpdated();
	}
	
	protected override void OnFocus()
	{
		base.OnFocus();
		_tween?.Kill();
		_tween = CreateTween().SetParallel();
		_tween.TweenProperty(_icon, "scale", Vector2.One * 1.2f, 0.05);
		_tween.TweenProperty(_icon, "self_modulate", _activeColor, 0.05);
		NHoverTipSet nHoverTipSet = NHoverTipSet.CreateAndShow(_drawingToolHolder, _hoverTip);
		nHoverTipSet.GlobalPosition = _drawingToolHolder.GlobalPosition + new Vector2(10f, -132f);
	}

	protected override void OnUnfocus()
	{
		base.OnUnfocus();
		_tween?.Kill();
		_tween = CreateTween().SetParallel();
		_tween.TweenProperty(_icon, "scale", Vector2.One * 1.1f, 0.05);
		_tween.TweenProperty(_icon, "self_modulate", _inactiveColor, 0.05);
		NHoverTipSet.Remove(_drawingToolHolder);
	}

	private void OnControllerUpdated()
	{
		LocString description = new("better_drawing", "UNDO_BUTTON.description");
		LocString title = (!NControllerManager.Instance.IsUsingController) ? new LocString("better_drawing", "UNDO_BUTTON.title_mkb") : new LocString("better_drawing", "UNDO_BUTTON.title_controller");
		_hoverTip = new HoverTip(title, description);
	}
}
