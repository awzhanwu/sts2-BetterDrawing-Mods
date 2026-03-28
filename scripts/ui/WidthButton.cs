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

	private Label _widthLabel;

	private HoverTip _hoverTip;

	private Tween? _tween;

	private bool _isOpen;

	private bool _beOpenByCtrl;

	private static readonly Color _activeColor = new Color(1f, 1f, 1f);

	private static readonly Color _inactiveColor = new Color(0.4f, 0.4f, 0.4f);

	public WidthButton() : base()
	{
		Name = "WidthButton";
		CustomMinimumSize = new Vector2(60, 60);
		FocusNeighborLeft = "../ClearButton";
		FocusNeighborRight = "../ColorButton";
		FocusMode = FocusModeEnum.All;

		_icon = new TextureRect(){
            Name = "Icon",
            Texture = GD.Load<Texture2D>("res://assets/BetterDrawing/PenNib.png"),
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
            OffsetTop = -469f,
            OffsetLeft = 22f,
            OffsetRight = 38f,
			MouseFilter = MouseFilterEnum.Ignore
        };
		_widthSlider = new VSlider(){
            Name = "WidthSlider",
            Visible = false,

            MinValue = 1f,
            MaxValue = 32f,
            Step = 0.5f,
            Value = 4f,

            AnchorBottom = 1f,
            AnchorTop = 1f,
            OffsetBottom = -60f,
            OffsetTop = -469f,
            OffsetLeft = 22f,
            OffsetRight = 38f,
        };
		_widthSlider.Connect("value_changed", new Callable(this, nameof(OnWidthChanged)));
		_widthLabel = new(){
            Name = "WidthLabel",
            Visible = false,
            Text = "4.0",
            GrowHorizontal = GrowDirection.Both,

            AnchorBottom = 1f,
            AnchorTop = 1f,
            OffsetBottom = -469f,
            OffsetTop = -492f,
            OffsetLeft = 18f,
            OffsetRight = 41f,
			MouseFilter = MouseFilterEnum.Ignore
        };
        _widthLabel.AddThemeStyleboxOverride("normal", new StyleBoxFlat(){BgColor = new Color(0f, 0f, 0f, 0.8f)});
		AddChild(_icon);
        AddChild(_background);
        AddChild(_widthSlider);
        AddChild(_widthLabel);
	}

	public override void _Ready()
	{
		ConnectSignals();
		_drawingToolHolder = (Control)GetParent();
		NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(OnControllerUpdated));
		NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(OnControllerUpdated));
		OnControllerUpdated();
	}

	private void OnWidthChanged(float newWidth)
	{
		DrawingDataAccess._widthMarker.UpdateWidth(newWidth + 18f);
		_widthLabel.Text = newWidth.ToString("F1");
		DrawingDataAccess._playerWidths[DrawingDataAccess._netService.NetId] = newWidth;
		DrawingDataAccess._netService.SendMessage(new BetterDrawingMessage()
		{
			type = BetterDrawingEventType.SetWidth,
			width = newWidth,
		});
	}

	protected override void OnRelease()
	{
		base.OnRelease();
		OpenOrClose(!_isOpen);
	}
	
	private void OpenOrClose(bool isOpen, bool changeFocus = true)
	{
		_isOpen = isOpen;
		_widthSlider.Visible = isOpen;
		_background.Visible = isOpen;
		_widthLabel.Visible = isOpen;
		if (changeFocus)
		{
			if (isOpen) OnUnfocus();
			else OnFocus();
		}
	}

	public void OpenOrCloseByCtrl(bool isOpen)
	{
		if (isOpen)
		{
			if (_isOpen) return;
			_beOpenByCtrl = true;
			OpenOrClose(true, false);
		}
		else
		{
			if (!_beOpenByCtrl) return;
			_beOpenByCtrl = false;
			OpenOrClose(false, false);
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
		LocString title = (!NControllerManager.Instance.IsUsingController) ? new LocString("better_drawing", "WIDTH_BUTTON.title_mkb") : new LocString("better_drawing", "WIDTH_BUTTON.title_controller");
		_hoverTip = new HoverTip(title, description);
	}
}
