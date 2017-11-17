using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UTJ {

public class SystemManager : MonoBehaviour {

	// singleton
	static SystemManager instance_;
	public static SystemManager Instance { get { return instance_; } }

	public System.Diagnostics.Stopwatch stopwatch_;
	private bool subjective_ = true;
	private float view_offset_z_ = 0f;
	private float flow_speed_target_ = 0f;
	private float flow_speed_ = 0f;
	private Thread update_thread_;
	private int rendering_front_;
	public int getRenderingFront() { return rendering_front_; }
	private DrawBuffer[] draw_buffer_;
	private System.Threading.ManualResetEvent manual_reset_event_;
	private int fps_;
	private float dt_;
	private float dt_ratio_ = 1f;
	public float getDT() { return dt_; }
	public int getFPS()	{ return fps_; }
	public void setFPS(int fps)	{
		fps = Mathf.Clamp(fps, 1, 300);
		fps_ = fps;
		dt_ = dt_ratio_ / (float)fps_;
	}
	public void setBulletTime(bool flg) {
		dt_ratio_ = flg ? 0.1f : 1f;
	}
	private bool pause_ = false;
	private int fast_forward_ratio_ = 1;
	private bool reset_position_ = false;

	private long update_frame_;
	private long update_sync_frame_;
	private long render_frame_;
	private long render_sync_frame_;
	private double update_time_;
	private int gc_start_count_;
	private long used_heap_size_;
	private long mono_heap_size_;
	private long mono_used_size_;
	private float prev_frame_time_;
	private float elapsed_frame_time_;
	private float elapsed_frame_time_display_;

	private MyCamera my_camera_;
	private Player player_;
    public GameObject playerCamera;
    public GameObject player_prefab_;
	public GameObject zako_prefab_;
	public GameObject dragon_head_prefab_;
	public GameObject dragon_body_prefab_;
	public GameObject dragon_tail_prefab_;
	public GameObject shutter_prefab_;
	public Material debris_material_;
	public Material spark_material_;
	public Material beam_material_;
	public Material beam2_material_;
	public Material trail_material_;
	public Material explosion_material_;
	public Material hahen_material_;
	public Material shockwave_material_;
	public Material lockon_range_material_;
	public Material sight_material_;
	public Material shield_material_;
	public Sprite[] sprites_;
	public Material vrsprite_material_;
	public Material sprite_material_;
	public Font font_;
	public Material font_material_;

	public GameObject camera_holder_;
	private Transform camera_holder_transform_;
	private Camera camera_;
	public Matrix4x4 ProjectionMatrix { get; set; }

	private GameObject player_go_;
	private Transform player_arm_transform_;
	private const int ZAKO_MAX = 32;
	private const int DRAGON_HEAD_MAX = 1;
	private const int DRAGON_BODY_MAX = 8;
	private const int DRAGON_TAIL_MAX = 1;
	private const int SHUTTER_MAX = 16;
	private GameObject[] zako_pool_;
	private GameObject[] dragon_head_pool_;
	private GameObject[] dragon_body_pool_;
	private GameObject[] dragon_tail_pool_;
	private GameObject[] shutter_pool_;
	private UVScroller[] uv_scroller_list_;
	
	// audio
	public const int AUDIO_CHANNEL_MAX = 4;

	private const int AUDIOSOURCE_BULLET_MAX = 4;
	private AudioSource[] audio_sources_bullet_;
	int audio_source_bullet_index_;
	public AudioClip se_bullet_;

	private const int AUDIOSOURCE_EXPLOSION_MAX = 4;
	private AudioSource[] audio_sources_explosion_;
	int audio_source_explosion_index_;
	public AudioClip se_explosion_;

	private const int AUDIOSOURCE_MISSILE_MAX = 4;
	private AudioSource[] audio_sources_missile_;
	int audio_source_missile_index_;
	public AudioClip se_missile_;

	private const int AUDIOSOURCE_LOCKON_MAX = 4;
	private AudioSource[] audio_sources_lockon_;
	int audio_source_lockon_index_;
	public AudioClip se_lockon_;

	private const int AUDIOSOURCE_SHIELD_MAX = 4;
	private AudioSource[] audio_sources_shield_;
	int audio_source_shield_index_;
	public AudioClip se_shield_;

	private AudioSource audio_sources_voice_ikuyo_;
	public AudioClip se_voice_ikuyo_;
	private AudioSource audio_sources_voice_uwaa_;
	public AudioClip se_voice_uwaa_;
	private AudioSource audio_sources_voice_sorosoro_;
	public AudioClip se_voice_sorosoro_;
	private AudioSource audio_sources_voice_ototo_;
	public AudioClip se_voice_ototo_;
	private AudioSource audio_sources_voice_yoshi_;
	public AudioClip se_voice_yoshi_;

	private AudioSource audio_sources_bgm_;
	public AudioClip bgm01_;
	private bool is_bgm_playing_;

	// work
	private long pause_back_time_ = 0;

	// debug
	private int display_fps_;
	public long update_tick_;
	public long render_update_tick_;
	public long render_tick_;
	public long render_start_tick_;
	public long render_tick2_;
	public long render_start_tick2_;
	public long render_tick3_;
	public long render_tick4_;

	private void initialize()
	{
#if UNITY_PSP2
		UnityEngine.PSVita.Diagnostics.enableHUD = true;
#endif

		DontDestroyOnLoad(gameObject);

		stopwatch_ = new System.Diagnostics.Stopwatch();
		stopwatch_.Start();
		rendering_front_ = 0;

		camera_ = GameObject.Find("MainCamera").GetComponent<Camera>(); // Camera.main;
		camera_.eventMask = 0;

		{
			player_go_ = Instantiate(player_prefab_) as GameObject;
			player_arm_transform_ = player_go_.transform.Find("fighter_arm");
			{
				var go = new GameObject();
				go.name = "LockonRangeRenderer";
				go.transform.position = CV.Vector3Zero;
				go.transform.rotation = CV.QuaternionIdentity;
				var lr = go.AddComponent<LockonRangeRenderer>();
				LockonRangeRenderer.setInstance(lr);
				go.transform.SetParent(player_go_.transform);
				go.transform.localPosition = new Vector3(0f, 0.5f, 0f);
			}
			{
				var go = new GameObject();
				go.name = "ShieldRenderer";
				go.transform.position = Vector3.zero;
				go.transform.rotation = Quaternion.identity;
				go.AddComponent<ShieldRenderer>();

				go.transform.SetParent(player_go_.transform);
				go.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
		}

		Options.Instance.init();
		InputManager.Instance.init();
		GameManager.Instance.init();
		TaskManager.Instance.init();
		MyCollider.createPool();
		LockTarget.createPool();
		Missile.createPool();
		Bullet.createPool();
		Enemy.createPool();
		EnemyBullet.createPool();
		Shutter.createPool();
		Debris.Instance.init(debris_material_);
		Spark.Instance.init(spark_material_);
		Beam.Instance.init(beam_material_);
		Beam2.Instance.init(beam2_material_);
		Trail.Instance.init(trail_material_);
		Explosion.Instance.init(explosion_material_);
		Hahen.Instance.init(hahen_material_);
		Shockwave.Instance.init(shockwave_material_);
		// HUD.Instance.init();
		LockonRange.Instance.init(lockon_range_material_);
		LockonRangeRenderer.Instance.init(camera_);
		Shield.Instance.init(shield_material_);
		Sight.Instance.init(sight_material_);
		if (SightRenderer.Instance)
			SightRenderer.Instance.init(camera_);
		VRSprite.Instance.init(sprites_, vrsprite_material_);
		if (VRSpriteRenderer.Instance)
			VRSpriteRenderer.Instance.init(camera_);
		Notice.createPool();
		MySprite.Instance.init(sprites_, sprite_material_);
		MySpriteRenderer.Instance.init(camera_);
		MyFont.Instance.init(font_, font_material_);
		MyFontRenderer.Instance.init(camera_);

		draw_buffer_ = new DrawBuffer[2];
		for (int i = 0; i < 2; ++i) {
			draw_buffer_[i].init();
		}
		manual_reset_event_ = new System.Threading.ManualResetEvent(false);
#if UNITY_PS4
		setFPS(120);
#else
		setFPS(90);
#endif
		update_frame_ = 0;
		update_sync_frame_ = 0;
		update_time_ = 0f;
		render_frame_ = 0;
		render_sync_frame_ = 0;

		camera_holder_transform_ = camera_holder_.transform;
		ProjectionMatrix = camera_.projectionMatrix;

		if (zako_prefab_ != null) {
			zako_pool_ = new GameObject[ZAKO_MAX];
			for (var i = 0; i < ZAKO_MAX; ++i) {
				zako_pool_[i] = Instantiate(zako_prefab_) as GameObject;
				zako_pool_[i].SetActive(false);
			}
		}
		if (dragon_head_prefab_ != null) {
			dragon_head_pool_ = new GameObject[DRAGON_HEAD_MAX];
			for (var i = 0; i < DRAGON_HEAD_MAX; ++i) {
				dragon_head_pool_[i] = Instantiate(dragon_head_prefab_) as GameObject;
				dragon_head_pool_[i].SetActive(false);
			}
		}
		if (dragon_body_prefab_ != null) {
			dragon_body_pool_ = new GameObject[DRAGON_BODY_MAX];
			for (var i = 0; i < DRAGON_BODY_MAX; ++i) {
				dragon_body_pool_[i] = Instantiate(dragon_body_prefab_) as GameObject;
				dragon_body_pool_[i].SetActive(false);
			}
		}
		if (dragon_tail_prefab_ != null) {
			dragon_tail_pool_ = new GameObject[DRAGON_TAIL_MAX];
			for (var i = 0; i < DRAGON_TAIL_MAX; ++i) {
				dragon_tail_pool_[i] = Instantiate(dragon_tail_prefab_) as GameObject;
				dragon_tail_pool_[i].SetActive(false);
			}
		}
		if (shutter_prefab_ != null) {
			shutter_pool_ = new GameObject[SHUTTER_MAX];
			for (var i = 0; i < SHUTTER_MAX; ++i) {
				shutter_pool_[i] = Instantiate(shutter_prefab_) as GameObject;
				shutter_pool_[i].SetActive(false);
			}
		}

		uv_scroller_list_ = new UVScroller[8];

		// audio
		audio_sources_bullet_ = new AudioSource[AUDIOSOURCE_BULLET_MAX];
		for (var i = 0; i < AUDIOSOURCE_BULLET_MAX; ++i) {
			audio_sources_bullet_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_bullet_[i].clip = se_bullet_;
			audio_sources_bullet_[i].volume = 0.4f;
		}
		audio_source_bullet_index_ = 0;
		audio_sources_explosion_ = new AudioSource[AUDIOSOURCE_EXPLOSION_MAX];
		for (var i = 0; i < AUDIOSOURCE_EXPLOSION_MAX; ++i) {
			audio_sources_explosion_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_explosion_[i].clip = se_explosion_;
			audio_sources_explosion_[i].volume = 0.25f;
		}
		audio_source_explosion_index_ = 0;
		audio_sources_missile_ = new AudioSource[AUDIOSOURCE_MISSILE_MAX];
		for (var i = 0; i < AUDIOSOURCE_MISSILE_MAX; ++i) {
			audio_sources_missile_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_missile_[i].clip = se_missile_;
			audio_sources_missile_[i].volume = 0.25f;
		}
		audio_source_missile_index_ = 0;
		audio_sources_lockon_ = new AudioSource[AUDIOSOURCE_LOCKON_MAX];
		for (var i = 0; i < AUDIOSOURCE_LOCKON_MAX; ++i) {
			audio_sources_lockon_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_lockon_[i].clip = se_lockon_;
			audio_sources_lockon_[i].volume = 0.20f;
		}
		audio_source_lockon_index_ = 0;
		audio_sources_shield_ = new AudioSource[AUDIOSOURCE_SHIELD_MAX];
		for (var i = 0; i < AUDIOSOURCE_SHIELD_MAX; ++i) {
			audio_sources_shield_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_shield_[i].clip = se_shield_;
			audio_sources_shield_[i].volume = 0.25f;
		}
		audio_source_shield_index_ = 0;

		audio_sources_voice_ikuyo_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_ikuyo_.clip = se_voice_ikuyo_;
		audio_sources_voice_ikuyo_.volume = 0.75f;
		audio_sources_voice_uwaa_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_uwaa_.clip = se_voice_uwaa_;
		audio_sources_voice_uwaa_.volume = 0.75f;
		audio_sources_voice_sorosoro_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_sorosoro_.clip = se_voice_sorosoro_;
		audio_sources_voice_sorosoro_.volume = 0.75f;
		audio_sources_voice_ototo_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_ototo_.clip = se_voice_ototo_;
		audio_sources_voice_ototo_.volume = 0.75f;
		audio_sources_voice_yoshi_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_yoshi_.clip = se_voice_yoshi_;
		audio_sources_voice_yoshi_.volume = 0.75f;

		audio_sources_bgm_ = gameObject.AddComponent<AudioSource>();
		audio_sources_bgm_.clip = bgm01_;
		audio_sources_bgm_.volume = 0.5f;
		audio_sources_bgm_.loop = true;
		is_bgm_playing_ = false;
	
		gc_start_count_ = System.GC.CollectionCount(0 /* generation */);

		my_camera_ = MyCamera.create();
		player_ = Player.create();

		flow_speed_target_ = 0f;
		flow_speed_ = 0f;
	}

	public void setFlowSpeed(float v)
	{
		flow_speed_target_ = v;
	}

	void OnDestroy()
	{
		if (update_thread_ != null) {
			update_thread_.Abort();
		}
	}

	public void restart()
	{
		GameManager.Instance.restart();
		TaskManager.Instance.restart();
		MyCollider.restart();
		LockTarget.restart();
		TubeScroller.Instance.restart();
		Beam.Instance.restart();
		Trail.Instance.restart();
		Sight.Instance.restart();
		setBulletTime(false);
		flow_speed_target_ = 0f;
		flow_speed_ = 0f;
	}

	private void renderUpdate_debug_info(int updating_front)
	{
		// debug info
		if (InputManager.Instance.getButton(InputManager.Button.Debug) > 0) {
			if (Options.Instance.PerformanceMeter) {
				Options.Instance.PerformanceMeter = false;
			} else {
				if (pause_) {
					Options.Instance.PerformanceMeter = true;
				}
			}
		}

		if (!Options.Instance.PerformanceMeter) {
		    {
				MyFont.Instance.putNumber(updating_front, (int)(1f/(elapsed_frame_time_display_)*100f), 5 /* keta */, 2f /* scale */,
										  -440f, 192f, MyFont.Type.Green, 2 /* decimal_point */);
				MyFont.Instance.putString(updating_front, "FPS", 1f /* scale */, -200f, 192f, MyFont.Type.Green);
               
			}
		} else {
		    {
				MyFont.Instance.putNumber(updating_front, (int)update_frame_, 8 /* keta */, 1f /* scale */,
										  -440f, 256f, MyFont.Type.Green);
			}
		    {
				MyFont.Instance.putNumber(updating_front, (int)(1f/(elapsed_frame_time_display_) * 1000f), 9 /* keta */, 1f /* scale */,
										  -240f, 256f, MyFont.Type.Green);
			}
		    {
				int gc_count = System.GC.CollectionCount(0 /* generation */) - gc_start_count_;
				MyFont.Instance.putNumber(updating_front, gc_count, 8 /* keta */, 1f /* scale */,
										  -440f, 224f, MyFont.Type.Red);
                    
			}
			if (used_heap_size_ != 0) {
				int bytes = (int)used_heap_size_;
				MyFont.Instance.putNumber(updating_front, bytes, 9 /* keta */, 1f /* scale */,
										  -240f, 224f, MyFont.Type.Red);
			}
			if (mono_heap_size_ != 0) {
				int bytes = (int)mono_heap_size_;
				MyFont.Instance.putNumber(updating_front, bytes, 9 /* keta */, 1f /* scale */,
										  -40f, 224f, MyFont.Type.Red);
			}
			if (mono_used_size_ != 0) {
				int bytes = (int)mono_used_size_;
				MyFont.Instance.putNumber(updating_front, bytes, 9 /* keta */, 1f /* scale */,
										  160f, 224f, MyFont.Type.Red);
			}
		    {
				int task_count = TaskManager.Instance.getCount();
				MyFont.Instance.putNumber(updating_front, task_count, 8 /* keta */, 1f /* scale */,
										  -440f, 192f, MyFont.Type.Blue);
			}
			{
				MyFont.Instance.putNumber(updating_front, (int)(TubeScroller.Instance.getDistance()), 8 /* keta */, 1f /* scale */,
										  -240f, 192f, MyFont.Type.Blue);
			}
			{
				MyFont.Instance.putNumber(updating_front, fast_forward_ratio_, 3 /* keta */, 1f /* scale */,
										  -40f, 192f, MyFont.Type.Mazenta);
			}

			var height = 5f;
			var length = 440f;
#if UNITY_PS4
			var badget = 8.333f;
#else
			var badget = 16.666f;
#endif
			var update_y = 262f;
			var render_y = 254f;
		    {
				var rect0b = new Rect(-length*0.5f, update_y, length, height);
				MySprite.Instance.put(updating_front, ref rect0b, MySprite.Kind.Square, MySprite.Type.Black);

				var update_ratio = (float)update_tick_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var rect1b = new Rect(length*(update_ratio*0.5f - 1f), update_y,
									  length*update_ratio, height);
				MySprite.Instance.put(updating_front, ref rect1b, MySprite.Kind.Square, MySprite.Type.Blue);

				var render_update_ratio = (float)render_update_tick_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var rect2b = new Rect(length*(update_ratio - 1f + render_update_ratio*0.5f), update_y,
									  length*update_ratio, height);
				MySprite.Instance.put(updating_front, ref rect2b, MySprite.Kind.Square, MySprite.Type.Green);
			}
#if UNITY_PSP2 || UNITY_PS4
		    {
				var render_ratio = (float)render_tick_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var rect0b = new Rect(-length*0.5f, render_y, length, height);
				MySprite.Instance.put(updating_front, ref rect0b, MySprite.Kind.Square, MySprite.Type.Black);
				var rect1b = new Rect(length*(render_ratio*0.5f - 1f), render_y, length*render_ratio, height);
				MySprite.Instance.put(updating_front, ref rect1b, MySprite.Kind.Square, MySprite.Type.Red);

				var render_ratio_begin = (float)render_tick3_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var render_ratio_end = (float)render_tick4_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var render_ratio_len = render_ratio_end - render_ratio_begin;
				var rect2b = new Rect(length*(render_ratio_len*0.5f - 1f + render_ratio_begin*0.5f), render_y,
				                      length*(render_ratio_len), height);
				MySprite.Instance.put(updating_front, ref rect2b, MySprite.Kind.Square, MySprite.Type.Magenta);
			}
#else
		    {
				var render_msec = (float)render_tick2_*1000f/((float)System.Diagnostics.Stopwatch.Frequency);
				var render_ratio = render_msec/badget;
				var rect0b = new Rect(-length*0.5f, render_y, length, height);
				MySprite.Instance.put(updating_front, ref rect0b, MySprite.Kind.Square, MySprite.Type.Black);
				var rect1b = new Rect(length*(render_ratio*0.5f - 1f), render_y, length*render_ratio, height);
				MySprite.Instance.put(updating_front, ref rect1b, MySprite.Kind.Square, MySprite.Type.Red);

				int render_mseci = (int)(render_msec * 1000f);
				MyFont.Instance.putNumber(updating_front, render_mseci, 9 /* keta */, 1f /* scale */,
										  -40f, 256f, MyFont.Type.Yellow);
			}
#endif
		}
	}



        //瞎猜一次，这里是模拟地面移动速度
	private void update_flow_speed(float dt)
	{
		const float accel = 60f; 
            
		if (Mathf.Abs(flow_speed_ - flow_speed_target_) <= accel * dt) {
			flow_speed_ = flow_speed_target_; 

                
            } else if (flow_speed_ < flow_speed_target_) {
			flow_speed_ += accel * dt;
		} else {
			flow_speed_ += -accel * dt;
		}
	}

	private void main_loop()
	{
		long begin_time = stopwatch_.ElapsedTicks;
		
		// flip
		int updating_front = 1 - rendering_front_;
		
		// update
		if (!pause_) {
			// adaptive frame rate
			setFPS((int)(1f/elapsed_frame_time_)); // call here for stable DT during pause.

			if (Options.Instance.PerformanceMeter &&
				InputManager.Instance.getButton(InputManager.Button.FFWPlus) > 0) {
				fast_forward_ratio_ += 9;
			}
			if (InputManager.Instance.getButton(InputManager.Button.FFWMinus) > 0) {
				fast_forward_ratio_ -= 9;
			}
			fast_forward_ratio_ = Mathf.Clamp(fast_forward_ratio_, 1, 10);
			
			for (var i = 0; i < fast_forward_ratio_; ++i) {
				update_flow_speed(dt_);
				TubeScroller.Instance.update(dt_, flow_speed_);
				GameManager.Instance.update(dt_, update_time_);
				Player.Instance.update(dt_, update_time_, flow_speed_);
				TaskManager.Instance.update(dt_, update_time_, flow_speed_);
				// lockon
				bool locked = false;
				if (player_.isNowLocking()) {
					locked = LockTarget.checkAll(player_, update_time_);
				}
				// collision
				MyCollider.calculate();
				
				if (locked) {
					registSound(DrawBuffer.SE.Lockon);
				}
				if (InputManager.Instance.getButton(InputManager.Button.Pause) > 0) {
					pause_ = true;
					registBgm(DrawBuffer.BGM.Pause);
					registMotion(DrawBuffer.Motion.Pause);
				}
				++update_frame_;
				update_time_ += dt_;
			}
		} else {
			if (InputManager.Instance.getButton(InputManager.Button.Pause) > 0) {
				pause_ = false;
				registBgm(DrawBuffer.BGM.Resume);
				registMotion(DrawBuffer.Motion.Resume);
			} else {
				if (InputManager.Instance.getButton(InputManager.Button.Back) > 0) {
					long elapsed_tick = stopwatch_.ElapsedTicks - pause_back_time_;
					float elapsed = (float)elapsed_tick/((float)System.Diagnostics.Stopwatch.Frequency);
					if (elapsed > 1f) {
						GameManager.Instance.setReplayMode(false);
						restart();
                           
						pause_ = false;
					}
				} else {
					pause_back_time_ = stopwatch_.ElapsedTicks;
				}
			}
		}
		// profile
		update_tick_ = stopwatch_.ElapsedTicks - begin_time;
		begin_time = stopwatch_.ElapsedTicks;

		// begin
		MyFont.Instance.begin();
		MySprite.Instance.begin();
		VRSprite.Instance.begin();
		Sight.Instance.begin();
		Beam.Instance.begin(updating_front);
		Beam2.Instance.begin(updating_front);
		Trail.Instance.begin(updating_front);
		Spark.Instance.begin();
		Explosion.Instance.begin();
		Hahen.Instance.begin();
		Shockwave.Instance.begin();
		Shield.Instance.begin();

		// renderUpdate
		draw_buffer_[updating_front].beginRender();
		my_camera_.renderUpdate(updating_front, ref draw_buffer_[updating_front]);
		player_.renderUpdate(updating_front, ref draw_buffer_[updating_front]);
		TaskManager.Instance.renderUpdate(updating_front, my_camera_, ref draw_buffer_[updating_front]);

		// debug info
		renderUpdate_debug_info(updating_front);

		// end
		Shield.Instance.end(updating_front);
		Shockwave.Instance.end(updating_front);
		Hahen.Instance.end(updating_front);
		Explosion.Instance.end(updating_front);
		Spark.Instance.end(updating_front);
		Trail.Instance.end();
		Beam2.Instance.end();
		Beam.Instance.end();
		Sight.Instance.end(updating_front);
		VRSprite.Instance.end(updating_front);
		MySprite.Instance.end(updating_front);
		MyFont.Instance.end(updating_front);

		render_update_tick_ = stopwatch_.ElapsedTicks - begin_time;
	}

	private void thread_entry()
	{
		// TaskManager.Instance.setCamera(camera);
		// restart();

		for (;;) {
			try {
				main_loop();
				while (update_sync_frame_ >= render_sync_frame_) {
					manual_reset_event_.WaitOne();
					manual_reset_event_.Reset();
				}
				++update_sync_frame_;

			} catch (System.Exception e) {
				Debug.Log(e);
			}
		}
	}

	void Awake()
	{
		SystemManager.instance_ = this;
		initialize();
		update_thread_ = new Thread(thread_entry);
		update_thread_.Start();
	}
	
	void Start()
	{
            #region 在游戏刚刚开始的时候调用一次重置游戏的方法，避免飞机开始时的降落
            //在游戏刚刚开始的时候调用一次重置游戏的方法，避免飞机开始时的降落
            print("此时出于自动战斗模式，点击切换游戏模式");
            ReplayManager.IsAutoAttack = false;
            SystemManager.Instance.restart();
            SystemManager.Instance.BloodVolume = 200f;
            EnemyBullet.AttackNumber = 0;
            SystemManager.Instance.NowBloodVolume = 1;
#endregion


            //由於将分数的面板改到预制物体上面，改为全局查找 就这样找，这样快
            fenshu = GameObject.Find("CanvasFenshuManage").transform.FindChild("Fenshu").gameObject.GetComponent<Text>();
            Tip = GameObject.Find("CanvasFenshuManage").transform.FindChild("Tip").gameObject.GetComponent<Image>();
            TipTwo = GameObject.Find("CanvasFenshuManage").transform.FindChild("TipTwo").gameObject.GetComponent<Image>();
            TipThree = GameObject.Find("CanvasFenshuManage").transform.FindChild("TipThree").gameObject.GetComponent<Image>();


            Score = GameObject.Find("CanvasFenshuManage").transform.FindChild("Score").gameObject.GetComponent<Text>();

            _mgameTip = GameObject.Find("GameTipBeganGame").GetComponent<Text>();
            tube_B01 = GameObject.Find("tube_B01(Clone)");

            _health = GameObject.Find("Health").GetComponent<Image>();
#if UNITY_5_3
		VRManager.instance.SetupHMDDevice(); // more graceful
		// VRManager.instance.BeginVRSetup();	 // than this
#endif
            StartCoroutine(Times());
	}
        IEnumerator Times()
        {
            yield return new WaitForSeconds(1);
            playerCamera = playerCamera.transform.Find("MainCamera").gameObject;
        }
        public float realtimeSinceStartup { get { return ((float)stopwatch_.ElapsedTicks) /  (float)System.Diagnostics.Stopwatch.Frequency; } }

	public Camera getCamera() { return camera_; }

	public void registUVScroller(UVScroller uv_scroller)
	{
		for (var i = 0; i < uv_scroller_list_.Length; ++i) {
			if (uv_scroller_list_[i] == null) {
				uv_scroller_list_[i] = uv_scroller;
				return;
			}
		}
		Debug.LogError("exceed UVScroller regist.");
		Debug.Assert(false);
	}

	public void registSound(DrawBuffer.SE se)
	{
		int updating_front = 1 - rendering_front_;
		draw_buffer_[updating_front].registSound(se);
	}

	public void registBgm(DrawBuffer.BGM bgm)
	{
		int updating_front = 1 - rendering_front_;
		draw_buffer_[updating_front].registBgm(bgm);
	}

	public void registMotion(DrawBuffer.Motion motion)
	{
		int updating_front = 1 - rendering_front_;
		draw_buffer_[updating_front].registMotion(motion);
	}

        /*
         * 以下は MailThread
         */

        // 入力更新
        public bool IsBegan = false;  //Temp测试
        public bool IsBeganDanyi = false;

        public bool IsRankMiss = false;

        IEnumerator WaitMiss()
        {
            yield return new WaitForSeconds(4f);
            RankManager.instance.Hide();
        }
        private void input_update()
	    {
            
            if (IsRankMiss)
            {
                RankManager.instance.Hide();
                IsRankMiss = false;
            }


            #region 玩家开火控制
            if (Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.Alpha3)|| Input.GetKeyDown(KeyCode.Alpha4)|| Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                //开始游戏时，如果有按键按下，停止开始的背景音，开始游戏，在游戏重置时，再次开启开始的背景音
                AudioControl.IsPlayBeganAudioClip = false;
                Player.Instance.CanFire = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKeyUp(KeyCode.JoystickButton0))
            {
                Player.Instance.CanFire = false;
            }
#endregion
            

            if (GameManager.Instance.IsEnd)
            {

                GameObject.Find("Canvas").transform.FindChild("Image").gameObject.SetActive(true);
                StartCoroutine(WaitMiss());
            }
            else
            {
                GameObject.Find("Canvas").transform.FindChild("Image").gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Alpha3)|| Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                IsBegan = true;
                IsBeganDanyi = true;
                print("IsBeganIsBeganIsBeganIsBeganIsBeganIsBeganIsBegan"+ IsBegan);
            }

            
            if (IsBegan)//按键准备就绪并且游戏开始的声音播放完毕
            {
                IsCanPlayDaojishi = true;
                int[] buttons = InputManager.Instance.referButtons();
                #region 头部可以控制飞机的运动
                //buttons[(int)InputManager.Button.Horizontal] = (int)(Input.GetAxis("Horizontal") * 4096f)
                //        + (playerCamera? Mathf.Clamp((int)(( playerCamera.transform.rotation * Vector3.forward).x * 2 * 4096), -4096, 4096):0);
                //buttons[(int)InputManager.Button.Vertical] = (int)(Input.GetAxis("Vertical") * (-4096f))
                //       + (playerCamera? Mathf.Clamp((int)((playerCamera.transform.rotation * Vector3.forward).y * 2 * 4096), -4096, 4096):0);
                #endregion

                #region 头部不能控制飞机的运动
                buttons[(int)InputManager.Button.Horizontal] = (int)(Input.GetAxis("Horizontal") * 4096f*0.5f*0.5f);    //在这里 *0.5f 可以将飞机的偏移速度降低
                        
                buttons[(int)InputManager.Button.Vertical] = (int)(Input.GetAxis("Vertical") * (-4096f*0.5f*0.5f));    //在这里 *0.5f 可以将飞机的偏移速度降低

                #endregion

                buttons[(int)InputManager.Button.Fire] = (int)((Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha3)|| Input.GetKey(KeyCode.JoystickButton0)) ? 1 : 0);
                buttons[(int)InputManager.Button.Back] = (int)((Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.JoystickButton0)) ? 1 : 0);
                buttons[(int)InputManager.Button.Debug] = (int)(Input.GetButtonDown("Fire3") ? 1 : 0);
                //buttons[(int)InputManager.Button.Camera] = (int)(Input.GetButtonDown("Camera") ? 1 : 0);
                //buttons[(int)InputManager.Button.Pause] = (int)(Input.GetButtonDown("Pause") ? 1 : 0);
                buttons[(int)InputManager.Button.FFWPlus] = (int)(Input.GetButtonDown("FFWPlus") ? 1 : 0);
                buttons[(int)InputManager.Button.FFWMinus] = (int)(Input.GetButtonDown("FFWMinus") ? 1 : 0);
                InputManager.Instance.flip();
            }

		
	}
        //只有游戏开始之后才能去播放倒计时
        public bool IsCanPlayDaojishi = false;
        //private void Update()
        //{
        //    
        //}
        // オブジェクト描画(SetActive)
        private void render(ref DrawBuffer draw_buffer)
	{
		// player
		if (player_go_ != null) {
			player_go_.transform.position = draw_buffer.player_transform_.position_;
			player_go_.transform.rotation = draw_buffer.player_transform_.rotation_;
			player_arm_transform_.localPosition = draw_buffer.player_arm_offset_;
		}
		int zako_idx = 0;
		int dragon_head_idx = 0;
		int dragon_body_idx = 0;
		int dragon_tail_idx = 0;
		int shutter_idx = 0;
		for (var i = 0; i < draw_buffer.object_num_; ++i) {
			switch (draw_buffer.object_buffer_[i].type_) {
				case DrawBuffer.Type.None:
					Debug.Assert(false);
					break;
				case DrawBuffer.Type.Zako:
					zako_pool_[zako_idx].SetActive(true);
					zako_pool_[zako_idx].transform.position = draw_buffer.object_buffer_[i].transform_.position_;
					zako_pool_[zako_idx].transform.rotation = draw_buffer.object_buffer_[i].transform_.rotation_;
					++zako_idx;
					break;
				case DrawBuffer.Type.DragonHead:
					dragon_head_pool_[dragon_head_idx].SetActive(true);
					dragon_head_pool_[dragon_head_idx].transform.position = draw_buffer.object_buffer_[i].transform_.position_;
					dragon_head_pool_[dragon_head_idx].transform.rotation = draw_buffer.object_buffer_[i].transform_.rotation_;
					++dragon_head_idx;
					break;
				case DrawBuffer.Type.DragonBody:
					dragon_body_pool_[dragon_body_idx].SetActive(true);
					dragon_body_pool_[dragon_body_idx].transform.position = draw_buffer.object_buffer_[i].transform_.position_;
					dragon_body_pool_[dragon_body_idx].transform.rotation = draw_buffer.object_buffer_[i].transform_.rotation_;
					++dragon_body_idx;
					break;
				case DrawBuffer.Type.DragonTail:
					dragon_tail_pool_[dragon_tail_idx].SetActive(true);
					dragon_tail_pool_[dragon_tail_idx].transform.position = draw_buffer.object_buffer_[i].transform_.position_;
					dragon_tail_pool_[dragon_tail_idx].transform.rotation = draw_buffer.object_buffer_[i].transform_.rotation_;
					++dragon_tail_idx;
					break;
				case DrawBuffer.Type.Shutter:
					shutter_pool_[shutter_idx].SetActive(true);
					shutter_pool_[shutter_idx].transform.position = draw_buffer.object_buffer_[i].transform_.position_*1.2f;
					shutter_pool_[shutter_idx].transform.rotation = draw_buffer.object_buffer_[i].transform_.rotation_;
					++shutter_idx;
					break;
			}
		}
		for (var i = zako_idx; i < ZAKO_MAX; ++i) {
			zako_pool_[i].SetActive(false);
		}
		for (var i = dragon_head_idx; i < DRAGON_HEAD_MAX; ++i) {
			dragon_head_pool_[i].SetActive(false);
		}
		for (var i = dragon_body_idx; i < DRAGON_BODY_MAX; ++i) {
			dragon_body_pool_[i].SetActive(false);
		}
		for (var i = dragon_tail_idx; i < DRAGON_TAIL_MAX; ++i) {
			dragon_tail_pool_[i].SetActive(false);
		}
		for (var i = shutter_idx; i < SHUTTER_MAX; ++i) {
              
			shutter_pool_[i].SetActive(false);
		}

		for (var i = 0; i < AUDIO_CHANNEL_MAX; ++i) {
			if (draw_buffer.se_[i] != DrawBuffer.SE.None) {
				switch (draw_buffer.se_[i]) {
					case DrawBuffer.SE.Bullet:
						audio_sources_bullet_[audio_source_bullet_index_].Play();
						++audio_source_bullet_index_;
						if (audio_source_bullet_index_ >= AUDIOSOURCE_BULLET_MAX) {
							audio_source_bullet_index_ = 0;
						}
						break;
					case DrawBuffer.SE.Explosion:
						audio_sources_explosion_[audio_source_explosion_index_].Play();
						++audio_source_explosion_index_;
						if (audio_source_explosion_index_ >= AUDIOSOURCE_EXPLOSION_MAX) {
							audio_source_explosion_index_ = 0;
						}
						break;
					case DrawBuffer.SE.Missile:
						audio_sources_missile_[audio_source_missile_index_].Play();
						++audio_source_missile_index_;
						if (audio_source_missile_index_ >= AUDIOSOURCE_MISSILE_MAX) {
							audio_source_missile_index_ = 0;
						}
						break;
					case DrawBuffer.SE.Lockon:
						audio_sources_lockon_[audio_source_lockon_index_].Play();
						++audio_source_lockon_index_;
						if (audio_source_lockon_index_ >= AUDIOSOURCE_LOCKON_MAX) {
							audio_source_lockon_index_ = 0;
						}
						break;
					case DrawBuffer.SE.Shield:
						audio_sources_shield_[audio_source_shield_index_].Play();
						++audio_source_shield_index_;
						if (audio_source_shield_index_ >= AUDIOSOURCE_SHIELD_MAX) {
							audio_source_shield_index_ = 0;
						}
						break;

					case DrawBuffer.SE.VoiceIkuyo:
						audio_sources_voice_ikuyo_.Play();
						break;
					case DrawBuffer.SE.VoiceUwaa:
						audio_sources_voice_uwaa_.Play();
						break;
					case DrawBuffer.SE.VoiceSorosoro:
						audio_sources_voice_sorosoro_.Play();
						break;
					case DrawBuffer.SE.VoiceOtoto:
						audio_sources_voice_ototo_.Play();
						break;
					case DrawBuffer.SE.VoiceYoshi:
						audio_sources_voice_yoshi_.Play();
						break;
				}
				draw_buffer.se_[i] = DrawBuffer.SE.None;
			}
		}

		switch (draw_buffer.bgm_) {
			case DrawBuffer.BGM.Keep:
				break;
			case DrawBuffer.BGM.Stop:
				audio_sources_bgm_.Stop();
				is_bgm_playing_ = false;
				break;
			case DrawBuffer.BGM.Pause:
				if (is_bgm_playing_)
					audio_sources_bgm_.Pause();
				break;
			case DrawBuffer.BGM.Resume:
				if (is_bgm_playing_)
					audio_sources_bgm_.Play();
				break;
			case DrawBuffer.BGM.Battle:
				audio_sources_bgm_.Play();
				is_bgm_playing_ = true;
				break;
		}
		draw_buffer.bgm_ = DrawBuffer.BGM.Keep;

		switch (draw_buffer.motion_) {
			case DrawBuffer.Motion.Keep:
				break;
			case DrawBuffer.Motion.Play:
				TubeScroller.Instance.setPause(false);
				break;
			case DrawBuffer.Motion.Pause:
				TubeScroller.Instance.setPause(true);
				break;
			case DrawBuffer.Motion.Resume:
				TubeScroller.Instance.setPause(false);
				break;
			case DrawBuffer.Motion.GoodLuck:
				TubeScroller.Instance.setOperatorMotionGoodLuck();
				break;
		}
		draw_buffer.motion_ = DrawBuffer.Motion.Keep;
	}

	public void resetView()
	{
		view_offset_z_ = -camera_.transform.localPosition.z;
	}

	public void setSubjective(bool flg)
	{

		subjective_ = flg;
	}

	private void camera_update()
	{
		// camera view for VR
		PerformanceFetcher.PushMarker("camera_view");
		{
			bool reset_view = false;
			if (InputManager.Instance.getButton(InputManager.Button.Camera) > 0) {
				subjective_ = !subjective_;
				if (subjective_) {
					reset_view = true;
				}
			}
			if (reset_position_) {
				reset_view = true;
				reset_position_ = false;
			}
			if (reset_view) {
				resetView();
			}
			if (subjective_) {
				camera_holder_transform_.position = (player_go_.transform.position +
													 new Vector3(0, 0.5f, 0f + view_offset_z_));
			} else {
				camera_holder_transform_.position = new Vector3(0, 0, 0);
			}
		}
		PerformanceFetcher.PopMarker();
	}

	private void unity_update()
	{
		beginPerformanceMeter2();

		// input phase
		PerformanceFetcher.PushMarker("input_update");
		input_update();
		PerformanceFetcher.PopMarker();

		//
		PerformanceFetcher.PushMarker("several render");
		double render_time = update_time_ - dt_;
		render(ref draw_buffer_[rendering_front_]);
		Debris.Instance.render(rendering_front_, camera_, render_time, flow_speed_, dt_);
		Spark.Instance.render(rendering_front_, camera_, render_time, flow_speed_);
		Beam.Instance.render(rendering_front_);
		Beam2.Instance.render(rendering_front_);
		Trail.Instance.render(rendering_front_);
		if (explosion_material_ != null) {
			Explosion.Instance.render(rendering_front_, camera_, render_time, flow_speed_ * 0.25f);
		}
		Hahen.Instance.render(rendering_front_, render_time);
		Shockwave.Instance.render(rendering_front_, camera_, render_time);
		Shield.Instance.render(rendering_front_, render_time);
		Sight.Instance.render(rendering_front_, camera_, render_time);
		LockonRange.Instance.render(dt_);
		LockonRangeRenderer.Instance.render(render_time, dt_);
		TubeScroller.Instance.render();
		PerformanceFetcher.PopMarker();

		PerformanceFetcher.PushMarker("hud render");
		VRSprite.Instance.render(rendering_front_, camera_);
		MySprite.Instance.render(rendering_front_);
		MyFont.Instance.render(rendering_front_);
		PerformanceFetcher.PopMarker();

		// memory investigation
		used_heap_size_ = (long)Profiler.usedHeapSize;
		mono_heap_size_ = (long)Profiler.GetMonoHeapSize();
		mono_used_size_ = (long)Profiler.GetMonoUsedSize();

		// frame
		elapsed_frame_time_ = Time.time - prev_frame_time_;
		elapsed_frame_time_display_ = Mathf.Lerp(elapsed_frame_time_display_, elapsed_frame_time_, 0.01f);
		prev_frame_time_ = Time.time;
	}

	private void end_of_frame()
	{
		if (Time.deltaTime > 0) {
			++render_sync_frame_;
			++render_frame_;
			rendering_front_ = 1 - rendering_front_; // flip
			manual_reset_event_.Set();
			stopwatch_.Start();
		} else {
			stopwatch_.Stop();
		}
	}

	public void beginPerformanceMeter()
	{
		render_start_tick_ = stopwatch_.ElapsedTicks;
	}
	public void endPerformanceMeter()
	{
		render_tick_ = stopwatch_.ElapsedTicks - render_start_tick_;
	}
	public void beginPerformanceMeter2()
	{
		render_start_tick2_ = stopwatch_.ElapsedTicks;
	}
	public void endPerformanceMeter2()
	{
		render_tick2_ = stopwatch_.ElapsedTicks - render_start_tick2_;
	}

        //枚举游戏帮助界面的提示
        public enum GameTip
        {
            GameBeganTip,
            WarningTip,
            AttackTip,
            DoorTip,
        }

        public GameTip mGameTip = GameTip.GameBeganTip;

        //游戏提示界面
        private Text _mgameTip;

        //通道位置标志物
        private GameObject tube_B01;

        //血量条的显示控制
        private Image _health;
        #region 定义玩家总血量的值
        [HideInInspector]
        public float  BloodVolume = 200f;//总血量
        [HideInInspector]
        public int NowBloodVolume=1;//当前血量
        [HideInInspector]
        public int ScoreNumber = 0;//当前分数
        public bool IsGameOver = false;

        public bool IsGameOverRank = false;
        [HideInInspector]
        public Text fenshu;
        public Image Image;
        [HideInInspector]
        public Image Tip;
        [HideInInspector]
        public Image TipTwo;
        [HideInInspector]
        public Image TipThree;

        [HideInInspector]
        public float time = 0;
        [HideInInspector]
        public bool IsTipShowTip = false;
        [HideInInspector]
        public float timeTipBegan = 2f;
        [HideInInspector]
        public bool IsBeganShowTipTwo = false;

        [HideInInspector]
        public Text Score;//玩家得分


        public GameObject CameraBackage;
        public GameObject debrisRenderer;

        public float BecomeBlackTime = 0;
       
        #endregion
        // The Update
        void Update()
	    {
            #region 判断当前是否是自动战斗，如果是，任意键按下则切回游戏模式
            //判断当前是否是自动战斗，如果是，任意键按下则切回游戏模式
            if (ReplayManager.IsAutoAttack==true && (Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.Alpha3)|| Input.GetKeyDown(KeyCode.Alpha4)|| Input.GetKeyDown(KeyCode.JoystickButton0)))
            {
                print("此时出于自动战斗模式，点击切换游戏模式");

                UnityEngine.Application.LoadLevel("main");
                print("重新加载场景");

                //ReplayManager.IsAutoAttack = false;
                SystemManager.Instance.restart();
                //SystemManager.Instance.BloodVolume = 200f;
                //EnemyBullet.AttackNumber = 0;
                //SystemManager.Instance.NowBloodVolume = 1;

                //SystemManager.instance_.IsBegan = true;
            }
            #endregion



            #region 通过判断通道位置来处理显示提示先后的逻辑
            //通过判断通道位置来处理显示提示先后的逻辑
            //每次變換状态之后都要主动激活_mgameTip，然后再_mgameTip身上，显示5秒之后自动消失
            if (tube_B01==null)
            {
                tube_B01 = GameObject.Find("tube_B01(Clone)");
            }
            if (tube_B01.GetComponent<Transform>().position.z <= -511f&& mGameTip == GameTip.GameBeganTip)
            {
                _mgameTip.gameObject.SetActive(true);
                mGameTip = GameTip.WarningTip;
            }
            if (tube_B01.GetComponent<Transform>().position.z <= -1047f && mGameTip == GameTip.WarningTip)
            {
                _mgameTip.gameObject.SetActive(true);
                mGameTip = GameTip.AttackTip;
            }
            if (tube_B01.GetComponent<Transform>().position.z <= -6571f && mGameTip == GameTip.AttackTip)
            {
                _mgameTip.gameObject.SetActive(true);
                mGameTip = GameTip.DoorTip;
            }
            if (tube_B01.GetComponent<Transform>().position.z <= -10000f && mGameTip == GameTip.DoorTip)
            {
                
                mGameTip = GameTip.GameBeganTip;
            }
            if (tube_B01.GetComponent<Transform>().position.z <= -10000f)
            {
                _mgameTip.gameObject.SetActive(false);
            }
            if (tube_B01.GetComponent<Transform>().position.z == -500f)
            {
                StartCoroutine(waitGameTipEnum());
                mGameTip = GameTip.GameBeganTip;
            }


            //在这里处理游戏开始时 各种游戏提醒的逻辑：1、按任意建开始游戏；2、注意，前方危险；3、尽快击毙前方敌机
            switch (mGameTip)
            {
                case GameTip.GameBeganTip:
                    //将提示的界面显示为: 1、按任意建开始游戏
                    
                    _mgameTip.text = "按任意建开始游戏";
                    break;
                case GameTip.WarningTip:
                    //将提示的界面显示为: 2、注意，前方危险
                    _mgameTip.text = "注意，前方危险";
                    break;
                case GameTip.AttackTip:
                    //将提示的界面显示为: 3、尽快击毙前方敌机
                    _mgameTip.text = "尽快击毙前方敌机";
                    break;
                case GameTip.DoorTip:
                    //将提示的界面显示为: 4、注意前方危险门
                    _mgameTip.text = "注意前方危险";
                    break;
                default:
                    break;
            }
            #endregion

            #region 此时游戏结束，刚死亡，将排行榜消失出来，排行榜消失之后再界面变黑，重新刷新位置
            if (GameManager.Instance.IsEnd|| IsGameOver)  //两种情况下的游戏结束
            {

            }

            #endregion
            
            
            #region     游戏结束屏幕渐变，渐变黑色
            if (GameManager.Instance.CameraBackageBecomeBlack)
            {
                //更改相機模式
                print("我要变黑了");
                BecomeBlackTime += Time.deltaTime;
                if (BecomeBlackTime>=4f)
                { 
                    CameraBackage.gameObject.SetActive(true);
                    //Temp 
                    //GameObject.Find("player 1(Clone)").transform.FindChild("ShieldRenderer").gameObject.SetActive(false);
                    GameObject.Find("player 1(Clone)").transform.FindChild("cockpit-07_Prefab").gameObject.SetActive(false);
                    GameObject.Find("CanvasFenshuManage").transform.FindChild("Fenshu").gameObject.SetActive(false);//隐藏血量

                    GameObject.Find("CanvasFenshuManage").transform.FindChild("Health").gameObject.SetActive(false);//显示血量条

                    GameObject.Find("CanvasFenshuManage").transform.FindChild("Score").gameObject.SetActive(false);//隐藏分数
                    debrisRenderer.SetActive(false);
                    
                    StartCoroutine(WaitBack());
                }
            }

#endregion


            #region 控制開始显示图片
            time += Time.deltaTime;
            if (time>= timeTipBegan+1f && !IsTipShowTip)
            {
                IsTipShowTip = true;
                //Tip.gameObject.SetActive(true);
                StartCoroutine(ShowTipFlash());

            }
            if (time >= 15)
            {
                if (Tip)
                {
                    Tip.gameObject.SetActive(false);
                }
                
            }
            if ((Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Alpha3)|| Input.GetKeyDown(KeyCode.JoystickButton0)) && !IsBeganShowTipTwo/*&& GameTimeResourcesControl._instance.IsResurceEnd*/)
            {
                Tip.gameObject.SetActive(false);
                IsBeganShowTipTwo = true;
                //StartCoroutine(ShowTipFlash());   //調用手柄提示
            }
            #endregion
               
            
            #region 计算玩家血量 Li
                if (BloodVolume >= 1)
            {
                
                NowBloodVolume = (int)(BloodVolume - EnemyBullet.AttackNumber*0.3f);
                //print(NowBloodVolume+ "++++++++++++++++++NowBloodVolume");
                if (NowBloodVolume <= 0)
                {
                    print("游戲結束");

                    //如果玩家被打死，则将各种游戏提示隐藏
                    _mgameTip.gameObject.SetActive(false);

                    IsGameOver = true;
                    if (NowBloodVolume<0)
                    {
                        NowBloodVolume = 0;
                    }
                    print(NowBloodVolume + "________________NowBloodVolume");
                }
            }
            //血量分数显示
            //fenshu.text = "剩余能量：" + NowBloodVolume.ToString();

            //处理血量条和血量值相关联问题逻辑
            _health.fillAmount = NowBloodVolume * 0.005f;


            //分数显示
            Score.text =  /*"当前得分：" +*/ Explosion.Instance.PlayerAttackEnemyNumber.ToString();

            #endregion
            
            
            #region 游戏结束逻辑
            if (IsGameOver)
            {
                IsGameOver = false;
                //游戏结束时若是被敌人打死，则重新加载场景,显示游戏结束的界面
                Image.gameObject.SetActive(true);
                print("激活游戲結束的面板"+IsGameOver);

                //BloodVolume = 350f;
                //NowBloodVolume = 1;
                GameManager.Instance.CameraBackageBecomeBlack = true;
                StartCoroutine(iNITGmae());
            }

            #endregion

        render_tick3_ = stopwatch_.ElapsedTicks - render_start_tick_;
		PerformanceFetcher.PushMarker("Update");
		unity_update();
		end_of_frame();
		PerformanceFetcher.PopMarker();
		render_tick4_ = stopwatch_.ElapsedTicks - render_start_tick_;
	    }

        //重置游戏后，注意游戏的状态提示回到最初，出现时间控制在合适范围
        IEnumerator waitGameTipEnum()
        {
            yield return new WaitForSeconds(3f);
            _mgameTip.gameObject.SetActive(true);
        }



        IEnumerator WaitBack()
        {
            yield return new WaitForSeconds(5f);
            CameraBackage.gameObject.SetActive(false);
            GameObject.Find("player 1(Clone)").transform.FindChild("cockpit-07_Prefab").gameObject.SetActive(true);
            GameObject.Find("CanvasFenshuManage").transform.FindChild("Fenshu").gameObject.SetActive(true);//显示血量

            GameObject.Find("CanvasFenshuManage").transform.FindChild("Health").gameObject.SetActive(true);//显示血量条

            GameObject.Find("CanvasFenshuManage").transform.FindChild("Score").gameObject.SetActive(true);//显示分数
            debrisRenderer.SetActive(true);
        }

            IEnumerator ShowTipFlash()
        {
            Tip.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.5f);
            TipThree.gameObject.SetActive(false);
            TipTwo.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            TipThree.gameObject.SetActive(true);
            TipTwo.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            TipThree.gameObject.SetActive(false);
            TipTwo.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            TipThree.gameObject.SetActive(true);
            TipTwo.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            TipThree.gameObject.SetActive(false);
            TipTwo.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            TipThree.gameObject.SetActive(true);
            TipTwo.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            TipThree.gameObject.SetActive(false);
            TipTwo.gameObject.SetActive(false);
            
        }


        IEnumerator iNITGmae()
        {
            yield return new WaitForSeconds(4f);
            RankManager.instance.Hide();
            yield return new WaitForSeconds(3f);
            Image.gameObject.SetActive(false);
            SystemManager.Instance.restart();
            BloodVolume = 200f;
            EnemyBullet.AttackNumber = 0;
            NowBloodVolume = 1;
        }
        void LateUpdate()
	    {
		    camera_update();
	    }

	void OnApplicationQuit()
	{
		update_thread_.Abort();
	}
}

} // namespace UTJ {
