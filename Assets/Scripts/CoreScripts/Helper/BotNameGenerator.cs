using UnityEngine;

namespace CoreScripts.Helper
{
    public static class BotNameGenerator
    {
        private static readonly string[] names =
        {
    "Hank",
    "Tex",
    "Gus",
    "Buck",
    "Roy",
    "Ned",
    "Jack",
    "Cole",
    "Clint",
    "Wade",
    "Rex",
    "Zane",
    "Jett",
    "Blaze",
    "Dusty",
    "Colt",
    "Wyatt",
    "Ryder",
    "Boone",
    "Cash"
};

        public static string GetRandomName()
        {
            return names[Random.Range(0, names.Length)];
        }
    }
}