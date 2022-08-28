using Godot;
using System;

namespace Project
{
    public static class Global
    {
        // Nodes
        public static GameMaster gameMaster;
        public static Player player;

        // UI nodes
        public static UserInterface   ui;
        public static BlackoutHandler blackoutHandler;
        public static DialogueHandler dialogueHandler;
        public static PaperHandler    paperHandler;
        public static InteractionProgress interactionProgress;
        public static CutsceneSkipper cutsceneSkipper;

        // Facility nodes
        public static TheFacilityEvents facilityEvents;
        public static PlagueDoctor plagueDoctor;
        public static VentFromAmongus vent;

        // Variables
        public static RandomNumberGenerator rng = new RandomNumberGenerator();
    }
}