using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(RCCDashboardInputs))]

public class RCCUIDashboardDisplay : MonoBehaviour {

	private RCCDashboardInputs inputs;
	
	public Text RPMLabel;
	public Text KMHLabel;
	public Text GearLabel;
	
	void Start () {
		
		inputs = GetComponent<RCCDashboardInputs>();
		
	}
	
	
	void Update () {
		
		RPMLabel.text = inputs.RPM.ToString("0");
		KMHLabel.text = inputs.KMH.ToString("0");
		GearLabel.text = inputs.Gear >= 0 ? (inputs.Gear + 1).ToString("0") : "R";
	}

}
