using Clenka.Benelvis.BackendRsvp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Clenka.Benelvis.BackendRsvp.Services.PDFService
{
    public class ContentComponent : IInvitationComponent
    {
        public InvitationModel Model { get; set; }

        public ContentComponent(InvitationModel model)
        {
            Model = model;
        }
        public void Compose(IContainer container)
        {

                container.Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).AlignCenter().PaddingBottom(5).Text("Bernice Weds Elvis").Bold();
                    column.Item().AlignCenter().PaddingBottom(5).Text(Model.InvitationTitle).SemiBold();
                    column.Item().Text($"");
                    column.Item().Text($"Dear {Model.Title} {Model.LastName}");
                    column.Item().Text($"");
                    column.Item().Text($"{Model.InvitationText}");
                    column.Item().Text($"");
                    column.Item().Text($"Be our guest");
                    column.Item().Text($"Bernice & Elvis");
                    column.Item().Text($"");
                    column.Item().AlignCenter().PaddingBottom(5).Text($"Your assigned Table");
                    column.Item().AlignCenter().PaddingBottom(5).Text($"#{Model.Seat}").SemiBold();

                });
            
            
        }
    }
}
