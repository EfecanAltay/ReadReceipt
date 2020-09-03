using ReadReceipt.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadReceipt.Services
{
    public interface IShareService
    {
        Task ShareAsExcell(IEnumerable<Receipt> receipts, IEnumerable<string> recipients = null);
    }
}