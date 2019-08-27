using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class CrosswordsAlgorithm : Practice {


    public RectTransform tileParent;
	public CrosswordTile tilePrefab;
	
	private List<CrossedWord> CrToShow = new List<CrossedWord>();
	
	private List<CrosswordTile> tileslist  = new List<CrosswordTile>();

    Vector2 tileSize;
    Rect rect;


    void Start()
    {
        rect = RectTransformUtility.PixelAdjustRect(tileParent, transform.root.GetComponent<Canvas>());
        Dico.AddPack("food");
        GenerateCrossWords();
        DrawGameTest();
    }
	
	public void GenerateCrossWords()
	{
		List<CrossedWord> fixedwordsList = new List<CrossedWord>();

        Debug.Log("Should make the full list accessible instead of that");
        List<string> words = Dico.GetRandomKeys(10);

        foreach (string key in words)
		{
            fixedwordsList.Add(new CrossedWord(Dico.Foreign(key) , key));
		}
		
		
		// The very final crosswords
		
		List<CrossedWord> CrossWordsToKeep = new List<CrossedWord>();
			
		float crosswordLength = float.MaxValue;
		int WordsPlaced = 0;
		int crosswordMinX = 0, crosswordMinY = 0;
		
		// Looping to choose the best grid (looping arbitrary "the number of words" time) 
		
		for(int gen = 0 ;  gen < fixedwordsList.Count ; gen++)
		{
			// Not Touching the initial List
			List <CrossedWord> allWords = new List<CrossedWord>(fixedwordsList);
			
			if(gen % 2 == 1)
			{
				// Shuffling the words half the tries because sometimes not starting with the longer word can be a good option too
				for(int j = allWords.Count - 1 ; j > 0; j--)
				{
					int r = UnityEngine.Random.Range(0, j+1);
					CrossedWord tmp = allWords[r];
					allWords[r] = allWords[j];
					allWords[j] = tmp;
				}
			}
			else
			{
				allWords.Sort();
				allWords.Reverse();
			}
			
			// The final crosswords for this loop only
			List<CrossedWord> finalWords = new List<CrossedWord>();
			
			// Adding the first word we found
			finalWords.Add(new CrossedWord(allWords[0]));
			// Removing this word from the list
			allWords.RemoveAt(0);
			
			// Initial size knowing the fact that the first word is Horizontal
			int minX = 0, maxX = finalWords[0].Size - 1, minY = 0, maxY = 0;
			
			// Loop on all the words we want in the crossword
			int maxLoop = Mathf.FloorToInt(allWords.Count*allWords.Count );
			
			int z = 0;
			int i = 0;
			for(; 0 != allWords.Count && z < maxLoop ;z++)
			{
				
				// The current word we want to place 
				CrossedWord currentWordToPlace = new CrossedWord(allWords[i]);
				
				
				// Will tell us if we succeed placing it
				bool bIsPlaced = false;
				
				// Will always be the best position we find, initialise here with arbitrary values
				Tile BestStartingPosition = new Tile(0,0);
				CrossedWord.Direction BestDirection = CrossedWord.Direction.Horizontal;
				
				// Will be a score to tell us which position is "conceptually" the best
				float score = float.MaxValue;
				
				// Loop on all the existing words in the crossword
				for(int j = 0; j< finalWords.Count; j++)
				{
					// The current already placed word that we will used to try to place the new word
					CrossedWord currentWordPlaced = finalWords[j];
					
					// If we must placed the new one according to the existing one, the new one will be the other direction
					currentWordToPlace.WordDirection = currentWordPlaced.WordDirection == CrossedWord.Direction.Horizontal ? CrossedWord.Direction.Vertical : CrossedWord.Direction.Horizontal;
					
					// An array wich gave us for each letter (e.g. array[0] for the first letter) the tiles on which the current placed word has the same letter 
					List<Tile>[] intersectionForEachLetter = currentWordPlaced.SimilarLetterTiles(currentWordToPlace);
					
					// Loop on all the letters
					for(int k = 0; k < intersectionForEachLetter.Length;k++)
					{
						// Looking for each given tile for one letter
						for(int l = 0; l < intersectionForEachLetter[k].Count; l++)
						{
							// Getting the tile
							Tile currentCommonTile = intersectionForEachLetter[k][l];
							
							// Given the direction of the placed word and the intersection tile we calculate the new word potential starting position
							if( currentWordPlaced.WordDirection == CrossedWord.Direction.Horizontal )
							{
								currentWordToPlace.StartingPosition = new Tile(currentCommonTile.X, currentCommonTile.Y - k);
							}
							else
							{
								currentWordToPlace.StartingPosition = new Tile(currentCommonTile.X -k , currentCommonTile.Y);
							}
							
							// Loop on all the words in the crossword to check if the place we want the new word isn't in conflict with the existings words
							// the int to tell us how many correct intersection we have
							int iCanBePlaced = 0;
							// the boolean to tell us a conflict
							bool bCanBePlaced = true;
							for(int m = 0; m < finalWords.Count && bCanBePlaced; m++)
							{
								// ca = 0 means no conflict, -1 means a conflict, 1 means a good intersection
								int ca = finalWords[m].CanAccept(currentWordToPlace);
								if(ca > 0)
									iCanBePlaced += ca;
								if(ca == -1)
									bCanBePlaced = false;
							}
							
							// The place is OK and have minimum one good intersection
							if(bCanBePlaced && iCanBePlaced > 0)
							{
								// Calculate a score to find the best place
								
								// how much intersection but the opposite value
								int crossedNumber = (0 - iCanBePlaced);
								
								// a conceptual score that should be the less the better
								float tmpScore =  UnityEngine.Random.Range(0,10) + crossedNumber *100; 
								
								// if this score si better than a previous one we keep this position and tell that we succeed placing at least one time this word
								if( tmpScore < score)
								{
									bIsPlaced = true;
									
									// Updating the new best score
									score = tmpScore;
									BestStartingPosition = currentWordToPlace.StartingPosition;
									BestDirection = currentWordToPlace.WordDirection;
								}
							}
						}
					}
				}
				
				// We have at least one position to place this new word
				if(bIsPlaced)
				{
					// getting this saved position
					currentWordToPlace.StartingPosition = BestStartingPosition;
					currentWordToPlace.WordDirection = BestDirection;
					// adding this word to the crossword
					finalWords.Add(currentWordToPlace);
					
					// Shuffling the crossword array to have more random factor on the grid creation (doesn't really matters but linear operation so it's ok)
					for(int j = finalWords.Count - 1 ; j > 0; j--)
					{
						int r = UnityEngine.Random.Range(0, j+1);
						CrossedWord tmp = finalWords[r];
						finalWords[r] = finalWords[j];
						finalWords[j] = tmp;
					}
					
					// Updating the grid Rectangle if necessary
					minX = Mathf.Min(minX, currentWordToPlace.StartingPosition.X);
					minY = Mathf.Min(minY, currentWordToPlace.StartingPosition.Y);
					
					maxX = Mathf.Max(maxX, currentWordToPlace.WordDirection == CrossedWord.Direction.Horizontal ? currentWordToPlace.StartingPosition.X + currentWordToPlace.Size - 1 : currentWordToPlace.StartingPosition.X);
					maxY = Mathf.Max(maxY, currentWordToPlace.WordDirection == CrossedWord.Direction.Vertical ? currentWordToPlace.StartingPosition.Y + currentWordToPlace.Size - 1 : currentWordToPlace.StartingPosition.Y);
				
					allWords.RemoveAt(i);
					if(allWords.Count > 0)
						i = i % allWords.Count;
				}
				else
				{
				 	i = (i+1) % allWords.Count;	
				}
			}
			
			// Final new length of the grid
			float newLength = Mathf.Sqrt((maxX - minX)*(maxX - minX) + (maxY - minY)*(maxY - minY));
			// Final new number of word we succeed to put on the grid
			int currentWordsPlaced = finalWords.Count;
			
			// if it's a better grid (smaller and more words). Indeed, we allow a bigger crossword proportionally to how much more words it contains
			if(newLength - (currentWordsPlaced - WordsPlaced)*4 < crosswordLength  && WordsPlaced < currentWordsPlaced)
			{
				// Keeping this grid in memory
				CrossWordsToKeep = finalWords;
				// Updating best grid values
				crosswordLength = newLength;

                WordsPlaced = currentWordsPlaced;
				
				crosswordMinX = minX;
				crosswordMinY = minY;

                float siz = Mathf.Min(rect.width / (maxX - minX + 2), rect.height / (maxY - minY + 2));
                tileSize = Vector2.one * siz;
            }
		
		}
		
		//Debug.Log(CrossWordsToKeep.Count+"/"+fixedwordsList.Count+" size: "+crosswordLength);
		
		for(int r = 0; r < CrossWordsToKeep.Count; r++)
		{
			CrossWordsToKeep[r].StartingPosition.X -= crosswordMinX;
			CrossWordsToKeep[r].StartingPosition.Y = -CrossWordsToKeep[r].StartingPosition.Y + crosswordMinY;
		}
		
		CrToShow = CrossWordsToKeep;

    }


    public void DrawGameTest()
	{
		foreach(CrosswordTile go in tileslist)
		{
			Destroy(go.gameObject);
		}
		tileslist.Clear();

        CrosswordTile clue = null;
		foreach(CrossedWord crw in CrToShow)
		{
			for(int i = -1 ; i < crw.Size; i++)
			{
                Vector2 p;
                if (crw.WordDirection == CrossedWord.Direction.Horizontal)
				{
                    p = new Vector2((crw.StartingPosition.X + i + 1), crw.StartingPosition.Y - 1);
				}
				else
				{
                    p = new Vector2(crw.StartingPosition.X + 1, (crw.StartingPosition.Y - i - 1));
                }

                bool bAlreadyOnBoard = false;
                for (int j = 0; j < tileslist.Count && !bAlreadyOnBoard; j++)
                {
                    if (tileslist[j].pos == p)
                    {
                        bAlreadyOnBoard = true;
                        clue.tiles.Add(tileslist[j]);
                        break;
                    }
                }
                if (!bAlreadyOnBoard)
                {
                    CrosswordTile aTile = Instantiate(tilePrefab, tileParent);
                    aTile.Init(tileSize, p);
                    if (i == -1)
                    {
                        aTile.SetClue(crw.Key);
                        clue = aTile;
                    }
                    else
                    {
                        aTile.solution = crw.Word[i].ToString().ToUpper();
                        clue.tiles.Add(aTile);
                    }
                    tileslist.Add(aTile);
                }
            }
		}
	}
	
	//private void OnGUI()
	//{
	//	if(GUI.Button(new Rect(400,0,200,200),"Retry"))
	//	{
	//		GenerateCrossWords();
	//		DrawGameTest();
	//	}
	//}
}
