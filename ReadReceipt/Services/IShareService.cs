using ReadReceipt.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadReceipt.Services
{
    public interface IShareService
    {
        Task ShareAsExcell(ReceiptGroup receipts, IEnumerable<string> recipients = null);
        Task ShareAsExcell(IEnumerable<ReceiptGroup> receipts, IEnumerable<string> recipients = null);
    }
}