using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho3DNet;
using ImGuiNET;


namespace rbfx
{
	class Program
	{
		static void Main(string[] args)
		{
			 
			using (var context = new Context())
			{
				using (var application = new DemoApplication(context))
				{
					application.Run();
				}
			}
		}

	}

	class DemoApplication : Application
	{
		private Scene _scene;
		private Viewport _viewport;
		private Node _camera;
		private Node _cube;
		private Node _light;

		public DemoApplication(Context context) : base(context)
		{
		}


		public void Dispose()
		{
			Engine.Renderer.SetViewport(0, null);    // Enable disposal of viewport by making it unreferenced by engine.
			_viewport.Dispose();
			_scene.Dispose();
			_camera.Dispose();
			_cube.Dispose();
			_light.Dispose();
			base.Dispose();
		}
		 

		public override void Setup()
		{
			var currentDir = Directory.GetCurrentDirectory();
			engineParameters_[Urho3D.EpFullScreen] = false;
			engineParameters_[Urho3D.EpWindowWidth] = 800;
			engineParameters_[Urho3D.EpWindowHeight] = 600;
			engineParameters_[Urho3D.EpWindowTitle] = "Hello C#";
			engineParameters_[Urho3D.EpResourcePrefixPaths] = $"{currentDir};{currentDir}/..";
		}

		public override void Start()
		{
			UI.Input.SetMouseVisible(true);

			// Viewport
			_scene = new Scene(Context);
			_scene.CreateComponent<Octree>();

			_camera = _scene.CreateChild("Camera");
			_viewport = new Viewport(Context);
			_viewport.Scene = _scene;
			_viewport.Camera = (_camera.CreateComponent<Camera>());
			Engine.Renderer.SetViewport(0, _viewport);

			// Background
			Engine.Renderer.DefaultZone.FogColor = (new Color(0.5f, 0.5f, 0.7f));

			// Scene
			_camera.Position = (new Vector3(0, 2, -2));
			_camera.LookAt(Vector3.Zero);

			// Cube
			_cube = _scene.CreateChild("Cube");
			var model = _cube.CreateComponent<StaticModel>();
			model.SetModel(Cache.GetResource<Model>("Models/Box.mdl"));
			model.SetMaterial(0, Cache.GetResource<Material>("Materials/Stone.xml"));
			var rotator = _cube.CreateComponent<RotateObject>();

			// Light
			_light = _scene.CreateChild("Light");
			_light.CreateComponent<Light>();
			_light.Position = (new Vector3(0, 2, -1));
			_light.LookAt(Vector3.Zero);

			SubscribeToEvent(E.Update, args =>
			{
				var timestep = args[E.Update.TimeStep].Float;
				Debug.Assert(this != null);

				if (ImGui.Begin("Urho3D.NET"))
				{
					ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), $"Hello world from C#.\nFrame time: {timestep}");
				}
				ImGui.End();
			});
		}
	}
}
