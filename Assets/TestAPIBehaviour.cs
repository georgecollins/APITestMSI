using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestAPIBehaviour : MonoBehaviour {

    int one_time_flag = 0;

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

    // Company Info
    // https://iextrading.com/developer/docs/#company
    // example /stock/aapl/company

    public class CompanyInfo
    {
        public string symbol;
        public string companyName;
        public string exchange;
        public string industry;
        public string website;
        public string description;
        public string CEO;
        public string issueType;
        public string sector;
        public string tags;  // an array - it probably won't read correctly
    }

    // Use this for initialization
    void Start () {
        // StartCoroutine(GetText());

        StartCoroutine(GetCompanyInfo());
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


        }
    }

    IEnumerator GetCompanyInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.iextrading.com/1.0/stock/aapl/company");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            string s = System.Text.Encoding.UTF8.GetString(results);
            CompanyInfo info = JsonUtility.FromJson<CompanyInfo>(s);
            GameObject co_disp = GameObject.Find("CompanyText");
            co_disp.GetComponent<Text>().text = info.description;

        }
    }
    // Update is called once per frame

    void Update ()
    {
        GetCompanyInfo();
	}
}
