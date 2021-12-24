import py_lib
import argparse
import types


def call_module_parse_arg(module, subparsers):
    module_name = module.__name__[len(py_lib.__name__) + 1:]
    sub_parser = subparsers.add_parser(module_name)
    sub_parser.set_defaults(which=module_name)
    if "parse_arg" not in module.__dict__.keys():
        return
    module.__dict__["parse_arg"](sub_parser)


def call_module_main(args):
    py_lib.__dict__[args.which].__dict__["main"](args)


def main():
    parser = argparse.ArgumentParser()
    subparsers = parser.add_subparsers()

    for key, module in py_lib.__dict__.items():
        # noinspection PyUnresolvedReferences
        if not isinstance(module, types.ModuleType):
            continue
        if "main" not in module.__dict__.keys():
            continue
        call_module_parse_arg(module, subparsers)

    args = parser.parse_args()
    if "which" not in args:
        import sys
        parser.print_help(sys.stderr)
        sys.exit(1)
    call_module_main(args)


if __name__ == '__main__':
    main()
