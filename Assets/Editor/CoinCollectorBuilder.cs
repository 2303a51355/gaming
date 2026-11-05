using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[InitializeOnLoad]
public static class CoinCollectorBuilder
{
    const string ScenePath = "Assets/CoinCollector.unity";
    static CoinCollectorBuilder()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (!File.Exists(ScenePath)) Generate();
        };
    }

    [MenuItem("Tools/Coin Collector/Generate Game Scene")]
    public static void Generate()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var game = new GameObject("Game Manager");
        game.AddComponent<CoinCollectorGame>();

        Material grass = Mat(new Color(0.20f,0.52f,0.29f));
        Material road = Mat(new Color(0.16f,0.18f,0.21f));
        Material curb = Mat(new Color(0.72f,0.75f,0.77f));
        Material wall = Mat(new Color(0.83f,0.86f,0.89f));
        Material blue = Mat(new Color(0.12f,0.42f,0.85f));
        Material blueLight = Mat(new Color(0.28f,0.62f,0.98f));
        Material gold = Mat(new Color(1f,0.72f,0.04f), true);
        Material red = Mat(new Color(0.72f,0.18f,0.16f));
        Material bark = Mat(new Color(0.32f,0.19f,0.10f));
        Material leaves = Mat(new Color(0.10f,0.42f,0.18f));
        Material lampMat = Mat(new Color(0.18f,0.20f,0.23f));
        Material white = Mat(new Color(0.92f,0.94f,0.98f));
        Material skin = Mat(new Color(0.92f,0.66f,0.48f));
        Material shirt = Mat(new Color(0.08f,0.30f,0.78f));
        Material pants = Mat(new Color(0.10f,0.13f,0.22f));
        Material shoes = Mat(new Color(0.08f,0.07f,0.06f));

        Make("Grass Park", PrimitiveType.Cube, new Vector3(0,-0.55f,0), new Vector3(36,1,36), grass);
        // Perimeter road loop with sidewalks and low boundary walls.
        Make("North Road", PrimitiveType.Cube, new Vector3(0,-0.02f,14), new Vector3(34,0.12f,3.2f), road);
        Make("South Road", PrimitiveType.Cube, new Vector3(0,-0.02f,-14), new Vector3(34,0.12f,3.2f), road);
        Make("East Road", PrimitiveType.Cube, new Vector3(14,-0.02f,0), new Vector3(3.2f,0.12f,25), road);
        Make("West Road", PrimitiveType.Cube, new Vector3(-14,-0.02f,0), new Vector3(3.2f,0.12f,25), road);
        Make("North Sidewalk", PrimitiveType.Cube, new Vector3(0,0.05f,11.9f), new Vector3(34,0.22f,0.7f), curb);
        Make("South Sidewalk", PrimitiveType.Cube, new Vector3(0,0.05f,-11.9f), new Vector3(34,0.22f,0.7f), curb);
        Make("East Sidewalk", PrimitiveType.Cube, new Vector3(11.9f,0.05f,0), new Vector3(0.7f,0.22f,23), curb);
        Make("West Sidewalk", PrimitiveType.Cube, new Vector3(-11.9f,0.05f,0), new Vector3(0.7f,0.22f,23), curb);
        Make("North Boundary Wall", PrimitiveType.Cube, new Vector3(0,1.1f,17.7f), new Vector3(36,2.2f,0.5f), wall);
        Make("South Boundary Wall", PrimitiveType.Cube, new Vector3(0,1.1f,-17.7f), new Vector3(36,2.2f,0.5f), wall);
        Make("East Boundary Wall", PrimitiveType.Cube, new Vector3(17.7f,1.1f,0), new Vector3(0.5f,2.2f,36), wall);
        Make("West Boundary Wall", PrimitiveType.Cube, new Vector3(-17.7f,1.1f,0), new Vector3(0.5f,2.2f,36), wall);

        // Road lane dashes.
        for(int i=-5;i<=5;i++)
        {
            Make("Road Mark North "+i, PrimitiveType.Cube, new Vector3(i*2.7f,0.055f,14), new Vector3(1.2f,0.025f,0.09f), white);
            Make("Road Mark South "+i, PrimitiveType.Cube, new Vector3(i*2.7f,0.055f,-14), new Vector3(1.2f,0.025f,0.09f), white);
        }

        // Raised platforms and stepping stones that make high coins reachable.
        Vector3[] plats = {
            new Vector3(-7,0.35f,-5), new Vector3(6,0.55f,-7),
            new Vector3(-5,0.45f,7), new Vector3(8,0.35f,6),
            new Vector3(0,0.25f,9), new Vector3(1,0.45f,-10)
        };
        for(int i=0;i<plats.Length;i++)
            Make("Jump Platform "+(i+1), PrimitiveType.Cube, plats[i], new Vector3(4.2f,0.7f,4.2f), (i%2==0?blue:blueLight));

        // Short stair blocks beside the taller platforms.
        Make("Step A", PrimitiveType.Cube, new Vector3(-8.5f,0.18f,-2.2f), new Vector3(2.2f,0.36f,1.8f), blueLight);
        Make("Step B", PrimitiveType.Cube, new Vector3(-7.8f,0.38f,-3.8f), new Vector3(2.0f,0.76f,1.8f), blue);
        Make("Step C", PrimitiveType.Cube, new Vector3(4.5f,0.18f,-9.5f), new Vector3(2.0f,0.36f,1.8f), blueLight);
        Make("Step D", PrimitiveType.Cube, new Vector3(5.2f,0.38f,-8.0f), new Vector3(2.0f,0.76f,1.8f), blue);

        // Decorative low obstacles, kept away from main approach paths.
        for(int i=0;i<8;i++)
        {
            float a=i*Mathf.PI*2f/8f;
            Vector3 pos=new Vector3(Mathf.Cos(a)*9.5f,0.8f,Mathf.Sin(a)*9.5f);
            Make("Challenge Pillar "+(i+1), PrimitiveType.Cylinder, pos, new Vector3(1.1f,1.6f,1.1f), red);
        }

        // Trees around the park edge.
        for(int i=0;i<12;i++)
        {
            float a=i*Mathf.PI*2f/12f;
            Vector3 pos=new Vector3(Mathf.Cos(a)*15.2f,0,Mathf.Sin(a)*15.2f);
            Make("Tree Trunk "+i, PrimitiveType.Cylinder, pos+Vector3.up*1.0f, new Vector3(0.55f,2f,0.55f), bark);
            Make("Tree Crown "+i, PrimitiveType.Sphere, pos+Vector3.up*3.0f, new Vector3(2.4f,2.6f,2.4f), leaves);
        }
        // Street lamps around the loop.
        Vector3[] lampPos={new Vector3(-10,0,12),new Vector3(10,0,12),new Vector3(-10,0,-12),new Vector3(10,0,-12)};
        for(int i=0;i<lampPos.Length;i++)
        {
            Make("Lamp Post "+i, PrimitiveType.Cylinder, lampPos[i]+Vector3.up*2f, new Vector3(0.16f,4f,0.16f), lampMat);
            Make("Lamp Head "+i, PrimitiveType.Sphere, lampPos[i]+Vector3.up*4.1f, new Vector3(0.55f,0.25f,0.55f), gold);
        }

        // Richer park background: winding garden paths, pond, flower beds, benches,
        // shrubs, rocks and a central gazebo to make the park feel less empty.
        Material pathMat = Mat(new Color(0.68f,0.61f,0.46f));
        Material waterMat = Mat(new Color(0.10f,0.48f,0.72f));
        Material flowerRed = Mat(new Color(0.88f,0.18f,0.28f));
        Material flowerYellow = Mat(new Color(1.0f,0.72f,0.12f));
        Material benchWood = Mat(new Color(0.38f,0.20f,0.10f));
        Material stone = Mat(new Color(0.42f,0.46f,0.49f));
        Material shrubMat = Mat(new Color(0.08f,0.32f,0.12f));

        // Meandering tan garden paths across the grass.
        Make("Garden Path A", PrimitiveType.Cube, new Vector3(-5f,-0.01f,0f), new Vector3(1.7f,0.08f,17f), pathMat).transform.rotation=Quaternion.Euler(0,18,0);
        Make("Garden Path B", PrimitiveType.Cube, new Vector3(4f,-0.005f,1f), new Vector3(1.6f,0.08f,16f), pathMat).transform.rotation=Quaternion.Euler(0,-24,0);
        Make("Garden Path C", PrimitiveType.Cube, new Vector3(0f,0.0f,5f), new Vector3(18f,0.08f,1.5f), pathMat).transform.rotation=Quaternion.Euler(0,12,0);

        // Decorative pond and stepping stones in the north-east garden.
        Make("Park Pond", PrimitiveType.Cylinder, new Vector3(8.4f,-0.12f,9.0f), new Vector3(4.2f,0.18f,3.0f), waterMat);
        for(int i=0;i<5;i++)
            Make("Pond Stone "+i, PrimitiveType.Sphere, new Vector3(6.7f+i*0.75f,0.05f,8.0f+(i%2)*0.65f), new Vector3(0.85f,0.22f,0.65f), stone);

        // Central gazebo with four posts and a roof.
        Material roofMat = Mat(new Color(0.48f,0.16f,0.12f));
        Make("Gazebo Floor", PrimitiveType.Cylinder, new Vector3(0f,0.12f,0f), new Vector3(4.4f,0.25f,4.4f), stone);
        for(int i=0;i<4;i++)
        {
            float x=(i%2==0?-1.65f:1.65f), z=(i<2?-1.65f:1.65f);
            Make("Gazebo Post "+i, PrimitiveType.Cylinder, new Vector3(x,1.35f,z), new Vector3(0.22f,2.7f,0.22f), benchWood);
        }
        Make("Gazebo Roof", PrimitiveType.Cube, new Vector3(0f,2.85f,0f), new Vector3(4.3f,0.35f,4.3f), roofMat);

        // Benches facing the garden paths.
        for(int i=0;i<4;i++)
        {
            Vector3 bp=new Vector3(i<2?-9f:9f,0.35f,(i%2==0?-3f:4f));
            Make("Bench Seat "+i, PrimitiveType.Cube, bp, new Vector3(2.0f,0.18f,0.65f), benchWood);
            Make("Bench Back "+i, PrimitiveType.Cube, bp+new Vector3(0,0.55f,-0.25f), new Vector3(2.0f,0.75f,0.16f), benchWood);
            Make("Bench Leg L "+i, PrimitiveType.Cube, bp+new Vector3(-0.72f,-0.28f,0), new Vector3(0.15f,0.55f,0.45f), lampMat);
            Make("Bench Leg R "+i, PrimitiveType.Cube, bp+new Vector3(0.72f,-0.28f,0), new Vector3(0.15f,0.55f,0.45f), lampMat);
        }

        // Flower beds and shrubs create visual depth without blocking the whole route.
        for(int i=0;i<18;i++)
        {
            float a=i*Mathf.PI*2f/18f;
            Vector3 fp=new Vector3(Mathf.Cos(a)*7.8f,0.18f,Mathf.Sin(a)*7.8f);
            Make("Flower Bed "+i, PrimitiveType.Cylinder, fp, new Vector3(1.0f,0.18f,1.0f), shrubMat);
            Make("Flower "+i, PrimitiveType.Sphere, fp+Vector3.up*0.35f, new Vector3(0.42f,0.42f,0.42f), i%2==0?flowerRed:flowerYellow);
        }
        for(int i=0;i<14;i++)
        {
            float a=i*Mathf.PI*2f/14f;
            Vector3 sp=new Vector3(Mathf.Cos(a)*11.0f,0.55f,Mathf.Sin(a)*11.0f);
            Make("Park Shrub "+i, PrimitiveType.Sphere, sp, new Vector3(1.4f,1.1f,1.4f), shrubMat);
        }

        // Small hurdle bars and zig-zag challenge posts on inner grass routes.
        for(int i=0;i<5;i++)
        {
            float z=-8f+i*4f;
            Make("Garden Hurdle "+i, PrimitiveType.Cube, new Vector3(-2.5f,0.42f,z), new Vector3(2.2f,0.18f,0.22f), red);
            Make("Hurdle Support L "+i, PrimitiveType.Cylinder, new Vector3(-3.45f,0.35f,z), new Vector3(0.12f,0.7f,0.12f), lampMat);
            Make("Hurdle Support R "+i, PrimitiveType.Cylinder, new Vector3(-1.55f,0.35f,z), new Vector3(0.12f,0.7f,0.12f), lampMat);
        }

        // Player root with CharacterController and visible low-poly human model.
        var player=new GameObject("Player");
        player.tag="Player"; player.transform.position=new Vector3(0,1,0);
        var cc=player.AddComponent<CharacterController>(); cc.height=2f; cc.radius=0.38f; cc.center=new Vector3(0,0,0);
        var controller=player.AddComponent<PlayerController>();
        Transform model=new GameObject("Human Character Model").transform; model.SetParent(player.transform); model.localPosition=Vector3.zero;
        // Torso and head
        AddPart("Body Shirt",PrimitiveType.Capsule,model,new Vector3(0,0.95f,0),new Vector3(0.58f,0.72f,0.34f),shirt);
        AddPart("Head",PrimitiveType.Sphere,model,new Vector3(0,1.62f,0),new Vector3(0.34f,0.38f,0.34f),skin);
        AddPart("Hair",PrimitiveType.Sphere,model,new Vector3(0,1.78f,-0.025f),new Vector3(0.36f,0.19f,0.36f),pants);
        // Leg pivots and limbs
        Transform leftLeg=MakeLimb("Left Leg",model,new Vector3(-0.17f,0.58f,0),new Vector3(0.19f,0.62f,0.22f),pants);
        Transform rightLeg=MakeLimb("Right Leg",model,new Vector3(0.17f,0.58f,0),new Vector3(0.19f,0.62f,0.22f),pants);
        AddPart("Left Shoe",PrimitiveType.Cube,leftLeg,new Vector3(0,-0.29f,0.08f),new Vector3(0.22f,0.12f,0.38f),shoes);
        AddPart("Right Shoe",PrimitiveType.Cube,rightLeg,new Vector3(0,-0.29f,0.08f),new Vector3(0.22f,0.12f,0.38f),shoes);
        Transform leftArm=MakeLimb("Left Arm",model,new Vector3(-0.39f,1.12f,0),new Vector3(0.18f,0.62f,0.20f),shirt);
        Transform rightArm=MakeLimb("Right Arm",model,new Vector3(0.39f,1.12f,0),new Vector3(0.18f,0.62f,0.20f),shirt);
        AddPart("Left Hand",PrimitiveType.Sphere,leftArm,new Vector3(0,-0.31f,0),new Vector3(0.15f,0.15f,0.15f),skin);
        AddPart("Right Hand",PrimitiveType.Sphere,rightArm,new Vector3(0,-0.31f,0),new Vector3(0.15f,0.15f,0.15f),skin);
        controller.leftLeg=leftLeg; controller.rightLeg=rightLeg; controller.leftArm=leftArm; controller.rightArm=rightArm;

        var pivot=new GameObject("Camera Pivot").transform; pivot.SetParent(player.transform); pivot.localPosition=new Vector3(0,1.25f,0);
        var cam=new GameObject("Main Camera").AddComponent<Camera>(); cam.tag="MainCamera";
        cam.transform.SetParent(pivot); cam.transform.localPosition=new Vector3(0,0,-5.5f); cam.transform.localRotation=Quaternion.identity;
        cam.fieldOfView=65; cam.gameObject.AddComponent<AudioListener>(); controller.cameraPivot=pivot;

        // Coins are deliberately mixed between the perimeter road, inner grass,
        // garden paths and reachable platform edges. Some require a small detour or jump.
        Vector3[] coinPositions = new Vector3[]
        {
            // Road coins: clear route markers, not one straight line.
            new Vector3(-11f,0.85f,14f), new Vector3(-5f,0.85f,14f), new Vector3(3f,0.85f,14f), new Vector3(10f,0.85f,14f),
            new Vector3(14f,0.85f,9f), new Vector3(14f,0.85f,2f), new Vector3(14f,0.85f,-6f), new Vector3(14f,0.85f,-12f),
            new Vector3(10f,0.85f,-14f), new Vector3(3f,0.85f,-14f), new Vector3(-5f,0.85f,-14f), new Vector3(-11f,0.85f,-14f),
            new Vector3(-14f,0.85f,-9f), new Vector3(-14f,0.85f,-2f), new Vector3(-14f,0.85f,6f), new Vector3(-14f,0.85f,11f),

            // Grass detours: coins around trees, flower beds and garden corners.
            new Vector3(-9f,0.85f,9f), new Vector3(-7f,0.85f,10f), new Vector3(-4f,0.85f,8f), new Vector3(-2f,0.85f,11f),
            new Vector3(3f,0.85f,10f), new Vector3(7f,0.85f,9f), new Vector3(9f,0.85f,6f), new Vector3(10f,0.85f,2f),
            new Vector3(9f,0.85f,-3f), new Vector3(7f,0.85f,-7f), new Vector3(3f,0.85f,-9f), new Vector3(-2f,0.85f,-10f),
            new Vector3(-7f,0.85f,-8f), new Vector3(-10f,0.85f,-5f), new Vector3(-10f,0.85f,0f), new Vector3(-9f,0.85f,5f),

            // Garden zig-zag trail with small turns and obstacle approaches.
            new Vector3(-6f,0.85f,2f), new Vector3(-4f,0.85f,4f), new Vector3(-1f,0.85f,3f), new Vector3(2f,0.85f,5f),
            new Vector3(5f,0.85f,3f), new Vector3(6f,0.85f,0f), new Vector3(4f,0.85f,-3f), new Vector3(1f,0.85f,-5f),
            new Vector3(-2f,0.85f,-4f), new Vector3(-5f,0.85f,-2f), new Vector3(-8f,0.85f,3f), new Vector3(8f,0.85f,-1f),

            // A few raised/edge coins encourage jumping onto the low platforms.
            new Vector3(-7f,1.15f,-5f), new Vector3(6f,1.35f,-7f), new Vector3(-5f,1.25f,7f),
            new Vector3(8f,1.15f,6f), new Vector3(0f,1.05f,9f), new Vector3(1f,1.25f,-10f),

            // Extra grass-side collectibles near park features.
            new Vector3(-12f,0.85f,4f), new Vector3(-11f,0.85f,-4f), new Vector3(11f,0.85f,5f), new Vector3(11f,0.85f,-5f),
            new Vector3(-4f,0.85f,12f), new Vector3(5f,0.85f,12f), new Vector3(-4f,0.85f,-12f), new Vector3(5f,0.85f,-12f),
            new Vector3(-8f,0.85f,-11f), new Vector3(8f,0.85f,11f)
        };
        for(int i=0;i<coinPositions.Length;i++)
        {
            var coin=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.name="Coin_"+(i+1).ToString("00"); coin.transform.position=coinPositions[i];
            coin.transform.localScale=new Vector3(0.72f,0.12f,0.72f);
            coin.GetComponent<Renderer>().sharedMaterial=gold;
            coin.GetComponent<Collider>().isTrigger=true;
            coin.AddComponent<CoinPickup>();
        }

        var light=new GameObject("Directional Light").AddComponent<Light>();
        light.type=LightType.Directional; light.intensity=1.25f; light.transform.rotation=Quaternion.Euler(48,-32,0);
        RenderSettings.ambientLight=new Color(0.68f,0.72f,0.78f);
        RenderSettings.fog=true; RenderSettings.fogColor=new Color(0.65f,0.76f,0.86f); RenderSettings.fogDensity=0.006f;

        EditorSceneManager.SaveScene(scene,ScenePath);
        AssetDatabase.SaveAssets();
        Debug.Log("Enhanced Coin Collector scene generated. Press Play!");
    }

    static Transform MakeLimb(string n,Transform parent,Vector3 localPos,Vector3 scale,Material mat)
    {
        var pivot=new GameObject(n+" Pivot").transform; pivot.SetParent(parent); pivot.localPosition=localPos;
        AddPart(n,PrimitiveType.Capsule,pivot,new Vector3(0,-0.18f,0),scale,mat);
        return pivot;
    }
    static GameObject AddPart(string n,PrimitiveType type,Transform parent,Vector3 localPos,Vector3 scale,Material mat)
    {
        var o=GameObject.CreatePrimitive(type); o.name=n; o.transform.SetParent(parent); o.transform.localPosition=localPos; o.transform.localScale=scale;
        var c=o.GetComponent<Collider>(); if(c) Object.DestroyImmediate(c);
        o.GetComponent<Renderer>().sharedMaterial=mat; return o;
    }
    static GameObject Make(string n,PrimitiveType t,Vector3 p,Vector3 s,Material m)
    {
        var o=GameObject.CreatePrimitive(t); o.name=n; o.transform.position=p; o.transform.localScale=s; o.GetComponent<Renderer>().sharedMaterial=m; return o;
    }
    static Material Mat(Color c,bool emission=false)
    {
        Shader sh=Shader.Find("Standard"); if(sh==null) sh=Shader.Find("Universal Render Pipeline/Lit");
        var m=new Material(sh); m.color=c;
        if(emission){m.EnableKeyword("_EMISSION");m.SetColor("_EmissionColor",c*0.5f);}
        return m;
    }
}
