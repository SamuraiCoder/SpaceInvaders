public static class ConstValues 
{
    public const string PLAYER_NAME = "PlayerShip";
    public const float PLAYER_WIDTH = 100.0f;
    public const string ENEMY_PREFIX_NAME = "Enemy_";
    public const float SPACING_ENEMY_COLUMN = 150;
    public const float SPACING_ENEMY_ROW = 150;
    public const string ENEMY_BLACK_1 = "Enemies/enemyBlack1";
    public const string ENEMY_BLACK_2 = "Enemies/enemyBlack2";
    public const string ENEMY_BLACK_3 = "Enemies/enemyBlack3";
    public const string ENEMY_BLACK_4 = "Enemies/enemyBlack4";
    public const string ENEMY_BLACK_5 = "Enemies/enemyBlack5";
    public const string ENEMY_BLUE_1 = "Enemies/enemyBlue1";
    public const string ENEMY_BLUE_2 = "Enemies/enemyBlue2";
    public const string ENEMY_BLUE_3 = "Enemies/enemyBlue3";
    public const string ENEMY_BLUE_4 = "Enemies/enemyBlue4";
    public const string ENEMY_BLUE_5 = "Enemies/enemyBlue5";
    public const string ENEMY_GREEN_1 = "Enemies/enemyGreen1";
    public const string ENEMY_GREEN_2 = "Enemies/enemyGreen2";
    public const string ENEMY_GREEN_3 = "Enemies/enemyGreen3";
    public const string ENEMY_GREEN_4 = "Enemies/enemyGreen4";
    public const string ENEMY_GREEN_5 = "Enemies/enemyGreen5";
    public const string ENEMY_RED_1 = "Enemies/enemyRed1";
    public const string ENEMY_RED_2 = "Enemies/enemyRed2";
    public const string ENEMY_RED_3 = "Enemies/enemyRed3";
    public const string ENEMY_RED_4 = "Enemies/enemyRed4";
    public const string ENEMY_RED_5 = "Enemies/enemyRed5";
    public enum ColorEnemyPool { BLACK, BLUE, GREEN, RED }
    public enum EnemyDirectionSense { GOING_RIGHT, GOING_LEFT, GOING_DOWN }
    public const float ENEMY_SPEED = 1500.0f;
    public const float AI_ENEMY_PACE_CHECK = 1.0f;
    public const float AI_ENEMY_NOTIFY_SURROUNDINGS_CHECK = 2.0f;
    public enum ShootingEntityType { PLAYER, ENEMY }
    public const float LASER_COOLING_ENEMY = 8.0f;
    public const float LASER_COOLING_PLAYER = 3.0f;
    public const string ENEMY_LAYER = "EnemyShip";
    public const string LASER_LAYER = "LaserProjectile";
    public const string LASER_ENEMY_LAYER = "LaserEnemyProjectile";
    public const float PLAYER_FIRE_RATE = 1.0f;
    public const int SCORE_BLACK = 20;
    public const int SCORE_BLUE = 15;
    public const int SCORE_GREEN = 10;
    public const int SCORE_RED = 25;
    public const int SCORE_MISTERY = 50;
    public const string WIN_TEXT = "You win!";
    public const string LOSE_TEXT = "You lose...";
    public const string SCORE_PREFS_KEY = "USER_SCORE";
    public const string LEVEL_PLAYED_PREFS_KEY = "LEVEL_PLAYED";
    public const int MAX_AMOUNT_LEVELS = 5;
}
