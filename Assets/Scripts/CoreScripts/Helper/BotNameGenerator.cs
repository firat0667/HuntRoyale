using UnityEngine;

namespace CoreScripts.Helper
{
    public static class BotNameGenerator
    {
        private static readonly string[] names =
        {
    "Dusty Bill",
    "Slim Hank",
    "Deadeye Joe",
    "Lucky Pete",
    "Crazy Cactus",
    "Wild Buck",
    "Whiskey Sam",
    "Rusty Colt",
    "Tiny Tex",
    "Dirty Dalton",
    "Bandit Bob",
    "Iron Gus",
    "Trigger Tom",
    "Old Man Oak",
    "Cactus Jack",
    "One-Eyed Roy",
    "Quickdraw Ned",
    "Barrel Ben",
    "Snake Ryder",
    "Goldtooth Greg"
        };

        public static string GetRandomName()
        {
            return names[Random.Range(0, names.Length)];
        }
    }
}