// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace GameConfig
{

using global::System;
using global::FlatBuffers;

public struct Entity : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Entity GetRootAsEntity(ByteBuffer _bb) { return GetRootAsEntity(_bb, new Entity()); }
  public static Entity GetRootAsEntity(ByteBuffer _bb, Entity obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Entity __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public uint Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public string AssetName { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetAssetNameBytes() { return __p.__vector_as_span(6); }
#else
  public ArraySegment<byte>? GetAssetNameBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetAssetNameArray() { return __p.__vector_as_array<byte>(6); }

  public static Offset<Entity> CreateEntity(FlatBufferBuilder builder,
      uint Id = 0,
      StringOffset AssetNameOffset = default(StringOffset)) {
    builder.StartObject(2);
    Entity.AddAssetName(builder, AssetNameOffset);
    Entity.AddId(builder, Id);
    return Entity.EndEntity(builder);
  }

  public static void StartEntity(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddId(FlatBufferBuilder builder, uint Id) { builder.AddUint(0, Id, 0); }
  public static void AddAssetName(FlatBufferBuilder builder, StringOffset AssetNameOffset) { builder.AddOffset(1, AssetNameOffset.Value, 0); }
  public static Offset<Entity> EndEntity(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Entity>(o);
  }
};

public struct EntityList : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static EntityList GetRootAsEntityList(ByteBuffer _bb) { return GetRootAsEntityList(_bb, new EntityList()); }
  public static EntityList GetRootAsEntityList(ByteBuffer _bb, EntityList obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public EntityList __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Entity? Data(int j) { int o = __p.__offset(4); return o != 0 ? (Entity?)(new Entity()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int DataLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<EntityList> CreateEntityList(FlatBufferBuilder builder,
      VectorOffset dataOffset = default(VectorOffset)) {
    builder.StartObject(1);
    EntityList.AddData(builder, dataOffset);
    return EntityList.EndEntityList(builder);
  }

  public static void StartEntityList(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddData(FlatBufferBuilder builder, VectorOffset dataOffset) { builder.AddOffset(0, dataOffset.Value, 0); }
  public static VectorOffset CreateDataVector(FlatBufferBuilder builder, Offset<Entity>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateDataVectorBlock(FlatBufferBuilder builder, Offset<Entity>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartDataVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<EntityList> EndEntityList(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<EntityList>(o);
  }
};


}
