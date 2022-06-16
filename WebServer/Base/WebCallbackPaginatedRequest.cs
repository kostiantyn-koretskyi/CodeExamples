using System;
using System.Collections.Generic;

namespace Networking.WebServer
{
    public abstract class WebCallbackPaginatedRequest<T> : WebBasePaginatedRequest<T>, IPaginatedDataSource<T> where T : ITemplate 
    {
        private int m_requestedOffset = 0;
        private int m_offset = 0;
        private int m_size = 0;
        private int m_loadingListSize = 0;
        protected List<string> m_excludedItems = new List<string>();

        public sealed override void GetItems(int offset, int size, Action<List<T>> callback) {
            if (m_waitingRequestResult) {
                m_requestsQueue.Enqueue(new RequestData
                {
                    Offset = offset, Size = size, Callback = callback
                });
                return;
            }

            m_requestedOffset = offset;
            m_offset = offset;
            m_size = size;
            if (m_items.Count >= m_size + m_offset) {
                callback.Invoke(m_items.GetRange(m_offset, m_size));
                return;
            }

            if (HasReceivedAllItems) {
                if (m_items.Count > m_offset) {
                    callback.Invoke(m_items.GetRange(m_offset, m_items.Count - m_offset));
                }
                else {
                    callback.Invoke(new List<T>());
                }

                return;
            }

            m_itemsReceivedCallback += callback;
            m_waitingRequestResult = true;
            m_loadingListSize = m_size;
            m_offset -= m_excludedItems.Count;
            SendRequest(m_offset, m_loadingListSize);
        }

        protected override void Clear() {
            base.Clear();
            m_excludedItems.Clear();
            m_requestedOffset = 0;
            m_offset = 0;
        }

        protected abstract void SendRequest(int offset, int size);

        protected void HandleReceivedItems(bool responseSucceed, List<T> receivedItems) {
            if (responseSucceed) {
                ReceivedData = true;
                var receivedItemsCount = receivedItems.Count;
                if (receivedItemsCount == 0 || m_size + m_offset == 0 || receivedItemsCount % m_loadingListSize > 0) {
                    HasReceivedAllItems = true;
                }

                m_items.AddRange(receivedItems);
            }

            HandleCallback();
            m_waitingRequestResult = false;
            HandleNextRequest();
        }

        private void HandleCallback() {
            if (m_size + m_offset == 0) {
                SendRequestResult(m_items);
            }
            else if (m_items.Count >= m_size + m_requestedOffset) {
                SendRequestResult(m_items.GetRange(m_requestedOffset, m_size));
            }
            else if (HasReceivedAllItems && m_items.Count > m_requestedOffset) {
                SendRequestResult(m_items.GetRange(m_offset, m_items.Count - m_requestedOffset));
            }
            else {
                SendRequestResult(new List<T>());
            }
        }
    }
}