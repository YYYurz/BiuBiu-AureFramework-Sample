# coding=utf-8

import os

file1 = 'gen_excel.cmd'
file2 = 'CSharp读表类生成.cmd'
file3 = 'Cp2Folder.py'


# if os.system('start ' + file1) == 0:
# 	print("完成1")
# 	if os.system('start ' + file2) == 0:
# 		print("完成2")
# 		if os.system('python ' + file3) == 0:
# 			print("完成3")

os.system("gen_excel.cmd")
os.system("CSharpGenClass.cmd")
os.system("GenLuaTable.py")
os.system("Cp2Folder.py")
