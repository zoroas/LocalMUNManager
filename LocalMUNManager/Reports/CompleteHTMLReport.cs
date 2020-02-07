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
    public class CompleteHTMLReport
    {
        public static void GenerateHTMLReport()
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Report", // Default file name
                    DefaultExt = ".html", // Default file extension
                    Filter = "Comma Separated Values (.html, .htm)|*.html" // Filter files by extension
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

//                Card[] chaperones = allParticipants.Where(x=>x.IsChaperone.Equals(true)).ToArray<Card>();
                Card[] directors = allParticipants.Where(x => x.IsDirector.Equals(true)).ToArray<Card>();
                Card[] icjAdvocates = allParticipants.Where(x => x.IsICJAdvocate.Equals(true)).ToArray<Card>();
                Card[] icjJudges = allParticipants.Where(x => x.IsICJJudge.Equals(true)).ToArray<Card>();
                Card[] specialConfDelegates = allParticipants.Where(x => x.IsSpecialConferenceDelegate.Equals(true)).ToArray<Card>();
                Card[] secCouncilDelegates = allParticipants.Where(x => x.IsSecurityCouncilDelegate.Equals(true)).ToArray<Card>();
                Card[] genAssemblyDelegates = allParticipants.Where(x => x.IsGeneralAssemblyDelegate.Equals(true)).ToArray<Card>();
                Card[] pressMembers = allParticipants.Where(x => x.IsPress.Equals(true)).ToArray<Card>();
                Card[] officers = allParticipants.Where(x => x.IsOfficer.Equals(true)).ToArray<Card>();
                Card[] admins = allParticipants.Where(x => x.IsAdmin.Equals(true)).ToArray<Card>();

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("<html>");
                        w.WriteLine("<head>");
                        w.WriteLine("<title>Report</title>");

                        w.WriteLine("<style>");
                        w.WriteLine("");
                        w.WriteLine("table {");
                        w.WriteLine("    font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;");
                        w.WriteLine("    border-collapse: collapse;");
                        w.WriteLine("    width: 100%;");
                        w.WriteLine("    display: 'none'; }");
                        w.WriteLine("");
                        w.WriteLine("td, th {");
                        w.WriteLine("    border: 1px solid #ddd;");
                        w.WriteLine("    padding: 8px; }");
                        w.WriteLine("");
                        w.WriteLine("tr:nth-child(even){ background-color: #f2f2f2; }");
                        w.WriteLine("");
//                        w.WriteLine("tr:hover { background-color: yellow; }");
                        w.WriteLine("");
                        w.WriteLine("    th {");
                        w.WriteLine("        padding-top: 12px;");
                        w.WriteLine("        font-weight: bold;");
                        w.WriteLine("        padding-bottom: 12px;");
                        w.WriteLine("        text-align: left;");
                        w.WriteLine("        background-color: #4CAF50;");
                        w.WriteLine("        color: white;");
                        w.WriteLine("    }");
                        w.WriteLine("");
                        w.WriteLine("    button {");
                        w.WriteLine("        padding: 11px;");
                        w.WriteLine("        margin: 5px;");
                        w.WriteLine("        width: 90%;");
                        w.WriteLine("    }");
                        w.WriteLine("");
                        w.WriteLine("</style>");

                        w.WriteLine("</head>");
                        w.WriteLine("<body>");

                        w.WriteLine("<table>");
                        w.WriteLine("    <tr>");
                        w.WriteLine("        <td style = 'width: 150px;' rowspan = '2' valign = 'top'>");
                        w.WriteLine("                     <table margin-top = '0px'>");
                        w.WriteLine("                              <tr>");
                        w.WriteLine("                                  <td><button id = 'bttotals' onclick = \"showTable(t='totals')\">Totals </button></td>");
                        w.WriteLine("                                 </tr>");
                        w.WriteLine("                                 <tr>");
                        w.WriteLine("                                     <td><button id = 'btschools' onclick = \"showTable(t='schools')\">Schools </button>");
                        w.WriteLine("                                        </td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                                    <tr>");
                        w.WriteLine("                                        <td><button id = 'btschoolsanddelegations' onclick = \"showTable(t='schoolsanddelegations')\">Schools and Delegations</button></td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                                    <tr>");
                        w.WriteLine("                                        <td><button id = 'btdelegations' onclick = \"showTable(t='delegations')\">Delegations </button></td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                                    <tr>");
                        w.WriteLine("                                        <td><button id = 'btchairapplicants' onclick = \"showTable(t='chairapplicants')\">Chair Applicants </button></td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                                    <tr>");
                        w.WriteLine("                                        <td><button id = 'btopeningspeachesga' onclick = \"showTable(t = 'openingspeachesga')\">Opening Speaches General Assembly </button></td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                                    <tr>");
                        w.WriteLine("                                        <td><button id = 'btopeningspeachesconference' onclick = \"showTable(t = 'openingspeachesconference')\">Opening Speaches Special Conference </button></td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                                    <tr>");
                        w.WriteLine("                                        <td><button id = 'btschooladvisors' onclick = \"showTable(t = 'schooladvisors')\">School Advisors </button></td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                                    <tr>");
                        w.WriteLine("                                        <td><button id = 'btwarnings' onclick = \"showTable(t = 'warnings')\">Warnings </button></td>");
                        w.WriteLine("                                    </tr>");
                        w.WriteLine("                     </table>");
                        w.WriteLine("        </td>");
                        w.WriteLine("        <td>");


                        //w.WriteLine("<table style = 'width: 90%;'>");
                        //w.WriteLine("    <tr>");
                        //w.WriteLine("        <td><button id = 'bttotals' onclick = \"showTable(t='totals')\"> Totals </button></td>");
                        //w.WriteLine("        <td><button id = 'btschools' onclick = \"showTable(t='schools')\"> Schools </button></td>");
                        //w.WriteLine("        <td><button id = 'btschoolsanddelegations' onclick = \"showTable(t='schoolsanddelegations')\"> Schools and Delegations</button></td>");
                        //w.WriteLine("        <td><button id = 'btdelegations' onclick = \"showTable(t='delegations')\"> Delegations </button></td>");
                        //w.WriteLine("        <td><button id = 'btchairapplicants' onclick = \"showTable(t='chairapplicants')\"> Chair Applicants </button></td>");
                        //w.WriteLine("        <td><button id = 'btopeningspeachesga' onclick = \"showTable(t='openingspeachesga')\"> Opening Speaches General Assembly </button></td>");
                        //w.WriteLine("        <td><button id = 'btopeningspeachesconference' onclick = \"showTable(t='openingspeachesconference')\"> Opening Speaches Special Conference </button></td>");
                        //w.WriteLine("        <td><button id = 'btschooladvisors' onclick = \"showTable(t='schooladvisors')\"> School Advisors </button></td>");
                        //w.WriteLine("        <td><button id = 'btwarnings' onclick = \"showTable(t='warnings')\">Warnings</button></td>");
                        //w.WriteLine("    </tr>");
                        //w.WriteLine("</table>");

                        w.WriteLine("           <h1 id='totals' style='display: none'>Totals</h1>");
                        w.WriteLine("           <table id='tbtotals' valign='top' style='display: none'>");
                        w.WriteLine("               <tr><td>Total Participants:</td><td>" + allParticipants.ToArray().Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Schools:</td><td>" + schools.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Available Delegations:</td><td>" + Delegation.GetAllObjDelegations(ApplicationSettings.LocalRoot).Length + "</td></tr>");
                        //w.WriteLine("               <tr><td>Total Chaperones:</td><td>" + chaperones.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Directors:</td><td>" + directors.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total ICJ Judges:</td><td>" + icjJudges.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total ICJ Advocates:</td><td>" + icjAdvocates.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Special Conference Delegates:</td><td>" + specialConfDelegates.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Security Council Delegates:</td><td>" + secCouncilDelegates.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total General Assembly Delegates:</td><td>" + genAssemblyDelegates.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Press Members:</td><td>" + pressMembers.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Officers:</td><td>" + officers.Length + "</td></tr>");
                        w.WriteLine("               <tr><td>Total Admins:</td><td>" + admins.Length + "</td></tr>");
                        w.WriteLine("           </table><br>");

                        w.WriteLine("           <h1 id='schools' style='display: none'>Schools</h1>");

                        w.WriteLine("           <table id='tbschools' style='display: none'>");
                        foreach (School s in schools)
                        {
                            w.WriteLine("           <tr><td>" + s.Name + "</td></tr>");
                        }
                        w.WriteLine("           </table>");

                        w.WriteLine("           <h1 id='schoolsanddelegations' style='display: none'>Schools and Delegations</h1>");
                        w.WriteLine("           <table id='tbschoolsanddelegations' valign='top' style='display: none'>");
                        w.WriteLine("               <tr><td><button onclick='sortTable(\"tbschoolsanddelegations\",0)'>School</button></td><td><button onclick='sortTable(\"tbschoolsanddelegations\",1)'>Delegation</button></td>");
                        foreach (School s in schools)
                        {
                            Delegation[] ds = s.GetDelegationsObj(ApplicationSettings.LocalRoot);
                            foreach (Delegation d in ds)
                            {
                                try
                                {
                                    w.WriteLine("<tr><td>" + s.Name + "</td><td>" + d.Name + "</td></tr>");
                                }
                                catch(DelegationException x)
                                {
                                    Console.WriteLine(x);
                                }
                            }
                        }
                        w.WriteLine("           </table>");

                        w.WriteLine("           <h1 id='schooladvisors' style='display: none'>School Advisors</h1>");
                        w.WriteLine("           <table id='tbschooladvisors' valign='top' style='display: none'>");
                        w.WriteLine("               <tr><td><button onclick=\"sortTable('tbschooladvisors',0)\">School</button></td><td><button onclick=\"sortTable('tbschooladvisors',1)\">Name</button></td>");
                        foreach (School s in schools)
                        {
                            w.WriteLine("           <tr><td>" + s.Name + "</td><td>" + s.MUNDirector + "</td></tr>");
                        }
                        w.WriteLine("           </table>");

                        w.WriteLine("           <h1 id='delegations' style='display: none'>Delegations</h1>");
                        w.WriteLine("           <table id='tbdelegations' style='display: none'>");
                        w.WriteLine("               <tr><td><button onclick='sortTable(\"tbdelegations\",0)'>Delegate</button></td><td><button onclick='sortTable(\"tbdelegations\",1)'>School</button></td><td><button onclick='sortTable(\"tbdelegations\",2)'>Delegation</button></td><td><button onclick='sortTable(\"tbdelegations\",3)'>Forum</button></td><td><button onclick='sortTable(\"tbdelegations\",4)'>Has Opening Speach</button></td></tr>");
                        foreach (School s in schools)
                        {
                            foreach (Card c in s.GetCardsList(ApplicationSettings.LocalRoot))
                            {
                                w.WriteLine("           <tr><td>" + c.FirstName + " " + c.LastName + "</td><td>" +
                                    c.School + "</td><td>" + c.Country + "</td><td>" + c.Forum + "</td><td>" + (c.HasOpeningSpeach ? "Yes" : "No") 
                                    + "</td></tr>");
                            }
                        }
                        w.WriteLine("           </table>");

                        w.WriteLine("           <h1 id='chairapplicants' style='display: none'>Chair Applicants</h1>");
                        w.WriteLine("           <table id='tbchairapplicants' style='display: none'>");
                        w.WriteLine("               <tr><td><button onclick='sortTable(\"tbchairapplicants\",0)'>Delegation</button></td><td><button onclick='sortTable(\"tbchairapplicants\",1)'>Participant</button></td><td><button onclick='sortTable(\"tbchairapplicants\",2)'>School</button></td></tr>");
                        foreach (Card card in allParticipants.Where(x => x.IsChairman.Equals(true)))
                        {
                            w.WriteLine("           <tr><td>" + card.Country + "</td><td>" + card.Fullname + "</td><td>" + card.School + "</td></tr>");
                        }
                        w.WriteLine("           </table>");

                        w.WriteLine("           <h1 id='openingspeachesconference' style='display: none'>Opening Speaches (Special Conference)</h1>");
                        w.WriteLine("           <table id='tbopeningspeachesconference' style='display: none'>");
                        w.WriteLine("               <tr><td><button onclick='sortTable(\"tbopeningspeachesconference\",0)'>Delegation</button></td><td><button onclick='sortTable(\"tbopeningspeachesconference\",1)'>Participant</button></td><td><button onclick='sortTable(\"tbopeningspeachesconference\",2)'>School</button></td></tr>");
                        foreach (Card card in allParticipants.Where(x => x.HasOpeningSpeach.Equals(true) && x.IsSpecialConferenceDelegate).OrderBy(x => x.Country))
                        {
                            w.WriteLine("           <tr><td>" + card.Country + "</td><td>" + card.Fullname + "</td><td>" + card.School + "</td></tr>");
                        }
                        w.WriteLine("           </table>");

                        w.WriteLine("           <h1 id='openingspeachesga' style='display: none'>Opening Speaches (General Assembly)</h1>");
                        w.WriteLine("           <table id='tbopeningspeachesga' style='display: none'>");
                        w.WriteLine("               <tr><td><button onclick='sortTable(\"tbopeningspeachesga\",0)'>Delegation</button></td><td><button onclick='sortTable(\"tbopeningspeachesga\",1)'>Participant</button></td><td><button onclick='sortTable(\"tbopeningspeachesga\",2)'>School</button></td></tr>");

                        foreach (Card card in allParticipants.Where(x => x.HasOpeningSpeach.Equals(true) && x.IsGeneralAssemblyDelegate).OrderBy(x => x.Country))
                        {
                            w.WriteLine("           <tr><td>" + card.Country + "</td><td>" + card.Fullname + "</td><td>" + card.School + "</td></tr>");
                        }
                        w.WriteLine("           </table>");

                        w.WriteLine("           <h1 id='warnings' style='display: none'>Warnings - Please check the following: </h1>");
                        w.WriteLine("           <table id='tbwarnings' style='display: none'>");
                        w.WriteLine("               <tr><th>Warning</th><th>Data</th></tr>");
                        foreach (Card card in allParticipants.Where(x => x.HasOpeningSpeach.Equals(true) && x.IsSecurityCouncilDelegate).OrderBy(x => x.Country))
                        {
                            w.WriteLine("           <tr><td>Is marked to have Security Council opening speach</td><td>Delegation:" + card.Country + "  Participant: " + card.Fullname + "  School:" + card.School + "</td></tr>");
                        }

                        foreach (School s in schools)
                        {
                            Boolean found = false;
                            Card[] thisCards = allParticipants.Where(x => x.IsDirector && x.School.Equals(s.Name)).ToArray();
                            foreach (Card card in thisCards)
                            {
                                if (card.Fullname.Trim().ToUpper().Equals(s.MUNDirector.Trim().ToUpper()))
                                    found = true;
                            }
                            if (!found)
                            {
                                String participants = "";
                                foreach (String x in thisCards.Select(x => x.Fullname))
                                {
                                    participants = participants + x + "; ";
                                }
                                w.WriteLine("           <tr><td>School Director is not participant</td><td> School: " + s.Name + "  Advisor: " + s.MUNDirector + "  Participants: " + participants + "</td></tr>");
                            }
                        }

                        w.WriteLine("           </table>");

                        w.WriteLine("       </td>");
                        w.WriteLine("    </tr>");
                        w.WriteLine("</table>");

                        w.WriteLine("<script>");

                        w.WriteLine("function sortTable(t, n)");
                        w.WriteLine("{");
                        w.WriteLine("    var table, rows, i, x, min, current, temp, currentValue, columnSize;");
                        w.WriteLine("    table = document.getElementById(t);");
                        w.WriteLine("    i = 0;");
                        w.WriteLine("    rows = table.rows;");
                        w.WriteLine("    if (rows.length > 0)");
                        w.WriteLine("    {");
                        w.WriteLine("        columnSize = rows[i].getElementsByTagName('TD').length;");
                        w.WriteLine("    }");
                        w.WriteLine("    for (min = 1; min < (rows.length - 2); min++)");
                        w.WriteLine("    {");
                        w.WriteLine("        i = min;");
                        w.WriteLine("        for (current = min + 1; current < rows.length - 1; current++)");
                        w.WriteLine("        {");
                        w.WriteLine("            x = rows[i].getElementsByTagName('TD')[n].innerHTML.toLowerCase();");
                        w.WriteLine("            currentValue = rows[current].getElementsByTagName('TD')[n].innerHTML.toLowerCase();");
                        w.WriteLine("            if (x > currentValue)");
                        w.WriteLine("            {");
                        w.WriteLine("                i = current;");
                        w.WriteLine("            }");
                        w.WriteLine("        }");
                        w.WriteLine("");
                        w.WriteLine("        for (var k = 0; k < columnSize; k++)");
                        w.WriteLine("        {");
                        w.WriteLine("            temp = rows[i].getElementsByTagName('TD')[k].innerHTML;");
                        w.WriteLine("            rows[i].getElementsByTagName('TD')[k].innerHTML = rows[min].getElementsByTagName('TD')[k].innerHTML;");
                        w.WriteLine("            rows[min].getElementsByTagName('TD')[k].innerHTML = temp;");
                        w.WriteLine("        }");
                        w.WriteLine("    }");
                        w.WriteLine("}");



                        w.WriteLine("function hideTables()");
                        w.WriteLine("{");
                        w.WriteLine("    var t = document.getElementById('tbschools');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbtotals');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbschoolsanddelegations');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbdelegations');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbopeningspeachesga');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbchairapplicants');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbopeningspeachesconference');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbschooladvisors');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('tbwarnings');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("");
                        w.WriteLine("    t = document.getElementById('schools');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('totals');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('schoolsanddelegations');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('delegations');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('openingspeachesga');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('chairapplicants');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('openingspeachesconference');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('schooladvisors');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("    t = document.getElementById('warnings');");
                        w.WriteLine("    t.style.display = 'none';");
                        w.WriteLine("}");
                        w.WriteLine("");
                        w.WriteLine("function showTable(t)");
                        w.WriteLine("{");
                        w.WriteLine("    hideTables();");
                        w.WriteLine("    var x = document.getElementById('tb' + t);");
                        w.WriteLine("    x.style.display = 'block';");
                        w.WriteLine("    x = document.getElementById(t);");
                        w.WriteLine("    x.style.display = 'block';");
                        w.WriteLine("}");

                        w.WriteLine("</script>");

                        w.WriteLine("</body>");
                        w.WriteLine("</html>");
                    }
                }

                Console.WriteLine(filename);
                System.Diagnostics.Process.Start(filename);

            }
            catch (Exception)
            {
                MessageBox.Show("Error ocurred creating file. Please contact application administrator");
            }
        }
    }
}
