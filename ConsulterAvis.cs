using Biblio1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsControlLibrary;

namespace SAE2._4
{
    public partial class ConsulterAvis : Form
    {
        DataSet dsAvis; // Variable pour stocker les avis
        string CdeRec; // Variable pour stocker le code de la recette

        // Constructeur de la classe ConsulterAvis
        // Paramètres :
        //   - Recette : code de la recette
        //   - ds : DataSet contenant les données des avis
        public ConsulterAvis(string Recette, DataSet ds)
        {
            InitializeComponent();

            dsAvis = ds; // Stocke le DataSet des avis dans la variable dsAvis
            CdeRec = Recette; // Stocke le code de la recette dans la variable CdeRec
        }

        // Méthode appelée lors du chargement du formulaire
        private void ConsulterAvis_Load(object sender, EventArgs e)
        {
            lblbTitreAvis.Text = lblbTitreAvis.Text + getNomRec(CdeRec); // Ajoute le nom de la recette à un label

            int hauteur = 0; // Variable pour gérer la position verticale des labels d'avis

            // Parcourt toutes les lignes de la table "Avis" du DataSet
            foreach (DataRow dtr in dsAvis.Tables["Avis"].Rows)
            {
                string pseudoAvis = "";

                // Recherche du pseudo correspondant à l'avis
                foreach (DataRow dtrNom in dsAvis.Tables["Pseudo"].Rows)
                {
                    if (dtrNom[0].ToString().CompareTo(dtr[1].ToString()) == 0)
                    {
                        pseudoAvis = dtrNom[1].ToString();
                    }
                }

                // Vérifie si l'avis correspond à la recette en cours
                if (dtr[0].ToString().Contains(CdeRec) && dtr[0].ToString().Length == 4 + CdeRec.Length)
                {
                    // Crée un nouveau label pour afficher l'avis
                    GroupBox grpAvis = new GroupBox();
                    grpAvis.BackColor = Color.White;
                    grpAvis.Width = 500;
                    grpAvis.Height = 100;
                    Label AvisTxt = new Label();
                    Label AvisNote = new Label();
                    PictureBox pcbEtoiles = new PictureBox();
                    pcbEtoiles.ImageLocation = "../../Properties/Etoile.jpg";
                    pcbEtoiles.Width = 50;
                    pcbEtoiles.Height = 50;
                    pcbEtoiles.Load();
                    pcbEtoiles.SizeMode = PictureBoxSizeMode.StretchImage;
                    pcbEtoiles.Top = 50;
                    pcbEtoiles.Left = 120;
                    AvisTxt.Font = lblFont.Font;
                    AvisNote.Font = lblFont.Font;
                    AvisTxt.Top = 5;
                    AvisNote.Top = 70;
                    AvisNote.Width = 100;
                    AvisNote.Height = 40;
                    AvisTxt.Left = 20;
                    AvisNote.Left = 20;
                    AvisTxt.Width = 400;
                    AvisTxt.Height = 50;
                    AvisNote.Text = "Note = " + dtr[2].ToString();
                    AvisTxt.Text = "Avis de : " + pseudoAvis + "\nL'avis est " + dtr[3].ToString().Replace("_", " ");
                    grpAvis.Controls.Add(AvisTxt);
                    grpAvis.Controls.Add(AvisNote);
                    grpAvis.Controls.Add(pcbEtoiles);
                    pnlAvis.Controls.Add(grpAvis); // Ajoute le label à un conteneur

                    hauteur += 110; // Met à jour la position verticale pour le prochain label
                }
            }
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
            foreach (DataRow dtr in dsAvis.Tables["Recettes"].Rows)
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

        private void btnMenu5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
