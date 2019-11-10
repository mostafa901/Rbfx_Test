using Urho3DNet;


namespace rbfx
{
	[ObjectFactory]
	class RotateObject : LogicComponent
	{
		public RotateObject(Context context) : base(context)
		{
			UpdateEventMask = UpdateEvent.UseUpdate;

		}

		public override void Update(float timeStep)
		{
			var d = new Quaternion(10 * timeStep, 20 * timeStep, 30 * timeStep);
			Node.Rotate(d);
		}
	}
}
