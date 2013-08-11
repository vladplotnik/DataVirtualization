using System.Threading.Tasks;

namespace DataVirtualization.ViewModel
{
    public static class Schedulers
    {
        public static TaskScheduler UIThread { get; set; }
    }
}
