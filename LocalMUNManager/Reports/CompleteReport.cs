using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IMUNModel;
using LocalMUNManager.model;
using Newtonsoft.Json;

namespace LocalMUNManager.Reports
{
    public class CompleteReport
    {
        public static void GenerateReport()
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Document", // Default file name
                    DefaultExt = ".csv", // Default file extension
                    Filter = "Comma Separated Values (.csv)|*.csv" // Filter files by extension
                };

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result != true)
                {
                    return;
                }
                // Save document
                string filename = dlg.FileName;

                School[] schools = School.GetAllSchools(ApplicationSettings.LocalRoot);
                String serverRoot = Properties.Settings.Default.ServerRootPath;
                String schoolsFilePath = serverRoot + "/schools";

                List<Card> allParticipants = new List<Card>();

                foreach (School s in schools)
                {
                    Card[] ds = s.GetCardsList(ApplicationSettings.LocalRoot);
                    foreach (Card d in ds)
                    {
                        allParticipants.Add(d);
                    }
                }

                allParticipants = allParticipants.OrderBy(x => x.FirstName).ToList();

        //        Card[] chaperones = allParticipants.Where(x=>x.IsChaperone.Equals(true)).ToArray<Card>();
                Card[] directors = allParticipants.Where(x => x.IsDirector.Equals(true)).ToArray<Card>();
                Card[] icjAdvocates = allParticipants.Where(x => x.IsICJAdvocate.Equals(true)).ToArray<Card>();
                Card[] icjJudges = allParticipants.Where(x => x.IsICJJudge.Equals(true)).ToArray<Card>();
                Card[] specialConfDelegates = allParticipants.Where(x => x.IsSpecialConferenceDelegate.Equals(true)).ToArray<Card>();
                Card[] secCouncilDelegates = allParticipants.Where(x => x.IsSecurityCouncilDelegate.Equals(true)).ToArray<Card>();
                Card[] genAssemblyDelegates = allParticipants.Where(x => x.IsGeneralAssemblyDelegate.Equals(true)).ToArray<Card>();
                Card[] pressMembers = allParticipants.Where(x => x.IsPress.Equals(true)).ToArray<Card>();
                Card[] officers = allParticipants.Where(x => x.IsOfficer.Equals(true)).ToArray<Card>();
                Card[] admins = allParticipants.Where(x => x.IsAdmin.Equals(true)).ToArray<Card>();

                String text = "Totals\r\n" + 
                    "\tTotal Participants: \t" + allParticipants.ToArray().Length +
                    "\r\n\tTotal Schools: \t" + schools.Length +
                    "\r\n\tTotal Delegations: \t" + Delegation.GetAllObjDelegations(ApplicationSettings.LocalRoot).Length +
             //       "\r\n\tTotal Chaperones: \t" + chaperones.Length +
                    "\r\n\tTotal Directors: \t" + directors.Length +
                    "\r\n\tTotal ICJ Judges: \t" + icjJudges.Length +
                    "\r\n\tTotal ICJ Advocates: \t" + icjAdvocates.Length +
                    "\r\n\tTotal Special Conference Delegates: \t" + specialConfDelegates.Length +
                    "\r\n\tTotal Security Council Delegates: \t" + secCouncilDelegates.Length +
                    "\r\n\tTotal General Assembly Delegates: \t" + genAssemblyDelegates.Length +
                    "\r\n\tTotal Press Members: \t" + pressMembers.Length +
                    "\r\n\tTotal Officers: \t" + officers.Length +
                    "\r\n\tTotal Admins: \t" + admins.Length +
                    "\r\n\r\nSchools";

                foreach (School s in schools)
                {
                    text = text + "\r\n\t" + s.Name ;
                }

                text += "\r\n\r\nSchools\tDelegations\r\n";

                foreach (School s in schools)
                {
                    text = text + s.Name + "\r\n";
                    Delegation[] ds = s.GetDelegationsObj(ApplicationSettings.LocalRoot);
                    foreach (Delegation d in ds)
                    {
                        text = text + "\t" + d.Name + "\r\n";
                    }
                }

                text += "\r\n\r\nSchools\tAdvisor\r\n";

                foreach (School s in schools)
                {
                    text = text + "\r\n" + s.Name + "\t" + s.MUNDirector;
                }
                
                text += "\r\n\r\nDelegations\r\n\tName\tSchool\tDelegation\tForum\tHas Opening Speach";

                foreach (School s in schools)
                {
                    foreach (Card c in s.GetCardsList(ApplicationSettings.LocalRoot))
                    {
                        text = text + "\r\n\t" +
                            c.FirstName + " " + c.LastName + "\t" +
                            c.School + "\t" +
                            c.Country + "\t" +
                            c.Forum + "\t" + 
                            (c.HasOpeningSpeach?"Yes":"No") + "\t";
                    }
                }

                text = text + "\r\n\r\nChair Applicants" + 
                    "\r\n\tDelegation\tParticipant\tSchool";

                foreach (Card card in allParticipants.Where(x=>x.IsChairman.Equals(true)))
                {
                    text = text + "\r\n\t"  + card.Country +
                        "\t" + card.Fullname + "\t"+ card.School;
                }

                text = text + "\r\n\r\nOpening Speaches (Special Conference)" +
                        "\r\n\tDelegation\tParticipant\tSchool";

                foreach (Card card in allParticipants.Where(x => x.HasOpeningSpeach.Equals(true) && x.IsSpecialConferenceDelegate).OrderBy(x => x.Country))
                {
                    text = text + "\r\n\t" + card.Country +
                        "\t" + card.Fullname + "\t" + card.School;
                }

                text = text + "\r\n\r\nOpening Speaches (General Assembly)" +
                        "\r\n\tDelegation\tParticipant\tSchool";

                foreach (Card card in allParticipants.Where(x => x.HasOpeningSpeach.Equals(true) && x.IsGeneralAssemblyDelegate).OrderBy(x => x.Country))
                {
                    text = text + "\r\n\t" + card.Country +
                        "\t" + card.Fullname + "\t" + card.School;
                }

                text = text + "\r\n\r\nOpening Speaches (Security Council)" +
                        "\r\n\tDelegation\tParticipant\tSchool";

                foreach (Card card in allParticipants.Where(x => x.HasOpeningSpeach.Equals(true) && x.IsSecurityCouncilDelegate).OrderBy(x=>x.Country))
                {
                    text = text + "\r\n\t" + card.Country +
                        "\t" + card.Fullname + "\t" + card.School;
                }

                System.IO.File.WriteAllText(filename, text, Encoding.Unicode);
            }
            catch (Exception)
            {
                MessageBox.Show("Error ocurred creating file. Please contact application administrator");
            }
        }
    }
}
