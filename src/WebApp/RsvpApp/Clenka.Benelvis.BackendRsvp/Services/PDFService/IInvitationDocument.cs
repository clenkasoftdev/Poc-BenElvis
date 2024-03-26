using QuestPDF.Infrastructure;

namespace Clenka.Benelvis.BackendRsvp.Services.PDFService
{
    public interface IInvitationDocument : IDocument
    {
        DocumentMetadata GetMetadata();
        DocumentSettings GetSettings();
        void Compose(IDocumentContainer container);
    }
}
