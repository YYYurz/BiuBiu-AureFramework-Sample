# automatically generated by the FlatBuffers compiler, do not modify

# namespace: GameConfig

import flatbuffers

class Scene(object):
    __slots__ = ['_tab']

    @classmethod
    def GetRootAsScene(cls, buf, offset):
        n = flatbuffers.encode.Get(flatbuffers.packer.uoffset, buf, offset)
        x = Scene()
        x.Init(buf, n + offset)
        return x

    # Scene
    def Init(self, buf, pos):
        self._tab = flatbuffers.table.Table(buf, pos)

    # Scene
    def Id(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(4))
        if o != 0:
            return self._tab.Get(flatbuffers.number_types.Uint32Flags, o + self._tab.Pos)
        return 0

    # Scene
    def AssetName(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(6))
        if o != 0:
            return self._tab.String(o + self._tab.Pos)
        return None

    # Scene
    def MapConfig(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(8))
        if o != 0:
            return self._tab.String(o + self._tab.Pos)
        return None

    # Scene
    def SceneWindowId(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(10))
        if o != 0:
            return self._tab.Get(flatbuffers.number_types.Uint32Flags, o + self._tab.Pos)
        return 0

def SceneStart(builder): builder.StartObject(4)
def SceneAddId(builder, Id): builder.PrependUint32Slot(0, Id, 0)
def SceneAddAssetName(builder, AssetName): builder.PrependUOffsetTRelativeSlot(1, flatbuffers.number_types.UOffsetTFlags.py_type(AssetName), 0)
def SceneAddMapConfig(builder, MapConfig): builder.PrependUOffsetTRelativeSlot(2, flatbuffers.number_types.UOffsetTFlags.py_type(MapConfig), 0)
def SceneAddSceneWindowId(builder, SceneWindowId): builder.PrependUint32Slot(3, SceneWindowId, 0)
def SceneEnd(builder): return builder.EndObject()
