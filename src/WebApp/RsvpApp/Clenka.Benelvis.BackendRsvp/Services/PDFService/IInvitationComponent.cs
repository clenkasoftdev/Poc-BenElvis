using QuestPDF.Infrastructure;
namespace Clenka.Benelvis.BackendRsvp.Services.PDFService
{
    public interface IInvitationComponent : IComponent
    {
        void Compose(IContainer container);
    }
}
