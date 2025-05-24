using System;

public class DataManager : SingletonBase<DataManager>
{
    private int _rowCount = 0;  // score, increases when moving forward
    private int _sweetCount = 0;    // coin, increases differently depending on the type of item when triggered, be used to buy characters
    
    // set calling event when value changed at setter (for update UI)
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
