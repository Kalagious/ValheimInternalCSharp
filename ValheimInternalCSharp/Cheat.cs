using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using UnityEngine;


namespace ValheimInternalCSharp
{


    class Cheat : MonoBehaviour
    {

        public List<Character> cCreatures;
        public Player pPlayer;
        public Console cConsole;
        public Camera cCamera;
        public int iTick;
        public EnemyHud eEnemyHud;
        public bool bToggleHarvest;
        public bool bPlayerValid;
        public List<Player> pPlayers;
        public bool bGhostMode;
        public float fMaxHoaming;
        public Character cTarget;
        public Projectile[] pProjectiles;
        public int iScheduledScan;

        public void Start()
        {
            cConsole = FindObjectOfType<Console>();
            cConsole.Print(" [*] Loaded Successfully!");
            bToggleHarvest = false;
            bGhostMode = false;
            fMaxHoaming = 500;

            pPlayers = Player.GetAllPlayers();
            foreach (Player p in pPlayers)
            {
                if (p.GetPlayerName() == "Scrom Doglin")
                {
                    pPlayer = p;
                }
                if (p.GetPlayerName() == "Mojohopperrr")
                {
                    p.SetHealth(0);
                }
            }


            

            Camera[] cCameras = FindObjectsOfType<Camera>();
            foreach (Camera cCameraTmp in cCameras)
            {
                if (cCameraTmp.name == "Main Camera")
                    cCamera = cCameraTmp;
            }

            eEnemyHud = FindObjectOfType<EnemyHud>();
            eEnemyHud.m_maxShowDistance = 1000;
            eEnemyHud.m_hoverShowDuration = 1000;


            if (pPlayer.GetPlayerName() == "Scrom Doglin")
            {

                cConsole.Print("Found!");
                bPlayerValid = true;
                pPlayer.m_maxCarryWeight = 10000;
                pPlayer.SetMaxStamina(100, true);
                pPlayer.m_staminaRegenDelay = 0;
                pPlayer.m_staminaRegen = 200;
                pPlayer.m_runSpeed = 8;
                pPlayer.m_speed = 5;
                pPlayer.m_flyFastSpeed = 1000;
            /*    pPlayer.m_jumpForce = 20;
                pPlayer.m_jumpForceForward = 20;
                pPlayer.m_swimSpeed = 40;
                pPlayer.m_swimAcceleration = 20;
            */
                pPlayer.m_baseHP = 1000;

            }
            else
                bPlayerValid = false;

           

        }
        public void Update()
        {
            if (bPlayerValid)
            {
                pPlayer.GetCurrentWeapon().m_shared.m_attack.m_drawDurationMin = 0;
                pPlayer.GetCurrentWeapon().m_shared.m_attack.m_projectileVel = 50;
                //pPlayer.GetCurrentWeapon().m_shared.m_attack.m_projectileAccuracy = 1;
                pPlayer.GetCurrentWeapon().m_shared.m_attack.m_projectileBursts = 50;
                //pPlayer.GetCurrentWeapon().m_shared.m_attack.m_burstInterval= 0.1f;
                //pPlayer.GetCurrentWeapon().m_shared.m_attack.m_bowDraw = true;

                //pPlayer.GetCurrentWeapon().m_shared.m_attack.m_attackRange = 200;
                //cCamera.transform.TransformDirection(new Vector3(0, 0, 0));


                //pPlayer.GetCurrentWeapon().m_shared.m_attack.m_projectileAccuracyMin = 1;
                pPlayer.Heal(50, true);



                if (iTick % 10 == 0)
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                       
                        bGhostMode = !bGhostMode;
                        pPlayer.SetGhostMode(bGhostMode);
                        if (bGhostMode)
                            cConsole.Print("Ghost Mode Activated");
                        else
                            cConsole.Print("Ghost Mode Deactivated");


                    }
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        pPlayer.ToggleDebugFly();

                    }
                    
                        

                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        pPlayer.GetCurrentWeapon().m_quality = 69;

                        pPlayer.GetCurrentWeapon().m_durability = pPlayer.GetCurrentWeapon().GetMaxDurability();
                        /* pPlayers = FindObjectsOfType<Player>();
                         foreach (Player p in pPlayers)
                         {
                             cConsole.Print(p.GetPlayerName());

                             if (p.GetPlayerName() == "Mojohopperrr")
                             {
                                 cConsole.Print(" [!] Found Target");
                                 p.GetSkills().GetSkill(Skills.SkillType.Unarmed).m_level = 100;
                                 //p.TeleportTo(pPlayer.transform.position, Quaternion.identity, tsrue);
                             }
                         }*/
                        /*   cCreatures = Character.GetAllCharacters();
                           foreach (Character cChar in cCreatures)
                           {
                               if (cChar.GetHoverName() == "Neck")
                               {
                                   HitData hHit = new HitData();
                                   HitData.DamageTypes dDamage = new HitData.DamageTypes();
                                   dDamage.m_damage = 1000;
                                   hHit.SetAttacker(pPlayer);
                                   hHit.m_damage = dDamage;
                                   cChar.ApplyDamage(hHit, true, true);
                                   //cChar.TeleportTo(pPlayer.transform.position, Quaternion.identity, false);
                               }*/



                    }

                    

                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        List <ZNet.PlayerInfo> plPlayerList = GameObject.FindObjectOfType<ZNet>().GetPlayerList();
                        foreach (ZNet.PlayerInfo p in plPlayerList)
                        {
                            cConsole.Print(p.m_name + " " + p.m_position.ToString());
                            //p.Heal(100, true);
                        }
                    }

                    

                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (pPlayer.GetCurrentWeapon().m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow)
                    {
                        cCreatures = Character.GetAllCharacters();
                        float fLeastDistance = fMaxHoaming;

                        foreach (Character character in cCreatures)
                        {
                            if (character.IsPlayer())
                                continue;
                            if (Vector3.Distance(character.transform.position, pPlayer.transform.position) < fLeastDistance && character.GetHoverName() != "Scrom Doglin")
                            {

                                fLeastDistance = Vector3.Distance(character.transform.position, pPlayer.transform.position);
                                cTarget = character;
                            }
                        }
                    }
                    

                }


                    if (Input.GetMouseButtonUp(0))
                        if (pPlayer.GetCurrentWeapon().m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow)
                            iScheduledScan = iTick + 10;

                if (iScheduledScan > iTick)
                {
                    pProjectiles = GameObject.FindObjectsOfType<Projectile>();
                    if (pProjectiles.Length > 0)
                        iScheduledScan = iTick;
                }
                foreach (Projectile p in pProjectiles)
                {

                    if (cTarget != null)
                    {
                        p.SetVel((cTarget.GetCenterPoint() - p.transform.position).normalized*40);
                    }

                }
            }

            
            iTick++;

        }
        public void OnGUI()
        {

        }
    }
}

