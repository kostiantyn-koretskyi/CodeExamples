using System;
using System.Collections.Generic;

namespace Networking.WebServer
{
    public abstract class WebBasePaginatedRequest<T> where T : ITemplate
    {

        public bool ReceivedData { get; protected set; }
        protected bool m_waitingRequestResult;
        protected Queue<RequestData> m_requestsQueue = new Queue<RequestData>();
        protected List<T> m_items = new List<T>();
        protected event Action<List<T>> m_itemsReceivedCallback = delegate { };

        protected WebBasePaginatedRequest() {
            HasReceivedAllItems = false;
        }

        public bool HasReceivedAllItems { get; protected set; }

        protected void SendRequestResult(List<T> items) {
            m_itemsReceivedCallback.Invoke(items);
            m_itemsReceivedCallback = delegate { };
        }

        protected void HandleNextRequest() {
            m_itemsReceivedCallback = delegate { };
            if (m_requestsQueue.Count > 0) {
                var request = m_requestsQueue.Dequeue();
                GetItems(request.Offset, request.Size, request.Callback);
            }
        }

        protected virtual void Clear() {
            m_items.Clear();
            ReceivedData = false;
            m_waitingRequestResult = false;
            HasReceivedAllItems = false;
            m_requestsQueue.Clear();
            m_itemsReceivedCallback = delegate { };
        }

        public abstract void GetItems(int offset, int size, Action<List<T>> callback);

        protected class RequestData
        {
            public int Offset;
            public int Size;
            public Action<List<T>> Callback;
        }
    }
}