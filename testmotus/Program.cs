using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        
        int score = 0;
        string highName="";
        int highScore = 0;
        string lireScore = "";

        if (File.Exists("high_score.txt"))
        {
            lireScore = File.ReadAllLines("high_score.txt")[0];
            string[] tableauScore = lireScore.Split(';');
            highScore = int.Parse(tableauScore[1]);
            highName = tableauScore[0];
        }        
        bool recommencer = true;

        Console.WriteLine("_ _ _ _ _BIENVENUE SUR MOTUS_ _ _ _ _"); // Titre
        Console.ForegroundColor = ConsoleColor.Green;               // Legende    
        Console.WriteLine("LETTRE BIEN PLACEE");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("LETTRE MAL PLACEE");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("LETTRE ABSENTE");
        Console.ResetColor();
        Console.WriteLine();
        if (File.Exists("high_score.txt")) Console.WriteLine($"Le meilleur score est détenu par {highName} avec {highScore} mot(s) trouvé(s)");
        Console.WriteLine();
        Console.Write("Entrez votre nom : ");
        string nomJoueur = Console.ReadLine().ToUpper();

        do
        {
            string[] listeDeMots = File.ReadAllLines("FR.txt");
            int tentativesMax = 6;
            int nbEssais = 0;
            Random rnd = new Random();
            int random = rnd.Next(0, listeDeMots.Length);
            string motATrouver = "AAAAAAAA"; //listeDeMots[random].ToUpper();
            string motSaisi = "";
            string resultat = "";


            while ((nbEssais < tentativesMax) && (motATrouver != motSaisi) && (recommencer))
            {


                do // boucle pour la saisie utilisateur, message d'erreur tant que != de 8 caractères
                {

                    Console.WriteLine();
                    Console.WriteLine($"**Il vous reste {tentativesMax - nbEssais} essai(s)**");
                    Console.WriteLine();
                    Console.Write($"Saisissez un mot de 8 lettres commençant par {motATrouver[0]}: ");
                   


                    motSaisi = Console.ReadLine().ToUpper();
                    if (motSaisi.Length < motATrouver.Length)
                    {
                        Console.WriteLine();
                        Console.WriteLine("----Vous avez saisi moins de 8 lettres----");
                        Console.WriteLine();
                        nbEssais++;
                    }
                    if (motSaisi.Length > motATrouver.Length)
                    {
                        Console.WriteLine();
                        Console.WriteLine("----Vous avez saisi plus de 8 lettres----");
                        Console.WriteLine();
                        nbEssais++;
                    }
                }
                while ((motSaisi.Length != motATrouver.Length) && (nbEssais < tentativesMax));

                for (int i = 0; i < motSaisi.Length; i++) // boucle sur chaque lettre du mot saisie
                {
                    bool bienPlace = false; // passe a true si la lettre est bien placée
                    bool present = false; // passe a true si la lettre est présente mais mal placée

                    for (int j = 0; j < motSaisi.Length; j++) // boucle sur chaque lettre du mot à trouver
                    {
                        if ((motSaisi[i] == motATrouver[j]) && (i == j)) bienPlace = true;

                        if (motSaisi[i] == motATrouver[j]) present = true;
                    }

                    if (bienPlace) // si bien placé, ecrit la lettre en vert
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(motSaisi[i]);
                        Console.ResetColor();
                    }

                    else if (present) // si mal placée, écrit la lettre en jaune
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(motSaisi[i]);
                        Console.ResetColor();
                    }
                    else // sinon ecrit la lettre en rouge
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(motSaisi[i]);
                        Console.ResetColor();
                    }
                }
                nbEssais++;
                resultat = motSaisi;

                if (resultat == motATrouver) //si le resultat est correct on recommence en reinitialisant les données.
                {
                    Console.WriteLine();
                    Console.WriteLine("*************************************************************");
                    Console.WriteLine($"Bravo vous avez trouvé le mot {motATrouver} en {nbEssais} essai(s)");
                    Console.WriteLine("*************************************************************");
                    score++;
                    Console.WriteLine($"Jusqu'ici, vous avez trouvé {score} mot(s)");
                    Console.WriteLine();

                    while (true)
                    {
                        Console.Write("Voulez vous continuer ? OUI / NON : ");
                        string retry = Console.ReadLine();
                        if (retry.ToUpper() == "OUI")
                        {
                            recommencer = true;
                            break;
                        }
                        if (retry.ToUpper() == "NON")
                        {
                            recommencer = false;
                            break;
                        }
                    }


                    motSaisi = "";
                    nbEssais = 0;
                    random = rnd.Next(0, listeDeMots.Length);
                    motATrouver = "AAAAAAAA"; // listeDeMots[random].ToUpper();

                }
            }


            {
                if (!recommencer)
                {
                    if (score > highScore)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"BRAVO ! avec {score} mot(s) trouvé(s), vous détenez le meilleur score");
                        lireScore = (nomJoueur + ';' + score).ToString();
                        File.WriteAllText("high_score.txt", lireScore);
                    }
                    Console.WriteLine();
                    Console.WriteLine($"Le jeu est terminé, au total vous avez trouvé {score} mot(s).");
                    Console.WriteLine();
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Le jeu est terminé, le mot était {motATrouver}, au total vous avez trouvé {score} mot(s).");
                    Console.WriteLine();

                    if ((score > highScore) || (!File.Exists("high_score.txt")))
                    {
                        Console.WriteLine();
                        Console.WriteLine($"BRAVO ! avec {score} mot(s) trouvé(s), vous détenez le meilleur score");
                        lireScore = (nomJoueur + ';' + score).ToString();
                        File.WriteAllText("high_score.txt", lireScore);
                    }
                    Console.ReadKey();
                    recommencer = false;
                }

            }
        }
        while (recommencer);


    }
}


