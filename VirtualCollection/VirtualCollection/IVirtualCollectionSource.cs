﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace VirtualCollection.VirtualCollection
{
    public interface IVirtualCollectionSource<T>
    {
        event EventHandler<VirtualCollectionSourceChangedEventArgs> CollectionChanged;
        event EventHandler<EventArgs> CountChanged;

        int? Count { get; }
        void Refresh(RefreshMode mode);
        Task<IList<T>> GetPageAsync(int start, int pageSize, IList<SortDescription> sortDescriptions);
    }

    public class VirtualCollectionSourceChangedEventArgs : EventArgs
    {
        public ChangeType ChangeType { get; private set; }

        public VirtualCollectionSourceChangedEventArgs(ChangeType changeType)
        {
            ChangeType = changeType;
        }
    }

    public enum ChangeType
    {
        /// <summary>
        /// Current data is invalid and should be cleared
        /// </summary>
        Reset,
        /// <summary>
        /// Current data may still be valid, and can be shown whilst refreshing
        /// </summary>
        Refresh,
    }
}
