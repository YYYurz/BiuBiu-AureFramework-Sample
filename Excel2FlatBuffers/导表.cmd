@echo off

svn up out

python .\PyTools\tools.py gen_config ^
    --config-dir ExcelTable ^
    --out-proto-dir out\proto ^
    --out-cs-dir out\cs ^
    --out-lua-dir out\lua ^
    --out-cpp-dir out\cpp ^
    --out-bytes-dir out\bytes ^
    --out-csv-dir out\csv

if exist ..\arkServer (
    xcopy /y out\csv\*.csv   ..\arkServer\bin\CSVTable /s /e /h
)

svn add --force --depth infinity out
svn ci out -m "1. 上传导出配置表"

echo 导表完成
pause
