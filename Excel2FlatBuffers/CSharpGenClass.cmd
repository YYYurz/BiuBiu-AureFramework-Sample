@echo off

rem python .\gen_reader.py
rem .\PyTools\gen_reader.exe .\ExcelTable\ ..\BoxBoxPro\Assets\GameMain\Runtime\Data\DataTable\TableReaderInst.cs
python .\PyTools\py_lib\gen_reader.py .\ExcelTable\ ..\BoxBoxPro\Assets\GameMain\Runtime\Data\DataTable\TableReaderInst.cs

echo 生成完成

