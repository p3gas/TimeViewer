using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeViewer1.Service
{
    public interface ITimeProviderService
    {
        public Task<DateTime> GetTime(string location);
        public Task<List<string>> GetLocationNames();
    }
}
