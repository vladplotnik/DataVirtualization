using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DataVirtualization.Northwind;
using VirtualCollection.VirtualCollection;
using VirtualCollection.Framework.Extensions;

namespace DataVirtualization.ViewModel
{
    public class MainViewModel : VirtualCollection.Framework.MVVM.ViewModel
    {
        private string _search;
        private NorthwindOrderSource _source;
        
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                RaisePropertyChanged(() => Search);
            }
        }

        public VirtualCollection<Order> Items { get; private set; }

        public MainViewModel()
        {
            _source = new NorthwindOrderSource();
            Items = new VirtualCollection<Order>(_source, pageSize: 20, cachedPages: 5);

            this.ObservePropertyChanged(() => Search)
                .Throttle(TimeSpan.FromSeconds(0.25))
                .ObserveOnDispatcher()
                .Subscribe(_ => _source.Search = Search);
        }

        protected override void OnViewLoaded()
        {
            Items.Refresh();
        }
    }
}
