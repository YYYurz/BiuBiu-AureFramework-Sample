import os
import subprocess
import sys
import shutil
from contextlib import contextmanager


def run_shell(cmd: str, cwd=None, wait=False, output=False, print_output=False):
    if print_output:
        print("[run_shell] {}".format(cmd))
        print("[in dir] {}".format(cwd))
    if output or print_output:
        p = subprocess.Popen(cmd, shell=True, cwd=cwd, stdout=subprocess.PIPE,
                             stderr=subprocess.STDOUT)
    else:
        p = subprocess.Popen(cmd, shell=True, cwd=cwd)
    if wait:
        import sys
        temp, _ = p.communicate()
        if print_output:
            try:
                sys.stdout.write(temp.decode(sys.stdout.encoding))
            except UnicodeDecodeError as e:
                print(e)
        if p.returncode:
            if output:
                raise subprocess.CalledProcessError(returncode=p.returncode, cmd=cmd, output=temp)
            else:
                raise subprocess.CalledProcessError(returncode=p.returncode, cmd=cmd)
        if output:
            return p.returncode, temp
        return p.returncode
    return 0


def run_shell_async(cmd: str, cwd=None, output=False):
    print("[run_shell_async] {}".format(cmd))
    print("[in dir] {}".format(cwd))
    if output:
        p = subprocess.Popen(cmd, shell=True, cwd=cwd, stdout=subprocess.PIPE,
                             stderr=subprocess.STDOUT)
    else:
        p = subprocess.Popen(cmd, shell=True, cwd=cwd)
    return p


def wait_subprocess(ps: [subprocess.Popen]):
    import sys
    while len(ps):
        p = ps[len(ps) - 1]
        return_code = p.poll()
        if return_code is None:
            continue
        if return_code == 0:
            ps.pop()
            continue
        if p.stdout == subprocess.PIPE:
            try:
                sys.stdout.write(p.stdout.decode(sys.stdout.encoding))
            except UnicodeDecodeError as e:
                print(e)
        sys.exit(return_code)


def system(cmd: str, cwd=None):
    if cwd is not None:
        with pushd(cwd):
            os.system(cmd)
    else:
        os.system(cmd)


@contextmanager
def pushd(new_dir: str):
    previous_dir = os.getcwd()
    os.chdir(new_dir)
    yield
    os.chdir(previous_dir)


def get_current_path():
    if getattr(sys, 'frozen', None):   #??????sys???frozen??????
        ret = os.path.realpath(os.path.dirname(sys.executable))  #????????????????????????????????????
    else:
        ret = os.path.realpath(os.path.dirname(__file__))   #?????????????????????????????????
    return ret


def clean_dir(to_clean_dir, ignores=None):
    if ignores is None:
        ignores = list()
    import glob
    lists = os.walk(to_clean_dir)   #??????to_clean_dir???????????????????????????lists???
    ignores = list(map(lambda x:
                       list(map(lambda y:
                                y.replace("\\", "/"),
                                glob.glob(x, recursive=True))),
                       ignores))     #??????????????????ignores?????????
    for root, dirs, files in lists:
        for f in files:
            path = os.path.join(root, f)    #??????????????????????????????
            # tmp = getSubDir(DIR, path)
            finded = False
            for v in ignores:
                # l = list(map(lambda a: a.replace("\\", "/"), glob.glob(v, recursive=True)))
                # print("---------")
                # print(path, v)
                # for g in l:
                #     print(g)
                if path.replace("\\", "/") in v:    #??????????????????????????????ignores?????????
                    finded = True                   #finded???true?????????  break
                    break
            if finded:
                continue 
            os.remove(path)                         #?????????????????????ignore????????????   os.remove??????????????????
        for d in dirs:
            path = os.path.join(root, d)   #??????????????????????????????
            # tmp = getSubDir(DIR, path)
            finded = False
            for v in ignores:
                # l = list(map(lambda a: a.replace("\\", "/"), glob.glob(v, recursive=True)))
                # print("---------")
                # print(path, v)
                # for g in l:
                #     print(g)
                if path.replace("\\", "/") in v:
                    finded = True
                    break
            if finded:
                continue
            shutil.rmtree(path)      #????????????????????????


def mk_out_dir(out_dir, cleanup_first, ignores=None):   
    if ignores is None:          #ignores????????????
        ignores = list()
    if os.path.exists(out_dir):  #out_dir??????????????????
        if cleanup_first:
            clean_dir(out_dir, ignores)  #cleanup_first???true????????????  ??????clean_dir??????out_dir????????????ignores????????????
    else:
        os.makedirs(out_dir)    #???out_dir?????????????????????????????????


def dos2unix_cmd(dos2unix, in_file_path):
    import platform
    if platform.system().startswith("CYGWIN"):
        in_file_path = "$(cygpath -w {})".format(in_file_path)
    cmd: str = "{} -k {}".format(dos2unix, in_file_path)
    return cmd


def os_is_win32():
    return sys.platform == 'win32'


def os_is_32bit_windows():
    if not os_is_win32():
        return False

    arch = os.environ['PROCESSOR_ARCHITECTURE'].lower()
    arch_w = "PROCESSOR_ARCHITEW6432" in os.environ
    return arch == "x86" and not arch_w


def os_is_mac():
    return sys.platform == 'darwin'


def os_is_linux():
    return 'linux' in sys.platform


def version_compare(a: str, op: str, b: str):
    """Compares two version numbers to see if a op b is true

    op is operator
    op can be ">", "<", "==", "!=", ">=", "<="
    a and b are version numbers (dot separated)
    a and b can be string, float or int

    Please note that: 3 == 3.0 == 3.0.0 ... ("==" is not a simple string cmp)
    """
    allowed = [">", "<", "==", "!=", ">=", "<="]
    if op not in allowed:
        raise ValueError("op must be one of {}".format(allowed))

    # Use recursion to simplify operators:
    if op[0] == "<":  # Reverse args and inequality sign:
        return version_compare(b, op.replace("<", ">"), a)
    if op == ">=":
        return version_compare(a, "==", b) or version_compare(a, ">", b)
    if op == "!=":
        return not version_compare(a, "==", b)

    # We now have 1 of 2 base cases, "==" or ">":
    assert op in ["==", ">"]

    a = [int(x) for x in str(a).split(".")]
    b = [int(x) for x in str(b).split(".")]

    for i in range(max(len(a), len(b))):
        ai, bi = 0, 0  # digits
        if len(a) > i:
            ai = a[i]
        if len(b) > i:
            bi = b[i]
        if ai > bi:
            if op == ">":
                return True
            else:  # op "=="
                return False
        if ai < bi:
            # Both "==" and ">" are False:
            return False
    if op == ">":
        return False  # op ">" and all digits were equal
    return True  # op "==" and all digits were equal
