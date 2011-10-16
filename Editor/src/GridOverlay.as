    package
    {
       import com.pblabs.engine.debug.*;

       import flash.display.Sprite;
       
       public class GridOverlay extends Sprite
       {
          /*********************************************************************
           * CONSTANTS
           *********************************************************************/
          
          /*********************************************************************
           * PROPERTIES
           *********************************************************************/
           protected var _gridHeight:Number;
           protected var _gridWidth:Number;
           protected var _cellSize:Number;
          /*********************************************************************
           * METHODS
           *********************************************************************/
          
          public function GridOverlay(Width:Number, Height:Number, CellSize:Number)
          {
             super();
             if(Width > 0 )
             {
                _gridWidth = Width;
                
             }
             else
             {
                Logger.error(this, "GridScreen", "Width must be greater than 0");
                throw "Width must be greater than 0";
             }
             
             if(Height > 0 )
             {
                _gridHeight = Height;
                
             }
             else
             {
                Logger.error(this, "GridScreen", "Height must be greater than 0");
                throw "Height must be greater than 0";
             }
             
             if(CellSize > 0 )
             {
                _cellSize = CellSize;
                
             }
             else
             {
                Logger.error(this, "GridScreen", "CellSize must be greater than 0");
                throw "CellSize must be greater than 0";
             }

             CreateGrid();
          }
          
          public  function set GridWidth(value:Number):void
          {
             if(value > 0)
             {
                _gridWidth = value;
             }
          }
          
          public  function set GridHeight(value:Number):void
          {
             if(value > 0)
             {
                _gridHeight = value;
             }
          }

          public  function set CellSize(value:Number):void
          {
             if(value > 0)
             {
                _cellSize = value;
             }
          }
          
          protected function CreateGrid():void
          {
             var step:Number = (_gridWidth / _cellSize);
             
             graphics.lineStyle(1, 0x000000, 1);
             for(var i:Number = 0; i< step; i++)
             {
                graphics.moveTo(i * _cellSize, 0);
                graphics.lineTo(i * _cellSize, _gridHeight);            
             }
             step = (_gridHeight / _cellSize);
             for(i = 0; i< step; i++)
             {
                graphics.moveTo(0, i * _cellSize);
                graphics.lineTo(_gridWidth, i * _cellSize);            
             }
             
          }
       }
    }
