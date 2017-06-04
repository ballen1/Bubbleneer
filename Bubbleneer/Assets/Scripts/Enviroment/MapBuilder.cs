using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    public Text squidNumText;
    public int squidNum = 0;

    public GameObject crab1;
    public GameObject crab2;
    public GameObject crab3;

    public static float TILE_DIMENSION = 3.2f;

    public NodeManager nodeManager;

    public GroundManager groundManager;

    public GameObject[] objects;
    public string[] words;
    enum pieces { GA, GB, GC, GD, GE, GF, CA, CB, IA, IB, SA, SB, BA, BB, PA, PB, PC, PD, WA, QA, VA, VB, VC };
    enum directions { N = 180, S = 0, E = -90, W = 90 };

    private int MapZDimension;
    private int MapXDimension;
    private int MapYDimension;

    private int StartingCash;
    private CashManager Bank;

    private int MinimumSucceedScore;
    private RoundSystem Rounds;

    void Awake()
    {
        Bank = GetComponent<CashManager>();
        Rounds = GetComponent<RoundSystem>();
    }

    public void ParseMap(BubbleLevel Map)
    {

        if (Bank == null)
        {
            Bank = GetComponent<CashManager>();
        }
        if (Rounds == null)
        {
            Rounds = GetComponent<RoundSystem>();
        }

        TextAsset textFile = (TextAsset)Resources.Load("MapFiles/" + Map.GetLevelFileName()) as TextAsset;
        string mapText = textFile.text;

        char[] delimiterChars = { ',', '\n', '\0' };
        words = mapText.Split(delimiterChars);

        Rounds.SetMapName(Map.GetLevelName());

        MapZDimension = Int32.Parse(words[0]);
        MapXDimension = Int32.Parse(words[1]);
        MapYDimension = Int32.Parse(words[2]);

        StartingCash = Int32.Parse(words[3]);

        MinimumSucceedScore = Int32.Parse(words[4]);
        Rounds.SetMinimumSucceedScore(MinimumSucceedScore);

        Rounds.SetRoundTimes(float.Parse(words[5]), float.Parse(words[6]));

        int y = 0;
        int i = 7;
        //loop through the entire map file add instantiate each piece
        while (i < words.Length)
        {
            for (int z = MapZDimension - 1; z >= 0; z--)
            {
                for (int x = 0; x < MapXDimension; x++)
                {
                    if (words[i].Substring(0, 2).Equals("IN"))
                    {
                        y++;
                        x--;
                    }
                    //build piece if it is not blank
                    else if (!words[i].Substring(0, 2).Equals("00"))
                    {
                        int pieceIndex = (int)(pieces)Enum.Parse(typeof(pieces), words[i].Substring(0, 2));
                        int dir = (int)(directions)Enum.Parse(typeof(directions), words[i][2].ToString());

                        GameObject newPiece = Instantiate(objects[pieceIndex], new Vector3(x * 3.2f, y * 3.2f, z * 3.2f), objects[pieceIndex].transform.rotation);
                        newPiece.transform.Rotate(new Vector3(0, dir, 0));

                        //drop items on level 2 and up
                        if (y > 0)
                        {
                            newPiece.transform.position -= new Vector3(0, y * 0.2f, 0);
                        }

                        //drop crevices down
                        if (words[i][0].Equals('B') || words[i][0].Equals('Q'))
                        {
                            newPiece.transform.position -= new Vector3(0, 3, 0);
                        }

                        //Add terminals to 3D pipe array
                        if (words[i][0].Equals('P'))
                        {
                            if (words[i][1].Equals('C') || words[i][1].Equals('D'))
                            {
                                newPiece.tag = "Occupied";
                            }
                            nodeManager.insertPipe(newPiece, words[i][1].ToString(), words[i][2].ToString());
                        }

                        //add ground to groundArray
                        if ((words[i].Substring(0, 2).Equals("GA") || words[i][0].Equals('V')))
                        {
                            //spawn crabs with ground underneath them
                            groundManager.insertGround(newPiece.transform, newPiece);
                            if (words[i].Substring(0, 2).Equals("VA"))
                            {
                                GameObject crabObj = Instantiate(crab1, newPiece.transform.position + new Vector3(0, 0.25f, 0), newPiece.transform.rotation);
                                crabObj.GetComponent<CrabController>().parentSquare = newPiece;
                            }
                            else if (words[i].Substring(0, 2).Equals("VB"))
                            {
                                GameObject crabObj = Instantiate(crab2, newPiece.transform.position + new Vector3(0, 0.25f, 0), newPiece.transform.rotation);
                                crabObj.GetComponent<CrabController>().parentSquare = newPiece;
                            }
                            else if (words[i].Substring(0, 2).Equals("VC"))
                            {
                                GameObject crabObj = Instantiate(crab3, newPiece.transform.position, newPiece.transform.rotation);
                                crabObj.GetComponent<CrabController>().parentSquare = newPiece;
                            }
                        }
                        //count the number of squids on a map
                        if (words[i].Substring(0, 2).Equals("QA"))
                        {
                            squidNum++;
                        }
                    }
                    i++;
                }
            }
        }

        squidNumText.text = "x" + squidNum;
    }

    void Start()
    {
        Bank.SetCurrentCashValue(StartingCash);
    }

    public int GetXDimension()
    {
        return MapXDimension;
    }

    public int GetZDimension()
    {
        return MapZDimension;
    }

    public int GetYDimension()
    {
        return MapYDimension;
    }
}
