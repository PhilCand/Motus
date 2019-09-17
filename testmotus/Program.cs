using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {

        int score = 0;
        var scoresList = new Dictionary<string, int>();

        if (File.Exists("high_scores.txt"))                          // Si le fichier de score existe on recupere le meilleur score
        {
            string[] tableauScore = File.ReadAllLines("high_scores.txt");
            foreach (string ligneScore in tableauScore)
            {
                scoresList.Add(ligneScore.Split(';')[0], int.Parse(ligneScore.Split(';')[1]));
            }

            
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
        Console.Write("Entrez votre nom : ");                       // Saisie nom du joueur
        string nomJoueur = Console.ReadLine().ToUpper();
        int bestScore = 0;
        scoresList.TryGetValue(nomJoueur, out bestScore);
        Console.WriteLine();
        Console.WriteLine($"Bonjour {nomJoueur}, votre meilleur score est de {bestScore}.");
        
        do                                                         // boucle principale tant que (recommencer = true)
        {
            string[] listeDeMots = File.ReadAllLines("FR.txt");
            int tentativesMax = 6;
            int nbEssais = 0;
            Random rnd = new Random();
            int random = rnd.Next(0, listeDeMots.Length);
            string motATrouver = listeDeMots[random].ToUpper();
            string motSaisi = "";
            string resultat = "";


            while ((nbEssais < tentativesMax) && (motATrouver != motSaisi) && (recommencer))
            {


                do // boucle pour la saisie utilisateur, message d'erreur tant que != de 8 caractères ou absent du dictionnaire
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
                    }
                    else if (motSaisi.Length > motATrouver.Length)
                    {
                        Console.WriteLine();
                        Console.WriteLine("----Vous avez saisi plus de 8 lettres----");
                    }
                    else if (!Array.Exists(listeDeMots, element => element == motSaisi))
                    {
                        Console.WriteLine();
                        Console.WriteLine("----Le mot saisi n'existe pas dans le dictionnaire----");
                    }

                }
                while ((motSaisi.Length != motATrouver.Length) || (!Array.Exists(listeDeMots, element => element == motSaisi)));

                for (int i = 0; i < motSaisi.Length; i++)           // boucle sur chaque lettre du mot saisie
                {
                    bool bienPlace = false;                         // passe a true si la lettre est bien placée
                    bool present = false;                           // passe a true si la lettre est présente mais mal placée

                    for (int j = 0; j < motSaisi.Length; j++)       // boucle sur chaque lettre du mot à trouver
                    {
                        if ((motSaisi[i] == motATrouver[j]) && (i == j)) bienPlace = true;

                        if (motSaisi[i] == motATrouver[j]) present = true;
                    }

                    if (bienPlace)                                  // si bien placé, ecrit la lettre en vert
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(motSaisi[i]);
                        Console.ResetColor();
                    }

                    else if (present)                               // si mal placée, écrit la lettre en jaune
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(motSaisi[i]);
                        Console.ResetColor();
                    }
                    else                                            // sinon ecrit la lettre en rouge
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(motSaisi[i]);
                        Console.ResetColor();
                    }
                }
                nbEssais++;
                resultat = motSaisi;

                if (resultat == motATrouver)                        //si le resultat est correct on propose de continuer.
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
                        Console.Write("Continuer ? OUI / NON : ");
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

                    motSaisi = "";                              // on réinitialise les données
                    nbEssais = 0;
                    random = rnd.Next(0, listeDeMots.Length);
                    motATrouver = listeDeMots[random].ToUpper();

                }
            }


            {
                if (!recommencer)                               // Si le joueur a gagné mais ne veut pas recommencer
                {

                    Console.WriteLine();
                    Console.WriteLine($"Le jeu est terminé, au total vous avez trouvé {score} mot(s).");
                    AjouterScore();
                    AfficherScore();
                    Console.WriteLine();
                    Console.ReadKey();
                }
                else                                             //on lui propose de recommencer s'il a raté les 6 essais
                {
                    Console.WriteLine();
                    Console.WriteLine($"Le jeu est terminé, le mot était {motATrouver}, au total vous avez trouvé {score} mot(s).");
                    AjouterScore();
                    AfficherScore();
                    Console.WriteLine();

                }
                while (true)
                {
                    Console.WriteLine();
                    Console.Write("Recommencer ? OUI / NON : ");
                    string retry = Console.ReadLine();
                    if (retry.ToUpper() == "OUI")
                    {
                        recommencer = true;
                        motSaisi = "";                              // on réinitialise les données + le score
                        nbEssais = 0;
                        score = 0;
                        random = rnd.Next(0, listeDeMots.Length);
                        motATrouver = listeDeMots[random].ToUpper();
                        break;
                    }
                    if (retry.ToUpper() == "NON")
                    {
                        Console.ReadKey();
                        recommencer = false;
                        break;
                    }
                }
            }

        }
        while (recommencer);

        void AfficherScore()
        {
            Console.WriteLine("TABLEAU DES SCORES");
            Console.WriteLine("-------------------");
            Console.WriteLine("P - Joueur - Score");

            var myList = scoresList.ToList();
            myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            for (int i = myList.Count - 1; i >= 0; i--)
            {

                Console.WriteLine($"{scoresList.Count - i} - {myList[i].Key} - {myList[i].Value}");
            }
        }

        void AjouterScore()
        {
            if ((scoresList.ContainsKey(nomJoueur)) && (score > bestScore))
            {
                scoresList.Remove(nomJoueur);
                scoresList.Add(nomJoueur, score);
            }
            
            if (!scoresList.ContainsKey(nomJoueur))
                scoresList.Add(nomJoueur, score);
            var myList = scoresList.ToList();
            myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            string[] majScores = new string[scoresList.Count];
            for (int i = myList.Count - 1; i >= 0; i--)
            {
                majScores[scoresList.Count - i - 1] = myList[i].Key + ";" + myList[i].Value;
            }

            File.WriteAllLines("high_scores.txt", majScores);
        }

    }
}


