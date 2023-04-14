using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected class TimerFinisher : Finisher
    {
        private float _time;

        public TimerFinisher(float time) => _time = time;

        internal override Component Clone() => new TimerFinisher(_time);

        internal override bool TryInitialize(Stats stats) => true;

        internal override void Start() => GameManager.FixedUpdateEvent += Tick;

        internal override bool IsFinished() => _time >= 0;

        internal override void Stop() => GameManager.FixedUpdateEvent -= Tick;

        private void Tick() => _time -= Time.fixedDeltaTime;
    }
}
