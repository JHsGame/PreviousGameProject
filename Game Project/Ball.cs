using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Ball : MonoBehaviour
{
    [Header("히어로 부분")]
    public int i_HeroType = 0; //0=기본, 1~히어로(csv 순서)
    public bool b_zeroDMG = false;
    public int i_firerate = 0;
    public List<Transform> tf_Muzzle;
    public List<Transform> tf_forHero22;
    [Header("------------")]

    public Material[] m_StageMaterial = new Material[5];

    public bool Attacked;
    public bool b_NaviOn;
    public float Damage;
    public float f_CharDamage;
    public int i_ListNum;
    public string sfxName_NormalHit;
    public string sfxName_CriHit;
    public int i_bounceCount = 0;
    public int i_Ability = 0;
    public int i_WeaponNUM;

    public Rigidbody myrigid;
    private NavMeshAgent myNav;

    public bool attack = false;
    public GameObject go_HitOBJ;
    public bool b_Head = false;
    public bool b_OneTouch = false;
    public bool b_isworkingCoroutine = false;
    public List<Vector3> vec_Myvelocity = new List<Vector3>();
    public List<Vector3> vec_MyPos = new List<Vector3>();
    public List<GameObject> go_HitObj;
    public List<GameObject> go_ColEnter;

    private Coroutine c_LaunchBallCoroutine;
    //public List<Vector3> testlist;

    void Awake()
    {
        i_ListNum = 99999;
        myrigid = transform.GetComponent<Rigidbody>();
        myNav = transform.GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        i_bounceCount = 0;
        Attacked = false;
        b_Head = false;
        go_HitOBJ = null;
        if (ChangeManager_Scr.instance.i_CharType != 4)
        {
            f_CharDamage = 0;
        }
        else
        {
            f_CharDamage = Damage * ((int)ChangeManager_Scr.instance.Script.PlayerUpgradeData[ChangeManager_Scr.instance.Script.i_nowUpgradeSteps]["Human_resources"] * 0.01f);
            f_CharDamage = Mathf.RoundToInt(f_CharDamage);
        }

        if(Inventory_Scr.instance.AddAttack > 0)
        {
            f_CharDamage += Inventory_Scr.instance.AddAttack;
        }

        switch (i_HeroType)
        {
            case 1:
            case 3:
            case 15:
                float plusDamage = ((int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"]) * 0.01f;
                if((ManPowerManager_Scr.instance.BoughtINmanList.Count - ManPowerManager_Scr.instance.heroList.Count) > 0)
                {
                    Damage = (int)((ManPowerManager_Scr.instance.i_AllAtk / (ManPowerManager_Scr.instance.BoughtINmanList.Count - ManPowerManager_Scr.instance.heroList.Count)) * plusDamage);
                }
                else
                {
                    Damage = 1;
                }
                break;


            case 5:
            case 11:
            case 17:
                c_LaunchBallCoroutine = StartCoroutine(longrangeattack());
                float plusDamage2 = ((int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"]) * 0.01f;


                if ((ManPowerManager_Scr.instance.BoughtINmanList.Count - ManPowerManager_Scr.instance.heroList.Count) > 0)
                {
                    Damage = (int)((ManPowerManager_Scr.instance.i_AllAtk / (ManPowerManager_Scr.instance.BoughtINmanList.Count - ManPowerManager_Scr.instance.heroList.Count)) * plusDamage2);
                }
                else
                {
                    Damage = 1;
                }
                break;

            case 6:
            case 12:
            case 18:
                c_LaunchBallCoroutine = StartCoroutine(longrangeattack());
                Damage = 0;
                i_Ability = (int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"];
                break;

            case 2:
            case 4:
            case 16:
                Damage = 0;
                i_Ability = (int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"];
                break;

            case 7:
            case 9:
            case 13:
            case 19:
                if ((ManPowerManager_Scr.instance.BoughtINmanList.Count - ManPowerManager_Scr.instance.heroList.Count) > 0)
                {
                    Damage = (int)(ManPowerManager_Scr.instance.i_AllAtk / (ManPowerManager_Scr.instance.BoughtINmanList.Count - ManPowerManager_Scr.instance.heroList.Count));
                }
                else
                {
                    Damage = 1;
                }
                i_Ability = 0;
                break;

            case 8:
            case 10:
            case 14:
            case 20:
                Damage = 0;
                i_Ability = 0;
                break;

            case 21:
            case 22:
                Damage = 99999;
                break;
        }
    }

    private void OnDisable()
    {
        attack = false;
        i_ListNum = 99999;
        b_OneTouch = false;
        go_HitObj.Clear();
    }

    public void deletList()
    {
        go_ColEnter.Clear();
        vec_Myvelocity.Clear();
        vec_MyPos.Clear();
        // go_HitObj.Clear();
    }

    private void FixedUpdate()
    {
        if (!b_NaviOn)
        {
            myrigid.velocity = myrigid.velocity.normalized * Player_Input.instance.f_Acceleration;
            if (transform.position.y != 0.05f)
                transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);


            myNav.enabled = false;
            float angle = Mathf.Atan2(myrigid.velocity.x, myrigid.velocity.z);
            //transform.localEulerAngles = new Vector3(0, (angle * 180) / Mathf.PI, 90);
            transform.rotation = Quaternion.Euler(new Vector3(0, (angle * 180) / Mathf.PI, 0));
        }
    }

    void Update()
    {
        if (!attack)
        {
            deletList();
        }

        if (i_ListNum == 0)
        {
            b_Head = true;
        }

        if (Button_Option.instance.b_Golobby)
        {
            transform.position = transform.parent.position;
            myrigid.velocity = Vector3.zero;
            myrigid.rotation = Quaternion.Euler(Vector3.zero);
            myrigid.inertiaTensor = new Vector3(1, 1, 1);
            myrigid.inertiaTensorRotation = Quaternion.Euler(new Vector3(1, 1, 1));
            Attacked = false;
            gameObject.SetActive(false);
        }

        if (b_NaviOn)
        {
            deletList();
            myrigid.velocity = Vector3.forward;
            myNav.enabled = true;
            myNav.destination = Player_Input.instance.tf_NavidestinationTs.position;

            if (b_isworkingCoroutine)
            {
                StopCoroutine(c_LaunchBallCoroutine);
                b_isworkingCoroutine = false;
            }
        }
    }


    IEnumerator longrangeattack()
    {
        WaitForSeconds delay = new WaitForSeconds(1f / i_firerate);

        b_isworkingCoroutine = true;

        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            int MuzzleCount = 0;
            int childCount = ManPowerManager_Scr.instance.BulletGroup[i_WeaponNUM].childCount;

            for (int i = 0; i < childCount; i++)
            {
                if (!ManPowerManager_Scr.instance.BulletGroup[i_WeaponNUM].GetChild(i).gameObject.activeSelf)
                {
                    GameObject obj = ManPowerManager_Scr.instance.BulletGroup[i_WeaponNUM].GetChild(i).gameObject;
                    obj.transform.GetComponent<Bullet>().Ball_Scr = this;
                    obj.transform.GetComponent<Bullet>().tf_Muzzle = tf_Muzzle[MuzzleCount];
                    MuzzleCount++;
                    obj.SetActive(true);
                    if (MuzzleCount >= tf_Muzzle.Count)
                    {
                        break;
                    }
                }
            }

            if (MuzzleCount < tf_Muzzle.Count)
            {
                int tmp = tf_Muzzle.Count - MuzzleCount;
                for (int j = 0; j < tmp; j++)
                {
                    //bullet 새로 생성해서 발사하기 
                    GameObject obj = Instantiate(ManPowerManager_Scr.instance.g_Bullet[i_WeaponNUM], ManPowerManager_Scr.instance.BulletGroup[i_WeaponNUM].transform);
                    //    obj.transform.SetParent(ManPowerManager_Scr.instance.BulletGroup.transform);
                    obj.transform.GetComponent<Bullet>().Ball_Scr = this;
                    obj.transform.GetComponent<Bullet>().tf_Muzzle = tf_Muzzle[MuzzleCount];
                    MuzzleCount++;
                    obj.SetActive(true);
                }
            }

            yield return delay;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        go_ColEnter.Add(collision.gameObject);

        b_OneTouch = false;

        attack = true;

        if (collision.transform.CompareTag("Block_Top") ||
                collision.transform.CompareTag("Block_Middle") ||
                collision.transform.CompareTag("Block_Low") ||
                collision.transform.CompareTag("Wall"))
        {
            Attacked = true;
        }
        if (collision.transform.CompareTag("Block_Top") ||
            collision.transform.CompareTag("Block_Middle") ||
            collision.transform.CompareTag("Block_Low"))
        {
            if(i_HeroType != 22)
            {
                BlockControl healthManager = collision.transform.GetComponent<BlockControl>();
                healthManager.go_HitBall = transform.gameObject;
                if (healthManager && healthManager.f_HitPoint > 0)
                {

                    int MinCri= 0;

                    if (ChangeManager_Scr.instance.i_CharType != 5)
                    {
                        MinCri = (int)Inventory_Scr.instance.AddCritical;
                    }
                    else
                    {
                        MinCri = (int)ChangeManager_Scr.instance.Script.PlayerUpgradeData[ChangeManager_Scr.instance.Script.i_nowUpgradeSteps]["Promotion"] + (int)Inventory_Scr.instance.AddCritical;
                    }

                    int Criresult = Random.Range(1, 101); 
                    
                    if (Criresult <= MinCri)
                    {
                        SoundManager_sfx.instance.PlaySE(sfxName_CriHit, false);
                        switch (i_HeroType)
                        {
                            case 2:
                            case 3:
                            case 9:
                            case 6:
                            case 8:
                            case 11:
                            case 12:
                            case 14:
                            case 17:
                            case 18:
                            case 20:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                            case 4:
                            case 10:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                break;
                            default:
                                healthManager.ApplyDamage((Damage + f_CharDamage) * 2, true, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                        }

                    }
                    else
                    {
                        SoundManager_sfx.instance.PlaySE(sfxName_NormalHit, false);
                        switch (i_HeroType)
                        {

                            case 2:
                            case 3:
                            case 9:
                            case 6:
                            case 8:
                            case 11:
                            case 12:
                            case 14:
                            case 17:
                            case 18:
                            case 20:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                            case 4:
                            case 10:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                break;

                            default:
                                healthManager.ApplyDamage((Damage + f_CharDamage), false, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                        }

                    }

                    if (healthManager.f_HitPoint <= 0)
                    {
                        Player_Input.instance.OpenNewhead();
                        healthManager.setDir(transform.position);

                        CopyCollisionExit(collision);
                        if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count)
                        {
                            Ball script = Player_Input.instance.ShotBall[i_ListNum + 1].transform.GetComponent<Ball>();
                            script.b_Head = true;
                        }

                    }
                    switch (i_HeroType)
                    {
                        case 7:
                        case 9:
                        case 13:
                        case 17:
                        case 19:
                            Damage += (int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"]; ;
                            break;

                        case 8:
                        case 10:
                        case 14:
                        case 18:
                        case 20:
                            i_Ability += (int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"]; ;
                            break;

                        case 21:
                        case 22:
                            touchGate();
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < tf_forHero22.Count; i++)
                {
                    BlockControl healthManager = tf_forHero22[i].GetComponent<BlockControl>();



                    healthManager.go_HitBall = transform.gameObject;
                    if (healthManager && healthManager.f_HitPoint > 0)
                    {

                        if (ChangeManager_Scr.instance.i_CharType != 5)
                        {
                            SoundManager_sfx.instance.PlaySE(sfxName_NormalHit, false);

                            switch (i_HeroType)
                            {
                                case 3:
                                    healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                    break;
                                case 4:
                                    healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                    break;
                                case 9:
                                    healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                    break;
                                case 10:
                                    healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                    break;

                                default:
                                    healthManager.ApplyDamage((Damage + f_CharDamage), false, i_bounceCount, false, i_Ability, i_HeroType);
                                    break;
                            }
                        }
                        else
                        {
                            int MinCri = (int)ChangeManager_Scr.instance.Script.PlayerUpgradeData[ChangeManager_Scr.instance.Script.i_nowUpgradeSteps]["Promotion"];

                            int Criresult = Random.Range(1, 101);
                            if (Criresult <= MinCri)
                            {
                                SoundManager_sfx.instance.PlaySE(sfxName_CriHit, false);
                                switch (i_HeroType)
                                {
                                    case 3:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                        break;
                                    case 4:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                        break;
                                    case 9:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                        break;
                                    case 10:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                        break;

                                    default:
                                        healthManager.ApplyDamage((Damage + f_CharDamage) * 2, true, i_bounceCount, false, i_Ability, i_HeroType);
                                        break;
                                }
                            }
                            else
                            {
                                SoundManager_sfx.instance.PlaySE(sfxName_NormalHit, false);
                                switch (i_HeroType)
                                {
                                    case 3:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                        break;
                                    case 4:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                        break;
                                    case 9:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                        break;
                                    case 10:
                                        healthManager.ApplyDamage(0, false, i_bounceCount, false, -1, i_HeroType);
                                        break;

                                    default:
                                        healthManager.ApplyDamage((Damage + f_CharDamage), false, i_bounceCount, false, i_Ability, i_HeroType);
                                        break;
                                }
                            }
                        }


                        if (healthManager.f_HitPoint <= 0)
                        {
                            Player_Input.instance.OpenNewhead();
                            healthManager.setDir(transform.position);

                            CopyCollisionExit(collision);
                            if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count)
                            {
                                Ball script = Player_Input.instance.ShotBall[i_ListNum + 1].transform.GetComponent<Ball>();
                                script.b_Head = true;
                            }

                        }
                        switch (i_HeroType)
                        {
                            case 7:
                            case 9:
                            case 13:
                            case 17:
                            case 19:
                                Damage += (int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"]; ;
                                break;

                            case 8:
                            case 10:
                            case 14:
                            case 18:
                            case 20:
                                i_Ability += (int)Bag_Hero_ItmeCSV.instance.HeroData[i_HeroType - 1]["{0}"]; ;
                                break;

                            case 21:
                            case 22:
                                touchGate();
                                break;
                        }
                    }
                }
            }
        }
    }

    public void CopyCollisionExit(Collision collision)
    {
        if (transform.gameObject.activeSelf && !b_OneTouch)
        {
            if (!b_NaviOn)
            {/*
                if (go_ColEnter.Count == 1)
                {
                    go_HitOBJ = collision.gameObject;
                }
                else if (go_ColEnter.Count == 2)
                {
                    if (go_ColEnter[0].transform.GetComponent<PhysicManager>().i_RespawnNum > go_ColEnter[1].transform.GetComponent<PhysicManager>().i_RespawnNum)
                    {
                        go_HitOBJ = go_ColEnter[0];
                    }
                    else
                    {
                        go_HitOBJ = go_ColEnter[1];
                    }
                }
                else if (go_ColEnter.Count > 2)
                {
                    if (go_ColEnter[0].transform.GetComponent<PhysicManager>().i_RespawnNum > go_ColEnter[1].transform.GetComponent<PhysicManager>().i_RespawnNum)
                    {
                        go_HitOBJ = go_ColEnter[0];
                    }
                    else
                    {
                        go_HitOBJ = go_ColEnter[1];
                    }

                    if (go_HitOBJ.transform.GetComponent<PhysicManager>().i_RespawnNum < go_ColEnter[2].transform.GetComponent<PhysicManager>().i_RespawnNum)
                    {
                        go_HitOBJ = go_ColEnter[2];
                    }
                }
                */
                if (!b_Head)
                {
                    Ball script = Player_Input.instance.ShotBall[i_ListNum - 1].transform.GetComponent<Ball>();


                    if (script.vec_MyPos.Count > 0 &&
                        (script.vec_MyPos[0].x + 0.5f >= transform.position.x && script.vec_MyPos[0].x - 0.5f <= transform.position.x) &&
                        (script.vec_MyPos[0].z + 0.5f >= transform.position.z && script.vec_MyPos[0].z - 0.5f <= transform.position.z))
                    {
                        if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count)
                        {
                            vec_MyPos.Add(script.vec_MyPos[0]);
                            vec_Myvelocity.Add(script.vec_Myvelocity[0]);
                            // go_HitObj.Add(go_HitOBJ);
                        }

                        transform.position = script.vec_MyPos[0];
                        myrigid.velocity = script.vec_Myvelocity[0];
                        script.vec_MyPos.RemoveAt(0);
                        script.vec_Myvelocity.RemoveAt(0);
                        //script.go_HitObj.RemoveAt(0);

                        Attacked = true;
                    }
                    else
                    {
                        if (Player_Input.instance.i_BlockStreak <= 0 && Player_Input.instance.b_openhead)
                        {
                            /*
                            if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count && script.vec_MyPos.Count > 0)
                            {
                                vec_MyPos.Add(script.vec_MyPos[0]);
                                vec_Myvelocity.Add(script.vec_Myvelocity[0]);
                                go_HitObj.Add(go_HitOBJ); 
                            }

                            if(script.vec_MyPos.Count > 0)
                            {

                                transform.position = script.vec_MyPos[0];
                                myrigid.velocity = script.vec_Myvelocity[0];
                                script.vec_MyPos.RemoveAt(0);
                                script.vec_Myvelocity.RemoveAt(0);
                                script.go_HitObj.RemoveAt(0);
                            }*/

                            if (script.vec_MyPos.Count > 0)
                            {
                                if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count)
                                {
                                    vec_MyPos.Add(script.vec_MyPos[0]);
                                    vec_Myvelocity.Add(script.vec_Myvelocity[0]);
                                    // go_HitObj.Add(go_HitOBJ);
                                }

                                transform.position = script.vec_MyPos[0];
                                myrigid.velocity = script.vec_Myvelocity[0];
                                script.vec_MyPos.RemoveAt(0);
                                script.vec_Myvelocity.RemoveAt(0);
                                // script.go_HitObj.RemoveAt(0);

                            }
                            else
                            {
                                b_Head = true;
                                vec_MyPos.Add(transform.position);
                                vec_Myvelocity.Add(myrigid.velocity.normalized);
                            }
                            Attacked = true;
                        }
                        else
                        {
                            b_Head = true;
                            vec_MyPos.Add(transform.position);
                            vec_Myvelocity.Add(myrigid.velocity.normalized);
                            //  go_HitObj.Add(go_HitOBJ);
                        }
                    }
                }
                else if (b_Head)
                {
                    vec_MyPos.Add(transform.position);
                    vec_Myvelocity.Add(myrigid.velocity.normalized);
                    // go_HitObj.Add(go_HitOBJ);
                }
            }
            b_OneTouch = true;
            go_ColEnter.Clear();
            i_bounceCount++;
            go_HitObj.Add(collision.gameObject);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (transform.gameObject.activeSelf && !b_OneTouch)
        {
            if (!b_NaviOn)
            {
                /*
                if (go_ColEnter.Count == 1)
                {
                    go_HitOBJ = collision.gameObject;
                }
                else if (go_ColEnter.Count == 2)
                {
                    if (go_ColEnter[0].transform.GetComponent<PhysicManager>().i_RespawnNum > go_ColEnter[1].transform.GetComponent<PhysicManager>().i_RespawnNum)
                    {
                        go_HitOBJ = go_ColEnter[0];
                    }
                    else
                    {
                        go_HitOBJ = go_ColEnter[1];
                    }
                }
                else if (go_ColEnter.Count > 2)
                {
                    if (go_ColEnter[0].transform.GetComponent<PhysicManager>().i_RespawnNum > go_ColEnter[1].transform.GetComponent<PhysicManager>().i_RespawnNum)
                    {
                        go_HitOBJ = go_ColEnter[0];
                    }
                    else
                    {
                        go_HitOBJ = go_ColEnter[1];
                    }

                    if (go_HitOBJ.transform.GetComponent<PhysicManager>().i_RespawnNum < go_ColEnter[2].transform.GetComponent<PhysicManager>().i_RespawnNum)
                    {
                        go_HitOBJ = go_ColEnter[2];
                    }
                }
                */
                if (!b_Head)
                {
                    Ball script = Player_Input.instance.ShotBall[i_ListNum - 1].transform.GetComponent<Ball>();


                    if (script.vec_MyPos.Count > 0 &&
                        (script.vec_MyPos[0].x + 0.5f >= transform.position.x && script.vec_MyPos[0].x - 0.5f <= transform.position.x) &&
                        (script.vec_MyPos[0].z + 0.5f >= transform.position.z && script.vec_MyPos[0].z - 0.5f <= transform.position.z))
                    {
                        if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count)
                        {
                            vec_MyPos.Add(script.vec_MyPos[0]);
                            vec_Myvelocity.Add(script.vec_Myvelocity[0]);
                            // go_HitObj.Add(go_HitOBJ);
                        }

                        transform.position = script.vec_MyPos[0];
                        myrigid.velocity = script.vec_Myvelocity[0];
                        script.vec_MyPos.RemoveAt(0);
                        script.vec_Myvelocity.RemoveAt(0);
                        //script.go_HitObj.RemoveAt(0);

                        Attacked = true;
                    }
                    else
                    {
                        if (Player_Input.instance.i_BlockStreak <= 0 && Player_Input.instance.b_openhead)
                        {
                            /*
                            if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count && script.vec_MyPos.Count > 0)
                            {
                                vec_MyPos.Add(script.vec_MyPos[0]);
                                vec_Myvelocity.Add(script.vec_Myvelocity[0]);
                                go_HitObj.Add(go_HitOBJ); 
                            }

                            if(script.vec_MyPos.Count > 0)
                            {

                                transform.position = script.vec_MyPos[0];
                                myrigid.velocity = script.vec_Myvelocity[0];
                                script.vec_MyPos.RemoveAt(0);
                                script.vec_Myvelocity.RemoveAt(0);
                                script.go_HitObj.RemoveAt(0);
                            }*/

                            if (script.vec_MyPos.Count > 0)
                            {
                                if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count)
                                {
                                    vec_MyPos.Add(script.vec_MyPos[0]);
                                    vec_Myvelocity.Add(script.vec_Myvelocity[0]);
                                    // go_HitObj.Add(go_HitOBJ);
                                }

                                transform.position = script.vec_MyPos[0];
                                myrigid.velocity = script.vec_Myvelocity[0];
                                script.vec_MyPos.RemoveAt(0);
                                script.vec_Myvelocity.RemoveAt(0);
                                // script.go_HitObj.RemoveAt(0);

                            }
                            else
                            {
                                b_Head = true;
                                vec_MyPos.Add(transform.position);
                                vec_Myvelocity.Add(myrigid.velocity.normalized);
                            }
                            Attacked = true;
                        }
                        else
                        {
                            b_Head = true;
                            vec_MyPos.Add(transform.position);
                            vec_Myvelocity.Add(myrigid.velocity.normalized);
                            //  go_HitObj.Add(go_HitOBJ);
                        }
                    }
                }
                else if (b_Head)
                {
                    vec_MyPos.Add(transform.position);
                    vec_Myvelocity.Add(myrigid.velocity.normalized);
                    // go_HitObj.Add(go_HitOBJ);
                }
            }
            b_OneTouch = true;
            go_ColEnter.Clear();
            i_bounceCount++;
            go_HitObj.Add(collision.gameObject);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if ((Attacked && other.CompareTag("Gate_Ball")) || Attacked && other.CompareTag("Gate_End"))
        {

            touchGate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Block_Top") ||
            other.transform.CompareTag("Block_Middle") ||
            other.transform.CompareTag("Block_Low"))
        {
            if(i_HeroType != 22)
            {
                BlockControl healthManager = other.transform.GetComponent<BlockControl>();
                //   healthManager.go_HitBall = transform.gameObject;
                if (healthManager && healthManager.f_HitPoint > 0)
                {
                    int MinCri = 0;

                    if (ChangeManager_Scr.instance.i_CharType != 5)
                    {
                        MinCri = (int)Inventory_Scr.instance.AddCritical;
                    }
                    else
                    {
                        MinCri = (int)ChangeManager_Scr.instance.Script.PlayerUpgradeData[ChangeManager_Scr.instance.Script.i_nowUpgradeSteps]["Promotion"] + (int)Inventory_Scr.instance.AddCritical;
                    }

                    int Criresult = Random.Range(1, 101);

                    if (Criresult <= MinCri)
                    {
                        SoundManager_sfx.instance.PlaySE(sfxName_CriHit, false);
                        switch (i_HeroType)
                        {
                            case 3:
                                healthManager.ApplyDamage((Damage + f_CharDamage) * 2, false, i_bounceCount, false, -1, i_HeroType);
                                break;
                            case 4:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                            case 9:
                                healthManager.ApplyDamage((Damage + f_CharDamage) * 2, false, i_bounceCount, false, -1, i_HeroType);
                                break;
                            case 10:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                        }
                    }
                    else
                    {
                        SoundManager_sfx.instance.PlaySE(sfxName_NormalHit, false);
                        switch (i_HeroType)
                        {
                            case 3:
                                healthManager.ApplyDamage((Damage + f_CharDamage), false, i_bounceCount, false, -1, i_HeroType);
                                break;
                            case 4:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                            case 9:
                                healthManager.ApplyDamage((Damage + f_CharDamage), false, i_bounceCount, false, -1, i_HeroType);
                                break;
                            case 10:
                                healthManager.ApplyDamage(0, false, i_bounceCount, false, i_Ability, i_HeroType);
                                break;
                        }

                    }

                    if (healthManager.f_HitPoint <= 0)
                    {
                        Player_Input.instance.OpenNewhead();
                        healthManager.setDir(transform.position);


                        if (healthManager.go_HitBall != null)
                        {
                            //CopyCollisionExit(other);
                            if (i_ListNum + 1 < Player_Input.instance.ShotBall.Count)
                            {
                                Ball script = Player_Input.instance.ShotBall[i_ListNum + 1].transform.GetComponent<Ball>();
                                script.b_Head = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!tf_forHero22.Contains(other.transform))
                {
                    tf_forHero22.Add(other.transform);

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Block_Top") ||
            other.transform.CompareTag("Block_Middle") ||
            other.transform.CompareTag("Block_Low"))
        {
            if (i_HeroType == 22)
            {
                if (tf_forHero22.Contains(other.transform))
                {
                    tf_forHero22.Remove(other.transform);

                }
            }
        }
    }

    void touchGate()
    {
        transform.GetComponent<NavMeshAgent>().enabled = false;
        myrigid.velocity = Vector3.zero;
        myrigid.rotation = Quaternion.Euler(Vector3.zero);
        Player_Input.instance.i_GetBackBallCount++;
        if (!Player_Input.instance.b_isNaviOn && Player_Input.instance.i_GetBackBallCount == 1)
        {
            if (transform.position.x <= -2.6f)
            {
                Player_Input.instance.v_PlayerPos = new Vector3(-2.6f, 2f, -9f);
            }
            else if (transform.position.x >= 2.6f)
            {
                Player_Input.instance.v_PlayerPos = new Vector3(2.6f, 2f, -9f);
            }
            else
            {
                Player_Input.instance.v_PlayerPos = new Vector3(transform.position.x, 2f, -9f);
            }
        }
        Attacked = false;
        b_NaviOn = false;
        myrigid.inertiaTensor = new Vector3(1, 1, 1);
        myrigid.inertiaTensorRotation = Quaternion.Euler(new Vector3(1, 1, 1));
        gameObject.SetActive(false);
    }
}