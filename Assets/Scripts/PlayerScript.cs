using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // stats
    public int moveSpeed;
    public int tacticalSpeed;
    public int naturalArmor;
    public int maxHealth;
    public int actionPointMax = 2;
    public int respawnTime;
    public int movementCost = 1;
    public int maxUltimateCD;
    public int ultimateAPCost;

    // editable things
    [SerializeField] TextAsset characterJSON;
    public int myTeam;
    [SerializeField] Transform characterSprite;
    [SerializeField] StatusLibrary statusLibrary;
    public List<Sprite> abilityIcons;
    public Sprite ultimateIcon;
    public Sprite portrait;

    // references
    TurnManager tm;
    InputManager im;
    UIManager uim;
    MouseOverTiles mouseOverTiles;
    Pathfinding pathfinding;
    PlayerMovement playerMovement;
    CharacterAbilities characterAbilities;
    TileHolder tileHolder;
    HealthBar healthBar;

    // non editable public variables
    [System.NonSerialized] public CharacterData characterData;
    [System.NonSerialized] public CharacterEvents characterEvents;
    [System.NonSerialized] public int health;
    [System.NonSerialized] public int ultimateCD;
    [System.NonSerialized] public int moveSpeedModifier = 0;
    [System.NonSerialized] public int turnMeter = 0;
    [System.NonSerialized] public int armor;
    [System.NonSerialized] public int activeAbility = 0;
    [System.NonSerialized] public bool movementAbility = false;
    [System.NonSerialized] public int actionPoints;
    [System.NonSerialized] public List<int> actionPointCosts = new List<int>();
    [System.NonSerialized] public List<int> manaCosts = new List<int>();
    [System.NonSerialized] public List<int> maxAbilityCooldowns = new List<int>();
    [System.NonSerialized] public int[] abilityCooldowns;
    [System.NonSerialized] public int respawnTimer;
    [System.NonSerialized] public bool dead = false;
    [System.NonSerialized] public bool livingShardplate = false;
    private int silenced = 0;
    public int Silenced
    {
        get { return silenced; }
        set { silenced = value; characterEvents.Silenced(); }
    }
    [System.NonSerialized] public int rooted = 0;
    [System.NonSerialized] public int speedBost = 0;
    [System.NonSerialized] public int stun = 0;
    [System.NonSerialized] public bool dayOfBlackSun = false;
    [System.NonSerialized] public int sleep = 0;
    [System.NonSerialized] public int frenzy = 0;
    [System.NonSerialized] public bool ultimateActive = false;
    [System.NonSerialized] public List<int> greyedOutAbilities = new List<int>();
    [System.NonSerialized] public List<int> silenceableAbilities = new List<int>();

    // non editable variables
    Vector2 hell = new Vector2(1000, 1000);
    bool respawning = false;
    float characterSpriteX;
    float defaultDirection;

    private void Awake()
    {
        characterEvents = GetComponent<CharacterEvents>();
        tm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>();
        im = tm.gameObject.GetComponent<InputManager>();
        uim = tm.gameObject.GetComponent<UIManager>();
        mouseOverTiles = tm.gameObject.GetComponent<MouseOverTiles>();
        tileHolder = GameObject.FindGameObjectWithTag("TileHolder").GetComponent<TileHolder>();
        tm.players.Add(this);
        gameObject.tag = "Team" + myTeam.ToString();
        health = maxHealth;
        turnMeter = Random.Range(0, 301);
    }

    private void Start()
    {
        characterData = JSONreader.ReadJSON(characterJSON);

        characterSpriteX = characterSprite.localScale.x;
        if(myTeam == 1)
        {
            defaultDirection = characterSpriteX;
        }
        else
        {
            defaultDirection = characterSpriteX * -1;
        }
        FaceCharacter(null);
        pathfinding = GetComponent<Pathfinding>();
        playerMovement = GetComponent<PlayerMovement>();
        characterAbilities = GetComponent<CharacterAbilities>();
        healthBar = GetComponentInChildren<HealthBar>();
        actionPoints = actionPointMax;
        SetupInputs();
        actionPointCosts.Add(0);
        actionPointCosts.Add(movementCost);
        maxAbilityCooldowns.Add(0);
        maxAbilityCooldowns.Add(0);
        ultimateCD = maxUltimateCD;
        CalculateArmor();
    }

    private void Update()
    {
        if(tm.currentPlayer == this)
        {
            if (respawning)
            {
                TileScript mouseTile = mouseOverTiles.GetClickedTile();
                if (mouseTile != null && mouseTile.spawnPointTeam == myTeam)
                {
                    pathfinding.ResetTiles();
                    foreach (TileScript tile in tileHolder.tileArray)
                    {
                        if (tile.spawnPointTeam == myTeam)
                        {
                            tile.UpdateSelectionColor(3, true);
                        }
                    }
                    mouseTile.UpdateSelectionColor(3, false);
                }
            }
            else
            {
                if(activeAbility == 0)
                {
                    pathfinding.ResetTiles();
                }

                TileScript currentTile = pathfinding.GetCurrentTile();
                if(currentTile != null)
                {
                    currentTile.UpdateSelectionColor(1, true);
                }
            }
        }

        if(health <= 0 && !dead)
        {
            Death();
        }
    }

    public void FaceCharacter(Transform transformToFace)
    {
        if(transformToFace == null || transformToFace.position.x == transform.position.x)
        {
            characterSprite.localScale = new Vector3(defaultDirection, characterSprite.localScale.y, characterSprite.localScale.z);
            return;
        }

        if (transformToFace.position.x > transform.position.x)
        {
            characterSprite.localScale = new Vector3(characterSpriteX, characterSprite.localScale.y, characterSprite.localScale.z);
        }
        else
        {
            characterSprite.localScale = new Vector3(-characterSpriteX, characterSprite.localScale.y, characterSprite.localScale.z);
        }
    }

    public void StartTurn()
    {
        ActivateAbility(0);
        if(respawnTimer > 0)
        {
            respawnTimer--;
            if(respawnTimer <= 0)
            {
                respawning = true;
                foreach(TileScript tile in tileHolder.tileArray)
                {
                    if(tile.spawnPointTeam == myTeam)
                    {
                        tile.UpdateSelectionColor(3,true);
                    }
                } 
            }
        }

        if (!dead)
        {
            actionPoints = actionPointMax;
        }

        StatusEffects();

        characterAbilities.StartTurn();
        uim.SetupUIforTurn(this);
    }

    public void EndTurn()
    {
        characterAbilities.EndTurn();
    }

    public void TakeDamage(int damage)
    {
        if(damage > armor)
        {
            health -= damage - armor;
            if(health < 0)
            {
                health = 0;
            }
        }

        sleep = 0;

        healthBar.UpdateHealthbar();
    }

    public void GetHealed(int amount)
    {
        if (health < maxHealth)
        {
            health += amount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        healthBar.UpdateHealthbar();
        characterEvents.Heal();
    }

    public void ReduceUltimateCD()
    {
        if(ultimateCD > 0)
        {
            ultimateCD--;
        }
    }

    void Respawn()
    {
        if (!respawning)
        {
            return;
        }

        TileScript clickedTile = mouseOverTiles.GetClickedTile();
        if(clickedTile != null && clickedTile.spawnPointTeam == myTeam)
        {
            characterAbilities.Respawn();
            respawning = false;
            health = maxHealth;
            healthBar.UpdateHealthbar();
            actionPoints = actionPointMax;
            transform.position = clickedTile.transform.position;
            dead = false;
            pathfinding.ResetTiles();
            FaceCharacter(null);
        }
    }

    public void ActivateAbility(int abilityID)
    {
        if (!CanActivateAbility(abilityID))
        {
            return;
        }

        if(actionPoints >= actionPointCosts[abilityID])
        {
            activeAbility = abilityID;
            if(abilityID == 1)
            {
                movementAbility = true;
                playerMovement.ClearPath();
            }
            else
            {
                movementAbility = false;
            }
        }
    }

    bool CanActivateAbility(int abiltiyID)
    {
        if(abiltiyID >= abilityCooldowns.Length)
        {
            return false;
        }

        if (dead || stun > 0 || sleep > 0 || abilityCooldowns[abiltiyID] > 0)
        {
            return false;
        }

        if (silenced > 0 && silenceableAbilities.Contains(abiltiyID))
        {
            return false;
        }

        return true;
    }

    void StatusEffects()
    {
        greyedOutAbilities.Clear();

        if(rooted > 0)
        {
            greyedOutAbilities.Add(1);
            rooted--;
        }

        if(stun > 0)
        {
            stun--;
            if(stun > 0)
            {
                for(int i = 0; i < actionPointCosts.Count; i++)
                {
                    greyedOutAbilities.Add(i);
                }                
            }
        }

        if(sleep > 0)
        {
            sleep--;
            if(sleep > 0)
            {
                for (int i = 0; i < actionPointCosts.Count; i++)
                {
                    greyedOutAbilities.Add(i);
                }
            }
        }

        if(silenced > 0 || dayOfBlackSun)
        {
            silenced--;
            if(silenced > 0 || dayOfBlackSun)
            {
                for (int i = 0; i < actionPointCosts.Count; i++)
                {
                    if (silenceableAbilities.Contains(i))
                    {
                        greyedOutAbilities.Add(i);
                    }
                }
            }
            else
            {
                silenced = 0;
            }
        }

        if(frenzy > 0)
        {
            actionPoints += 1;
            frenzy--;
        }

        CalculateMoveSpeed();
        CalculateArmor();
    }

    public void CalculateArmor()
    {
        armor = naturalArmor;

        armor = characterAbilities.ArmorModifications(armor);

        if(livingShardplate && armor < statusLibrary.livingSharplateArmor)
        {
            armor = statusLibrary.livingSharplateArmor;
        }

        healthBar.UpdateArmorIcons();
    }

    public void CalculateMoveSpeed()
    {
        moveSpeedModifier = 0;
        if(speedBost > 0)
        {
            speedBost--;
            moveSpeedModifier++;
        }
    }

    public void NextTurn()
    {
        if (respawning)
        {
            return;
        }

        if(frenzy <= 0)
        {
            characterEvents.Frenzy(false);
        }

        turnMeter = 0;
        healthBar.UpdateTurnMeterBar();
        ActivateAbility(0);
        pathfinding.ResetTiles();
        tm.NextTurn();
    }

    public void IncrementTurnMeter()
    {
        turnMeter += tacticalSpeed;
        healthBar.UpdateTurnMeterBar();
    }

    public void IncreaseTurnMeter(int amount)
    {
        turnMeter += amount;
        healthBar.UpdateTurnMeterBar();
        characterEvents.GainTurnMeter();
    }

    public void DecreaseTurnMeter(int amount)
    {
        turnMeter -= amount;
        if(turnMeter < 0)
        {
            turnMeter = 0;
        }
        healthBar.UpdateTurnMeterBar();
        characterEvents.LoseTurnMeter();
    }

    void SetupInputs()
    {
        im.controls[tm.players.IndexOf(this)].PlayerTurn.DeselectAbility.performed += ctx => ActivateAbility(0);
        im.controls[tm.players.IndexOf(this)].PlayerTurn.One.performed += ctx => ActivateAbility(1);
        im.controls[tm.players.IndexOf(this)].PlayerTurn.LeftClick.performed += ctx => Respawn();
        im.controls[tm.players.IndexOf(this)].PlayerTurn.EndTurn.performed += ctx => NextTurn();
    }

    void Death()
    {
        TileScript currentTile = pathfinding.GetCurrentTile();
        currentTile.occupied = false;
        currentTile.occupation = currentTile.gameObject;
        respawnTimer = respawnTime;
        dead = true;
        transform.position = hell;
    }
}
