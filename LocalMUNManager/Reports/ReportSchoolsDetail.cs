using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalMUNManager.model;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Charting;
using PdfSharp.Fonts;
//using PdfSharp.Forms;
using PdfSharp.Drawing;
using IMUNModel;

namespace LocalMUNManager.Reports
{
    public class ReportSchoolsDetail
    {
        //School school;

        public ReportSchoolsDetail(School school)
        {
            try
            {
                //this.school = school;
                //PdfDocument doc = new PdfDocument();
                //doc.Info.Title = "Order Number " + sale.idSale;
                //PdfPage page = doc.AddPage();

                //// Get an XGraphics object for drawing 
                //XGraphics gfx = XGraphics.FromPdfPage(page);

                //// Create a font 
                //XFont fontTitle = new XFont("Verdana", 9, XFontStyle.Regular);
                //XFont fontDetail = new XFont("Lucida Sans Typewriter", 9, XFontStyle.Regular);
                //XFont fontBold = new XFont("Lucida Sans Typewriter", 9, XFontStyle.Bold);

                //int line = 1;
                //int lMargin = 20;

                //String s = "";

                //String[] lines = MyDatabase.GetDb().Settings.FirstOrDefault(x => x.idSettings.Equals(MyDatabase.SETTING_HEADER)).Value.Replace("\n", "").Split('\r');

                //foreach (String hLine in lines)
                //{
                //    gfx.DrawString(hLine, fontTitle, XBrushes.Black,
                //        new XRect(lMargin, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);
                //}

                //s = "Order Number: " + sale.idSale;
                //gfx.DrawString(s, fontTitle, XBrushes.Black,
                //    new XRect(lMargin + 450, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);
                //s = "Customer: " + sale.CustomerName;
                //gfx.DrawString(s, fontTitle, XBrushes.Black,
                //    new XRect(lMargin, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);
                //s = "Address: " + sale.Addresss;
                //gfx.DrawString(s, fontTitle, XBrushes.Black,
                //    new XRect(lMargin, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);
                //s = "NIF: " + (sale.HasNIF ? (sale.SameAsFees ? " (Same as Fees)" : sale.NIF) : "User requested that no NIF is associated");
                //gfx.DrawString(s, fontTitle, XBrushes.Black,
                //    new XRect(lMargin, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);
                //s = "Email: " + (String.IsNullOrEmpty(sale.Email) ? "-" : sale.Email);
                //gfx.DrawString(s, fontTitle, XBrushes.Black,
                //    new XRect(lMargin, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);
                //s = "Date: " + sale.Date;
                //gfx.DrawString(s, fontTitle, XBrushes.Black,
                //    new XRect(lMargin, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);

                //line++;
                //int startDetailX = lMargin + 10;
                //int startDetailY = line * 20 - 5;

                //s = String.Format("{0,-62} {1,4} {2,10}  VAT {3,10}", "Product", "Quant", "Unit Price", "Total");
                //gfx.DrawString(s, fontDetail, XBrushes.Black,
                //    new XRect(lMargin + 10, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);

                //foreach (SaleDetail sd in sale.SaleDetails)
                //{
                //    String p1 = String.Format("{0:C2}", sd.ProductUnitPrice).Replace(" ", "");
                //    String p2 = String.Format("{0:C2}", sd.Total).Replace(" ", "");
                //    s = String.Format("{0,-63} {1,4} {2,10} {3,3}% {4,10}", sd.Product.Description.Substring(0, Math.Min(sd.Product.Description.Length, 63)), sd.Quantity, p1, sd.ProductVat, p2);
                //    gfx.DrawString(s, fontDetail, XBrushes.Black,
                //        new XRect(lMargin + 10, line++ * 20, page.Width, page.Height), XStringFormats.TopLeft);
                //}

                //String tot = String.Format("{0:C2}", sale.Total).Replace(" ", "");
                //s = String.Format("{0} {1,10}", (new String(' ', 78)) + "Total:", tot);


                //gfx.DrawString(s, fontBold, XBrushes.Black,
                //    new XRect(lMargin + 10, line++ * 20 + 2, page.Width, page.Height), XStringFormats.TopLeft);

                //DrawRectangle(gfx, new Rectangle(lMargin, startDetailY, 535, (line * 20) - startDetailY));
                //DrawLine(gfx, new Rectangle(lMargin, startDetailY + 22, 535, 0));
                //DrawLine(gfx, new Rectangle(lMargin, (line * 20) - 22, 535, 0));

                //string folder = Properties.Settings.Default.localPDFFolder;
                //if (!Directory.Exists(folder))
                //{
                //    MessageBox.Show("Folder does not exist. Please check settings window.");
                //    return;
                //}
                //string filename = folder + "/" + "Order" + sale.idSale + ".pdf";
                //doc.Save(filename);

                //sendMessage("CAISL Order printed " + sale.idSale, filename);

                //// ...and start a viewer. 
                //Process.Start(filename);
            }
            catch (System.Net.Mail.SmtpException)
            {
             //   MessageBox.Show("Could not send files.");
            }
            catch (Exception)
            {

              //  MessageBox.Show("Could not create PDF file. Please check if another PDF file is already opened.");
            }
        }


    }
}
