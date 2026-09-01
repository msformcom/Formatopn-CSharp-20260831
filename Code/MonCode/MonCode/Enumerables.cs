namespace MonCode;

[TestClass]
public class Enumerables
{
    [TestMethod]
    // Ne pas utiliser pour les ensemble dont ne nombre change
    // Utiliser pour un accès rapide à un élément par son index
    public void Tableaux()
    {
        // tableau de int de 1 dimension
        // CSharp indexation 0
        int length = 10;
        int[] tab1D = new int[length]; // Tableaux => taille fixe initialisée directement
        tab1D[0] = 2;

        tab1D= new int[10] {1,2,3,4,5,6,7,8,9,10};
        var DeuxiemeElement = tab1D[1];
        var nbElements = tab1D.Length;

        int[,] tab2D = new int[10, 5] ;


        // Tableau 2D => taille fixe initialisée directement
        tab2D[0,1] = 2;
        nbElements = tab2D.Length; // nombre total d'éléments dans le tableau 2D => 50
        var nbDimensions= tab2D.Rank; // nombre de dimensions du tableau 2D => 2
        var tailleDimension2 = tab2D.GetLength(1); // taille de la dimension 2 => 5

        int[,,] tab3D = new int[10, 5, 2]; // Tableau 3D => taille fixe initialisée directement
        tab3D[0, 1, 3] = 2;
        int[,,,] tab4D = new int[10, 5, 2, 3]; // Tableau 4D => taille fixe initialisée directement
        int[] tab1D2 = new int[5]; // Tableau 1D => taille fixe initialisée directement
        tab1D.CopyTo(tab1D2, 0); // Copie le tableau tab1D dans tab1D2 à partir de l'index 0
    }

    [TestMethod]
    public void List()
    {
        List<int> entiers = new List<int>() { 1,2,3};
        entiers.Add(4); // 1,2,3,4
        entiers.Add(5); // 1,2,3,4,5
        entiers.RemoveAt(2);    // 1,2,4,5
        entiers.Add(4); // 1,2,4,5,4
        entiers.Remove(4); // 1,2,5,4 => supprime le premier élément trouvé

        entiers.RemoveAll(e => e < 3); // 5,4
        entiers.RemoveAll(e => e== 3); // 5,4
        //entiers.RemoveAll(filtre); // 5,4 => supprime tous les éléments qui satisfont le prédicat filtre
        var e = entiers[1];

        List<Produit> catalogue = new List<Produit>();
        catalogue.OrderBy(c => c.Prix).ThenBy(c=>c.NbStock).Reverse();
        catalogue.RemoveAll(p => p.Prix < 10);

        var p = catalogue[2];
        // retire le produit p de la liste catalogue (uniquement la premiere occurence)
        // Comparaison par referenec
        catalogue.Remove(p); 
       
        bool filtre(int e) { 
            return e < 3;
        }
   
    }

    [TestMethod]
    public void IEnumerableTest()
    {
        IEnumerable<char> lettres=new char[] { 'a', 'b', 'c' }; 
        lettres = new List<char>() { 'a', 'b', 'c' };
        lettres = "Toto";


        var nouvelleChaine=lettres.OrderBy(c => c );
    }

    [TestMethod]
    public void SelectionEntiers()
    {
        // Utilisation Linq sur Objects (IEnumerable)
        // Pour manipuler des ensembles
        var entiers = new List<int>() { 1, 2, 6, 7, 2, 3, 4, 2, 3, 2, 7, 9, 19, 12 };
        // Liste des entiers pairs
        // % modulo => reste de la division entière
        var entiersPairs = entiers.Where(e => e % 2 == 0);

        // CREATE VIEW EmployesBienPayes AS
        // SELECT * FROM Employes WHERE salaireM > 890;

        // SELECT * FROM EmployesBienPayes
        var entiersPairsOrdreCroissant = entiers.Where(e => 
        e % 2 == 0
        ).OrderBy(e => e); // Ca ne créé pas une liste ni un tableau

        var entiersPairsOrdreCroissantListe = entiers.Where(c=>
            c%2==0
            ).Take(2).ToList();

        entiersPairsOrdreCroissant =entiersPairs.OrderBy(e=>e);
        entiersPairsOrdreCroissant.Skip(2).Take(4);

        var maSelection = entiers.Where(c => 
        c % 2 == 0
        ).Take(10).ToList();

        var count = maSelection.Count(); // 7 avec 14 executions filtre 
        //{ 1, 2, 6, 7, 2, 3, 4, 2, 3, 2, 7, 9, 19, 12 }
        
        
        foreach (var e in maSelection)
        {
            if (e == 4)
            {
                break;
            }
        }




        var selection=from e in entiers
                      where e % 2 == 0
                      orderby e
                      select e*2;
        // LinqToObject => filtrer et ordonner les elements en memoire
        // LinqToEnttities => envoyer une requete à une BDD
        // SELECT e FROM Entiers e WHERE e % 2 = 0 ORDER BY e
        selection = entiers.Where(e=>e%2==0)
                            .OrderBy(e=>e)
                            .Select(e=> e * 2);

        


    }

}
