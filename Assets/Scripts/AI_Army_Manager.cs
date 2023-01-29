using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* AI Army Manager is the AI "controle panel" that the AI use to send comends and get fied back form its units.
 * 
 */

public class AI_Army_Manager : MonoBehaviour {
    public Manager AI_manger,Enemy_manger;
    private AI_Min_Max AI;
    public PathTree paths;
    public AI_Logic_Analyzer logic;
    public GameObject point;
    public bool spone, capture, attack;

    private float time = 0;

    public float pereod_time = 1.5f ;

    private void Awake()
    {
        AI = new AI_Min_Max();
        AI_manger.Enemys = Enemy_manger.Army;
    }


    void tree_constracter(PathNode _node ,int layer,ushort _i)
    {
        if (layer <= 0)
        {
            return;
        }
        else
        {
            if(spone)
                Unit_Construction(_node);
            if(capture)
                Point_Of_Intrest(_node); 
            if(attack)
                Enemy_Activety(_node, _i);

            int count = _node.pCount;
            for(int i = 0;i < count; i++)
            {
                tree_constracter(_node.pList[i], layer-1, _i++);
            }
        }
    }

    void tree_constracter(PathNode _node, int layer, ushort _i, GameObject _unit)
    {
        if (layer <= 0)
        {
            return;
        }
        else
        {
            Point_Of_Intrest(_node);
            Enemy_Activety(_node, _i);

            int count = _node.pCount;
            for (int i = 0; i < count; i++)
            {
                tree_constracter(_node.pList[i], layer - 1, _i++, _unit);
            }
        }
    }

    int dis_value(Inc_Block inc)
    {
        return (int)logic.func1((int)Vector3.Distance(inc.Positon, inc.Target.transform.position), logic.max , logic.DisPriority);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time > pereod_time)
        {
            Test();
            time = 0;
        }
    }

    public void Test()
    {
        paths = new PathTree();
        tree_constracter(paths.Head, 1, 0);
        paxe test = AI.findPath(paths.Head);
        if(test != null)
            if(test.Inc != null)
              execute_comende(test);
    }

    //execute_comende translate the comend value to the upropriate comend and execute it
    void execute_comende(paxe _a) 
    {
        switch ((int)_a.Inc.comande)
        {
            case 0:
                break;
            case 1:

                break;
            case 2:
                CaptureCommend(_a.Inc.Unit, _a.Inc.Target);
                break;
            case 3:
                AttackCommend(_a.Inc.Unit, _a.Inc.Target);
                break;
            case 4:
                SponCommend(_a.Inc.Unit, _a.Inc.sets);
                break;
        }
       
    }


    //Point_Of_Intrest constract all the Path to capter the points that the AI can cheuse.
    public void Point_Of_Intrest(PathNode _path)
    {
        GameObject CPoint;
        GameObject UnitP;
        int size = AI_manger.Game_Manger.Points.Count;
        int Ary = AI_manger.Army.Count;

        for (int i = 0; i < size; i++)
        {
            CPoint = AI_manger.Game_Manger.Points[i];
            if (CPoint.GetComponent<neutraleBiulding>().unitManager != AI_manger)
            {
                for (int j = 0; j < Ary; j++)
                {
                    UnitP = AI_manger.Army[j];
                    if (UnitP.GetComponent<UnitController>().isActive == false)
                    {
                        if (_path.IncBlock == null)
                            _path.addPath((int)logic.AnalyzeDis(UnitP,CPoint)
                                , new Inc_Block(UnitP, CPoint, 2)); // Note: Need to replase value 0 with a comande value calculater, and com '2' is for captuer point comande.
                        else
                            _path.addPath(dis_value(_path.IncBlock), new Inc_Block(UnitP, CPoint, UnitP.transform.position, 2));
                        //---------------------------------------------------------------------------**--**--**--
                    }
                }
            }
        }
    }


    //Point_Of_Intrest constract all the Path for the AI to attack visibal enemys on the map .
    public void Enemy_Activety(PathNode _path, ushort _i)
    {
        GameObject EnemyP;
        GameObject UnitP;
        ushort size = (ushort)AI_manger.Enemys.Count;
        ushort Ary = (ushort)AI_manger.Army.Count;

        for (ushort i = _i; i < size; i++)
        {
            EnemyP = AI_manger.Enemys[i];
            if (/*EnemyP.GetComponent<UnitController>().visbye.Exists(istag => istag == AI_manger.Unit) == true*/ true)
            {
                for (ushort j = 0; j < Ary; j++)
                {
                    UnitP = AI_manger.Army[j];
                    if (UnitP.GetComponent<UnitController>().isActive == false)
                    {
                        float a = logic.AnalyzeDis(UnitP, EnemyP); //culcualte the priorety of the disition
                        if (_path.IncBlock == null)
                            _path.addPath((int)a, new Inc_Block(UnitP, EnemyP, 3)); // Note: Need to replase value 0 with a comande value calculater, And com '3' is for attack comande.
                        else
                            _path.addPath((int)a, new Inc_Block(UnitP, EnemyP, UnitP.transform.position, 3));
                        //---------------------------------------------------------------------------**--**--**--
                    }
                }
            }
        }
    }
    // Unit_Construction constract all the Path of the AI to spon new units.
    public void Unit_Construction(PathNode _path)
    {
        int size = AI_manger.Coustructon_biulding.Count;
        for(int i = 0; i < size; i++){ // loop thorw every Biulding that can spon units 

            BiuldingLogic currant = AI_manger.Coustructon_biulding[i].GetComponent<BiuldingLogic>(); // Hold all the current the Biulding charateristics
            int sponer_count = currant.Sponers.Count; 
            for (int j = 0; j < sponer_count; j++) // loops throw every sponer in the Biulding sponing list
            {
                GameObject sponer = currant.Sponers[j];
                _path.addPath((int)logic.AnalyzeBiulding(AI_manger, AI_manger.Coustructon_biulding[i],sponer), new Inc_Block(AI_manger.Coustructon_biulding[i] , j , 4));
            }
        } 
    }


    //SponeUnit while be incharge of all the "sponing" of new units for the AI 
    public int SponeUnit(GameObject _unit){

		if (AI_manger.Coustructon_biulding == null) {
			return -1; // -1 mens that the Biulding that Spone the unit das not exist.
		} else {
			for (byte i = 0; i < AI_manger.Coustructon_biulding.Count; i++) {
				
				for (byte j = 0; j < AI_manger.Coustructon_biulding[i].GetComponent<BiuldingLogic>().Sponers.Count; j++) {
					
					if (_unit == AI_manger.Coustructon_biulding [i].GetComponent<BiuldingLogic> ().Sponers [j]) {
						AI_manger.Coustructon_biulding [i].GetComponent<BiuldingLogic> ().SponMenu (j);
					return 0;
					}
				}//End inner loop.
			}//End Outer loop.
			return -2; // -2 mens that the biulding that is respocible for the Sopne of the unit.  das't 
		}

	}


    //MoveCommend lets the AI send a move commend to a unit
    public byte MoveCommand(GameObject Unit, Vector3 Target)
    {
        Unit.GetComponent<UnitController>().MoveCommend(Target);
        return 0;
    }

    //AttackCommend lets the AI send an attack commend to a unit on an enemy
    public byte AttackCommend(GameObject Unit, GameObject Target)
    {
        Unit.GetComponent<UnitController>().AttackCommend(Target.GetComponent<Collider>());
        return 0;
    }

    //CaptureCommend lets the AI send a capture commend to a unit to capture a point
    public void CaptureCommend(GameObject Unit, GameObject Target)
    {
        formations form = new formations();
        form.CraetFormation(Target.transform.position, 1, 0, 0);
        Unit.GetComponent<UnitController>().CaptureCommend(Target.GetComponent<Collider>(), Target.transform.position);    
    }

    void SponCommend(GameObject Biulding , int Sponer)
    {
        Biulding.GetComponent<BiuldingLogic>().SponMenu(Sponer);
    }

}

/* the Inc_Block class 
 *  this class hold all the data the AI need to execut a comend to a unit
 *  
 *  variables:
 *  unit - unit points to the charcter that the AI gives the orther.
 *  target - the target points to the objective of the unit.
 *  pos - holds the corent vector of the unit if it is in the present or will bie in futier turns.
 *  com(commend) - teals to the game what is the comend that the AI want to do.
 */
public class Inc_Block 
{
    GameObject unit, target; // unit points to the charcter that the AI gives the orther. ,the target points to the objective of the unit.
    Vector3 pos; // holds the corent vector of the unit if it is in the present or will bie in futier turns.
    private byte com = 0; // teals to the game what is the comend that the AI want to do.
    private int set = 0;

    public Inc_Block()
    { }
    public Inc_Block(GameObject _unit , int _set , byte _com) // constructer for instruction Blocks use to spon new units
    {
        unit = _unit;
        set = _set;
        com = _com;
    }
    public Inc_Block(GameObject _unit, GameObject _target, byte _com)
    {
        unit = _unit;
        target = _target;
        pos = _unit.transform.position;
        com = _com;
    } //constructer for instruction Blocks use to " move , cature , attack " orders 

    public Inc_Block(GameObject _unit, GameObject _target,Vector3 _pos, byte _com)
    {
        unit = _unit;
        target = _target;
        pos = _pos;
        com = _com;
    } //constructer for instruction Blocks use to " move , cature , attack " orders with vetural positon


    public byte comande
    {
        set
        {
            com = value;
        }
        get
        {
            return com;
        }
    }

    public int sets
    {
        set
        {
            set = value;
        }
        get
        {
            return set;
        }
    }

    public Vector3 Positon
    {
        set
        {
            pos = value;
        }
        get
        {
            return pos;
        }
    }

    public GameObject Unit
    {
        get
        {
            return unit;
        }
    }

    public GameObject Target
    {
        get
        {
            return target;
        }
    }

}


/* the pexe class
 * this class is the a stract (memory block) the the AI algoritem is using to make decitions.
 * 
 * variables;
 * value - determens how practical is the commend stord in the Inc_block ( the higher the value the more practical ).
 * index - point to the index of the node in the layer.
 * Inc - hold all the data for the AI to execute a commend.
 */
public class paxe
{
    public int value;
    public byte index;
    public Inc_Block Inc;

    public paxe(int _v, byte _i, Inc_Block _com)
    {
        value = _v;
        Inc = _com;
        index = _i;
    }
}

