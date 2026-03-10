using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using BetterDrawing.scripts.ui;

namespace BetterDrawing.scripts.patches;

[HarmonyPatch(typeof(NMapScreen), "_Ready")]
class Patch_AddButton
{
    static void Postfix(NMapScreen __instance)
    {
        WidthButton widthButton = new()
        {
            Name = "WidthButton",
            CustomMinimumSize = new Vector2(60, 60),
        };
        widthButton.AddChild(new TextureRect(){
            Name = "Icon",
            Texture = GD.Load<Texture2D>("res://assets/PenNib.png"),
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            Size = new Vector2(50f, 50f),
            Position = new Vector2(3f, 3f),
            SelfModulate = new Color(0.3f, 0.3f, 0.3f),
        });
        widthButton.AddChild(new ColorRect(){
            Name = "Background",
            Visible = false,
            Color = new Color(0f, 0f, 0f, 0.8f),

            AnchorBottom = 1f,
            AnchorTop = 1f,
            OffsetBottom = -60f,
            OffsetTop = -492f,
            OffsetLeft = 22f,
            OffsetRight = 38f,
        });
        widthButton.AddChild(new VSlider(){
            Name = "WidthSlider",
            Visible = false,

            MinValue = 1f,
            MaxValue = 32f,
            Step = 0.5f,
            Value = 4f,

            AnchorBottom = 1f,
            AnchorTop = 1f,
            OffsetBottom = -60f,
            OffsetTop = -492f,
            OffsetLeft = 22f,
            OffsetRight = 38f,
        });

        ColorButton colorButton = new()
        {
            Name = "ColorButton",
            CustomMinimumSize = new Vector2(60, 60),
        };
        colorButton.AddChild(new TextureRect(){
            Name = "Icon",
            Texture = GD.Load<Texture2D>("res://assets/Prism.png"),
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            Size = new Vector2(50f, 50f),
            Position = new Vector2(3f, 3f),
            SelfModulate = new Color(0.3f, 0.3f, 0.3f),
        });
        colorButton.AddChild(new ColorRect(){
            Name = "Background",
            Visible = false,
            Color = new Color(0f, 0f, 0f, 0.8f),

            AnchorBottom = 1f,
            AnchorTop = 1f,
            OffsetBottom = -60f,
            OffsetTop = -492f,
            OffsetRight = 298f,
        });
        colorButton.AddChild(new ColorPicker(){
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
        });

        NinePatchRect drawingTools = __instance.GetNode<NinePatchRect>("%DrawingTools");
        drawingTools.GetChild<HBoxContainer>(0).AddChild(widthButton);
        drawingTools.GetChild<HBoxContainer>(0).AddChild(colorButton);
        drawingTools.Size += new Vector2(120f, 0f);
    }
}