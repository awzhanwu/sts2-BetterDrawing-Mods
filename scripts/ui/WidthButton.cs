using Godot;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace BetterDrawing.scripts.ui;

partial class WidthButton : NButton
{
	private Control _drawingToolHolder;

	private TextureRect _icon;

	private ColorRect _background;

	private VSlider _widthSlider;

	private HoverTip _hoverTip;

	private Tween? _tween;

	private bool _isOpen;

	private static readonly Color _activeColor = new Color(1f, 1f, 1f);

	private static readonly Color _inactiveColor = new Color(0.3f, 0.3f, 0.3f);

	public override void _Ready()
	{
		ConnectSignals();
		_drawingToolHolder = (Control)GetParent();
		_icon = GetNode<TextureRect>("Icon");
		_background = GetNode<ColorRect>("Background");
		_widthSlider = GetNode<VSlider>("WidthSlider");
		_widthSlider.Connect("value_changed", new Callable(this, nameof(OnWidthChanged)));

		NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(OnControllerUpdated));
		NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(OnControllerUpdated));
		OnControllerUpdated();
	}

	private void OnWidthChanged(float newWidth){
		DrawingDataAccess._playerWidths[DrawingDataAccess._netService.NetId] = newWidth;
		DrawingDataAccess._netService.SendMessage(new BetterDrawingMessage()
		{
			type = BetterDrawingEventType.SetWidth,
			width = newWidth,
		});
	}

	protected override void OnPress()
	{
		base.OnPress();
		OpenOrClose(!_isOpen);
	}
	
	private void OpenOrClose(bool isOpen)
	{
		_isOpen = isOpen;
		_widthSlider.Visible = isOpen;
		_background.Visible = isOpen;
		if (isOpen) {
			OnUnfocus();
		}
		else {
			OnFocus();
		}
	}

	protected override void OnFocus()
	{
		if (_isOpen) {
			return;
		}
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
		_tween.TweenProperty(_icon, "self_modulate", _isOpen ? _activeColor : _inactiveColor, 0.05);
		NHoverTipSet.Remove(_drawingToolHolder);
	}

	private void OnControllerUpdated()
	{
		LocString description = new("better_drawing", "WIDTH_BUTTON.description");
		LocString title = new("better_drawing", "WIDTH_BUTTON.title_mkb");
		_hoverTip = new HoverTip(title, description);
	}
}
