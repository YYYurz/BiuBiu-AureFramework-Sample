# coding=utf-8

import shutil
import os
import time
import sys
import getopt


class CopyFileUtil:
    @classmethod
    def CopyFileFromTo(cls, strFrom, strTo):
        if os.path.isfile(strFrom):
            shutil.copyfile(strFrom, strTo)
        else:
            for fileName in os.listdir(strFrom):
                sourceFileName = os.path.join(strFrom, fileName)
                targetFileName = os.path.join(strTo, fileName)
                # 如果源文件一个文件
                # 1.判断目标路径是否存在 不存在那么创建
                # 2.判断目标文件是否存在
                if os.path.isfile(sourceFileName):
                    if not os.path.exists(strTo):
                        os.makedirs(strTo)
                    if not os.path.exists(targetFileName):
                        shutil.copyfile(sourceFileName, targetFileName)
                    if (os.path.exists(targetFileName) and (os.path.getsize(targetFileName) != os.path.getsize(sourceFileName))):
                        shutil.copyfile(sourceFileName, targetFileName)
                if os.path.isdir(sourceFileName):
                    if os.path.exists(targetFileName):
                        shutil.rmtree(targetFileName)
                    shutil.copytree(sourceFileName, targetFileName)

    @classmethod
    def CopyTreeFromTo(cls, strFrom, strTo):
        if not os.path.exists(strFrom):
            print("can not find the from path" + strFrom)
        else:
            # if not os.path.exists(strTo):
            #    os.makedirs(strTo)
            try:
                shutil.copytree(strFrom, strTo)
            except Exception as err:
                print(err)

    @classmethod
    def CopyTreeFromToWithFileType(cls, fileTypeArr, strFrom, strTo):
        if os.path.isfile(strTo):
            print("error")
            return
        if not os.path.exists(strTo):
            os.makedirs(strTo)
        CopyFileUtil.CopyTreeFromToWithFileTypeRecursion(
            fileTypeArr, strFrom, strTo)

    @classmethod
    def CopyTreeFromToWithFileTypeRecursion(cls, fileTypeArr, strFrom, strTo):
        if os.path.isfile(strFrom):
            strTempFileType = strFrom.split(".")[-1]
            if strTempFileType in fileTypeArr:
                shutil.copyfile(strFrom, strTo)
            return
        else:
            if not os.listdir(strFrom):
                return
            for fileName in os.listdir(strFrom):
                sourceFileName = os.path.join(strFrom, fileName)
                targetFileName = os.path.join(strTo, fileName)
                if os.path.isfile(sourceFileName):
                    strTempFileType = sourceFileName.split(".")[-1]
                    if strTempFileType in fileTypeArr:
                        shutil.copyfile(sourceFileName, targetFileName)
                if os.path.isdir(sourceFileName):
                    if not os.path.exists(targetFileName):
                        os.makedirs(targetFileName)
                    CopyFileUtil.CopyTreeFromToWithFileTypeRecursion(
                        fileTypeArr, sourceFileName, targetFileName)


# ["lua"], "./out/lua/GameConfig", "../AircraftBasic/Assets/GameAssets/LuaScripts/GameConfig"
#CopyFileUtil.CopyTreeFromToWithFileType(["txt", "docx"], "H:/123", "H:/target")

if __name__ == "__main__":
    CopyFileUtil.CopyTreeFromToWithFileType(
        ["bytes"], "./out/bytes", "../BoxBoxPro/Assets/GameAssets/DataTables/bytes")

    CopyFileUtil.CopyTreeFromToWithFileType(
        ["txt"], "./out/lua/GameConfig", "../BoxBoxPro/Assets/GameAssets/LuaScripts/GameConfig")

    CopyFileUtil.CopyTreeFromToWithFileType(
        ["cs"], "./out/cs", "../BoxBoxPro/Assets/GameMain/Runtime/Data/DataTable/FlatBuffers")
