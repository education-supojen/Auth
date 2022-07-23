using Auth.Application.Interfaces.Services;

namespace Auth.Infrastructure.Services;

public class IdentityProducer : IIdentityProducer
{
    /// <summary>
    /// 起始的時間戳:唯一時間，這是一個避免重復的隨機量，自行設定不要大于當前時間戳，
    /// 一個計時周期表示一百納秒，即一千萬分之一秒， 1 毫秒內有 10,000 個計時周期，即 1 秒內有 1,000 萬個計時周期，
    /// </summary>
    private static readonly long StartTimeStamp = new DateTime(2022,1,1).Ticks/10000;
    
    // ====================================================================================================
    // Variables - 
    //     每一個部分佔用的位數 
    // ====================================================================================================
    private const int SequenceBit = 12;  // 序列號占用的位數
    private const int MachiningBit = 5;  // 機器標識占用的位數
    private const int DataCenterBit = 5; // 資料中心占用的位數

    private const long MaxSequence = -1L ^ (-1L << SequenceBit);
    private const long MaxMachiningNum = -1L ^ (-1L << MachiningBit);
    private const long MaxDataCenterNum = -1L ^ (-1L << DataCenterBit);

    private const int MachiningLeft = SequenceBit;
    private const int DataCenterLeft = SequenceBit + MachiningBit;
    private const int TimeStampLeft = DataCenterLeft + DataCenterBit;
    
    private readonly long _dataCenterId;  //資料中心
    private readonly long _machineId;     //機器標識
    private long _sequence; //序列號
    private long _lastTimeStamp = -1;  //上一次時間戳
    
    /// <summary>
    /// 取得詞間戳
    /// </summary>
    /// <returns></returns>
    private long GetNextMill()
    {
        long mill = GetNewTimeStamp();
        while (mill <= _lastTimeStamp)
        {
            mill = GetNewTimeStamp();
        }
        return mill;
    }

    /// <summary>
    /// 產生新的時間戳
    /// </summary>
    /// <returns></returns>
    private static long GetNewTimeStamp()
    {
        return DateTime.Now.Ticks/10000;            
    }
    
    /// <summary>
    /// 根據指定的資料中心ID和機器標志ID生成指定的序列號
    /// </summary>
    /// <param name="dataCenterId">資料中心ID</param>
    /// <param name="machineId">機器標志ID</param>
    public IdentityProducer(long dataCenterId, long machineId)
    {
        if (dataCenterId is > MaxDataCenterNum or < 0)
        {                
            throw new ArgumentException("DtaCenterId can't be greater than MAX_DATA_CENTER_NUM or less than 0！");
        }
        if (machineId is > MaxMachiningNum or < 0)
        {
            throw new ArgumentException("MachineId can't be greater than MAX_MACHINE_NUM or less than 0！");
        }
        this._dataCenterId = dataCenterId;
        this._machineId = machineId;
    }

    /// <summary>
    /// 產生下一個ID
    /// </summary>
    /// <returns></returns>
    public long GetId()
    {
        var currTimeStamp = GetNewTimeStamp();
        if (currTimeStamp < _lastTimeStamp)
        {
            //如果當前時間戳比上一次生成ID時時間戳還小，拋出例外，因為不能保證現在生成的ID之前沒有生成過
            throw new Exception("Clock moved backwards.  Refusing to generate id");
        }

        if (currTimeStamp == _lastTimeStamp)
        {
            //相同毫秒內，序列號自增
            _sequence = (_sequence + 1) & MaxSequence;
            //同一毫秒的序列數已經達到最大
            if (_sequence == 0L)
            {
                currTimeStamp = GetNextMill();
            }
        }
        else
        {
            //不同毫秒內，序列號置為0
            _sequence = 0L;
        }

        _lastTimeStamp = currTimeStamp;

        return (currTimeStamp - StartTimeStamp) << TimeStampLeft //時間戳部分
                | _dataCenterId << DataCenterLeft                //資料中心部分
                | _machineId << MachiningLeft                    //機器標識部分
                | _sequence;                                     //序列號部分
    }
}