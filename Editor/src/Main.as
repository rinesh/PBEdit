package 
{
	import Box2D.Dynamics.b2Body;
	import com.pblabs.animation.Animator;
	import com.pblabs.animation.AnimatorComponent;
	import com.pblabs.animation.AnimatorType;
	import com.pblabs.engine.core.IPBObject;
	import com.pblabs.engine.core.ObjectType;
	import com.pblabs.engine.core.SchemaGenerator;
	import com.pblabs.engine.debug.Logger;
	import com.pblabs.engine.entity.EntityComponent;
	import com.pblabs.engine.entity.IEntityComponent;
	import com.pblabs.engine.serialization.Serializer;
	import com.pblabs.engine.serialization.TypeUtility;
	import com.pblabs.rendering2D.SpriteRenderer;
	import com.pblabs.rendering2D.spritesheet.SpriteContainerComponent;
	import com.pblabs.rendering2D.SpriteSheetRenderer;
	import flash.external.ExternalInterface;
	import flash.utils.describeType;
	import flash.utils.Dictionary;
	import flash.utils.getQualifiedClassName;
	import GridOverlay;
	import com.pblabs.animation.PointAnimator;
	import com.pblabs.box2D.Box2DDebugComponent;
	import com.pblabs.box2D.Box2DManagerComponent;
	import com.pblabs.box2D.Box2DSpatialComponent;
	import com.pblabs.box2D.CircleCollisionShape;
	import com.pblabs.box2D.CollisionShape;
	import com.pblabs.box2D.PolygonCollisionShape;
	import com.pblabs.engine.core.InputKey;
	import com.pblabs.engine.core.NameManager;
	import com.pblabs.engine.entity.allocateEntity;
	import com.pblabs.engine.entity.IEntity;
	import com.pblabs.engine.entity.PropertyReference;
	import com.pblabs.engine.PBUtil;
	import com.pblabs.rendering2D.BitmapDataScene;
	import com.pblabs.rendering2D.DisplayObjectScene;
	import com.pblabs.rendering2D.SimpleShapeRenderer;
	import com.pblabs.rendering2D.SimpleSpatialComponent;
	import com.pblabs.rendering2D.ui.PBLabel;
	import com.pblabs.rendering2D.ui.SceneView;
	import flash.display.Sprite;
	import flash.events.Event;
	import com.pblabs.engine.PBE;
	import flash.geom.Point;
	import flash.net.LocalConnection;
	import flash.utils.getTimer;
	import flash.utils.setInterval;

	/**
	 * ...
	 * @author Rinesh
	 */
	public class Main extends Sprite 
	{
		//private var score:PBLabel;
		public static var StartTimer:Number = 0;
        public static var CurrentTime:Number = 60.0;
		public static var LevelDuration:Number = 25000;
		public static var BallsRemaining:int = 2;
		public var conn:LocalConnection = null;
		public var schema_XML:XML = null;
		public var schemaDone:Boolean = false;
		public var editorScreen:EditorScreen = null;
		public static var m_currentEntity:IEntity = null;
		
		public function Main():void 
		{
			if (stage) init();
			else addEventListener(Event.ADDED_TO_STAGE, init);
		}
		
		public function OnSchemaReceived(type:String,data:String):void 
		{
			
			switch( type )
			{
				case "START":
				break;
		 
				case "END":
					if (ExternalInterface.available)
					{
						ExternalInterface.call("SchemaRecieved", schema_XML.toXMLString());
					}
				break;
		 
				case "ERROR":
				break;
		 
				case "TYPE":
				{
					var myXML:XML = new XML(data);
					myXML.ignoreWhite = true;
					
					schema_XML.appendChild(myXML);
					
					var xmlList:XMLList = myXML.child("factory");
		 
					//Check accessors:
					for each (var acc:XML in xmlList.accessor)
					{
						//DO SOMETHING.
					}
		 
					//Check public variables:
					for each (var pVar:XML in xmlList.variable)
					{
						//DO SOMETHING.
					}
				}
				break;
			}	
		}
		
		public function generateSchema(msg:String):String 
		{
			SchemaGenerator.instance.generateSchema();
			return msg;
		}
		
		public function playPause(msg:String):String 
		{
			if (PBE.processManager.timeScale == 0)
			{
				PBE.processManager.timeScale = 1;
			}
			else
			{
				PBE.processManager.timeScale = 0;
			}
			return msg;
		}
		
		public function currentEntity(entityName:String):String 
		{
			m_currentEntity = PBE.lookupEntity(entityName);
			if (m_currentEntity)
			{
				//var entityXML:XML= <thing />;;// = <entity name={selectedEntity.name} />;
				//if(selectedEntity.alias!=null)
                //  entityXML = <entity name={selectedEntity.name} alias={selectedEntity.alias} />;   
				//m_currentEntity.serialize(entityXML);
				//ExternalInterface.call("SelectedEntityChanged", entityXML.toXMLString());
			}	
			var entity:IEntity = editorScreen.selectEntity(entityName);
			//m_currentEntity = entity;
			if (!entity)
			return "false";
			return "true";
		}
		
		public function newEntity(entityName:String):String 
		{
			var entity:IEntity = PBE.allocateEntity();
			entity.initialize(entityName);
			m_currentEntity = entity;
			return "true";
		}
		
		public function newComponent(componentString:String):String 
		{
			deserializeComponents(componentString);
			var entityXML:XML= <thing />;
			m_currentEntity.serialize(entityXML);
			//PBE.log(this,"m_currentEntity: "+entityXML.toString());
			return entityXML.toXMLString();

			return "false";
		}
		
		public function getAllEntities():String 
		{
			var xml:XML = <things />;         
         
			 for each(var object:IPBObject in PBE.nameManager.objectList)
			 {
				if(object is IEntity)
				{         
				   var entity:IEntity = object as IEntity;
				   var entityXML:XML;// = <entity name={entity.name} />;
				   if(entity.alias!=null)
					  entityXML = <entity name={entity.name} alias={entity.alias} />;   
					else
					  entityXML = <entity name={entity.name} />;
				   xml.appendChild(entityXML);
				}
			 }   
			//PBE.log(this, "serializePBE: "+xml.toXMLString());
			return xml;      
		}
		
		public function deserializeComponents(componentString:String):String 
		{
			//componentString = componentString.replace("\n      ", "");
			//PBE.log(this,"componentString: "+componentString);
			var xml:XML = new XML(componentString);
			Serializer.instance.setCurrentEntity(m_currentEntity);
			// Process each component tag in the xml.
            for each (var componentXML:XML in xml.*)
            {
                // Error if it's an unexpected tag.
                if(componentXML.name().toString().toLowerCase() != "component")
                {
                    Logger.error(this, "deserialize", "Found unexpected tag '" + componentXML.name().toString() + "', only <component/> is valid, ignoring tag. Error in entity '" + name + "'.");
                    continue;
                }
                
                var componentName:String = componentXML.attribute("name");
                var componentClassName:String = componentXML.attribute("type");
                var component:IEntityComponent = null;
                //PBE.log(this, "componentName: " + componentName +" componentClassName: " + componentClassName);
                if (componentClassName.length > 0)
                {
                    // If it specifies a type, instantiate a component and add it.
                    component = TypeUtility.instantiate(componentClassName) as IEntityComponent;
                    if (!component)
                    {
                        Logger.error(this, "deserialize", "Unable to instantiate component " + componentName + " of type " + componentClassName + " on entity '" + name + "'.");
                        continue;
                    }
                    
                    if (!m_currentEntity.addComponent(component, componentName))
                        continue;
                }
                else
                {
                    // Otherwise just get the existing one of that name.
                    component = m_currentEntity.lookupComponentByName(componentName);
                    if (!component)
                    {
                        Logger.error(this, "deserialize", "No type specified for the component " + componentName + " and the component doesn't exist on a parent template for entity '" + name + "'.");
                        continue;
                    }
                }

                // Deserialize the XML into the component.
                Serializer.instance.deserialize(component, componentXML);
            }
			return "true";
		}
		
		private function init(e:Event = null):void 
		{
			removeEventListener(Event.ADDED_TO_STAGE, init);	
			
			conn = new LocalConnection();
				conn.client = this;
				try {
					conn.connect("_SchemaConnection");
				} catch (error:ArgumentError) {
					PBE.log(this,"Can't connect.");
				}
			

			
			if (ExternalInterface.available)
			{
				ExternalInterface.addCallback("generateSchema", generateSchema);
				ExternalInterface.addCallback("getAllEntities", getAllEntities);
				ExternalInterface.addCallback("playPause", playPause);
				ExternalInterface.addCallback("newEntity", newEntity);
				ExternalInterface.addCallback("newComponent", newComponent);
				ExternalInterface.addCallback("currentEntity", currentEntity);
			}
			
			
			
			// entry point
			PBE.startup(this);
			
			schema_XML = new XML("<thing>ertet</thing>");			
			PBE.registerType(SimpleShapeRenderer);
			PBE.registerType(Box2DSpatialComponent);
			PBE.registerType(Box2DDebugComponent);
			PBE.registerType(Box2DManagerComponent);
			PBE.registerType(CollisionShape);
			PBE.registerType(CircleCollisionShape);
			PBE.registerType(PolygonCollisionShape);
			PBE.registerType(SpriteRenderer);
			PBE.registerType(SpriteSheetRenderer);
			PBE.registerType(SpriteContainerComponent);
			PBE.registerType(ObjectType);
			PBE.registerType(AnimatorComponent);
			PBE.registerType(Animator);
			PBE.registerType(b2Body);
			
			//var description:XML = describeType(SimpleShapeRenderer);
			//PBE.log(this, "SimpleShapeRenderer: " + description.toXMLString());	
			
			var scene:IEntity = PBE.initializeScene(new SceneView(), "SceneDB", DisplayObjectScene, Box2DManagerComponent);
			//PBE.scene.zoom = 0.8;
			PBE.processManager.timeScale = 0;
			
			//var debug:Box2DDebugComponent = new Box2DDebugComponent();
			//debug.spatialManager = PBE.spatialManager as Box2DManagerComponent;
			//scene.addComponent(debug, "Debug");
			
			var spatialManager:Box2DManagerComponent = PBE.spatialManager as Box2DManagerComponent;
			spatialManager.gravity = new Point(0, 10);
			//spatialManager.world.SetContactListener(new ContactListenerPBE());
			PBE.defineEntityByFunction("Circle", createObject);
			
			var circle:IEntity;
			for (var i:int; i < BallsRemaining; i++)
			{
				circle = PBE.makeEntity("Circle", { "@Box2DSpatial.position" :new Point(100 * Math.random(), 200 * Math.random()) });
				circle.initialize("Circle" + i.toString());
			}

			PBE.defineEntityByFunction("Wall", createWall);

			var wall:IEntity;
			wall = PBE.makeEntity("Wall", { "@Box2DSpatial.position" :new Point(263, 210) });
			wall.initialize("Wall11");
			
			//var xml:XML = <things/>;
			//wall.serialize(xml);
			//PBE.log(this, "scene: " + xml.toXMLString());
			//
			//wall = PBE.makeEntity("Wall", { "@Box2DSpatial.position" :new Point(-263, 210) });
			//wall.initialize("Wall12");
			//
			//wall = PBE.makeEntity("Wall", { "@Box2DSpatial.position" :new Point(0, -180) });
			//wall.initialize("Wall2");
			//
			//wall = PBE.makeEntity("Wall", { "@Box2DSpatial.position" :new Point(-200, 0), "@Box2DSpatial.rotation" : 90  });
			//wall.initialize("Wall3");
			//
			//wall = PBE.makeEntity("Wall", { "@Box2DSpatial.position" :new Point(200, 0), "@Box2DSpatial.rotation" :90  });
			//wall.initialize("Wall4");
			//
			//wall = PBE.makeEntity("Wall", { "@Box2DSpatial.position" :new Point(0, 250), "@Renderer.fillColor" :0xFF00000 });
			//wall.initialize("Ground");
			//
			//var comp:Box2DSpatialComponent = wall.lookupComponentByName("Box2DSpatial") as Box2DSpatialComponent;
			//comp.collisionShapes[0].isTrigger = true;
			
			// Set up the game timer.
            //StartTimer = PBE.processManager.virtualTime;
            //setInterval(_UpdateTimer, 10);
			
			// Set up our screens.
            //PBE.screenManager.registerScreen("splash", new SplashScreen("../assets/Images/intro.png", "game"));
            //PBE.screenManager.registerScreen("game", new GameScreen());
            //PBE.screenManager.registerScreen("gameOver", new GameOverScreen());
			editorScreen = new EditorScreen();
			PBE.screenManager.registerScreen("Editor", editorScreen);
            PBE.screenManager.goto("Editor");
			//PBE.screenManager.push("game");
			
			//var grid:GridOverlay = new GridOverlay(stage.width, stage.height, 30);
			//PBE.mainStage.addChild(grid);
			
			//serializePBE();
		}
		
		public static function resetTimerAndScore():void
        {
            StartTimer = PBE.processManager.virtualTime;
            CurrentTime = 0.0;            
        }
	
		private function _UpdateTimer():void
        {   
            stage.focus = stage;
        }
		 
		private function createObject():IEntity
		{
			var circle:IEntity = PBE.allocateEntity();
			
			var spatial:Box2DSpatialComponent = new Box2DSpatialComponent();
         
			spatial.position = new Point(0, 0);
			spatial.spatialManager = PBE.spatialManager as Box2DManagerComponent;
			spatial.canSleep = false;
			spatial.collisionType = new ObjectType("Circle");
			spatial.collidesWithTypes = new ObjectType("Circle","Wall");
			//spatial.linearVelocity = new Point(500, 500);
			var collisionShape:CircleCollisionShape = new  CircleCollisionShape();
			//collisionShape.radius = 2;// / (PBE.spatialManager as Box2DManagerComponent).scale;
			collisionShape.restitution = 0.9;
			collisionShape.friction = 0;
			collisionShape.createShape(spatial);
			spatial.collisionShapes = new Array();
			spatial.collisionShapes.push(collisionShape);
			circle.addComponent(spatial, "Box2DSpatial");
			
			//var debug:Box2DDebugComponent = new Box2DDebugComponent();
			//debug.spatialManager = PBE.spatialManager as Box2DManagerComponent;
			//circle.addComponent(debug, "Debug");
			
			var renderer:SimpleShapeRenderer = new SimpleShapeRenderer()
			renderer.positionProperty = new PropertyReference("@Box2DSpatial.position");
			renderer.rotationProperty = new PropertyReference("@Box2DSpatial.rotation");
			renderer.sizeProperty = new PropertyReference("@Box2DSpatial.size");
			renderer.radius = 10;
			renderer.lineColor = 0x000000;
			renderer.fillColor = 0x0000FF0;
			renderer.scene = PBE.scene;
			
			circle.addComponent(renderer, "Renderer" );
			
			 // Animation
			  var spriteAnimator:AnimatorComponent = new AnimatorComponent();
			  spriteAnimator.animations = new Dictionary();
			 
			  var DownAnimation:Animator = new Animator();
			  DownAnimation.animationType = AnimatorType.LOOP_ANIMATION;
			  DownAnimation.duration = 1; // animation speed
			  DownAnimation.repeatCount = -1;
			  DownAnimation.startValue = 0;
			  DownAnimation.targetValue = 2;
			  
			  var upAnimation:Animator = new Animator();
			  upAnimation.animationType = AnimatorType.LOOP_ANIMATION;
			  upAnimation.duration = 1; // animation speed
			  upAnimation.repeatCount = -1;
			  upAnimation.startValue = 0;
			  upAnimation.targetValue = 2;
			 
			  spriteAnimator.animations['down'] = DownAnimation;
			  spriteAnimator.animations['up'] = upAnimation;
			  spriteAnimator.reference = new PropertyReference("@Renderer.spriteIndex");
			  circle.addComponent( spriteAnimator,"Animator");
			//var anim:AnimatorComponent = new AnimatorComponent();
			
			//circle.addComponent(anim, "anim" );
			return circle;
			
		}
		
		private function createWall():IEntity
		{
			var wall:IEntity = PBE.allocateEntity();
			
			var spatial:Box2DSpatialComponent = new Box2DSpatialComponent();
         
			spatial.position = new Point(0, 0);
			spatial.spatialManager = PBE.spatialManager as Box2DManagerComponent;
			spatial.canMove = false; spatial.canRotate = false;
			spatial.collisionType = new ObjectType("Wall");
			spatial.collidesWithTypes = new ObjectType("Circle");
			spatial.size = new Point(500, 50);
			var collisionShape:PolygonCollisionShape = new  PolygonCollisionShape();
			collisionShape.vertices = new Array();
			collisionShape.vertices.push(new Point(-1, -1));
			collisionShape.vertices.push(new Point(1, -1));
			collisionShape.vertices.push(new Point(1, 1));
			collisionShape.vertices.push(new Point(-1, 1));
			collisionShape.createShape(spatial);
			spatial.collisionShapes = new Array();
			spatial.collisionShapes.push(collisionShape);
			wall.addComponent(spatial, "Box2DSpatial");
			
			var renderer:SimpleShapeRenderer = new SimpleShapeRenderer();
			renderer.sizeProperty = new PropertyReference("@Box2DSpatial.size");
			renderer.positionProperty = new PropertyReference("@Box2DSpatial.position");
			renderer.rotationProperty = new PropertyReference("@Box2DSpatial.rotation");
			renderer.isSquare = true; renderer.isCircle = false;
			renderer.lineColor = 0x000000;
			renderer.fillColor = 0xFF00FF0;
			renderer.scene = PBE.scene;
			
			wall.addComponent(renderer, "Renderer" );
			
			return wall;
			
		}
		
	   public function serializePBE():XML
       {
         var xml:XML = <things />;         
         
         for each(var object:IPBObject in PBE.nameManager.objectList)
         {
            if(object is IEntity)
            {         
               var entity:IEntity = object as IEntity;
               var entityXML:XML = <entity name={entity.name} />;
               if(entity.alias!=null)
                  entityXML = <entity name={entity.name} alias={entity.alias} />;   
               //entity.serialize(entityXML);
               xml.appendChild(entityXML);
            }
		    //PBE.log(this, "name: "+object.name);				
         }   
	 
		 PBE.log(this, "serializePBE: "+xml.toXMLString());		 
         return xml;      
       }
		
	}
	
}