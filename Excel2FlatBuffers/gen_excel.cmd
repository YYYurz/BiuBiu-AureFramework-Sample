@echo off

python .\PyTools\tools.py gen_config ^
    --config-dir ExcelTable ^
    --out-cs-dir out\cs ^
    --out-lua-dir out\lua ^
    --out-bytes-dir out\bytes 

echo 导表完成

