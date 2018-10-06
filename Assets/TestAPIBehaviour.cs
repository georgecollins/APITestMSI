using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestAPIBehaviour : MonoBehaviour {


    public class EarningsDataResponse
    {
        public double actualEPS;   // Actual earnings per share for the period
        public double consensusEPS;  // Consensus EPS estimate trend for the period
        public double estimatedEPS; // Earnings per share estimate for the period
        public string announceTime;// Time of earnings announcement.BTO(Before open), DMT(During trading), AMC(After close)
        public int numberOfEstimates; //  Number of estimates for the period
        public double EPSSurpriseDollar; // Dollar amount of EPS surprise for the period
        public string EPSReportDate; // string Expected earnings report date YYYY-MM-DD
        public string fiscalPeriod;   // The fiscal quarter the earnings data applies to Q# YYYY
        public string fiscalEndDate; // representing the company fiscal quarter end YYYY-MM-DD
        public double yearAgo;    // Represents the EPS of the quarter a year ago
        public double yearAgoChangePercent; // Represents the percent difference between the quarter a year ago actualEPS and current period actualEPS.
        public double estimatedChangePercent;//  Represents the percent difference between the quarter a year ago actualEPS and current period estimatedEPS.
        public int symbolId;  // Represents the IEX id for the stock

    }
    // Use this for initialization
    void Start () {
        StartCoroutine(GetText());
	}

    public class JsonExample
    {
        public int id;
        public string name;
        public double data;
    }

    public class JsonTest
    {
        public string name;
    }

    public class StockTest
    {
        public string symbol;
        // public double actualEPS;
        // public string fiscalPeriod;
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
            byte[] parsed = new byte[1024];

            int count = 0;
            do
            {
                count++;
            } while (results[count] != 91);
            // 0b1011101);

            count++;
            int newcount = 0;
            do
            {
                parsed[newcount] = results[count];
                count++;
                newcount++;
            } while (results[count] != 125);
            parsed[newcount] = 125;

            string p = System.Text.Encoding.UTF8.GetString(parsed);
            Debug.Log("Tried to parse results");
            Debug.Log(p);

            EarningsDataResponse resp = JsonUtility.FromJson<EarningsDataResponse>(p);

            Debug.Log(resp.actualEPS);


            string s = System.Text.Encoding.UTF8.GetString(results);

      
            Debug.Log(s);


            Debug.Log("Info from non paresed");
            EarningsDataResponse edr = JsonUtility.FromJson<EarningsDataResponse>(s);
            Debug.Log(edr.announceTime);

            string temp = "{\"actualEPS\":2.34,\"consensusEPS\":2.17,\"estimatedEPS\":2.17,\"announceTime\":\"AMC\",\"numberOfEstimates\":10,\"EPSSurpriseDollar\":0.17,\"EPSReportDate\":\"2018-07-31\",\"fiscalPeriod\":\"Q3 2018\",\"fiscalEndDate\":\"2018-06-30\",\"yearAgo\":1.67,\"yearAgoChangePercent\":0.40119760479041916,\"estimatedChangePercent\":0.29940119760479045,\"symbolId\":11}";
            EarningsDataResponse edr2 = JsonUtility.FromJson<EarningsDataResponse>(temp);
            Debug.Log("single line parsed" + temp);

            Debug.Log(edr2.actualEPS);


            // Scratch tests

            /*
            JsonExample j = new JsonExample();
            j.id = 12;
            j.name = "Fred";
            j.data =3.141;

            s = JsonUtility.ToJson(j);


            Debug.Log(s);

            JsonExample aread = JsonUtility.FromJson<JsonExample>(s);

            Debug.Log(aread.name);

            JsonTest tread = JsonUtility.FromJson<JsonTest>(s);

            Debug.Log(tread.name);
            */
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
