@echo off

rem python .\gen_reader.py
rem .\PyTools\gen_reader.exe .\ExcelTable\ ..\BiuBiu\Assets\GameScript\Runtime\Data\DataTable\TableReaderInst.cs
python .\PyTools\py_lib\gen_reader.py .\ExcelTable\ ..\BiuBiu\Assets\GameScript\Runtime\Data\DataTable\TableReaderInst.cs

echo 生成完成

