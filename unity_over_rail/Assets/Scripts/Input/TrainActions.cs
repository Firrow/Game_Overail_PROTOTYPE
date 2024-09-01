using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// API
/// Script that contains actions can be done by trains
/// </summary>

namespace overail.TrainActions_
{
    public static class TrainActions
    {
        public static void APIChangeDirection(int index, int directionValue)
        {
            Train train = GetTrainFromIndex(index);

            if (train is not null)
            {
                train.PlayerChoiceDirection(directionValue);
            }
        }

        public static void APIOnAccelerateTrain(int index, bool isAccelerate)
        {
            Train train = GetTrainFromIndex(index);

            if (train is not null)
            {
                train.PlayerIncreaseVelocity(isAccelerate);
            }
        }

        public static void APIOnDecelerateTrain(int index, bool isDecelerate)
        {
            Train train = GetTrainFromIndex(index);

            if (train is not null)
            {
                train.PlayerDecreaseVelocity(isDecelerate);
            }
        }

        public static void APIOnMoveWeapon(int index, Vector2 valueMovement, bool isKeyboard)
        {
            Train train = GetTrainFromIndex(index);

            if (train is not null)
            {
                train.PlayerMoveWeapon(valueMovement, isKeyboard);
            }
        }

        public static void APIOnShoot(int index, float valueShoot)
        {
            Train train = GetTrainFromIndex(index);

            if (train is not null && valueShoot == 0)
            {
                train.PlayerShoot();
            }
        }

        public static void APIUsePickObject(int index, float valueUseObject)
        {
            Train train = GetTrainFromIndex(index);

            if (train is not null && valueUseObject == 0)
            {
                train.UsePickObject();
            }
        }

        private static Train GetTrainFromIndex(int index)
        {
            return Trains.FirstOrDefault(t => t.TrainIndex == index);
        }



        public static Train[] Trains //TODO : voir pour passer par le DataContainer quand il sera en static
        {
            get { return GameObject.FindObjectsOfType<Train>(); }
        }
    }
}

