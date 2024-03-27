using Clenka.Benelvis.BackendRsvp.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace Clenka.Benelvis.BackendRsvp.Services.PDFService
{
    public class InvitationDocument : IInvitationDocument
    {
        public InvitationModel Model { get; set; }

        public InvitationDocument(InvitationModel model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;
        public void Compose(IDocumentContainer container)
        {
            container
           .Page(page =>
           {
               page.Margin(50);

               page.Header().Element(ComposeHeader);
               page.Content().Element(ComposeContent);


               page.Footer().AlignCenter().Text(x =>
               {
                   x.CurrentPageNumber();
                   x.Span(" / ");
                   x.TotalPages();
               });

           });
        }

        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"Table #{Model.Seat}").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span($"{Model.IssueDate:d}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Identifier: ").SemiBold();
                        text.Span($"{Model.Id}");
                    });
                });

                row.ConstantItem(100).Height(50).Column(column => {
                    column.Item().AlignCenter().PaddingTop(5).Text("Ben Weds Elvis");
                    
                });
            });
        }

      
        void ComposeRemarks(IContainer container)
        {
            if (Model.Attendance != "Alone")
            {
                container.Background(Colors.Grey.Lighten3).Padding(5).Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text("Remarks").FontSize(12);
                    column.Item().Text(Model.Remarks).FontSize(10);
                    column.Item().Text("");
                    column.Item().Text("This is a couple invitation. It is valid for you and your partner.").FontSize(9);
                    column.Item().Text("");
                    column.Item().Text($"Read more at https://www.benwedselvis.com").FontSize(9);
                    column.Item().Text($"Powered by {Model.CreatedBy}").FontSize(9);
                });
            }
            else
            {
                container.Background(Colors.Grey.Lighten3).Padding(5).Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text("Remarks").FontSize(12);
                    column.Item().Text(Model.Remarks).FontSize(10); 
                    column.Item().Text("");
                    column.Item().Text("This invitation  is valid for a single person. For you alone.").FontSize(9);
                    column.Item().Text("");
                    column.Item().Text($"Read more at https://www.benwedselvis.com").FontSize(9); 
                    column.Item().Text($"Powered by {Model.CreatedBy}").FontSize(9);
                });

            }
           
        }


        void ComposeCoupleImage(IContainer container)
        {
          // If there is an image in QRCODES folder
          // container.Image("QRCODES/qr.png").Height(100);

            container.Width(0.8f,Unit.Inch).Padding(2).Column(column =>
            {
                column.Item().AlignCenter().PaddingBottom(1).Image($"QRCODES/bernicebgbig.jpg").WithCompressionQuality(ImageCompressionQuality.Medium);
            });

        }

        void ComposeCoupleImageBig(IContainer container)
        {
            // If there is an image in QRCODES folder
            // container.Image("QRCODES/qr.png").Height(100);

            container.Width(2.5f, Unit.Inch).Padding(2).Column(column =>
            {
                column.Item().AlignCenter().PaddingBottom(1).Image($"QRCODES/bernicebgbig.jpg").WithCompressionQuality(ImageCompressionQuality.Medium);
            });

        }

        void ComposeQrImage(IContainer container)
        {
            // If there is an image in QRCODES folder
            // container.Image("QRCODES/qr.png").Height(100);

            container.Width(1.2f, Unit.Inch).Padding(2).Column(column =>
            {
                column.Item().AlignCenter().PaddingBottom(1).Image($"QRCODES/{Model.Id}.jpg").WithCompressionQuality(ImageCompressionQuality.Medium);
            });

        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new ContentComponent(Model));
                });

                column.Item().AlignCenter().PaddingTop(5).Element(ComposeQrImage);               

                if (!string.IsNullOrWhiteSpace(Model.Remarks))
                    column.Item().PaddingTop(25).Element(ComposeRemarks);

                column.Item().AlignCenter().PaddingTop(5).Element(ComposeCoupleImage);
                column.Item().AlignCenter().PaddingTop(5).Element(ComposeCoupleImageBig);
            });
        }

        QuestPDF.Infrastructure.Image LoadImageWithTransparency(string fileName, float transparency)
        {
            using var originalImage = SKImage.FromEncodedData(fileName);

            using var surface = SKSurface.Create(originalImage.Width, originalImage.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var canvas = surface.Canvas;

            using var transparencyPaint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateBlendMode(SKColors.White.WithAlpha((byte)(transparency * 255)), SKBlendMode.DstIn)
            };

            canvas.DrawImage(originalImage, new SKPoint(0, 0), transparencyPaint);

            var encodedImage = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100).ToArray();
            return Image.FromBinaryData(encodedImage);
        }

        //void ComposeTable(IContainer container)
        //{
        //    container.Table(table =>
        //    {
        //        // step 1
        //        table.ColumnsDefinition(columns =>
        //        {
        //            columns.ConstantColumn(100);
        //        });

        //        // step 2
        //        table.Header(header =>
        //        {
        //            header.Cell().Element(CellStyle).Text("Ben Weds Elvis");
        //            header.Cell().Element(CellStyle).Text("Special Invitation");
        //            static IContainer CellStyle(IContainer container)
        //            {
        //                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
        //            }
        //        });

        //        // step 3     

        //    });
        //}

    }


}
