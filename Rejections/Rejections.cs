using System;
using System.Collections.Generic;
public interface Rejectionss
{
    List<string> IgnoreList { get; set; }
    void Record(string symbol, DateTime now);
    bool isPermitted(string symbol, DateTime now);

}
public class Rejects : Rejectionss
{
    int max_rejections;
    double wait_time;
    public List<string> IgnoreList { get; set; }
    Dictionary<string, Tuple<DateTime, int>> reject_symbols;
/// <param name="wait_time">the minimum time between requests (in minutes).</param>
/// <param name="max_rejections">max # of times a request for a symbol can be rejected before it can no longer be permitted. </param>
    public Rejects(double wait_time, int max_rejections)
    {
        this.wait_time = wait_time;
        this.max_rejections = max_rejections;
        this.reject_symbols = new Dictionary<string, Tuple<DateTime, int>>();
    }
/// <param name="symbol">the name of the stock.</param>
/// <returns>Tuple of the last rejected request's time, and # of rejected requests for the symbol. </returns>
    public Tuple<DateTime, int> getRecord(string symbol){
        if(symbol==null){return null;}
        if (this.reject_symbols.ContainsKey(symbol)) { return this.reject_symbols[symbol]; }
        else { return null; }
    }
/// <param name="symbol">the name of the stock.</param>
/// <param name="now">the time of the rejected request. </param>
    public void Record(string symbol, DateTime now)
    {
        if(symbol==null){return;}
        if (this.IgnoreList != null && this.IgnoreList.Contains(symbol)) { return; }
        Tuple<DateTime, int> val = null;
        if (this.reject_symbols.TryGetValue(symbol, out val))
        {
            this.reject_symbols[symbol] = new Tuple<DateTime, int>(now, val.Item2 + 1);
        }
        else
        {
            this.reject_symbols[symbol] = new Tuple<DateTime, int>(now, 1);
        }
    }
/// <param name="symbol">the name of the stock.</param>
/// <param name="now">the time of the request. </param>
/// <returns>True if: 
///1.) Is on the IgnoreList. 
///2.) Number of rejections does not exceed max rejections.
///3.) Time of the request - the last request exceeds the wait time.
///False otherwise.
///</returns>
    public bool isPermitted(string symbol, DateTime now)
    {
        if(symbol==null){return false;}
        if (this.IgnoreList != null && this.IgnoreList.Contains(symbol)) { return true; }
        Tuple<DateTime, int> val = null;
        if (this.reject_symbols.TryGetValue(symbol, out val))
        {
            if (val.Item2 >= this.max_rejections) { return false; }
            else if ((now - val.Item1).TotalMinutes > this.wait_time) { return true; }
            else { return false; }
        }
        else { return true; }
    }
}
