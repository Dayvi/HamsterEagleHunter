using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Amplitude - Factors into the height.
// Persistance - Factors into the height.
// Frequency - How close together the hills are.
// Octaves - How smooth the terrain is.


public class TerrainGenerator : MonoBehaviour 
{
	#region Fields
	
	// References
	Terrain terrain;
	
	int terrainWidth;
	int terrainHeight;
	int terrainAlphaWidth;
	int terrainAlphaHeight;
	int playableWidth;
	int playableHeight;
	float randomSeed;

	public float seed;
	
	int numberOfImportantAreas = 5;
	
	// Perlin noise
	public float amplitude;
	public float frequency;
	public float persistance;
	public float octaves;
	public float tileSize;
	public float height;
	
	enum Areas { None, Start };
	enum TileTypes { Boundary, Grass, Dirt, River }
	
	// Debug Variables
	public bool lockTerrain; // Locks the terrain, for testing purposes only.
	
	#endregion
	
	#region Unity Functions
	
	void Start() 
	{
		terrain = GetComponent<Terrain>();
		
		// Get the size of the terrain
		terrainWidth  = terrain.terrainData.heightmapWidth;
		terrainHeight = terrain.terrainData.heightmapHeight;
		terrainAlphaWidth  = terrain.terrainData.alphamapWidth;
		terrainAlphaHeight = terrain.terrainData.alphamapWidth;
		playableWidth =  terrainWidth / 3;
		playableHeight = terrainHeight / 3;
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 100, 30), "Generate"))
		{
			bool generated = false; // Fail safe retry mechanism for if the generation fails
			int tries = 0; // For testing purposes. On final build there should never be chance for an infinte loop here.
			
			// Keep trying until we get a map without an error.
			while (!generated)
			{
				try
				{
					generated = Generate();
				}
				catch (System.Exception exc)
				{
					Debug.LogException(exc);
				}
				
				tries++;
				
				if (tries > 40) // After 40 tries cancel generation.
				{
					Debug.Log("Infinite Loop");
					break;
				}
			}
		}
	}
	
	void OnDestroy()
	{
		// If not locked
		if (!lockTerrain)
			Reset();
	}
	
	#endregion
	
	#region Functions
	
	// Returns a bool because on occasion this process fails
	bool Generate() 
	{
		#region Generate Terrain
		
		float[,] terrainHeights = new float[terrainWidth, terrainHeight]; // A temporary heightmap that will be written to the terrain at the end.
		TileTypes[,] tiles = new TileTypes[playableWidth, playableHeight]; // A map of all the terrain types which gets converted into textures.
		
		Reset(); 
		
		// NOTE: Fix this so that any number will generate a usable seed.
		randomSeed = 2 + seed * seed;
		
		// Get the perlin noise, also raise the terrain by 0.1 so we can add rivers later.
		for (int x = 0; x < terrainWidth; x++)
		{
			for (int y = 0; y < terrainHeight; y++)
			{
				terrainHeights[x, y] = (float)GetHeight(((float)x / (float)terrainWidth) * tileSize, ((float)y / (float)terrainHeight) * tileSize) / height + 0.1f;
			}
		}
		
		#endregion 
		
		#region Generate Areas and Landmark Positions
		
		#region STEP 2: GENERATE BOUNDARY AREA 
		
		// Generate a dot representation of a random polygon.
		Vector2[] polygonPoints = GeneratePolygon(20, new Rect(20, 20, playableWidth - 20, playableHeight - 20));
		List<Vector2> shapeOutline = new List<Vector2>();
		
		// Connect the dots using Bresenham's Line Algorithim
		for (int i = 0; i < polygonPoints.Length; i++)
		{
			if (i != polygonPoints.Length - 1)
				shapeOutline.AddRange(GenerateLine(polygonPoints[i], polygonPoints[i + 1]));
			else
				shapeOutline.AddRange(GenerateLine(polygonPoints[i], polygonPoints[0]));
		}
		
		// Plot the line on the tile grid
		foreach (Vector2 vec in shapeOutline)
		{
			tiles[(int)vec.x, (int)vec.y] = TileTypes.Grass;
		}
		
		// Fill in the polygon
		tiles = FloodFill(new Vector2(playableWidth / 2, playableHeight / 2), TileTypes.Grass, tiles, TileTypes.Boundary);
		
		#endregion
		
		#region STEP 1: MAYBE GENERATE A RIVER
		
		if (true)//Random.value < 0.5)
		{
			List<Vector2> riverPoints = new List<Vector2>();
			
			// Select a start point
			// Pick random direction 0 = up, 1 = right, 2 = down, 3 = left
			int startDirection = Random.Range(0, 3);		
			
			// Select a random start point along the edge of the start direction.
			if (startDirection == 0)
				riverPoints.Add(new Vector2(Random.Range(0, playableWidth), 0)); // Up
			else if (startDirection == 1)
				riverPoints.Add(new Vector2(playableWidth, Random.Range(0, playableHeight))); // Right
			else if (startDirection == 2)
				riverPoints.Add(new Vector2(Random.Range(0, playableWidth), playableHeight)); // Down
			else if (startDirection == 3)
				riverPoints.Add(new Vector2(0, Random.Range(0, playableHeight))); // Left
			
			// pick a random mid point
			riverPoints.Add(new Vector2(Random.Range(20, playableWidth - 20), Random.Range(20, playableWidth - 20)));
			
			// Select a end point
			// Pick random direction 0 = up, 1 = right, 2 = down, 3 = left
			int endDirection = startDirection;
				
			// make sure the end and start direction are not the same thing.
			while (endDirection == startDirection)
			{
				endDirection = Random.Range(0, 3);	
			}
			
			// Select a random end point along the edge of the end direction.
			if (endDirection == 0)
				riverPoints.Add(new Vector2(Random.Range(0, playableWidth), 0)); // Up
			else if (endDirection == 1)
				riverPoints.Add(new Vector2(playableWidth, Random.Range(0, playableHeight))); // Right
			else if (endDirection == 2)
				riverPoints.Add(new Vector2(Random.Range(0, playableWidth), playableHeight)); // Down
			else if (endDirection == 3)
				riverPoints.Add(new Vector2(0, Random.Range(0, playableHeight))); // Left
			
			// Generate a curve out of the river points and then gener
			BezierPath riverCurve = new BezierPath();
			riverCurve.SamplePoints((List<Vector2>)riverPoints, 10, 1000, 0.33f);
			List<Vector2> riverCurvePoints = riverCurve.GetDrawingPoints2();
			List<Vector2> riverDrawPoints = new List<Vector2>();
			
			int riverThickness = Random.Range(2, 4);
			
			// Get the points we need to draw to.
			for (int i = 0; i < riverCurvePoints.Count - 1; i++)
			{
				riverDrawPoints.AddRange(GenerateThickLine(riverCurvePoints[i], riverCurvePoints[i + 1], riverThickness));
			}
			
			// Apply the draw points to the map. NOTE: Later apply an interpolating technique to smooth outt the edges of the river terrain.		
			foreach (Vector2 vec in riverDrawPoints)
			{
				if (vec.x > 0 && vec.x < playableWidth && vec.y > 0 && vec.y < playableHeight)
				{
					tiles[(int)vec.x, (int)vec.y] = TileTypes.River;
					
					terrainHeights[(int)vec.x, (int)vec.y] = 0.01f; // make the terrain a little lower than the base height
				}
			}
			
			// Continue to lower the area around the river until it reaches normal height
			bool gapCovered = false; // has a set height between river tiles and normal tiles been reached
			
			while (!gapCovered)
			{		
				gapCovered = true;
				
				for (int x = 0; x < playableWidth; x++)
				{
					for (int y = 0; y < playableHeight; y++)
					{
						if (tiles[x, y] != TileTypes.River)
						{
							// Get the average height of all adjacent river tiles
							float riverTileHeight = 0; // The average height of river tiles next to this one.
							List<float> riverTileHeights = new List<float>();
							
							// Check if it's next to a river tile.
							if (x - 1 > 0 && tiles[x - 1, y] == TileTypes.River) // Left
								riverTileHeights.Add(terrainHeights[x - 1, y]);
							
							if (x + 1 < playableWidth && tiles[x + 1, y] == TileTypes.River) // Right
								riverTileHeights.Add(terrainHeights[x + 1, y]);
							
							if (y - 1 > 0 && tiles[x, y - 1] == TileTypes.River) // Top
								riverTileHeights.Add(terrainHeights[x, y - 1]);
							
							if (y + 1 < playableHeight && tiles[x, y + 1] == TileTypes.River) // Bottom
								riverTileHeights.Add(terrainHeights[x, y + 1]);
							
							/*if (x - 1 > 0  && y - 1 > 0 && tiles[x - 1, y - 1] == TileTypes.River) // Top Left
								riverTileHeights.Add(terrainHeights[x - 1, y - 1]);
							
							if (x + 1 < playableWidth && y - 1 > 0 && tiles[x + 1, y - 1] == TileTypes.River) // Top Right
								riverTileHeights.Add(terrainHeights[x + 1, y - 1]);
							
							if (x - 1 > 0 && y + 1 < playableHeight && tiles[x - 1, y + 1] == TileTypes.River) // Bottom Left
								riverTileHeights.Add(terrainHeights[x - 1, y + 1]);
							
							if (x + 1 < playableWidth && y + 1 < playableHeight && tiles[x + 1, y + 1] == TileTypes.River) // Bottom Right
								riverTileHeights.Add(terrainHeights[x + 1, y + 1]);*/
							
							if (riverTileHeights.Count == 0)
								continue;
							
							// Get the average height of all the tiles around it.
							foreach (float f in riverTileHeights)
								riverTileHeight += f;
							
							riverTileHeight /= riverTileHeights.Count;
												
							// Check its atleast 0.01f from it's neighbor.
							if (terrainHeights[x, y] - riverTileHeight > 0.02f)
							{
								terrainHeights[x, y] = riverTileHeight + 0.02f;
								tiles[x, y] = TileTypes.River;
								gapCovered = false;
							}
						}
					}
				}
			}
		}
		
		#endregion
		
		#region STEP 3: GENERATE IMPORTANT AREAS AND PATHS 
		
		// A list of the location of the major areas. NOTE: At the moment just clearings.
		List<Rect> importantAreas = new List<Rect>();
		
		// Generate starting area.
		int tries = 0; // Prevent infinite loops, If an error occurs may go infinite.
		
		// Pick some random points to be important areas.
		for (int i = 0; i < numberOfImportantAreas; i++)
		{
			bool validArea = false; // If the point is within the boundary
			Rect importantArea = new Rect(0, 0, 0, 0);
			
			while (!validArea)
			{
				validArea = true;
				
				Vector2 point = new Vector2(Random.Range(0, playableWidth), Random.Range(0, playableHeight));
				
				// Check if the point is near any existing areas.
				foreach (Rect rect in importantAreas)
					if (Vector2.Distance(point, new Vector2(rect.x, rect.y)) < 40) // Make sure the area is atleast 5 tiles away from other areas.
						validArea = false;
				
				// Validity of area check.
				if (validArea && tiles[(int)point.x, (int)point.y] == TileTypes.Grass)
				{				
					// Generate a random sized rectangle around the point.
					int width = Random.Range(4, 8);
					int height = Random.Range(4, 8);
					importantArea = new Rect(point.x + (width / 2), point.y - (height / 2), width, height);
					
					// Check if the area collides with the boundary.
					for (int x = (int)importantArea.x; x < importantArea.x + importantArea.width; x++)
						for (int y = (int)importantArea.y; y < importantArea.y + importantArea.height; y++)
							if (x > 0 && x < playableWidth && y > 0 && y < playableWidth)
							{
								if (tiles[x, y] == TileTypes.Boundary || tiles[x, y] == TileTypes.River) // If an index out of range exception appears here it is most likley because the important areas are spread to far apart and it can't find a place to put them.
									validArea = false;
							}
							else
							{
								validArea = false;
							}
				}
				else
					validArea = false;
				
				tries++;
			
				if (tries > 100000) // Break after LOTS of tries.
					return false;
			}
			
			importantAreas.Add(importantArea);
		}
		
		// apply the areas to the tile map :FOR TESTING:
		foreach (Rect rect in importantAreas)
		{
			tiles = EllipseMidpoint((int)(rect.x + (rect.width / 2)), (int)(rect.y + (rect.height / 2)), (int)rect.width, (int)rect.height, tiles);
			
			// Fill in the ellipse.
			tiles = FloodFill(new Vector2(rect.x + (rect.width / 2), (int)(rect.y + (rect.height / 2))), TileTypes.Dirt, tiles, TileTypes.Grass, TileTypes.Boundary);		
		}
		
		#endregion
		
		#region STEP 4: Generate Paths Connecting The Areas
		
		// Pick a random important area, find the closest nodes and pick random ones to connect
		int[] connectionTo = new int[importantAreas.Count]; // What area is connected to what other area
		
		for (int i = 0; i < importantAreas.Count; i++)
		{
			bool connectionFound = false;
			int attempts = 0;

			connectionTo[i] = -1; // Fill connection to blank, we'll need 0
			
			// Pick a random area to connect to.
			while (!connectionFound)
			{
				if (attempts == numberOfImportantAreas - 1) // if we have tried every area then no connection can be found.
					break;
				
				int areaToConnect = Random.Range(0, importantAreas.Count - 1); // Select a random area.
				
				if (areaToConnect != i && connectionTo[areaToConnect] != i) // Check that we are not connecting it to itself and check that what we are connecting to isnt already connected to us.
				{
					connectionTo[i] = areaToConnect; 
					connectionFound = true;
				}
				
				attempts++;
			}
		}
		
		List<Vector2> connectionDrawPoints = new List<Vector2>();
		
		// Connect them via path and curve the path a bit in a random direction
		for (int i = 0; i < connectionTo.Length; i++)
		{
			if (connectionTo[i] == -1) // if no connection
				continue;
			
			Vector2 currentArea = new Vector2(importantAreas[i].x + (importantAreas[i].width / 2), importantAreas[i].y + (importantAreas[i].height / 2));
			Vector2 areaConnectingTo = new Vector2(importantAreas[connectionTo[i]].x + (importantAreas[connectionTo[i]].width / 2), importantAreas[connectionTo[i]].y + (importantAreas[connectionTo[i]].height / 2));
			// Pick a random point along the line the curve out the line
			Vector2 curvePoint = new Vector2(Random.Range(Mathf.Min(currentArea.x, areaConnectingTo.x), Mathf.Max(currentArea.x, areaConnectingTo.x)), Random.Range(Mathf.Min(currentArea.y, areaConnectingTo.y), Mathf.Max(currentArea.y, areaConnectingTo.y)));	
			
			// Create the curve
			List<Vector2> sourcePoints = new List<Vector2>() {currentArea, curvePoint, areaConnectingTo};
			BezierPath curve = new BezierPath();
			curve.SamplePoints(sourcePoints, 10, 1000, 0.33f);
			List<Vector2> curvePoints = curve.GetDrawingPoints2();
			
			// connect the curve points
			for (int j = 0; j < curvePoints.Count; j++)
			{
				if (j < curvePoints.Count - 1)
				{
					connectionDrawPoints.AddRange(GenerateLine(curvePoints[j], curvePoints[j + 1]));
				}
			}
		}
		
		// Add the paths to the map
		foreach (Vector2 vec in connectionDrawPoints)
		{
			if (new Rect(0, 0, playableWidth, playableHeight).Contains(vec)) // Check its within the level
				tiles[(int)vec.x, (int)vec.y] = TileTypes.Dirt;
		}
		
		#endregion
		
		#endregion
		
		#region Generate Terrain Texture
		
		// TEST TO SHOW AREAS ON TERRAIN, DOES NOT REPRESENT FINAL TEXTURE SPLATS//
		float[,,] textureAlphas = new float[terrainAlphaWidth, terrainAlphaHeight, terrain.terrainData.alphamapLayers];
		
		for (int x = 0; x < terrainAlphaWidth; x++)
		{
			for (int y = 0; y < terrainAlphaHeight; y++)
			{
				//textureAlphas[x, y, 0] = 0.1f;
				
				if (x < playableWidth && y < playableHeight)
				{
					if (tiles[x, y] == TileTypes.Grass)
						textureAlphas[x, y, 1] = 1;
					else if (tiles[x, y] == TileTypes.Dirt || tiles[x, y] == TileTypes.River)
						textureAlphas[x, y, 2] = 1;
				}
			}
		}
		
		terrain.terrainData.SetAlphamaps(0, 0, textureAlphas);
		
		#endregion
		
		// Apply terrain height changes
		terrain.terrainData.SetHeights(0, 0, terrainHeights);
		
		return true;
	}
	
	#region Ellipse Functions
	
	private TileTypes[,] EllipseMidpoint(int xCenter, int yCenter, int xRadius, int yRadius, TileTypes[,] map)
	{
		TileTypes[,] returnMap = map;
		
		float rxSq = xRadius * xRadius;
		float rySq = yRadius * yRadius;
		float x = 0;
		float y = yRadius;
		float p;
		float px = 0;
		float py = 2 * rxSq * y;
		
		returnMap = DrawEllipse(xCenter, yCenter, x, y, returnMap);
		
		// Region 1
		p = rySq - (rxSq * yRadius) + (0.25f * rxSq);
		
		while (px < py)
		{
			x++;
			
			px = px + 2 * rySq;
			
			if (p < 0)
				p = p + rySq + px;
			else
			{
				y--;
				py = py - 2 * rxSq;
				p = p + rySq + px - py;
			}
			
			returnMap = DrawEllipse(xCenter, yCenter, x, y, returnMap);
		}
		
		// Region 2
		p = rySq * (x + 0.5f) * (x + 0.5f) + rxSq * (y - 1) * (y - 1) - rxSq*rySq;
		
		while (y > 0)
		{
			y--;
			py = py - 2 * rxSq;
			
			if (p > 0)
				p = p + rxSq - py;
			else
			{
				x++;
				px = px + 2 * rySq;
				p = p + rxSq - py + px;
			}
			
			returnMap = DrawEllipse(xCenter, yCenter, x, y, returnMap);
		}
		
		return returnMap;
	}
	
	private TileTypes[,] DrawEllipse(float xCenter, float yCenter, float x, float y, TileTypes[,] _returnMap)
	{
		TileTypes[,] returnMap = _returnMap;
		
		returnMap[(int)(xCenter + x), (int)(yCenter + y)] = TileTypes.Dirt;
		returnMap[(int)(xCenter - x), (int)(yCenter + y)] = TileTypes.Dirt;
		returnMap[(int)(xCenter + x), (int)(yCenter - y)] = TileTypes.Dirt;
		returnMap[(int)(xCenter - x), (int)(yCenter - y)] = TileTypes.Dirt;
		
		return returnMap;
	}
	
	#endregion
	
	private Vector2[] GeneratePolygon(int verticeCount, Rect bounds)
	{
		// Pick random radii.
		float[] radii = new float[verticeCount];
		const float minRadius = 0.5f;
		const float maxRadius = 1.0f;
		
		for (int i = 0; i < verticeCount; i++)
		{
			radii[i] = Random.Range(minRadius, maxRadius);
		}
		
		// Pick random angle weights.
		float[] angleWeights = new float[verticeCount];
		const float minWeight = 1.0f;
		const float maxWeight = 10.0f;
		
		float totalWeight = 0;
		
		for (int i = 0; i < verticeCount; i++)
		{
			angleWeights[i] = Random.Range(minWeight, maxWeight);
			totalWeight += angleWeights[i];
		}
		
		// Convert weights into fractions of 2 * Pi radians.
		float[] angles = new float[verticeCount];
		float toRadians = 2 * Mathf.PI / totalWeight;
		
		for (int i = 0; i < verticeCount; i++)
		{
			angles[i] = angleWeights[i] * toRadians;
		}
		
		// Calculate the points locations.
		Vector2[] points = new Vector2[verticeCount];
		float rx = bounds.width / 2f;
		float ry = bounds.height / 2f;
		float cx = bounds.x + bounds.width / 2;
		float cy = bounds.y + bounds.height / 2;
		
		float theta = 0;
		
		for (int i = 0; i < verticeCount; i++)
		{
			points[i] = new Vector2(cx + (int)(rx * radii[i] * Mathf.Cos(theta)), 
									cy + (int)(ry * radii[i] * Mathf.Sin(theta)));
			theta +=  angles[i];
		}
		
		// Return the points.
		return points;
	}
	
	private IEnumerable<Vector2> GenerateLine(Vector2 a, Vector2 b)
	{
		List<Vector2> linePoints = new List<Vector2>();
		bool steep = Mathf.Abs((int)b.y - (int)a.y) > Mathf.Abs((int)b.x - (int)a.x);
		
		if (steep)
		{
			int t;
			
			t = (int)a.x; // Swap xA and yA
			a.x = (int)a.y;
			a.y = t;
			
			t = (int)b.x; // Swap xB and yB;
			b.x = (int)b.y;
			b.y = t;
		}
		
		if (a.x > b.x)
		{
			int t;
			
			t = (int)a.x; // Swap xA and xB
			a.x = (int)b.x;
			b.x = t;
			
			t = (int)a.y; // Swap a.y and b.y
			a.y = (int)b.y;
			b.y = t;
		}
		
		int dX = (int)b.x - (int)a.x; // deltaX
		int dY = Mathf.Abs((int)b.y - (int)a.y); // deltaY
		int error = dX / 2;
		int yStep = ((int)a.y < (int)b.y) ? 1 : -1;
		int y = (int)a.y;
		
		for (int x = (int)a.x; x <= (int)b.x; x++)
		{
			yield return new Vector2((steep ? y : x), (steep ? x : y));
			
			error = error - dY;
			
			if (error < 0)
			{
				y += yStep;
				error += dX;
			}
		}
		
		yield break;
	}
	
	List<Vector2> GenerateThickLine(Vector2 vec1, Vector2 vec2, int lineWidth)
    {
		List<Vector2> drawPoints = new List<Vector2>();
		
   		int x1 = (int)vec1.x;
      	int y1 = (int)vec1.y;

       	int x2 = (int)vec2.x;
       	int y2 = (int)vec2.y;

       	if(vec1.x > vec2.x)
       	{
         	x1 = (int)vec2.x;
         	y1 = (int)vec2.y;

         	x2 = (int)vec1.x;
         	y2 = (int)vec1.y;
      	}

       	int dx	    = Mathf.Abs(x2 - x1);
       	int dy 		= Mathf.Abs(y2 - y1);
       	int inc_dec = ((y2 >= y1) ? 1 : -1);

       	if(dx > dy)
      	{
         	int two_dy    =	(2 * dy);
         	int two_dy_dx =	(2 * (dy - dx));
         	int p		  =	((2 * dy) - dx);

         	int x = x1;
         	int y = y1;

         	while(x <= x2)
        	{		
				for (int i = 0; i < lineWidth + 1; i++)
				{
					drawPoints.Add(new Vector2(x, y + i));
				}

           		x++;

           		if(p < 0)
				{
              		p += two_dy;
				}
           		else
              	{
             		y += inc_dec;
             		p += two_dy_dx;
              	}
        	}
      	}

       	else
      	{
         	int two_dx = (2 * dx);
         	int two_dx_dy = (2 * (dx - dy));
         	int p = ((2 * dx) - dy);

         	int x = x1;
         	int y = y1;

         	while(y != y2)
       	 	{			
				for (int i = 0; i < lineWidth + 1; i++)
				{
					drawPoints.Add(new Vector2(x, y + i));
				}

           		y += inc_dec;

           		if(p < 0)
              		p += two_dx;
           		else
              	{
             		x++;
             		p += two_dx_dy;
              	}
        	}
      	}
		
		return drawPoints;
    }
	
	// Standard Flood Fill Algorithm
	private TileTypes[,] FloodFill(Vector2 start, TileTypes replacement, TileTypes[,] tiles, params TileTypes[] seed)
   	{
		TileTypes[,] returnTiles = tiles;
     	int w = playableWidth;
        int h = playableHeight;

        if (start.y < 0 || start.y > h - 1 || start.x < 0 || start.x > w - 1)
            return tiles;

        Stack<Vector2> stack = new Stack<Vector2>();
        stack.Push(start);
		
        while (stack.Count > 0)
        {
            Vector2 p = stack.Pop();
            int x = (int)p.x;
            int y = (int)p.y;
			
            if (y < 0 || y > h - 1 || x < 0 || x > w - 1)
                continue;
			
            TileTypes val = returnTiles[x, y];
			
            if (seed.Contains(val))
            {
				returnTiles[x, y] = replacement;
                stack.Push(new Vector2(x + 1, y));
                stack.Push(new Vector2(x - 1, y));
                stack.Push(new Vector2(x, y + 1));
                stack.Push(new Vector2(x, y - 1));
        	}
        }
		
		return returnTiles;
  	}
	
	void Reset()
	{
		float[,] defaultHeight  = new float[terrainWidth, terrainHeight];
		float[,,] defaultAlphas = new float[terrainAlphaWidth, terrainAlphaHeight, terrain.terrainData.alphamapLayers] ;
		
		for (int x = 0; x < terrainWidth; x++)
			for (int y = 0; y < terrainHeight; y++)
				defaultHeight[x, y] = 0;
		
		for (int x = 0; x < terrainAlphaWidth; x++)
		{
			for (int y = 0; y < terrainAlphaHeight; y++)
			{
				defaultAlphas[x, y, 0] = 1;
				defaultAlphas[x, y, 1] = 0;
			}
		}
		
		terrain.terrainData.SetHeights(0, 0, defaultHeight);
		terrain.terrainData.SetAlphamaps(0, 0, defaultAlphas);
	}
	
	#region Perlin Noise
	
	double GetHeight(double x, double y)
	{
		return amplitude * Total(x, y);
	}
	
	double Total(double i, double j)
	{
		// Properties of an octave
		double t = 0f;
		double a = 1; // Amplitude.
		double f = frequency; // Frequency.
		
		for (int k = 0; k < octaves; k++)
		{
			t += GetValue(j * f + randomSeed, i * f + randomSeed) * a;
			a *= persistance;
			f *= 2;
		}
		
		return t;
	}
	
	double GetValue(double x, double y)
	{
		int xInt = (int)x;
		int yInt = (int)y;
		double xFrac = x - xInt;
		double yFrac = y - yInt;
		
		// Noise Values.
		double n01 = Noise(xInt - 1, yInt - 1);
  		double n02 = Noise(xInt + 1, yInt - 1);
  		double n03 = Noise(xInt - 1, yInt + 1);
  		double n04 = Noise(xInt + 1, yInt + 1);
  		double n05 = Noise(xInt - 1, yInt);
  		double n06 = Noise(xInt + 1, yInt);
  		double n07 = Noise(xInt, yInt - 1);
  		double n08 = Noise(xInt, yInt + 1);
  		double n09 = Noise(xInt, yInt);

  		double n12 = Noise(xInt + 2, yInt - 1);
  		double n14 = Noise(xInt + 2, yInt + 1);
  		double n16 = Noise(xInt + 2, yInt);

  		double n23 = Noise(xInt - 1, yInt + 2);
  		double n24 = Noise(xInt + 1, yInt + 2);
  		double n28 = Noise(xInt, yInt + 2);

  		double n34 = Noise(xInt + 2, yInt + 2);

    	// Find corner noise values.
    	double x0y0 = 0.0625d *(n01 + n02 + n03 + n04) + 0.125d * (n05 + n06 + n07 + n08) + 0.25d *(n09);  
    	double x1y0 = 0.0625d *(n07 + n12 + n08 + n14) + 0.125d * (n09 + n16 + n02 + n04) + 0.25d *(n06);  
    	double x0y1 = 0.0625d *(n05 + n06 + n23 + n24) + 0.125d * (n03 + n04 + n09 + n28) + 0.25d *(n08);  
    	double x1y1 = 0.0625d *(n09 + n16 + n28 + n34) + 0.125d * (n08 + n14 + n06 + n24) + 0.25d *(n04);  

    	// Interpolate the values.
    	double v1    = Interpolate(x0y0, x1y0, xFrac); // Interpolate in x direction (y).
    	double v2    = Interpolate(x0y1, x1y1, xFrac); // Interpolate in x direction (y+1).
    	double final = Interpolate(v1, v2, yFrac);     // Interpolate in y direction.

    	return final;
	}
	
	double Interpolate(double x, double y, double a)
	{
		double negativeA = 1d - a;
		double negativeASqr = negativeA * negativeA;
		double fac1 = 3.0d * (negativeASqr) - 2.0d * (negativeASqr * negativeA);
		double aSqr = a * a;
		double fac2 = 3.0d * aSqr - 2.0 * (aSqr * a);
		
		return x * fac1 + y * fac2; // Add the weighted factors.
	}
	
	double Noise(int x, int y)
	{
		int n = x + y * 57;
		n = (n << 13) ^ n;
		int t = (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff;
    	return 1.0d - (double)t * 0.931322574615478515625e-9;
	}
	
	#endregion

	#endregion
}
