using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace ImageCapturing
{
    public enum TRIGGER_STATUS { ON, OFF, NONE };
        
    public class TriggerBase : ProgressBase
    {
        protected int ResponseDelayTime = 0;
        protected int SignalInterval = 20;
        protected int SignalHZ = 50;
        public string PortName = "NONE";
        protected int TimeoutCount = 0;
        protected int TimeoutTotal = 0;
        protected Timer TriggerTime = null;

        public delegate void TriggerChangedDelegate(TRIGGER_STATUS status);
        public event TriggerChangedDelegate TriggerChanged;
        public event TriggerChangedDelegate TriggerStatus;
        
        public static TriggerBase GetTrigger()
        {
            TriggerBase Trigger = new TriggerBase();
            Trigger.InitParam();
            string tmp = Trigger.PortName.ToLower();
            if (tmp == "none")
            {
                Trigger = new TriggerNONE();
            }
            else if (tmp.Contains("com"))
            {
                //Trigger = new TriggerCOM();
            }
            Trigger.InitParam();
            return Trigger;
        }
        
        public TriggerBase()
        {
            TriggerTime = new Timer();
            TriggerTime.Elapsed += new ElapsedEventHandler(TriggerTime_Elapsed);

            InitParam();
        }

        public virtual void Dispose()
        {
           
        }

        public void RefreshHostHandle(IntPtr handle)
        {
            //HostHandle = handle;
            //Stop();
        }

        protected virtual void TriggerTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            TriggerChange(TRIGGER_STATUS.ON);
            Stop();
        }

        public virtual void InitParam()
        {
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.TriggerDelayTime), out ResponseDelayTime))
            {
                ResponseDelayTime = 0;
            }
            PortName = CapturePub.readCaptrueValue(XmlField.TriggerPort);
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.SignalInterval), out SignalInterval))
            {
                SignalInterval = 2;
            }
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.SignalTimeout), out SignalHZ))
            {
                SignalHZ = 50;
            }
        }

        public virtual void Start()
        {
            TriggerTime.Interval = SignalInterval;
            TimeoutTotal = SignalInterval * SignalHZ / 1000;
            TriggerTime.Start();
        }

        public virtual void Stop()
        {
            TriggerTime.Stop();
        }

        protected virtual void TriggerChange(TRIGGER_STATUS status)
        {
            //Console.WriteLine("-----");
            if (ResponseDelayTime > 0)
            {
                int time = ResponseDelayTime / (int)TriggerTime.Interval;
                for (int i = 0; i < time; i++)
                {
                    SetProgressBegin("Preparing irradiation and capturing..." + (time - i),0);
                    System.Threading.Thread.Sleep((int)TriggerTime.Interval);
                }
                System.Threading.Thread.Sleep((int)(ResponseDelayTime - TriggerTime.Interval * time));
            }
            if (TriggerChanged != null)
            {
                TriggerChanged(status);
            }         
        }

        protected virtual void TriggerStatusShow(TRIGGER_STATUS status)
        {
            if (TriggerStatus != null)
            {
                TriggerStatus(status);
            }
        }
    }
}
