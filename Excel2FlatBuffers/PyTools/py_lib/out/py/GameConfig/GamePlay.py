# automatically generated by the FlatBuffers compiler, do not modify

# namespace: GameConfig

import flatbuffers

class GamePlay(object):
    __slots__ = ['_tab']

    @classmethod
    def GetRootAsGamePlay(cls, buf, offset):
        n = flatbuffers.encode.Get(flatbuffers.packer.uoffset, buf, offset)
        x = GamePlay()
        x.Init(buf, n + offset)
        return x

    # GamePlay
    def Init(self, buf, pos):
        self._tab = flatbuffers.table.Table(buf, pos)

    # GamePlay
    def Id(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(4))
        if o != 0:
            return self._tab.Get(flatbuffers.number_types.Uint32Flags, o + self._tab.Pos)
        return 0

    # GamePlay
    def SceneId(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(6))
        if o != 0:
            return self._tab.Get(flatbuffers.number_types.Uint32Flags, o + self._tab.Pos)
        return 0

    # GamePlay
    def MapConfig(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(8))
        if o != 0:
            return self._tab.String(o + self._tab.Pos)
        return None

    # GamePlay
    def PreloadEntities(self, j):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(10))
        if o != 0:
            a = self._tab.Vector(o)
            return self._tab.Get(flatbuffers.number_types.Uint32Flags, a + flatbuffers.number_types.UOffsetTFlags.py_type(j * 4))
        return 0

    # GamePlay
    def PreloadEntitiesAsNumpy(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(10))
        if o != 0:
            return self._tab.GetVectorAsNumpy(flatbuffers.number_types.Uint32Flags, o)
        return 0

    # GamePlay
    def PreloadEntitiesLength(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(10))
        if o != 0:
            return self._tab.VectorLen(o)
        return 0

    # GamePlay
    def PlayerAsset(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(12))
        if o != 0:
            return self._tab.String(o + self._tab.Pos)
        return None

def GamePlayStart(builder): builder.StartObject(5)
def GamePlayAddId(builder, Id): builder.PrependUint32Slot(0, Id, 0)
def GamePlayAddSceneId(builder, SceneId): builder.PrependUint32Slot(1, SceneId, 0)
def GamePlayAddMapConfig(builder, MapConfig): builder.PrependUOffsetTRelativeSlot(2, flatbuffers.number_types.UOffsetTFlags.py_type(MapConfig), 0)
def GamePlayAddPreloadEntities(builder, PreloadEntities): builder.PrependUOffsetTRelativeSlot(3, flatbuffers.number_types.UOffsetTFlags.py_type(PreloadEntities), 0)
def GamePlayStartPreloadEntitiesVector(builder, numElems): return builder.StartVector(4, numElems, 4)
def GamePlayAddPlayerAsset(builder, PlayerAsset): builder.PrependUOffsetTRelativeSlot(4, flatbuffers.number_types.UOffsetTFlags.py_type(PlayerAsset), 0)
def GamePlayEnd(builder): return builder.EndObject()
