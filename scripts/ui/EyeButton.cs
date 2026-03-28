using Godot;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace BetterDrawing.scripts.ui;

public partial class EyeButton : NButton
{

	private TextureRect _flash;

	private TextureRect _visuals;

	private Tween? _hoverTween;

	private bool _isHide = false;

	private ulong _playerId;

	private Control? _stateParent =	null;

	public EyeButton(ulong playerId) : base()
	{
		Size = new Vector2(28f, 28f);
		AnchorLeft = AnchorRight = 1f;
		AnchorTop = AnchorBottom = 0.5f;
		OffsetLeft = -28f * 4;
		OffsetTop = -14f;
		OffsetRight = -28f * 3;
		OffsetBottom = 14f;
		Visible = false;

		_visuals = new()
		{
			Texture = GD.Load<AtlasTexture>("res://images/atlases/ui_atlas.sprites/peek_button.tres"),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
			Size = new Vector2(28f, 28f),
			MouseFilter = MouseFilterEnum.Ignore
		};
		_flash = new()
		{
			Texture = GD.Load<CompressedTexture2D>("res://images/packed/common_ui/peek_button_sdf.png"),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			Size = new Vector2(34f, 34f),
			Position = new Vector2(-3f, -3f),
			Modulate = new Color("#eab9ac"),
			ShowBehindParent = true,
			Material = new CanvasItemMaterial(){ BlendMode = CanvasItemMaterial.BlendModeEnum.Add},
			MouseFilter = MouseFilterEnum.Ignore
		};
		_visuals.AddChild(_flash);
		AddChild(_visuals);

		_playerId = playerId;
		object? state = DrawingDataAccess.GetStateByPlayerId(playerId);
		if (state != null) _stateParent = DrawingDataAccess.GetViewportByState(state).GetParent<Control>();
	}

	public override void _Ready()
	{
		ConnectSignals();
	}

	public void SetHideOrNot(bool isHide)
	{
		if (_isHide == isHide)
			return;
		if (_stateParent == null) _stateParent = DrawingDataAccess.GetViewportByState(DrawingDataAccess.GetStateByPlayerId(_playerId)).GetParent<Control>();
		_stateParent.Visible = _flash.Visible = !isHide;
		_isHide = isHide;
	}

	protected override void OnRelease()
	{
		SetHideOrNot(!_isHide);
		_hoverTween?.Kill();
		_hoverTween = CreateTween();
		_hoverTween.TweenProperty(_visuals, "scale", Vector2.One, 0.15);
	}

	protected override void OnPress()
	{
		base.OnPress();
		_hoverTween?.Kill();
		_hoverTween = CreateTween();
		_hoverTween.TweenProperty(_visuals, "scale", Vector2.One * 0.95f, 0.05);
	}

	protected override void OnFocus()
	{
		base.OnFocus();
		_hoverTween?.Kill();
		_hoverTween = CreateTween();
		_hoverTween.TweenProperty(_visuals, "scale", Vector2.One * 1.1f, 0.05);
	}

	protected override void OnUnfocus()
	{
		base.OnUnfocus();
		_hoverTween?.Kill();
		_hoverTween = CreateTween();
		_hoverTween.TweenProperty(_visuals, "scale", Vector2.One, 0.15);
	}
}
