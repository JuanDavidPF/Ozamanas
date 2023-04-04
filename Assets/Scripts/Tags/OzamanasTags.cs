using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Tags
{   

    [System.Flags]
    public enum ActionButtonTypes
    {
        Exit = 1 << 0,
        ToSky = 1 << 1,
        ToCamp= 1 << 2,
        ToFire= 1 << 3,
        ToCodex= 1 << 4,
        Continue= 1 << 5,
        Restart= 1 << 6,
        Play= 1 << 7
    }

    public enum ActionButtonType
    {
        Exit,
        ToSky ,
        ToCamp,
        ToFire,
        ToCodex,
        Continue,
        Restart,
        Play
    }

    public enum Scenes
    {
        Gameplay,
        Fire,
        Camp,
        Sky,
        Settings,
        Credits,
        Narrative,

        Explicative_HUD,

        Interactuable_HUD,

        Lose_Menu,

        Win_Menu,

        Gameplay_HUD,

        Pause_Menu,

        Main_Menu,

        Quit_Modal,

        Codex,

        ExitGame_Menu
    }

    public enum PhraseSequenceState
    {
        Default,
        Active,
        Finnished
    }

    public enum PreRequisiteType
    {
        UnlockedForce,
        LevelComplete,
        OnScene
    }

     public enum CellPointerType
        {
            Pointer,
            PullPointer,
            PushPointer,

            MountainPointer,

            ExpansionPointer,

            MouseOverPointer,

            ReleaseRangePointer,

            AttackRangePointer,

            SelectionPointer
        }

     public enum PlacementMode
        {
            SinglePlacement,
            DoublePlacement,
            TriplePlacement
        }

    public enum AddForceType
    {
        VerticalJump,
        BackFlip,
        FrontFlip
    }
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
        Idling = 2,

        Waiting = 3
       

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
        Destroyed = 5,
        Captured = 6,
        Attacked = 7,
        AddForce =8
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