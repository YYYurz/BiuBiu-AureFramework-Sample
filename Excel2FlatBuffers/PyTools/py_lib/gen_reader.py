import os
import sys
from jinja2 import Template

tmp_str = '''using FlatBuffers;
using GameConfig;
{# datalist( itemName, filename ) #}
{% for member in datalist %}
public class {{ member[0] }}TableReader : TableReader<{{ member[0] }}, {{ member[0] }}List, {{ member[0] }}TableReader>
{
    public override string TablePath => "Assets/GameAssets/DataTables/bytes/{{ member[1] }}.bytes";   
    protected override {{ member[0] }}? GetData({{ member[0] }}List dataList, int i)
    {
        return dataList.Data(i);
    }
    protected override int GetDataLength({{ member[0] }}List dataList)
    {
        return dataList.DataLength;
    }
    protected override uint GetKey({{ member[0] }} data)
    {
        return data.Id;
    }
    protected override {{ member[0] }}List GetTableDataList(ByteBuffer byteBuffer)
    {
        return {{ member[0] }}List.GetRootAs{{ member[0] }}List(byteBuffer);
    }
}
{% endfor %}
'''

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: exec xslx_dir cs_dir")
        exit(-1)

    xslx_dir = sys.argv[1]#"F:\\arkSlk\\ExcelTable\\"
    target_dir = sys.argv[2]#"F:\\ProjectX\\Client\\Assets\\Code\\Table\\TableReaderInst.cs"

    list = os.listdir(xslx_dir)  #获取ExcelTable下所有文件
    tmp_list = []
    param_dict = {}
    for i in range(0, len(list)):
        index = list[i].rfind('.')
        base_name = list[i][:index]
        if base_name.startswith('~$'):
            continue

        class_name = base_name
        index = base_name.rfind('_')
        if index != -1:
            class_name =  base_name[:index]

        mem_item = (class_name, base_name)
        tmp_list.append(mem_item)
    param_dict["datalist"] = tmp_list

    tpl = Template(tmp_str)
    result = tpl.render(param_dict)

    code_file = open(target_dir, 'w')  #以写模式打开 TableReaderInst,cs
    code_file.write(result)            #将result写入 TableReaderInst,cs







