// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using IronPdf;
// using System.IO;
// using PdfSharpCore.Pdf;
// using TheArtOfDev.HtmlRenderer.PdfSharp;

// namespace AbsenDulu.BE.Controllers.Document;
// [ApiController]
// public class GenerateController : ControllerBase
// {
//     [HttpPost]
//     [Authorize]
//     [Route("[controller]/export")]
//     public IActionResult ExportPdf()
//     {
//         try
//         {
//             var html = @"
//         <h1>HI..! Welcome to the PDF Tutorial!</h1>
//         <p> This is 1st Page </p>
//         <div style='page-break-after: always;'></div>
//         <h2> This is 2nd Page after page break!</h2>
//         <div style='page-break-after: always;'></div>
//         <p> This is 3rd Page</p>
//         <div style='page-break-after: always;'></div>
//         <link href='https://fonts.googleapis.com/css?family=Libre Barcode 128' rel='stylesheet'>
//         <p style='font-family: 'Libre Barcode 128', serif; font-size:30px;'> Hello Google Fonts</p>";

//             var data = new PdfDocument();
//             PdfGenerator.
//         }
//         catch (Exception exception)
//         {
//             return BadRequest(exception.Message);
//         }
//     }

// }
