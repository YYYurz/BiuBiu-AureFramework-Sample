// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace GameConfig
{

using global::System;
using global::FlatBuffers;

public struct FP : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public FP __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public long Raw { get { return __p.bb.GetLong(__p.bb_pos + 0); } }

  public static Offset<FP> CreateFP(FlatBufferBuilder builder, long Raw) {
    builder.Prep(8, 8);
    builder.PutLong(Raw);
    return new Offset<FP>(builder.Offset);
  }
};


}