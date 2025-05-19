using System;

public class DataManager : SingletonBase<DataManager>
{
    private int _rowCount = 0;  // Score 개념, 앞으로 전진할 때 1씩 증가
    private int _sweetCount = 0;    // Coin 개념, 아이템 트리거 시 아이템 종류에 따라 다르게 증가, 캐릭터 구입에 필요한 재화
    
    // setter에서 값 변하면 이벤트 호출하도록 설정(for UI 갱신)
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
