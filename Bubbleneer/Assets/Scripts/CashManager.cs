using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CashManager : MonoBehaviour {

	private Dictionary<string, int> PipeCosts;

#region VARIABLES
    // zero by default
    private int currentCash;
    // zero by default
    private int lowerCashLimit;
    // 9999 by default
    private int upperCashLimit;

	public Text cashText;
#endregion

	void Awake() {
		lowerCashLimit = 0;
		upperCashLimit = 9999;
		currentCash = 0;
	}

#region STARTUP / INITIALIZATION FUNCTIONS 
    void Start () {
		PipeCosts = new Dictionary<string, int> ();
		PipeCosts.Add ("ButtonPipeStraight", 100);
		PipeCosts.Add ("ButtonPipeCurve", 200);
		PipeCosts.Add ("ButtonPipeUp", 300);
		PipeCosts.Add ("ScissorCut", 100);
		PipeCosts.Add ("Bait", 200);
	}

    /*public void Init(int currentCash, int lowerCashLimit, int upperCashLimit)
    {
        this.currentCash = currentCash;
        this.lowerCashLimit = lowerCashLimit;
        this.upperCashLimit = upperCashLimit;
    }*/
    #endregion

#region CURRENT CASH VALUE FUNCTIONS
    /// <summary>
    /// <para>Increment the current cash value.</para> 
    /// <para>Returns true if operation succeeds.</para> 
    /// <para>Will fail and return false if operation goes above upper cash limit or below lower cash limit.</para>
    /// </summary>
    /// <param name="incValue"></param>
    /// <returns></returns>
    public bool AddCash(int incValue)
    {
        bool result = true;
        if((currentCash + incValue) < lowerCashLimit)
        {
            result = false;
            Debug.Log("Attempted cash decrement went below lower limit");
        }
        else if ((currentCash + incValue) > upperCashLimit)
        {
            result = false;
            Debug.Log("Attempted cash increment went above upper limit");
        }
        else
        {
            currentCash += incValue;
			UpdateCashText ();
        }
        return result;
    }

    /// <summary>
    /// <para>Set the current cash value.</para>
    /// <para>Returns true if operation succeeds.</para> 
    /// <para>Will fail and return false if operation goes above upper cash limit or below lower cash limit.</para>
    /// </summary>
    /// <param name="setValue"></param>
    /// <returns></returns>
    public bool SetCurrentCashValue(int setValue)
    {
        bool result = true;
        if (setValue < lowerCashLimit)
        {
            result = false;
            Debug.Log("Attempted cash set went below lower limit");
        }
        else if (setValue > upperCashLimit)
        {
            result = false;
            Debug.Log("Attempted cash set went above upper limit");
        }
        else
        {
            currentCash = setValue;
			UpdateCashText ();
        }
        return result;
    }

	public bool RemoveCash(int Cash) {
		bool result = true;
		if((currentCash - Cash) < lowerCashLimit)
		{
			result = false;
			Debug.Log("Attempted cash decrement went below lower limit");
		}
		else
		{
			currentCash -= Cash;
			UpdateCashText ();
		}
		return result;
	}

    /// <summary>
    /// Returns the current cash value.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCashValue()
    {
        return currentCash;
    }
#endregion

#region CASH LIMIT FUNCTIONS
    /// <summary>
    /// Return the lower cash limit value.
    /// </summary>
    /// <returns></returns>
    public int GetLowerCashLimit()
    {
        return lowerCashLimit;
    }

    /// <summary>
    /// Return the upper cash limit value.
    /// </summary>
    /// <returns></returns>
    public int GetUpperCashLimit()
    {
        return upperCashLimit;
    }

    /// <summary>
    /// Set the lower cash limit value.
    /// </summary>
    /// <param name="setValue"></param>
    public void SetLowerCashLimit(int setValue)
    {
        lowerCashLimit = setValue;
    }

    /// <summary>
    /// Set the upper cash limit value.
    /// </summary>
    /// <param name="setValue"></param>
    public void SetUpperCashLimit(int setValue)
    {
        upperCashLimit = setValue;
    }

    /// <summary>
    /// Set both cash limit values (upper and lower)
    /// </summary>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    public void SetCashLimits(int lowerLimit, int upperLimit)
    {
        lowerCashLimit = lowerLimit;
        upperCashLimit = upperLimit;
    }
#endregion

	private void UpdateCashText() {
		cashText.text = "$" + currentCash;
	}

	// Returns cost of pipe, returns -1 if name was not found in dictioanry
	public int GetPipePrice(string PipeName) {
		if (PipeCosts.ContainsKey (PipeName)) {
			return PipeCosts [PipeName];
		} else {
			Debug.LogError ("Pipe name does not exist!");
			return -1;
		}
	}

	public bool CanAfford(int Money) {
		return ((currentCash - Money) >= lowerCashLimit);
	}

}
