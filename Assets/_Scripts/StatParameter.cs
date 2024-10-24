using System;
using R3;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class StatParameter
    {
        public ReadOnlyReactiveProperty<float> MaxValueReadonly => MaxValue;
        public ReadOnlyReactiveProperty<float> MinValueReadonly => MinValue;
        public ReadOnlyReactiveProperty<float> CurrentValueReadonly => CurrentValue;

        [field: SerializeField] public SerializableReactiveProperty<float> MaxValue { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<float> CurrentValue { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<float> MinValue { get; private set; }

        // TODO: ADD DEBUFFS
        //[SerializeField] private List<> _previous = 0;

        public void ChangeCurrent(float previous, float current)
        {
            CurrentValue.Value = current;
        }


        public StatParameter(float min, float max, float current = float.NaN)
        {
            MinValue = new(min);
            MaxValue = new(max);

            if (float.IsNaN(current))
            {
                CurrentValue = new();
                CurrentValue.Value = MaxValue.Value;
            }
        }
    }
}