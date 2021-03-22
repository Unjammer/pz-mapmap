/*******************************************************************
 * Author: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapMapLib
{
	public class MMTile
	{
		public String tile;
		public Int32 offX;
		public Int32 offY;
		public Int32 x;
		public Int32 y;
		public Int32 z;

		public MMTile(String tilename){
			this.tile = tilename;
			this.offX = this.offY = this.x = this.y = this.z = 0;
		}
		public MMTile(String tilename, Int32 offx, Int32 offy, Int32 x, Int32 y, Int32 z){
			this.tile = tilename;
			this.offX = offx;
			this.offY = offy;
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	public class MMGridSquare
	{
		private List <MMTile> top, middle, bottom, floor;
		private int roomID = 0;
		private bool hasContainer = false; //for future use
		private string container;
		private Int32 x;
		private Int32 y;
		private Int32 z;
		public const Int32 FLOOR = 0;
		public const Int32 BOTTOM = 1;
		public const Int32 MIDDLE = 2;
		public const Int32 TOP = 3;

		public MMGridSquare(Int32 x, Int32 y, Int32 z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.top = new List<MMTile>();
			this.bottom = new List<MMTile>();
			this.middle = new List<MMTile>();
			this.floor = new List<MMTile>();
		}

		public void AddTile(Int32 which, string tile, Int32 offsetX, Int32 offsetY){
			switch (which){
				case TOP:
					this.top.Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z));
					break;
				case MIDDLE:
					this.middle.Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z));
					break;
				case BOTTOM:
					this.bottom.Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z));
					break;
				case FLOOR:
				default:
					this.floor.Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z));
					break;
			};
		}
		public void AddTile(string tile, Int32 offsetX, Int32 offsetY) {
			if (tile == null)
				return;
			if ((tile.Contains("wall") && !tile.Contains("graffiti")) ||
					tile.Contains("carpentry_02_80") || tile.Contains("carpentry_02_81") | tile.Contains("carpentry_02_82") | tile.Contains("carpentry_02_83") // Log walls
					){
				this.AddTile(BOTTOM, tile, offsetX, offsetY);
				return;
			}
			if (tile.Contains("floor") || tile.Contains("blends")){
				this.AddTile(FLOOR, tile, offsetX, offsetY);
				return;
			}
			this.AddTile(MIDDLE, tile, offsetX, offsetY);
		}

		public void AddTile(string tile)
		{
			//check for container here?
			this.AddTile(tile, 0, 0);
		}

		public List<MMTile> GetTiles(Int32 which) {
			switch (which){
				case TOP:
					return this.top;
				case MIDDLE:
					return this.middle;
				case BOTTOM:
					return this.bottom;
				case FLOOR:
				default:
					return this.floor;
			}
			// return null;
		}

		public void SetRoomID(int roomid)
		{
			this.roomID = roomid;
		}

		public int GetRoomID()
		{
			return this.roomID;
		}

		public bool HasContainer()
		{
			return hasContainer;
		}

		public string GetContainerType()
		{
			return this.container;
		}

		public void Reset()
		{
			this.roomID = 0;
			this.hasContainer = false;
			this.container = null;
			this.ResetTiles();
		}
		
		public void ResetTiles(){
			this.top.Clear();
			this.middle.Clear();
			this.bottom.Clear();
			this.floor.Clear();
		}
	}

	public class MMCellData
	{
		private MMGridSquare[,,] squares;

		public MMCellData()
		{
			this.squares = new MMGridSquare[8,900,900];
			for (int z = 0; z < 8; z++)
				for (int x = 0; x < 900; x++)
					for (int y = 0; y < 900; y++)
						this.squares[z, x, y] = new MMGridSquare(x, y, z);
		}

		public MMGridSquare SetSquare(int x, int y, int z)
		{
			MMGridSquare gs = this.squares[z, x, y];
			return gs;
		}

		public MMGridSquare GetSquare(int x, int y, int z)
		{
			return this.squares[z, x, y];
		}

		public void Reset()
		{
			for (int z = 0; z < 8; z++)
				for (int x = 0; x < 900; x++)
					for (int y = 0; y < 900; y++)
						this.squares[z, x, y].Reset();
		}
	}
}
