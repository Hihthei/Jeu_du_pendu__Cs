using System;
using System.Runtime.ExceptionServices;

using AsciiArt;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if(lettres.Contains(lettre))
                    Console.Write(lettre + " ");
                else
                    Console.Write("_ ");
            }
            Console.WriteLine();
        }

        static bool MotTrouvees(string mot, List<char> lettres)
        {
            foreach(var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }

            if (mot.Length == 0)
            {
                return true;
            }

            return false;

        }

        static char DemanderUneLettre()
        {
            while (true)
            {
                Console.Write("Rentrer une lettre : ");
                string reponse = Console.ReadLine();

                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("ERREUR : Vous devez rentrer une lettre.");
            }
        }

        static void DevinerMot(string mot)
        {
            char lettre;
            List<char> lettreTrouvees = new List<char>();

            List<char> lettreFausses = new List<char>();


            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (true)
            {
                Console.WriteLine(Ascii.PENDU[ NB_VIES - viesRestantes ]);
                Console.WriteLine();

                Console.WriteLine("Il vous restes " + viesRestantes + " vies.");
                AfficherMot(mot, lettreTrouvees);
                Console.WriteLine();
                lettre = DemanderUneLettre();
                Console.Clear();

                if (lettreTrouvees.Contains(lettre) || lettreFausses.Contains(lettre))
                {
                    Console.WriteLine("Vous avez déjà rentré cette lettre !");
                }
                else if (mot.Contains(lettre))
                {

                    Console.WriteLine("Cette lettre est dans le mot.");
                    lettreTrouvees.Add(lettre);
                    if (MotTrouvees(mot, lettreTrouvees))
                    {
                        Console.WriteLine("Félicitations ! Vous avez trouvez le mot \" " + mot + " \" !");
                        return;
                    }
                }
                else if (viesRestantes == 1)
                {
                    Console.WriteLine(Ascii.PENDU[NB_VIES]);
                    Console.WriteLine();
                    Console.WriteLine("Vous avez perdu !");
                    Console.WriteLine("Le mot était : " + mot);
                    return;
                }
                else
                {
                    Console.WriteLine("Cette lettre n'est pas dans le mot.");
                    lettreFausses.Add(lettre);
                    viesRestantes--;
                }
                if (lettreFausses.Count != 0)
                {
                    Console.WriteLine("Le mot ne contient pas les lettres suivantes : " + String.Join(", ", lettreFausses));
                }
                Console.WriteLine();
            }
        }



        static string[] ChargerMot(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERREUR : Impossible de le lire le fichier \"" + nomFichier + "\"");
                Console.WriteLine("Code d'erreur : "+ ex.Message);
                Console.WriteLine();
            }

            return null;
        }


        static bool Rejouer()
        {
            Console.Write("\nVoulez-vous rejouer ? (o/n) : ");
            var tmp = Console.ReadLine();

            if (tmp.Length != 1)
            {
                Console.Clear();
                Console.WriteLine("ERREUR : Vous devez rentrer une réponse valide");
                return Rejouer();
            }
            else if (tmp.Equals( "o" )) // /!\ Equals compare uniquement des chaînes de caractère !! Pas possible d'utiliser tmp.Equals( 'o' )
            {
                return true;
            }
            else if (tmp.Equals( "n" ))
            {
                return false;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("ERREUR : Vous devez rentrer une réponse valide");
                return Rejouer();
            }
        }

        static void Main(string[] args)
        {
            var mots = ChargerMot("mots.txt");

            if ((mots == null) || (mots.Length == 0))
            {
                Console.WriteLine();
                Console.WriteLine("La liste de mots est vide.");
            }
            else
            {
                bool rejouer = true;
                while (rejouer)
                {
                    Console.Clear();
                    Random r = new Random();
                    int i = r.Next(mots.Length);
                    string mot = mots[i].Trim().ToUpper(); //Trim -> enlève les espaces avant et après les chaines de caractères : " Village " => "Village"
                    DevinerMot(mot);

                    rejouer = Rejouer();
                }

                Console.Clear();
                Console.WriteLine("Merci d'avoir joué !\n");
            }
        }
    }
}