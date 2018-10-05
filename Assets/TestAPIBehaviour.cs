using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestAPIBehaviour : MonoBehaviour {


    public class EarningsArray
    {
        public string symbol { get; set; }
        public EarningsDataResponse[] earnings { get; set; }
    }

    public class EarningsDataResponse
    {
        public double actualEPS { get; set; } // Actual earnings per share for the period
        public double consensusEPS { get; set; }  // Consensus EPS estimate trend for the period
        public double estimatedEPS { get; set; } // Earnings per share estimate for the period
        public string announceTime { get; set; } // Time of earnings announcement.BTO(Before open), DMT(During trading), AMC(After close)
        public int numberOfEstimates { get; set; } //  Number of estimates for the period
        public double EPSSurpriseDollar { get; set; } // Dollar amount of EPS surprise for the period
        public string EPSReportDate { get; set; } // string Expected earnings report date YYYY-MM-DD
        public string fiscalPeriod { get; set; }   // The fiscal quarter the earnings data applies to Q# YYYY
        public string fiscalEndDate { get; set; }// representing the company fiscal quarter end YYYY-MM-DD
        public double yearAgo { get; set; }    // Represents the EPS of the quarter a year ago
        public double yearAgoChangePercent { get; set; } // Represents the percent difference between the quarter a year ago actualEPS and current period actualEPS.
        public double estimatedChangePercent { get; set; } //  Represents the percent difference between the quarter a year ago actualEPS and current period estimatedEPS.
        public int symbolId { get; set; }  // Represents the IEX id for the stock

    }
    // Use this for initialization
    void Start () {
        StartCoroutine(GetText());
	}

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.iextrading.com/1.0/stock/aapl/earnings/1");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show result as text
            Debug.Log(www.downloadHandler.text);
            // or retrieve resuls as binary data
            byte[] results = www.downloadHandler.data;

            string s = System.Text.Encoding.UTF8.GetString(results);

            // Object info = JsonUtility.FromJson<Object>(s);

            EarningsArray info = JsonUtility.FromJson<EarningsArray>(s);

            // var historicalDataList = response.Content.ReadAsAsync<List<HistoricalDataResponse>>().GetAwaiter().GetResult();
            Debug.Log(info);

            Debug.Log(s);

            EarningsDataResponse edr = JsonUtility.FromJson<EarningsDataResponse>(s);
            Debug.Log(edr.announceTime);

            string temp = "{ \"actualEPS\":2.34,\"consensusEPS\":2.17,\"estimatedEPS\":2.17,\"announceTime\":\"AMC\",\"numberOfEstimates\":10,\"EPSSurpriseDollar\":0.17,\"EPSReportDate\":\"2018-07-31\",\"fiscalPeriod\":\"Q3 2018\",\"fiscalEndDate\":\"2018-06-30\",\"yearAgo\":1.67,\"yearAgoChangePercent\":0.40119760479041916,\"estimatedChangePercent\":0.29940119760479045,\"symbolId\":11}";
            EarningsDataResponse edr2 = JsonUtility.FromJson<EarningsDataResponse>(temp);

            Debug.Log(edr2.announceTime);

            Debug.Log("this is drving crazy" + temp);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
