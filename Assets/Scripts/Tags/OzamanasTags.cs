using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Tags
{

    public enum TutorialType
{
    Explicative,
    Interactuable
}

public enum LevelState
{

    Blocked = 0,
    Playable = 1,
    Finished = 2,

}

public enum PlayerState
{

    Running = 1,
        Idling = 2
       

}

 public enum PhysicMode
        {
            Kinematic,
            Physical,
            Intelligent,
        }

        public enum MachineAltitude
    {
        Terrestrial ,
        Aerial,
        Subterrestrial
       
    }

    public enum MachineHierarchy
{
    Boss = 0,
    Regular =1,
    Destructor
}

    public enum MachineSpeed
    {
        Reverse = -1,
        VeryLow =0,
        Low =1,
        Medium = 2,
        High =3 ,
        VeryHigh = 4
    }

    public enum MachineTraits
    {
        IncreaseSpeed ,
        ReduceSpeed ,
        StopMachine ,
        DisarmMachine ,
        RepairMachine ,
        DoubleDamage,
        Invulnerable,
        GotoBase
    }

    public enum MachineState
    {
        Running = 1,
        Blocked = 2,
        Idling = 3,
        Acting = 4,

        Destroyed = 5
    }

     public enum TreeType
    {
        Tree = 1,
        Flower = 2
    }

     public enum TreeState
    {
        Hidden = 1,
        Expose = 2,
        Destroyed =3
    }
}