using BB;
using GameFramework;
using GameFramework.Event;

public sealed class InputEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(InputEventArgs).GetHashCode();
    
    public override int Id => EventId;

    public float OffsetX
    {
        get;
        private set;
    }
    
    public float OffsetY
    {
        get;
        private set;
    }

    public GameEnum.INPUT_TYPE InputType
    {
        get;
        private set;
    }
    
    public static InputEventArgs Create(GameEnum.INPUT_TYPE inputType, float offsetX, float offsetY)
    {
        var inputEventArgs = ReferencePool.Acquire<InputEventArgs>();
        inputEventArgs.OffsetX = offsetX;
        inputEventArgs.OffsetY = offsetY;
        inputEventArgs.InputType = inputType;

        return inputEventArgs;
    }

    public static InputEventArgs Create(GameEnum.INPUT_TYPE inputType)
    {
        var inputEventArgs = ReferencePool.Acquire<InputEventArgs>();
        inputEventArgs.InputType = inputType;

        return inputEventArgs;
    }

    /// <summary>
    /// 清理加载配置成功事件。
    /// </summary>
    public override void Clear()
    {
        OffsetX = 0f;
        OffsetY = 0f;
    }
}
