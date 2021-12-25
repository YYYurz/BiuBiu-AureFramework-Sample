using FlatBuffers;
using GameConfig;


public class DTEntityTableReader : TableReader<DTEntity, DTEntityList, DTEntityTableReader>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/DTEntity.bytes";   
    protected override DTEntity? GetData(DTEntityList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(DTEntityList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(DTEntity data)
    {
        return data.Id;
    }
    protected override DTEntityList GetTableDataList(ByteBuffer byteBuffer)
    {
        return DTEntityList.GetRootAsDTEntityList(byteBuffer);
    }
}

public class DTGameConfigTableReader : TableReader<DTGameConfig, DTGameConfigList, DTGameConfigTableReader>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/DTGameConfig.bytes";   
    protected override DTGameConfig? GetData(DTGameConfigList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(DTGameConfigList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(DTGameConfig data)
    {
        return data.Id;
    }
    protected override DTGameConfigList GetTableDataList(ByteBuffer byteBuffer)
    {
        return DTGameConfigList.GetRootAsDTGameConfigList(byteBuffer);
    }
}

public class DTSceneTableReader : TableReader<DTScene, DTSceneList, DTSceneTableReader>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/DTScene.bytes";   
    protected override DTScene? GetData(DTSceneList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(DTSceneList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(DTScene data)
    {
        return data.Id;
    }
    protected override DTSceneList GetTableDataList(ByteBuffer byteBuffer)
    {
        return DTSceneList.GetRootAsDTSceneList(byteBuffer);
    }
}

public class DTSoundTableReader : TableReader<DTSound, DTSoundList, DTSoundTableReader>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/DTSound.bytes";   
    protected override DTSound? GetData(DTSoundList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(DTSoundList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(DTSound data)
    {
        return data.Id;
    }
    protected override DTSoundList GetTableDataList(ByteBuffer byteBuffer)
    {
        return DTSoundList.GetRootAsDTSoundList(byteBuffer);
    }
}

public class DTUIWindowTableReader : TableReader<DTUIWindow, DTUIWindowList, DTUIWindowTableReader>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/DTUIWindow.bytes";   
    protected override DTUIWindow? GetData(DTUIWindowList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(DTUIWindowList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(DTUIWindow data)
    {
        return data.Id;
    }
    protected override DTUIWindowList GetTableDataList(ByteBuffer byteBuffer)
    {
        return DTUIWindowList.GetRootAsDTUIWindowList(byteBuffer);
    }
}

public class DTVocationTableReader : TableReader<DTVocation, DTVocationList, DTVocationTableReader>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/DTVocation.bytes";   
    protected override DTVocation? GetData(DTVocationList dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength(DTVocationList dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey(DTVocation data)
    {
        return data.Id;
    }
    protected override DTVocationList GetTableDataList(ByteBuffer byteBuffer)
    {
        return DTVocationList.GetRootAsDTVocationList(byteBuffer);
    }
}
