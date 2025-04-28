using TMPro;
using UnityEngine;

//Classe pour le contr�le du jeu contenant des variables n�cessaire � l'ensemble de la sc�ne
public class GameController : MonoBehaviour
{

    private int days = 1;
    private int moneyWin = 0;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI textJourInventaire;

    // Start is called before the first frame update
    public void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //On applique les param�tres sauvegard�s
        if (PlayerPrefs.HasKey("playerX"))
        {
            var actualPlayerRotation = player.transform.rotation;
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("playerX"), PlayerPrefs.GetFloat("playerY"), PlayerPrefs.GetFloat("playerZ"));
            player.transform.rotation = new Quaternion(actualPlayerRotation.x, PlayerPrefs.GetFloat("playerRotationY"), actualPlayerRotation.z, actualPlayerRotation.w);
            days = PlayerPrefs.GetInt("actualDays");
            moneyWin = PlayerPrefs.GetInt("moneyWin");
            moneyText.text = MainManager.Instance.GetMoney().ToString();
            textJourInventaire.text = days.ToString();
        }
    }

    //Fonction pour le changement de jour
    public void NewDay()
    {
        days++;
        moneyWin = 0;
        //On liste tout les terrains cultivables de la sc�ne
        GameObject[] listeOfCultivable = GameObject.FindGameObjectsWithTag("CapsuleDirt");
        foreach(GameObject cultivable in listeOfCultivable)
        {
            //On r�cup�re le script du plant
            Harvestable script = cultivable.GetComponent<Harvestable>();
            //Si on as quelque chose de plant�, on ajoute un jour � la culture.
            if (script != null)
            {
                script.AddDay();
            }
        }

        //On r�cup�re la liste des arbres fruitier
        GameObject[] listeOfFruitedTree = GameObject.FindGameObjectsWithTag("TreeLand");
        foreach (GameObject fruitedTree in listeOfFruitedTree)
        {
            //On r�cup�re le script du plant
            TreeLand script = fruitedTree.GetComponent<TreeLand>();
            //Si on as quelque chose de plant�, on ajoute un jour � l'arbre'.
            if (script != null)
            {
                script.AddDay();
            }
        }
    }

    //Fonction permettant d'ajouter de l'argent � la somme gagn� aujourd'hui et au total et met � jour le texte
    public void AddMoney(int amount)
    {
        moneyWin += amount;
        MainManager.Instance.AddMoney(amount);
        moneyText.text = MainManager.Instance.GetMoney().ToString();
    }

    //Fonction permettant de d�penser de l'argent et met � jour le texte
    public void SpendMoney(int amount)
    {
        MainManager.Instance.SpendMoney(amount);
        moneyText.text = MainManager.Instance.GetMoney().ToString();
    }

    //Fonction permettant de r�cup�rer la somme gagn� aujourd'hui
    public int GetMoneyWin()
    {
        return moneyWin;
    }

    //Fonction permettant de r�cup�rer le num�ro du jour actuel
    public int GetDays()
    {
        return days;
    }

}
