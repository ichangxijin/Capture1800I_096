using System;
using System.Collections.Generic;
using System.Text;

namespace AMRT
{
    public unsafe class PanelManager
    {
        #region Variables Members

        uint nRet;
        uint pdwNumSensors;
        int bEnableIRQ = 1;
        int bAlwaysOpen = 0;

        public IntPtr hAcqDesc;
        uint nChannelType;
        int nChannelNr;
        uint dwFrames;
        public uint dwRows;
        public uint dwColumns;
        uint dwDataType;
        uint dwSortFlags;
        uint dwAcqType;
        uint dwSystemID;
        uint dwSyncMode;
        uint dwHwAccess;
        ushort dwGain;
        ushort dwBinning;

        PKL_Interface.CHwHeaderInfo Info = new PKL_Interface.CHwHeaderInfo();
        PKL_Interface.CHwHeaderInfoEx InfoEx = new PKL_Interface.CHwHeaderInfoEx();

        #endregion

        # region Initialization

        public bool IsLink()
        {
            uint nRet;
            const int HIS_ALL_OK = 0;
            nRet = PKL_Interface.Acquisition_EnumSensors(out pdwNumSensors, bEnableIRQ, bAlwaysOpen);
            return (nRet == HIS_ALL_OK && pdwNumSensors >= 0);
        }

        public bool Init()
        {
            uint nRet;
            const int HIS_ALL_OK = 0;
            nRet = PKL_Interface.Acquisition_EnumSensors(out pdwNumSensors, bEnableIRQ, bAlwaysOpen);
            if (nRet != HIS_ALL_OK || pdwNumSensors == 0) return false;

            //Iterate through all sensors, and select the last sensor
            uint Pos = 0;
            do
            {
                nRet = PKL_Interface.Acquisition_GetNextSensor(ref Pos, out hAcqDesc);
            } while (Pos != 0);
            if (nRet != HIS_ALL_OK) return false;

            //ask for communication device type and its number
            if (PKL_Interface.Acquisition_GetCommChannel(hAcqDesc, out nChannelType, out nChannelNr) != HIS_ALL_OK)
            {
                return false;
            }

            //ask for data organization of all sensors
            if ((nRet = PKL_Interface.Acquisition_GetConfiguration(hAcqDesc, out dwFrames, out dwRows,
                out dwColumns, out dwDataType, out dwSortFlags, out bEnableIRQ,
                out dwAcqType, out dwSystemID, out dwSyncMode, out dwHwAccess)) != HIS_ALL_OK)
            {
                return false;
            }

            if ((nRet = PKL_Interface.Acquisition_GetHwHeaderInfoEx(hAcqDesc, ref Info, ref InfoEx)) == HIS_ALL_OK)
            {
                dwGain = InfoEx.wGain;
            }

            //PKL_Interface.GetCameraBinningMode(hAcqDesc, out dwBinning);
            return true;
        }

        #endregion

    }
}
