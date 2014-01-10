using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVirtualization.Northwind;
using VirtualCollection.VirtualCollection;

namespace DataVirtualization.ViewModel
{
    public class NorthwindOrderSource : VirtualCollectionSource<Order>
    {
        private string _search;

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                Refresh(RefreshMode.ClearStaleData);
            }
        }

        protected override Task<IList<Order>> GetPageAsyncOverride(int start, int pageSize, IList<SortDescription> sortDescriptions)
        {
            return GetQueryResults(start, pageSize, sortDescriptions)
                .ContinueWith(t =>
                    {
                        SetCount((int)t.Result.TotalCount);
                        return (IList<Order>)(t.Result).ToList();

                    }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private Task<QueryOperationResponse<Order>> GetQueryResults(int start, int pageSize, IList<SortDescription> sortDescriptions)
        {
            var context = new NorthwindEntities(new Uri("http://localhost/WebApp/Northwind.svc/"));

            var orderByString = CreateOrderByString(sortDescriptions);
            var query = context.Orders
                .AddQueryOption("$skip", start)
                .AddQueryOption("$top", pageSize)
                .IncludeTotalCount();

            if (!string.IsNullOrEmpty(Search))
            {
                query = query.AddQueryOption("$filter", "(substringof('" + Search + "',CustomerID) eq true)");
            }

            if (orderByString.Length > 0)
            {
                query = query.AddQueryOption("$orderby", orderByString);
            }

            return Task.Factory.FromAsync<IEnumerable<Order>>(query.BeginExecute, query.EndExecute, null)
                .ContinueWith(t => (QueryOperationResponse<Order>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
        }

        private string CreateOrderByString(IList<SortDescription> sortDescriptions)
        {
            var sb = new StringBuilder();

            if (sortDescriptions != null)
            {
                foreach (var sortDescription in sortDescriptions)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(sortDescription.PropertyName + " " +
                              (sortDescription.Direction == ListSortDirection.Ascending ? "asc" : "desc"));
                }
            }

            return sb.ToString();
        }
    }
}
