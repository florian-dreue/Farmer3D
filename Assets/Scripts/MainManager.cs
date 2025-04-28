using UnityEngine;

//Classe permettant de cr�er une variable accessible partout dans le jeu
public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    private int money = 0;

    //Fonction appel� au d�but du cycle de vie de la class
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("money"))
        {
            money = PlayerPrefs.GetInt("money");
        }
    }

    //Fonction permettant de r�cup�rer l'argent du joueur
    public int GetMoney()
    {
        return money;
    }

    //Fonction permettant d'ajouter de l'argent au joueur
    public void AddMoney(int amount)
    {
        money += amount;
    }

    //Fonction permettant de retirer de l'argent au joueur
    public void SpendMoney(int amount)
    {
        money -= amount;
    }

}
