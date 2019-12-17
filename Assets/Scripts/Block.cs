using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Block object that represents all possible blocks.
/// It is in charge of rendering a block as weö as managing its state and appearance.
/// </summary>
public class Block
{
	enum Cubeside {BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK};
	public enum BlockType {GRASS, CACTUS, WATER, STONE, SAND, BEDROCK, NOCRACK,
							CRACK1, CRACK2, CRACK3, CRACK4, AIR, PUMPKIN, LISA, JULIA, SIGNBASE, SIGNLEFT, SIGNMIDDLE, SIGNRIGHT, HELGE};

	public BlockType blockType;
	public bool isSolid;

	public bool isWater = false;
	public Chunk owner;
	GameObject parent;
	public Vector3 position;

	public BlockType health;
	public int currentHealth;
	int[] blockHealthMax = {3, 3, 10, 4, 2, -1, 0, 0, 0, 0, 0, 0, 3, 3, 3, 2, 2, 2, 2, 5};

    // Hard-coded UVs based on blockuvs.txt
	Vector2[,] blockUVs = { 
		// TODO: change grass textures -> no dirt

		/*0 GRASS TOP*/		{new Vector2( 0.0625f, 0.375f ), new Vector2( 0.125f, 0.375f),
								new Vector2( 0.0625f, 0.4375f ),new Vector2( 0.125f, 0.4375f )},
		/*1 GRASS SIDE*/	{new Vector2( 0.0625f, 0.375f ), new Vector2( 0.125f, 0.375f),
								new Vector2( 0.0625f, 0.4375f ),new Vector2( 0.125f, 0.4375f )},
		/*2 CACTUS TOP */	{ new Vector2(0.3125f, 0.6875f), new Vector2(0.375f,0.6875f), 
								new Vector2(0.3125f,0.75f), new Vector2(0.375f,0.75f)},
		/*3 CACTUS SIDE*/	{ new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f),
                                new Vector2(0.375f,0.75f), new Vector2(0.4375f,0.75f)},
		/*4 WATER*/			{ new Vector2(0.875f,0.125f),  new Vector2(0.9375f,0.125f),
 								new Vector2(0.875f,0.1875f), new Vector2(0.9375f,0.1875f)},
		/*5 STONE*/			{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
								new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},  
		/*6 SAND*/			{ new Vector2(0.125f,0.875f),  new Vector2(0.1875f,0.875f),
 								new Vector2(0.125f,0.9375f), new Vector2(0.1875f,0.9375f)},
		/*7 BEDROCK*/		{new Vector2( 0.3125f, 0.8125f ), new Vector2( 0.375f, 0.8125f),
								new Vector2( 0.3125f, 0.875f ),new Vector2( 0.375f, 0.875f )},
		/*8 NOCRACK*/		{new Vector2( 0.6875f, 0f ), new Vector2( 0.75f, 0f),
								new Vector2( 0.6875f, 0.0625f ),new Vector2( 0.75f, 0.0625f )},
		/*9 CRACK1*/		{ new Vector2(0f,0f),  new Vector2(0.0625f,0f),
 								new Vector2(0f,0.0625f), new Vector2(0.0625f,0.0625f)},
 		/*10 CRACK2*/		{ new Vector2(0.0625f,0f),  new Vector2(0.125f,0f),
 								new Vector2(0.0625f,0.0625f), new Vector2(0.125f,0.0625f)},
 		/*11 CRACK3*/		{ new Vector2(0.125f,0f),  new Vector2(0.1875f,0f),
 								new Vector2(0.125f,0.0625f), new Vector2(0.1875f,0.0625f)},
 		/*12 CRACK4*/		{ new Vector2(0.1875f,0f),  new Vector2(0.25f,0f),
 								new Vector2(0.1875f,0.0625f), new Vector2(0.25f,0.0625f)},
		/*14 PUMPKIN SIDE FACE */ { new Vector2(0.4375f,0.5f),  new Vector2(0.5f,0.5f),
 								new Vector2(0.4375f,0.5625f), new Vector2(0.5f,0.5625f)},
		/*13 PUMPKIN SIDE */ { new Vector2(0.375f,0.5f),  new Vector2(0.4375f,0.5f),
 								new Vector2(0.375f,0.5625f), new Vector2(0.4375f,0.5625f)},
		/*15 PUMPKIN TOP */ { new Vector2(0.375f,0.5625f),  new Vector2(0.4375f,0.5625f),
 								new Vector2(0.375f,0.625f), new Vector2(0.4375f,0.625f)},
		/*16 LISA TOP */	{ new Vector2(0.7f, 0.4375f), new Vector2(0.75f,0.4375f),
                                new Vector2(0.7f,0.5f), new Vector2(0.75f,0.5f)},
		/*17 LISA SIDE */	{ new Vector2(0.625f, 0.4375f), new Vector2(0.6875f,0.4375f),
                                new Vector2(0.625f,0.5f), new Vector2(0.6875f,0.5f)},
		/*18 JULIA TOP */	{ new Vector2(0.5625f, 0.4375f), new Vector2(0.625f,0.4375f),
                                new Vector2(0.5625f,0.5f), new Vector2(0.625f,0.5f)},
		/*19 JULIA SIDE */	{ new Vector2(0.5f, 0.4375f), new Vector2(0.5625f,0.4375f),
                                new Vector2(0.5f,0.5f), new Vector2(0.5625f,0.5f)},
		/*20 SIGNBASE */	{ new Vector2(0.9375f, 0.5625f), new Vector2(1.0f,0.5625f),
                                new Vector2(0.9375f,0.625f), new Vector2(1.0f,0.625f)},
		/*21 TEXT LEFT */	{ new Vector2(0.8125f, 0.5f), new Vector2(0.875f,0.5f),
                                new Vector2(0.8125f,0.5625f), new Vector2(0.875f,0.5625f)},
		/*22 TEXT MIDDLE */	{ new Vector2(0.875f, 0.5f), new Vector2(0.9375f,0.5f),
                                new Vector2(0.875f,0.5625f), new Vector2(0.9375f,0.5625f)},
		/*23 TEXT RIGHT */	{ new Vector2(0.9375f, 0.5f), new Vector2(1.0f,0.5f),
                                new Vector2(0.9375f,0.5625f), new Vector2(1.0f,0.5625f)},
		/*24 SIGN SIDE */	{ new Vector2(0.25f, 0.9375f), new Vector2(0.3125f,0.9375f),
                                new Vector2(0.25f,1.0f), new Vector2(0.3125f,1.0f)},
						

		
		// /*DIRT*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
		// 						new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		// /*LEAVES*/			{ new Vector2(0.0625f,0.375f),  new Vector2(0.125f,0.375f),
 		// 						new Vector2(0.0625f,0.4375f), new Vector2(0.125f,0.4375f)},
 		// /*WOOD*/			{ new Vector2(0.375f,0.625f),  new Vector2(0.4375f,0.625f),
 		// 						new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f)},
 		// /*WOODBASE*/		{ new Vector2(0.375f,0.625f),  new Vector2(0.4375f,0.625f),
 		// 						new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f)},	  
 		// /*GOLD*/			{ new Vector2(0f,0.8125f),  new Vector2(0.0625f,0.8125f),
 		// 						new Vector2(0f,0.875f), new Vector2(0.0625f,0.875f)},
		// /*REDSTONE*/		{new Vector2( 0.1875f, 0.75f ), new Vector2( 0.25f, 0.75f),
		// 						new Vector2( 0.1875f, 0.8125f ),new Vector2( 0.25f, 0.8125f )},
		// /*DIAMOND*/			{new Vector2( 0.125f, 0.75f ), new Vector2( 0.1875f, 0.75f),
		// 						new Vector2( 0.125f, 0.8125f ),new Vector2( 0.1875f, 0.8125f )},
	}; 

    /// <summary>
    /// Constructs a block.
    /// </summary>
    /// <param name="b">Type of block</param>
    /// <param name="pos">Position of the block</param>
    /// <param name="p">Parent GameObject</param>
    /// <param name="o">Owner of the block (i.e. chunk)</param>
	public Block(BlockType b, Vector3 pos, GameObject p, Chunk o)
	{
		blockType = b;
		owner = o;
		parent = p;
		position = pos;
		SetType(blockType);
	}

    /// <summary>
    /// Sets the BlockType of the block. It determines if a block is solid, air or fluid.
    /// It also sets the health of the block.
    /// </summary>
    /// <param name="b">BlockType to be set</param>
	public void SetType(BlockType b)
	{	
		blockType = b;
		if(blockType == BlockType.WATER)
		{
			isWater = true;
			parent = owner.fluid.gameObject;
		}
		else
			parent = owner.chunk.gameObject;

		if(blockType == BlockType.AIR || blockType == BlockType.WATER)
			isSolid = false;
		else
			isSolid = true;
		
		if(blockType == BlockType.SAND){
			if(hasAnyWaterNeighbour()){
				blockType = BlockType.GRASS;
			}
		}
		health = BlockType.NOCRACK;
		currentHealth = blockHealthMax[(int)blockType];
	}

    /// <summary>
    /// Restores the health of the block and removes the cracks by redrawing the chunk.
    /// </summary>
	public void Reset()
	{
		health = BlockType.NOCRACK;
		currentHealth = blockHealthMax[(int)blockType];
		owner.Redraw();
	}

    /// <summary>
    /// Sets the type of the to be placed block, which was originally Air or Water, to the desired block to be placed.
    /// Rewards the chunk.
    /// </summary>
    /// <param name="b">BlockType to be set</param>
    /// <returns>Returns always true after updating the chunk.</returns>
	public bool BuildBlock(BlockType b)
	{
        // If water or sand got placed, activate the drop and flow coroutines respectively.
		//if(b == BlockType.WATER)
		//{
		//	owner.mb.StartCoroutine(owner.mb.Flow(this, 
		//								BlockType.WATER, 
		//								blockHealthMax[(int)BlockType.WATER],15));
		//}
		//else if(b == BlockType.SAND)
		//{
		//	owner.mb.StartCoroutine(owner.mb.Drop(this, 
		//								BlockType.SAND, 
		//								20));
		//}
		//else
		//{
			SetType(b);
			owner.Redraw();
		//}
		return true;
	}

    /// <summary>
    /// Reduces the blocks heatlh. Destroys the block if it does not have any health remaining.
    /// </summary>
    /// <returns>Returns true if the block was destroyed. Returns false if the block is still alive.</returns>
	public bool HitBlock()
	{
		if(currentHealth == -1) return false;
		currentHealth--;
		health++;
		Debug.Log(GetBlockType((int)position.x,(int)position.y,(int)position.z)); 
		//printNeighbours();
		if(currentHealth == (blockHealthMax[(int)blockType]-1))
		{
			owner.mb.StartCoroutine(owner.mb.HealBlock(position));
		}

		if(currentHealth <= 0)
		{
			blockType = BlockType.AIR;
			isSolid = false;
			health = BlockType.NOCRACK;
			owner.Redraw();
			//owner.UpdateChunk();
			return true;
		}

		owner.Redraw();
		return false;
	}


	private void printNeighbours(){
		Debug.Log(GetBlockType((int)position.x,(int)position.y,(int)position.z + 1)); 
		Debug.Log(GetBlockType((int)position.x,(int)position.y,(int)position.z - 1)); 
		Debug.Log(GetBlockType((int)position.x - 1,(int)position.y,(int)position.z)); 
		Debug.Log(GetBlockType((int)position.x + 1,(int)position.y,(int)position.z));
	}
    /// <summary>
    /// Assembles one side of a cube's mesh by selecting the UVs, defining the vertices and calculating the normals.
    /// </summary>
    /// <param name="side">Quad to be created for this side</param>
	private void CreateQuad(Cubeside side)
	{
		Mesh mesh = new Mesh();
	    mesh.name = "ScriptedMesh" + side.ToString(); 

		Vector3[] vertices = new Vector3[4];
		Vector3[] normals = new Vector3[4];
		Vector2[] uvs = new Vector2[4];
		List<Vector2> suvs = new List<Vector2>();
		int[] triangles = new int[6];

		// All possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		if(blockType == BlockType.GRASS && side == Cubeside.TOP)
		{
			uv00 = blockUVs[0,0];
			uv10 = blockUVs[0,1];
			uv01 = blockUVs[0,2];
			uv11 = blockUVs[0,3];
		}
		else if(blockType == BlockType.GRASS)
		{
			uv00 = blockUVs[1,0];
			uv10 = blockUVs[1,1];
			uv01 = blockUVs[1,2];
			uv11 = blockUVs[1,3];
		}
		// TODO: grass bottom = sand??
		// else if(blockType == BlockType.GRASS && side == Cubeside.BOTTOM)
		// {
		// 	uv00 = blockUVs[(int)(BlockType.DIRT+2),0];
		// 	uv10 = blockUVs[(int)(BlockType.DIRT+2),1];
		// 	uv01 = blockUVs[(int)(BlockType.DIRT+2),2];
		// 	uv11 = blockUVs[(int)(BlockType.DIRT+2),3];
		// }
		else if (blockType == BlockType.CACTUS && side == Cubeside.TOP) {
			uv00 = blockUVs[2,0];
			uv10 = blockUVs[2,1];
			uv01 = blockUVs[2,2];
			uv11 = blockUVs[2,3];
		} else if (blockType == BlockType.PUMPKIN && side == Cubeside.FRONT) {
			uv00 = blockUVs[(int)(blockType+1),0];
			uv10 = blockUVs[(int)(blockType+1),1];
			uv01 = blockUVs[(int)(blockType+1),2];
			uv11 = blockUVs[(int)(blockType+1),3];
		} else if (blockType == BlockType.PUMPKIN && side == Cubeside.TOP) {
			uv00 = blockUVs[(int)(blockType+3),0];
			uv10 = blockUVs[(int)(blockType+3),1];
			uv01 = blockUVs[(int)(blockType+3),2];
			uv11 = blockUVs[(int)(blockType+3),3];
        } else if (blockType == BlockType.LISA && side == Cubeside.TOP) {
            uv00 = blockUVs[(int)(blockType + 3), 0];
            uv10 = blockUVs[(int)(blockType + 3), 1];
            uv01 = blockUVs[(int)(blockType + 3), 2];
            uv11 = blockUVs[(int)(blockType + 3), 3];
        } else if (blockType == BlockType.LISA)
        {
            uv00 = blockUVs[(int)(blockType + 4), 0];
            uv10 = blockUVs[(int)(blockType + 4), 1];
            uv01 = blockUVs[(int)(blockType + 4), 2];
            uv11 = blockUVs[(int)(blockType + 4), 3];

        } else if (blockType == BlockType.JULIA && side == Cubeside.TOP)
        {
            uv00 = blockUVs[(int)(blockType + 4), 0];
            uv10 = blockUVs[(int)(blockType + 4), 1];
            uv01 = blockUVs[(int)(blockType + 4), 2];
            uv11 = blockUVs[(int)(blockType + 4), 3];

        } else if (blockType == BlockType.JULIA || blockType == BlockType.SIGNBASE)
        {
            uv00 = blockUVs[(int)(blockType + 5), 0];
            uv10 = blockUVs[(int)(blockType + 5), 1];
            uv01 = blockUVs[(int)(blockType + 5), 2];
            uv11 = blockUVs[(int)(blockType + 5), 3];

        } else if (blockType == BlockType.HELGE)
		{
            uv00 = blockUVs[6, 0];
            uv10 = blockUVs[6, 1];
            uv01 = blockUVs[6, 2];
            uv11 = blockUVs[6, 3];
		} else if (blockType == BlockType.SIGNLEFT)
		{	
			if (side == Cubeside.BACK){
				uv00 = blockUVs[21, 0];
				uv10 = blockUVs[21, 1];
				uv01 = blockUVs[21, 2];
				uv11 = blockUVs[21, 3];
			} else if (side == Cubeside.FRONT) {
				uv00 = blockUVs[23, 0];
				uv10 = blockUVs[23, 1];
				uv01 = blockUVs[23, 2];
				uv11 = blockUVs[23, 3];
			} else {
				uv00 = blockUVs[24, 0];
				uv10 = blockUVs[24, 1];
				uv01 = blockUVs[24, 2];
				uv11 = blockUVs[24, 3];
			}
		} else if (blockType == BlockType.SIGNRIGHT)
		{	
			if (side == Cubeside.FRONT){
				uv00 = blockUVs[21, 0];
				uv10 = blockUVs[21, 1];
				uv01 = blockUVs[21, 2];
				uv11 = blockUVs[21, 3];
			} else if (side == Cubeside.BACK) {
				uv00 = blockUVs[23, 0];
				uv10 = blockUVs[23, 1];
				uv01 = blockUVs[23, 2];
				uv11 = blockUVs[23, 3];
			} else {
				uv00 = blockUVs[24, 0];
				uv10 = blockUVs[24, 1];
				uv01 = blockUVs[24, 2];
				uv11 = blockUVs[24, 3];
			}
		} else if (blockType == BlockType.SIGNMIDDLE)
		{	
			if (side == Cubeside.BACK || side == Cubeside.FRONT){
				uv00 = blockUVs[22, 0];
				uv10 = blockUVs[22, 1];
				uv01 = blockUVs[22, 2];
				uv11 = blockUVs[22, 3];
			} else {
				uv00 = blockUVs[24, 0];
				uv10 = blockUVs[24, 1];
				uv01 = blockUVs[24, 2];
				uv11 = blockUVs[24, 3];
			}
		}
        else {
			uv00 = blockUVs[(int)(blockType+2),0];
			uv10 = blockUVs[(int)(blockType+2),1];
			uv01 = blockUVs[(int)(blockType+2),2];
			uv11 = blockUVs[(int)(blockType+2),3];
		}

		// Set cracks
		suvs.Add(blockUVs[(int)(health+2),3]);
		suvs.Add(blockUVs[(int)(health+2),2]);
		suvs.Add(blockUVs[(int)(health+2),0]);
		suvs.Add(blockUVs[(int)(health+2),1]);

		//{uv11, uv01, uv00, uv10};

		// All possible vertices 
		// Top vertices
		Vector3 p0 = new Vector3( -0.5f,  -0.5f,  0.5f );
		Vector3 p1 = new Vector3(  0.5f,  -0.5f,  0.5f );
		Vector3 p2 = new Vector3(  0.5f,  -0.5f, -0.5f );
		Vector3 p3 = new Vector3( -0.5f,  -0.5f, -0.5f );		 
		// Bottom vertices
		Vector3 p4 = new Vector3( -0.5f,   0.5f,  0.5f );
		Vector3 p5 = new Vector3(  0.5f,   0.5f,  0.5f );
		Vector3 p6 = new Vector3(  0.5f,   0.5f, -0.5f );
		Vector3 p7 = new Vector3( -0.5f,   0.5f, -0.5f );
		
		switch(side)
		{
			case Cubeside.BOTTOM:
				vertices = new Vector3[] {p0, p1, p2, p3};
				normals = new Vector3[] {Vector3.down, Vector3.down, 
											Vector3.down, Vector3.down};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] { 3, 1, 0, 3, 2, 1};
			break;
			case Cubeside.TOP:
				vertices = new Vector3[] {p7, p6, p5, p4};
				normals = new Vector3[] {Vector3.up, Vector3.up, 
											Vector3.up, Vector3.up};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};
			break;
			case Cubeside.LEFT:
				vertices = new Vector3[] {p7, p4, p0, p3};
				normals = new Vector3[] {Vector3.left, Vector3.left, 
											Vector3.left, Vector3.left};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};
			break;
			case Cubeside.RIGHT:
				vertices = new Vector3[] {p5, p6, p2, p1};
				normals = new Vector3[] {Vector3.right, Vector3.right, 
											Vector3.right, Vector3.right};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};
			break;
			case Cubeside.FRONT:
				vertices = new Vector3[] {p4, p5, p1, p0};
				normals = new Vector3[] {Vector3.forward, Vector3.forward, 
											Vector3.forward, Vector3.forward};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};
			break;
			case Cubeside.BACK:
				vertices = new Vector3[] {p6, p7, p3, p2};
				normals = new Vector3[] {Vector3.back, Vector3.back, 
											Vector3.back, Vector3.back};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};
			break;
		}

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.SetUVs(1,suvs);
		mesh.triangles = triangles;
		 
		mesh.RecalculateBounds();
		
		GameObject quad = new GameObject("Quad");
		quad.transform.position = position;
	    quad.transform.parent = this.parent.transform;

     	MeshFilter meshFilter = (MeshFilter) quad.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = mesh;
	}

    /// <summary>
    /// Subtracts or adds the world's chunk size to convert a global block position value to local.
    /// This is important to determine a neighbouring block, which is located in a neighbouring chunk!
    /// </summary>
    /// <param name="i">Block position value (e.g. x coordinate)</param>
    /// <returns>Returns the local value of the block position's coordinate</returns>
	int ConvertBlockIndexToLocal(int i)
	{
		if(i <= -1) 
			i = World.chunkSize+i; 
		else if(i >= World.chunkSize) 
			i = i-World.chunkSize;
		return i;
	}

    Vector3 ConvertPositionIntoGlobal(Vector3 pos)
    {
        Vector3 globalPos = parent.transform.position + pos;
        return globalPos;
    }

    /// <summary>
    /// Given a position of a block, it returns the type of the specified block.
    /// </summary>
    /// <param name="x">x position of the block</param>
    /// <param name="y">y position of the block</param>
    /// <param name="z">z position of the block</param>
    /// <returns>Returns the BlockType of a block that was specified by its position</returns>
    public BlockType GetBlockType(int x, int y, int z)
	{
		Block b = GetBlock(x, y, z);
		if(b == null)
			return BlockType.AIR;
		else
			return b.blockType;
	}

    /// <summary>
    /// Returns the specified block, but checks first if the the position is found in a neighbouring chunk.
    /// </summary>
    /// <param name="x">x position of the block</param>
    /// <param name="y">y position of the block</param>
    /// <param name="z">z position of the block</param>
    /// <returns>Returns the block that was specified by its position</returns>
	public Block GetBlock(int x, int y, int z)
	{
        Block[,,] chunks;

        // Test whether the adjacent block is in a neighbouring chunk or not
        // This is determined upon the indices
        // If one indice is -1 or is equal to the chunk size, the block is located in a neighbouring chunk
        if (x < 0 || x >= World.chunkSize ||
           y < 0 || y >= World.chunkSize ||
           z < 0 || z >= World.chunkSize)
        {
            return World.GetWorldBlock(ConvertPositionIntoGlobal(new Vector3(x, y, z)));
        }
        // Block in this chunk
        else
            chunks = owner.chunkData;

        return chunks[x, y, z];
    }

    /// <summary>
    /// Tests whether the specificed block is solid or not solid.
    /// </summary>
    /// <param name="x">x position of the block</param>
    /// <param name="y">y position of the block</param>
    /// <param name="z">z position of the block</param>
    /// <returns>Returns true if the specified block is solid</returns>
    public bool HasSolidNeighbour(int x, int y, int z)
	{
		try
		{
			Block b = GetBlock(x,y,z);
			if(b != null)
				return (b.isSolid || b.blockType == blockType);
		}
		catch(System.IndexOutOfRangeException){}

		return false;
	}

	public bool HasWaterNeighbour(int x, int y, int z){
		try
		{
			Block b = GetBlock(x,y,z);
			if(b != null)
				return (b.isWater);
		}
		catch(System.IndexOutOfRangeException){}

		return false;
	}

	public bool hasAnyWaterNeighbour(){
		if(HasWaterNeighbour((int)position.x,(int)position.y,(int)position.z + 1) 
			|| HasWaterNeighbour((int)position.x,(int)position.y,(int)position.z - 1) 
			|| HasWaterNeighbour((int)position.x,(int)position.y + 1,(int)position.z) 
			|| HasWaterNeighbour((int)position.x,(int)position.y - 1,(int)position.z)
			|| HasWaterNeighbour((int)position.x - 1,(int)position.y,(int)position.z) 
			|| HasWaterNeighbour((int)position.x + 1,(int)position.y,(int)position.z)){
			return true;
		} 
		return false;
	}
    /// <summary>
    /// Determines if a side of a cube is to be drawn as a mesh or not, depending on having a solid neighbour or not. If a block is of type AIR, no quads are being created.
    /// </summary>
	public void Draw()
	{
		if(blockType == BlockType.AIR) return;
		// Solid or same neighbour
		if(!HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z + 1))
			CreateQuad(Cubeside.FRONT);
		if(!HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z - 1))
			CreateQuad(Cubeside.BACK);
		if(!HasSolidNeighbour((int)position.x,(int)position.y + 1,(int)position.z))
			CreateQuad(Cubeside.TOP);
		if(!HasSolidNeighbour((int)position.x,(int)position.y - 1,(int)position.z))
			CreateQuad(Cubeside.BOTTOM);
		if(!HasSolidNeighbour((int)position.x - 1,(int)position.y,(int)position.z))
			CreateQuad(Cubeside.LEFT);
		if(!HasSolidNeighbour((int)position.x + 1,(int)position.y,(int)position.z))
			CreateQuad(Cubeside.RIGHT);
	}
}
