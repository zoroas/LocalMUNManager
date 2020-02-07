using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using IMUNModel;

namespace LocalMUNManager.model
{
    public class PowerPointCreator
    {
        private static bool hasError = true;
        private static string error = "";

        public static bool HasError()
        {
            return hasError;
        }

        public static string GetError()
        {
            return error;
        }

        public static void InsertImageInSlide(PowerPoint.Slide slide, PowerPoint.Shape shape, string imagePath)
        {
            float left = shape.Left;
            float top = shape.Top;
            float width = shape.Width;
            float height = shape.Height;
            slide.Shapes.AddPicture(imagePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, left, top, width, height);
        }

        /// <param name="cards">The cards that are going to be output</param>
        /// <param name="templateName">Template file name. Just the filename, not full path.</param>
        /// <param name="pptOutput">The output powerpoint file.</param>
        public static void CreateBadges(Card[] cards, string templateName, string pptOutput)
        {
            PowerPoint.Application oPowerPoint = null;
            PowerPoint.Presentations oPres = null;
            PowerPoint.Presentation oPre = null;
            PowerPoint.Slides oSlides = null;
            PowerPoint.Slide oSlide = null;
            PowerPoint.Shapes oShapes = null;
            PowerPoint.Shape oShape = null;
            PowerPoint.TextFrame oTxtFrame = null;
            PowerPoint.TextRange oTxtRange = null;

            String serverRoot = Properties.Settings.Default.ServerRootPath;
            string pptTemplate = serverRoot +  @"\schools\templates\" + templateName;

            try
            {
                //int numberOfColumns = int.Parse(numberOfColumnsStr);

                // Create the PowerPoint
                oPowerPoint = new PowerPoint.Application();
                oPres = oPowerPoint.Presentations;
                File.Copy(pptTemplate, pptOutput);

                oPre = oPres.Open(pptOutput);
                oSlides = oPre.Slides;

                // Iterate over lines
                int i = 0;
                foreach (Card card in cards)
                {
                    i++;

                    String position = card.Forum;
                    position = position.Replace("Delegate", "");
                    position = position.Trim();

                    Dictionary<string, string> values = new Dictionary<string, string> {
                        { ":delegation:", String.IsNullOrEmpty(card.Country)?" ":card.Country },
                        { ":firstname:",  String.IsNullOrEmpty(card.FirstName)?" ":card.FirstName.ToUpper() },
                        { ":lastname:", String.IsNullOrEmpty(card.LastName)?" ":card.LastName.ToUpper() },
                        { ":lastnameinitial:", String.IsNullOrEmpty(card.LastNameInitial)?" ":card.LastNameInitial.ToUpper()+"." },
                        { ":forum:", String.IsNullOrEmpty(position)?" ": position },
                    };

                    DirectoryInfo dir = new DirectoryInfo (serverRoot + @"\schools\" + card.School);

                    oSlide = oSlides[1];
                    oSlide.Duplicate();
                    oSlide = oSlides[2];

                    String pictureFile = dir.FullName + @"\" + card.PictureInternalName;
                   // String pictureFile = (new Uri(photoFolder)).AbsolutePath;

                    // place a photo
                    for (int ishape = 1; ishape <= oSlide.Shapes.Count; ishape++)
                    {
                        PowerPoint.Shape shape = oSlide.Shapes[ishape];
                        if (shape.HasTextFrame == Microsoft.Office.Core.MsoTriState.msoTrue)
                        {
                            bool fileExists = File.Exists(pictureFile);
                            Console.WriteLine(fileExists);
                            String shapeLabel = shape.TextFrame.TextRange.Text;
                            if (shapeLabel.ToLower() == ":photo:" && File.Exists(pictureFile))
                            {
                                float left = shape.Left;
                                float top = shape.Top;
                                float width = shape.Width;
                                float height = shape.Height;
                                oSlide.Shapes.AddPicture(pictureFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, left, top, width, height);
                                shape.Delete();
                            }
                            else
                            {
                                foreach (string key in values.Keys)
                                {
                                    Console.Write(key + " " + values[key]);
                                    String oldValue = shape.TextFrame.TextRange.Text;
                                    int first = oldValue.ToLower().IndexOf(key);
                                    if (first != -1)
                                    {
                                        String newValue = oldValue.Substring(0, first != 0 ? first : 0) +
                                                          values[key] +
                                                          oldValue.Substring(first + key.Length);
                                        if (oldValue != newValue)
                                        {
                                            shape.TextFrame.TextRange.Text = newValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                oPowerPoint.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                // Clean up the unmanaged PowerPoint COM resources by explicitly 
                // calling Marshal.FinalReleaseComObject on all accessor objects. 
                // See http://support.microsoft.com/kb/317109.

                if (oTxtRange != null)
                {
                    Marshal.FinalReleaseComObject(oTxtRange);
                    oTxtRange = null;
                }
                if (oTxtFrame != null)
                {
                    Marshal.FinalReleaseComObject(oTxtFrame);
                    oTxtFrame = null;
                }
                if (oShape != null)
                {
                    Marshal.FinalReleaseComObject(oShape);
                    oShape = null;
                }
                if (oShapes != null)
                {
                    Marshal.FinalReleaseComObject(oShapes);
                    oShapes = null;
                }
                if (oSlide != null)
                {
                    Marshal.FinalReleaseComObject(oSlide);
                    oSlide = null;
                }
                if (oSlides != null)
                {
                    Marshal.FinalReleaseComObject(oSlides);
                    oSlides = null;
                }
                if (oPre != null)
                {
                    Marshal.FinalReleaseComObject(oPre);
                    oPre = null;
                }
                if (oPres != null)
                {
                    Marshal.FinalReleaseComObject(oPres);
                    oPres = null;
                }
                if (oPowerPoint != null)
                {
                    Marshal.FinalReleaseComObject(oPowerPoint);
                    oPowerPoint = null;
                }
            }
        }

        /// <param name="cards">The cards that are going to be output</param>
        /// <param name="templateName">Template file name. Just the filename, not full path.</param>
        /// <param name="pptOutput">The output powerpoint file.</param>
        public static void CreateCertificates(Card[] cards, string templateName, string pptOutput)
        {
            PowerPoint.Application oPowerPoint = null;
            PowerPoint.Presentations oPres = null;
            PowerPoint.Presentation oPre = null;
            PowerPoint.Slides oSlides = null;
            PowerPoint.Slide oSlide = null;
            PowerPoint.Shapes oShapes = null;
            PowerPoint.Shape oShape = null;
            PowerPoint.TextFrame oTxtFrame = null;
            PowerPoint.TextRange oTxtRange = null;

            String serverRoot = Properties.Settings.Default.ServerRootPath;
            string pptTemplate = serverRoot + @"\schools\templates\" + templateName;

            try
            {
                //int numberOfColumns = int.Parse(numberOfColumnsStr);

                // Create the PowerPoint
                oPowerPoint = new PowerPoint.Application();
                oPres = oPowerPoint.Presentations;
                File.Copy(pptTemplate, pptOutput);

                oPre = oPres.Open(pptOutput);
                oSlides = oPre.Slides;

                // Iterate over lines
                int i = 0;
                foreach (Card card in cards)
                {
                    i++;

                    String line1 = "";
                    String line2 = "";
                    if (card.IsCertificateDelegate)
                    {
                        line1 = "as a delegate of ";
                        line2 = card.Country;
                    }
                    else if (card.IsAdmin)
                    {
                        line1 = "as an";
                        line2 = "Admin Staff";
                    }
                    else if (card.IsICJAdvocate)
                    {
                        line1 = "as an advocate in the";
                        line2 = "International Court of Justice";
                    }
                    else if (card.IsOfficer)
                    {
                        line1 = "as";
                        line2 = card.Forum;
                    }
                    else if (card.IsICJJudge)
                    {
                        line1 = "as a judge in the";
                        line2 = "International Court of Justice";
                    }
                    //else if (card.IsChaperone)
                    //{
                    //    line1 = "as a";
                    //    line2 = "Chaperone";
                    //}
                    else if (card.IsPress)
                    {
                        line1 = "as";
                        line2 = "Press";
                    }
                    //else if (card.IsDirector)
                    //{
                    //    line1 = "as";
                    //    line2 = "Director";
                    //}

                    Dictionary<string, string> values = new Dictionary<string, string> {
                        { ":line1:", String.IsNullOrEmpty(line1)?" ":line1 },
                        { ":line2:", String.IsNullOrEmpty(line2)?" ":line2 },
                        { ":firstname:",  String.IsNullOrEmpty(card.FirstName)?" ":card.FirstName.ToUpper() },
                        { ":lastname:", String.IsNullOrEmpty(card.LastName)?" ":card.LastName.ToUpper() }
                    };

                    DirectoryInfo dir = new DirectoryInfo(serverRoot + @"\schools\" + card.School);

                    oSlide = oSlides[1];
                    oSlide.Duplicate();
                    oSlide = oSlides[2];

                    String pictureFile = dir.FullName + @"\" + card.PictureInternalName;
                    // String pictureFile = (new Uri(photoFolder)).AbsolutePath;

                    // place a photo
                    for (int ishape = 1; ishape <= oSlide.Shapes.Count; ishape++)
                    {
                        PowerPoint.Shape shape = oSlide.Shapes[ishape];
                        if (shape.HasTextFrame == Microsoft.Office.Core.MsoTriState.msoTrue)
                        {
                            bool fileExists = File.Exists(pictureFile);
                            Console.WriteLine(fileExists);
                            String shapeLabel = shape.TextFrame.TextRange.Text;
                            if (shapeLabel.ToLower() == ":photo:" && File.Exists(pictureFile))
                            {
                                float left = shape.Left;
                                float top = shape.Top;
                                float width = shape.Width;
                                float height = shape.Height;
                                oSlide.Shapes.AddPicture(pictureFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, left, top, width, height);
                                shape.Delete();
                            }
                            else
                            {
                                foreach (string key in values.Keys)
                                {
                                    Console.Write(key + " " + values[key]);
                                    String oldValue = shape.TextFrame.TextRange.Text;
                                    int first = oldValue.ToLower().IndexOf(key);
                                    if (first != -1)
                                    {
                                        String newValue = oldValue.Substring(0, first != 0 ? first : 0) +
                                                          values[key] +
                                                          oldValue.Substring(first + key.Length);
                                        if (oldValue != newValue)
                                        {
                                            shape.TextFrame.TextRange.Text = newValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                oPowerPoint.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                hasError = false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                // Clean up the unmanaged PowerPoint COM resources by explicitly 
                // calling Marshal.FinalReleaseComObject on all accessor objects. 
                // See http://support.microsoft.com/kb/317109.

                if (oTxtRange != null)
                {
                    Marshal.FinalReleaseComObject(oTxtRange);
                    oTxtRange = null;
                }
                if (oTxtFrame != null)
                {
                    Marshal.FinalReleaseComObject(oTxtFrame);
                    oTxtFrame = null;
                }
                if (oShape != null)
                {
                    Marshal.FinalReleaseComObject(oShape);
                    oShape = null;
                }
                if (oShapes != null)
                {
                    Marshal.FinalReleaseComObject(oShapes);
                    oShapes = null;
                }
                if (oSlide != null)
                {
                    Marshal.FinalReleaseComObject(oSlide);
                    oSlide = null;
                }
                if (oSlides != null)
                {
                    Marshal.FinalReleaseComObject(oSlides);
                    oSlides = null;
                }
                if (oPre != null)
                {
                    Marshal.FinalReleaseComObject(oPre);
                    oPre = null;
                }
                if (oPres != null)
                {
                    Marshal.FinalReleaseComObject(oPres);
                    oPres = null;
                }
                if (oPowerPoint != null)
                {
                    Marshal.FinalReleaseComObject(oPowerPoint);
                    oPowerPoint = null;
                }
            }
        }
    }
}
