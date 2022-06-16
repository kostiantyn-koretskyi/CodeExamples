using System;
using System.Collections.Generic;

namespace Networking.WebServer
{
    public interface IPaginatedDataSource<T> where T : ITemplate
    {
        void GetItems(int offset, int size, Action<List<T>> callback);
        bool HasReceivedAllItems { get; }
        bool ReceivedData { get; }
    }
}