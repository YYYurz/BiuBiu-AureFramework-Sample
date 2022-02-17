// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace GameConfig
{

using global::System;
using global::FlatBuffers;

public struct GamePlay : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static GamePlay GetRootAsGamePlay(ByteBuffer _bb) { return GetRootAsGamePlay(_bb, new GamePlay()); }
  public static GamePlay GetRootAsGamePlay(ByteBuffer _bb, GamePlay obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public GamePlay __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public uint Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public uint SceneId { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public string MapConfig { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetMapConfigBytes() { return __p.__vector_as_span(8); }
#else
  public ArraySegment<byte>? GetMapConfigBytes() { return __p.__vector_as_arraysegment(8); }
#endif
  public byte[] GetMapConfigArray() { return __p.__vector_as_array<byte>(8); }
  public string PreloadAssets { get { int o = __p.__offset(10); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetPreloadAssetsBytes() { return __p.__vector_as_span(10); }
#else
  public ArraySegment<byte>? GetPreloadAssetsBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public byte[] GetPreloadAssetsArray() { return __p.__vector_as_array<byte>(10); }
  public string PlayerAsset { get { int o = __p.__offset(12); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetPlayerAssetBytes() { return __p.__vector_as_span(12); }
#else
  public ArraySegment<byte>? GetPlayerAssetBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public byte[] GetPlayerAssetArray() { return __p.__vector_as_array<byte>(12); }

  public static Offset<GamePlay> CreateGamePlay(FlatBufferBuilder builder,
      uint Id = 0,
      uint SceneId = 0,
      StringOffset MapConfigOffset = default(StringOffset),
      StringOffset PreloadAssetsOffset = default(StringOffset),
      StringOffset PlayerAssetOffset = default(StringOffset)) {
    builder.StartObject(5);
    GamePlay.AddPlayerAsset(builder, PlayerAssetOffset);
    GamePlay.AddPreloadAssets(builder, PreloadAssetsOffset);
    GamePlay.AddMapConfig(builder, MapConfigOffset);
    GamePlay.AddSceneId(builder, SceneId);
    GamePlay.AddId(builder, Id);
    return GamePlay.EndGamePlay(builder);
  }

  public static void StartGamePlay(FlatBufferBuilder builder) { builder.StartObject(5); }
  public static void AddId(FlatBufferBuilder builder, uint Id) { builder.AddUint(0, Id, 0); }
  public static void AddSceneId(FlatBufferBuilder builder, uint SceneId) { builder.AddUint(1, SceneId, 0); }
  public static void AddMapConfig(FlatBufferBuilder builder, StringOffset MapConfigOffset) { builder.AddOffset(2, MapConfigOffset.Value, 0); }
  public static void AddPreloadAssets(FlatBufferBuilder builder, StringOffset PreloadAssetsOffset) { builder.AddOffset(3, PreloadAssetsOffset.Value, 0); }
  public static void AddPlayerAsset(FlatBufferBuilder builder, StringOffset PlayerAssetOffset) { builder.AddOffset(4, PlayerAssetOffset.Value, 0); }
  public static Offset<GamePlay> EndGamePlay(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GamePlay>(o);
  }
};

public struct GamePlayList : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static GamePlayList GetRootAsGamePlayList(ByteBuffer _bb) { return GetRootAsGamePlayList(_bb, new GamePlayList()); }
  public static GamePlayList GetRootAsGamePlayList(ByteBuffer _bb, GamePlayList obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public GamePlayList __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public GamePlay? Data(int j) { int o = __p.__offset(4); return o != 0 ? (GamePlay?)(new GamePlay()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int DataLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<GamePlayList> CreateGamePlayList(FlatBufferBuilder builder,
      VectorOffset dataOffset = default(VectorOffset)) {
    builder.StartObject(1);
    GamePlayList.AddData(builder, dataOffset);
    return GamePlayList.EndGamePlayList(builder);
  }

  public static void StartGamePlayList(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddData(FlatBufferBuilder builder, VectorOffset dataOffset) { builder.AddOffset(0, dataOffset.Value, 0); }
  public static VectorOffset CreateDataVector(FlatBufferBuilder builder, Offset<GamePlay>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateDataVectorBlock(FlatBufferBuilder builder, Offset<GamePlay>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartDataVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<GamePlayList> EndGamePlayList(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GamePlayList>(o);
  }
};


}