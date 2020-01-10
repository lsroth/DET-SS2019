using System.Collections;
using UnityEngine;

/// <summary>
/// MonoBehavior of a chunk' GameObject to process Coroutines.
/// </summary>
public class ChunkMB: MonoBehaviour
{
	Chunk owner;
    public GameObject mumie;
	public ChunkMB(){}

    /// <summary>
    /// Assigns the reference to the chunk who possesses this MonoBehavior.
    /// </summary>
    /// <param name="o"></param>
	public void SetOwner(Chunk o)
	{
		owner = o;
		//InvokeRepeating("SaveProgress",10,1000);
	}
    void Start(){
        mumie = World.mumie;
       // mumie = GameObject.FindGameObjectWithTag("mumie");
    }
    public void instantiateMumie (){
        for (int x = 0; x < World.chunkSize; x++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int z = 0; z < World.chunkSize; z++){
                    if(owner!= null)
                        if(owner.chunkData[x,y,z].helgeDestroyed){
                            owner.chunkData[x,y,z].helgeDestroyed = false;
                            int worldX = (int)(x + owner.chunk.transform.position.x);
                            int worldY = (int)(y + owner.chunk.transform.position.y);
                            int worldZ = (int)(z + owner.chunk.transform.position.z);
                            Vector3 pos = new Vector3(worldX,worldY,worldZ);
                            //Debug.Log("mumie at" + pos);
                            Instantiate(mumie, pos, Quaternion.identity);
                        }
                }
    }

    void Update(){
        instantiateMumie();
    }
    /// <summary>
    /// Coroutine is used to reset the block's health after some time.
    /// </summary>
    /// <param name="bpos">Position of the to be healed block.</param>
    /// <returns></returns>
	public IEnumerator HealBlock(Vector3 bpos)
	{
		yield return new WaitForSeconds(3);
		int x = (int) bpos.x;
		int y = (int) bpos.y;
		int z = (int) bpos.z;

		if(owner.chunkData[x,y,z].blockType != Block.BlockType.AIR)
			owner.chunkData[x,y,z].Reset();
	}

    /// <summary>
    /// Coroutine to allow a squence of dropping a Block downwards like sand.
    /// </summary>
    /// <param name="b">Block to be dropped</param>
    /// <param name="bt">BlockType</param>
    /// <param name="maxDrop">Maximum number of drops</param>
    /// <returns></returns>
    //public IEnumerator Drop(Block b, Block.BlockType bt, int maxDrop)
    //{
    //	Block thisBlock = b;
    //	Block prevBlock = null;
    //	for(int i = 0; i < maxDrop; i++)
    //	{
    //		Block.BlockType previousType = thisBlock.blockType;
    //		if(previousType != bt)
    //			thisBlock.SetType(bt);
    //		if(prevBlock != null)
    //			prevBlock.SetType(previousType);

    //		prevBlock = thisBlock;
    //		b.owner.Redraw();

    //		yield return new WaitForSeconds(0.2f);
    //		Vector3 pos = thisBlock.position;

    //		thisBlock = thisBlock.GetBlock((int)pos.x,(int)pos.y-1,(int)pos.z);
    //           Debug.Log(thisBlock);
    //		if(thisBlock.isSolid)
    //		{	
    //			yield break;
    //		}
    //	}
    //}

    /// <summary>
    /// Coroutine to allow a fluid Block to fall down and spreadout.
    /// </summary>
    /// <param name="b"></param>
    /// <param name="bt"></param>
    /// <param name="strength"></param>
    /// <param name="maxSize"></param>
    /// <returns></returns>
    public IEnumerator Flow(Block b, Block.BlockType bt, int strength, int maxSize)
    {
       // Reduce the strenth of the fluid block with each new block created(avoid infinite and exponentially growing number of fluid blocks)

       if (maxSize <= 0) yield break;
       if (b == null) yield break;
       if (strength <= 0) yield break;
       if (b.blockType != Block.BlockType.AIR) yield break;
       b.SetType(bt);
       b.currentHealth = strength;
       b.owner.Redraw();
       yield return new WaitForSeconds(1);

       int x = (int)b.position.x;
       int y = (int)b.position.y;
       int z = (int)b.position.z;

      // Flow down if air block is beneath
      Block below = b.GetBlock(x, y - 1, z);
       if (below != null && below.blockType == Block.BlockType.AIR)
       {
           StartCoroutine(Flow(b.GetBlock(x, y - 1, z), bt, strength, --maxSize));
           yield break;
       }
       else // Flow outward
       {
           --strength;
           --maxSize;
           //Flow left

           World.queue.Run(Flow(b.GetBlock(x - 1, y, z), bt, strength, maxSize));
           yield return new WaitForSeconds(1);

           //Flow right

           World.queue.Run(Flow(b.GetBlock(x + 1, y, z), bt, strength, maxSize));
           yield return new WaitForSeconds(1);

           //Flow forward

           World.queue.Run(Flow(b.GetBlock(x, y, z + 1), bt, strength, maxSize));
           yield return new WaitForSeconds(1);

           //Flow back

           World.queue.Run(Flow(b.GetBlock(x, y, z - 1), bt, strength, maxSize));
           yield return new WaitForSeconds(1);
       }
    }

    /// <summary>
    /// Saves the underlying chunk.
    /// </summary>
	private void SaveProgress()
	{
		if(owner.changed)
		{
			owner.Save();
			owner.changed = false;
		}
	}
}
