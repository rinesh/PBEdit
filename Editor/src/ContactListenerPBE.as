/*
* Copyright (c) 2006-2007 Erin Catto http://www.gphysics.com
*
* This software is provided 'as-is', without any express or implied
* warranty.  In no event will the authors be held liable for any damages
* arising from the use of this software.
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications, and to alter it and redistribute it
* freely, subject to the following restrictions:
* 1. The origin of this software must not be misrepresented; you must not
* claim that you wrote the original software. If you use this software
* in a product, an acknowledgment in the product documentation would be
* appreciated but is not required.
* 2. Altered source versions must be plainly marked as such, and must not be
* misrepresented as being the original software.
* 3. This notice may not be removed or altered from any source distribution.
*/

package 
{
	import Box2D.Collision.b2ContactPoint;
	import Box2D.Collision.Shapes.b2Shape;
	import Box2D.Dynamics.b2Body;
	import Box2D.Dynamics.b2ContactListener;
	import com.pblabs.box2D.Box2DSpatialComponent;
	import com.pblabs.engine.entity.IEntityComponent;
	import com.pblabs.engine.PBE;

public class ContactListenerPBE extends b2ContactListener
{
    private var bodies:Array;
	
	public function ContactListenerPBE():void 
	{

	}
   /// Called when a contact point is added. This includes the geometry
   /// and the forces.
   public override function Add(point:b2ContactPoint) : void
   {
	    var shape1:b2Shape = point.shape1;
		var shape2:b2Shape = point.shape2;
		
	    var comp:Box2DSpatialComponent = PBE.lookupComponentByName("Ground", "Spatial") as Box2DSpatialComponent;

		if ( shape1.GetBody() == comp.body || shape2.GetBody() == comp.body )
		{
			
			if (!bodies)
			  bodies = new Array();
			if ( shape1.GetBody() == comp.body )
			{
				if (!check(shape2.GetBody()))
				{
					bodies.push(shape2.GetBody());
					Main.BallsRemaining--;
					//PBE.log(this, "Ground Hit");
				}
			}
			//else//if ( shape2.GetBody() == comp.body )
			//{
				//if (!check())
				//{
					//bodies.push(shape1.GetBody());
					//Main.BallsRemaining--;
					//PBE.log(this, "Ground Hit");
				//}	 
			//}
		}
   }
	
   private function check(body:b2Body):Boolean
   {
	   var found:Boolean = false;
	   
	   var i:int;
	   var j:int;
	   for (i = 0 ; i < bodies.length; i++)
	   {
			if (bodies[i] == body)
			{
				//PBE.log(this, "Match found "+i+"and"+j);
			    found = true; 
			} 
	   }
	   return found;
   }
   /// Called when a contact point persists. This includes the geometry
   /// and the forces.
   //public virtual function Persist(point:b2ContactPoint) : void{};

   /// Called when a contact point is removed. This includes the last
   /// computed geometry and forces.
   //public virtual function Remove(point:b2ContactPoint) : void{};

   /// Called after a contact point is solved.
   //public virtual function Result(point:b2ContactResult) : void{};
}

}
