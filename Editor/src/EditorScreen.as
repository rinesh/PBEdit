package
{
	import com.pblabs.box2D.Box2DManagerComponent;
	import com.pblabs.box2D.Box2DSpatialComponent;
	import com.pblabs.engine.entity.EntityComponent;
	import com.pblabs.engine.entity.IEntity;
	import com.pblabs.engine.entity.PropertyReference;
    import com.pblabs.engine.PBE;
    import com.pblabs.engine.core.*;
	import com.pblabs.engine.PBUtil;
	import com.pblabs.engine.serialization.Serializer;
	import com.pblabs.rendering2D.DisplayObjectRenderer;
	import com.pblabs.rendering2D.IMobileSpatialObject2D;
	import com.pblabs.rendering2D.ISpatialManager2D;
	import com.pblabs.rendering2D.ISpatialObject2D;
	import com.pblabs.rendering2D.SimpleShapeRenderer;
	import com.pblabs.rendering2D.SimpleSpatialComponent;
	import com.pblabs.rendering2D.ui.PBButton;
    import com.pblabs.rendering2D.ui.PBLabel;
    import com.pblabs.rendering2D.ui.SceneView;
    import com.pblabs.screens.*;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;
    import flash.geom.*;
    import flash.text.*;
    
    /**
     * The main screen that implements core game logic and UI.
     * 
     */
    public class EditorScreen extends BaseScreen
    {
        public var sceneView:SceneView = new SceneView();
        public var lblTime:PBLabel = new PBLabel();
        public var lblScore:PBLabel = new PBLabel();
		private var spatialComponent:EntityComponent; 
		private var selectedEntity:IEntity = null; 
		private var m_move:Boolean = false;
		private var m_scaleX:Boolean=false;
		private var m_rotate:Boolean = false;
		
		private var Gizmo:IEntity = null;
		private var GizmoSpatial:Box2DSpatialComponent;
		private var MoveRenderer:SimpleShapeRenderer;
		private var ScaleXRenderer:SimpleShapeRenderer;
		private var RotateRenderer:SimpleShapeRenderer;
		private var rendererComponent:DisplayObjectRenderer;
		
		public var playButton:PBButton = new PBButton();
		
        public function EditorScreen()
        {
            // Set up the scene view to be full screen.
            sceneView.name = "MainView";
            sceneView.width = 640;
            sceneView.height = 640;
            addChild(sceneView);
            
            // Label to display the time remaining.
            //addChild(lblTime);
            lblTime.extents = new Rectangle(0, 0, 150, 30);
            lblTime.fontColor = 0x000000;
            lblTime.fontSize = 24;
			lblTime.caption = "Box";
            lblTime.refresh();
			
			
			//addChild(playButton);
            playButton.label = "PLAY/Pause";
            playButton.extents = new Rectangle(300, 10, 260, 40);
            playButton.fontSize = 32;
            playButton.refresh();
			playButton.addEventListener(MouseEvent.CLICK, function(e:*):void
            {
				if (PBE.processManager.timeScale == 0)
				{
					PBE.processManager.timeScale = 1;
				}
				else
				{
					PBE.processManager.timeScale = 0;
				}
            });
			
			if (stage) init();
			else addEventListener(Event.ADDED_TO_STAGE, init);
	    }
		
        private function init(e:Event = null):void 
		{
			removeEventListener(Event.ADDED_TO_STAGE, init);
			 //PBE.scene.sceneView
			stage.addEventListener(MouseEvent.MOUSE_DOWN, mouseDown);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, mouseMove);
			stage.addEventListener(MouseEvent.MOUSE_UP, function(e:*):void
            {
				m_move = false;
				m_scaleX = false;
				m_rotate = false;
				
				if( (m_move ||  m_scaleX || m_rotate) && ExternalInterface.available)
				{
					var entityXML:XML = <thing />; 
					selectedEntity.serialize(entityXML);
					ExternalInterface.call("SelectedEntityChanged", entityXML.toXMLString());
				}
			});
			
			
		}
		
		public function selectEntity(entityName:String):IEntity
        {
			var entity:IEntity = PBE.lookupEntity(entityName) as IEntity;
			if (!entity)
			   return null;
			Main.m_currentEntity =  entity;
			if (ExternalInterface.available)
			{
				var entityXML:XML = <thing />;
				entity.serialize(entityXML);
				ExternalInterface.call("SelectedEntityChanged", entityXML.toXMLString());
				// Note what entity we're deserializing to the Serializer.
				Main.m_currentEntity = entity;
			}
			rendererComponent = PBE.lookupComponentByType(entityName, IMobileSpatialObject2D) as DisplayObjectRenderer;
			spatialComponent  = PBE.lookupComponentByType(entityName, IMobileSpatialObject2D) as EntityComponent;
			if (!spatialComponent || !rendererComponent)
				return null;
			(spatialComponent as IMobileSpatialObject2D).position = rendererComponent.position;
			GizmoSpatial.position = (spatialComponent as IMobileSpatialObject2D).position;
			PBE.log(this, "Entity name: " + spatialComponent.owner.name);
			return entity;
		}
		
		public function mouseDown(e:MouseEvent):void
        {
			var i:int;
			
			var spatialManager:ISpatialManager2D = PBE.spatialManager
			var worldPoint:Point = PBE.scene.transformScreenToWorld(new Point(e.stageX, e.stageY));
			
			var results:Array= new Array();
			if (PBE.scene.getRenderersUnderPoint(new Point(e.stageX, e.stageY), results))
			{
				for (i = 0; i < results.length; i++ )
				{
					//spatialComponent  = results[0] as Box2DSpatialComponent;
					rendererComponent = results[i];
					if (!rendererComponent)
						return;
					
					if ( MoveRenderer != rendererComponent &&  ScaleXRenderer != rendererComponent &&  RotateRenderer != rendererComponent)
					{
						spatialComponent  = PBE.lookupComponentByType(rendererComponent.owner.name, IMobileSpatialObject2D) as EntityComponent;
						if (!spatialComponent)
							return;
						(spatialComponent as IMobileSpatialObject2D).position = rendererComponent.position;
						GizmoSpatial.position = (spatialComponent as IMobileSpatialObject2D).position;
						PBE.log(this, "Entity name: " + spatialComponent.owner.name);
						break;
					}
				}
			}
			
			//PBE.scene.getRenderersUnderPoint(new Point(e.stageX, e.stageY), results);
			for (i = 0; i < results.length; i++ )
			{
				if ( MoveRenderer == (results[i] as SimpleShapeRenderer))
				{
					m_move = true;
				}
				if ( ScaleXRenderer == (results[i] as SimpleShapeRenderer))
				{
					m_scaleX = true;
				}
				if ( RotateRenderer == (results[i] as SimpleShapeRenderer))
				{
					m_rotate = true;
				}
			}
			
			if (spatialComponent.owner as IEntity != selectedEntity && ExternalInterface.available)
			{
				selectedEntity = spatialComponent.owner as IEntity;
				var entityXML:XML= <thing />; 
				selectedEntity.serialize(entityXML);
				ExternalInterface.call("SelectedEntityChanged", entityXML.toXMLString());
				// Note what entity we're deserializing to the Serializer.
				Main.m_currentEntity = selectedEntity;
				PBE.log(this, "selectedEntity: "+selectedEntity.name) ;
			}
		}
		
		public function mouseMove(e:MouseEvent):void
        {
			if (m_move)
			{
				var worldPoint:Point = PBE.scene.transformScreenToWorld(new Point(e.stageX, e.stageY));
				GizmoSpatial.position = worldPoint;
				(spatialComponent as IMobileSpatialObject2D).position = rendererComponent.position = GizmoSpatial.position ;
			}
			if (m_scaleX )
			{
				worldPoint = PBE.scene.transformScreenToWorld(new Point(e.stageX, e.stageY));
				if (!worldPoint.equals(ScaleXRenderer.position) )
				{
					var size:Point = (spatialComponent as IMobileSpatialObject2D).size;
					var scale:Point = worldPoint.subtract(ScaleXRenderer.position);
					PBE.log(this, "Scale: " + scale.toString());
					size.x += scale.x / 10; size.y += -scale.y / 10;//scale.x /= 10;scale.y /= 10;
					PBE.log(this, "Size: " + size.toString());
					if ((spatialComponent as IMobileSpatialObject2D).size.x < 10)
					{
					  (spatialComponent as IMobileSpatialObject2D).size.x = 10;
					}
					if ((spatialComponent as IMobileSpatialObject2D).size.y < 10)
					{
					  (spatialComponent as IMobileSpatialObject2D).size.y = 10;
					}
					
					(spatialComponent as IMobileSpatialObject2D).size = rendererComponent.size =  size;
				}
			}
			if (m_rotate )
			{
				worldPoint = PBE.scene.transformScreenToWorld(new Point(e.stageX, e.stageY));
				var vec:Point= new Point();
				vec.x = worldPoint.x - (spatialComponent as IMobileSpatialObject2D).position.x;
				vec.y = worldPoint.y - (spatialComponent as IMobileSpatialObject2D).position.y;
				vec.normalize(1);
				PBE.log(this, "Vec: "+vec.x+","+vec.y);
				var angle:Number = Math.atan(vec.y/vec.x);
				(spatialComponent as IMobileSpatialObject2D).rotation = rendererComponent.rotation = PBUtil.getDegreesFromRadians(angle);
				//PBE.log(this, "Angle: "+angle.toString());
			}
			
			//if( (m_move ||  m_scaleX || m_rotate) && ExternalInterface.available)
			//{
				//var entityXML:XML = <thing />; 
				//selectedEntity.serialize(entityXML);
				//ExternalInterface.call("SelectedEntityChanged", entityXML.toXMLString());
			//}
		}
		
        public override function onShow() : void
        {
			if (!Gizmo)
			  createGizmo();
        }
        
        /**
         * Called every frame; used to update time remaining and score. Only display
         * aspects of the game are updated here. You will notice that currentTime
         * is updated; that is so it is always super-smooth, but the gameplay
         * logic happens in onTick.
         */
        public override function onFrame(delta:Number) : void
        {
			
        }
        
        /**
         * Gameplay logic happens here; in this game, the only thing is to check
         * if the user is out of time.
         */
        public override function onTick(delta:Number) : void
        {

        }
		
		private function createGizmo():void
		{
			Gizmo= PBE.allocateEntity();
			
			GizmoSpatial = new Box2DSpatialComponent();
         
			GizmoSpatial.position = new Point(0, 0);
			GizmoSpatial.spatialManager = PBE.spatialManager as Box2DManagerComponent;
			GizmoSpatial.canMove = false; GizmoSpatial.canRotate = false;
			GizmoSpatial.size = new Point(10, 10);
			Gizmo.addComponent(GizmoSpatial, "Spatial");

			MoveRenderer = createRenderer();
			MoveRenderer.fillColor = 0x00000FF;
			Gizmo.addComponent(MoveRenderer, "MoveComponent" );
			ScaleXRenderer = createRenderer();
			ScaleXRenderer.positionOffset = new Point(30, 0);
			Gizmo.addComponent(ScaleXRenderer, "ScaleXComponent" );
			RotateRenderer = createRenderer();
			Gizmo.addComponent(RotateRenderer, "RotateComponent" );
			RotateRenderer.fillColor = 0x00FF000;
			RotateRenderer.positionOffset = new Point(-30, 0);
			
			Gizmo.initialize("Gizmo");
			
		}
		
		private function createRenderer():SimpleShapeRenderer
		{
			var renderer:SimpleShapeRenderer = new SimpleShapeRenderer();
			renderer.sizeProperty = new PropertyReference("@Spatial.size");
			renderer.positionProperty = new PropertyReference("@Spatial.position");
			renderer.rotationProperty = new PropertyReference("@Spatial.rotation");
			renderer.isSquare = true; renderer.isCircle = false;
			renderer.size = new Point(10, 10);
			renderer.lineColor = 0x000000;
			renderer.fillColor = 0xFF00000;
			renderer.scene = PBE.scene;
			return renderer;
		}

    }
}

