import argparse
import os
import shutil

from py_lib import util


def build_dll(dir_map):
    msbuild_path = r'"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"'
    cmd_list = [msbuild_path, "{}/FightLogic.sln".format(dir_map.in_fight_dir),
                "-t:Rebuild /p:Configuration=Release /p:Platform=\"any cpu\""]
    cmd = " ".join(cmd_list)
    util.run_shell(cmd, wait=True, output=False)


def copy_dll(dir_map):
    filename = "FightLogic.dll"
    shutil.copyfile(os.path.join(dir_map.in_fight_dir, "FightLogic/bin/Release", filename),
                    os.path.join(dir_map.project_dir, "../Client/Assets/Plugins", filename))


def remove_dll(dir_map):
    filename = "FightLogic.dll"
    dll_path = os.path.join(dir_map.project_dir, "../Client/Assets/Plugins", filename)
    if os.path.exists(dll_path):
        os.remove(dll_path)
    dll_meta_path = os.path.join(dir_map.project_dir, "../Client/Assets/Plugins", "{}.meta".format(filename))
    if os.path.exists(dll_meta_path):
        os.remove(dll_meta_path)


def copy_source(dir_map):
    in_dir = os.path.join(dir_map.in_fight_dir, "FightLogic")
    out_dir = os.path.join(dir_map.project_dir, "../Client/Assets/Plugins/FightLogicDll")
    # util.mk_out_dir(out_dir, True, ["*.meta"])
    if os.path.exists(out_dir):
        shutil.rmtree(out_dir)
    shutil.copytree(in_dir, out_dir,
                    ignore=shutil.ignore_patterns("bin", "obj", "Properties", "*.csproj", "*.xml"))


def remove_source(dir_map):
    dir_name = "FightLogicDll"
    dll_path = os.path.join(dir_map.project_dir, "../Client/Assets/Plugins", dir_name)
    if os.path.exists(dll_path):
        shutil.rmtree(dll_path)
    dll_meta_path = os.path.join(dir_map.project_dir, "../Client/Assets/Plugins", "{}.meta".format(dir_name))
    if os.path.exists(dll_meta_path):
        os.remove(dll_meta_path)


def parse_arg(parser: argparse.ArgumentParser):
    parser.add_argument("-m", "--mode", dest="mode", choices=["debug", "release"],
                        default="release", help="build mode")


# noinspection PyUnusedLocal
def main(args: argparse.Namespace):
    class DirMap:
        cur_dir = None
        project_dir = None
        in_fight_dir = None

    dir_map = DirMap()
    dir_map.cur_dir = util.get_current_path()
    dir_map.project_dir = os.path.abspath(os.path.join(dir_map.cur_dir, os.path.pardir))
    dir_map.in_fight_dir = os.path.join(dir_map.project_dir, "../FightLogic")
    if args.mode == "release":
        remove_source(dir_map)
        build_dll(dir_map)
        copy_dll(dir_map)
    else:
        remove_dll(dir_map)
        copy_source(dir_map)
