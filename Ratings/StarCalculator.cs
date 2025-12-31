using UnityEngine;

namespace UNBEATAP.Ratings;

public static class StarCalculator
{
    public const int RatingDivisor = 5625;
    public const float AccuracyPower = 1.12f;


    public static float GetAccPower(float acc)
    {
        if(acc <= 50f)
        {
            return 0f;
        }

        return Mathf.Pow(acc -50f, AccuracyPower);
    }


    public static int GetGradeBonus(float acc, bool fc, bool fail)
    {
        if(fail)
        {
            return 0;
        }

        if(fc)
        {
            acc += 1f;
        }

        if(acc > 90f)
        {
            return 25;
        }
        else if(acc > 85)
        {
            return 20;
        }
        else if(acc > 75)
        {
            return 15;
        }
        else if(acc > 65)
        {
            return 12;
        }
        else return 10;
    }


    public static float GetRatingFromPlay(float level, float acc, bool fc, bool fail)
    {
        float accPower = GetAccPower(acc);
        float gradeBonus = GetGradeBonus(acc, fc, fail);

        float rating = level * (accPower + gradeBonus);
        return rating / RatingDivisor;
    }
}