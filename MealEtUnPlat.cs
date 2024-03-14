using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.AccessControl;
using WindowsFormsControlLibrary;
using Biblio1;
using System.ComponentModel.Design;

namespace SAE2._4
{
    public partial class FormMealEtUnPlat : Form
    {
        Dictionary<string, string> IngredientsChoisis = new Dictionary<string, string>();
        // Un dictionnaire pour stocker les ingrédients choisis, où la clé est le nom de l'ingrédient et la valeur est sa quantité ou autre information associée.

        Dictionary<string, string> IngredientsChoisisJusteAvant = new Dictionary<string, string>();
        // Un dictionnaire pour stocker les ingrédients choisis antérieurement, où la clé est le nom de l'ingrédient et la valeur est sa quantité ou autre information associée.

        List<string> codeRecette = new List<string>();
        // Une liste pour stocker les codes des recettes sélectionnées.

        // string chcon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\louisschlegel\source\repos\SAE-D21-main\SAE2.4\baseFrigo.accdb";
        string chcon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\acker\OneDrive\Bureau\BUT1\SAE\SAE24_ExploitationBDD_GIT\SAE24_ACKERMANN_SCHLEGEL_WILLEM\SAE2.4\baseFrigo.accdb";
        // Initialisation de la chaîne de connexion à la base de données.

        DataSet ds = new DataSet();
        // Création d'un DataSet qui contiendra l'ensemble de nos tables.

        BindingSource bs = new BindingSource();
        // Un BindingSource pour lier les données du DataSet à des contrôles d'interface utilisateur.

        DataSet dsEtapes = new DataSet();
        // Un autre DataSet pour stocker les étapes de la recette.

        OleDbConnection conn = new OleDbConnection();
        // Création d'une connexion OleDb pour se connecter à une base de données.

        string Arguments = "";
        // Une chaîne de caractères pour stocker les arguments de recherche de recette.

        int NbSelectionne = 0;
        // Un entier pour stocker le nombre d'éléments sélectionnés.

        string nomRecette = "";
        // Une chaîne de caractères pour stocker le nom de la recette.

        string NumeroRecette;
        // Une chaîne de caractères pour stocker le numéro de la recette.

        string pseudoNom = "";
        // Une chaîne de caractères pour stocker le pseudo.

        bool connecte = false;
        // Un booléen pour indiquer si l'utilisateur est connecté ou non.

        public FormMealEtUnPlat()
        {
            InitializeComponent();
        }

        /**
        * FormMealEtUnPlat_Load
        * 
        * Description :
        * Cette méthode est un gestionnaire d'événements pour l'événement "Load" du formulaire FormMealEtUnPlat. Elle est
        * responsable de l'initialisation de divers composants et données du formulaire lors de son chargement. La méthode 
        * établit une connexion avec la base de données, charge les données depuis la base de données dans des structures de 
        * données locales et effectue d'autres tâches d'initialisation.
        * 
        * Paramètres :
        * - sender (object) : L'objet qui a déclenché l'événement.
        * - e (EventArgs) : Arguments de l'événement.
        */
        private void FormMealEtUnPlat_Load(object sender, EventArgs e)
        {
            // Établir une connexion avec la base de données
            conn.ConnectionString = chcon; // Attribution de la chaîne de connexion à l'objet de connexion

            try
            {
                conn.Open(); // Ouvrir la connexion à la base de données

                txtPseudoMenu.Focus(); // Focus sur le textbox permettant d'entrer son Login

                // Effacer les éléments de la liste des ingrédients
                clbIngredients.Items.Clear();

                // Charger les structures de données locales avec les données de la base de données
                ChargementDsLocal();

                // Définir la source de données du BindingSource sur le DataSet
                bs.DataSource = ds;

                // Masquer les PictureBox utilisés pour indiquer les ingrédients sélectionnés
                pcbIngr1.Visible = false;
                pcbIngr2.Visible = false;
                pcbIngr3.Visible = false;

                // Générer des boutons radio pour chaque catégorie dans le DataSet
                int x = 10;
                int y = 10;
                int cmpt = 0;
                int cmpt2 = 0;

                // Première boucle pour compter les catégories dans le DataSet
                foreach (DataRow dataRow in ds.Tables["Catégories"].Rows)
                {
                    cmpt++;
                }

                // Deuxième boucle pour générer des boutons radio pour chaque catégorie
                foreach (DataRow dataRow in ds.Tables["Catégories"].Rows)
                {
                    cmpt2++;

                    // Réinitialiser les coordonnées si la position actuelle dépasse la largeur maximale
                    if (x > 400)
                    {
                        x = 10;
                        y = y + 50;
                    }

                    // Créer un bouton radio
                    RadioButton ra = new RadioButton();
                    ra.Width = 150;
                    ra.Text = dataRow[1].ToString();
                    ra.Font = lblFamille.Font;
                    ra.Left = x;
                    ra.Top = y;

                    // Ajouter le bouton radio au groupe
                    grpCat.Controls.Add(ra);
                    x += 175;

                    // Affecter un événement pour la sélection de la catégorie souhaitée
                    ra.CheckedChanged += new System.EventHandler(this.rdbCat_CheckedChanged);

                    // Ajouter un dernier bouton radio nommé "Aucun" après avoir initialisé tous les boutons de catégorie
                    if (cmpt == cmpt2)
                    {
                        if (x > 400)
                        {
                            x = 10;
                            y = y + 50;
                        }

                        // Créer un bouton radio "Aucun"
                        RadioButton ra2 = new RadioButton();
                        ra2.Width = 150;
                        ra2.Text = "Aucun";
                        ra2.Font = lblFamille.Font;
                        ra2.Left = x;
                        ra2.Top = y;

                        // Ajouter le bouton radio "Aucun" au groupe
                        grpCat.Controls.Add(ra2);
                        x = x + 175;
                    }
                }

                // Initialiser la comboBox contenant toutes les recettes pouvant être évaluées
                foreach (DataRow dtr in ds.Tables["Recettes"].Rows)
                {
                    cboRecetteEval.Items.Add(dtr[1].ToString());
                }
            }
            catch (OleDbException)
            {
                MessageBox.Show("Erreur dans la requête SQL");
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Erreur d'accès à la base de données");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void EffacerImage(PictureBox pictureBox) 
        { 
            pictureBox.Visible = false; 
        }

        /*
        Fonction: btnCuisiner_Click
        Description: Cette méthode est déclenchée lorsque le bouton "Cuisiner" est cliqué. Elle sélectionne l'onglet de la fenêtre qui permet la sélection des éléments.
        Paramètres:
            - sender: L'objet qui a déclenché l'événement (dans ce cas, le bouton "Cuisiner").
            - e: Les arguments de l'événement.
        Retour: Aucun
        */
        private void btnCuisiner_Click(object sender, EventArgs e)
        {
            tbcFenetre.SelectTab(1); // Passer à l'onglet 1 de la fenêtre (sélection des éléments)
        }

        /*
            Fonction: btnFavoris_Click
            Description: Cette méthode est déclenchée lorsque le bouton "Favoris" est cliqué. Elle sélectionne l'onglet de la fenêtre qui permet de voir les recettes favorites de l'utilisateur.
            Paramètres:
                - sender: L'objet qui a déclenché l'événement (dans ce cas, le bouton "Favoris").
                - e: Les arguments de l'événement.
            Retour: Aucun
        */
        private void btnFavoris_Click(object sender, EventArgs e)
        {
            if (connecte)
            {
                tbcFenetre.SelectTab(7); // Passer à l'onglet 8 de la fenêtre (recettes favorites)
            }
            else
            {
                MessageBox.Show("Veuillez vous connecter !");
            }
        }

        /*
            Fonction: btnAvis_Click
            Description: Cette méthode est déclenchée lorsque le bouton "Avis" est cliqué. Elle sélectionne l'onglet de la fenêtre qui permet de voir les avis.
            Paramètres:
                - sender: L'objet qui a déclenché l'événement (dans ce cas, le bouton "Avis").
                - e: Les arguments de l'événement.
            Retour: Aucun
        */
        private void btnAvis_Click(object sender, EventArgs e)
        {
            if (connecte)
            {
                tbcFenetre.SelectTab(6); // Passer à l'onglet 7 de la fenêtre (onglet avis)
            }
            else
            {
                MessageBox.Show("Veuillez vous connecter !");
            }
        }


        /*
         * Méthode utilisée pour charger les tables d'une base de données locale dans un objet DataSet.
         * Elle utilise le schéma OleDb pour obtenir les noms des tables de la base de données,
         * puis remplit l'objet DataSet avec les données de chaque table.
         */
        private void ChargementDsLocal()
        {
            ds.Clear();
            // Obtient le schéma des tables de la base de données à l'aide de la connexion OleDb
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

            // Parcourt chaque ligne dans le schéma des tables
            foreach (DataRow dr in dt.Rows)
            {
                // Obtient le nom de la table à partir de la colonne "TABLE_NAME" de la ligne
                string nomTable = dr["TABLE_NAME"].ToString();

                // Vérifie si le nom de la table n'est pas "Table des erreurs"
                if (nomTable != "Table des erreurs")
                {
                    // Crée un OleDbDataAdapter pour sélectionner toutes les données de la table
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM " + nomTable, conn);

                    // Remplit l'objet DataSet "ds" avec les données de la table
                    da.Fill(ds, nomTable);
                }
            }
        }


        /*
         * Méthode appelée lorsque le bouton "btnBasePatisserie" est cliqué.
         * Elle met à jour les éléments de l'interface utilisateur en fonction de la catégorie sélectionnée.
         */
        private void btnBasePatisserie_Click(object sender, EventArgs e)
        {
            // Convertit le sender en objet Button
            System.Windows.Forms.Button t = (System.Windows.Forms.Button)sender;

            // Récupère le numéro de catégorie à partir de la propriété Tag du bouton
            int numCat = Convert.ToInt16(t.Tag);

            // Efface les éléments de la liste des ingrédients
            clbIngredients.Items.Clear();

            // Met à jour le texte du label "lblFamille" avec le texte du bouton
            lblFamille.Text = t.Text;

            // Parcourt chaque ligne dans le tableau "Ingrédients" de l'objet DataSet "ds"
            foreach (DataRow dr in ds.Tables["Ingrédients"].Rows)
            {
                // Vérifie si la catégorie de l'ingrédient correspond au numéro de catégorie sélectionné
                if (dr[2].ToString() == numCat.ToString())
                {
                    // Ajoute le nom de l'ingrédient à la liste des ingrédients
                    clbIngredients.Items.Add(dr[1].ToString());
                }
            }

            // Trie la liste des ingrédients dans l'ordre alphabétique
            List<string> ingredientsList = new List<string>();
            foreach (var item in clbIngredients.Items)
            {
                ingredientsList.Add(item.ToString());
            }
            ingredientsList.Sort();

            clbIngredients.Items.Clear();

            // Ajoute les ingrédients triés à la liste des ingrédients
            foreach (var ingredient in ingredientsList)
            {
                clbIngredients.Items.Add(ingredient);
            }

            // Vérifie si l'ingrédient est déjà choisi dans la liste IngredientsChoisis
            foreach (KeyValuePair<string, string> str in IngredientsChoisis)
            {
                for (int i = 0; i < clbIngredients.Items.Count; i++)
                {
                    if (str.Key.CompareTo(ds.Tables["Ingrédients"].Rows[i][0].ToString()) == 0)
                    {
                        // Coche l'élément de la liste correspondant à l'ingrédient choisi
                        clbIngredients.SetItemChecked(i, true);
                        break;
                    }
                }
            }
        }


        /*
        * Méthode appelée lorsque l'état de la case à cocher d'une catégorie change.
        * Elle met à jour la variable Arguments avec le texte de la catégorie sélectionnée.
        */
        private void rdbCat_CheckedChanged(object sender, EventArgs e)
        {
            // Convertit le sender en RadioButton
            RadioButton ra = (RadioButton)sender;

            // Vérifie si le RadioButton est coché
            if (ra.Checked)
            {
                // Vérifie si la variable Arguments n'est pas vide
                if (!Arguments.Equals(""))
                {
                    // Réinitialise la variable Arguments à une chaîne vide
                    Arguments = "";
                }

                // Met à jour la variable Arguments avec le texte du RadioButton sélectionné
                Arguments = ra.Text;
            }
        }



        /*
        * Méthode appelée lorsque le bouton de validation de modification est cliqué.
        * Elle effectue une série d'opérations pour afficher les recettes correspondant aux ingrédients sélectionnés.
        */
        private void btnValiderModif_Click(object sender, EventArgs e)
        {
            pnlChoixRecette.Controls.Clear();
            // Sélectionne l'onglet 3 de la fenêtre
            tbcFenetre.SelectTab(3);

            // Crée une liste pour stocker les recettes sélectionnées
            List<string> lstRecSelec = new List<string>();

            // Nettoie le code de recette
            codeRecette.Clear();

            // Vérifie si des ingrédients ont été choisis
            try
            {
                int hauteurRec = 40;

                // Ouvre la connexion à la base de données
                conn.Open();

                string requeteValider = "";
                if (IngredientsChoisis.Count > 0)
                {
                    // Parcourt chaque paire clé-valeur dans la liste des ingrédients choisis
                    foreach (KeyValuePair<string, string> str in IngredientsChoisis)
                    {
                        // Construit la requête SQL pour sélectionner les recettes correspondantes
                        requeteValider = @"SELECT * FROM Recettes
                                      WHERE tempsCuisson <= " + ucTempsChoix1.getTemps +
                                             " AND categPrix <= " + ucBudgetJauge1.getBudget +
                                             " AND codeRecette IN (SELECT codeRecette FROM IngrédientsRecette WHERE codeIngredient = " + int.Parse(str.Key) + ")";


                        // Crée une commande pour exécuter la requête SQL
                        OleDbCommand commandRechercher = new OleDbCommand(requeteValider, conn);

                        // Exécute la requête SQL et récupère le lecteur de données
                        OleDbDataReader reader = commandRechercher.ExecuteReader();

                        // Parcourt les résultats du lecteur de données
                        while (reader.Read())
                        {
                            // Ajoute le code de recette à la liste des recettes sélectionnées
                            lstRecSelec.Add(reader.GetValue(0).ToString());

                            // Passe au résultat suivant dans le lecteur de données
                            reader.NextResult();
                        }

                        // Ferme le lecteur de données
                        reader.Close();
                    }
                }
                else
                {
                    // Construit la requête SQL pour sélectionner les recettes correspondantes
                    requeteValider = @"SELECT * FROM Recettes
                                      WHERE tempsCuisson <= " + ucTempsChoix1.getTemps +
                                      " AND categPrix <= " + ucBudgetJauge1.getBudget;

                    // Crée une commande pour exécuter la requête SQL
                    OleDbCommand commandRechercher = new OleDbCommand(requeteValider, conn);

                    // Exécute la requête SQL et récupère le lecteur de données
                    OleDbDataReader reader = commandRechercher.ExecuteReader();

                    // Parcourt les résultats du lecteur de données
                    while (reader.Read())
                    {
                       
                        // Ajoute le code de recette à la liste des recettes sélectionnées
                        lstRecSelec.Add(reader.GetValue(0).ToString());

                        // Passe au résultat suivant dans le lecteur de données
                        reader.NextResult();
                    }

                    // Ferme le lecteur de données
                    reader.Close();
                }

                // Parcourt la liste des recettes sélectionnées
                foreach (string code in lstRecSelec)
                {
                    // Génère dynamiquement des UserControls pour afficher les recettes
                    UcAffichageRecette afficheRecette = new UcAffichageRecette(chcon, getImageRec(code), getNomRec(code), getTempsRec(code), getBudget(code), int.Parse(code));

                    // Ajoute des gestionnaires d'événements aux UserControls
                    afficheRecette.DoubleClick += new System.EventHandler(this.btnLancerRec_Click);
                    afficheRecette.CheckedChanged += tempsCout_CheckedChanged;
                    afficheRecette.Top = hauteurRec;
                    afficheRecette.Left = 90;

                    afficheRecette.Tag = code;

                    // Génère dynamiquement des boutons pour consulter les avis sur la recette
                    Button btnAvisRec = new Button();


                    btnAvisRec.BackColor = Color.White;

                    btnAvisRec.Top = hauteurRec +10;

                    btnAvisRec.Font = lblIngr1.Font;
                    btnAvisRec.Text = "Avis";

                    btnAvisRec.Height = 80;

                    btnAvisRec.Width = 80;

                    btnAvisRec.Tag = code;

                    btnAvisRec.Click += new System.EventHandler(this.btnVoirAvis_Click);

                    RadioButton ra = new RadioButton();
                    ra.Width = 150;
                    ra.Tag = code;
                    ra.Text = "";
                    ra.Left = 520;
                    ra.Top = hauteurRec + 50;

                    // Ajoute les UserControls et les boutons aux groupes de choix de recettes
                    pnlChoixRecette.Controls.Add(afficheRecette);
                    pnlChoixRecette.Controls.Add(btnAvisRec);
                    pnlChoixRecette.Controls.Add(ra);
                    hauteurRec += 120;
                }
            }

            catch (OleDbException)
            {
                MessageBox.Show("Erreur dans la requête SQL");
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Erreur d'accès à la base de données");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
       
        }

        /*
        Fonction: ComparerCodeRecette
        Description: Cette méthode compare les codes de recette fournis en entrée et retourne une liste des codes de recette qui apparaissent exactement trois fois dans la liste.
        Paramètres:
            - codeRec: Une liste de chaînes de caractères représentant les codes de recette à comparer.
        Retour: Une liste de chaînes de caractères contenant les codes de recette qui apparaissent trois fois.
        */
        List<string> ComparerCodeRecette(List<string> codeRec)
        {
            // Création d'un dictionnaire pour stocker les occurrences de chaque code de recette
            Dictionary<string, int> occurences = new Dictionary<string, int>();

            // Liste pour stocker les codes de recette qui apparaissent trois fois
            List<string> CdeRec = new List<string>();

            // Parcours de chaque code de recette dans la liste fournie
            foreach (string recettes in codeRec)
            {
                // Vérification si le code de recette existe déjà dans le dictionnaire
                if (occurences.ContainsKey(recettes))
                {
                    // Incrémentation du nombre d'occurrences du code de recette
                    occurences[recettes]++;
                }
                else
                {
                    // Ajout du code de recette au dictionnaire avec une occurrence initiale de 1
                    occurences[recettes] = 1;
                }
            }

            // Parcours des paires clé-valeur dans le dictionnaire
            foreach (KeyValuePair<string, int> occu in occurences)
            {
                // Vérification si le code de recette apparaît exactement trois fois
                if (occu.Value == 3)
                {
                    // Ajout du code de recette à la liste des codes de recette qui apparaissent trois fois
                    CdeRec.Add(occu.Key);
                }
            }

            // Retour de la liste des codes de recette qui apparaissent trois fois
            return CdeRec;
        }

        /*
            Fonction: getBudget
            Description: Cette méthode prend un code de recette en entrée et recherche le budget correspondant dans une table de données.
            Paramètres:
                - codeRecette: Le code de recette dont on souhaite obtenir le budget.
            Retour: Le budget de la recette correspondante sous forme d'un entier. Si la recette n'est pas trouvée, la valeur 0 est retournée.
        */
        private int getBudget(string codeRecette)
        {
            // Parcours des lignes de la table "Recettes"
            foreach (DataRow dtr in ds.Tables["Recettes"].Rows)
            {
                // Vérification si le code de recette correspond à celui recherché
                if (dtr["codeRecette"].ToString() == codeRecette)
                {
                    // Conversion de la valeur du budget en entier et retour
                    return int.Parse(dtr[5].ToString());
                }
            }

            // Retour de la valeur 0 si la recette n'est pas trouvée
            return 0;
        }

        /*
            Fonction: getImageRec
            Description: Cette méthode prend un code de recette en entrée et recherche l'image correspondante dans une table de données.
            Paramètres:
                - codeRecette: Le code de recette dont on souhaite obtenir l'image.
            Retour: L'image de la recette correspondante sous forme d'une chaîne de caractères. Si la recette n'est pas trouvée, la valeur "0" est retournée.
        */
        private string getImageRec(string codeRecette)
        {
            // Parcours des lignes de la table "Recettes"
            foreach (DataRow dtr in ds.Tables["Recettes"].Rows)
            {
                // Vérification si le code de recette correspond à celui recherché
                if (dtr["codeRecette"].ToString() == codeRecette)
                {
                    // Retour de la valeur de l'image sous forme de chaîne de caractères
                    return dtr[4].ToString();
                }
            }

            // Retour de la valeur "0" si la recette n'est pas trouvée
            return "0";
        }

        /*
            Fonction: getNomRec
            Description: Cette méthode prend un code de recette en entrée et recherche le nom correspondant dans une table de données.
            Paramètres:
                - codeRecette: Le code de recette dont on souhaite obtenir le nom.
            Retour: Le nom de la recette correspondante sous forme d'une chaîne de caractères. Si la recette n'est pas trouvée, la valeur "0" est retournée.
        */
        private string getNomRec(string codeRecette)
        {
            // Parcours des lignes de la table "Recettes"
            foreach (DataRow dtr in ds.Tables["Recettes"].Rows)
            {
                // Vérification si le code de recette correspond à celui recherché
                if (dtr["codeRecette"].ToString() == codeRecette)
                {
                    // Retour du nom de la recette sous forme de chaîne de caractères
                    return dtr[1].ToString();
                }
            }

            // Retour de la valeur "0" si la recette n'est pas trouvée
            return "0";
        }

        /*
            Fonction: getTempsRec
            Description: Cette méthode prend un code de recette en entrée et recherche le temps correspondant dans une table de données.
            Paramètres:
                - codeRecette: Le code de recette dont on souhaite obtenir le temps.
            Retour: Le temps de la recette correspondante sous forme d'un entier. Si la recette n'est pas trouvée, la valeur 0 est retournée.
        */
        private int getTempsRec(string codeRecette)
        {
            // Parcours des lignes de la table "Recettes"
            foreach (DataRow dtr in ds.Tables["Recettes"].Rows)
            {
                // Vérification si le code de recette correspond à celui recherché
                if (dtr["codeRecette"].ToString() == codeRecette)
                {
                    // Conversion de la valeur du temps en entier et retour
                    return int.Parse(dtr[3].ToString());
                }
            }

            // Retour de la valeur 0 si la recette n'est pas trouvée
            return 0;
        }

        /*
        Ce code génère un document PDF à partir des données fournies dans un formulaire.
        Il crée un document PDF contenant un récapitulatif des informations de la recette, telles que le nom de la recette
        et la durée de préparation. Le document PDF est ensuite enregistré sur le disque local.
        */
        private void btnPDF_Click(object sender, EventArgs e)
        {
            // Initialisation des variables
            string nomFichier = nomRecette + "PDF"; // Nom du fichier PDF à créer
            int hauteur = 10; // Hauteur de la page 1
            int hauteur2 = 40; // Hauteur de la page 2
            int hauteur3 = 40; // Hauteur de la page 3
            PdfDocument DocumentPdf = new PdfDocument(); // Création du document PDF

            // Création et configuration de la première page du PDF
            PdfPage page = DocumentPdf.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Création et configuration de la deuxième page du PDF
            PdfPage pageNb2 = DocumentPdf.AddPage();
            XGraphics gfx2 = XGraphics.FromPdfPage(pageNb2);

            // Création et configuration de la troisième page du PDF
            PdfPage pageNb3 = DocumentPdf.AddPage();
            XGraphics gfx3 = XGraphics.FromPdfPage(pageNb3);

            // Définition des polices à utiliser dans le document
            XFont PoliceTitre = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont PoliceSousTitre = new XFont("Arial", 14, XFontStyle.Underline);
            XFont PoliceParagraphe = new XFont("Arial", 12);

            // Ajout du titre principal du document
            TextePDF("Récapitulatif " + nomRecette + " :", 0, hauteur, 600, 50, gfx, PoliceTitre);
            hauteur += 50;

            // Ajout de la section "Informations"
            TextePDF("Informations :", 0, hauteur, 600, 50, gfx, PoliceSousTitre);
            hauteur += 40;

            // Parcours des données de recettes pour trouver les informations de la recette en cours
            foreach (DataRow dtr in ds.Tables["Recettes"].Rows)
            {
                if (dtr["codeRecette"].ToString() == btnLancerRec.Tag.ToString())
                {
                    // Ajout des informations de temps et de budget de la recette
                    TextePDF("Temps : " + dtr[3].ToString(), 100, hauteur, 100, 50, gfx, PoliceParagraphe);
                    string TypeBudget = "";
                    if (dtr[5].ToString() == "0")
                    {
                        TypeBudget = "Peu onéreux";
                    }
                    if (dtr[5].ToString() == "1")
                    {
                        TypeBudget = "Abordable";
                    }
                    if (dtr[5].ToString() == "2")
                    {
                        TypeBudget = "Onéreux";
                    }
                    TextePDF("Budget : " + TypeBudget, 200, hauteur, 100, 50, gfx, PoliceParagraphe);
                }
            }
            hauteur += 40;

            // Vérification de la sélection des listes d'ingrédients et d'ustensiles
            if (rdbListeNon1.Checked == true || rdbListeNon2.Checked == true)
            {
                int hauteurprec = hauteur;

                // Ajout de la section "Ingrédients"
                TextePDF("Ingrédients :", 0, hauteur, 100, 50, gfx, PoliceSousTitre);

                // Parcours des données d'ingrédients de la recette
                foreach (DataRow dtr in ds.Tables["IngrédientsRecette"].Rows)
                {
                    if (dtr[0].ToString() == btnLancerRec.Tag.ToString())
                    {
                        // Recherche de l'ingrédient correspondant dans la table des ingrédients
                        foreach (DataRow dtr2 in ds.Tables["Ingrédients"].Rows)
                        {
                            if (dtr[1].ToString().CompareTo(dtr2[0].ToString()) == 0)
                            {
                                hauteur += 40;
                                // Ajout de l'ingrédient à la liste
                                TextePDFGauche("- " + dtr2[1], 40, hauteur, 100, 50, gfx, PoliceParagraphe);
                            }
                        }
                    }
                }
                hauteur += 50;

                // Ajout de la section "Ustensiles"
                TextePDF("Ustensiles :", 300, hauteurprec, 100, 50, gfx, PoliceSousTitre);
                List<String> Ustensiles = new List<string>();

                // Parcours des données d'ustensiles nécessaires à la recette
                foreach (DataRow dtr in ds.Tables["BesoinsUstensiles"].Rows)
                {
                    if (dtr[0].ToString() == btnLancerRec.Tag.ToString())
                    {
                        Ustensiles.Add(dtr[1].ToString());
                        // Recherche de l'ustensile correspondant dans la table des ustensiles
                        foreach (DataRow dtr2 in ds.Tables["Ustensiles"].Rows)
                        {
                            if (Ustensiles.Contains(dtr2[0].ToString()))
                            {
                                hauteurprec += 40;
                                // Ajout de l'ustensile à la liste
                                TextePDFGauche("- " + dtr2[1], 340, hauteurprec, 100, 50, gfx, PoliceParagraphe);
                                Ustensiles.Remove(dtr2[0].ToString());
                            }
                        }
                    }
                }
            }

            int i = 1; // Compteur d'étapes
            bool page1 = false; // Indicateur de passage à la deuxième page

            // Connexion à la base de données
            try
            {
                conn.Open();
                ChargementDsLocal();
            }
            catch (OleDbException)
            {
                MessageBox.Show("Erreur dans la requête SQL");
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Erreur d'accès à la base");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            // Parcours des étapes de la recette
            foreach (DataRow numRecette in ds.Tables["EtapesRecette"].Rows)
            {
                if (numRecette[0].ToString() == btnLancerRec.Tag.ToString() && int.Parse(numRecette[1].ToString()) >= i)
                {
                    if (hauteur > 600)
                    {
                        // Passage à la deuxième page en raison d'un trop grand nombre d'étapes
                        TextePDF("Etape " + i + " :", 300, hauteur2, 100, 50, gfx2, PoliceSousTitre);
                        hauteur2 += 40;

                        // Ajout de l'image de l'étape (ou une image par défaut si l'image n'est pas disponible)
                        try
                        {
                            ImgPDF("../../Properties/" + numRecette["imageEtape"].ToString(), 490, hauteur2, 75, 75, gfx2);
                        }
                        catch
                        {
                            ImgPDF("../../Properties/cocotte.jpg", 490, hauteur2, 75, 75, gfx2);
                        }

                        // Ajout du texte de l'étape avec alignement justifié
                        TextePDFAlignement(numRecette["texteEtape"].ToString(), XParagraphAlignment.Justify, 20, hauteur2, 450, 50, gfx2, PoliceParagraphe);
                        hauteur2 = hauteur2 + 50;
                        page1 = true;
                        i++;
                    }
                    else
                    {
                        // Ajout de l'étape à la première page
                        TextePDF("Etape " + i + " :", 300, hauteur, 100, 50, gfx, PoliceSousTitre);
                        hauteur += 40;

                        // Ajout de l'image de l'étape (ou une image par défaut si l'image n'est pas disponible)
                        try
                        {
                            ImgPDF("../../Properties/" + numRecette["imageEtape"].ToString(), 490, hauteur, 75, 75, gfx);
                        }
                        catch
                        {
                            ImgPDF("../../Properties/cocotte.jpg", 490, hauteur, 75, 75, gfx);
                        }

                        // Ajout du texte de l'étape avec alignement justifié
                        TextePDFAlignement(numRecette["texteEtape"].ToString(), XParagraphAlignment.Justify, 20, hauteur, 450, 50, gfx, PoliceParagraphe);
                        hauteur += 50;
                        i++;
                    }
                }
            }

            // Vérification de la sélection des listes de courses
            if (rdbListeOui1.Checked == true || rdbListeOui2.Checked == true)
            {
                // Ajout de la section "Liste de course"
                TextePDF("Liste de course :", 100, hauteur3, 100, 50, gfx3, PoliceTitre);
                hauteur3 += 50;
                int hauteurprec = hauteur3;

                // Ajout de la section "Ingrédients"
                TextePDF("Ingrédients :", 0, hauteur3, 100, 50, gfx3, PoliceSousTitre);
                hauteur3 += 50;
                List<string> ingredientSelect = new List<string>();

                // Parcours des ingrédients de la recette
                foreach (DataRow dtr in ds.Tables["IngrédientsRecette"].Rows)
                {
                    if (dtr[0].ToString() == "1")
                    {
                        foreach (DataRow dtr2 in ds.Tables["Ingrédients"].Rows)
                        {
                            if (dtr[1].ToString().CompareTo(dtr2[0].ToString()) == 0 && !ingredientSelect.Contains(dtr[1].ToString()))
                            {
                                hauteur3 += 40;
                                ingredientSelect.Add(dtr[1].ToString());
                                // Ajout de l'ingrédient à la liste de courses
                                TextePDFGauche("- " + dtr2[1], 40, hauteur3, 100, 50, gfx3, PoliceParagraphe);
                            }
                        }
                    }
                }
                hauteur3 += 50;

                // Ajout de la section "Ustensiles"
                TextePDF("Ustensiles :", 300, hauteurprec, 100, 50, gfx3, PoliceSousTitre);
                List<String> Ustensiles = new List<string>();

                // Parcours des ustensiles nécessaires à la recette
                foreach (DataRow dtr in ds.Tables["BesoinsUstensiles"].Rows)
                {
                    if (dtr[0].ToString() == "1")
                    {
                        Ustensiles.Add(dtr[1].ToString());
                        foreach (DataRow dtr2 in ds.Tables["Ustensiles"].Rows)
                        {
                            if (Ustensiles.Contains(dtr2[0].ToString()))
                            {
                                hauteurprec += 50;
                                // Ajout de l'ustensile à la liste de courses
                                TextePDFGauche("- " + dtr2[1], 340, hauteurprec, 100, 50, gfx3, PoliceParagraphe);
                                Ustensiles.Remove(dtr2[0].ToString());
                            }
                        }
                    }
                }
            }

            DocumentPdf.Save(nomFichier); // Sauvegarde du PDF avec le nom de fichier spécifié
            Process.Start(nomFichier); // Ouverture automatique du PDF
        }

        // Affiche l'image associée à l'ingrédient sélectionné dans un PictureBox.
        // <param name="pictureBox">Le PictureBox où afficher l'image.</param>
        private void AfficherImageCompte(PictureBox pictureBox)
        {
            pictureBox.Visible = true;

            try
            {
                // Tente de charger une image avec l'extension .jpg correspondant à l'ingrédient choisi
                pictureBox.ImageLocation = "../../Properties/" + IngredientsChoisis.Last().Value + ".jpg";
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Load();
            }
            catch
            {
                try
                {
                    // Si le chargement précédent a échoué, tente de charger une image avec l'extension .png
                    pictureBox.ImageLocation = "../../Properties/" + IngredientsChoisis.Last().Value + ".png";
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Load();
                }
                catch
                {
                    // Si les chargements précédents ont échoué, affiche une image par défaut (lama.png)
                    pictureBox.ImageLocation = "../../Properties/lama.png";
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Load();
                }
            }
        }

        private void TextePDF(string contenu, int x, int y, int largeur, int hauteur, XGraphics Xgfx, XFont police)
        {
            XRect ZoneTexte = new XRect(x, y, largeur, hauteur);
            // Crée un rectangle définissant la zone de texte avec les coordonnées (x, y), la largeur et la hauteur spécifiées.

            XTextFormatter tfTitre = new XTextFormatter(Xgfx);
            // Crée un formateur de texte en utilisant le contexte graphique spécifié.

            Xgfx.DrawRectangle(XBrushes.White, ZoneTexte);
            // Dessine un rectangle blanc à l'emplacement spécifié pour effacer toute zone de texte précédente.

            Xgfx.DrawString(contenu, police, XBrushes.Black, ZoneTexte, XStringFormats.Center);
            // Dessine le contenu spécifié à l'intérieur du rectangle avec la police et la couleur spécifiées, en centrant le texte.
        }

        private void TextePDFGauche(string contenu, int x, int y, int largeur, int hauteur, XGraphics Xgfx, XFont police)
        {
            XRect ZoneTexte = new XRect(x, y, largeur, hauteur);
            // Crée un rectangle définissant la zone de texte avec les coordonnées (x, y), la largeur et la hauteur spécifiées.

            XTextFormatter tfTitre = new XTextFormatter(Xgfx);
            // Crée un formateur de texte en utilisant le contexte graphique spécifié.

            Xgfx.DrawRectangle(XBrushes.White, ZoneTexte);
            // Dessine un rectangle blanc à l'emplacement spécifié pour effacer toute zone de texte précédente.

            Xgfx.DrawString(contenu, police, XBrushes.Black, ZoneTexte, XStringFormats.TopLeft);
            // Dessine le contenu spécifié à l'intérieur du rectangle avec la police et la couleur spécifiées, en alignant le texte en haut à gauche.
        }

        private void TextePDFAlignement(string contenu, XParagraphAlignment disposition, int x, int y, int largeur, int hauteur, XGraphics Xgfx, XFont police)
        {
            XTextFormatter tfPara = new XTextFormatter(Xgfx);
            // Crée un formateur de texte en utilisant le contexte graphique spécifié.

            XRect ZoneTexte = new XRect(x, y, largeur, hauteur);
            // Crée un rectangle définissant la zone de texte avec les coordonnées (x, y), la largeur et la hauteur spécifiées.

            Xgfx.DrawRectangle(XBrushes.White, ZoneTexte);
            // Dessine un rectangle blanc à l'emplacement spécifié pour effacer toute zone de texte précédente.

            tfPara.Alignment = disposition;
            // Définit l'alignement du texte en utilisant la disposition spécifiée.

            tfPara.DrawString(contenu, police, XBrushes.Black, ZoneTexte, XStringFormats.TopLeft);
            // Dessine le contenu spécifié à l'intérieur du rectangle avec la police et la couleur spécifiées, en utilisant l'alignement spécifié.
        }
        private void ImgPDF(string Chemin, int x, int y, int largeur, int hauteur, XGraphics Xgfx)
        {
            XRect rcImage = new XRect(x, y, largeur, hauteur);
            // Crée un rectangle définissant la position et les dimensions de l'image.

            Xgfx.DrawRectangle(XBrushes.LightGray, rcImage);
            // Dessine un rectangle gris clair à l'emplacement spécifié pour représenter l'espace réservé à l'image.

            Xgfx.DrawImage(XImage.FromFile(Chemin), rcImage);
            // Dessine l'image spécifiée à l'intérieur du rectangle aux coordonnées et dimensions spécifiées.
        }

        // Méthode qui ajoute un code de recette en fonction de l'ingrédient sélectionné
        private string AjouterCodeRecette(string ck)
        {
            // Parcourt toutes les lignes du DataTable "Ingrédients" dans le DataSet "ds"
            foreach (DataRow dr in ds.Tables["Ingrédients"].Rows)
            {
                // Vérifie si la valeur de la deuxième colonne (index 1) de la ligne est égale à l'ingrédient sélectionné
                if (dr[1].ToString().CompareTo(ck) == 0)
                {
                    // Retourne la valeur de la première colonne (index 0) de la ligne (le code de recette correspondant)
                    return dr[0].ToString();
                }
            }

            // Retourne une chaîne vide si aucun code de recette n'a été trouvé pour l'ingrédient sélectionné
            return "";
        }

        // Gère l'événement Click du bouton 'btnGauche'.
        // Sélectionne l'onglet de l'index 1 dans le contrôle 'tbcFenetre'.
        private void btnGauche_Click(object sender, EventArgs e)
        {
            tbcFenetre.SelectTab(1);
        }

        // Gère l'événement Click du bouton 'btnMenu'.
        // Sélectionne l'onglet de l'index 0 dans le contrôle 'tbcFenetre'.
        private void btnMenu_Click(object sender, EventArgs e)
        {
            tbcFenetre.SelectTab(0);
        }


        private void btnValiderAvis_Click(object sender, EventArgs e)
        {
            int nbID = ds.Tables["Avis"].Rows.Count + 1;
            // Récupère le nombre d'ID déjà présents dans la table "Avis" et ajoute 1 pour obtenir le nouvel ID.

            string ID = nbID.ToString();
            // Convertit le nouvel ID en chaîne de caractères.

            int nbCbo = cboRecetteEval.SelectedIndex + 1;
            // Ajoute les informations supplémentaires à l'ID : les 3 premières lettres du pseudo en majuscules et l'index de la recette évaluée.
            ID += txtPseudoMenu.Text.Substring(0, 3).ToUpper() + nbCbo.ToString();

            if (connecte)
            {
                // Vérifie si l'utilisateur est connecté.

                try
                {
                    conn.Open();
                    // Ouvre la connexion à la base de données.

                    double note = 0;
                    // Variable pour stocker la note de l'avis.

                    foreach (Control chk in ucNoteAvis1.Controls)
                    {
                        // Parcourt tous les contrôles enfants du contrôle ucNoteAvis1.

                        if (chk.Name.Contains("checkBox"))
                        {
                            // Vérifie si le contrôle est une case à cocher.

                            if (((CheckBox)chk).Checked)
                            {
                                // Vérifie si la case à cocher est cochée.
                                note += 0.50;
                                // Ajoute 0.50 à la note.
                            }
                        }
                    }

                    // Construction de la requête SQL pour insérer l'avis dans la table "Avis".
                    string requeteAvis = @"INSERT INTO Avis VALUES ('" + ID + "', '" + pseudoNom + "', '" + note + "', '" + rctAvis.Text.ToString().Replace(" ", "_") + "')";
                    // Affiche la requête SQL dans une boîte de dialogue (à des fins de débogage).

                    OleDbCommand ajouterAvis = new OleDbCommand(requeteAvis, conn);
                    // Crée une commande OleDb pour exécuter la requête SQL.

                    ajouterAvis.ExecuteNonQuery();
                    // Exécute la commande pour insérer l'avis dans la base de données.

                    MessageBox.Show("Avis envoyé !");
                    rctAvis.Clear();
                    cboRecetteEval.SelectedIndex = -1;
                    ucNoteAvis1.ReinitPictureBox();
                    pcbImageAvis.Visible = false;
                }
                catch (OleDbException)
                {
                    MessageBox.Show("Erreur dans la requête SQL");
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Erreur d'accès à la base de données");
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    // Ferme la connexion à la base de données.
                }
            }
            else
            {
                MessageBox.Show("Veuillez vous connecter !");
                // Affiche un message indiquant à l'utilisateur de se connecter s'il n'est pas connecté.
            }
        }

        // Gestion de l'événement CheckedChanged lorsqu'une checkbox est sélectionnée ou désélectionnée
        private void tempsCout_CheckedChanged(object sender, EventArgs e)
        {
            pnlFavoris.Controls.Clear();
            // Récupération du UserControl UcAffichageRecette qui a déclenché l'événement
            UcAffichageRecette Favoris = (UcAffichageRecette)sender;

            // Récupération de la valeur de la propriété 'favoris' du UserControl si nécessaire
            bool getFavoris = Favoris.getFavoris;
            string ContenuFavoris = "";

            List<string> ListFavoris = new List<string>();

            // Vérification si l'utilisateur est connecté
            if (connecte && !getFavoris)
            {
                try
                {
                    // Ouverture de la connexion à la base de données
                    conn.Open();

                    // Parcours des lignes de la table "Pseudo" du DataSet pour récupérer la liste des favoris de l'utilisateur actuellement connecté
                    foreach (DataRow dtr in ds.Tables["Pseudo"].Rows)
                    {
                        if (dtr[1].ToString().CompareTo(txtPseudoMenu.Text) == 0)
                        {
                            foreach (string str in dtr[2].ToString().Split(','))
                            {
                                ListFavoris.Add(str);
                            }
                        }
                    }
                    
                    ListFavoris.Remove(((UcAffichageRecette)sender).Tag.ToString());

                    foreach (string str in ListFavoris)
                    {
                        if(str != String.Empty)
                        {
                            ContenuFavoris += str + ',';
                        }
                        
                    }
                    if (ContenuFavoris != String.Empty)
                    {
                        ContenuFavoris = ContenuFavoris.Substring(0, ContenuFavoris.Length - 1);
                    }

                    // Construction de la requête SQL pour mettre à jour la liste des favoris dans la base de données
                    string requeteAjouterFavoris = @"UPDATE Pseudo
                                                    SET Favoris = '" + ContenuFavoris +
                                                    "' WHERE Pseudo = '" + txtPseudoMenu.Text + "'";

                    // Exécution de la requête SQL
                    OleDbCommand commandFav = new OleDbCommand(requeteAjouterFavoris, conn);
                    commandFav.ExecuteNonQuery();

                    // Chargement à nouveau des données locales depuis la base de données
                    ChargementDsLocal(); 
                }
                catch (OleDbException)
                {
                    MessageBox.Show("Erreur dans la requête SQL");
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Erreur d'accès à la base");
                }
                finally
                {
                    // Fermeture de la connexion si elle est ouverte
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                // Parcours à nouveau des lignes de la table "Pseudo" pour afficher un message indiquant que l'utilisateur est connecté et ajouter les favoris
                foreach (DataRow dtr in ds.Tables["Pseudo"].Rows)
                {
                    if (dtr[1].ToString().CompareTo(txtPseudoMenu.Text) == 0)
                    {
                        connecte = true;

                        if (dtr[2].ToString() != String.Empty)
                        {
                            AjouterFavoris(dtr[2].ToString());
                        }
                    }
                }
            }
            // Vérification si l'utilisateur est connecté
            else if (connecte && getFavoris)
            {
                try
                {
                    // Ouverture de la connexion à la base de données
                    conn.Open();

                    // Parcours des lignes de la table "Pseudo" du DataSet pour récupérer la liste des favoris de l'utilisateur actuellement connecté
                    foreach (DataRow dtr in ds.Tables["Pseudo"].Rows)
                    {
                        if (dtr[1].ToString().CompareTo(txtPseudoMenu.Text) == 0)
                        {
                            ContenuFavoris = dtr[2].ToString();
                        }
                    }
                    string requeteAjouterFavoris;
                    // Construction de la requête SQL pour mettre à jour la liste des favoris dans la base de données
                    if (ContenuFavoris == String.Empty)
                    {
                        requeteAjouterFavoris = @"UPDATE Pseudo
                                             SET Favoris = '" + ContenuFavoris + ((UcAffichageRecette)sender).Tag.ToString() +
                                                  "' WHERE Pseudo = '" + txtPseudoMenu.Text + "'";
                    }
                    else
                    {
                        requeteAjouterFavoris = @"UPDATE Pseudo
                                             SET Favoris = '" + ContenuFavoris + "," + ((UcAffichageRecette)sender).Tag.ToString() +
                                                     "' WHERE Pseudo = '" + txtPseudoMenu.Text + "'";
                    }

                    // Exécution de la requête SQL
                    OleDbCommand commandFav = new OleDbCommand(requeteAjouterFavoris, conn);
                    commandFav.ExecuteNonQuery();

                    // Chargement à nouveau des données locales depuis la base de données
                    ChargementDsLocal();
                }
                catch (OleDbException)
                {
                    MessageBox.Show("Erreur dans la requête SQL");
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Erreur d'accès à la base");
                }
                finally
                {
                    // Fermeture de la connexion si elle est ouverte
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

                // Parcours à nouveau des lignes de la table "Pseudo" pour afficher un message indiquant que l'utilisateur est connecté et ajouter les favoris
                foreach (DataRow dtr in ds.Tables["Pseudo"].Rows)
                {
                    if (dtr[1].ToString().CompareTo(txtPseudoMenu.Text) == 0)
                    {
                        connecte = true;

                        if (dtr[2].ToString() != String.Empty)
                        {
                            AjouterFavoris(dtr[2].ToString());
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez vous connecter !");
                Favoris.setFavoris();
            }
        }


        // Méthode appelée lors du clic sur le bouton btnReinisitaliser
        private void btnReinisitaliser_Click(object sender, EventArgs e)
        {
            // Réinitialisation des valeurs et des éléments de l'interface utilisateur
            clbIngredients.Items.Clear();
            IngredientsChoisisJusteAvant.Clear();
            IngredientsChoisis.Clear(); // Efface tous les ingrédients choisis de la liste IngredientsChoisis

            // Masque les images correspondant aux ingrédients choisis
            pcbIngr1.Visible = false;
            pcbIngr2.Visible = false;
            pcbIngr3.Visible = false;

            lblFamille.Text = "Nom de la Famille"; // Réinitialise le texte du label lblFamille à sa valeur par défaut
            lblIngr1.Text = "Ingrédient 1"; // Réinitialise le texte du label lblIngr1 à sa valeur par défaut
            lblIngr2.Text = "Ingrédient 2"; // Réinitialise le texte du label lblIngr2 à sa valeur par défaut
            lblIngr3.Text = "Ingrédient 3"; // Réinitialise le texte du label lblIngr3 à sa valeur par défaut

            NbSelectionne = 0; // Réinitialise le compteur NbSelectionne à 0

            clbIngredients.Items.Clear(); // Efface tous les éléments de la CheckedListBox clbIngredients
        }

        // Méthode appelée lors du clic sur le bouton btnLancerRec
        private void btnLancerRec_Click(object sender, EventArgs e)
        {
            int code = 0; // Variable pour stocker le code de recette sélectionné
            foreach (Control rdb in pnlChoixRecette.Controls)
            {
                if (rdb.Width == 150)
                {
                    if (((RadioButton)rdb).Checked)
                    {
                        code = int.Parse(rdb.Tag.ToString()); // Récupère le code de recette à partir de la propriété Tag du RadioButton
                        btnLancerRec.Tag = code; // Stocke le code de recette dans la propriété Tag du bouton btnLancerRec
                    }
                }

            }
            if (code != 0)
            {
                tbcFenetre.SelectTab(5); // Sélectionne l'onglet 5 (tabPage) dans le contrôle tbcFenetre
                bs.DataSource = ds.Tables["EtapesRecette"]; // Définit la source de données du BindingSource bs sur la table "EtapesRecette" du DataSet ds

                if(ds.Tables["Recettes"].Rows[code - 1][5].ToString().CompareTo("1") == 0)
                {
                    lblPrix1.Text = "Prix : Peu Cher"; // Affiche le prix de la recette 
                    lblPrix2.Text = "Prix : Peu Cher"; 
                }
                else if (ds.Tables["Recettes"].Rows[code - 1][5].ToString().CompareTo("2") == 0)
                {
                    lblPrix1.Text = "Prix : Cher"; // Affiche le prix de la recette 
                    lblPrix2.Text = "Prix : Cher"; 
                }
                else
                {
                    lblPrix1.Text = "Prix : Très Cher"; // Affiche le prix de la recette 
                    lblPrix2.Text = "Prix : Très Cher"; 
                }
                   

                lblPersonnes1.Text = "Pour " + ds.Tables["Recettes"].Rows[code - 1][2].ToString() + " personnes"; // Affiche le nombre de personnes de la recette correspondant au code dans le label lblPersonnes1
                lblPersonnes2.Text = "Pour " + ds.Tables["Recettes"].Rows[code - 1][2].ToString() + " personnes"; // Affiche le nombre de personnes de la recette correspondant au code dans le label lblPersonnes2

                lblTemps1.Text = "Temps : " + ds.Tables["Recettes"].Rows[code - 1][3].ToString() + "mins"; // Affiche le temps de préparation de la recette correspondant au code dans le label lblTemps1
                lblTemps2.Text = "Temps : " + ds.Tables["Recettes"].Rows[code - 1][3].ToString() + "mins"; // Affiche le temps de préparation de la recette correspondant au code dans le label lblTemps2

                lblNomRecette1.Text = ds.Tables["Recettes"].Rows[code - 1][1].ToString(); // Affiche le nom de la recette correspondant au code dans le label lblNomRecette1
                lblNomRecette2.Text = ds.Tables["Recettes"].Rows[code - 1][1].ToString(); // Affiche le nom de la recette correspondant au code dans le label lblNomRecette2

                int x = 20;
                int y = 50;
                foreach (DataRow dtr in ds.Tables["IngrédientsRecette"].Rows)
                {
                    if (dtr[0].ToString() == code.ToString())
                    {
                        foreach (DataRow dtr2 in ds.Tables["Ingrédients"].Rows)
                        {
                            if (x > 600)
                            {
                                x = 10;
                                y = y + 50;
                                pnlIngredVisual2.AutoScroll = true;
                                pnlIngredVisual1.AutoScroll = true;
                            }
                            if (dtr[1].ToString().CompareTo(dtr2[0].ToString()) == 0)
                            {

                                Label lblIngred = new Label();
                                lblIngred.Left = x;
                                lblIngred.Top = y;
                                lblIngred.Height = 30;
                                lblIngred.Width = 280;
                                lblIngred.Text = "- " + dtr2[1].ToString();

                                Label lblIngred2 = new Label();
                                lblIngred2.Left = x;
                                lblIngred2.Top = y;
                                lblIngred2.Height = 30;
                                lblIngred2.Width = 280;
                                lblIngred2.Text = "- " + dtr2[1].ToString();

                                pnlIngredVisual2.Controls.Add(lblIngred); // Ajoute le label lblIngred au groupe grpIngredVisual1
                                pnlIngredVisual1.Controls.Add(lblIngred2); // Ajoute le label lblIngred2 au groupe grpIngredVisual2
                                x += 300;
                            }
                        }
                    }
                }

                dsEtapes = ds.Copy(); // Crée une copie du DataSet ds dans la variable dsEtapes
                int hauteurEtape = 0;
                int decalageEtapes = 30;
                foreach (DataRow drEtapes in dsEtapes.Tables["EtapesRecette"].Rows)
                {
                    if (drEtapes[0].ToString() != code.ToString())
                    {
                        drEtapes.Delete(); // Supprime la ligne si le code de recette ne correspond pas à celui sélectionné
                    }

                    else
                    {
                        if (decalageEtapes > 400)
                        {
                            decalageEtapes = 30;
                            hauteurEtape += 300;
                            pnlVisualRecetteInt.AutoScroll = true;
                        }
                        PetiteEtape PtitEtape = new PetiteEtape();
                        PtitEtape.lblEtape.Text = "Etape : " + drEtapes[1];
                        if (drEtapes[3].ToString().Length < 4 || drEtapes[3].ToString().CompareTo("") == 0)
                        {
                            PtitEtape.pcbImage.ImageLocation = "../../Properties/cocotte.jpg";
                        }
                        else
                        {
                            PtitEtape.pcbImage.ImageLocation = "../../Properties/" + drEtapes[3].ToString();
                        }

                        PtitEtape.pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                        PtitEtape.pcbImage.Load();
                        PtitEtape.rtxtConsigne.Text = drEtapes[2].ToString();
                        PtitEtape.lblEtape.Font = lblIngr1.Font;
                        PtitEtape.rtxtConsigne.Font = lblPersonnes1.Font;
                        PtitEtape.Margin = new System.Windows.Forms.Padding(3);
                        PtitEtape.Height = 250;
                        PtitEtape.Top = hauteurEtape;
                        PtitEtape.Left = decalageEtapes;
                        decalageEtapes += 300;

                        pnlVisualRecetteInt.Controls.Add(PtitEtape); // Ajoute le contrôle PtitEtape au panel1
                    }
                }
                lblEtapes.Text = "Etape : 1";
                try
                {
                    pcbEtapeEtape.ImageLocation = "../../Properties/" + ((DataRowView)bs.Current)["imageEtape"].ToString();
                    pcbEtapeEtape.SizeMode = PictureBoxSizeMode.StretchImage;
                    pcbEtapeEtape.Load();
                }
                catch
                {

                    pcbEtapeEtape.ImageLocation = "../../Properties/cocotte.jpg";
                    pcbEtapeEtape.SizeMode = PictureBoxSizeMode.StretchImage;
                    pcbEtapeEtape.Load();
                }
                rctEtapeContenu.Text = ((DataRowView)bs.Current)["texteEtape"].ToString();
            }
            else
            {
                MessageBox.Show("Veuillez indiquer la recette à visualiser"); // Affiche un message indiquant de sélectionner une recette
            }
        }

        private void btnGauche2_Click(object sender, EventArgs e)
        {
            tbcFenetre.SelectTab(2);
        }

        // Méthode appelée lors du clic sur le bouton btnValiderPseudo
        private void btnValiderPseudo_Click(object sender, EventArgs e)
        {
            if (txtPseudoMenu.Text.ToString() == String.Empty)
            {
                MessageBox.Show("Veuillez Saisir un pseudo !");
            }
            else
            {
                bool AjoutPseudo = true; // Variable booléenne pour indiquer si le pseudo doit être ajouté ou non
                foreach (DataRow dtr in ds.Tables["Pseudo"].Rows)
                {
                    // Vérifie si la valeur de la deuxième colonne (index 1) de la ligne correspond au pseudo entré dans le champ txtPseudoMenu
                    if (dtr[1].ToString().CompareTo(txtPseudoMenu.Text) == 0)
                    {
                        AjoutPseudo = false; // Si le pseudo est trouvé dans la table, on ne l'ajoute pas
                        pseudoNom = dtr[0].ToString(); // Récupère le pseudo à partir de la première colonne (index 0) de la ligne
                        txtPseudoMenu.Enabled = false;
                    }
                }
                if (AjoutPseudo == true)
                {
                    // Si le pseudo n'existe pas dans la table, crée une nouvelle instance de la classe Pseudo avec le pseudo entré dans txtPseudoMenu
                    Pseudo pseudo = new Pseudo(txtPseudoMenu.Text);

                    if (pseudo.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            conn.Open();
                            ChargementDsLocal();
                        }
                        catch (OleDbException)
                        {
                            MessageBox.Show("Erreur dans la requête SQL");
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("Erreur d'accès à la base de données");
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                        }
                    }

                        connecte = pseudo.Connecte; // Met à jour la variable connecte avec la valeur de la propriété Connecte de l'objet Pseudo
                    if (connecte)
                    {
                        txtPseudoMenu.Enabled = false;
                        pseudoNom = txtPseudoMenu.Text;
                    }

                }
                else
                {
                    foreach (DataRow dtr in ds.Tables["Pseudo"].Rows)
                    {
                        // Recherche à nouveau le pseudo dans la table
                        if (dtr[1].ToString().CompareTo(txtPseudoMenu.Text) == 0)
                        {
                            MessageBox.Show("Vous êtes connectés"); // Affiche un message indiquant que l'utilisateur est connecté
                            connecte = true; // Met à jour la variable connecte à true

                            if (dtr[2].ToString() != String.Empty)
                            {
                                AjouterFavoris(dtr[2].ToString()); // Appelle une méthode AjouterFavoris avec la valeur de la troisième colonne (index 2) de la ligne
                            }
                        }
                    }

                }
            }
            
        }


        private void AjouterFavoris(string favorisList)
        {
            pnlFavoris.Controls.Clear();
            // Efface tous les contrôles enfants du panel pntlFavoris.

            int hauteurFav = 20;
            // Variable pour suivre la position verticale des contrôles des recettes dans le panel pntlFavoris.

            foreach (string favoris in favorisList.Split(','))
            {
                // Parcourt chaque élément de la liste des favoris séparés par des virgules.

                // Crée une instance de UcAffichageRecette pour afficher la recette.
                UcAffichageRecette afficheRecette = new UcAffichageRecette(chcon, getImageRec(favoris), getNomRec(favoris), getTempsRec(favoris), getBudget(favoris), int.Parse(favoris));

                // Positionne le contrôle de l'affichage de la recette.
                afficheRecette.Top = hauteurFav;
                hauteurFav += 100;
                afficheRecette.Left = 30;
                afficheRecette.setFavoris();
                afficheRecette.CheckedChanged += tempsCout_CheckedChanged;
                afficheRecette.Tag = afficheRecette.getCode;
                foreach(Control ctrl in afficheRecette.Controls)
                {
                    if(ctrl.Name.Contains("check"))
                    {
                        ((CheckBox)ctrl).Checked = true;
                    }
                }

                // Ajoute le contrôle de l'affichage de la recette au panel pntlFavoris.
                pnlFavoris.Controls.Add(afficheRecette);
            }
        }

        /*
         * Méthode utilisée pour gérer l'événement KeyPress de la zone de texte "txtPseudoMenu".
         * Elle restreint la saisie de caractères en n'acceptant que les lettres, les chiffres et la touche Entrée.
         * Lorsque la touche Entrée est enfoncée, elle déclenche l'événement Click du bouton "btnValiderPseudo".
         */
        private void txtPseudoMenu_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Empêche le caractère d'être traité par le contrôle TextBox
            e.Handled = true;

            // Vérifie si la touche enfoncée est la touche Entrée, une lettre ou un chiffre
            if (e.KeyChar == (char)Keys.Enter || char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back)
            {
                // Permet au caractère d'être traité par le contrôle TextBox
                e.Handled = false;

                // Vérifie si la touche enfoncée est la touche Entrée
                if (e.KeyChar == (char)Keys.Enter)
                {
                    // Déclenche l'événement Click du bouton "btnValiderPseudo"
                    btnValiderPseudo_Click(sender, e);
                }
            }
        }


        /*
         * Méthode appelée lorsque le bouton "btnVoirAvis" est cliqué.
         * Elle affiche la fenêtre "ConsulterAvis" pour consulter les avis d'une recette.
         */
        private void btnVoirAvis_Click(object sender, EventArgs e)
        {
            // Convertit le sender en objet Button
            System.Windows.Forms.Button t = (System.Windows.Forms.Button)sender;

            // Récupère le numéro de catégorie à partir de la propriété Tag du bouton
            string numCat = t.Tag.ToString();

            // Crée une instance de la fenêtre "ConsulterAvis" en lui passant le numéro de catégorie et l'objet DataSet "ds" en paramètres
            ConsulterAvis consultAvisRecette = new ConsulterAvis(numCat, ds);

            // Affiche la fenêtre "ConsulterAvis"
            consultAvisRecette.Show();
        }

        /*
         * Méthode appelée lorsque le bouton "btnBSAvant" est cliqué.
         * Elle gère le défilement des étapes d'une recette à l'aide d'un BindingSource.
         */
        private void btnBSAvant_Click(object sender, EventArgs e)
        {
            // Définit la source de données du BindingSource comme étant la table "EtapesRecette" du DataSet
            bs.DataSource = dsEtapes.Tables["EtapesRecette"];

            // Convertit le sender en objet Button
            System.Windows.Forms.Button t = (System.Windows.Forms.Button)sender;

            // Récupère le numéro de catégorie à partir de la propriété Tag du bouton et le convertit en entier
            int numCat = int.Parse(t.Tag.ToString());

            // Effectue une action en fonction de la valeur de numCat
            if (numCat == 0) // GAUCHE
            {
                bs.MovePrevious();
            }
            else if (numCat == 1) // DROITE
            {
                bs.MoveNext();
            }
            else if (numCat == 2) // DEBUT
            {
                bs.MoveFirst();
            }
            else // FIN
            {
                bs.MoveLast();
            }

            // Met à jour le texte du Label avec la valeur actuelle du BindingSource
            lblEtapes.Text = "Etape : " + ((DataRowView)bs.Current)["NumEtape"].ToString();

            try
            {
                // Charge l'image de l'étape actuelle dans le PictureBox
                pcbEtapeEtape.ImageLocation = "../../Properties/" + ((DataRowView)bs.Current)["imageEtape"].ToString();
                pcbEtapeEtape.SizeMode = PictureBoxSizeMode.StretchImage;
                pcbEtapeEtape.Load();
            }
            catch
            {
                // Charge une image par défaut si aucune image n'est disponible pour l'étape actuelle
                pcbEtapeEtape.ImageLocation = "../../Properties/cocotte.jpg";
                pcbEtapeEtape.SizeMode = PictureBoxSizeMode.StretchImage;
                pcbEtapeEtape.Load();
            }

            // Affiche le contenu textuel de l'étape actuelle dans le RichTextBox
            rctEtapeContenu.Text = ((DataRowView)bs.Current)["texteEtape"].ToString();
        }

        /*
         * Fonction: btnLogout_Click
         * Description: Cette fonction est appelée lorsque le bouton de déconnexion est cliqué.
         *              Elle effectue les actions nécessaires pour se déconnecter de l'application.
        */
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Nettoie les contrôles dans le panneau pntlFavoris
            pnlFavoris.Controls.Clear();
            txtPseudoMenu.Text = String.Empty;
            txtPseudoMenu.Enabled = true;
            // Marque l'utilisateur comme déconnecté en définissant la variable connecte à false
            connecte = false;
        }

        /*
         * Fonction: rdbOrdreAlpha2_CheckedChanged
         * Description: Cette fonction est appelée lorsqu'un bouton radio de tri alphabétique est coché.
         *              Elle effectue les actions nécessaires pour effectuer le tri dans l'ordre alphabétique.
         */
        private void rdbOrdreAlpha2_CheckedChanged(object sender, EventArgs e)
        {
            // Convertit l'objet sender en un objet RadioButton
            System.Windows.Forms.RadioButton t = (System.Windows.Forms.RadioButton)sender;

            // Récupère la valeur du tag associée au bouton radio
            string numCat = t.Tag.ToString();

            // Effectue les actions nécessaires pour effectuer le tri dans l'ordre alphabétique
            // (Le code pour le tri alphabétique sera ajouté ici)
        }

        /*
         * Fonction: rdbIntegral1_CheckedChanged
         * Description: Cette fonction est appelée lorsqu'un bouton radio d'intégrale est coché.
         *              Elle effectue les actions nécessaires en fonction de l'option sélectionnée.
         */
        private void rdbIntegral1_CheckedChanged(object sender, EventArgs e)
        {
            // Convertit l'objet sender en un objet RadioButton
            RadioButton rdbVisual = (RadioButton)sender;

            // Vérifie la valeur du tag associée au bouton radio
            if (rdbVisual.Tag.ToString().CompareTo("1") == 0)
            {
                // Sélectionne l'onglet 5 dans le contrôle tbcFenetre (TabControl)
                tbcFenetre.SelectTab(5);

                // Coche le bouton radio rdbIntegrale2
                rdbIntegrale2.Checked = true;
            }
            else
            {
                // Sélectionne l'onglet 4 dans le contrôle tbcFenetre (TabControl)
                tbcFenetre.SelectTab(4);

                // Coche le bouton radio rdbEtapeEtape1
                rdbEtapeEtape1.Checked = true;
            }
        }

        private void btnReinitialiser_Click(object sender, EventArgs e)
        {
            rctAvis.Clear();
            cboRecetteEval.SelectedIndex = -1;
            ucNoteAvis1.ReinitPictureBox();
            pcbImageAvis.Visible = false;
        }

        private void cboRecetteEval_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {
                pcbImageAvis.Visible = true;
                pcbImageAvis.ImageLocation = "../../Properties/" + getImageRec((cboRecetteEval.SelectedIndex + 1).ToString());
                pcbImageAvis.SizeMode = PictureBoxSizeMode.StretchImage;
                pcbImageAvis.Load();
            }
            catch
            {

                pcbImageAvis.ImageLocation = "../../Properties/cocotte.jpg";
                pcbImageAvis.SizeMode = PictureBoxSizeMode.StretchImage;
                pcbImageAvis.Load();
            }
            
        }

        private void btnToChoixRecette1_Click(object sender, EventArgs e)
        {
            tbcFenetre.SelectTab(3);
        }

        private void btnSuivant_Click(object sender, EventArgs e)
        {
            tbcFenetre.SelectTab(2);
        }


        // Méthode appelée lors de la sélection d'un élément dans la CheckedListBox clbIngredients
        // Méthode appelée lors de la sélection d'un élément dans la CheckedListBox clbIngredients
        private void clbIngredients_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Conversion du sender en CheckedListBox
            System.Windows.Forms.CheckedListBox t = (System.Windows.Forms.CheckedListBox)sender;

            int i = 0;
            List<PictureBox> lstpcb = new List<PictureBox>();
            lstpcb.Add(pcbIngr1);
            lstpcb.Add(pcbIngr2);
            lstpcb.Add(pcbIngr3);
            List<Label> lstlabel = new List<Label>();
            lstlabel.Add(lblIngr1);
            lstlabel.Add(lblIngr2);
            lstlabel.Add(lblIngr3);

            // Effacer les images et les labels précédents
            EffacerImage(pcbIngr1);
            EffacerImage(pcbIngr2);
            EffacerImage(pcbIngr3);
            lblIngr1.Text = "Ingredient 1";
            lblIngr2.Text = "Ingredient 2";
            lblIngr3.Text = "Ingredient 3";
            IngredientsChoisis.Clear();

            if (IngredientsChoisisJusteAvant.Count < 3)
            {

                // Copier les ingrédients choisis précédemment dans IngredientsChoisis
                foreach (KeyValuePair<string, string> kvp in IngredientsChoisisJusteAvant)
                {
                    IngredientsChoisis.Add(kvp.Key, kvp.Value);
                }

                // Parcourir les éléments cochés dans la CheckedListBox
                foreach (String ck in clbIngredients.CheckedItems)
                {
                    if (IngredientsChoisis.Count < 3)
                    {
                        // Vérifier si l'ingrédient est déjà choisi
                        if (!IngredientsChoisis.ContainsValue(ck))
                        {
                            // Ajouter l'ingrédient à IngredientsChoisis et IngredientsChoisisJusteAvant
                            IngredientsChoisis.Add(AjouterCodeRecette(ck), ck);
                            IngredientsChoisisJusteAvant.Add(AjouterCodeRecette(ck), ck);
                        }
                    }
                }

                // Afficher les images et les labels des ingrédients choisis
                foreach (KeyValuePair<string, string> kvp in IngredientsChoisis)
                {
                    AfficherImageCompte(lstpcb[i]);
                    lstlabel[i].Text = kvp.Value;
                    i++;
                }
            }
            else
            {
                MessageBox.Show("Liste Pleine");
            }
        }
    }
}

