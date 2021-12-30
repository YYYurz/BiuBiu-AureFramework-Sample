// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace GameConfig
{

using global::System;
using global::FlatBuffers;

public struct Scene : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Scene GetRootAsScene(ByteBuffer _bb) { return GetRootAsScene(_bb, new Scene()); }
  public static Scene GetRootAsScene(ByteBuffer _bb, Scene obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Scene __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public uint Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public string AssetName { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetAssetNameBytes() { return __p.__vector_as_span(6); }
#else
  public ArraySegment<byte>? GetAssetNameBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetAssetNameArray() { return __p.__vector_as_array<byte>(6); }

  public static Offset<Scene> CreateScene(FlatBufferBuilder builder,
      uint Id = 0,
      StringOffset AssetNameOffset = default(StringOffset)) {
    builder.StartObject(2);
    Scene.AddAssetName(builder, AssetNameOffset);
    Scene.AddId(builder, Id);
    return Scene.EndScene(builder);
  }

  public static void StartScene(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddId(FlatBufferBuilder builder, uint Id) { builder.AddUint(0, Id, 0); }
  public static void AddAssetName(FlatBufferBuilder builder, StringOffset AssetNameOffset) { builder.AddOffset(1, AssetNameOffset.Value, 0); }
  public static Offset<Scene> EndScene(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Scene>(o);
  }
};

public struct SceneList : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static SceneList GetRootAsSceneList(ByteBuffer _bb) { return GetRootAsSceneList(_bb, new SceneList()); }
  public static SceneList GetRootAsSceneList(ByteBuffer _bb, SceneList obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public SceneList __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Scene? Data(int j) { int o = __p.__offset(4); return o != 0 ? (Scene?)(new Scene()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int DataLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<SceneList> CreateSceneList(FlatBufferBuilder builder,
      VectorOffset dataOffset = default(VectorOffset)) {
    builder.StartObject(1);
    SceneList.AddData(builder, dataOffset);
    return SceneList.EndSceneList(builder);
  }

  public static void StartSceneList(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddData(FlatBufferBuilder builder, VectorOffset dataOffset) { builder.AddOffset(0, dataOffset.Value, 0); }
  public static VectorOffset CreateDataVector(FlatBufferBuilder builder, Offset<Scene>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateDataVectorBlock(FlatBufferBuilder builder, Offset<Scene>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartDataVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<SceneList> EndSceneList(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<SceneList>(o);
  }
};


}