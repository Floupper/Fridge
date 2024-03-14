using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAE2._4
{
    public partial class Pseudo : Form
    {
        // Chemin de connexion à la base de données
        // string chcon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\louisschlegel\source\repos\SAE-D21-main\SAE2.4\baseFrigo.accdb";
        string chcon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\acker\OneDrive\Bureau\SAE\SAE24_ExploitationBDD_GIT\SAE2.4\baseFrigo.accdb";

        // Connexion à la base de données
        OleDbConnection connec = new OleDbConnection();

            // DataSet pour stocker les tables de la base de données
            DataSet dsPseudo = new DataSet();

            // Indicateur de connexion
            bool connecte = false;

            // Constructeur de la classe Pseudo
            public Pseudo(string pseudo)
            {
                InitializeComponent();
                lblNvPseudo.Text = pseudo; // Initialisation de l'étiquette lblNvPseudo avec la valeur du pseudo
            }

            // Propriété Connecte pour accéder à l'indicateur de connexion
            public bool Connecte
            {
                get { return this.connecte; } // Accesseur pour l'indicateur de connexion
                set { this.connecte = value; } // Mutateur pour l'indicateur de connexion
            }

            // Méthode exécutée lorsque la fenêtre est chargée
            private void Pseudo_Load(object sender, EventArgs e)
            {
                connec.ConnectionString = chcon; // Définition du chemin de connexion

                try
                {
                    connec.Open(); // Ouverture de la connexion à la base de données
                    ChargementDsLocal(); // Chargement des tables de la base de données dans le DataSet
                }
                catch (OleDbException)
                {
                    MessageBox.Show("Erreur dans la requête SQL"); // Affichage d'un message d'erreur en cas d'exception OleDb
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Erreur d'accès à la base"); // Affichage d'un message d'erreur en cas d'exception InvalidOperationException
                }
                finally
                {
                    if (connec.State == ConnectionState.Open)
                    {
                        connec.Close(); // Fermeture de la connexion si elle est ouverte
                    }
                }
            }

            // Méthode exécutée lorsqu'on clique sur le bouton ValiderPseudo
            private void btnValiderPseudo_Click(object sender, EventArgs e)
            {
                try
                {
                    connec.Open(); // Ouverture de la connexion à la base de données

                    int nbID = dsPseudo.Tables["Pseudo"].Rows.Count + 1; // Calcul du nombre d'ID en ajoutant 1 au nombre de lignes dans la table "Pseudo"
                    string ID = nbID.ToString() + lblNvPseudo.Text.Substring(0, 3).ToUpper(); // Création de l'ID en concaténant le nombre d'ID et les 3 premiers caractères du pseudo en majuscules

                    string requeteAjoutUtil = "INSERT INTO Pseudo (ID, Pseudo, Favoris) VALUES('" + ID + "','" + lblNvPseudo.Text + "','')"; // Requête SQL pour ajouter l'utilisateur à la table "Pseudo"
                    OleDbCommand cmdAjouterPseudo = new OleDbCommand(requeteAjoutUtil, connec); // Création d'une commande OleDb avec la requête d'ajout
                    cmdAjouterPseudo.ExecuteNonQuery(); // Exécution de la commande pour effectuer l'ajout dans la base de données

                    MessageBox.Show("Utilisateur ajouté !"); // Affichage d'un message de succès
                    connecte = true; // Indicateur de connexion mis à vrai
                    DialogResult = DialogResult.OK; // Fermeture de la fenêtre courante
                    ChargementDsLocal(); // Rechargement des tables de la base de données dans le DataSet
                }
                catch (OleDbException)
                {
                    MessageBox.Show("Erreur dans la requête SQL"); // Affichage d'un message d'erreur en cas d'exception OleDb
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Erreur d'accès à la base"); // Affichage d'un message d'erreur en cas d'exception InvalidOperationException
                }
                finally
                {
                    if (connec.State == ConnectionState.Open)
                    {
                    
                        connec.Close(); // Fermeture de la connexion si elle est ouverte
                    }
                }
            }

            // Méthode pour charger les tables de la base de données dans le DataSet
            private void ChargementDsLocal()
            {
                dsPseudo.Clear();
                DataTable dt = connec.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); // Récupération du schéma des tables de la base de données

                foreach (DataRow dr in dt.Rows)
                {
                    string nomTable = dr["TABLE_NAME"].ToString(); // Récupération du nom de la table

                    if (nomTable != "Table des erreurs") // Vérification si la table n'est pas "Table des erreurs"
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM " + nomTable, connec); // Création d'un OleDbDataAdapter pour sélectionner toutes les données de la table
                        da.Fill(dsPseudo, nomTable); // Remplissage du DataSet avec les données de la table
                    }
                }
            }

            // Méthode exécutée lorsqu'on clique sur le bouton IgnorerNvPseudo
            private void btnIgnorerNvPseudo_Click(object sender, EventArgs e)
            {
            DialogResult = DialogResult.Cancel; // Fermeture de la fenêtre courante
            }
        }
}


