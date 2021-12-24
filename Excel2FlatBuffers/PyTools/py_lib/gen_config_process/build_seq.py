from . import build_struct

class BuildSeq:

    def __init__(self):
        self._func_list = list()
        self._sub_seqs = list()
        self._val = None

    def store_func(self, func, args=None):
        if len(self._sub_seqs):
            self._sub_seqs[-1].store_func(func, args)
        else:
            self._func_list.append([func, args])

    def begin_sub_seq(self):
        cur_seq = self.get_cur_seq()
        seq = BuildSeq()
        cur_seq._func_list.append(seq)
        cur_seq._sub_seqs.append(seq)

    def end_sub_seq(self):
        if len(self._sub_seqs[-1].get_sub_seqs()):
            return self._sub_seqs[-1].end_sub_seq()
        return self._sub_seqs.pop()

    def get_cur_seq(self):
        if len(self._sub_seqs):
            return self._sub_seqs[-1].get_cur_seq()
        return self

    def process(self):
        ret = None
        for v in self._func_list:
            if isinstance(v, list):
                continue
            ret = v.process()
        for v in self._func_list:
            if isinstance(v, BuildSeq):
                continue
            if len(v[1]) > 1:
                for i in range(len(v[1])):
                    if isinstance(v[1][i], BuildSeq):
                        v[1][i] = v[1][i].get_val()
                for i in range(len(v[1])):
                    if isinstance(v[1][i], build_struct.BuildStruct):
                        v[1][i] = v[1][i].process()
                if v[1][-1] is not None:
                    ret = v[0](*(v[1][1:]))
            else:
                ret = v[0]()
        self._val = ret
        return ret

    def get_sub_seqs(self):
        return self._sub_seqs

    def get_val(self):
        return self._val
