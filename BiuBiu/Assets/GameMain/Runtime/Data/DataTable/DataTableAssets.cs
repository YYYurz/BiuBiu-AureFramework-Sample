using System;
using System.Collections.Generic;

namespace BiuBiu
{
    public class DataTableAssets
    {
        private readonly Dictionary<Type, ITableReader> dicDataTableReaders = new Dictionary<Type, ITableReader>();

        public T GetDataTableReader<T>()
        {
            if (dicDataTableReaders.TryGetValue(typeof(T), out var tableReader))
            {
                return (T)tableReader;
            }

            return default;
        }
    }
}
