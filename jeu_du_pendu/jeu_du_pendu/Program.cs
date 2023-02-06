using System;
using System.Runtime.ExceptionServices;

using AsciiArt;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        ///@Brief : permet d'afficher un mot
        ///@param mot : le mot aléatoire qu'il faut retrouver
        ///@param lettres : list de letres que nous avons déjà trouvé
        ///@exit : affichage dans le terminal du mot selon si on a les lettres ou pas
        static void AfficherMot(string mot, List<char> lettres)
        {            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if(lettres.Contains(lettre))//Si la lettre entrée par l'utilisateur (et stockée dans lettres) est dans le mot
                    Console.Write(lettre + " ");//Afficher la lettre
                else
                    Console.Write("_ ");
            }
            Console.WriteLine();
        }


        ///@Brief : permet de savoir si on a trouvé le mot ou pas
        ///@param mot : le mot aléatoire qu'il faut retrouver
        ///@param lettres : list de letres que nous avons déjà trouvé
        ///@exit : vraie si on a trouvé le mot, faux si il reste des lettres à trouver
        static bool MotTrouvees(string mot, List<char> lettres)
        {
            foreach(var lettre in lettres)//Pour chaque lettre trouvée dans le mot
            {
                mot = mot.Replace(lettre.ToString(), "");//On les remplace par "", -> on les enlève du mot
            }

            if (mot.Length == 0)//Quand la taille du mot == 0, il n'y a plus de lettre à trouver, on a trouvé le mot
            {
                return true;
            }

            return false;

        }


        ///@Brief : permet de demander une lettre à l'utilisateur
        ///@exit : renvoie la lettre que l'utilisateur a entrée
        static char DemanderUneLettre()
        {
            while (true)
            {
                Console.Write("Rentrer une lettre : ");
                string reponse = Console.ReadLine();

                if (reponse.Length == 1)//On vérifie que l'utilisateur à envoyé une seule lettre
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("ERREUR : Vous devez rentrer une lettre.");
            }
        }


        ///@Brief : boucle pour répéter les fonctions pour trouver le mot
        ///@param mot : le mot aléatoire qu'il faut retrouver
        ///@exit : affichage dans le terminal si le mot est trouvé ou non
        static void DevinerMot(string mot)
        {
            char lettre;
            List<char> lettreTrouvees = new List<char>();

            List<char> lettreFausses = new List<char>();


            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (true)
            {
                Console.WriteLine(Ascii.PENDU[ NB_VIES - viesRestantes ]);//On affiche le pendu
                Console.WriteLine();

                Console.WriteLine("Il vous restes " + viesRestantes + " vies.");//Le nombre de vies restantes
                AfficherMot(mot, lettreTrouvees);
                Console.WriteLine();
                lettre = DemanderUneLettre();
                Console.Clear();

                if (lettreTrouvees.Contains(lettre) || lettreFausses.Contains(lettre))//On vérifie que c'est la première utilisations de la lettre
                {
                    Console.WriteLine("Vous avez déjà rentré cette lettre !");
                }
                else if (mot.Contains(lettre))//On vérifie que la lettre est contenue dans le mot ou pas
                {

                    Console.WriteLine("Cette lettre est dans le mot.");
                    lettreTrouvees.Add(lettre);
                    if (MotTrouvees(mot, lettreTrouvees))
                    {
                        Console.WriteLine("Félicitations ! Vous avez trouvez le mot \" " + mot + " \" !");
                        return;
                    }
                }
                else if (viesRestantes == 1)//Si c'était notre dernière vie, alors on est mourru
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
                if (lettreFausses.Count != 0)//On vérifie le nombre de lettre fausse, pour savoir si on doit les afficher ou non
                {
                    Console.WriteLine("Le mot ne contient pas les lettres suivantes : " + String.Join(", ", lettreFausses));
                }
                Console.WriteLine();
            }
        }



        ///@Brief : Permet de charger le fichier contenant les mots
        ///@param nomFichier : le nom du fichier
        ///@exit : retourne une chaine de caractère qui contient toutes les lignes du fichier
        static string[] ChargerMot(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);//Retourne toutes les lignes du fichier "nomFichier"

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERREUR : Impossible de le lire le fichier \"" + nomFichier + "\"");
                Console.WriteLine("Code d'erreur : "+ ex.Message);//Renvoie l'exception
                Console.WriteLine();
            }

            return null;
        }


        ///@Brief : Question au joueur pour savoir s'il veut rejouer ou non
        ///@exit : renvoie vrai s'il veut rejouer, faux sinon
        static bool Rejouer()
        {
            Console.Write("\nVoulez-vous rejouer ? (o/n) : ");
            var tmp = Console.ReadLine();

            if (tmp.Length != 1)//On vérifie que l'utilisateur a bien entré une réponse correct
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
            else//Renvoie une erreur si l'utilisateur n'a pas écrit 'o' ou 'n'
            {
                Console.Clear();
                Console.WriteLine("ERREUR : Vous devez rentrer une réponse valide");
                return Rejouer();
            }
        }


        static void Main(string[] args)
        {
            var mots = ChargerMot("mots.txt");//On charge les mots du fichier

            if ((mots == null) || (mots.Length == 0))//On vérifie qu'on a réussi à ouvrir le fichier, et qu'il n'est pas vide
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
                    int i = r.Next(mots.Length);//On prend un mot aléatoire dans le fichier
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