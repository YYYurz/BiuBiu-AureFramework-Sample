class BuildStruct:

    def __init__(self, func, args):
        self._func = func
        self._args = args

    def process(self):
        return self._func(*self._args)
