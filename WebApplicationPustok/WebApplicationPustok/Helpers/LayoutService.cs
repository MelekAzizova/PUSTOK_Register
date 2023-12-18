using NuGet.Configuration;
using WebApplicationPustok.Context;
using WebApplicationPustok.Models;

namespace WebApplicationPustok.Helpers
{
    public class LayoutService
    {
        PustokDbContext _context { get; }

        public LayoutService(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<Setting> GetSettingsAsync()
            => await _context.Settings.FindAsync(1);
    }
}
