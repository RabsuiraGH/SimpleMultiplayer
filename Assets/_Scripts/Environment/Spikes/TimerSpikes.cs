using System;
using UnityEngine;

namespace Core
{
    public class TimerSpikes : ToggleSpikes
    {
        [SerializeField] private float _timeInCloseState = 2;
        [SerializeField] private float _timeInOpenState = 3;
        private float _timeInCloseStateCounter = 0;
        private float _timeInOpenStateCounter = 0;

        private void Update()
        {
            if(!IsHost) return;

            if (_isClosed)
            {
                _timeInCloseStateCounter -= Time.deltaTime;

                if (_timeInCloseStateCounter <= 0)
                {
                    ToggleSpikesRPC(true);
                }
            }
            else
            {
                _timeInOpenStateCounter -= Time.deltaTime;

                if (_timeInOpenStateCounter <= 0)
                {
                    ToggleSpikesRPC(false);
                }
            }
        }

        protected override void OpenSpikes()
        {
            base.OpenSpikes();
            _timeInOpenStateCounter = _timeInOpenState;
        }

        protected override void CloseSpikes()
        {
            base.CloseSpikes();
            _timeInCloseStateCounter = _timeInCloseState;
        }
    }
}