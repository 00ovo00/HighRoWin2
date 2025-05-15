using System;

public class DataManager : SingletonBase<DataManager>
{
    private int _rowCount = 0;
    private int _sweetCount = 0;
    
    public Action OnScoreChanged;
    public Action OnCoinChanged;
    
    public int RowCount
    {
        get
        {
            return _rowCount;
        }
        set
        {
            _rowCount = value;
            OnScoreChanged?.Invoke();
        }
    }
    
    public int SweetCount
    {
        get
        {
            return _sweetCount;
        }
        set
        {
            _sweetCount = value;
            OnCoinChanged?.Invoke();
        }
    }
}
