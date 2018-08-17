using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{
    class TriggerNONE : TriggerBase
    {
        public TriggerNONE()
            : base()
        {

        }

        public override void InitParam()
        {
            base.InitParam();

            SignalInterval = 1000;
        }

        protected override void TriggerTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (TriggerTime.Enabled)
            {
                Stop();
                TriggerChange(TRIGGER_STATUS.ON);
            }
        }
    }
}
