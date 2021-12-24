# coding=utf-8

import os
import shutil

strbuf = ""
index = 0

for root, dirs, files in os.walk("./out/lua/GameConfig"):
    for file in files:
        if os.path.splitext(file)[1] == ".lua":
            print("find file " + file)
            strbuf = ""
            with open(os.path.join(root, file)) as fp:
                for line in fp.readlines():
                    index = line.find("= require('flatbuffers")
                    if index > -1:
                        strbuf += line[:(index + 11)] + \
                            "3rd." + line[(index + 11):]
                    else:
                        strbuf += line
                fp.close()

            with open(os.path.join(root, file), "w") as fp:
                fp.write(strbuf)
                fp.close()

            if os.path.exists(os.path.join(root, file + ".txt")):
                os.remove(os.path.join(root, file + ".txt"))

            os.rename(os.path.join(root, file),
                      os.path.join(root, file + ".txt"))
