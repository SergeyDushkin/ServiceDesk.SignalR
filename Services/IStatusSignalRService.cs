using System.Threading.Tasks;
using servicedesk.Common.Events;

namespace servicedesk.SignalR.Services
{
    public interface IStatusSignalRService
    {
        Task PublishStatusSetAsync(NextStatusSet @event);
        Task PublishStatusRejectedAsync(SetNewStatusRejected @event);
    }
}