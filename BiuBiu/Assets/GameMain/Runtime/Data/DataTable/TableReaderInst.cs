using FlatBuffers;
using GameConfig;


public class EntityTableReader : TableReader<Entity, EntityList>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/Entity.bytes";   
    protected override Entity? GetData(EntityList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(EntityList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(Entity data)
    {
        return data.Id;
    }
    protected override EntityList GetTableDataList(ByteBuffer byteBuffer)
    {
        return EntityList.GetRootAsEntityList(byteBuffer);
    }
}

public class SceneTableReader : TableReader<Scene, SceneList>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/Scene.bytes";   
    protected override Scene? GetData(SceneList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(SceneList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(Scene data)
    {
        return data.Id;
    }
    protected override SceneList GetTableDataList(ByteBuffer byteBuffer)
    {
        return SceneList.GetRootAsSceneList(byteBuffer);
    }
}

public class SoundTableReader : TableReader<Sound, SoundList>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/Sound.bytes";   
    protected override Sound? GetData(SoundList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(SoundList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(Sound data)
    {
        return data.Id;
    }
    protected override SoundList GetTableDataList(ByteBuffer byteBuffer)
    {
        return SoundList.GetRootAsSoundList(byteBuffer);
    }
}

public class UIWindowTableReader : TableReader<UIWindow, UIWindowList>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/UIWindow.bytes";   
    protected override UIWindow? GetData(UIWindowList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(UIWindowList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(UIWindow data)
    {
        return data.Id;
    }
    protected override UIWindowList GetTableDataList(ByteBuffer byteBuffer)
    {
        return UIWindowList.GetRootAsUIWindowList(byteBuffer);
    }
}
