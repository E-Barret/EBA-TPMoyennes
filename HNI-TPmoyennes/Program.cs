using System;
using System.Collections.Generic;
using System.Linq;

namespace TPMoyennes
{
    class Program
    {
        static void Main(string[] args)
        {
            // Création d'une classe
            Classe sixiemeA = new Classe("6eme A");
            // Ajout des élèves à la classe
            sixiemeA.ajouterEleve("Jean", "RAGE");
            sixiemeA.ajouterEleve("Paul", "HAAR");
            sixiemeA.ajouterEleve("Sibylle", "BOQUET");
            sixiemeA.ajouterEleve("Annie", "CROCHE");
            sixiemeA.ajouterEleve("Alain", "PROVISTE");
            sixiemeA.ajouterEleve("Justin", "TYDERNIER");
            sixiemeA.ajouterEleve("Sacha", "TOUILLE");
            sixiemeA.ajouterEleve("Cesar", "TICHO");
            sixiemeA.ajouterEleve("Guy", "DON");
            // Ajout de matières étudiées par la classe
            sixiemeA.ajouterMatiere("Francais");
            sixiemeA.ajouterMatiere("Anglais");
            sixiemeA.ajouterMatiere("Physique/Chimie");
            sixiemeA.ajouterMatiere("Histoire");

            Random random = new Random();
            // Ajout de 5 notes à chaque élève et dans chaque matière
            for (int ieleve = 0; ieleve < sixiemeA.eleves.Count; ieleve++)
            {
                for (int matiere = 0; matiere < sixiemeA.matieres.Count; matiere++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        sixiemeA.eleves[ieleve].ajouterNote(new Note(matiere, (float)((6.5 +
                       random.NextDouble() * 34)) / 2.0f));
                        // Note minimale = 3
                    }
                }
            }

            Eleve eleve = sixiemeA.eleves[6];
            // Afficher la moyenne d'un élève dans une matière
            Console.Write(eleve.prenom + " " + eleve.nom + ", Moyenne en " + sixiemeA.matieres[1] + " : " +
            eleve.Moyenne(1) + "\n");
            // Afficher la moyenne générale du même élève
            Console.Write(eleve.prenom + " " + eleve.nom + ", Moyenne Generale : " + eleve.Moyenne() + "\n");
            // Afficher la moyenne de la classe dans une matière
            Console.Write("Classe de " + sixiemeA.nomClasse + ", Moyenne en " + sixiemeA.matieres[1] + " : " +
            sixiemeA.Moyenne(1) + "\n");
            // Afficher la moyenne générale de la classe
            Console.Write("Classe de " + sixiemeA.nomClasse + ", Moyenne Generale : " + sixiemeA.Moyenne() + "\n");
            Console.Read();
        }
    }
}
// Classes fournies par HNI Institut
class Note
{
    public int matiere { get; private set; }
    public float note { get; private set; }
    public Note(int m, float n)
    {
        matiere = m;
        note = n;
    }
}

class Classe
{
    //Attributs
    public string nomClasse;
    public List<Eleve> eleves;
    public List<Matiere> matieres;

    //Constructeur
    public Classe(string nomClasse)
    {
        this.nomClasse = nomClasse;
        eleves = new List<Eleve>();
        matieres = new List<Matiere>();
    }

    //Méthodes
    public void ajouterEleve(string prenom, string nom)
    {
        if (eleves.Count <= 30)
        {
            eleves.Add(new Eleve(prenom, nom));
        }
        else
        {
            throw new Exception("Dépassement du nombre d'élève autorisé par classe");
        }
    }
    public void ajouterMatiere(string matiere)
    {
        if (matieres.Count <= 10)
        {
            matieres.Add(new Matiere(matiere));
        }
        else
        {
            throw new Exception("Dépassement du nombre de matière autorisé par classe");
        }
    }
    public float Moyenne(int i)     //Moyenne de la classe dans la matière n°i (moyenne des moyennes des élèves dans cette matière)
    {
        float moyenne = 0;
        int nbEleve = eleves.Count;
        if (nbEleve != 0)
        {
            foreach (Eleve j in eleves)
            {
                moyenne += j.Moyenne(i);
            }
            moyenne /= nbEleve;
            return eleves[0].MiseEnForme(moyenne);
        }
        else
        {
            throw new Exception("Pas d'élève dans la classe, moyenne de classe impossible");
        }
    }
    public float Moyenne()          //Moyenne générale de la classe (moyenne des moyennes par matière)
    {
        int nbMatiere = matieres.Count;
        if (nbMatiere != 0)
        {
            float moyenne = 0;
            for (int i = 0; i < nbMatiere; i++)
            {
                moyenne += Moyenne(i);
            }
            moyenne /= nbMatiere;
            return eleves[0].MiseEnForme(moyenne);
        }
        else
        {
            throw new Exception("Classe sans matière, moyenne de classe impossible");
        }
    }
}

class Eleve
{
    //Attribut
    public string nom;
    public string prenom;
    int nbNote;
    IDictionary<int,List<float>> notes;

    //Constructeur
    public Eleve(string prenom, string nom)
    {
        this.prenom = prenom;
        this.nom = nom;
        nbNote = 0;
        notes = new Dictionary<int, List<float>>();
    }

    //Méthodes
    public void ajouterNote(Note note)          //Les notes sont classées par matière grâce à un dictionnaire
    {
        if (!notes.ContainsKey(note.matiere))
        {
            notes.Add(note.matiere, new List<float>());
        }
        if (nbNote <= 200)
        {
            notes[note.matiere].Add(note.note);
            nbNote++;
        }
        else
        {
            throw new Exception("Dépassement du nombre de note autorisé pour l'élève");
        }
    }
    public float Moyenne(int i)         //Moyenne de la matière n°i
    {
        int nbNote = notes[i].Count;
        if (nbNote != 0)
        {
            float moyenne = 0;
            foreach(float note in notes[i])
            {
                moyenne += note;
            }
            moyenne /= nbNote;
            return MiseEnForme(moyenne);
        }
        else
        {
            throw new Exception("Elève sans note dans la matière, moyenne impossible");
        }
    }
    public float Moyenne()              //Moyenne générale de l'élève (moyenne des moyennes par matières)
    {
        int nbMatiere = notes.Count;
        if (nbMatiere != 0)
        {
            float moyenne = 0;
            for (int i = 0; i < nbMatiere; i++)
            {
                moyenne += Moyenne(i);
            }
            moyenne /= nbMatiere;
            return MiseEnForme(moyenne);
        }
        else
        {
            throw new Exception("l'élève est dans une classe sans matière, moyenne générale impossible");
        }
    }

    public float MiseEnForme(float moyenne)
    {
        Decimal a = Convert.ToDecimal(moyenne);
        a = decimal.Round(a, 2);
        moyenne = Convert.ToSingle(a);
        return moyenne;
    }
}

class Matiere
{
    public string matiere;

    public Matiere(string matiere)
    {
        this.matiere = matiere;
    }
    public override string ToString()
    {
        return matiere;
    }
}