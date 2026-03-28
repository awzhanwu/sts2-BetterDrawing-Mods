using Godot;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace BetterDrawing.scripts.ui;

partial class ColorButton : NButton
{
	private Control _drawingToolHolder;

	private TextureRect _icon;

	private ColorRect _background;

	private ColorPicker _colorPicker;

	private HoverTip _hoverTip;

	private Tween? _tween;

	private bool _isOpen;

	private static readonly Color _activeColor = new Color(1f, 1f, 1f);

	private static readonly Color _inactiveColor = new Color(0.4f, 0.4f, 0.4f);

	public ColorButton() : base()
	{
		Name = "ColorButton";
		CustomMinimumSize = new Vector2(60, 60);
		FocusNeighborLeft = "../WidthButton";
		FocusMode = FocusModeEnum.All;

		_icon = new TextureRect(){
            Name = "Icon",
            Texture = GD.Load<Texture2D>("res://assets/BetterDrawing/Prism.png"),
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            Size = new Vector2(50f, 50f),
            Position = new Vector2(3f, 3f),
            SelfModulate = new Color(0.3f, 0.3f, 0.3f),
			MouseFilter = MouseFilterEnum.Ignore
        };
		_background = new ColorRect(){
            Name = "Background",
            Visible = false,
            Color = new Color(0f, 0f, 0f, 0.8f),

            AnchorBottom = 1f,
            AnchorTop = 1f,
            OffsetBottom = -60f,
            OffsetTop = -492f,
            OffsetRight = 298f,
			MouseFilter = MouseFilterEnum.Ignore
        };
		_colorPicker = new ColorPicker(){
            Name = "ColorPicker",
            Visible = false,

            EditAlpha = false,
            EditIntensity = false,
            DeferredMode = true,
            CanAddSwatches = false,
            ColorModesVisible = false,
            PresetsVisible = false,

            AnchorBottom = 1f,
            AnchorTop = 1f,
            OffsetBottom = -60f,
            OffsetTop = -492f,
            OffsetRight = 298f,
        };
		_colorPicker.Connect("color_changed", new Callable(this, nameof(OnColorChanged)));

		AddChild(_icon);
        AddChild(_background);
        AddChild(_colorPicker);
	}

	public override void _Ready()
	{
		ConnectSignals();
		_drawingToolHolder = (Control)GetParent();
		_hoverTip = new HoverTip(new LocString("better_drawing", "COLOR_BUTTON.title"), new LocString("better_drawing", "COLOR_BUTTON.description"));
	}

	private void OnColorChanged(Color newColor)
	{
		DrawingDataAccess._widthMarker.DefaultColor = newColor;
		DrawingDataAccess._playerColors[DrawingDataAccess._netService.NetId] = newColor;
		DrawingDataAccess._netService.SendMessage(new BetterDrawingMessage()
		{
			type = BetterDrawingEventType.SetColor,
			color = newColor,
		});
	}

	protected override void OnRelease()
	{
		base.OnRelease();
		OpenOrClose(!_isOpen);
	}
	
	private void OpenOrClose(bool isOpen)
	{
		_isOpen = isOpen;
		_colorPicker.Visible = isOpen;
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
}
