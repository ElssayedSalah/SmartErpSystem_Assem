using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PresentationLayer.Helpers
{
    public class AppHub:Hub
    {
        public void UpdateProgressBar(string message)
        {
             Clients.All.SendAsync("UpdateProgressBar", message);
        }
    }
}
