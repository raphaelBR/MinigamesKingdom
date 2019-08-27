using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CrossedWord : IComparable
{
	public enum Direction
	{
		Horizontal,
		Vertical
	}
	
	private string _word;
	private string _key;

	private Tile _startingPosition;
	private Direction _wordDirection;
	
	
	public string Word
	{
		get {return _word;}
		set {_word = value;}
	}
	
	public string Key
	{
		get {return _key;}
		set {_key = value;}
	}
	
	public int Size
	{
		get{return _word.Length;}
	}
	
	public Tile StartingPosition
	{
		get{return _startingPosition;}
		set{_startingPosition = value;}
	}
	
	public Direction WordDirection
	{
		get{return _wordDirection;}
		set{_wordDirection = value;}
	}
	
	public CrossedWord(string word, string key){
		Word = word;
		Key = key;
		WordDirection = Direction.Horizontal;
		StartingPosition = new Tile(0,0);
	}
	
	public CrossedWord(CrossedWord previous){
		Word = previous.Word;
		Key = previous.Key;
		WordDirection = previous.WordDirection;
		StartingPosition = new Tile(previous.StartingPosition.X, previous.StartingPosition.Y);
	}	
	
	public int CompareTo(object obj)
	{
		if(obj == null) return 1;
		
		CrossedWord otherCrossWord = obj as CrossedWord;
		if(otherCrossWord != null)
		{
			return Size.CompareTo(otherCrossWord.Size);	
		}
		else
		{
			throw new ArgumentException("Object is not a CrossedWord");	
		}		
	}
	
	// Give the letter at given Tile position, '0' if this word is not over this tile (used for a more simple utilisation of this class)
	public char LetterOnTile(Tile t)
	{
		if(isWordOverTile(t))
		{
			switch(WordDirection)
			{
				case Direction.Horizontal : return Word[t.X - StartingPosition.X];
				case Direction.Vertical : return Word[t.Y - StartingPosition.Y];
				default : return '0';
			}
		}
		else
		{
			return '0';	
		}
	}
	
	// Give the tile under the "pos" position of the word string
	public Tile TileAtWordPosition(int pos)
	{
		if(pos >= 0 && pos < Size)
		{
			switch(WordDirection)
			{
				case Direction.Horizontal : return new Tile(StartingPosition.X + pos, StartingPosition.Y);
				case Direction.Vertical : return new Tile(StartingPosition.X, StartingPosition.Y + pos);
				default : throw new MissingMemberException("This Word has no direction");
			}
		}
		else
		{
			throw new ArgumentOutOfRangeException();	
		}
	}
	
	// Tells us if one of the words letter is on the given Tile
	public bool isWordOverTile(Tile t)
	{
		return (WordDirection == Direction.Horizontal && t.Y == StartingPosition.Y && t.X >= StartingPosition.X && t.X < StartingPosition.X + Size) 
			|| (WordDirection == Direction.Vertical && t.X == StartingPosition.X && t.Y >= StartingPosition.Y && t.Y < StartingPosition.Y + Size) ;
	}
	
	// An array wich gave us for each letter of c (e.g. array[0] for the first letter) the tiles on which the current instance has the same letter
	public List<Tile>[] SimilarLetterTiles(CrossedWord c)
	{
		List<Tile>[] tilesForEachLetter = new List<Tile>[c.Size];
		for(int i = 0; i< c.Size ; i++)
		{
			List<Tile> TilesForCurrentLetter = new List<Tile>();
			for(int j = 0; j<Size; j++)
			{
				if(c.Word[i] == Word[j])
					TilesForCurrentLetter.Add(TileAtWordPosition(j));
			}
			tilesForEachLetter[i] = TilesForCurrentLetter;
		}
		return tilesForEachLetter;
	}
	
	// Tells if instance can accept the crossword (no superposition...)
	public int CanAccept(CrossedWord c)
	{
		// BOTH HORIZONTAL 
		if(WordDirection == Direction.Horizontal && c.WordDirection == Direction.Horizontal )
		
			// Having more than 1 line between them
			if( Math.Abs(c.StartingPosition.Y - StartingPosition.Y) > 1 )
				return 0;
        
        if (Math.Abs(c.StartingPosition.Y - StartingPosition.Y) <= 2)
            return -1;

        // Having less than 1 line between them but not touching nor supersposing
        if ( Math.Abs(c.StartingPosition.Y - StartingPosition.Y) <= 2 && (StartingPosition.X > c.StartingPosition.X + c.Size || StartingPosition.X + Size < c.StartingPosition.X)) 
			return 2;
						
		// BOTH VERTICAL 
		if(WordDirection == Direction.Vertical && c.WordDirection == Direction.Vertical )
		
			// Having more than 1 row between them
			if( Math.Abs(c.StartingPosition.X - StartingPosition.X) > 1  ) 
				return 0;

        if (Math.Abs(c.StartingPosition.X - StartingPosition.X) <= 2)
            return -1;

        // Having less than 1 row between them but not touching nor supersposing
        if ( Math.Abs(c.StartingPosition.X - StartingPosition.X)  <= 2  && (StartingPosition.Y > c.StartingPosition.Y + c.Size || StartingPosition.Y + Size < c.StartingPosition.Y))
			return 2;
						
		// INSTANCE HORIZONTAL AND OTHER VERTICAL
		if(WordDirection == Direction.Horizontal && c.WordDirection == Direction.Vertical)
		{
			Tile potentialIntersection = new Tile(c.StartingPosition.X, StartingPosition.Y);
			
			char instanceChar = LetterOnTile(potentialIntersection);
			char otherChar = c.LetterOnTile(potentialIntersection);
			// IF CROSSING ON THE SAME LETTER --> TRUE
			if(isWordOverTile(potentialIntersection) && c.isWordOverTile(potentialIntersection) && instanceChar == otherChar)
			{
				if(instanceChar != '0')
					return 1;
				else
					return 0;
			}
			else if(instanceChar == '0' && (potentialIntersection.X < StartingPosition.X - 1 || potentialIntersection.X > StartingPosition.X + Size))
			{
				return 0;
			}
		}
		
		// INSTANCE VERTICAL AND OTHER HORIZONTAL
		if(WordDirection == Direction.Vertical && c.WordDirection == Direction.Horizontal)
		{
			Tile potentialIntersection = new Tile(StartingPosition.X, c.StartingPosition.Y);
			
			char instanceChar = LetterOnTile(potentialIntersection);
			char otherChar = c.LetterOnTile(potentialIntersection);
			// IF CROSSING ON THE SAME LETTER --> TRUE
			if(isWordOverTile(potentialIntersection) && c.isWordOverTile(potentialIntersection) && instanceChar == otherChar)
			{
				if(instanceChar != '0')
					return 1;
				else
					return 0;
			}
			else if(instanceChar == '0' && (potentialIntersection.Y < StartingPosition.Y - 1 || potentialIntersection.Y > StartingPosition.Y + Size))
			{
				return 0;
			}
		}
		
				
		return -1;
	}
}
