// Dmitrovskis Martynas IFIN-2/2
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

/*--------------------------------------------------------------------------------------------
    U1-2. Krepšinis.
Krepšinio mokykloje treniruotes lankančių sąrašas yra tekstiniame faile: būsimo krepšininko 
vardas ir pavardė, amžius ir ūgis. Pirmoje eilutėje yra krepšinio mokyklos pavadinimas. Turime 
dviejų mokyklų duomenis.

Skaičiavimai:
    Raskite, koks būsimų krepšininkų amžiaus vidurkis ir koks ūgio vidurkis kiekvienoje 
    mokykloje.
    Surašykite į atskirą rinkinį visus abiejų mokyklų sportininkus, kurių ūgis didesnis už 
    vidurkį.
    Surikiuokite rezultatų sąrašą amžiaus didėjimo ir vardus abecėlės tvarka.
    Pašalinkite iš rezultatų sąrašo krepšininkus, kurių amžius yra didesnis už nurodytą 
    klaviatūra.

Reikalavimai programai:
    Grafinė vartotojo sąsaja(GVS);
    Duomenų klasė;
    Sąrašo elemento (mazgo) klasė ir konteinerinė sąrašo klasė (klasėje gali būti tik 
vienas sąrašas);
    Sąsajos IEnumerable realizavimas.
--------------------------------------------------------------------------------------------*/

namespace U1_2._Krepšinis_L4
{
    public partial class Form1 : Form
    {
        string[] CFd;
        string CFr;

        PlayersContainer playerCont1;
        PlayersContainer playerCont2;
        PlayersContainer newPlayerCont = new PlayersContainer();

        string age, school1, school2;

        public Form1()
        {
            InitializeComponent();

            setAgeToolStripMenuItem.Enabled = true;
            readDataToolStripMenuItem.Enabled = false;
            printDataToolStripMenuItem.Enabled = false;
            findAverageToolStripMenuItem.Enabled = false;
            formatToolStripMenuItem.Enabled = false;
            sortToolStripMenuItem.Enabled = false;
            removeToolStripMenuItem.Enabled = false;
        }

        //=========================================================================
        // GRAPHIC USER INTERFACE METHODS
        //=========================================================================

        /// <summary>
        /// Actions of the "Įvesti amžių" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setAgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ageLabel.Text = "Amžius";
            age = ageBox.Text;

            if (Convert.ToInt32(age) >= 0)
            {
                setAgeToolStripMenuItem.Enabled = false;
                readDataToolStripMenuItem.Enabled = true;

                notificationLabel.Text = "Amžius įvestas!";
            }
            else
            {
                ageBox.Clear();
                notificationLabel.Text = "Klaidingai įvestas amžius, bandykite iš naujo!";
                ageLabel.Text = "Amžius";
                age = ageBox.Text;
                setAgeToolStripMenuItem.Enabled = false;
                setAgeToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// Actions of the "Įvesti pradinius duomenis" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "Pasirinkite duomenų failus";
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                CFd = openFileDialog1.FileNames;
                playerCont1 = ReadPlayerForwards(CFd[0], ref school1);
                playerCont2 = ReadPlayerBackwards(CFd[1], ref school2);
            }

            readDataToolStripMenuItem.Enabled = false;
            printDataToolStripMenuItem.Enabled = true;
            findAverageToolStripMenuItem.Enabled = true;
            formatToolStripMenuItem.Enabled = true;
            sortToolStripMenuItem.Enabled = true;
            removeToolStripMenuItem.Enabled = true;

            notificationLabel.Text = "Pradiniai duomenys nuskaityti iš failų\n "
                + CFd[0] + "\n" + CFd[1];
        }

        /// <summary>
        /// Actions of the "Spausdinti" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Title = "Pasirinkite rezultatų failą";
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                CFr = saveFileDialog1.FileName;

                if (File.Exists(CFr))
                    File.Delete(CFr);

                PrintPlayerArray(CFr, playerCont1, school1);
                PrintPlayerArray(CFr, playerCont2, school2);
            }

            string resultFile = File.ReadAllText(CFr);
            results.Text = resultFile;
            printDataToolStripMenuItem.Enabled = false;

            notificationLabel.Text = "Pradiniai duomenys faile\n" + CFr;
        }

        /// <summary>
        /// Actions of the "Baigti" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Actions of the "Rasti vidurkius" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findAverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintAverage(CFr, playerCont1, school1 + " žaidėjų vidutinis " +
                    "amžius ir ūgis:");

            PrintAverage(CFr, playerCont2, school2 + " žaidėjų vidutinis " +
                "amžius ir ūgis:");

            string resultFile = File.ReadAllText(CFr);
            results.Text = resultFile;
            findAverageToolStripMenuItem.Enabled = false;

            notificationLabel.Text = "Vidurkiai apskaičiuoti!";
        }

        /// <summary>
        /// Actions of the "Formatuoti" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Format(playerCont1, newPlayerCont);
            Format(playerCont2, newPlayerCont);

            PrintPlayerArray(CFr, newPlayerCont, "Naujai suformuotas " +
                        "žaidėjų sąrašas");

            string resultFile = File.ReadAllText(CFr);
            results.Text = resultFile;
            formatToolStripMenuItem.Enabled = false;

            notificationLabel.Text = "Naujas sąrašas suformuotas!";
        }

        /// <summary>
        /// Actions of the "Rikiuoti" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newPlayerCont.BubbleSort();
            PrintPlayerArray(CFr, newPlayerCont, "Surikiuotas " +
                        "žaidėjų sąrašas");

            string resultFile = File.ReadAllText(CFr);
            results.Text = resultFile;
            sortToolStripMenuItem.Enabled = false;

            notificationLabel.Text = "Žaidėjai surikiuoti!";
        }

        /// <summary>
        /// Actions of the "Šalinti" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove(newPlayerCont, age);

            newPlayerCont.Start();

            if (newPlayerCont.Exists() == true)
            {
                PrintPlayerArray(CFr, newPlayerCont, "Galutinis " +
                        "žaidėjų sąrašas");
            }
            else
            {
                using (StreamWriter fr = new StreamWriter(File.Open(CFr, FileMode.Append)))
                {
                    fr.WriteLine("Nėra duomenų!");
                }
            }

            string resultFile = File.ReadAllText(CFr);
            results.Text = resultFile;
            removeToolStripMenuItem.Enabled = false;

            notificationLabel.Text = "Netinkami žaidėjai pašalinti iš sąrašo!";
        }

        /// <summary>
        /// Actions of the "Informacija" menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Programos versija: 17.5.3\n" +
                            "Programos sukūrimo data: 2023 balandis\n" +
                            "Programos autorius: MD",
                            "Informacija apie programą");
        }

        //===================================================================
        // USER METHODS
        //===================================================================

        /// <summary>
        /// Reads players data from front from a file to an array
        /// </summary>
        /// <param name="fn">file name</param>
        /// <returns>the reference array of players data</returns>
        static PlayersContainer ReadPlayerForwards(string fn, ref string school)
        {
            var A = new PlayersContainer();
            using (var reader = new StreamReader(fn))
            {
                string line;
                school = line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    string nameSurname = parts[0];
                    int age = int.Parse(parts[1]);
                    double height = double.Parse(parts[2]);
                    Player play = new Player(nameSurname, age, height);
                    A.AddForwards(play);
                }
            }
            return A;
        }

        /// <summary>
        /// Reads players data from back from a file to an array
        /// </summary>
        /// <param name="fn">file name</param>
        /// <returns>the reference array of players data</returns>
        static PlayersContainer ReadPlayerBackwards(string fn, ref string school)
        {
            var A = new PlayersContainer();
            using (var reader = new StreamReader(fn))
            {
                string line;
                school = line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    string nameSurname = parts[0];
                    int age = int.Parse(parts[1]);
                    double height = double.Parse(parts[2]);
                    Player play = new Player(nameSurname, age, height);
                    A.AddBackwards(play);
                }
            }
            return A;
        }

        /// <summary>
        /// Prints player data on file
        /// </summary>
        /// <param name="fn">file name</param>
        /// <param name="player">array of players data</param>
        /// <param name="comment">a comment</param>
        static void PrintPlayerArray(string fn, PlayersContainer player, string comment)
        {
            const string header =
                        "--------------------------------------\r\n"
                      + " Nr. Pavardė ir vardas   Amžius  Ūgis \r\n"
                      + "--------------------------------------";
            using (StreamWriter fr = new StreamWriter(File.Open(fn, FileMode.Append)))
            {
                fr.WriteLine("\n " + comment);
                fr.WriteLine(header);

                int i = 0;
                for (player.Start(); player.Exists(); player.Next())
                {
                    fr.WriteLine("{0, 3} {1}", i + 1, player.GetPlayer().ToString());
                    i++;
                }
                fr.WriteLine("--------------------------------------");
            }
        }

        /// <summary>
        /// Finds the average age of all team members
        /// </summary>
        /// <param name="player">array of players data</param>
        /// <returns>average age</returns>
        static double AverageAge(PlayersContainer player)
        {
            int sum = 0;
            int count = 0;
            for (player.Start(); player.Exists(); player.Next())
            {
                sum += player.GetPlayer().Age;
                count++;
            }
            if (count != 0)
                return (double)sum / count;
            else
                return 0.0;
        }

        /// <summary>
        /// Finds the average height of all team members
        /// </summary>
        /// <param name="player">array of players data</param>
        /// <returns>average height</returns>
        static double AverageHeight(PlayersContainer player)
        {
            double sum = 0;
            int count = 0;
            for (player.Start(); player.Exists(); player.Next())
            {
                sum += player.GetPlayer().Height;
                count++;
            }
            if (count != 0)
                return (double)sum / count;
            else
                return 0.0;
        }

        /// <summary>
        /// Prints answers about average age and height
        /// </summary>
        /// <param name="fn">file name</param>
        /// <param name="PlayContainer">players container</param>
        /// <param name="comment">a comment</param>
        static void PrintAverage(string fn,
            PlayersContainer PlayContainer, string comment)
        {
            using (StreamWriter fr = new StreamWriter(File.Open(fn, FileMode.Append)))
            {
                fr.WriteLine(comment);
                fr.WriteLine("{0, 2:f0} metai, {1, 3:f2} metrai",
                AverageAge(PlayContainer), AverageHeight(PlayContainer));
            }
        }

        /// <summary>
        /// Formats a new array from two other arrays
        /// </summary>
        /// <param name="playContainer">players container</param>
        /// <param name="newContainer">new players container</param>
        static void Format(PlayersContainer playContainer, PlayersContainer newContainer) 
        {
            double average = AverageHeight(playContainer);
            foreach (Player play in playContainer)
            {
                if (play.Height > average)
                {
                    newContainer.AddForwards(play);
                }
            }
        }

        /// <summary>
        /// Removes players who are older than the provided age
        /// </summary>
        /// <param name="finalContainer">players container</param>
        /// <param name="age">priveded age</param>
        static void Remove(PlayersContainer finalContainer, string age) 
        {
            int playerAge = Convert.ToInt32(age);

            for (finalContainer.Start(); finalContainer.Exists(); finalContainer.Next())
            {
                if (finalContainer.GetPlayer().Age > playerAge)
                {
                    finalContainer.RemovePlayers(finalContainer.GetPlayer());
                }
            }
        }

        // End USER METHODS
    }
}
