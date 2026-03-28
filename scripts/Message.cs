using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace BetterDrawing.scripts;

public enum BetterDrawingEventType
{
    SetColor,
	SetWidth,
    Undo,
}

public class BetterDrawingMessage : INetMessage, IPacketSerializable
{
	public bool ShouldBroadcast => true;

	public NetTransferMode Mode => NetTransferMode.Reliable;

	public LogLevel LogLevel => LogLevel.VeryDebug;

	public BetterDrawingEventType type;

    public Color color;

    public float width;

	public void Serialize(PacketWriter writer)
	{
		writer.WriteEnum(type);
        if (type == BetterDrawingEventType.SetColor)
        {
            writer.WriteFloat(color.R);
            writer.WriteFloat(color.G);
            writer.WriteFloat(color.B);
        }
		else if (type == BetterDrawingEventType.SetWidth)
			writer.WriteFloat(width);
	}

	public void Deserialize(PacketReader reader)
	{
		type = reader.ReadEnum<BetterDrawingEventType>();
        if (type == BetterDrawingEventType.SetColor)
        {
            color = new Color(
				reader.ReadFloat(),
				reader.ReadFloat(),
				reader.ReadFloat()
			);
        }
		else if (type == BetterDrawingEventType.SetWidth)
			width = reader.ReadFloat();
	}
}
