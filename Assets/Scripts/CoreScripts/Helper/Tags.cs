
using System;

public static class Tags
{
    public static string Player_Tag = "Player";
    public static string Enemy_Tag = "Enemy";
    public static string Ground_Tag = "Ground";
    public static string Bullet_Tag = "Bullet";
    public static string EnemyBullet_Tag = "EnemyBullet";
    public static string Water_Slime_Tag = "Slime";

    public static string SlimeCollectAnim = "Collect";
    public static string Building_Tag = "Building";
    public static string HealZone_Tag = "HealZone";
}
public static class AnimTag
{
    public static string PlayerRun_Anim = "Run";
    public static string PlayerDash_Anim = "Dash";
    public static string PlayerJump_Anim = "Jump";
    public static string PlayerShot_Anim = "Shot";
    public static string PlayerDead_Anim = "Dead";
    public static string PlayerIdle_Anim = "Idle";
    public static string PlayerTakeDamage_Anim = "TakeDamage";

    public static string PlayerWalkForwardORBack_Anim = "y";
    public static string PlayerWalkLeftORRight_Anim = "x";

    public static string EnemyRun_Anim = "EnemyRun";
    public static string EnemyFly_Anim = "Fly";
    public static string EnemyJump_Anim = "EnemyJump";
    public static string EnemyAttack_Anim = "EnemyShot";
    public static string EnemyDead_Anim = "EnemyDead";
    public static string EnemyTakeDamage_Anim = "EnemyTakeDamage";

    public static string BossRun_Anim = "BossRun";
    public static string BossDead_Anim = "BossDead";
    public static string BossIdle_Anim = "BossIdle";
    public static string BossShot_Anim = "BossShot";
    public static string BossTakeDamage_Anim = "BossTakeDamage";


    public static string Speed_Param = "Speed";
    public static string Attack_Param = "Attack";
    public static string isDead_Param = "Dead";
    public static string MoveX_Param="MoveX";
    public static string MoveY_Param="MoveY";
    public static string IsMoving_Param="IsMoving";



}
public static class KeyTags
{
    public static string KEY_PLAYER = "Player";
    public static string KEY_GAME_LOOP_CONTROLLER = "GameLoopController";
}
public static class EventTags
{
    public const string EVENT_PLAYER_SPAWNED = "EVENT_PLAYER_SPAWNED";
    public static string EVENT_PLAYER_DIED = "PlayerDied";
    public static string EVENT_GAME_WIN = "GameWin";
    public static string EVENT_GAME_LOSE = "GameLose";
    public static string EVENT_GAME_STARTED = "GameStarted";
    public static string EVENT_GAME_RESTARTED = "GameRestarted";
    public static string EVENT_MATCH_TIME_UPDATED = "MatchTimeUpdated";

}

public static class GameRegistryTags
{
    public static string GAME_REGISTRY_SPAWNER_PROJECTILE = "ProjectileSpawner";
    public static string GAME_REGISTRY_SPAWNER_SUMMON = "SummonSpawner";
}

    public static class LayerTags
{ 
    public static string Player_Layer = "Player";
    public static string Enemy_Layer = "Enemy";
    public static string Boss_Layer = "Boss";
    public static string Ground_Layer = "Ground";
    public static string Obstacle_Layer = "Obstacle";
    public static string Dead_Layer = "Dead";
}
public static class PlayerPrefsTag
{
    public static string TutorialCheck_Prefs = "Tutorial";
    public static string Gold_Prefs = "Gold";
}
public static class NameTags
{
    public static string Player_Name = "Player";
}
